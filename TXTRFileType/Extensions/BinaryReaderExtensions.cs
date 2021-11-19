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

namespace TXTRFileType.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static byte[] ReadAllBytes(this BinaryReader @this)
        {
            const int bufferSize = 4096;
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = @this.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static short[] ReadInt16(this BinaryReader @this, int count)
        {
            short[] output = new short[count];
            for (int i = 0; i < count; i++)
                output[i] = @this.ReadInt16();
            return output;
        }

        public static ushort[] ReadUInt16(this BinaryReader @this, int count)
        {
            ushort[] output = new ushort[count];
            for (int i = 0; i < count; i++)
                output[i] = @this.ReadUInt16();
            return output;
        }

        public static int[] ReadInt32(this BinaryReader @this, int count)
        {
            int[] output = new int[count];
            for (int i = 0; i < count; i++)
                output[i] = @this.ReadInt32();
            return output;
        }

        public static uint[] ReadUInt32(this BinaryReader @this, int count)
        {
            uint[] output = new uint[count];
            for (int i = 0; i < count; i++)
                output[i] = @this.ReadUInt32();
            return output;
        }

        public static long[] ReadInt64(this BinaryReader @this, int count)
        {
            long[] output = new long[count];
            for (int i = 0; i < count; i++)
                output[i] = @this.ReadInt64();
            return output;
        }

        public static ulong[] ReadUInt64(this BinaryReader @this, int count)
        {
            ulong[] output = new ulong[count];
            for (int i = 0; i < count; i++)
                output[i] = @this.ReadUInt64();
            return output;
        }
    }
}
