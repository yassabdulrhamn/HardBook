library dsp_TestPas;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters. }

uses
  SysUtils,
  Classes,
  Graphics,
  Types,
  KMPPlgIn in '..\KMPPlgIn.pas';

{$R *.res}

var
 winampDSPHeader : TwinampDSPHeader;
 winampDSPModule : TwinampDSPModule;

 kmpDSPHeader : TkmpDSPHeader;
 kmpDSPModule : array[0..2] of TkmpDSPModule;


function getwinampDSPModule(num : integer) : PwinampDSPModule; cdecl;
begin
 if num=0 then Result:=@winampDSPModule
 else Result:=nil;
end;

function ModifyAudio(this_mod : PwinampDSPModule;samples : PByte;
  numsamples,bps,nch,srate : integer) : integer; cdecl;
var
 i,rr,aa : integer;
 ps : pSmallint;
 pb : pByte;
begin
 if bps=16 then begin
   ps:=pSmallint(samples);
   if nch=2 then begin
     for i:=0 to numsamples-1 do begin
       rr:=Random($8fff div 5);
       aa:=ps^+rr;
       if aa<-32768 then aa:=-32768
       else if aa>32767 then aa:=32767;
       ps^:=aa;
       inc(ps);
       rr:=Random($8fff div 5);
       aa:=ps^+rr;
       if aa<-32768 then aa:=-32768
       else if aa>32767 then aa:=32767;
       ps^:=aa;
       inc(ps);
     end;
   end
   else if nch=1 then begin
     for i:=0 to numsamples-1 do begin
       rr:=Random($8fff div 5);
       aa:=ps^+rr;
       if aa<-32768 then aa:=-32768
       else if aa>32767 then aa:=32767;
       ps^:=aa;
       inc(ps);
     end;
   end;
 end
 else if bps=8 then begin
   pb:=samples;
   if nch=2 then begin
     for i:=0 to numsamples-1 do begin
       rr:=Random($7f div 5);
       aa:=pb^+rr;
       if aa<0 then aa:=0
       else if aa>255 then aa:=255;
       pb^:=aa;
       inc(pb);
       rr:=Random($7f div 5);
       aa:=pb^+rr;
       if aa<0 then aa:=0
       else if aa>255 then aa:=255;
       pb^:=aa;
       inc(pb);
     end;
   end
   else if nch=1 then begin
     for i:=0 to numsamples-1 do begin
       rr:=Random($7f div 5);
       aa:=pb^+rr;
       if aa<0 then aa:=0
       else if aa>255 then aa:=255;
       pb^:=aa;
       inc(pb);
     end;
   end;
 end;

 Result:=numsamples;
end;

function getkmpDSPModule(num : integer) : PkmpDSPModule; cdecl;
begin
 if num=0 then Result:=@kmpDSPModule[0]
 else if num=1 then Result:=@kmpDSPModule[1]
 else if num=2 then Result:=@kmpDSPModule[2]
 else Result:=nil;
end;

function ModifyVideoHori(this_mod : PkmpDSPModule;image : PkmpYV12Image; ewidth : integer) : integer; cdecl;
var
 i,j : integer;
 ps,pd : pByte;
begin
 ps:=image^.y_plain;
 for i:=0 to image^.Height-1 do begin
   pd:=ps;
   for j:=0 to ewidth-1 do begin
     if (j and 3)=0 then pd^:=0;
     inc(pd);
   end;
   inc(ps,image^.Width);
 end;
 Result:=0;
end;

function ModifyVideoVert(this_mod : PkmpDSPModule;image : PkmpYV12Image; ewidth : integer) : integer; cdecl;
var
 i : integer;
 pp : pByte;
begin
 pp:=image^.y_plain;
 for i:=0 to image^.Height-1 do begin
   if (i and 3)=0 then FillChar(pp^,ewidth,0);
   inc(pp,image^.Width);
 end;
 Result:=0;
end;

function TimeDisplay(this_mod : PkmpDSPModule;image : PkmpYV12Image; ewidth : integer) : integer; cdecl;
var
 Bmp : TBitmap;
 Img : PkmpYV12Image;
 p : Pointer;
 RR : TRect;
