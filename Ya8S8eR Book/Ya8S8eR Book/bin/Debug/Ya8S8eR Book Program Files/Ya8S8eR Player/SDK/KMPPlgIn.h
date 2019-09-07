#include <windows.h>
#include <objbase.h>
#include <initguid.h>
#if (1100 > _MSC_VER)
#include <olectlid.h>
#else
#include <olectl.h>
#endif
#include <stdio.h>


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


// header version: 0x20 == 0.20 == winamp 2.0
#define DSP_HDRVER 0x20



// DSP plugin interface

// notes:
// any window that remains in foreground should optimally pass unused
// keystrokes to the parent (winamp's) window, so that the user
// can still control it. As for storing configuration,
// Configuration data should be stored in <dll directory>\plugin.ini
// (look at the vis plugin for configuration code)

typedef struct winampDSPModule {
  char *description;		// description
  HWND hwndParent;			// parent window (filled in by calling app)
  HINSTANCE hDllInstance;	// instance handle to this DLL (filled in by calling app)

  void (*Config)(struct winampDSPModule *this_mod);  // configuration dialog (if needed)
  int (*Init)(struct winampDSPModule *this_mod);     // 0 on success, creates window, etc (if needed)

  // modify waveform samples: returns number of samples to actually write
  // (typically numsamples, but no more than twice numsamples, and no less than half numsamples)
  // numsamples should always be at least 128. should, but I'm not sure
  int (*ModifySamples)(struct winampDSPModule *this_mod, short int *samples, int numsamples, int bps, int nch, int srate);
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

  
  void (*Quit)(struct winampDSPModule *this_mod);    // called when unloading

  void *userData; // user data, optional
} winampDSPModule;

typedef struct {
  int version;       // DSP_HDRVER
  char *description; // description of library
  winampDSPModule* (*getModule)(int);	// module retrieval function
} winampDSPHeader;

// exported symbols
typedef winampDSPHeader* (*winampDSPGetHeaderType)();
// winampDSPHeader *winampDSPGetHeader2()
// ���� DLL���� Exports�ؾ� �Ǵ� �Լ��� ����
// �� ���� �Լ��� ������Ѿ� �Ѵ�....





////////////////////////////////////////////////////////////////////////////
//
// Video PlugIn Type( Modifyed from Winamp DSP Plugin type ...)
//
// �������� DSP�÷������� ������ ���� �����ؼ� KMP�� ����
// �÷������� ����Ѵ�.
////////////////////////////////////////////////////////////////////////////




 // KMP���� �����Ǵ� ȭ���� ����
 // KMP������ �÷��� YV12�̹����� ������ ����Ѵ�.
 // Y�� ũ��� �̹����� ũ��� ����,
 // U,V�� ũ��� ���ΰ� 2���۰�, ���ΰ� 2���۴�.
 // �� U,V�� ũ��� Y�� ũ�⺸�� 4�谡 �۴�.
typedef struct kmpYV12Image {
   int Width, Height; // ����, ������ ũ��
   unsigned char *y_plain; // Y�÷��� ����, ũ��:Width*Height
   unsigned char *u_plain; // U�÷��� ����, ũ��:Width*Height/4
   unsigned char *v_plain; // V�÷��� ����, ũ��:Width*Height/4
   unsigned char *Mask;    // ������ ���� ����ũ ���������� ���
} kmpYV12Image;


DEFINE_GUID(IID_IYV12ImageProcessor,
0x249F44BC, 0xB8BF, 0x411F, 0xB0, 0x47, 0xD9, 0xC0, 0x17, 0xDF, 0xEC, 0xF2);

