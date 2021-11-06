﻿using System;
using System.IO;
using System.Text;

namespace TXTRFileType.IO
{
    public class EndianBinaryWriter : BinaryWriter
    {
        public Encoding Encoding { get; }
        public bool IsLittleEndian = BitConverter.IsLittleEndian;

        public EndianBinaryWriter(Stream input) : base(input)
        {
            Encoding = Encoding.UTF8;
        }

        public EndianBinaryWriter(Stream input, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
        }

        public EndianBinaryWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
        }

        public EndianBinaryWriter(Stream input, bool isLittleEndian) : base(input)
        {
            Encoding = Encoding.UTF8;
            IsLittleEndian = isLittleEndian;
        }

        public EndianBinaryWriter(Stream input, bool isLittleEndian, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
            IsLittleEndian = isLittleEndian;
        }

        public EndianBinaryWriter(Stream input, bool isLittleEndian, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
            IsLittleEndian = isLittleEndian;
        }

        private void WriteForEndianness(byte[] buffer, bool isLittleEndian)
        {
            if (isLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            Write(buffer);
        }

        public override void Write(bool value) => Write(value, IsLittleEndian);

        public override void Write(ushort value) => Write(value, IsLittleEndian);

        public override void Write(short value) => Write(value, IsLittleEndian);

        public override void Write(uint value) => Write(value, IsLittleEndian);

        public override void Write(int value) => Write(value, IsLittleEndian);

        public override void Write(ulong value) => Write(value, IsLittleEndian);

        public override void Write(long value) => Write(value, IsLittleEndian);

        public override void Write(float value) => Write(value, IsLittleEndian);

        public override void Write(double value) => Write(value, IsLittleEndian);

        public void Write(bool value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(ushort value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(short value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(uint value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(int value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(ulong value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(long value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(float value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(double value, bool endianness) => WriteForEndianness(BitConverter.GetBytes(value), endianness);
    }
}
