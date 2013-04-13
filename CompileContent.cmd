call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86

mkdir /p ContentBuild

msbuild /property:XNAContentPipelineTargetPlatform="Windows" /p:Configuration=Release /property:XNAContentPipelineTargetProfile=Reach ICGameContent/ICGameContent.contentproj /p:OutputPath=ContentBuild

xcopy /y /d /i /s ICGameContent\ContentBuild\Content\* ContentBuild\

rm ICGameContent\cachefile-*.txt