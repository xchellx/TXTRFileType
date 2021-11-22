# TXTRFileType
TXTR file type plugin for Paint.NET to load TXTR files from the Metroid Prime series.
With this you'll be able to extract, edit, and create TXTR texture files for or from the Metroid prime series.

**Note:** GX2 support is not included and not planned therefore TXTR files from the Donkey Kong Country Returns and Donkey Kong Country: Tropical Freeze games are not supported. This is open for contributions, however.

## Features
- Supports loading and saving all texture formats
- Supports loading and generating mipmaps
- Automatic image coordinate flipping

## Building
### Requirements
- Visual Studio 2019 or greater. [Get it here](https://visualstudio.microsoft.com/downloads/).
- .NET 6.0 SDK. [Get it here](https://dotnet.microsoft.com/download/dotnet/6.0).
- Paint.NET (installed, store, or portable version). [Get it from the official website](https://www.getpaint.net) or [the Microsoft Store](https://www.microsoft.com/en-us/p/paintnet/9nbhcs1lx4r0).

After building, you should copy the DLL of the plugin, it's .deps.json file, and all the DLLs required specified inside the `"dependencies":` entry inside the .deps.json to `%PDNINSTALLDIR%\FileTypes\TXTRFileType`.

### Environment Variables And Post Build Event
There is a post build event that copies the plugin's .dll and .deps.json to the plugin directory (as well as creating this directory).

The dependencies specified in .deps.json (and for the dependencies of the dependencies of their selves) must always be manually copied to the plugin directory.

If this post build event is disabled, then plugin DLL and .deps.json file must be manually copied to the plugin directory.

The project and the post build event depends on some environment variables:

- Set the environment variable `PDNINSTALLDIR` to where you have paint.net installed, whether that be the path to the installed version, portable version, or store version. Example: `C:\Program Files\paint.net` (no leading backslash). This is required for the PostBuild event and for the paint.net DLL references.
- Set the environment variable `PDNENABLEPBE` to 'true' to enable post build event and set to `"false"` disable it. Unless you are debugging, set to `"false"` otherwise keep it `"true"`.
- Set the environment variable `PDNPLUGINTYPE` to `"FileType"` or `"Effect"` according to what type of plugin you are making. This is required to prevent post build event from failing. For this plugin, it's a FileType so set to `"FileType"`.

You can set this variables using either the `SETX` command or `Control Panel -> System -> Advanced System Properties -> Environment Variables`

With the `SETX` command, it's as simple as `SETX VARIABLENAME "VARIABLECONTENT"` where `VARIABLENAME` is the name of the variable to set and `VARIABLECONTENT` is the value of the variable to set.

With the `Control Panel` method, you just click `New` then fill in `Variable name:` and `Variable value:` and press OK.

After adding the environment variables, restart Visual Studio if you had it open when doing this. If the variables still do not resolve, force close `explorer.exe` and restart it. If still not working, restart your PC.

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