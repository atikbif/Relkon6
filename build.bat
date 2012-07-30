cls
color 0a

%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe ./IDE/Relkon6.sln /t:Rebuild /p:Configuration=Release

cd .\Firmware\STM32F107
call .\build.bat

cd ..\..\Distr

Relkon6.exe

