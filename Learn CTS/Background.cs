using System.Drawing;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Background : Texture
    {

        public Background(int x) : base(x, 0, true)
        {
        }

        public override void updateGraphic(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(this.getX()< -e.ClipRectangle.Width)
            {
                this.setX(this.getX() + e.ClipRectangle.Width);
            }
            g.DrawImage(this.getImage(), new Point(this.getX(), 0));
            g.DrawImage(this.getImage(), new Point(this.getImage().Width+this.getX(), 0));
        }
    }
}
