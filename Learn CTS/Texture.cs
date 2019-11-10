using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Learn_CTS
{
    /// <summary>
    /// Classe définissant une texture
    /// </summary>
    class Texture
    {

        // Attributes
        private static string projectDir;

        private int x;
        private int y;
        private int? z;
        private string name;
        private string path_image;
        private string path_hitbox;
        private Image image;
        private Bitmap hitbox;
        protected List<Texture> list_childs = new List<Texture>();
        private bool collide = true;
        private bool collide_only_z = false;
        private int width;
        private int height;

        /// <summary>
        /// Initialize the path of the folder of the images and hitboxes corresponding to the game.
        /// </summary>
        /// <param name="game">The name of the game.</param>

        public static void InitializePath(string game)
        {
            Texture.projectDir = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images";
        }

        /// <summary>
        /// Constructor of a texture which is placed at the specified coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        private Texture(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor of a texture which is placed at the specified coordinates x,y but specifies if it collides only on depth.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="b">Specifies if the texture collides only when the depth is the same as the other texture.</param>

        public Texture(int x, int y, bool b) : this(x,y)
        {
            this.collide_only_z = b;
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Texture(String name, int x, int y) : this(x,y)
        {
            this.name = name;
            this.path_image = GetCustomPathImage(name);
            this.path_hitbox = GetCustomPathHitbox(name);
            try
            {
                this.SetImage(CreateImage(this.path_image));
                this.SetHitbox(CreateHitbox(this.path_hitbox));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y and specifies if it collides only on depth.
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="b">Specifies if the texture collides only when the depth is the same as the other texture.</param>

        public Texture(String name, int x, int y, bool b) : this(name, x, y)
        {
            this.collide_only_z = b;
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y and a custom depth z.
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">THe depth position compared to the other textures.</param>

        public Texture(String name, int x, int y, int z) : this(name, x, y)
        {
            this.z = z;
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y, a custom depth z and specifies if it collides only on depth.
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">THe depth position compared to the other textures.</param>
        /// <param name="b">Specifies if the texture collides only when the depth is the same as the other texture.</param>

        public Texture(String name, int x, int y, int z, bool b) : this(name, x, y, b)
        {
            this.z = z;
        }


        /// <summary>
        /// Move the texture and all its childs, by adding a to the x coordinate and b to the y coordinate.
        /// </summary>
        /// <param name="a">Number that will be added to the x coordinate.</param>
        /// <param name="b">Number that will be added to the x coordinate.</param>

        public virtual void Move(int a, int b)
        {
            this.x += a;
            this.y += b;
            foreach (Texture t in this.list_childs)
            {
                t.Move(a, b);
            }
        }

        /// <summary>
        /// Paint itself to the window.
        /// </summary>
        /// <param name="e"></param>

        public virtual void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if ((this.GetX() + this.GetWidth() >= 0 && this.GetX() > e.ClipRectangle.Width) || (this.GetY() + this.GetHeight() >= 0 && this.GetY() < e.ClipRectangle.Height))
            {
                g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
                /*Rectangle r = new Rectangle(this.GetX(), this.GetY(), this.GetWidth(), this.GetHeight());
                this.Invalidate(r);*/
            }
        }

        /// <summary>
        /// Create a image from a file specified by the path.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>The image created from the file.</returns>

        public Image CreateImage(String path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("L'image de la texture n'a pas été trouvée, ou elle est inaccessible.\n" +
                                "Veuillez qu'elle soit bien ici : " + path);
            }
            return null;

        }

        /// <summary>
        /// Create a hitbox from a file specified by the path.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>Returns the bitmap of the hitbox.</returns>

        public Bitmap CreateHitbox(String path)
        {
            try
            {
                return new Bitmap(Image.FromFile(path));
            }
            catch (Exception)
            {
                MessageBox.Show("L'image de la hitbox n'a pas été trouvée, ou elle est inaccessible.\n" +
                                "Veuillez qu'elle soit bien ici : " + path);
            }
            return null;
        }

        /// <summary>
        /// Method that can determine if the hitbox of the texture is hit at these coordinates.
        /// </summary>
        /// <param name="c">The x coordinate that will be tested.</param>
        /// <param name="d">The y coordinate that will be tested.</param>
        /// <returns>True if the hitbox is hit, false otherwise.</returns>

        public bool IsHitboxHit(int c, int d)
        {
            if (c - this.x >= 0 && c - this.x < this.width && d - this.y >= 0 && d - this.y < this.height)
            {
                return !Color.Equals(this.hitbox.GetPixel(c - this.x, d - this.y), Color.FromArgb(0, 0, 0, 0));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the instance is colliding with the texture t or its childs
        /// </summary>
        /// <param name="t">The texture that will be tested.</param>
        /// <returns>true if this hit the hitbox of t or one of its childs, false otherwise.</returns>

        public virtual bool CollideWith(Texture t)
        {
            if (Texture.ReferenceEquals(this, t) || !this.collide || !t.CanCollide())
            {
                return false;
            }
            if (this.collide_only_z && t.CollidesOnlyOnZ() && Math.Abs(this.GetZ() - t.GetZ()) > 8)
            {
                return false;
            }
            Rectangle r1 = new Rectangle(this.GetX(), this.GetY(), this.width, this.height);
            Rectangle r2 = new Rectangle(t.GetX(), t.GetY(), t.GetImage().Width, t.GetImage().Height);
            if (r1.IntersectsWith(r2))
            {
                Rectangle r3 = Rectangle.Intersect(r1, r2);
                for (int i = 0; i < r3.Width; i += 8)
                {
                    for (int j = 0; j < r3.Height; j += 8)
                    {
                        if (this.IsHitboxHit(r3.X + i, r3.Y + j) && t.IsHitboxHit(r3.X + i, r3.Y + j))
                        {
                            return true;
                        }
                    }
                }
                foreach (Texture c in this.list_childs)
                {
                    if (c.CollideWith(t))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Method that paint the hitbox and the contour of the texture.
        /// </summary>
        /// <param name="e"></param>

        public void Debug(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(this.GetX(), this.GetY(), this.width, this.height);
            Pen pen = new Pen(Brushes.Red);
            pen.Width = 4;
            g.DrawRectangle(pen, rect);
            g.DrawImage(this.hitbox, new Point(x, y));
            pen.Dispose();
        }

        /// <summary>
        /// Comparator which sort the textures by depth.
        /// </summary>
        /// <param name="t1">The first texture.</param>
        /// <param name="t2">The second texture.</param>
        /// <returns>
        /// 1 if t1 is above t2
        /// -1 if t1 is below t2
        /// 0 if they are at the same depth.
        /// </returns>

        public static int Compare(Texture t1, Texture t2)
        {
            if (t1.GetZ() >= t2.GetZ())
            {
                return 1;
            }
            else if (t1.GetZ() < t2.GetZ())
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the directory where is stored the images
        /// </summary>
        /// <returns>A string which represents the path to the folder.</returns>

        public static string GetDirImages()
        {
            return projectDir;
        }

        /// <summary>
        /// Add a child to the texture.
        /// </summary>
        /// <param name="t">Texture that will be add as a child.</param>

        public virtual void AddChild(Texture t)
        {
            if (!list_childs.Contains(t))
            {
                this.list_childs.Add(t);
            }
        }

        /// <summary>
        /// Remove a child from the texture.
        /// </summary>
        /// <param name="t">Texture that will be remove from the childs.</param>

        public virtual void RemoveChild(Texture t)
        {
            if (list_childs.Contains(t))
            {
                this.list_childs.Remove(t);
            }
        }

        /// <summary>
        /// Remove all the childs from the texture.
        /// </summary>

        public void RemoveAllChilds()
        {
            foreach(Texture t in this.list_childs)
            {
                t.RemoveAllChilds();
            }
            this.list_childs = new List<Texture>();
        }

        /// <summary>
        /// Get a list of all the childs of the texture.
        /// </summary>
        /// <returns>All the childs contained in a list.</returns>

        public List<Texture> GetListChilds()
        {
            return this.list_childs;
        }

        /// <summary>
        /// Get the image used by the texture.
        /// </summary>
        /// <returns>The texture's image</returns>

        public Image GetImage()
        {
            if(this.image == null)
            {
                MessageBox.Show(this.name);
            }
            return this.image;
        }

        /// <summary>
        /// Get the path to the image used by the texture.
        /// </summary>
        /// <returns>The path to the texture's image.</returns>

        public string GetPathImage()
        {
            return this.path_image;
        }

        /// <summary>
        /// Get the path to the hitbox used by the texture.
        /// </summary>
        /// <returns>The path to the texture's hitbox.</returns>

        public string GetPathHitBox()
        {
            return this.path_hitbox;
        }

        /// <summary>
        /// Set the image used by the texture.
        /// </summary>
        /// <param name="img">The image that will be used.</param>

        public void SetImage(Image img)
        {
            this.image = img;
            this.width = this.image.Width;
            this.height = this.image.Height;
        }

        /// <summary>
        /// Set the hitbox used by the texture.
        /// </summary>
        /// <param name="bm">The bitmap that will be used.</param>

        public void SetHitbox(Bitmap bm)
        {
            this.hitbox = bm;
        }

        /// <summary>
        /// Can the texture collide with other textures.
        /// </summary>
        /// <returns>True if the texture has to collide with the others textures, false otherwise.</returns>

        public bool CanCollide()
        {
            return this.collide;
        }

        public virtual string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Get the X coordinate of the texture.
        /// </summary>
        /// <returns>The currect x coordinate.</returns>

        public int GetX()
        {
            return this.x;
        }

        /// <summary>
        /// Get the Y coordinate of the texture.
        /// </summary>
        /// <returns>The currect y coordinate.</returns>

        public int GetY()
        {
            return this.y;
        }

        /// <summary>
        /// Set the X coordinate of the texture.
        /// </summary>
        /// <param name="x">Set the x coordinate.</param>

        public void SetX(int x)
        {
            foreach(Texture t in this.list_childs)
            {
                t.SetX(x+(t.GetX()-this.GetX()));
            }
            this.x = x;
        }

        /// <summary>
        /// Set the Y coordinate of the texture.
        /// </summary>
        /// <param name="y">Set the y coordinate.</param>

        public void SetY(int y)
        {
            this.y = y;
            foreach (Texture t in this.list_childs)
            {
                t.SetY(y);
            }
        }

        /// <summary>
        /// Get the hitbox used by the texture.
        /// </summary>
        /// <returns>The texture's hitbox.</returns>

        public Bitmap GetHitbox()
        {
            return this.hitbox;
        }

        /// <summary>
        /// Get the Z coordinate.
        /// </summary>
        /// <returns>The z set by the user if set, or the y coordinate plus the height of the image used by the texture.</returns>

        public int GetZ()
        {
            if (this.z == null)
            {
                return this.GetY() + this.height;
            }
            else
            {
                return (int)this.z;
            }
        }

        /// <summary>
        /// Get the width of the image used by the texture.
        /// </summary>
        /// <returns>The width of the image.</returns>

        public int GetWidth()
        {
            return this.width;
        }

        /// <summary>
        /// Get the height of the image used by the texture.
        /// </summary>
        /// <returns>The height of the image.</returns>

        public int GetHeight()
        {
            return this.height;
        }

        /// <summary>
        /// Get the path of the image accordingly to the name.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns></returns>

        public String GetCustomPathImage(string name)
        {
            return projectDir + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + name + ".png";
        }

        /// <summary>
        /// Get the path of the hitbox accordingly to the name.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns></returns>

        public String GetCustomPathHitbox(string name)
        {
            return projectDir + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + name + "Hitbox.png";
        }

        /// <summary>
        /// Enable the collisions with the other textures.
        /// </summary>

        public void EnableCollisions()
        {
            this.collide = true;
        }

        /// <summary>
        /// Disable the collisions with the other textures.
        /// </summary>

        public void DisableCollisions()
        {
            this.collide = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public bool CollidesOnlyOnZ()
        {
            return this.collide_only_z;
        }

        /// <summary>
        /// Return a string with contains the name of the texture.
        /// </summary>
        /// <returns>The texture's name.</returns>

        public override string ToString()
        {
            if (this.name == null)
            {
                return this.GetType().Name;
            }
            else
            {
                return this.name;
            }
        }
    }
}