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
        private string path_image;
        private string path_hitbox;
        private bool locked;
        private Image image;
        private Bitmap hitbox;
        private int? z;

        public Texture(int x, int y, bool b)
        {
            this.x = x;
            this.y = y;
            this.locked = b;
            this.path_image = projectDir + "\\resources\\textures\\" + this.GetType().Name + ".png";
            this.path_hitbox = projectDir + "\\resources\\textures\\" + this.GetType().Name + "HitBox.png";
            try
            {
                SetImage(this.path_image);
                SetHitbox(this.path_hitbox);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void SetImage(String path)
        {
            try
            {
                this.image = Image.FromFile(path);
            }
            catch (IOException)
            {
                MessageBox.Show("L'image de la texture n'a pas été trouvée, ou elle est inaccessible.\n" +
                                "Veuillez qu'elle soit nommée ainsi : " + this.GetType().Name);
            }

        }

        public void SetHitbox(String path)
        {
            try
            {
                this.hitbox = new Bitmap(Image.FromFile(path));
            }
            catch (IOException)
            {
                MessageBox.Show("L'image de la hitbox n'a pas été trouvée, ou elle est inaccessible.\n" +
                                "Veuillez qu'elle soit nommée ainsi : " + this.GetType().Name + "HitBox");
            }
        }

        public Texture(int x, int y, int z, bool b) : this(x,y,b)
        {
            this.z = z;
        }

        public virtual new void Move(int a, int b)
        {
            this.x += a;
            this.y += b;
        }

        public virtual void UpdateGraphic(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(this.image, new Point(x, y));
        }

        public Image GetImage()
        {
            return this.image;
        }

        public string GetPathImage()
        {
            return this.path_image;
        }

        public string GetPathHitBox()
        {
            return this.path_hitbox;
        }

        public virtual bool IsHitboxHit(int c, int d)
        {
            if (c - this.x >= 0 && c - this.x < image.Width && d - this.y >= 0 && d - this.y < image.Height && this.hitbox.Size == this.GetImage().Size)
            {
                Color color = this.hitbox.GetPixel(c - this.x, d - this.y);
                return !Color.Equals(color, Color.FromArgb(0, 0, 0, 0));
            }
            else
            {
                return false;
            }
        }

        public bool IsLocked()
        {
            return this.locked;
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public void SetX(int x)
        {
            this.x = x;
        }

        public void SetY(int y)
        {
            this.y = y;
        }

        public Bitmap GetHitbox()
        {
            return this.hitbox;
        }

        public int GetZ()
        {
            if (this.z == null)
            {
                return this.GetY() + this.GetImage().Height;
            }
            else
            {
                return (int)this.z;
            }
        }

        public void Debug(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(this.GetX(), this.GetY(), this.GetImage().Width, this.GetImage().Height);
            Pen pen = new Pen(Brushes.Red);
            pen.Width = 4;
            g.DrawRectangle(pen, rect);
            g.DrawImage(this.hitbox, new Point(x, y));
            pen.Dispose();
        }

        public bool CollideWith(Texture t)
        {
            if((this.GetType().GetType().Name == "Character" && this.GetType().GetType().Name == "Character") || Texture.ReferenceEquals(this, t))
            {
                return false;
            }
            bool b = false;
            Rectangle r1 = new Rectangle(this.GetX(), this.GetY(), this.GetImage().Width, this.GetImage().Height);
            Rectangle r2 = new Rectangle(t.GetX(), t.GetY(), t.GetImage().Width, t.GetImage().Height);
            if (r1.IntersectsWith(r2)){
                Rectangle r3 = Rectangle.Intersect(r1, r2);
                for(int i = 0; i<r3.Width; i++)
                {
                    for(int j = 0; j<r3.Height; j++)
                    {
                        if(this.IsHitboxHit(r3.X+i,r3.Y+j) && t.IsHitboxHit(r3.X+i, r3.Y+j))
                        {
                            b = true;
                        }
                    }
                }
            }
            return b;
        }

        public override string ToString()
        {
            return this.GetType().Name + ": x=" + this.GetX() + ": y=" + this.GetY() + ": largeur="+this.image.Width + ": hauteur="+this.image.Height;
        }

        public static int Compare(Texture t1, Texture t2)
        {
            if(t1.GetZ() >= t2.GetZ())
            {
                return 1;
            }
            else if(t1.GetZ() < t2.GetZ())
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}