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
        private List<Image> animation_list_west = new List<Image>();
        private List<Image> animation_list_est = new List<Image>();
        private List<Point> list_objectives = new List<Point>();
        private int last_direction = 1;
        private string folder;
        private string name;
        private static int speed_character = 8;

        /// <summary>
        /// Get the speed of a character.
        /// </summary>
        /// <returns>The speed of a character.</returns>
        public static int GetCharacterSpeed()
        {
            return speed_character;
        }

        /// <summary>
        /// Construct a character.
        /// </summary>
        /// <param name="id">ID of the character.</param>
        /// <param name="name">Name of the character.</param>
        /// <param name="npc_folder">The folder of the character appearence. If null, a default appearence will be taken.</param>
        /// <param name="x">The position of the character on the x axis.</param>
        /// <param name="y">The position of the character on the y axis.</param>
        public Character(int id, string name, string npc_folder, int x, int y) : base(x, y, true)
        {
            string character_path = Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar;
            Random r = new Random();
            if (npc_folder == null)
            {
                this.folder = Directory.GetDirectories(@"" + character_path)[r.Next(0, Directory.GetDirectories(@"" + character_path).Length - 1)].Remove(0,character_path.Length);
                while (this.folder == "" || !Tools.Is_Valid(character_path + this.folder))
                {
                    this.folder = Directory.GetDirectories(@"" + character_path)[r.Next(0, Directory.GetDirectories(@"" + character_path).Length - 1)].Remove(0, character_path.Length);
                }
            }
            else this.folder = npc_folder;
            if (name == null) name = id.ToString();
            else this.name = name;
            this.id = id;
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                last_direction = 1;
            }
            else
            {
                last_direction = 3;
            }
            this.SetImage(this.CreateImage(character_path + this.folder + Path.DirectorySeparatorChar + last_direction.ToString()+"_0.png"));
            this.SetHitbox(this.CreateHitbox(character_path + this.folder + Path.DirectorySeparatorChar + "Hitbox.png"));
            for (int i = 0; i <= 8; i++)
            {
                animation_list_est.Add(this.CreateImage(character_path + this.folder + Path.DirectorySeparatorChar + "1_" + i.ToString() + ".png"));
                animation_list_west.Add(this.CreateImage(character_path + this.folder + Path.DirectorySeparatorChar + "3_" + i.ToString() + ".png"));
            }
        }

        /// <summary>
        /// Get the current image of the npc following the state of the animation.
        /// </summary>
        /// <returns>The current image of the animation.</returns>
        public override Image GetImage()
        {
            if(last_direction == 1)
            {
                return animation_list_est[c];
            }
            else
            {
                return animation_list_west[c];
            }
        }

        /// <summary>
        /// Set the default pose of the npc, used when the npc does not move anymore.
        /// </summary>
        public void SetDefaultPose()
        {
            this.c = 0;
        }

        /// <summary>
        /// Update the animation of the npc following its movements.
        /// </summary>
        /// <param name="a">The movement on the x axis.</param>
        /// <param name="b">The movement on the y axis.</param>
        public void UpdateMovement(int a, int b)
        {
            t++;
            if (t % 4 == 0)
            {
                t = 0;
                if (a > 0)
                {
                    this.last_direction = 1;
                }
                if (a < 0)
                {
                    this.last_direction = 3;
                }
                if (a != 0 || b!=0)
                {
                    this.c++;
                    if (c > 8)
                    {
                        this.c = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Get the ID of the Character.
        /// </summary>
        /// <returns>The ID of the character.</returns>
        public int GetID()
        {
            return this.id;
        }

        /// <summary>
        /// Get the name of the character.
        /// </summary>
        /// <returns>The name of the character.</returns>
        public override string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Get the last direction of the character.
        /// </summary>
        /// <returns>The last direction of the character. 3 is left and 1 is right.</returns>
        public int GetDirection()
        {
            return this.last_direction;
        }

        /// <summary>
        /// Set the current direction of the character.
        /// </summary>
        /// <param name="d">The new direction of the character. 3 is left and 1 is right.</param>
        public void SetDirection(int d)
        {
            if (d != 1 || d != 3) this.last_direction = d;
            else throw new ArgumentException();
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

        /// <summary>
        /// Set an objective on the x axis.
        /// </summary>
        /// <param name="x"></param>
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
