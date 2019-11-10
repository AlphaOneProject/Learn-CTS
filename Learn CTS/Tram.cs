
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
        private Texture doors_left;
        private Texture doors_right;
        private Texture interior;
        private Texture tram_outside;
        private Texture tram_inside;
        private int max_distance_stop;
        private bool player_inside = false;
        private int[] pos_doors = new int[6]{
            510,
            920,
            1528,
            1944,
            2552,
            2928
        };

        /// <summary>
        /// Constructor of a tram.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Tram(int x, int y) : base("Tram", x, y, -2000)
        {
            this.max_distance_stop = this.DistanceBeforeStopping();
            this.tram_inside = new Texture("TramInside", this.GetX(), this.GetY(), this.GetZ() + 1);
            this.tram_outside = new Texture("TramOutside", this.GetX(), this.GetY(), true);
            this.doors_left = new Texture("DoorsLeft", this.GetX(), this.GetY(), this.tram_outside.GetZ() + 1, true);
            this.doors_right = new Texture("DoorsRight", this.GetX(), this.GetY(), this.tram_outside.GetZ() + 1, true);
            this.tram_inside.DisableCollisions();
            this.interior = new Texture("interior", this.GetX(), this.GetY(), true);
            this.AddChild(doors_left);
            this.AddChild(doors_right);
            this.AddChild(interior);
            this.AddChild(tram_outside);
            this.AddChild(tram_inside);
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
                this.doors_left.SetX(this.GetX());
                this.AddChild(this.doors_left);
                this.doors_right.SetX(this.GetX());
                this.AddChild(this.doors_right);
                this.tram_outside.SetX(this.GetX());
                this.AddChild(this.tram_outside);
            }
        }

        /// <summary>
        /// Depending of the state of the tram, changes the hitbox, moves the tram or the doors.
        /// </summary>

        public void Update()
        {
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
                if (this.doors_left.GetX() >= this.GetX() - 76)
                {
                    OpenDoors();
                }
                else
                {
                    this.ChangeState();
                }
            }
            this.Move(this.speed, 0);
        }

        /// <summary>
        /// Open the doors.
        /// </summary>

        public void OpenDoors()
        {
            this.doors_left.Move(-2, 0);
            this.doors_right.Move(2, 0);
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
            UpdateState();
        }

        public void UpdateState()
        {
            if (this.GetState() != 0 && player_inside)
            {
                this.tram_inside.EnableCollisions();
            }
            else
            {
                this.tram_inside.DisableCollisions();
            }
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
            UpdateState();
        }

        /// <summary>
        /// Update the tram then paint it on the screen.
        /// </summary>
        /// <param name="e"></param>

        public override void OnPaint(PaintEventArgs e)
        {
            this.Update();
            base.OnPaint(e);
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

        public override void AddChild(Texture t)
        {
            if(t.GetType().Name == "Player")
            {
                this.player_inside = true;
                this.doors_left.DisableCollisions();
                this.doors_right.DisableCollisions();
            }
            base.AddChild(t);
        }

        public override void RemoveChild(Texture t)
        {
            if (t.GetType().Name == "Player")
            {
                this.player_inside = false;
                this.doors_left.EnableCollisions();
                this.doors_right.EnableCollisions();
            }
            base.RemoveChild(t);
        }

        public int GetIndexNearestDoor(int pos_c)
        {
            int min = Math.Abs(this.GetX()+pos_doors[0] - pos_c);
            int index = 0;
            for(int i = 1; i<pos_doors.Length; i++)
            {
                if (Math.Abs(this.GetX()+pos_doors[i] - pos_c) < min)
                {
                    min = Math.Abs(pos_doors[i] - pos_c);
                    index = i;
                }
            }
            return index;
        }

        public int GetPosDoor(int i)
        {
            return this.GetX() + this.pos_doors[i];
        }

        public int GetNumberDoors()
        {
            return this.pos_doors.Length;
        }
    }
}