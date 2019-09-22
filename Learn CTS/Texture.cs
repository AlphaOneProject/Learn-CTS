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
    abstract class Texture : PictureBox
    {

        private static string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        private int x;
        private int y;
        private string path;

        public Texture(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.path = projectDir + "\\ressources\\img\\"+this.GetType().Name+"Test.png";
        }

        public virtual void move(int a, int b)
        {
            this.x += a;
            this.y += b;
        }

        public virtual void updateGraphic(Graphics g, PaintEventArgs e)
        {
            Image image = Image.FromFile(this.path);
            g.DrawImage(image, new Point(x, y));
            image.Dispose();
        }

        public string getPath()
        {
            return this.path;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public void setX(int x)
        {
            this.x = x;
        }

        public void setY(int y)
        {
            this.y = y;
        }
    }
}
