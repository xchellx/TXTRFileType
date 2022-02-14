@echo off
%comspec% /k "set VSCMD_START_DIR="%~dp0" && "D:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvars64.bat" && dotnet publish .\libtxtr.csproj -r win-x64 -c Release --self-contained -p:PublishProfile=.\Properties\PublishProfiles\win-x64.pubxml & pause && exit"
