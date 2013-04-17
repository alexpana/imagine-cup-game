echo off

echo [Compile Content Phase] ---- Setting Up ...

call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86
set PATH=%PATH%;%~dp0\_external\2MGFX\
set MGFXTOOL=2MGFX.exe
setlocal ENABLEDELAYEDEXPANSION

echo [Compile Content Phase] ---- Building content project ...
if not exist ContentBuild mkdir ContentBuild

msbuild /property:XNAContentPipelineTargetPlatform="Windows" /p:Configuration=Release /property:XNAContentPipelineTargetProfile=Reach Equitrilium.Content/Equitrilium.Content.contentproj /p:OutputPath=../ContentBuild

echo [Compile Content Phase] ---- Compiling shaders ...
xcopy /y /d /i /s Equitrilium.Content\effects\* ContentBuild\Content\effects\

for /r %%i in (Equitrilium.Content\effects\*.fx) do (	
	rem SHADER MODEL >= 4.0
	call :do_conversion ContentBuild\Content\effects\v4\ %%~ni /dx11
	
	rem SHADER MODEL < 4.0
	rem call :do_conversion ContentBuild\Content\effects\ %%~ni
)

echo [Compile Content Phase] ---- Cleanup ...

if exist Equitrilium.Content\cachefile-*.txt del Equitrilium.Content\cachefile-*.txt
goto :success

rem FUNCTIONS
:do_conversion <path> <filename> <extraparams> (
	set InPath=%1\%2.fx
	set OutPath=%1\%2.mgfxo
	
	if not exist !InPath! goto :return
	
	call :convert !InPath! !OutPath! %3
	
	:return
	exit /b
)

:convert <fx> <mgfxo> <extraparams> (
	if not exist %2 goto :convert
	if "%~t1" == "%~t2" goto :return
	for /F "delims=" %%i in ('dir /B /A-D /OD %1 %2') do set newest=%%i
	if "%newest%" == "%~n2%~x2" goto :return
	
	:convert
	echo Converting "%1" to mgfxo... ( with params: %3)
	
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
