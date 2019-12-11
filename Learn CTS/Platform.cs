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

        public Platform(int x, int y, int z) : base("Platform", "platform", x, y, z)
        {
            list_terminals = new List<Texture>();
            Texture t;
            for (int i = 0; i <= 4; i++)
            {
                t = new Texture("Terminal", "platform", x + this.GetWidth()/6 + i* this.GetWidth() / 6, y - 8, true);
                list_terminals.Add(t);
                this.AddChild(t);
            }
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
            bool res = false;
            Texture p = Player.GetInstance();
            foreach (Texture t in list_terminals)
            {
                if (t.IsHitboxHit(c, d) && Math.Abs((t.GetX()+t.GetWidth()/2) - (p.GetX()+p.GetWidth()/2)) < 100 && Math.Abs(t.GetZ() - p.GetZ()) < 100) res = true;
            }
            return res;
        }
    }
}
