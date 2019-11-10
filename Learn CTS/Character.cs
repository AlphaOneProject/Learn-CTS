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
    abstract class Character : Texture
    {
        // Attributes

        private int id;
        private int c = 0;
        private int t = 0;
        //private int? obj_x;
        //private int? obj_y;
        private List<Image> animation_list_west = new List<Image>();
        private List<Image> animation_list_est = new List<Image>();
        private List<Point> list_objectives = new List<Point>();
        private int last_direction = 1;
        private static int m;
        private string folder;
        private string name;
        private bool animated = false;

        /// <summary>
        /// Constructor of character with a custom name.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Character(int id, String name, String folder, bool b, int x, int y) : base(x, y, true)
        {
            if (folder == null) folder = (id % 6 + 1).ToString();
            if (name == null) name = id.ToString();
            this.id = id;
            this.folder = folder;
            this.name = name;
            this.animated = b;
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                last_direction = 1;
            }
            else
            {
                last_direction = 3;
            }
            this.SetImage(this.CreateImage(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + this.folder + Path.DirectorySeparatorChar + last_direction.ToString()+"_0.png"));
            this.SetHitbox(this.CreateHitbox(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + this.folder + Path.DirectorySeparatorChar + "Hitbox.png"));
            for (int i = 0; i <= 8; i++)
            {
                animation_list_est.Add(this.CreateImage(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + this.folder + Path.DirectorySeparatorChar + "1_" + i.ToString() + ".png"));
                animation_list_west.Add(this.CreateImage(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + this.folder + Path.DirectorySeparatorChar + "3_" + i.ToString() + ".png"));
            }
        }

        public void SetDefaultPose()
        {
            if (last_direction == 1)
            {
                this.SetImage(animation_list_est[0]);
            }
            else /*(last_direction == 3)*/
            {
                this.SetImage(animation_list_west[0]);
            }
        }

        public void UpdateMovement(int a, int b)
        {
            t++;
            if (t % m == 0 && this.animated)
            {
                t = 0;
                if (a > 0)
                {
                    this.SetImage(animation_list_est[c]);
                    this.last_direction = 1;
                }
                if (a < 0)
                {
                    this.SetImage(animation_list_west[c]);
                    this.last_direction = 3;
                }
                if(b != 0)
                {
                    if(this.last_direction == 1)
                    {
                        this.SetImage(animation_list_est[c]);
                    }
                    else if(this.last_direction == 3)
                    {
                        this.SetImage(animation_list_west[c]);
                    }
                }
                if (a != 0 || b!=0)
                {
                    //hasMoved = true;
                    this.c++;
                    if (c > 8)
                    {
                        this.c = 1;
                    }
                }
            }
        }

        public static void SetM(int a)
        {
            m = a;
        }

        public override void OnPaint(PaintEventArgs e)
        {
            /*if (t % m == 0)
            {
                //Console.WriteLine(this.id +":"+hasMoved);
                if (!hasMoved)
                {
                    this.SetDefaultPose();
                }
                if (hasMoved)
                {
                    hasMoved = false;
                }
            }*/
            base.OnPaint(e);
        }

        public int GetID()
        {
            return this.id;
        }

        public string GetFolder()
        {
            if(folder == null)
            {
                return this.id.ToString();
            }
            else
            {
                return this.folder;
            }
        }

        public override string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Set coordinates as objective
        /// </summary>
        /// <param name="x">The x coordinate of the objective.</param>
        /// <param name="y">The y coordinate of the objective.</param>

        public virtual void SetObjective(int x, int y)
        {
            this.list_objectives.Add(new Point(x - (this.GetX() + this.GetWidth() / 2), y-(this.GetY() + this.GetHeight())));
        }

        public void SetObjectiveX(int x)
        {
            this.list_objectives.Add(new Point(x - (this.GetX() + this.GetWidth() / 2), 0));
        }
        public void SetObjectiveY(int y)
        {
            this.list_objectives.Add(new Point(0, y - (this.GetY() + this.GetHeight())));
        }

        /// <summary>
        /// Get the x coordinate of the objective.
        /// </summary>
        /// <returns>The x coordinate that has to be reached.</returns>

        public int GetObjX()
        {
            if (this.list_objectives.Count > 0)
            {
                return this.list_objectives[0].X;
            }
            return 0;
        }

        /// <summary>
        /// Get the y coordinate of the objective.
        /// </summary>
        /// <returns>The y coordinate that has to be reached.</returns>

        public int GetObjY()
        {
            if (this.list_objectives.Count > 0)
            {
                return this.list_objectives[0].Y;
            }
            return 0;
        }

        public void UpdateObjX(int x)
        {
            Point p = this.list_objectives[0];
            this.list_objectives[0] = new Point(p.X += x, p.Y);
        }

        public void UpdateObjY(int y)
        {
            Point p = this.list_objectives[0];
            this.list_objectives[0] = new Point(p.X, p.Y+=y);
        }

        /// <summary>
        /// Check if the player has an objective.
        /// </summary>
        /// <returns>Return true if he has an objective, false otherwise.</returns>

        public bool HasObjective()
        {
            return this.list_objectives.Count > 0;
        }

        /// <summary>
        /// Check if the player has reached his objective.
        /// </summary>
        /// <returns>True if the player has reached his objective.</returns>

        public bool ReachedObjective()
        {
            return (
                this.ReachedObjX() &&
                this.ReachedObjY());
        }

        public bool ReachedObjX()
        {
            return
                Math.Abs(this.GetObjX()) < 10;
        }

        public bool ReachedObjY()
        {
            return
                Math.Abs(this.GetObjY()) < 10;
        }

        /// <summary>
        /// Remove the player's objective.
        /// </summary>

        public void RemoveObjective()
        {
            if(this.list_objectives.Count > 0)
            {
                this.list_objectives.RemoveAt(0);
                if(this.list_objectives.Count == 0) this.SetDefaultPose();
            }
        }

        public void RemoveAllObjectives()
        {
            this.list_objectives.Clear();
        }

        public void Animated(bool b)
        {
            this.animated = b;
        }

        public override void Dispose()
        {
            foreach(Image i in animation_list_est)
            {
                i.Dispose();
            }
            foreach (Image i in animation_list_west)
            {
                i.Dispose();
            }
            base.Dispose();
        }
    }
}
