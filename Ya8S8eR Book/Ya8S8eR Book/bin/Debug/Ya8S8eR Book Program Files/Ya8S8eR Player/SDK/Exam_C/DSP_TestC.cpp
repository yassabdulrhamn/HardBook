// DSP_TestC.cpp : Defines the entry point for the DLL application.
//

#include <stdlib.h>
#include <windows.h>
#include "stdafx.h"
#include "..\KMPPlgIn.h"


winampDSPHeader _winampDSPHeader;
winampDSPModule _winampDSPModule;

kmpDSPHeader _kmpDSPHeader;
kmpDSPModule _kmpDSPModule[3];


winampDSPModule *getwinampDSPModule(int num)
{
 if(num==0) return &_winampDSPModule;
 else return NULL;
 }

int rand() // 비주얼 C에서 램덤변수를 못만들어서... 그냥 C는 되는데 
{  // 왜 윈도우에서는 안된는거야... 알아서 고쳐 써세요...
	 
	return 1;
}

int ModifyAudio(winampDSPModule *this_mod,short int *samples,
  int numsamples,int bps,int nch,int srate)
{
 int i,rr,aa;
 unsigned short *ps;
 unsigned char *pb;
 
 // 비주얼 C에서 랜덤변수를 못만들겟네...
 
 if (bps==16) {
	ps=(unsigned short *)samples;
	if (nch==2) {
		for(i=0;i<numsamples;i++) {
			rr=rand() % (0x8fff / 5);
            aa=*ps+rr;
            if(aa<-32768) aa=-32768;
			else if(aa>32767) aa=32767;
			*ps=aa;
			ps++;
			rr=rand() % (0x8fff / 5);
			aa=*ps+rr;
			if(aa<-32768) aa=-32768;
			else if(aa>32767) aa=32767;
			*ps=aa;
			ps++;
		}
	}
    else if(nch==1) {
		for(i=0;i<numsamples;i++) {
			rr=rand() % (0x8fff / 5);
			aa=*ps+rr;
			if(aa<-32768) aa=-32768;
			else if(aa>32767) aa=32767;
			*ps=aa;
			ps++;
		}
	}
 }
 else if(bps==8) {
	pb=(unsigned char *)samples;
	if(nch==2) {
		for(i=0;i<numsamples;i++) {
			rr=rand() % (0x7f / 5);
			aa=*pb+rr;
			if(aa<0) aa=0;
			else if(aa>255) aa=255;
			*pb=aa;
			pb++;
			rr=rand() % (0x7f / 5);
			aa=*pb+rr;
			if(aa<0) aa=0;
			else if(aa>255) aa=255;
			*pb=aa;
			pb++;
		}
	}
	else if(nch==1) {
		for(i=0;i<numsamples;i++) {
			rr=rand() % (0x7f / 5);
			aa=*pb+rr;
			if(aa<0) aa=0;
			else if(aa>255) aa=255;
			*pb=aa;
			pb++;
		}
	}
 }
 return numsamples;
}

kmpDSPModule *getkmpDSPModule(int num)
{
 if(num==0) return &_kmpDSPModule[0];
 else if(num==1) return &_kmpDSPModule[1];
 else if(num==2) return &_kmpDSPModule[2];
 else return NULL;
}

int ModifyVideoHori(kmpDSPModule *this_mod,kmpYV12Image *image,int ewidth)
{
 int i,j;
 unsigned char *ps,*pd;

 ps=image->y_plain;
 for(i=0;i<image->Height;i++) {
   pd=ps;
   for(j=0;j<ewidth;j++) {
     if ((j & 3)==0) *pd=0;
     pd++;
   }
   ps=ps+image->Width;
 }
 return 0;
}

int ModifyVideoVert(kmpDSPModule *this_mod,kmpYV12Image *image,int ewidth)
{
 int i;
 unsigned char *pp;

 pp=image->y_plain;
 for(i=0;i<image->Height;i++) {
   if ((i & 3)==0) memset(pp,0,ewidth);
   pp=pp+image->Width;
 }
 return 0;
}

