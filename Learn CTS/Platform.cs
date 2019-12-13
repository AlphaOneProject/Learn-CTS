using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Platform : Texture
    {

        private List<Texture> list_terminals;

        /// <summary>
        /// Constructor of platform.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">THe depth position compared to the other textures.</param>
        public Platform(int x, int y, int z) : base("platform", "platform", x, y, z)
        {
            list_terminals = new List<Texture>();
            Texture t;
            for (int i = 0; i <= 4; i++)
            {
                t = new Texture("terminal", "platform", x + this.GetWidth()/6 + i* this.GetWidth() / 6, y - 8, true);
                list_terminals.Add(t);
                this.AddChild(t);
            }
        }

        /// <summary>
        /// Optimize the paint environment and paint the platform
        /// </summary>
        /// <param name="e"></param>
        public override void OnPaint(PaintEventArgs e)
        {
            CompositingMode c = e.Graphics.CompositingMode;
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            base.OnPaint(e);
            e.Graphics.CompositingMode = c;
        }

        /// <summary>
        /// Check if a terminal has been hit and the player is close enough to it.
        /// </summary>
        /// <param name="x">The x coordinate of the mouse click.</param>
        /// <param name="y">The y coordinate of the mouse click.</param>
        /// <returns>True if a terminal has been hit and the player is close enough to it, false otherwise. </returns>
        public bool IsTerminalHit(int x, int y)
        {
            bool res = false;
            Texture p = Player.GetInstance();
            foreach (Texture t in list_terminals)
            {
                if (t.IsHitboxHit(x, y) && Math.Abs((t.GetX()+t.GetWidth()/2) - (p.GetX()+p.GetWidth()/2)) < 100 && Math.Abs(t.GetZ() - p.GetZ()) < 100) res = true;
            }
            return res;
        }
    }
}
