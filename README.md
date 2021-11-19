# TXTRFileType
TXTR file type plugin for Paint.NET to load TXTR files from the Metroid Prime series.
With this you'll be able to extract, edit, and create TXTR texture files for or from the Metroid prime series. GX2 support is not included and not planned therefore TXTR files from the Donkey Kong Country Returns and Donkey Kong Country: Tropical Freeze games is not supported. This is open for contributions, however.

## Features
- Supports loading and saving all texture formats
- Supports loading and generating mipmaps
- Automatic image coordinate flipping

## Building
- Visual Studio 2019 or greater. [Get it here](https://visualstudio.microsoft.com/downloads/).
- .NET 5.0 SDK. [Get it here](https://dotnet.microsoft.com/download/visual-studio-sdks).
- Paint.NET (installed, store, or portable version). [Get it from the official website](https://www.getpaint.net) or [the Microsoft Store](https://www.microsoft.com/en-us/p/paintnet/9nbhcs1lx4r0).


### Environment Variables And Post Build Event
There is a post build event that copies the plugin's .dll and .deps.json to the plugin directory (as well as creating this directory). The dependencies specified in .deps.json (and for the dependencies of the dependencies of their selves) must be manually copied to the plugin directory.

The project and the post build event depends on some environment variables:

- Set the environment variable "PDNINSTALLDIR" to the path of your Paint.NET install directory. This is required for the PostBuild event and for the paint.net DLL references.
- Set the environment variable "PDNENABLEPBE" to 'true' to enable post build event and set to 'false' disable it.
- Set the environment variable "PDNPLUGINTYPE" to "FileType" or "Effect" according to what type of plugin you are making. This is required to prevent post build event from failing.

You can set this variables using either the `SETX` command or `Control Panel -> System -> Advanced System Properties -> Environment Variables`

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