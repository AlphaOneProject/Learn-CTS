using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Background : Texture
    {

        /// <summary>
        /// Constructor of background.
        /// </summary>
        /// <param name="x">The x coordinate.</param>

        public Background(int x, int y) : base("Background", "background", x, y, -5000)
        {
            this.DisableCollisions();
        }

        /// <summary>
        /// Paint the background on the screen and repeat it.
        /// </summary>
        /// <param name="e"></param>

        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            CompositingMode c = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceCopy;
            if (this.GetX()< -this.GetWidth())
            {
                this.SetX(this.GetX() + this.GetWidth());
            }
            if (this.GetX() > 0)
            {
                this.SetX(-this.GetWidth());
            }
            g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
            g.DrawImage(this.GetImage(), new Point(this.GetWidth()+this.GetX(), this.GetY()));
            g.CompositingMode = c;
            foreach (Texture t in this.GetListChilds())
            {
                if (t.GetX() < -t.GetWidth())
                {
                    t.SetX(this.GetX() + t.GetWidth());
                }
                if (t.GetX() > 0)
                {
                    t.SetX(-t.GetWidth());
                }
                g.DrawImage(t.GetImage(), new Point(t.GetX(), t.GetY()));
                g.DrawImage(t.GetImage(), new Point(t.GetWidth() + t.GetX(), t.GetY()));
            }
        }
    }
}