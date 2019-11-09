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
        private int? obj_x;
        private int? obj_y;
        private List<Image> animation_list_west = new List<Image>();
        private List<Image> animation_list_est = new List<Image>();
        private bool hasMoved = false;
        private int last_direction = 1;
        private static int m;
        private string folder;
        private string name;

        /// <summary>
        /// Constructor of character with a custom name.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Character(int id, String name, String folder, int x, int y) : base(x, y, true)
        {
            this.id = id;
            this.folder = folder;
            this.name = name;
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
            if (t % m == 0 && this.GetType().Name == "Player")
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
                    hasMoved = true;
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

        public override void UpdateGraphic(PaintEventArgs e)
        {
            if(t%m == 0)
            {
                if (!hasMoved)
                {
                    this.SetDefaultPose();
                }
                if (hasMoved)
                {
                    hasMoved = false;
                }
            }
            base.UpdateGraphic(e);
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

        public void GoTo(int x, int y)
        {
            this.obj_x = x - (this.GetX() + this.GetWidth() / 2);
            this.obj_y = y - (this.GetY() + this.GetHeight());
        }

        /// <summary>
        /// Get the x coordinate of the objective.
        /// </summary>
        /// <returns>The x coordinate that has to be reached.</returns>

        public int GetObjX()
        {
            return (int)this.obj_x;
        }

        public void UpdateObjX(int x)
        {
            this.obj_x += x;
        }

        public void UpdateObjY(int y)
        {
            this.obj_y += y;
        }

        /// <summary>
        /// Get the y coordinate of the objective.
        /// </summary>
        /// <returns>The y coordinate that has to be reached.</returns>

        public int GetObjY()
        {
            return (int)this.obj_y;
        }

        /// <summary>
        /// Check if the player has an objective.
        /// </summary>
        /// <returns>Return true if he has an objective, false otherwise.</returns>

        public bool HasObjective()
        {
            return (this.obj_x != null && this.obj_y != null);
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
            return (
                this.obj_x < 8 &&
                this.obj_x > -8);
        }

        public bool ReachedObjY()
        {
            return (
                this.obj_y < 8 &&
                this.obj_y > -8);
        }

        /// <summary>
        /// Remove the player's objective.
        /// </summary>

        public void RemoveObjective()
        {
            this.obj_x = null;
            this.obj_y = null;
        }
    }
}
