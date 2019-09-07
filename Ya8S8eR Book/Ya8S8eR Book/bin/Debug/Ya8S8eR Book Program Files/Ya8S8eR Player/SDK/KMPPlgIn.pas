/////////////////////////////////////////////////////////////////////////////
//
//          KMP용 오디오/비디오 플러그인 인터페이스
//
//  KMP의 플러그인 모듈은 윈앰프의 DSP플러그인과
// 구조가 거의 같다. 오디오 플러그인의 경우는 읜앰프와 구조가
// 같기 때문에 윈앰프의 DSP플러그인을 KMP에서 사용하는 것이 가능하다.
// 비디오 플러그인의 경우도 거의 비슷하므로 쉽게 작성할수가 있을것이다.
// 그리고 함수 규약이 cdel이다.. 참고...
//
// 영상플러그인의 경우는 WaSaVi와 달라졌다... 또한 참고...
//
/////////////////////////////////////////////////////////////////////////////

unit KMPPlgIn;

interface

uses
  Windows;

// header version: 0x20 == 0.20 == winamp 2.0

const
DSP_HDRVER = $20;


// DSP plugin interface

// notes:
// any window that remains in foreground should optimally pass unused
// keystrokes to the parent (winamp's) window, so that the user
// can still control it. As for storing configuration,
// Configuration data should be stored in <dll directory>\plugin.ini
// (look at the vis plugin for configuration code)

type
//////////////////////////////////////////////////////////////////////////////
//
// Audio PlugIn Type...(same as Winamp DSP PlugIn Type...)
//
// 윈앰프의 DSP프러그인과 구조가 100%일치한다.
//////////////////////////////////////////////////////////////////////////////

 PwinampDSPModule = ^TwinampDSPModule;
 TwinampDSPModule = record
   description : pchar;		// description
   hwndParent : HWND;      	// parent window (filled in by calling app)
   hDllInstance : LongWord;	// instance handle to this DLL (filled in by calling app)

   Config : procedure(this_mod : PwinampDSPModule); cdecl;  // configuration dialog (if needed)
   Init : function(this_mod : PwinampDSPModule) : integer; cdecl;      // 0 on success, creates window, etc (if needed)

  // modify waveform samples: returns number of samples to actually write
  // (typically numsamples, but no more than twice numsamples, and no less than half numsamples)
  // numsamples should always be at least 128. should, but I'm not sure
   ModifySamples : function(this_mod : PwinampDSPModule;samples : PByte;
     numsamples,bps,nch,srate : integer) : integer; cdecl;
   // 윈앰프에서 사용하는 음성을 변경하는 함수...
   // samples : 샘플이 들어가 있는 버퍼
   // numsamples : 샘플의 갯수(실질적인 버퍼의 길이가 아니라 샘플의 갯수임...
   // bps : 샘플당 비트수 8 or 16
   // nch : 채널수 1 : 모노  , 2 : 스트레오
   // srate : 샘플링 레이트 44000등등..
   // 이함수에서 리턴값이 변경한 데이타의 갯수이다. 또한 실질적으로 출력으로
   // 나가는 갯수이기도 하다. 윈앰프에서 테스트한 경우는 입력받은 샘플보다
   // 많게해도 동작을 하였다. 따라서 KMP에서도 입력보다 3-4배정도의
   // 메모리를 내부적으로 할당을 해놓았다. 그러므로 3-4배 이상을 처리하면 오류가
   // 일어날수도 있다...
   //

   Quit : procedure(this_mod : PwinampDSPModule); cdecl;    // called when unloading

   userData : Pointer; // user data, optional
 end;


 PwinampDSPHeader = ^TwinampDSPHeader;
 TwinampDSPHeader = record
   version : integer;       // DSP_HDRVER
   description : pchar; // description of library
   winampDSPModule : function(num : integer) : PwinampDSPModule; cdecl;	// module retrieval function
 end;

