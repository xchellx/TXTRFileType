using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXTRFileType.Extensions
{
    public static class ToBinaryStringExtensions
    {
        /// <summary>
        /// Get a binary string representation of a <see cref="sbyte"/>
        /// </summary>
        /// <param name="input">The input <see cref="sbyte"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this sbyte input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(sbyte) * 8, '0');
        }

        /// <summary>
        /// Get a binary string representation of a <see cref="byte"/>
        /// </summary>
        /// <param name="input">The input <see cref="byte"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this byte input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(byte) * 8, '0');
        }

        /// <summary>
        /// Get a binary string representation of a <see cref="short"/>
        /// </summary>
        /// <param name="input">The input <see cref="short"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this short input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(short) * 8, '0');
        }


        /// <summary>
        /// Get a binary string representation of a <see cref="ushort"/>
        /// </summary>
        /// <param name="input">The input <see cref="ushort"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this ushort input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(ushort) * 8, '0');
        }


        /// <summary>
        /// Get a binary string representation of a <see cref="int"/>
        /// </summary>
        /// <param name="input">The input <see cref="int"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this int input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(int) * 8, '0');
        }

        /// <summary>
        /// Get a binary string representation of a <see cref="uint"/>
        /// </summary>
        /// <param name="input">The input <see cref="uint"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this uint input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(uint) * 8, '0');
        }

        /// <summary>
        /// Get a binary string representation of a <see cref="ulong"/>
        /// </summary>
        /// <param name="input">The input <see cref="ulong"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this ulong input)
        {
            uint low = (uint)(input & 0xFFFFFFFF);
            uint high = (uint)(input & 0xFFFFFFFF00000000) >> (sizeof(uint) * 8);
            return $"{Convert.ToString(high, 2).PadLeft(sizeof(uint) * 8, '0')}{Convert.ToString(low, 2).PadLeft(sizeof(uint) * 8, '0')}";
        }

        /// <summary>
        /// Get a binary string representation of a <see cref="long"/>
        /// </summary>
        /// <param name="input">The input <see cref="long"/></param>
        /// <returns>The binary string representation of <paramref name="input"/></returns>
        public static string ToBinaryString(this long input)
        {
            return Convert.ToString(input, 2).PadLeft(sizeof(long) * 8, '0');
        }
    }
}
