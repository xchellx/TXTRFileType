using System;
using System.IO;
using System.Reflection;
using System.Text;
using TXTRFileType.Extensions;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Generic utility class
    /// </summary>
    public static class Shared
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

        /// <summary>
        /// Check if an enum is actually defined and named
        /// </summary>
        /// <param name="e">The enum to check</param>
        /// <returns><see langword="true"/> if the enum <paramref name="e"/> is defined and named</returns>
        public static bool IsFlagDefined(Enum e)
        {
            return !decimal.TryParse(e.ToString(), out _);
        }

        /// <summary>
        /// Check if the current assembly has AnyCPU's Prefers32Bit mode enabled
        /// </summary>
        /// <returns><see langword="true"/> if AnyCPU Prefers32Bit mode is enabled on the current assembly</returns>
        public static bool IsPrefers32Bit() => IsPrefers32Bit(typeof(Shared).Assembly);

        /// <summary>
        /// Check if an assembly has AnyCPU's Prefers32Bit mode enabled
        /// </summary>
        /// <param name="asm">The target assembly to check</param>
        /// <returns><see langword="true"/> if AnyCPU Prefers32Bit mode is enabled on the assembly <paramref name="asm"/></returns>
        public static bool IsPrefers32Bit(Assembly asm)
        {
            PortableExecutableKinds peKind;
            asm.ManifestModule.GetPEKind(out peKind, out _);
            return (peKind & PortableExecutableKinds.Preferred32Bit) == PortableExecutableKinds.Preferred32Bit;
        }

        public static class StaticMembers
        {
            // Convenience fields
            /// <summary>Short alias to <see cref="Environment.NewLine"/></summary>
            public static readonly string NewLine = Environment.NewLine;
            /// <summary>Short alias to <see cref="Environment.DirectorySeparatorChar"/></summary>
            public static readonly char DirSepChar = Path.DirectorySeparatorChar;
            /// <summary>Short alias to <see cref="Environment.Is64BitProcess"/></summary>
            public static readonly bool Is64Bit = Environment.Is64BitProcess;
            /// <summary>Short alias to <see cref="Environment.Is64BitOperatingSystem"/></summary>
            public static readonly bool Is64BitOS = Environment.Is64BitOperatingSystem;
        }
    }
}