////////////////////////////////////
// Audio PlugIn DLL Exports Name...
////////////////////////////////////
//function winampDSPGetHeader2 : PwinampDSPHeader; cdecl;
// 음성 DLL에서 Exports해야 되는 함수의 형식
// 즉 위의 함수를 노출시켜야 한다....





////////////////////////////////////////////////////////////////////////////
//
// Video PlugIn Type( Modifyed from Winamp DSP Plugin type ...)
//
// 윈앰프의 DSP플러그인의 구조를 조금 변경해서 KMP의 비디오
// 플러그인을 사용한다.
////////////////////////////////////////////////////////////////////////////

const
 IID_IYV12ImageProcessor : TGUID = '{249F44BC-B8BF-411F-B047-D9C017DFECF2}';


type
 // KMP에서 제공되는 화상의 구조
 // KMP에서는 플레인 YV12이미지의 구조를 사용한다.
 // Y의 크기는 이미지의 크기와 같고,
 // U,V의 크기는 가로가 2배작고, 세로가 2배작다.
 // 즉 U,V의 크기는 Y의 크기보다 4배가 작다.
 PkmpYV12Image = ^TkmpYV12Image;
 TkmpYV12Image = record
   Width, Height : integer; // 가로, 세로의 크기 // 가로의 경우는 16의 배수가 되어야 됨...
   y_plain : pChar; // Y플레인 버퍼, 크기:Width*Height
   u_plain : pChar; // U플레인 버퍼, 크기:Width*Height/4
   v_plain : pChar; // V플레인 버퍼, 크기:Width*Height/4
   YMask : pChar;    // 투명을 위한 Y마스크 내부적으로 사용
   UVMask : pChar;    // 투명을 위한 UV마스크 내부적으로 사용
 end;

 // YV12이미지를 관리하고 처리하는데 도움을 주는 인터페이스
 // 현재 RGB32,24,565,555의 DIB이미지만 지원한다.
 // 델파이의 경우는 DelphiX의 TDIB나 TBitmap의 DIB를 이용하면
 // 쉽게 DIB와 YV12Image를 처리할수가 있다.
 // 예제를 참고하기 바람...
 IkmpYV12ImageProcessor = interface
 ['{249F44BC-B8BF-411F-B047-D9C017DFECF2}']
   function kmpAllocYV12Image(width,height : integer;out pImage : PkmpYV12Image) : HResult; stdcall;
            // width, height의 크기로 YV12Image구조체를 만든다...
            // 2의 배수로 정렬이 되어 있어야 되고,
            // 가능하면 16의 배수가 되어 있어야만 안정하게 동작한다.
            // AllocYV12Image로 만든것은 아래의 FreeYV12Image로 해제를 해야 된다.
   function kmpFreeYV12Image(var pImage : PkmpYV12Image) : HResult; stdcall;
            // AllocYV12Image로 만든 이미지를 해제 한다.
   function kmpCopyYV12Image(pSrc,pDst : PkmpYV12Image) : HResult; stdcall;
            // YV12Image를 복사한다.
   function kmpRGB32ToYV12(DIB32 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
   function kmpRGB24ToYV12(DIB24 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
   function kmpRGB565ToYV12(DIB565 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
   function kmpRGB555ToYV12(DIB555 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
            // RGB포멧(DIB)을 YV12이미지로 바꾼다.
   function kmpYV12ToRGB32(pImage : PkmpYV12Image; DIB32 : pByte; Width, Height : integer) : HResult; stdcall;
   function kmpYV12ToRGB24(pImage : PkmpYV12Image; DIB24 : pByte; Width, Height : integer) : HResult; stdcall;
   function kmpYV12ToRGB565(pImage : PkmpYV12Image; DIB565 : pByte; Width, Height : integer) : HResult; stdcall;
   function kmpYV12ToRGB555(pImage : PkmpYV12Image; DIB555 : pByte; Width, Height : integer) : HResult; stdcall;
            // YV12Image의 형식으로 된것을 RGB(DIB)의 형식으로 바꾼다.
            // 이때 Height에 음의수를 넣어주면 화면이 뒤집어져서 나온다.
            // Width는 양만 지원함...
   function kmpYV12NormalDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer) : HResult; stdcall;
            // 아무런 효과 없이 이미지를 그린다.
   function kmpYV12BlendDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer;alpha : integer) : HResult; stdcall;
            // 반투명효과로 이미지를 그린다. 0<=alpha<=255
   function kmpYV12TransDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer) : HResult; stdcall;
            // 배경색을 투명으로 그린다. 배경색은 0번째 색을 기준으로 한다.
   function kmpYV12BlendTransDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer;alpha : integer) : HResult; stdcall;
            // 배경색을 투명으로 그리고 반투명효과로 그린다. 0<=alpha<=255
 end;


 PkmpDSPModule = ^TkmpDSPModule;
 TkmpDSPModule = record
   description : pchar;		// description
   hwndParent : HWND;      	// parent window (filled in by calling app)
   hDllInstance : LongWord;	// instance handle to this DLL (filled in by calling app)

   Config : procedure(this_mod : PkmpDSPModule); cdecl;  // configuration dialog (if needed)
   Init : function(this_mod : PkmpDSPModule) : integer; cdecl;      // 0 on success, creates window, etc (if needed)

   BeforeModifyImage : function(this_mod : PkmpDSPModule;image : PkmpYV12Image;
     ewidth : integer) : integer; cdecl; // 더블샘플전에 불리는 함수
   // kmp에서 사용하는 영상을 변경하는 함수...
   // image : 이미지의 버퍼, 위에서 설명
   // ewidth : 실질적인 이미지의 가로 길이
   // image의 width는 할당된 이미지의 가로길이이다.
   // 실질적인것과 할당적인것이 차이가 나는 이유는 필터들이 연결될때에는
   // 버퍼가 정렬이 되어 있어야 됨... 따라서 실제로 영상이 들어가 있는 길이와
   // 할당된 버퍼의 길이가 틀려지게됨...
   // 보통 화상을 처리하는 경우는 별로 신경을 않쓰도 되지만 좌우를 뒤집는 경우는
   // 영상의 끝을 알아야되기 때문에 파라메터로 넘겨준다...
   // 즉 image의 width=ewidth+?? 여기서 ??는 추가 바이트로 정렬을 위해서 추가된 바이트이다.
   // 세로에서는 정렬이 필요가 없다.
   // 영상은 크기를 변경할수가 없다.
   // 리턴값은 있지만 처리하지 않아도 된다.
   AfterModifyImage : function(this_mod : PkmpDSPModule;image : PkmpYV12Image;
     ewidth : integer) : integer; cdecl; // 더블샘플후에 불리는 함수
   // 파라미터는 위와 같음.
   // BeforeModifyImage의 경우는 더블샘플이 동작할때만 불리어지게 된다.
   // 따라서 좀더빠른 이미지 처리를 위해서는 BeforeModifyImage를
   // 처리하면 된다.
   // 즉 더블샘플 동작시  BeforeModifyImage -> AfterModifyImage
   //             안하면        AfterModifyImage
   //  처리를 안할려면 nil로 설정하면 됨....
   //
   Quit : procedure(this_mod : PkmpDSPModule); cdecl;    // called when unloading

   userData : Pointer; // user data, optional
   ImageProcessor : IkmpYV12ImageProcessor; // 이미지 처리를 위한 인터페이스
                                         // 위에서 설명
 end;


 PkmpDSPHeader = ^TkmpDSPHeader;
 TkmpDSPHeader = record
   version : integer;       // DSP_HDRVER
   description : pchar; // description of library
   kmpDSPModule : function(num : integer) : PkmpDSPModule; cdecl;	// module retrieval function
 end;

///////////////////////////////////////
// Video PlugIn DLL Exports...
///////////////////////////////////////
//function kmpDSPGetHeader2 : PkmpDSPHeader; cdecl;
// 영상 DLL에서 Export해야되는 함수의 형식
// 즉 위의 함수를 노출시켜야 한다....


implementation

end.
