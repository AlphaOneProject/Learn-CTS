using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Platform : Texture
    {

        Texture terminals;

        /// <summary>
        /// Constructor of platform.
        /// </summary>
        /// <param name="x">The x coordinate.</param>

        public Platform(int x, int y, int z) : base("Platform", x, y, z)
        {
            terminals = new Texture("Terminals", x+840, y+32, true);
            this.AddChild(terminals);
        }

        /// <summary>
        /// Paint the platform
        /// </summary>
        /// <param name="e"></param>

        public override void OnPaint(PaintEventArgs e)
        {
            CompositingMode c = e.Graphics.CompositingMode;
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            base.OnPaint(e);
            e.Graphics.CompositingMode = c;
        }

        public bool IsTerminalHit(int c, int d)
        {
            if (terminals.IsHitboxHit(c, d)) return true;
            else return false;
        }
    }
}
