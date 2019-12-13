using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
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
        private bool visible = true;

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
        public Texture(int x, int y)
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
        /// Constructor of a texture which is placed at the specified coordinates x,y and a custom depth z.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="b">Specifies if the texture collides only when the depth is the same as the other texture.</param>
        public Texture(int x, int y, int z) : this(x, y)
        {
            this.z = z;
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Texture(string name, int x, int y) : this(x,y)
        {
            this.name = name;
            this.path_image = GetDefaultPathImage();
            this.path_hitbox = GetDefaultPathHitbox();
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
        /// Constructor of a texture with a custom name, a custom folder, placed at the specified coordinates x,y
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="folder">The folder where to find the images.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Texture(string name, string folder, int x, int y) : this(x, y)
        {
            this.name = name;
            this.path_image = GetCustomPathImage(folder, name);
            this.path_hitbox = GetCustomPathHitbox(folder, name);
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
        /// <param name="folder">The folder where to find the images.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="b">Specifies if the texture collides only when the depth is the same as the other texture.</param>
        public Texture(string name, string folder, int x, int y, bool b) : this(name, folder, x, y)
        {
            this.collide_only_z = b;
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y and a custom depth z.
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="folder">The folder where to find the images.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">THe depth position compared to the other textures.</param>
        public Texture(string name, string folder, int x, int y, int z) : this(name, folder, x, y)
        {
            this.z = z;
        }

        /// <summary>
        /// Constructor of a texture with a custom name, placed at the specified coordinates x,y, a custom depth z and specifies if it collides only on depth.
        /// </summary>
        /// <param name="name"> Name of the texture.</param>
        /// <param name="folder">The folder where to find the images.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">THe depth position compared to the other textures.</param>
        /// <param name="b">Specifies if the texture collides only when the depth is the same as the other texture.</param>
        public Texture(string name, string folder, int x, int y, int z, bool b) : this(name, folder, x, y, b)
        {
            this.z = z;
        }

        /// <summary>
        /// Move the texture and all its childs, by adding a to the x coordinate and b to the y coordinate.
        /// </summary>
        /// <param name="a">Number that will be added to the current x coordinate of the texture.</param>
        /// <param name="b">Number that will be added to the current y coordinate of the texture.</param>

        public virtual void Move(int a, int b)
        {
            this.Move(a, b, true);
        }

        /// <summary>
        /// Move the texture, by adding a to the x coordinate and b to the y coordinate.
        /// </summary>
        /// <param name="a">Number that will be added to the current x coordinate of the texture.</param>
        /// <param name="b">Number that will be added to the current y coordinate of the texture.</param>
        /// <param name="mc">True if the movement moves also the childs.</param>
        public virtual void Move(int a, int b, bool mc)
        {
            this.x += a;
            this.y += b;
            if (mc)
            {
                foreach (Texture t in this.list_childs)
                {
                    t.Move(a, b, mc);
                }
            }
        }

        /// <summary>
        /// Paint itself to the window if the texture is visible and on the screen.
        /// </summary>
        /// <param name="e">The paint environment.</param>
        public virtual void OnPaint(PaintEventArgs e)
        {
            if (this.visible && this.IsOnScreen(e))
            {
                Graphics g = e.Graphics;
                try
                {
                    g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
                }
                catch (System.ArgumentException) { }
            }
        }

        /// <summary>
        /// Check if the texture is on the screen.
        /// </summary>
        /// <param name="e">The paint environment.</param>
        /// <returns>True if the texture is on the screen.</returns>
        public bool IsOnScreen(PaintEventArgs e)
        {
            Rectangle screen = e.ClipRectangle;
            return this.GetX() + this.GetWidth() >= screen.X && this.GetX() <= screen.X + screen.Width && this.GetY() + this.GetHeight() >= screen.Y && this.GetY() <= screen.Y + screen.Height;
        }

        /// <summary>
        /// Create a image from a file specified by the path.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>The image created from the file.</returns>
        public Image CreateImage(string path)
        {
            try
            {
                Bitmap orig = new Bitmap(path);
                Bitmap clone = new Bitmap(orig.Width, orig.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                using (Graphics gr = Graphics.FromImage(clone))
                {
                    gr.DrawImage(orig, new Rectangle(0, 0, clone.Width, clone.Height));
                }
                orig.Dispose();
                return (Image)clone;
            }
            catch (Exception)
            {
                MessageBox.Show("L'image de la texture n'a pas été trouvée, ou elle est inaccessible.\n" +
                                "Veuillez vérifier qu'elle soit bien ici : " + path);
                GameWindow.GetInstance().Close();
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
            catch (IOException)
            {
            }
            return null;
        }

        /// <summary>
        /// Method that can determine if the hitbox of the texture is hit at these coordinates.
        /// </summary>
        /// <param name="c">The x coordinate that will be tested.</param>
        /// <param name="d">The y coordinate that will be tested.</param>
        /// <returns>True if the hitbox is hit, false otherwise.</returns>
        public virtual bool IsHitboxHit(int c, int d)
        {
            if (c - this.x >= 0 && c - this.x < this.width && d - this.y >= 0 && d - this.y < this.height)
            {
                bool b = false;
                try
                {
                    b = !Color.Equals(this.hitbox.GetPixel(c - this.x, d - this.y), Color.FromArgb(0, 0, 0, 0));
                }
                catch(Exception)
                {
                }
                return b;
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
        /// /// <param name="b">Check if t collides with the childs</param>
        /// <returns>true if this hit the hitbox of t or one of its childs, false otherwise.</returns>
        public virtual bool CollideWith(Texture t, bool b)
        {
            if (Texture.ReferenceEquals(this, t) || !this.collide || !t.CanCollide())
            {
                return false;
            }
            if (this.collide_only_z && t.CollidesOnlyOnZ() && Math.Abs(this.GetZ() - t.GetZ()) > 4)
            {
                return false;
            }
            Rectangle r1 = new Rectangle(this.GetX(), this.GetY(), this.width, this.height);
            Rectangle r2 = new Rectangle(t.GetX(), t.GetY(), t.GetImage().Width, t.GetImage().Height);
            if (r1.IntersectsWith(r2))
            {
                Rectangle r3 = Rectangle.Intersect(r1, r2);
                if(this.hitbox != null)
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
                if (b)
                {
                    foreach (Texture c in this.list_childs)
                    {
                        if (c.CollideWith(t,b))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Method that paint the hitbox and the edge of the texture image.
        /// </summary>
        /// <param name="e"></param>
        public void Debug(PaintEventArgs e)
        {
            if (this.hitbox == null || !this.collide) return;
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
            if (t1.GetZ() > t2.GetZ())
            {
                return 1;
            }
            else if (t1.GetZ() < t2.GetZ())
            {
                return -1;
            }
            else
            {
                if (t1.GetHashCode() > t2.GetHashCode())
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
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
        public virtual Image GetImage()
        {
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
            if(this.image != null) this.image.Dispose();
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

        /// <summary>
        /// Can the name of the texture.
        /// </summary>
        /// <returns>The name of the texture.</returns>
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
            foreach (Texture t in this.list_childs)
            {
                t.SetY(y + (t.GetY() - this.GetY()));
            }
            if (this.z != null)
            {
                this.z += (y - this.GetY());
            }
            this.y = y;
        }

        /// <summary>
        /// Set the custom depth of the texture.
        /// </summary>
        /// <param name="z"></param>
        public void SetZ(int z)
        {
            this.z = z;
        }

        /// <summary>
        /// Set the visible state of the texture.
        /// </summary>
        /// <param name="b">True if the texture is visible, false otherwise.</param>
        public void SetVisible(bool b)
        {
            this.visible = b;
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
        /// Get the depth coordinate.
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
        /// Check if the texture contains the texture argument as its child.
        /// </summary>
        /// <param name="te">The texture tested.</param>
        /// <returns>True if this texture contains tc, false otherwise.</returns>
        public bool Contains(Texture tc)
        {
            foreach(Texture t in list_childs)
            {
                if (t == tc) return true;
                if (t.GetListChilds().Count > 0) t.Contains(tc);
            }
            return false;
        }

        /// <summary>
        /// Get the path of the image accordingly to the name.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns>The custom path of the image.</returns>
        public string GetCustomPathImage(string folder, string name)
        {
            return projectDir + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + name + ".png";
        }

        /// <summary>
        /// Get the path of the hitbox accordingly to the name.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns>The custom path of the hitbox.</returns>
        public string GetCustomPathHitbox(string folder, string name)
        {
            return projectDir + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + name + "_hitbox.png";
        }
        
        /// <summary>
        /// Get the default path of the image.
        /// </summary>
        /// <returns>The default path of the image.</returns>
        public string GetDefaultPathImage()
        {
            return this.GetCustomPathImage("others", this.GetName());
        }

        /// <summary>
        /// Get the default path of the hitbox.
        /// </summary>
        /// <returns>The default path of the image.</returns>
        public string GetDefaultPathHitbox()
        {
            return this.GetCustomPathHitbox("others", this.GetName());
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
        /// Check if the texture collides only according to the depth.
        /// </summary>
        /// <returns>True if the textures collides only according to the depth.</returns>
        public bool CollidesOnlyOnZ()
        {
            return this.collide_only_z;
        }

        /// <summary>
        /// Dispose the image and the hitbox used by the texture.
        /// </summary>
        public virtual void Dispose()
        {
            foreach(Texture t in list_childs)
            {
                t.Dispose();
            }
            this.image.Dispose();
            if(this.hitbox != null) this.hitbox.Dispose();
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
