library dsp_winampvis;

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
  Forms,
  KMPPlgIn in '..\KMPPlgIn.pas',
  VisMain in 'VisMain.pas' {MainForm};

{$R *.res}

var
 winampDSPHeader : TwinampDSPHeader;
 winampDSPModule : TwinampDSPModule;


function getwinampDSPModule(num : integer) : PwinampDSPModule; cdecl;
begin
 if num=0 then Result:=@winampDSPModule
 else Result:=nil;
end;

function ModifyAudio(this_mod : PwinampDSPModule;samples : PByte;
  numsamples,bps,nch,srate : integer) : integer; cdecl;
begin
 Result:=numsamples;
 if this_mod^.userData<>nil then begin
   TMainForm(this_mod^.userData).AddSoundData(samples,numsamples,bps,nch,srate);
 end;  
end;

function Init(this_mod : PwinampDSPModule) : integer; cdecl;
begin
 if this_mod^.userData=nil then this_mod^.userData:=TMainForm.Create(Application);
 Application.ShowMainForm:=False;
 Result:=0;
end;

procedure Quit(this_mod : PwinampDSPModule); cdecl;
begin
 if this_mod^.userData<>nil then begin
   TMainForm(this_mod^.userData).Free;
   this_mod^.userData:=nil;
 end;
end;

function winampDSPGetHeader2 : PwinampDSPHeader; cdecl;
begin
 // For Audio
 winampDSPHeader.version:=$20;
 winampDSPHeader.description:='Winamp Visualization warapper v 0.1';
 winampDSPHeader.winampDSPModule:=getwinampDSPModule;

 winampDSPModule.description:='Winamp Visualization warapper v 0.1';
 winampDSPModule.hwndParent:=0;
 winampDSPModule.hDllInstance:=0;
 winampDSPModule.Config:=nil;
 winampDSPModule.Init:=Init;
 winampDSPModule.ModifySamples:=ModifyAudio;
 winampDSPModule.Quit:=Quit;
 winampDSPModule.userData:=nil;

 Result:=@winampDSPHeader;
end;

exports // 함수 노출
 winampDSPGetHeader2;

begin

end.
