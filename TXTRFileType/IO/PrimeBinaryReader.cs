using System;
using System.IO;
using System.Text;

// TODO: Maybe its wrong to assume its always gonna be ASCII... perhaps MP3 has Unicode/UTF-8?
namespace TXTRFileType.IO
{
    public class PrimeBinaryReader : EndianBinaryReader
    {
        public AssetIDLength assetIdLength = AssetIDLength.LENGTH_UINT;

        public PrimeBinaryReader(Stream input) : base(input, false, Encoding.ASCII, false)
        {
            assetIdLength = AssetIDLength.LENGTH_UINT;
        }

        public PrimeBinaryReader(Stream input, bool leaveOpen) : base(input, false, Encoding.ASCII, leaveOpen)
        {
            assetIdLength = AssetIDLength.LENGTH_UINT;
        }

        public PrimeBinaryReader(Stream input, AssetIDLength assetLen) : base(input, false, Encoding.ASCII, false)
        {
            assetIdLength = assetLen;
        }

        public PrimeBinaryReader(Stream input, AssetIDLength assetLen, bool leaveOpen) : base(input, false, Encoding.ASCII, leaveOpen)
        {
            assetIdLength = assetLen;
        }

        /// <summary>
        /// Read a null terminated string<br/>
        /// WARNING: This will read all the way to EOF if there is no null terminator
        /// </summary>
        /// <returns>The string without its null terminator</returns>
        public string ReadNullTerminatedString() => ReadCString(int.MaxValue, true);

        public string ReadASCIIString(int length) => ReadCString(length, false);

        private string ReadCString(int length, bool nullTerminated)
        {
            if (length > 0)
            {
                StringBuilder str = new StringBuilder();
                char c;
                for (int _ = 0; _ < length; _++)
                {
                    c = ReadChar();
                    if (nullTerminated && (c == 0x00))
                        break;
                    else
                        str.Append(c);
                }
                return str.ToString();
            }
            else
                return "";
        }

        public string ReadAssetID() => ReadAssetID(assetIdLength);

        public string ReadAssetID(AssetIDLength assetIdLength) => ReadASCIIString((int)assetIdLength);

        public string ReadFourCC() => ReadASCIIString(4);

        public void Align32() => Align(31);

        public void Align(int n)
        {
            BaseStream.Seek((BaseStream.Position + n) & ~n, SeekOrigin.Begin);
        }

        public enum AssetIDLength : int
        {
            /// <summary>Used by Metroid Prime and Metroid Prime 2: Echoes</summary>
            LENGTH_UINT = sizeof(uint),
            /// <summary>Used by Metroid Prime 3: Corruption</summary>
            LENGTH_ULONG = sizeof(ulong)
        }
    }
}
