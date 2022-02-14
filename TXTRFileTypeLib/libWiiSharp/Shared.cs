/* This file is part of libWiiSharp
 * Copyright (C) 2009 Leathl
 * 
 * libWiiSharp is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * libWiiSharp is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Net;

namespace libWiiSharp
{
    internal static class Shared
    {
        // Merges two string arrays into one without double entries.
        public static string[] MergeStringArrays(string[] a, string[] b)
        {
            List<string> sList = new(a);

            foreach (string currentString in b)
                if (!sList.Contains(currentString)) sList.Add(currentString);

            sList.Sort();
            return sList.ToArray();
        }

        // Compares two byte arrays.
        public static bool CompareByteArrays(byte[] first, int firstIndex, byte[] second, int secondIndex, int length)
        {
            if (first.Length < length || second.Length < length) return false;

            for (int i = 0; i < length; i++)
                if (first[firstIndex + i] != second[secondIndex + i]) return false;

            return true;
        }

        // Compares two byte arrays.
        public static bool CompareByteArrays(byte[] first, byte[] second)
        {
            if (first.Length != second.Length) return false;
            else
                for (int i = 0; i < first.Length; i++)
                    if (first[i] != second[i]) return false;

            return true;
        }

        // Turns a byte array into a string, default separator is a space.
        public static string ByteArrayToString(byte[] byteArray, char separator = ' ')
        {
            string res = string.Empty;

            foreach (byte b in byteArray)
                res += b.ToString("x2").ToUpper() + separator;

            return res.Remove(res.Length - 1);
        }

        // Turns a hex string into a byte array.
        public static byte[] HexStringToByteArray(string hexString)
        {
            byte[] ba = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length / 2; i++)
                ba[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);

            return ba;
        }

        // Counts how often the given char exists in the given string.
        public static int CountCharsInString(string theString, char theChar)
        {
            int count = 0;

            foreach (char thisChar in theString)
                if (thisChar == theChar)
                    count++;

            return count;
        }

        // Pads the given value to a multiple of the given padding value, default padding value is 64.
        public static long AddPadding(long value)
        {
            return AddPadding(value, 64);
        }

        // Pads the given value to a multiple of the given padding value, default padding value is 64.
        public static long AddPadding(long value, int padding)
        {
            if (value % padding != 0)
            {
                value += (padding - (value % padding));
            }

            return value;
        }

        // Pads the given value to a multiple of the given padding value, default padding value is 64.
        public static int AddPadding(int value)
        {
            return AddPadding(value, 64);
        }

        // Pads the given value to a multiple of the given padding value, default padding value is 64.
        public static int AddPadding(int value, int padding)
        {
            if (value % padding != 0)
            {
                value += (padding - (value % padding));
            }

            return value;
        }

        // Swaps endianness.
        public static ushort Swap(ushort value)
        {
            return (ushort)IPAddress.HostToNetworkOrder((short)value);
        }

        // Swaps endianness.
        public static uint Swap(uint value)
        {
            return (uint)IPAddress.HostToNetworkOrder((int)value);
        }

        // Swaps endianness
        public static ulong Swap(ulong value)
        {
            return (ulong)IPAddress.HostToNetworkOrder((long)value);
        }

        // Turns a ushort array into a byte array.
        public static byte[] UShortArrayToByteArray(ushort[] array)
        {
            List<byte> results = new();
            foreach (ushort value in array)
            {
                byte[] converted = BitConverter.GetBytes(value);
                results.AddRange(converted);
            }
            return results.ToArray();
        }

        // Turns a uint array into a byte array.
        public static byte[] UIntArrayToByteArray(uint[] array)
        {
            List<byte> results = new();
            foreach (uint value in array)
            {
                byte[] converted = BitConverter.GetBytes(value);
                results.AddRange(converted);
            }
            return results.ToArray();
        }

        // Turns a byte array into a uint array.
        public static uint[] ByteArrayToUIntArray(byte[] array)
        {
            UInt32[] converted = new UInt32[array.Length / 4];
            int j = 0;

            for (int i = 0; i < array.Length; i += 4)
                converted[j++] = BitConverter.ToUInt32(array, i);

            return converted;
        }

        // Turns a byte array into a ushort array.
        public static ushort[] ByteArrayToUShortArray(byte[] array)
        {
            ushort[] converted = new ushort[array.Length / 2];
            int j = 0;

            for (int i = 0; i < array.Length; i += 2)
                converted[j++] = BitConverter.ToUInt16(array, i);

            return converted;
        }
    }
}
