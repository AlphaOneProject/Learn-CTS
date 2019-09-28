using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Learn_CTS
{
    abstract class Texture : PictureBox
    {

        private static string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        private int x;
        private int y;
        private string path;
        private string pathhitbox;
        private bool locked;
        private Image img;
        private Bitmap hitbox;

        public Texture(int x, int y, bool b)
        {
            this.x = x;
            this.y = y;
            this.locked = b;
            this.path = projectDir + "\\ressources\\img\\"+this.GetType().Name+".png";
            this.pathhitbox = projectDir + "\\ressources\\img\\" + this.GetType().Name + "HitBox.png";
            this.img = Image.FromFile(this.path);
            this.hitbox = new Bitmap(Image.FromFile(this.pathhitbox));
        }

        public virtual void move(int a, int b)
        {
            this.x += a;
            this.y += b;
        }

        public virtual void updateGraphic(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(this.img, new Point(x, y));
        }

        public Image getImage()
        {
            return this.img;
        }

        public string getPath()
        {
            return this.path;
        }

        public bool isHitboxHit(int c, int d)
        {
            if(c - this.x > 0 && c - this.x <= img.Width && d - this.y > 0 && d - this.y <= img.Height)
            {
                Color color = this.hitbox.GetPixel(c - this.x, d - this.y);
                return !Color.Equals(color, Color.FromArgb(0,0,0,0));
            }
            else
            {
                return false;
            }
            
        }

        public bool isLocked()
        {
            return this.locked;
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
