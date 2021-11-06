using System;

namespace TXTRFileType.GX
{
    public static class Texture
    {
        internal static (ushort blockWidth, ushort blockHeight) GetBlockDimensions(TextureFormat textureFormat)
        {
            switch (textureFormat)
            {
                case TextureFormat.I4:
                    return (blockWidth: 8, blockHeight: 8);
                case TextureFormat.I8:
                    return (blockWidth: 8, blockHeight: 4);
                case TextureFormat.IA4:
                    return (blockWidth: 8, blockHeight: 4);
                case TextureFormat.IA8:
                    return (blockWidth: 4, blockHeight: 4);
                case TextureFormat.CI4:
                    return (blockWidth: 8, blockHeight: 8);
                case TextureFormat.CI8:
                    return (blockWidth: 8, blockHeight: 4);
                case TextureFormat.CI14X2:
                    return (blockWidth: 4, blockHeight: 4);
                case TextureFormat.RGB565:
                    return (blockWidth: 4, blockHeight: 4);
                case TextureFormat.RGB5A3:
                    return (blockWidth: 4, blockHeight: 4);
                case TextureFormat.RGBA8:
                    return (blockWidth: 4, blockHeight: 4);
                case TextureFormat.CMPR:
                    return (blockWidth: 8, blockHeight: 8);
                default:
                    throw new NotSupportedException($"Texture format '{textureFormat}' ({textureFormat:X}) is not supported");
            }
        }

        internal static (ushort blockWidthSize, ushort blockHeightSize) GetBlockSize(TextureFormat textureFormat,
            ushort textureWidth, ushort textureHeight, ushort blockWidth, ushort blockHeight)
        {
            return (blockWidthSize: (ushort)((textureWidth + (blockWidth - 1)) / blockWidth),
                blockHeightSize: (ushort)((textureHeight + (blockHeight - 1)) / blockHeight));
        }

        /* GX uses this upsampling technique to extract full 8-bit range */
        internal static byte Convert3To8(byte v)
        {
            /* Swizzle bits: 00000123 -> 12312312 */
            return (byte)((uint)((v << 5) | (v << 2)) | ((uint)v >> 1));
        }

        internal static byte Convert4To8(byte v)
        {
            /* Swizzle bits: 00001234 -> 12341234 */
            return (byte)((v << 4) | v);
        }

        internal static byte Convert5To8(byte v)
        {
            /* Swizzle bits: 00012345 -> 12345123 */
            return (byte)((uint)(v << 3) | ((uint)v >> 2));
        }

        internal static byte Convert6To8(byte v)
        {
            /* Swizzle bits: 00123456 -> 12345612 */
            return (byte)((uint)(v << 2) | ((uint)v >> 4));
        }
    }
}
