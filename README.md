# NOTE: This is no longer maintained, see [txtrtool](https://github.com/xchellx/txtrtool) instead.

# TXTRFileType
TXTRFileType is a collection of various tools, interfaces, and plugins to read and write TXTR files
from the Metroid Prime series.

**Note:** GX2 support is not included and not planned therefore TXTR files from Donkey Kong Country:
Tropical Freeze is not supported. This is open for contributions, however.

## Features
- Supports loading and saving all texture formats
- Supports loading and generating mipmaps
- Automatic image coordinate flipping
- A managed library TXTRFileTypeLib
- A command line program that interfaces with TXTRFileTypeLib
- A unmanaged library libtxtr that interfaces with TXTRFileTypeLib (cdecl calling convention)
- A python library libtxtrPython (libtxtr) that interfaces with libtxtr
- A plugin for Paint.NET
- A plugin for Krita (supports Windows x64 and Linux x64)

## Building
### Requirements
- Visual Studio 2022. [Get it here](https://visualstudio.microsoft.com/downloads/).
- .NET 6.0 SDK. [Get it here](https://dotnet.microsoft.com/download/dotnet/6.0).
- Paint.NET (installed, store, or portable version). [Get it from the official website](https://www.getpaint.net) or [the Microsoft Store](https://www.microsoft.com/en-us/p/paintnet/9nbhcs1lx4r0).
- A Windows OS to build libtxtr for Windows
- A Linux OS to build libtxtr for Linux

### TXTRFileTypeLib
#### Building TXTRFileTypeLib
Select Release and AnyCPU for the configuration then build.

#### Installing TXTRFileTypeLib
After building, copy TXTRFileTypeLib.dll, TXTRFileTypeLib.deps.json file, and all the dependencies
required specified inside the `"dependencies":` entry inside TXTRFileTypeLib.deps.json to wherever
you need to use TXTRFileTypeLib.

### TXTRFileType
#### Building TXTRFileType
The debug start action and library includes expect everything to be in `C:\Program Files\paint.net`.
If you installed Paint.NET to somewhere else or installed the Microsoft Store version, then you must
edit the csproj to reflect the location to where you installed it. Select Release and AnyCPU for the
configuration then build.

#### Installing TXTRFileType
After building, copy TXTRFileType.dll, TXTRFileType.deps.json file, and all the dependencies
required specified inside the `"dependencies":` entry inside TXTRFileType.deps.json to
`<PDNINSTALLDIR>\FileTypes\TXTRFileType` where `<PDNINSTALLDIR>` is the location Paint.NET, such as
`C:\Program Files\paint.net`.

### TXTRFileTypeCLI
#### Building TXTRFileTypeCLI
Right click the .csproj and click publish then choose the required configuration and click publish.

##### NOTE:
Do not strip the executable on Linux, the extra data in it is compressed libraries of the dotnet
runtime and other dependecies which need to be decompressed to memory at runtime (thus allowing the
executable to be self contained)

#### Installing TXTRFileTypeCLI
After building, copy the txtrtool.exe to wherever you need to use TXTRFileTypeCLI, copy txtrtool
to your OS's binary directory, and/or add txtrtool to your OS's PATH environment variable.

### libtxtr
#### Building libtxtr
An experimental IL compiler required which is only supported by dotnet CLI (at the time of writing).
Do not use VS2022 to publish, use 'dotnet publish' instead. This compiler is available in the
`Microsoft.DotNet.ILCompiler` nuget package, which is in a custom nuget source. Two scripts are
provided for convenience: `publish_win-x64.bat` for building libtxtr for Windows and
`publish_linux-x64.sh` for building libtxtr for Linux.
##### Note:
Cross compiling is not possible yet for native AOT. You must build for Windows on Windows and for
Linux on Linux. You cant build for one from the other and vice versa.

You can technically build libtxtr normally for use with COM interop or just normal use but there
isn't much benefit for that aswell as it is not a supported use case here.

The required nuget package for compiling to native AOT (`Microsoft.DotNet.ILCompiler`) is found at
the dotnet-experimental custom nuget source.

Also, x86 is not possible for native AOT yet; see https://github.com/dotnet/corert/issues/4589 and
https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/src/installer/pkg/projects/Microsoft.DotNet.ILCompiler/ILCompilerRIDs.props

##### dotnet-experimental nuget source info:
- **Name**: dotnet-experimental
- **Source**: https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-experimental/nuget/v3/index.json
##### More Info on Native AOT
- https://github.com/dotnet/corert/tree/master/samples/NativeLibrary
- https://github.com/dotnet/runtimelab/tree/feature/NativeAOT/samples/NativeLibrary
- https://github.com/dotnet/corert/blob/master/Documentation/how-to-build-and-run-ilcompiler-in-console-shell-prompt.md
- https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/README.md
- https://coderator.net/c-experimental-serisi-native-kutuphane-derliyoruz/
- http://web.archive.org/web/20210222045218/https://coderator.net/c-experimental-serisi-native-kutuphane-derliyoruz/
##### Soutions for Common Problems with Native AOT
- https://github.com/dotnet/corert/issues/5742
- https://github.com/dotnet/corert/issues/6282
- https://github.com/dotnet/corert/issues/5289
- https://github.com/dotnet/runtimelab/issues/589

#### Installing libtxtr
After building, copy the library to wherever you need to use libtxtr.

### libtxtrPython
#### Building libtxtrPython
No building is required.

#### Installing libtxtrPython
Simply copy libtxtrPython.py to wherever you need use libtxtrPython.

### TXTRFileTypeKrita
#### Building TXTRFileTypeKrita
No building is required for TXTRFileTypeKrita but libtxtr is required so that must be built.

#### Installing TXTRFileTypeKrita
Copy TXTRFileTypeKrita.desktop, the TXTRFileTypeKrita folder, and libtxtr.dll/libtxtr.so to
`%APPDATA%\krita\pykrita` on Windows and `~/.local/share/krita/pykrita` on Linux. Then, in Krita, go
to `Settings` -> `Configure Krita...` then click on `Python Plugin Manager` find
`TXTR Import/Export` and tick the checkbox next it. Restart Krita and go to `Settings` -> `Dockers`
then find `TXTR Import/Export` and tick the checkbox next to it. You should see the
`TXTR Import/Export` dock on the right side.

## License
```
TXTRFileType
Copyright (C) 2021 xchellx

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
```