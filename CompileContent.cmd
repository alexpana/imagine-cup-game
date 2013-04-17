echo off

echo [Compile Content Phase] ---- Setting Up ...

call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86
set PATH=%PATH%;%~dp0\_external\2MGFX\
set MGFXTOOL=2MGFX.exe
setlocal ENABLEDELAYEDEXPANSION

echo [Compile Content Phase] ---- Building content project ...
mkdir /p ContentBuild

msbuild /property:XNAContentPipelineTargetPlatform="Windows" /p:Configuration=Release /property:XNAContentPipelineTargetProfile=Reach Equitrilium.Content/Equitrilium.Content.contentproj /p:OutputPath=ContentBuild

echo [Compile Content Phase] ---- Copying results from the content project ....
xcopy /y /d /i /s Equitrilium.Content\ContentBuild\Content\* ContentBuild\


echo [Compile Content Phase] ---- Compiling shaders ...
if not exist ContentBuild\effects\v4 mkdir ContentBuild\effects\v4

for /r %%i in (Equitrilium.Content\effects\*.fx) do (
	set EffV4InPath=Equitrilium.Content\effects\v4\%%~ni.fx
	set EffV4OutPath=ContentBuild\effects\v4\%%~ni.mgfxo
	
	set EffInPath=Equitrilium.Content\effects\%%~ni.fx
	set EffOutPath=ContentBuild\effects\%%~ni.mgfxo
	
	rem SHADER MODEL >= 4.0
	call :do_conversion !EffV4InPath! !EffV4OutPath! /dx11
	
	rem SHADER MODEL < 4.0
	call :do_conversion !EffInPath! !EffOutPath!
	
	if exist ContentBuild\effects\%%~ni.xnb del ContentBuild\effects\%%~ni.xnb
)

echo [Compile Content Phase] ---- Cleanup ...

rm Equitrilium.Content\cachefile-*.txt
goto :success


rem FUNCTIONS
:do_conversion <fx> <mgfxo> <extraparams> (
	if "%~t1" == "%~t2" goto :return
	rem TODO: fix this
	rem for /F %%i in ('dir /B /O:D %1 %2') do set newest=%%i
	rem if "%newest%" == "%~n2%~x2" goto :return
	rem echo Converting "%1" to mgfxo... ( with params: %3)
	
	%MGFXTOOL% %1 %2 %3
	
	if errorlevel 1 goto :error
	
	:return
	exit /b
)

:error
echo [Compile Content Phase] ---- ERROR OCURRED!
exit /b 1

:success
echo [Compile Content Phase] ---- Done
exit /b 0
