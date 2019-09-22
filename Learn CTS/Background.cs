using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Background : Texture
    {

        public Background(int x) : base(x, 0)
        {
        }

        public override void updateGraphic(Graphics g, PaintEventArgs e)
        {
            Image imageBackground = Image.FromFile(this.getPath());
            if(this.getX()< -e.ClipRectangle.Width)
            {
                this.setX(this.getX() + e.ClipRectangle.Width);
            }
            g.DrawImage(imageBackground, new Point(this.getX(), 0));
            g.DrawImage(imageBackground, new Point(imageBackground.Width+this.getX(), 0));
            imageBackground.Dispose();
        }
    }
}
