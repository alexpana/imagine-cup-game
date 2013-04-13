call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86
set PATH=%PATH%;%~dp0\_external\2MGFX\
set MGFXTOOL=2MGFX.exe
setlocal ENABLEDELAYEDEXPANSION
mkdir /p ContentBuild

msbuild /property:XNAContentPipelineTargetPlatform="Windows" /p:Configuration=Release /property:XNAContentPipelineTargetProfile=Reach ICGameContent/ICGameContent.contentproj /p:OutputPath=ContentBuild

xcopy /y /d /i /s ICGameContent\ContentBuild\Content\* ContentBuild\

for /r %%i in (ICGameContent/effects/*.fx) do (
	set effinpath=%%~di%%~pi\ICGameContent\effects\%%~ni.fx
	set effoutpath=ContentBuild\effects\%%~ni.mgfxo
	rm ContentBuild\effects\%%~ni.xnb
	
	%MGFXTOOL% !effinpath! !effoutpath!	
	if errorlevel 1 goto :error
)


rm ICGameContent\cachefile-*.txt
goto :eof

:error
echo ERROR!!!!!!!!!!!

:eof
