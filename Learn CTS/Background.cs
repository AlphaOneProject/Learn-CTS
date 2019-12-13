using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Background : Texture
    {

        // Attributes

        //Duplication of the background, used for the collisions.
        private Texture double_b;

        /// <summary>
        /// Constructor of background
        /// </summary>
        /// <param name="name">Name of the background.</param>
        /// <param name="x">The position of the background on the x axis.</param>
        /// <param name="y">The position of the background on the y axis.</param>
        public Background(string name, int x, int y) : base(name, "background", x, y, -5000)
        {
            this.double_b = new Texture(name, "background", this.GetX() + this.GetWidth(), y, -5000);
        }

        /// <summary>
        /// Overriding the CollideWith method to take in account the duplicate background.
        /// </summary>
        /// <param name="t">The texture tested</param>
        /// <param name="b">Does it affect the background childs.</param>
        /// <returns>True if the texture collides with the hitbox of the background, false otherwise.</returns>
        public override bool CollideWith(Texture t, bool b)
        {
            this.double_b.SetX(this.GetX() + this.GetWidth());
            return this.double_b.CollideWith(t, b) || base.CollideWith(t, b);
        }

        /// <summary>
        /// Paint the background on the screen and repeat it.
        /// </summary>
        /// <param name="e">The paint environment.</param>
        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            CompositingMode c = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceCopy;
            if (this.GetX() < -this.GetWidth())
            {
                this.SetX(this.GetX() + this.GetWidth());
            }
            if (this.GetX() >= 0)
            {
                this.SetX(-this.GetWidth());
            }
            g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
            g.DrawImage(this.GetImage(), new Point(this.GetX() + this.GetWidth(), this.GetY()));
            g.CompositingMode = c;
        }
    }
}