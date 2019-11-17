using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    abstract class Transport : Texture
    {
        // Attributes

        private int state = 2;
        private int max_speed = 50;
        private int speed = 50;
        private bool is_inside = false;
        private Texture doors_left;
        private Texture doors_right;
        private Texture interior;
        private Texture outside;
        private Texture inside;
        private int max_distance_stop;
        private bool player_inside = false;
        private int[] pos_doors;

        /// <summary>
        /// Constructor of a vehicule.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>

        public Transport(string name, int x, int y, int[] pos_doors) : base(name, x, y, -2000)
        {
            this.pos_doors = pos_doors;
            this.max_distance_stop = this.DistanceBeforeStopping();
            this.inside = new Texture(name + "Inside", this.GetX(), this.GetY(), this.GetZ() + 1);
            this.outside = new Texture(name + "Outside", this.GetX(), this.GetY(), true);
            this.doors_left = new Texture(name + "DoorsLeft", this.GetX()+424, this.GetY()+112, this.outside.GetZ() + 1, true);
            this.doors_right = new Texture(name + "DoorsRight", this.GetX()+512, this.GetY()+112, this.outside.GetZ() + 1, true);
            this.inside.DisableCollisions();
            this.inside.ChangeVisible();
            this.interior = new Texture(name + "Interior", this.GetX()+480, this.GetY()+208, true);
            this.AddChild(doors_left);
            this.AddChild(doors_right);
            this.AddChild(interior);
            this.AddChild(outside);
            this.AddChild(this.inside);
        }

        /// <summary>
        /// Switch between outside and inside, which results in a change of hitbox, doors and the outside aspect.
        /// </summary>

        public void ChangeInside()
        {
            this.is_inside = !this.is_inside;
            if (this.is_inside)
            {
                this.RemoveChild(this.doors_left);
                this.RemoveChild(this.doors_right);
                this.RemoveChild(this.outside);
                foreach (Texture t in this.GetListChilds())
                {
                    if (t.GetType().Name == "NPC")
                    {
                        ((NPC)t).DisplayInteraction();
                    }
                }
            }
            else
            {
                this.doors_left.SetX(this.GetX());
                this.doors_left.SetY(this.GetY());
                this.AddChild(this.doors_left);
                this.doors_right.SetX(this.GetX());
                this.doors_right.SetY(this.GetY());
                this.AddChild(this.doors_right);
                this.outside.SetX(this.GetX());
                this.outside.SetY(this.GetY());
                this.AddChild(this.outside);
                foreach (Texture t in this.GetListChilds())
                {
                    if (t.GetType().Name == "NPC")
                    {
                        ((NPC)t).RemoveInteraction();
                    }
                }
            }
        }

        /// <summary>
        /// Depending of the state of the vehicule, changes the hitbox, moves the vehicule or the doors.
        /// </summary>

        public void Update()
        {
            if (this.GetState() == 1 && this.speed < this.max_speed)
            {
                if (this.doors_left.GetX() < this.GetX() + 424)
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
            else if (this.GetState() == 3 && this.speed > 0)
            {
                this.speed--;
            }
            else if (this.GetState() == 3 && this.speed <= 0)
            {
                if (this.doors_left.GetX() >= this.GetX() + 348)
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
        /// Check if the player is inside the vehicule.
        /// </summary>
        /// <returns></returns>

        public bool IsInside()
        {
            return this.is_inside;
        }

        /// <summary>
        /// Return the maximal speed of the vehicule.
        /// </summary>
        /// <returns></returns>

        public int GetMaxSpeed()
        {
            return this.max_speed;
        }

        /// <summary>
        /// Set the speed of the vehicule.
        /// </summary>
        /// <param name="v"></param>

        public void SetSpeed(int v)
        {
            this.speed = v;
        }

        /// <summary>
        /// Change the state of the vehicule.
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
                this.inside.EnableCollisions();
            }
            else
            {
                this.inside.DisableCollisions();
            }
        }

        public bool IsPlayerInside()
        {
            return this.player_inside;
        }

        /// <summary>
        /// Return the state of the vehicule.
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
        /// Return the state of the vehicule in a string.
        /// </summary>
        /// <returns></returns>

        public string GetStateString()
        {
            switch (this.GetState())
            {
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
        /// Get the actual speed of the vehicule.
        /// </summary>
        /// <returns></returns>

        public int GetSpeed()
        {
            return this.speed;
        }

        /// <summary>
        /// Set the state of the vehicule.
        /// </summary>
        /// <param name="s"></param>

        public void SetState(int s)
        {
            this.state = s;
            UpdateState();
        }

        /// <summary>
        /// Update the vehicule then paint it on the screen.
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
            for (int i = 1; i <= this.max_speed; i++)
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
            if (t.GetType().Name == "Player")
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
            int min = Math.Abs(this.GetX() + pos_doors[0] - pos_c);
            int index = 0;
            for (int i = 1; i < pos_doors.Length; i++)
            {
                if (Math.Abs(this.GetX() + pos_doors[i] - pos_c) < min)
                {
                    min = Math.Abs(this.GetX() + pos_doors[i] - pos_c);
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
