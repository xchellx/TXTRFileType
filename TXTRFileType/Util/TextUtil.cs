using System;
using System.Text;
using TXTRFileType.Extensions;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Text utility class
    /// </summary>
    public class TextUtil
    {
        /// <summary>
        /// Convert a value type to it's equivalent binary string representationn (size accurate
        /// with optional prefix and seperator as how it would appear in C# code itself)
        /// </summary>
        /// <typeparam name="T">The type of the value to convert</typeparam>
        /// <param name="input">The value to convert</param>
        /// <param name="usePrefix">Prefix binary string with "0b"</param>
        /// <param name="useSeperator">Seperate with "_" every 4 bits</param>
        /// <returns></returns>
        public static string ToBinaryString<T>(T input, bool usePrefix = false, bool useSeperator = false)
            where T : struct
        {
            string bin = null;
            if (typeof(T) == typeof(sbyte))
                bin = ((sbyte)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(byte))
                bin = ((byte)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(short))
                bin = ((short)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(ushort))
                bin = ((ushort)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(int))
                bin = ((int)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(uint))
                bin = ((uint)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(ulong))
                bin = ((ulong)(object)input).ToBinaryString();
            else if (typeof(T) == typeof(long))
                bin = ((long)(object)input).ToBinaryString();

            if (bin != null)
            {
                StringBuilder bincv = new StringBuilder(usePrefix ? "0b" : "");
                for (int c = 0; c < bin.Length; c += 4)
                {
                    for (int i = 0; i < 4; i++)
                        bincv.Append(bin[c + i]);
                    if (useSeperator && c != (bin.Length - 4))
                        bincv.Append('_');
                }
                return bincv.ToString();
            }
            else
                throw new NotSupportedException("Only byte, sbyte, ushort, short, uint, int, ulong, and long are supported");
        }

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
        }
    }
}
