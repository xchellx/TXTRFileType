#!/bin/bash
dotnet publish ./libtxtr.csproj -r linux-x64 -c Release --self-contained -p:PublishProfile=./Properties/PublishProfiles/linux-x64.pubxml
strip ./bin/publish/x64/Release/linux-x64/libtxtr.so
read -rsp $'Press any key to continue...\n' -n 1 key && exit
