
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Tram : Transport
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

        public Tram(int x, int y) : base("Tram", x, y, pos_doors)
        {
            this.AddChild(new Texture("TramInterior", this.GetX() + 480, this.GetY() + 208, true));
        }

        public override SoundPlayer GetCurrentAudio()
        {
            new SoundPlayer(@"c:\Windows\Media\chimes.wav");

            return base.GetCurrentAudio();
        }
    }
}