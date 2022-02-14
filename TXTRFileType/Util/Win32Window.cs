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
using System.Diagnostics;
using System.Windows.Forms;

namespace TXTRFileType.Util
{
    internal sealed class Win32Window : IWin32Window
    {
        public Win32Window(Control control)
        {
            Handle = control.Handle;
        }

        public Win32Window(NativeWindow window)
        {
            Handle = window.Handle;
        }

        public Win32Window(Process process)
        {
            Handle = process.MainWindowHandle;
        }

        public Win32Window(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; }
    }
}
