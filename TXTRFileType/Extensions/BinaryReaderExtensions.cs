using System;
using System.IO;

namespace TXTRFileType.Extensions
{
    public static class BinaryReaderExtensions
    {
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
