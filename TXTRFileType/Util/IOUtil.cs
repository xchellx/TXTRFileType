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
using System.IO;

namespace TXTRFileType.Util
{
    /// <summary>
    /// IO utility class
    /// </summary>
    public class IOUtil
    {
        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
            /// <summary>Short alias to <see cref="Environment.NewLine"/></summary>
            public static readonly string NewLine = Environment.NewLine;
            /// <summary>Short alias to <see cref="Environment.DirectorySeparatorChar"/></summary>
            public static readonly char DirSepChar = Path.DirectorySeparatorChar;
        }
    }
}
