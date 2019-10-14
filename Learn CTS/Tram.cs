
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Tram : Texture
    {

        // Attributes

        private static string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        private int state = 2;
        private int max_speed = 50;
        private int speed = 50;
        private bool inside = false;
        private String path_image_outside;
        private String path_hitbox_outside;
        private Image image_outside;
        private Bitmap hitbox_outside;
        private Bitmap hitbox_inside;
        private Texture doors_left;
        private Texture doors_right;
        private Texture test;
        private Texture tram_outside;
        private int max_distance_stop;

        /// <summary>
        /// Constructor of a tram.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Tram(int x, int y) : base(x, y, -2000)
        {
            this.max_distance_stop = this.DistanceBeforeStopping();
            this.path_image_outside = projectDir + "\\resources\\textures\\" + this.GetType().Name + "Outside.png";
            this.path_hitbox_outside = projectDir + "\\resources\\textures\\" + this.GetType().Name + "OutsideHitBox.png";
            this.image_outside = CreateImage(this.path_image_outside);
            this.hitbox_inside = CreateHitbox(this.GetPathHitBox());
            this.hitbox_outside = CreateHitbox(this.path_hitbox_outside);
            this.tram_outside = new Texture("TramOutside",this.GetX(), this.GetY());
            this.doors_left = new Texture("DoorsLeft", this.GetX(), this.GetY(), this.tram_outside.GetZ() + 1);
            this.doors_right = new Texture("DoorsRight", this.GetX(), this.GetY(), this.tram_outside.GetZ() + 1);
            this.tram_outside.DisableCollisions();
            this.test = new Texture("test", this.GetX(), this.GetY(), true);
            this.test.DisableCollisions();
            this.AddChild(doors_left);
            this.AddChild(doors_right);
            this.AddChild(test);
            this.AddChild(tram_outside);
        }

        /// <summary>
        /// Switch between outside and inside, which results in a change of hitbox, doors and the outside aspect.
        /// </summary>

        public void ChangeInside()
        {
            this.inside = !this.inside;
            if (this.inside)
            {
                this.RemoveChild(this.doors_left);
                this.RemoveChild(this.doors_right);
                this.RemoveChild(this.tram_outside);
            }
            else
            {
                this.AddChild(this.doors_left);
                this.AddChild(this.doors_right);
                this.AddChild(this.tram_outside);
            }
        }

        /// <summary>
        /// Depending of the state of the tram, changes the hitbox, moves the tram or the doors.
        /// </summary>

        public void Update()
        {
            if(this.inside || this.GetState() == 1)
            {
                this.SetHitbox(this.hitbox_inside);
            }
            else
            {
                this.SetHitbox(this.hitbox_outside);
            }
            if (this.GetState() == 1 && this.speed < this.max_speed)
            {
                if (this.doors_left.GetX() <= this.GetX())
                {
                    CloseDoors();
                }
                else
                {
                    this.speed++;
                    if (this.speed == this.max_speed)
                    {
                        this.ChangeState();
                    }
                }
            }
            else if(this.GetState() == 3 && this.speed > 0)
            {
                this.speed--;
            }
            else if (this.GetState() == 3 && this.speed <= 0)
            {
                OpenDoors();
            }
            this.Move(this.speed, 0);
        }

        /// <summary>
        /// Open the doors.
        /// </summary>

        public void OpenDoors()
        {
            if (this.doors_left.GetX() >= this.GetX() - 76)
            {
                this.doors_left.Move(-2, 0);
                this.doors_right.Move(2, 0);
            }
            else
            {
                this.ChangeState();
            }
        }

        /// <summary>
        /// Close the doors.
        /// </summary>

        public void CloseDoors()
        {
            this.doors_left.Move(2, 0);
            this.doors_right.Move(-2, 0);
        }

        /// <summary>
        /// Check if the player is inside the tram.
        /// </summary>
        /// <returns></returns>

        public bool IsInside()
        {
            return this.inside;
        }

        /// <summary>
        /// Return the maximal speed of the tram.
        /// </summary>
        /// <returns></returns>

        public int GetMaxSpeed()
        {
            return this.max_speed;
        }

        /// <summary>
        /// Set the speed of the tram.
        /// </summary>
        /// <param name="v"></param>

        public void SetSpeed(int v)
        {
            this.speed = v;
        }

        /// <summary>
        /// Change the state of the tram.
        /// </summary>

        public void ChangeState()
        {
            this.state++;
        }

        /// <summary>
        /// Return the state of the tram.
        /// </summary>
        /// <returns>
        /// 0 = Stopped
        /// 1 = Accelerating
        /// 2 = Moving at maximal speed
        /// 3 = Deccelerating
        /// </returns>

        public int GetState()
        {
            return this.state % 4;
        }

        /// <summary>
        /// Return the state of the tram in a string.
        /// </summary>
        /// <returns></returns>

        public string GetStateString()
        {
            switch (this.GetState()){
                case 0:
                    return "Stopped";
                case 1:
                    return "Leaving";
                case 2:
                    return "Moving";
                case 3:
                    return "Stopping";
                default:
                    return "France";
            }
        }

        /// <summary>
        /// Get the actual speed of the tram.
        /// </summary>
        /// <returns></returns>

        public int GetSpeed()
        {
            return this.speed;
        }

        /// <summary>
        /// Set the state of the tram.
        /// </summary>
        /// <param name="s"></param>

        public void SetState(int s)
        {
            this.state = s;
        }

        /// <summary>
        /// Update the tram then paint it on the screen.
        /// </summary>
        /// <param name="e"></param>

        public override void UpdateGraphic(PaintEventArgs e)
        {
            this.Update();
            base.UpdateGraphic(e);
        }

        private int DistanceBeforeStopping()
        {
            int s = 0;
            for(int i = 1; i<=this.max_speed; i++)
            {
                s += i;
            }
            return s;
        }

        public int GetDistanceMaxStop()
        {
            return this.max_distance_stop;
        }
    }
}
