/////////////////////////////////////////////////////////////////////////////
//
//          KMP�� �����/���� �÷����� �������̽�
//
//  KMP�� �÷����� ����� �������� DSP�÷����ΰ�
// ������ ���� ����. ����� �÷������� ���� �Ⱦ����� ������
// ���� ������ �������� DSP�÷������� KMP���� ����ϴ� ���� �����ϴ�.
// ���� �÷������� ��쵵 ���� ����ϹǷ� ���� �ۼ��Ҽ��� �������̴�.
// �׸��� �Լ� �Ծ��� cdel�̴�.. ����...
//
// �����÷������� ���� WaSaVi�� �޶�����... ���� ����...
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
// �������� DSP�������ΰ� ������ 100%��ġ�Ѵ�.
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
   // ���������� ����ϴ� ������ �����ϴ� �Լ�...
   // samples : ������ �� �ִ� ����
   // numsamples : ������ ����(�������� ������ ���̰� �ƴ϶� ������ ������...
   // bps : ���ô� ��Ʈ�� 8 or 16
   // nch : ä�μ� 1 : ���  , 2 : ��Ʈ����
   // srate : ���ø� ����Ʈ 44000���..
   // ���Լ����� ���ϰ��� ������ ����Ÿ�� �����̴�. ���� ���������� �������
   // ������ �����̱⵵ �ϴ�. ���������� �׽�Ʈ�� ���� �Է¹��� ���ú���
   // �����ص� ������ �Ͽ���. ���� KMP������ �Էº��� 3-4��������
   // �޸𸮸� ���������� �Ҵ��� �س��Ҵ�. �׷��Ƿ� 3-4�� �̻��� ó���ϸ� ������
   // �Ͼ���� �ִ�...
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
// ���� DLL���� Exports�ؾ� �Ǵ� �Լ��� ����
// �� ���� �Լ��� ������Ѿ� �Ѵ�....





////////////////////////////////////////////////////////////////////////////
//
// Video PlugIn Type( Modifyed from Winamp DSP Plugin type ...)
//
// �������� DSP�÷������� ������ ���� �����ؼ� KMP�� ����
// �÷������� ����Ѵ�.
////////////////////////////////////////////////////////////////////////////

const
 IID_IYV12ImageProcessor : TGUID = '{249F44BC-B8BF-411F-B047-D9C017DFECF2}';


