using System;
using System.IO;
using System.Text;

namespace TXTRFileType.IO
{
    public class EndianBinaryReader : BinaryReader
    {
        public Encoding Encoding { get; }
        public bool IsLittleEndian = BitConverter.IsLittleEndian;

        public EndianBinaryReader(Stream input) : base(input)
        {
            Encoding = Encoding.UTF8;
        }

        public EndianBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
        }

        public EndianBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
        }

        public EndianBinaryReader(Stream input, bool isLittleEndian) : base(input)
        {
            Encoding = Encoding.UTF8;
            IsLittleEndian = isLittleEndian;
        }

        public EndianBinaryReader(Stream input, bool isLittleEndian, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
            IsLittleEndian = isLittleEndian;
        }

        public EndianBinaryReader(Stream input, bool isLittleEndian, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
            IsLittleEndian = isLittleEndian;
        }

        private byte[] ReadForEndianness(int bytesToRead, bool isLittleEndian)
        {
            byte[] bytesRead = ReadBytes(bytesToRead);
            if (isLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytesRead);
            return bytesRead;
        }

        public override byte[] ReadBytes(int count)
        {
            byte[] bytesRead = base.ReadBytes(count);
            if (bytesRead.Length != 0)
                return bytesRead;
            else
                throw new EndOfStreamException();
        }

        public override bool ReadBoolean() => ReadBoolean(IsLittleEndian);

        public override ushort ReadUInt16() => ReadUInt16(IsLittleEndian);

        public override short ReadInt16() => ReadInt16(IsLittleEndian);

        public override uint ReadUInt32() => ReadUInt32(IsLittleEndian);

        public override int ReadInt32() => ReadInt32(IsLittleEndian);

        public override ulong ReadUInt64() => ReadUInt64(IsLittleEndian);

        public override long ReadInt64() => ReadInt64(IsLittleEndian);

        public override float ReadSingle() => ReadSingle(IsLittleEndian);

        public override double ReadDouble() => ReadDouble(IsLittleEndian);

        public bool ReadBoolean(bool isLittleEndian) => BitConverter.ToBoolean(ReadForEndianness(sizeof(bool), isLittleEndian), 0);

        public ushort ReadUInt16(bool isLittleEndian) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort), isLittleEndian), 0);

        public short ReadInt16(bool isLittleEndian) => BitConverter.ToInt16(ReadForEndianness(sizeof(short), isLittleEndian), 0);

        public uint ReadUInt32(bool isLittleEndian) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint), isLittleEndian), 0);

        public int ReadInt32(bool isLittleEndian) => BitConverter.ToInt32(ReadForEndianness(sizeof(int), isLittleEndian), 0);

        public ulong ReadUInt64(bool isLittleEndian) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong), isLittleEndian), 0);

        public long ReadInt64(bool isLittleEndian) => BitConverter.ToInt64(ReadForEndianness(sizeof(long), isLittleEndian), 0);

        public float ReadSingle(bool isLittleEndian) => BitConverter.ToSingle(ReadForEndianness(sizeof(float), isLittleEndian), 0);

        public double ReadDouble(bool isLittleEndian) => BitConverter.ToDouble(ReadForEndianness(sizeof(double), isLittleEndian), 0);
    }
}
