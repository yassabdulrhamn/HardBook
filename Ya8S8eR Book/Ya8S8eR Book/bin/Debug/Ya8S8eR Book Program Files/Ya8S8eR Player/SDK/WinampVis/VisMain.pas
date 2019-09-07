unit VisMain;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, ComCtrls, StdCtrls, Mask, ToolEdit, Registry,
  WinampVisBase, ImgList, Spin, RXShell;

type
  TVisPlugInBuf = record
    DLLName : string;
    DLLHandle : THandle;
    Header : PwinampVisHeader;
    Count : integer;
    Modules : array of PwinampVisModule;
    Enables : array of Boolean;
  end;
  TVisPlugInList=array of TVisPlugInBuf;

  TMainForm = class(TForm)
    TreeView1: TTreeView;
    Panel1: TPanel;
    Label1: TLabel;
    DirectoryEdit1: TDirectoryEdit;
    CheckBox1: TCheckBox;
    Button1: TButton;
    ImageList: TImageList;
    Label2: TLabel;
    SpinEdit1: TSpinEdit;
    RxTrayIcon1: TRxTrayIcon;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure TreeView1MouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure TreeView1AdvancedCustomDrawItem(Sender: TCustomTreeView;
      Node: TTreeNode; State: TCustomDrawState; Stage: TCustomDrawStage;
      var PaintImages, DefaultDraw: Boolean);
    procedure TreeView1CustomDrawItem(Sender: TCustomTreeView;
      Node: TTreeNode; State: TCustomDrawState; var DefaultDraw: Boolean);
    procedure TreeView1DblClick(Sender: TObject);
    procedure DirectoryEdit1Change(Sender: TObject);
    procedure Button1Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure RxTrayIcon1Click(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure SpinEdit1Change(Sender: TObject);
    procedure CheckBox1Click(Sender: TObject);
  private
    { Private declarations }
    FReg : TRegistry;
    FRenderThread : TThread;
    procedure SearchPlugIn;
    function  IndexofVisPlugInList(DLLName: string): integer;
    function  AddVisPlugInList(DLLName: string): integer;
    function  IsPlugInFile(Name: string): Boolean;
    procedure TestVisParent(Node: TTreeNode; index: integer);
    procedure EnabledVisPlugIn(DLLName: string; Index: integer; Flag: Boolean);
    procedure ShowPlugInError;
    procedure SetupVisPlugIn(DLLName: string; Index: integer);
    procedure FreeVisPlugIn;
    procedure SaveVisPlugIn(sName: string);
    procedure LoadVisPlugIn(sName: string);
  public
    { Public declarations }
    procedure AddSoundData(samples : PByte;numsamples,bps,nch,srate : integer);
  end;

  TRenderThread = class(TThread)
  private
    { Private declarations }
    FOldClock : DWORD;
    FAudioBuffer : array of Byte;
    FSampleRate : integer;
    FSampleMul : integer;
    FSampleLen : integer;
    FSampleCh : integer;
    FSampleBps : integer;
    frequency : array[0..1,0..1024-1] of Byte;
    waveform : array[0..1,0..1024-1] of Byte;
    FReal,FImag : array[0..1024-1] of Double;
    procedure UpdateSoundData(pSample: pByte; len, bps, nch, srate: integer);
    procedure AnalySampleData(samples: pByte; numsamples, bps, nch: integer);
    procedure GetSpectrum(var wave, freq: array of Byte);
  protected
    procedure Execute; override;
  end;


var
  MainForm: TMainForm;
  FFrameRate : integer;
  FVisPlugInList : TVisPlugInList;
  FUsePlugIn : Boolean;
  FIsTrayIcon : Boolean;

const
  sRegistryKey = '\Software\ChangwonUniv\KMP\Visualization';
  sPluginPath = 'PluginPath';
  sVisPlugKey = 'VisPlugInKey';
  sBaseVisPreset = 'BaseVisPreset';
  sUsePlugIn = 'UsePlugIn';
  sFrameRate = 'FrameRate';
  sIsTrayIcon = 'IsTrayIcon';

  cDataLen = 1024;

implementation

{$R *.dfm}


{ TRenderThread }


procedure TRenderThread.AnalySampleData(samples: pByte; numsamples, bps, nch : integer);
var
 pw : pSmallInt;
 pb : pByte;
 i,num : integer;
begin
 if numsamples>cDataLen then num:=cDataLen
 else num:=numsamples;
 if bps=8 then begin
   pb:=pByte(samples);
   if nch=1 then begin
     for i:=0 to num-1 do begin
       waveform[0][i]:=pb^;
       inc(pb);
     end;
   end
   else if nch=2 then begin
     for i:=0 to num-1 do begin
       waveform[0][i]:=pb^;
       inc(pb);
       waveform[1][i]:=pb^;
       inc(pb);
     end;;
   end;
 end
 else if bps=16 then begin
   pw:=pSmallInt(samples);
   if nch=1 then begin
     for i:=0 to num-1 do begin
       waveform[0][i]:=((pw^) div 256)+128;
       inc(pw);
     end;
   end
   else if nch=2 then begin
     for i:=0 to num-1 do begin
       waveform[0][i]:=((pw^) div 256)+128;
       inc(pw);
       waveform[1][i]:=((pw^) div 256)+128;
       inc(pw);
     end;
   end;
 end;
end;

procedure TRenderThread.GetSpectrum(var wave,freq : array of Byte);
var
 i,j,m,mmax,istep,aa,bb : integer;
 c,s,treal,timag : double;
 theta : double;
begin
 for i:=0 to cDataLen-1 do begin
   FReal[i]:=wave[i];
   FImag[i]:=0.0;
 end;
 j:=1;
 for i:=1 to cDataLen do begin
   if(i<j) then begin
     aa:=j-1;
     bb:=i-1;
     treal:=FReal[aa];
     FReal[aa]:=FReal[bb];
     FReal[bb]:=treal;
   end;
   m:=cDataLen div 2;
   while(j>m) do begin
     j:=j-m;
     m:=(m+1) div 2;
   end;
   j:=j+m;
 end;

 mmax:=1;
 while(cDataLen>mmax) do begin
   istep:=2*mmax;
   for m:=1 to mmax do begin
     theta:=PI*(m-1)/mmax;
     c:=cos(theta);
     s:=sin(theta);
     i:=m;
     while i<=cDataLen do begin
       j:=i+mmax;
       aa:=j-1;
       bb:=i-1;
       treal:=FReal[aa]*c-FImag[aa]*s;
       timag:=FImag[aa]*c+FReal[aa]*s;
       FReal[aa]:=FReal[bb]-treal;
       FImag[aa]:=FImag[bb]-timag;
       FReal[bb]:=FReal[bb]+treal;
       FImag[bb]:=FImag[bb]+timag;
       i:=i+istep;
     end;
   end;
   mmax:=istep;
 end;
 for i:=0 to cDataLen-1 do begin
   freq[i]:=Trunc(sqrt(FReal[i]*FReal[i]+FImag[i]*FImag[i])) shr 3;
 end;
end;

procedure TRenderThread.Execute;
var
 pp,i,j : integer;
 np : DWORD;
begin
 FreeOnTerminate:=False;
 FOldClock:=0;
 while not(Terminated) do begin
   np:=GetTickCount;
   if FAudioBuffer<>nil then begin
     if FOldClock=0 then pp:=0
     else pp:=(np-FOldClock);
     pp:=pp*FSampleRate*FSampleMul div 1000;
     if pp>FSampleLen*2 then pp:=FSampleLen*2;
     if (pp and $03)<>0 then pp:=(pp shr 2) shl 2;
     AnalySampleData(@FAudioBuffer[pp],FSampleLen,FSampleBps,FSampleCh);
     GetSpectrum(waveform[0],frequency[0]);
     if FSampleCh=2 then GetSpectrum(waveform[1],frequency[1]);
     for i:=0 to Length(FVisPlugInList)-1 do begin
       for j:=0 to FVisPlugInList[i].Count-1 do begin
         if FVisPlugInList[i].Enables[j] then begin
           Move(waveform[0],FVisPlugInList[i].Modules[j]^.waveformData[0],576);
           if FSampleCh=2 then Move(waveform[1],FVisPlugInList[i].Modules[j]^.waveformData[1],576);
           FVisPlugInList[i].Modules[j]^.sRate:=FSampleRate;
           FVisPlugInList[i].Modules[j]^.nCh:=FSampleCh;
           FVisPlugInList[i].Modules[j]^.spectrumNch:=FSampleCh;
           FVisPlugInList[i].Modules[j]^.waveformNch:=FSampleCh;
           FVisPlugInList[i].Modules[j]^.Render(FVisPlugInList[i].Modules[j]);
         end;
       end;
     end;
   end;
   Sleep(FFrameRate);
 end;
end;

procedure TRenderThread.UpdateSoundData(pSample: pByte; len, bps, nch, srate: integer);
begin
 FSampleRate:=srate;
 FSampleMul:=(nch*bps div 8);
 FSampleLen:=len;
 FSampleCh:=nch;
 FSampleBps:=bps;
 if Length(FAudioBuffer)<len*4 then SetLength(FAudioBuffer,len*4);
 Move(FAudioBuffer[len],FAudioBuffer[len shl 1],len);
 Move(FAudioBuffer[0],FAudioBuffer[len],len);
 Move(pSample^,FAudioBuffer[0],len);
 FOldClock:=GetTickCount;
end;

{ TMainForm }

procedure TMainForm.AddSoundData(samples: PByte; numsamples, bps, nch, srate: integer);
begin
 if FRenderThread<>nil then TRenderThread(FRenderThread).UpdateSoundData(samples,numsamples,bps,nch,srate);
end;

procedure TMainForm.FormCreate(Sender: TObject);
begin
 FReg:=TRegistry.Create;
 FReg.RootKey:=HKEY_CURRENT_USER;
 if FReg.OpenKey(sRegistryKey,True) then begin
   if FReg.ValueExists(sPluginPath) then DirectoryEdit1.Text:=FReg.ReadString(sPluginPath);
   if DirectoryEdit1.Text='' then DirectoryEdit1.Text:=ExtractFilePath(Application.ExeName)+'PlugIns';
   if FReg.ValueExists(sUsePlugIn) then FUsePlugIn:=FReg.ReadBool(sUsePlugIn)
   else FUsePlugIn:=True;
   if FReg.ValueExists(sIsTrayIcon) then FIsTrayIcon:=FReg.ReadBool(sIsTrayIcon)
   else FIsTrayIcon:=True;
   if FReg.ValueExists(sFrameRate) then FFrameRate:=FReg.ReadInteger(sFrameRate)
   else FFrameRate:=15;
   CheckBox1.Checked:=FUsePlugIn;
   SpinEdit1.Value:=FFrameRate;
   if FIsTrayIcon then RxTrayIcon1.Active:=True
   else Show;
   FReg.CloseKey;
 end;
 FRenderThread:=TRenderThread.Create(False);
 LoadVisPlugIn(sBaseVisPreset);
 SearchPlugIn;
end;

procedure TMainForm.FormDestroy(Sender: TObject);
begin
 if FReg.OpenKey(sRegistryKey,True) then begin
   FReg.WriteString(sPluginPath,DirectoryEdit1.Text);
   FReg.WriteBool(sUsePlugIn,FUsePlugIn);
   FReg.WriteBool(sIsTrayIcon,RxTrayIcon1.Active);
   FReg.WriteInteger(sFrameRate,FFrameRate);
   FReg.CloseKey;
 end;
 FRenderThread.Terminate;
 FRenderThread.WaitFor;
 FRenderThread.Free;
 SaveVisPlugIn(sBaseVisPreset);
 FreeVisPlugIn;
 FReg.Free;
end;

function TMainForm.IndexofVisPlugInList(DLLName: string): integer;
var
 i : integer;
begin
 Result:=-1;
 for i:=0 to Length(FVisPlugInList)-1 do begin
   if UpperCase(FVisPlugInList[i].DLLName)=UpperCase(DLLName) then begin
     result:=i;
     break;
   end;
 end;
end;

function TMainForm.AddVisPlugInList(DLLName: string): integer;
var
 i,jj : integer;
 Func : function : PwinampVisHeader; cdecl;
 DSPMod : PwinampVisModule;
begin
 Result:=IndexofVisPlugInList(DLLName);
 if Result>=0 then exit;
 SetLength(FVisPlugInList,Length(FVisPlugInList)+1);
 jj:=Length(FVisPlugInList)-1;
 FVisPlugInList[jj].DLLName:=DLLName;
 FVisPlugInList[jj].DLLHandle:=SafeLoadLibrary(pchar(DLLName));
 Func:=GetProcAddress(FVisPlugInList[Length(FVisPlugInList)-1].DLLHandle,'winampVisGetHeader');
 if Assigned(Func) then begin
   FVisPlugInList[jj].Header:=Func;
   FVisPlugInList[jj].Count:=0;
   FVisPlugInList[jj].Modules:=nil;
   FVisPlugInList[jj].Enables:=nil;
   i:=0;
   while True do begin
     DSPMod:=Func^.getModule(i);
     if DSPMod=nil then break;
     DSPMod^.hwndParent:=Handle;
     DSPMod^.hDllInstance:=FVisPlugInList[jj].DLLHandle;
     SetLength(FVisPlugInList[jj].Modules,Length(FVisPlugInList[jj].Modules)+1);
     SetLength(FVisPlugInList[jj].Enables,Length(FVisPlugInList[jj].Enables)+1);
     FVisPlugInList[jj].Modules[Length(FVisPlugInList[jj].Modules)-1]:=DSPMod;
     FVisPlugInList[jj].Enables[Length(FVisPlugInList[jj].Enables)-1]:=False;
     FVisPlugInList[jj].Count:=Length(FVisPlugInList[jj].Enables);
     inc(i);
   end;
 end;
 Result:=jj;
end;

function TMainForm.IsPlugInFile(Name: string): Boolean;
var
 DLLHandle : THandle;
 p : Pointer;
begin
 DLLHandle:=LoadLibrary(pchar(Name));
 p:=GetProcAddress(DLLHandle,'winampVisGetHeader');
 Result:=(p<>nil);
 FreeLibrary(DLLHandle);
end;

procedure TMainForm.TestVisParent(Node : TTreeNode;index : integer);
var
 i,cnt : integer;
begin
 cnt:=0;
 for i:=0 to Node.Count-1 do begin
   if FVisPlugInList[index].Enables[i] then begin
     Node[i].ImageIndex:=1;
     Node[i].SelectedIndex:=1;
     inc(cnt);
   end
   else begin
     Node[i].ImageIndex:=3;
     Node[i].SelectedIndex:=3;
   end;
 end;
 if cnt>=FVisPlugInList[index].Count then begin
   Node.ImageIndex:=0;
   Node.SelectedIndex:=0;
 end
 else if cnt=0 then begin
   Node.ImageIndex:=3;
   Node.SelectedIndex:=3;
 end
 else begin
   Node.ImageIndex:=2;
   Node.SelectedIndex:=2;
 end;
end;

procedure TMainForm.SearchPlugIn;
var
 done : integer;
 Rec : TSearchRec;
 s : string;
 i,n : integer;
 Node,Nd : TTreeNode;

 function IndexOfNode(Str : string) : TTreeNode;
 var
  j : integer;
 begin
   Result:=nil;
   for j:=0 to TreeView1.Items.Count-1 do begin
     if (TreeView1.Items[j].Parent=nil) and (TreeView1.Items[j].Text=Str) then begin
       Result:=TreeView1.Items[j];
       exit;
     end;
   end;
 end;

begin
 TreeView1.Items.BeginUpdate;
 try
   for i:=TreeView1.Items.Count-1 downto 0 do begin
     if TreeView1.Items[i].Count<=0 then TreeView1.Items[i].Delete;
   end;
   s:=DirectoryEdit1.LongName;
   if s<>'' then begin
     if s[Length(s)]<>'\' then s:=s+'\';
     done:=FindFirst(s+'vis_*.dll',faAnyFile,Rec);
     while done=0 do begin
       if IsPlugInFile(s+Rec.Name) then begin
         if IndexOfNode(s+Rec.Name)=nil then begin
           Node:=TreeView1.Items.AddChild(nil,s+Rec.Name);
           Node.ImageIndex:=3;
           Node.SelectedIndex:=3;
           Node.Data:=Pointer(-1);
         end;
       end;
       done:=FindNext(Rec);
     end;
   end;
   FindClose(Rec);

   for i:=0 to Length(FVisPlugInList)-1 do begin
     Node:=IndexOfNode(FVisPlugInList[i].DLLName);
     if Node=nil then Node:=TreeView1.Items.AddChild(nil,FVisPlugInList[i].DLLName);
     Node.Data:=Pointer(i);
     if Node.Count<=0 then begin
       for n:=0 to FVisPlugInList[i].Count-1 do begin
         Nd:=TreeView1.Items.AddChild(Node,StrPas(FVisPlugInList[i].Modules[n].description));
         Nd.Data:=Pointer(n);
         Nd.ImageIndex:=1;
       end;
     end;
     TestVisParent(Node,i);
   end;
 finally
   TreeView1.Items.EndUpdate;
 end;
end;

procedure TMainForm.ShowPlugInError;
begin
 ShowMessage('플러그인에서 오류가 발생했습니다.');
end;

procedure TMainForm.EnabledVisPlugIn(DLLName: string;Index : integer;Flag: Boolean);
var
 ii : integer;
begin
 try
   ii:=AddVisPlugInList(DLLName);
   if (Index<=Length(FVisPlugInList[ii].Enables)-1) and
    (FVisPlugInList[ii].Enables[Index]<>Flag) then begin
     FVisPlugInList[ii].Enables[Index]:=Flag;
     if Flag then begin
       try
         if Assigned(FVisPlugInList[ii].Modules[Index]^.Init) then
           FVisPlugInList[ii].Modules[Index]^.Init(FVisPlugInList[ii].Modules[Index]);
       except
         ShowPlugInError;
       end;
     end
     else begin
       try
         if Assigned(FVisPlugInList[ii].Modules[Index]^.Quit) then
           FVisPlugInList[ii].Modules[Index]^.Quit(FVisPlugInList[ii].Modules[Index]);
       except
         ShowPlugInError;
       end;
     end;
   end;
 except
   ShowPlugInError;
 end;
end;

procedure TMainForm.TreeView1MouseDown(Sender: TObject;
  Button: TMouseButton; Shift: TShiftState; X, Y: Integer);
var
 Node : TTreeNode;
 ht : THitTests;
 i,ii : integer;
 Nd : TTreeNode;
begin
 ht:=TTreeView(Sender).GetHitTestInfoAt(X,Y);
 Node:=TTreeView(Sender).GetNodeAt(X,Y);
 if (Node<>nil) then begin
   if (Node.Parent=nil) and (Node.Count<=0) then begin
     ii:=AddVisPlugInList(Node.Text);
     Node.Data:=Pointer(ii);
     if ii>=0 then begin
       for i:=0 to FVisPlugInList[ii].Count-1 do begin
         Nd:=TTreeView(Sender).Items.AddChild(Node,StrPas(FVisPlugInList[ii].Modules[i].description));
         Nd.ImageIndex:=3;
         Nd.SelectedIndex:=3;
         Nd.Data:=Pointer(i);
       end;
     end;
   end;
   if (htOnIcon in ht) then begin
     if Node.Parent=nil then begin
       if Node.ImageIndex<>3 then begin
         Node.ImageIndex:=3;
         Node.SelectedIndex:=3;
         ii:=IndexofVisPlugInList(Node.Text);
         for i:=0 to FVisPlugInList[ii].Count-1 do begin
           EnabledVisPlugIn(Node.Text,i,False);
           if Node.Count>i then begin
             Node.Item[i].ImageIndex:=3;
             Node.Item[i].SelectedIndex:=3;
           end;
         end;
       end;
     end
     else begin
       if Node.ImageIndex=3 then begin
         Node.ImageIndex:=1;
         Node.SelectedIndex:=1;
         EnabledVisPlugIn(Node.Parent.Text,Integer(Node.Data),True);
       end
       else begin
         Node.ImageIndex:=3;
         Node.SelectedIndex:=3;
         EnabledVisPlugIn(Node.Parent.Text,Integer(Node.Data),False);
       end;
       ii:=AddVisPlugInList(Node.Parent.Text);
       TestVisParent(Node.Parent,ii);
     end;
   end;
 end;
end;

procedure TMainForm.TreeView1AdvancedCustomDrawItem(
  Sender: TCustomTreeView; Node: TTreeNode; State: TCustomDrawState;
  Stage: TCustomDrawStage; var PaintImages, DefaultDraw: Boolean);
var
 r : TRect;
 s : string;
 ii : integer;
begin
 if (Node.Parent=nil) and (Node.Count>0) and (Stage=cdPostPaint) then begin
   ii:=integer(Node.Data);
   if ii>=0 then begin
     s:=StrPas(FVisPlugInList[ii].Header.description);
     s:=' '+s+'['+ExtractFileName(Node.Text)+']';
     r:=Node.DisplayRect(True);
     inc(r.Right,300);
     Sender.Canvas.FillRect(r);
     if not(cdsSelected in State) then Sender.Canvas.Font.Color:=clRed;
     DrawText(Sender.Canvas.Handle,pchar(s),Length(s),r,DT_VCENTER or DT_SINGLELINE);
     DefaultDraw:=False;
   end;
 end;
end;

procedure TMainForm.TreeView1CustomDrawItem(Sender: TCustomTreeView;
  Node: TTreeNode; State: TCustomDrawState; var DefaultDraw: Boolean);
begin
 if not(cdsSelected in State) then begin
   if Node.Parent=nil then Sender.Canvas.Font.Color:=clBlue
   else Sender.Canvas.Font.Color:=clBlack;
 end;
end;

procedure TMainForm.SetupVisPlugIn(DLLName: string;Index : integer);
var
 ii : integer;
begin
 try
   ii:=AddVisPlugInList(DLLName);
   if (Index<=FVisPlugInList[ii].Count-1) then begin
     if Assigned(FVisPlugInList[ii].Modules[Index]^.Config) then
       FVisPlugInList[ii].Modules[Index]^.Config(FVisPlugInList[ii].Modules[Index]);
   end;
 except
   ShowPlugInError;
 end;
end;

procedure TMainForm.TreeView1DblClick(Sender: TObject);
var
 name : string;
 ii : integer;
begin
 if (TTreeView(Sender).Selected<>nil) and (TTreeView(Sender).Selected.Parent<>nil) then begin
   ii:=integer(TTreeView(Sender).Selected.Data);
   name:=TTreeView(Sender).Selected.Parent.Text;
   SetupVisPlugIn(name,ii);
 end;
end;

procedure TMainForm.FreeVisPlugIn;
var
 i,j : integer;
 PlugIn : TVisPlugInList;
begin
 PlugIn:=FVisPlugInList;
 FVisPlugInList:=nil;
 for i:=0 to Length(PlugIn)-1 do begin
   for j:=0 to Length(PlugIn[i].Enables)-1 do begin
     if (PlugIn[i].Enables[j]) then begin
       try
         if Assigned(PlugIn[i].Modules[j]^.Quit) then
           PlugIn[i].Modules[j]^.Quit(PlugIn[i].Modules[j]);
       except

       end;
     end;
   end;
   FreeLibrary(PlugIn[i].DLLHandle);
   PlugIn[i].Modules:=nil;
   PlugIn[i].Enables:=nil;
 end;
end;

procedure TMainForm.SaveVisPlugIn(sName: string);
var
 i,j : integer;
 bb : Boolean;
begin
 FReg.DeleteKey(sRegistryKey+sVisPlugKey+'\'+sName);
 if FReg.OpenKey(sRegistryKey+sVisPlugKey+'\'+sName,True) then begin
   for i:=0 to Length(FVisPlugInList)-1 do begin
     FReg.CloseKey;
     bb:=False;
     if FReg.OpenKey(sRegistryKey+sVisPlugKey+'\'+sName+'\'+IntToStr(i),True) then begin
       FReg.WriteString('',FVisPlugInList[i].DLLName);
       for j:=0 to Length(FVisPlugInList[i].Enables)-1 do begin
         FReg.WriteBool(IntToStr(j),FVisPlugInList[i].Enables[j]);
         if FVisPlugInList[i].Enables[j] then bb:=True;
       end;
       FReg.CloseKey;
     end;
     if bb=False then FReg.DeleteKey(sRegistryKey+sVisPlugKey+'\'+sName+'\'+IntToStr(i));
   end;
   FReg.CloseKey;
 end;
end;

procedure TMainForm.LoadVisPlugIn(sName: string);
var
 i,j : integer;
 List : TStringList;
 ss : string;
begin
 FreeVisPlugIn;
 List:=TStringList.Create;
 try
   FReg.RootKey:=HKEY_CURRENT_USER;
   if FReg.KeyExists(sRegistryKey+sVisPlugKey+'\'+sName) then begin
     if FReg.OpenKey(sRegistryKey+sVisPlugKey+'\'+sName,False) then begin
       FReg.GetKeyNames(List);
       for i:=0 to List.Count-1 do begin
         FReg.CloseKey;
         if FReg.OpenKey(sRegistryKey+sVisPlugKey+'\'+sName+'\'+List[i],False) then begin
           ss:=FReg.ReadString('');
           AddVisPlugInList(ss);
           j:=0;
           while True do begin
             if FReg.ValueExists(IntToStr(j)) then EnabledVisPlugIn(ss,j,FReg.ReadBool(IntToStr(j)))
             else break;
             inc(j);
           end;
           FReg.CloseKey;
         end;
       end;
       FReg.CloseKey;
     end;
   end;
 finally
   List.Free;
 end;
end;

procedure TMainForm.DirectoryEdit1Change(Sender: TObject);
begin
 SearchPlugIn;
end;

procedure TMainForm.Button1Click(Sender: TObject);
begin
 FreeVisPlugIn;
 SearchPlugIn;
end;

procedure TMainForm.FormClose(Sender: TObject; var Action: TCloseAction);
begin
 RxTrayIcon1.Active:=True;
end;

procedure TMainForm.RxTrayIcon1Click(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
 RxTrayIcon1.Active:=False;
 Show;
end;

procedure TMainForm.SpinEdit1Change(Sender: TObject);
begin
 FFrameRate:=SpinEdit1.Value;
end;

procedure TMainForm.CheckBox1Click(Sender: TObject);
begin
 FUsePlugIn:=TCheckBox(Sender).Checked;
end;

end.