/*

int TimeDisplay(kmpDSPModule *this_mod,kmpYV12Image *image,int ewidth)
{
 HWND han;
 HDC comhdc,hdc;
 HBITMAP hbitmap;
 HBRUSH hbrush;
 RECT rr;
 SYSTEMTIME time;
 char buf[200];
 void *dib;
 BITMAPINFO bitmapinfo;
 kmpYV12Image *Img;

 han=GetDesktopWindow();
 hdc=GetDC(han);
 comhdc=CreateCompatibleDC(hdc);
 SetRect(&rr,0,0,image->Width,20);
 hbitmap=CreateCompatibleBitmap(hdc,rr.right,rr.bottom);
 hbrush=CreateSolidBrush(RGB(0,0,0));
 SelectObject(comhdc,hbrush);
 SelectObject(comhdc,hbitmap);
 FillRect(comhdc,&rr,hbrush);
 SetTextColor(comhdc,RGB(255,255,255));
 SetBkMode(comhdc,TRANSPARENT);
 GetSystemTime(&time);
 sprintf(buf,"%02d:%02d:%02d",time.wHour,time.wMinute,time.wSecond);
 TextOut(comhdc,1,1,buf,strlen(buf));
 dib=malloc(rr.right*rr.bottom*4);
 memset((void *)&bitmapinfo,0,sizeof(bitmapinfo));
 bitmapinfo.bmiHeader.biBitCount=32;
 bitmapinfo.bmiHeader.biHeight=rr.bottom;
 bitmapinfo.bmiHeader.biWidth=rr.right;
 bitmapinfo.bmiHeader.biPlanes=1;
 bitmapinfo.bmiHeader.biSize=sizeof(bitmapinfo.bmiHeader);
 bitmapinfo.bmiHeader.biSizeImage=rr.right*rr.bottom*4;
 GetDIBits(comhdc,hbitmap,0,rr.bottom,dib,&bitmapinfo,DIB_RGB_COLORS);
 DeleteObject(hbrush);
 DeleteObject(hbitmap);
 DeleteDC(comhdc);
 ReleaseDC(han,hdc);
 // 아이가나 DIB를 만들기 위해서 이렇게 많이 해야 되나....
 // 음... 역시 C는 복잡해...
 // 진짜 복작해...
 // 아직 최적화 되지 않았습니다.
 // 고수님들은 알아서 고쳐 쓰세요...

 if(this_mod->ImageProcessor!=NULL) {
     this_mod->ImageProcessor->kmpAllocYV12Image(rr.right,rr.bottom,&Img);
     this_mod->ImageProcessor->kmpRGB32ToYV12(dib,rr.right,rr.bottom,Img);
     this_mod->ImageProcessor->kmpYV12BlendTransDraw(Img,image,&rr,0,0,200);
     this_mod->ImageProcessor->kmpFreeYV12Image(&Img);
 }
 free(dib);

 return 0;
}
*/

int TimeDisplay(kmpDSPModule *this_mod,kmpYV12Image *image,int ewidth) 
{ 
//HWND han; 
HDC comhdc,hdc; 
HBITMAP hbitmap; 
HBRUSH hbrush; 
RECT rr; 
SYSTEMTIME time; 
char buf[200]; 
void *dib; 
BITMAPINFO bitmapinfo; 
kmpYV12Image *Img; 

// DesktopDC를 얻어냄..// 
hdc=GetDC(NULL); 
comhdc=CreateCompatibleDC(hdc); 
SetRect(&rr,0,0,image->Width,20); 
// CreateDIBSection을 하기 위한 설정 // 
memset((void *)&bitmapinfo,0,sizeof(bitmapinfo)); 
bitmapinfo.bmiHeader.biBitCount=32; 
bitmapinfo.bmiHeader.biHeight=rr.bottom; 
bitmapinfo.bmiHeader.biWidth=rr.right; 
bitmapinfo.bmiHeader.biPlanes=1; 
bitmapinfo.bmiHeader.biSize=sizeof(bitmapinfo.bmiHeader); 
bitmapinfo.bmiHeader.biSizeImage=rr.right*rr.bottom*4; 
bitmapinfo.bmiHeader.biCompression   = BI_RGB; 
// CreateDIBSection은 DDB(HBITMAP)하고 DIB을 동시에 생성한다.// 
hbitmap= CreateDIBSection(hdc, &bitmapinfo, DIB_RGB_COLORS, 
&dib, NULL, NULL); 
HBITMAP hOldBitmap = (HBITMAP)SelectObject(comhdc, hbitmap); 

// 이미지를 처리하는 과정 임.. // 
hbrush=CreateSolidBrush(RGB(0,0,0)); 
FillRect(comhdc,&rr,hbrush); 
DeleteObject(hbrush); 
SetTextColor(comhdc,RGB(255,255,255)); 
SetBkMode(comhdc,TRANSPARENT); 
GetSystemTime(&time); 
sprintf(buf,"%02d:%02d:%02d",time.wHour,time.wMinute,time.wSecond); 
TextOut(comhdc,1,1,buf,strlen(buf)); 
//dib=malloc(rr.right*rr.bottom*4); 
//GetDIBits(comhdc,hbitmap,0,rr.bottom,dib,&bitmapinfo,DIB_RGB_COLORS); 
//DeleteObject(hbitmap); <-- DeleteObject를 하면 dib까지 없어짐...// 
SelectObject(comhdc, hOldBitmap); 
DeleteDC(comhdc); 
ReleaseDC(NULL,hdc); 
// 아이가나 DIB를 만들기 위해서 이렇게 많이 해야 되나.... 
// 음... 역시 C는 복잡해... 
// 진짜 복작해... 
// 아직 최적화 되지 않았습니다. 
// 고수님들은 알아서 고쳐 쓰세요... 

if(this_mod->ImageProcessor!=NULL) { 
    this_mod->ImageProcessor->kmpAllocYV12Image(rr.right,rr.bottom,&Img); 
    this_mod->ImageProcessor->kmpRGB32ToYV12(dib,rr.right,rr.bottom,Img); 
    this_mod->ImageProcessor->kmpYV12BlendTransDraw(Img,image,&rr,0,0,200); 
    this_mod->ImageProcessor->kmpFreeYV12Image(&Img); 
} 
//free(dib); <--- 대신에 DeleteObject로 dib까지 지움// 
DeleteObject(hbitmap); 

return 0; 
} 



