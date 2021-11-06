namespace TXTRFileType.GX
{
    public enum PaletteFormat : uint
    {
        /// <summary>
        /// 8-bit greyscale intensity values with an additional 8-bit alpha channel.
        /// </summary>
        IA8 = 0x0U,
        /// <summary>
        /// 16-bit colors without alpha.
        /// </summary>
        RGB565 = 0x1U,
        /// <summary>
        /// 16-bit colors with alpha.
        /// </summary>
        RGB5A3 = 0x2U
    }
}
