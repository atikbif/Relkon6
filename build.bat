cls
color 0a

.\versionUpdater.exe 6 3 03.08.2012

%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe .\IDE\Relkon6.sln /t:Rebuild /p:Configuration=Release

cd .\Firmware\STM32F107
call .\build.bat