DECLARE_INTERFACE_(IkmpYV12ImageProcessor, IUnknown)
{
 // YV12�̹����� �����ϰ� ó���ϴµ� ������ �ִ� �������̽�
 // ���� RGB32,24,565,555�� DIB�̹����� �����Ѵ�.
 // ������ �����ϱ� �ٶ�...
   STDMETHOD(kmpAllocYV12Image) (THIS_
		int width,int height,kmpYV12Image **pImage
             ) PURE;
			// width, height�� ũ��� YV12Image����ü�� �����...
            // 2�� ����� ������ �Ǿ� �־�� �ǰ�,
            // �����ϸ� 16�� ����� �Ǿ� �־�߸� �����ϰ� �����Ѵ�.
            // AllocYV12Image�� ������� �Ʒ��� FreeYV12Image�� ������ �ؾ� �ȴ�.
   STDMETHOD(kmpFreeYV12Image) (THIS_
		kmpYV12Image **pImage
			) PURE;
            // AllocYV12Image�� ���� �̹����� ���� �Ѵ�.
   STDMETHOD(kmpCopyYV12Image) (THIS_
		kmpYV12Image *pSrc,kmpYV12Image *pDst
			) PURE;
            // YV12Image�� �����Ѵ�.
   STDMETHOD(kmpRGB32ToYV12) (THIS_
		void *DIB32,int Width,int Height,kmpYV12Image *pImage
			) PURE;
   STDMETHOD(kmpRGB24ToYV12) (THIS_
		void *DIB24,int Width,int Height,kmpYV12Image *pImage
			) PURE;
   STDMETHOD(kmpRGB565ToYV12) (THIS_
		void *DIB565,int Width,int Height,kmpYV12Image *pImage
			) PURE;
   STDMETHOD(kmpRGB555ToYV12) (THIS_
		void *DIB555,int Width,int Height,kmpYV12Image *pImage
			) PURE;
            // RGB����(DIB)�� YV12�̹����� �ٲ۴�.
   STDMETHOD(kmpYV12ToRGB32) (THIS_
		kmpYV12Image *pImage,void *DIB32,int Width,int Height
			) PURE;
   STDMETHOD(kmpYV12ToRGB24) (THIS_
		kmpYV12Image *pImage,void *DIB24,int Width,int Height
			) PURE;
   STDMETHOD(kmpYV12ToRGB565) (THIS_
		kmpYV12Image *pImage,void *DIB565,int Width,int Height
			) PURE;
   STDMETHOD(kmpYV12ToRGB555) (THIS_
		kmpYV12Image *pImage,void *DIB555,int Width,int Height
			) PURE;
            // YV12Image�� �������� �Ȱ��� RGB(DIB)�� �������� �ٲ۴�.
            // �̶� Height�� ���Ǽ��� �־��ָ� ȭ���� ���������� ���´�.
            // Width�� �縸 ������...
   STDMETHOD(kmpYV12NormalDraw) (THIS_
		kmpYV12Image *Src,kmpYV12Image *Dst,RECT *SRect,int dx,int dy
			) PURE;
            // �ƹ��� ȿ�� ���� �̹����� �׸���.
   STDMETHOD(kmpYV12BlendDraw) (THIS_
		kmpYV12Image *Src,kmpYV12Image *Dst,RECT *SRect,int dx,int dy,int alpha
			) PURE;
            // ������ȿ���� �̹����� �׸���. 0<=alpha<=255
   STDMETHOD(kmpYV12TransDraw) (THIS_
		kmpYV12Image *Src,kmpYV12Image *Dst,RECT *SRect,int dx,int dy
			) PURE;
            // ������ �������� �׸���. ������ 0��° ���� �������� �Ѵ�.
   STDMETHOD(kmpYV12BlendTransDraw) (THIS_
		kmpYV12Image *Src,kmpYV12Image *Dst,RECT *SRect,int dx,int dy,int alpha
			) PURE;
            // ������ �������� �׸���, ��ü�� ������ȿ���� �׸���. 0<=alpha<=255
};



typedef struct kmpDSPModule {
  char *description;		// description
  HWND hwndParent;			// parent window (filled in by calling app)
  HINSTANCE hDllInstance;	// instance handle to this DLL (filled in by calling app)

  void (*Config)(struct kmpDSPModule *this_mod);  // configuration dialog (if needed)
  int (*Init)(struct kmpDSPModule *this_mod);     // 0 on success, creates window, etc (if needed)

  // modify waveform samples: returns number of samples to actually write
  // (typically numsamples, but no more than twice numsamples, and no less than half numsamples)
  // numsamples should always be at least 128. should, but I'm not sure
  int (*BeforeModifySamples)(struct kmpDSPModule *this_mod,struct kmpYV12Image *image,int ewidth); // ������ �����ϴ��Լ�
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
  int (*AfterModifySamples)(struct kmpDSPModule *this_mod,struct kmpYV12Image *image,int ewidth); // ������ �����ϴ��Լ�
   // �Ķ���ʹ� ���� ����.
   // BeforeModifyImage�� ���� ��������� �����Ҷ��� �Ҹ������� �ȴ�.
   // ���� �������� �̹��� ó���� ���ؼ��� BeforeModifyImage��
   // ó���ϸ� �ȴ�.
   // �� ������� ���۽�  BeforeModifyImage -> AfterModifyImage
   //             ���ϸ�        AfterModifyImage
   //  ó���� ���ҷ��� NULL�� �����ϸ� ��....
   //

  void (*Quit)(struct kmpDSPModule *this_mod);    // called when unloading

  void *userData; // user data, optional
  IkmpYV12ImageProcessor *ImageProcessor; // �̹���ó���� ���� �������̽�
                                          // ������ ����  
} kmpDSPModule;

typedef struct {
  int version;       // DSP_HDRVER
  char *description; // description of library
  kmpDSPModule* (*getModule)(int);	// module retrieval function
} kmpDSPHeader;

// exported symbols
typedef kmpDSPHeader* (*kmpDSPGetHeaderType)();
// kmpDSPHeader *kmpDSPGetHeader2()
// ���� DLL���� Export�ؾߵǴ� �Լ��� ����
// �� ���� �Լ��� ������Ѿ� �Ѵ�....
