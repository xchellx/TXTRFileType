namespace TXTRFileType.GX
{
    public enum TextureFormat : uint
    {
        /// <summary>
        /// 4-bit greyscale intensity values. Two pixels per byte.
        /// </summary>
        I4      = 0x0U,
        /// <summary>
        /// 8-bit greyscale intensity values.
        /// </summary>
        I8      = 0x1U,
        /// <summary>
        /// 4-bit greyscale intensity values with an additional 4-bit alpha channel.
        /// </summary>
        IA4     = 0x2U,
        /// <summary>
        /// 8-bit greyscale intensity values with an additional 8-bit alpha channel.
        /// </summary>
        IA8     = 0x3U,
        /// <summary>
        /// 4-bit palette indices.
        /// </summary>
        CI4     = 0x4U,
        /// <summary>
        /// 8-bit palette indices.
        /// </summary>
        CI8     = 0x5U,
        /// <summary>
        /// 14-bit palette indices (14b index).
        /// </summary>
        CI14X2  = 0x6U,
        /// <summary>
        /// 16-bit colors without alpha.
        /// </summary>
        RGB565  = 0x7U,
        /// <summary>
        /// 16-bit colors with alpha.
        /// </summary>
        RGB5A3  = 0x8U,
        /// <summary>
        /// Uncompressed 32-bit colors with alpha.
        /// </summary>
        RGBA8   = 0x9U,
        /// <summary>
        /// Compressed textures (almost the same as DXT1, but with a couple small differences)
        /// </summary>
        CMPR    = 0xAU
    }
}
