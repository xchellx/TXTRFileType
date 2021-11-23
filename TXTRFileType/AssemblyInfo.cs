/*
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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRFileType
{
    internal static class AssemblyInfo
    {
        public const string AssemblyTitle = "TXTR FileType Plugin for Paint.NET";
        public const string AssemblyDescription = "Open TXTR files from the Metroid Prime series of games.";
#if DEBUG
        public const string AssemblyConfiguration = "Debug";
#else
        public const string AssemblyConfiguration = "Release";
#endif
        public const string AssemblyCompany = "Yonder";
        public const string AssemblyProduct = "TXTRFileType";
        public const string AssemblyCopyright = "Copyright © " + AssemblyCompany + " 2021";
        public const string AssemblyTrademark = "TXTRFileType";
        public const string AssemblyCulture = ""; // Do not modify!
        public const bool ComVisible = false;
        public const string Guid = "7afbafc5-3d1b-46f0-85ab-31a62a1b2d95";
        public const string AssemblyVersion = "1.0"; // major.minor
        public const string AssemblyFileVersion = "1.0.1.0"; // major.minor.build.revision
        public const string AssemblyInformationalVersion = "1.0.1"; // major.minor[.build]
        public const string SupportedOSPlatform = "windows"; // NET 5.0+ only!
        public const string WebsiteUri = "https://www.getpaint.net/redirect/plugins.html"; // Paint.NET only!
    }
}
