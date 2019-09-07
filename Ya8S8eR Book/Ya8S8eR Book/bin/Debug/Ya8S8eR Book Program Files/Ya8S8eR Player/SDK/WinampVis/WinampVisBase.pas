unit WinampVisBase;

interface

uses Types, Windows;

type
 PwinampVisModule=^TwinampVisModule;
 TwinampVisModule=packed record
  description : pchar; // description of module
  hwndParent : HWND;   // parent window (filled in by calling app)
  hDllInstance : THandle; // instance handle to this DLL (filled in by calling app)
  sRate : integer;		 // sample rate (filled in by calling app)
  nCh : integer;			 // number of channels (filled in...)
  latencyMs : integer;     // latency from call of RenderFrame to actual drawing
                     // (calling app looks at this value when getting data)
  delayMs : integer;       // delay between calls in ms

  // the data is filled in according to the respective Nch entry
  spectrumNch : integer;
  waveformNch : integer;
  spectrumData : array[0..1,0..575] of Byte;
  waveformData : array[0..1,0..575] of Byte;

  Config : procedure(this_mod : PwinampVisModule); cdecl; // configuration dialog
  Init : function(this_mod : PwinampVisModule) : integer; cdecl;    // 0 on success, creates window, etc
  Render : function(this_mod : PwinampVisModule) : integer; cdecl;  // returns 0 if successful, 1 if vis should end
  Quit : procedure(this_mod : PwinampVisModule); cdecl;   // call when done

  userData : Pointer; // user data, optional
 end;

 PwinampVisHeader=^TwinampVisHeader;
 TwinampVisHeader=packed record
  version : integer;       // VID_HDRVER
  description : pchar; // description of library
  getModule : function(index : integer) : PwinampVisModule; cdecl;
 end;

// exported symbols
// 아래의 함수를 노출시켜야함...
//function winampVisGetHeader : PwinampVisHeader;

const
 VIS_HDRVER = $101;

implementation

end.
