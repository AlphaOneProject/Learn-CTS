
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Tram : Vehicule
    {

        // Attributes

        private static int[] pos_doors = new int[]{
            510,
            920,
            1528,
            1944,
            2552,
            2928
        };

        /// <summary>
        /// Constructor of a tram.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Tram(int x, int y) : base("tram", x, y, pos_doors)
        {
        }
    }
}