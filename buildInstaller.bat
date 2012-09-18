call .\build.bat

cd ..\..\

.\Installer\makensis.exe .\Installer\Relkon6.nsi
set /p ver=<version
move /Y .\SetupRelkon.exe .\SetupRelkon-%ver%.exe