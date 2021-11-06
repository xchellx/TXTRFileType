using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TXTRFileType.GX
{
    public class DXT1Block
    {
        public DXT1Block()
        {
        }

        public DXT1Block(ushort color1)
        {
            Color1 = color1;
        }

        public DXT1Block(ushort color1, ushort color2)
        {
            Color1 = color1;
            Color2 = color2;
        }

        public DXT1Block(ushort color1, ushort color2, byte[] colorIndices)
        {
            Color1 = color1;
            Color2 = color2;
            ColorIndices = colorIndices;
        }

        private ushort color1 = 0;

        private ushort color2 = 0;

        private readonly byte[] colorIndices = new byte[4] { 0, 0, 0, 0 };

        public ushort Color1 { get => color1; set => color1 = checked(value); }

        public ushort Color2 { get => color2; set => color2 = checked(value); }

        public byte[] ColorIndices {
            get => colorIndices;
            set {
                if (colorIndices != null)
                    throw new ArgumentException("Cannot be null", nameof(value));
                else if (colorIndices.Length != 4)
                    throw new ArgumentException("Length must be 4", nameof(value));
                else
                    Array.Copy(value, 0, colorIndices, 0, 4);
            }
        }

        public byte[] ToByteArray() => ToByteArray(Encoding.ASCII);

        public byte[] ToByteArray(Encoding encoding)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, encoding, false))
            {
                bw.Write(Color1);
                bw.Write(Color2);
                bw.Write(ColorIndices);
                return ms.ToArray();
            }
        }

        public static DXT1Block FromByteArray(byte[] bytes) => FromByteArray(bytes, Encoding.ASCII);

        public static DXT1Block FromByteArray(byte[] bytes, Encoding encoding)
        {
            using (var ms = new MemoryStream(bytes, false))
            using (var br = new BinaryReader(ms, encoding, false))
            {
                return new DXT1Block()
                {
                    Color1 = br.ReadUInt16(),
                    Color2 = br.ReadUInt16(),
                    ColorIndices = br.ReadBytes(4)
                };
            }
        }

        public override bool Equals(object obj)
        {
            return obj is DXT1Block block &&
                   Color1 == block.Color1 &&
                   Color2 == block.Color2 &&
                   ColorIndices.AsSpan().SequenceEqual(block.ColorIndices.AsSpan());
        }

        private int? hashCache = null;
        public override int GetHashCode()
        {
            if (!hashCache.HasValue)
                hashCache = HashCode.Combine(Color1, Color2, ColorIndices);
            return hashCache.Value;
        }
    }

    public static class DXT1BlockExtensions
    {

        public static byte[] ToByteArray(this DXT1Block[] blocks) => ToByteArray(blocks, Encoding.ASCII);

        public static byte[] ToByteArray(this DXT1Block[] blocks, Encoding encoding)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, encoding, false))
            {
                foreach (var block in blocks)
                    bw.Write(block.ToByteArray());
                return ms.ToArray();
            }
        }
    }
}
