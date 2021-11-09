using System;
using System.Reflection;

namespace TXTRFileType.Util
{
    /// <summary>
    /// Assembly utility class
    /// </summary>
    public class AssemblyUtil
    {
        /// <summary>
        /// Check if the current assembly has AnyCPU's Prefers32Bit mode enabled
        /// </summary>
        /// <returns><see langword="true"/> if AnyCPU Prefers32Bit mode is enabled on the current assembly</returns>
        public static bool IsPrefers32Bit() => IsPrefers32Bit(typeof(AssemblyUtil).Assembly);

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

        /// <summary>Convenience fields</summary>
        public static class StaticMembers
        {
            /// <summary>Short alias to <see cref="Environment.Is64BitProcess"/></summary>
            public static readonly bool Is64Bit = Environment.Is64BitProcess;
            /// <summary>Short alias to <see cref="Environment.Is64BitOperatingSystem"/></summary>
            public static readonly bool Is64BitOS = Environment.Is64BitOperatingSystem;
        }
    }
}
