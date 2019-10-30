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
        private bool hasMoved = false;
        private int last_direction = 1;
        private static int m;

        /// <summary>
        /// Constructor of character.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Character(int x, int y) : base(x, y, true)
        {
        }

        /// <summary>
        /// Constructor of character with a custom name.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Character(int id, String name, int x, int y) : base(name, x, y, true)
        {
            this.id = id;
            for (int i = 0; i < 8; i++)
            {
                animation_list_est.Add(this.CreateImage(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + ((Character)this).GetID() + Path.DirectorySeparatorChar + "1_" + (i + 1).ToString() + ".png"));
                animation_list_west.Add(this.CreateImage(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + ((Character)this).GetID() + Path.DirectorySeparatorChar + "3_" + (i + 1).ToString() + ".png"));
            }
        }

        public void SetDefaultPose()
        {
            this.SetImage(this.CreateImage(Texture.GetDirImages() + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar + ((Character)this).GetID() + Path.DirectorySeparatorChar + this.last_direction.ToString() + "_0.png"));
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
                    if (c >= 8)
                    {
                        this.c = 0;
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
    }
}
