using System.Drawing;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Background : Texture
    {

        /// <summary>
        /// Constructor of background.
        /// </summary>
        /// <param name="x">The x coordinate.</param>

        public Background(int x) : base("Background",x, -482, -5000)
        {
        }

        /// <summary>
        /// Paint the background on the screen and repeat it.
        /// </summary>
        /// <param name="e"></param>

        public override void UpdateGraphic(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(this.GetX()< -this.GetWidth())
            {
                this.SetX(this.GetX() + this.GetWidth());
            }
            if (this.GetX() > 0)
            {
                this.SetX(-this.GetWidth());
            }
            g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
            g.DrawImage(this.GetImage(), new Point(this.GetWidth()+this.GetX(), this.GetY()));
        }
    }
}