type
 // KMP���� �����Ǵ� ȭ���� ����
 // KMP������ �÷��� YV12�̹����� ������ ����Ѵ�.
 // Y�� ũ��� �̹����� ũ��� ����,
 // U,V�� ũ��� ���ΰ� 2���۰�, ���ΰ� 2���۴�.
 // �� U,V�� ũ��� Y�� ũ�⺸�� 4�谡 �۴�.
 PkmpYV12Image = ^TkmpYV12Image;
 TkmpYV12Image = record
   Width, Height : integer; // ����, ������ ũ�� // ������ ���� 16�� ����� �Ǿ�� ��...
   y_plain : pChar; // Y�÷��� ����, ũ��:Width*Height
   u_plain : pChar; // U�÷��� ����, ũ��:Width*Height/4
   v_plain : pChar; // V�÷��� ����, ũ��:Width*Height/4
   YMask : pChar;    // ������ ���� Y����ũ ���������� ���
   UVMask : pChar;    // ������ ���� UV����ũ ���������� ���
 end;

 // YV12�̹����� �����ϰ� ó���ϴµ� ������ �ִ� �������̽�
 // ���� RGB32,24,565,555�� DIB�̹����� �����Ѵ�.
 // �������� ���� DelphiX�� TDIB�� TBitmap�� DIB�� �̿��ϸ�
 // ���� DIB�� YV12Image�� ó���Ҽ��� �ִ�.
 // ������ �����ϱ� �ٶ�...
 IkmpYV12ImageProcessor = interface
 ['{249F44BC-B8BF-411F-B047-D9C017DFECF2}']
   function kmpAllocYV12Image(width,height : integer;out pImage : PkmpYV12Image) : HResult; stdcall;
            // width, height�� ũ��� YV12Image����ü�� �����...
            // 2�� ����� ������ �Ǿ� �־�� �ǰ�,
            // �����ϸ� 16�� ����� �Ǿ� �־�߸� �����ϰ� �����Ѵ�.
            // AllocYV12Image�� ������� �Ʒ��� FreeYV12Image�� ������ �ؾ� �ȴ�.
   function kmpFreeYV12Image(var pImage : PkmpYV12Image) : HResult; stdcall;
            // AllocYV12Image�� ���� �̹����� ���� �Ѵ�.
   function kmpCopyYV12Image(pSrc,pDst : PkmpYV12Image) : HResult; stdcall;
            // YV12Image�� �����Ѵ�.
   function kmpRGB32ToYV12(DIB32 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
   function kmpRGB24ToYV12(DIB24 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
   function kmpRGB565ToYV12(DIB565 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
   function kmpRGB555ToYV12(DIB555 : pByte; Width, Height : integer;pImage : PkmpYV12Image) : HResult; stdcall;
            // RGB����(DIB)�� YV12�̹����� �ٲ۴�.
   function kmpYV12ToRGB32(pImage : PkmpYV12Image; DIB32 : pByte; Width, Height : integer) : HResult; stdcall;
   function kmpYV12ToRGB24(pImage : PkmpYV12Image; DIB24 : pByte; Width, Height : integer) : HResult; stdcall;
   function kmpYV12ToRGB565(pImage : PkmpYV12Image; DIB565 : pByte; Width, Height : integer) : HResult; stdcall;
   function kmpYV12ToRGB555(pImage : PkmpYV12Image; DIB555 : pByte; Width, Height : integer) : HResult; stdcall;
            // YV12Image�� �������� �Ȱ��� RGB(DIB)�� �������� �ٲ۴�.
            // �̶� Height�� ���Ǽ��� �־��ָ� ȭ���� ���������� ���´�.
            // Width�� �縸 ������...
   function kmpYV12NormalDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer) : HResult; stdcall;
            // �ƹ��� ȿ�� ���� �̹����� �׸���.
   function kmpYV12BlendDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer;alpha : integer) : HResult; stdcall;
            // ������ȿ���� �̹����� �׸���. 0<=alpha<=255
   function kmpYV12TransDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer) : HResult; stdcall;
            // ������ �������� �׸���. ������ 0��° ���� �������� �Ѵ�.
   function kmpYV12BlendTransDraw(Src,Dst : PkmpYV12Image;SRect : PRect;dx,dy : integer;alpha : integer) : HResult; stdcall;
            // ������ �������� �׸��� ������ȿ���� �׸���. 0<=alpha<=255
 end;


 PkmpDSPModule = ^TkmpDSPModule;
 TkmpDSPModule = record
   description : pchar;		// description
   hwndParent : HWND;      	// parent window (filled in by calling app)
   hDllInstance : LongWord;	// instance handle to this DLL (filled in by calling app)

   Config : procedure(this_mod : PkmpDSPModule); cdecl;  // configuration dialog (if needed)
   Init : function(this_mod : PkmpDSPModule) : integer; cdecl;      // 0 on success, creates window, etc (if needed)

   BeforeModifyImage : function(this_mod : PkmpDSPModule;image : PkmpYV12Image;
     ewidth : integer) : integer; cdecl; // ����������� �Ҹ��� �Լ�
   // kmp���� ����ϴ� ������ �����ϴ� �Լ�...
   // image : �̹����� ����, ������ ����
   // ewidth : �������� �̹����� ���� ����
   // image�� width�� �Ҵ�� �̹����� ���α����̴�.
   // �������ΰͰ� �Ҵ����ΰ��� ���̰� ���� ������ ���͵��� ����ɶ�����
   // ���۰� ������ �Ǿ� �־�� ��... ���� ������ ������ �� �ִ� ���̿�
   // �Ҵ�� ������ ���̰� Ʋ�����Ե�...
   // ���� ȭ���� ó���ϴ� ���� ���� �Ű��� �ʾ��� ������ �¿츦 ������ ����
   // ������ ���� �˾ƾߵǱ� ������ �Ķ���ͷ� �Ѱ��ش�...
   // �� image�� width=ewidth+?? ���⼭ ??�� �߰� ����Ʈ�� ������ ���ؼ� �߰��� ����Ʈ�̴�.
   // ���ο����� ������ �ʿ䰡 ����.
   // ������ ũ�⸦ �����Ҽ��� ����.
   // ���ϰ��� ������ ó������ �ʾƵ� �ȴ�.
   AfterModifyImage : function(this_mod : PkmpDSPModule;image : PkmpYV12Image;
     ewidth : integer) : integer; cdecl; // ��������Ŀ� �Ҹ��� �Լ�
   // �Ķ���ʹ� ���� ����.
   // BeforeModifyImage�� ���� ��������� �����Ҷ��� �Ҹ������� �ȴ�.
   // ���� �������� �̹��� ó���� ���ؼ��� BeforeModifyImage��
   // ó���ϸ� �ȴ�.
   // �� ������� ���۽�  BeforeModifyImage -> AfterModifyImage
   //             ���ϸ�        AfterModifyImage
   //  ó���� ���ҷ��� nil�� �����ϸ� ��....
   //
   Quit : procedure(this_mod : PkmpDSPModule); cdecl;    // called when unloading

   userData : Pointer; // user data, optional
   ImageProcessor : IkmpYV12ImageProcessor; // �̹��� ó���� ���� �������̽�
                                         // ������ ����
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
// ���� DLL���� Export�ؾߵǴ� �Լ��� ����
// �� ���� �Լ��� ������Ѿ� �Ѵ�....


implementation

end.
