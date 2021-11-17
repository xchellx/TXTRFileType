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