winampDSPHeader *winampDSPGetHeader2() // 오디오플러그인 Export함수
{
 // For Audio
 _winampDSPHeader.version=0x20;
 _winampDSPHeader.description="Test for Audio Plugin K-MultimediaPlayer";
 _winampDSPHeader.getModule=&getwinampDSPModule;

 _winampDSPModule.description="Add noise v0.001 for K-MultimediaPlayer";
 _winampDSPModule.hwndParent=0;
 _winampDSPModule.hDllInstance=0;
 _winampDSPModule.Config=NULL;
 _winampDSPModule.Init=NULL;
 _winampDSPModule.ModifySamples=ModifyAudio;
 _winampDSPModule.Quit=NULL;
 _winampDSPModule.userData=NULL;

 return &_winampDSPHeader;
}


kmpDSPHeader *kmpDSPGetHeader2() //비디오 플러그인 Export함수
{
 // For Video
 _kmpDSPHeader.version=0x20;
 _kmpDSPHeader.description="Test for Video Plugin K-MultimediaPlayer";
 _kmpDSPHeader.getModule=getkmpDSPModule;

 _kmpDSPModule[0].description="Hori Scanline v0.001 for K-MultimediaPlayer";
 _kmpDSPModule[0].hwndParent=0;
 _kmpDSPModule[0].hDllInstance=0;
 _kmpDSPModule[0].Config=NULL;
 _kmpDSPModule[0].Init=NULL;
 _kmpDSPModule[0].BeforeModifySamples=NULL;
 _kmpDSPModule[0].AfterModifySamples=ModifyVideoHori;
 _kmpDSPModule[0].Quit=NULL;
 _kmpDSPModule[0].userData=NULL;

 _kmpDSPModule[1].description="Hori Scanline v0.001 for K-MultimediaPlayer";
 _kmpDSPModule[1].hwndParent=0;
 _kmpDSPModule[1].hDllInstance=0;
 _kmpDSPModule[1].Config=NULL;
 _kmpDSPModule[1].Init=NULL;
 _kmpDSPModule[1].BeforeModifySamples=NULL;
 _kmpDSPModule[1].AfterModifySamples=ModifyVideoVert;
 _kmpDSPModule[1].Quit=NULL;
 _kmpDSPModule[1].userData=NULL;

 _kmpDSPModule[2].description="Time Display v0.001 for K-MultimediaPlayer";
 _kmpDSPModule[2].hwndParent=0;
 _kmpDSPModule[2].hDllInstance=0;
 _kmpDSPModule[2].Config=NULL;
 _kmpDSPModule[2].Init=NULL;
 _kmpDSPModule[2].BeforeModifySamples=NULL;
 _kmpDSPModule[2].AfterModifySamples=TimeDisplay;
 _kmpDSPModule[2].Quit=NULL;
 _kmpDSPModule[2].userData=NULL;

 return &_kmpDSPHeader;
}



BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    return TRUE;
}

