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

namespace TXTRFileTypeCLI.Logging
{
    [Flags]
    internal enum ConsoleLoggerFlags : byte
    {
        NONE    = 0b0000_0000,
        CONSOLE = 0b0000_0001,
        DEBUG   = 0b0000_0010,
        QUIET   = 0b0000_0100,
        COLORS  = 0b0000_1000,
        NOTAG   = 0b0001_0000
    }
}
