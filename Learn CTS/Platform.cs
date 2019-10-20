namespace Learn_CTS
{
    class Platform : Texture
    {

        /// <summary>
        /// Constructor of platform.
        /// </summary>
        /// <param name="x">The x coordinate.</param>

        public Platform(int x, int z) : base("Platform", x, 552 + 16, z)
        {
        }
    }
}
