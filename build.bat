cls
color 0a

call "%VS100COMNTOOLS%vsvars32.bat"

devenv.com ./IDE/Relkon6.sln /Rebuild Release

cd .\Firmware\STM32F107
call .\build.bat

pause

