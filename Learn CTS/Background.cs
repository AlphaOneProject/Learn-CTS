using System.Drawing;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Background : Texture
    {

        public Background(int x) : base(x, 0, -5000, true)
        {
        }

        public override void UpdateGraphic(PaintEventArgs e)
        {
            this.Update();
            Graphics g = e.Graphics;
            if(this.GetX()< -this.GetImage().Width)
            {
                this.SetX(this.GetX() + this.GetImage().Width);
            }
            g.DrawImage(this.GetImage(), new Point(this.GetX(), 0));
            g.DrawImage(this.GetImage(), new Point(this.GetImage().Width+this.GetX(), 0));
        }
    }
}