begin
 Bmp:=TBitmap.Create;
 try
   Bmp.Width:=image^.Width;
   Bmp.Height:=20;
   Bmp.HandleType:=bmDIB;
   Bmp.PixelFormat:=pf32bit; // 32Bit의 DIB를 만들기 위해서 설정한다.
   RR:=Rect(0,0,Bmp.Width,Bmp.Height);
   Bmp.Canvas.Brush.Color:=clBlack;
   Bmp.Canvas.Font.Color:=clWhite;
   Bmp.Canvas.FillRect(RR);
   Bmp.Canvas.TextOut(2,2,TimeToStr(Now));
   p:=Bmp.ScanLine[Bmp.Height-1]; // DIB이기때문에 재일 끝이 처음이다...
   if this_mod.ImageProcessor<>nil then begin
     this_mod.ImageProcessor.kmpAllocYV12Image(Bmp.Width,Bmp.Height,Img);
     try // YV12이미지를 만들고
       this_mod.ImageProcessor.kmpRGB32ToYV12(p,Bmp.Width,Bmp.Height,Img);
       // DIB32를 YV12로 변환한다.
       this_mod.ImageProcessor.kmpYV12BlendTransDraw(Img,image,@RR,0,0,200);
       // 반투명/ 배경투명효과로 그린다.
     finally
       this_mod.ImageProcessor.kmpFreeYV12Image(Img);
     end;
   end;
 finally
   Bmp.Free;
 end;

 Result:=0;
end;


function winampDSPGetHeader2 : PwinampDSPHeader; cdecl;
begin
 // For Audio
 winampDSPHeader.version:=$20;
 winampDSPHeader.description:='Test for Audio Plugin K-MultimediaPlayer';
 winampDSPHeader.winampDSPModule:=getwinampDSPModule;

 winampDSPModule.description:='Add noise v0.001 for K-MultimediaPlayer';
 winampDSPModule.hwndParent:=0;
 winampDSPModule.hDllInstance:=0;
 winampDSPModule.Config:=nil;
 winampDSPModule.Init:=nil;
 winampDSPModule.ModifySamples:=ModifyAudio;
 winampDSPModule.Quit:=nil;
 winampDSPModule.userData:=nil;

 Randomize;
 Result:=@winampDSPHeader;
end;


function kmpDSPGetHeader2 : PkmpDSPHeader; cdecl;
begin
 // For Video
 kmpDSPHeader.version:=$20;
 kmpDSPHeader.description:='Test for Video Plugin K-MultimediaPlayer';
 kmpDSPHeader.kmpDSPModule:=getkmpDSPModule;

 kmpDSPModule[0].description:='Hori Scanline v0.001 for K-MultimediaPlayer';
 kmpDSPModule[0].hwndParent:=0;
 kmpDSPModule[0].hDllInstance:=0;
 kmpDSPModule[0].Config:=nil;
 kmpDSPModule[0].Init:=nil;
 kmpDSPModule[0].BeforeModifyImage:=nil;
 kmpDSPModule[0].AfterModifyImage:=ModifyVideoHori;
 kmpDSPModule[0].Quit:=nil;
 kmpDSPModule[0].userData:=nil;

 kmpDSPModule[1].description:='Hori Scanline v0.001 for K-MultimediaPlayer';
 kmpDSPModule[1].hwndParent:=0;
 kmpDSPModule[1].hDllInstance:=0;
 kmpDSPModule[1].Config:=nil;
 kmpDSPModule[1].Init:=nil;
 kmpDSPModule[1].BeforeModifyImage:=nil;
 kmpDSPModule[1].AfterModifyImage:=ModifyVideoVert;
 kmpDSPModule[1].Quit:=nil;
 kmpDSPModule[1].userData:=nil;

 kmpDSPModule[2].description:='Time Display v0.001 for K-MultimediaPlayer';
 kmpDSPModule[2].hwndParent:=0;
 kmpDSPModule[2].hDllInstance:=0;
 kmpDSPModule[2].Config:=nil;
 kmpDSPModule[2].Init:=nil;
 kmpDSPModule[2].BeforeModifyImage:=nil;
 kmpDSPModule[2].AfterModifyImage:=TimeDisplay;
 kmpDSPModule[2].Quit:=nil;
 kmpDSPModule[2].userData:=nil;

 Result:=@kmpDSPHeader;
end;


exports // 함수 노출
 winampDSPGetHeader2,
 kmpDSPGetHeader2;

begin

end.
