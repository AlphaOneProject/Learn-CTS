using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    abstract class Vehicle : Texture
    {
        // Attributes

        private int state = 2;
        private int max_speed = 50;
        private int speed = 50;
        private bool is_inside = false;
        private Texture outside;
        private Texture inside;
        private int max_distance_stop;

        /// <summary>
        /// Constructor of a vehicle.
        /// </summary>
        /// <param name="name">The name of the vehicle.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Vehicle(string name, int x, int y) : base(name, "vehicle" + Path.DirectorySeparatorChar + name, x, y, -2000)
        {
            this.max_distance_stop = this.DistanceBeforeStopping();
            this.inside = new Texture(name + "_inside", "vehicle" + Path.DirectorySeparatorChar + name, this.GetX(), this.GetY(), this.GetZ() + 1);
            this.outside = new Texture(name + "_outside", "vehicle" + Path.DirectorySeparatorChar + name, this.GetX(), this.GetY(), true);
            this.inside.SetVisible(false);
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
                this.GetDoors()[0].SetVisible(false);
                this.GetDoors()[1].SetVisible(false);
                this.outside.SetVisible(false);
                NPC_Manager.GetInstance().MakeAllNPCsInteractives();
            }
            else
            {
                this.GetDoors()[0].SetVisible(true);
                this.GetDoors()[1].SetVisible(true);
                this.outside.SetVisible(true);
                NPC_Manager.GetInstance().MakeAllNPCsNotInteractives();
            }
        }

        /// <summary>
        /// Get the doors of the vehicle
        /// </summary>
        /// <returns>The first element in the array is the texture of the left doors, the second one is the texture of the right door.</returns>
        public abstract Texture[] GetDoors();

        /// <summary>
        /// Depending of the state of the vehicle, changes the hitbox, moves the vehicle or the doors.
        /// </summary>
        public void Update()
        {
            if (this.GetState() == 1 && this.speed < this.max_speed)
            {
                if (!CloseDoors())
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
                if (!OpenDoors())
                {
                    this.ChangeState();
                }
            }
            if (this.Contains(Player.GetInstance()))
            {
                this.GetDoors()[0].DisableCollisions();
                this.GetDoors()[1].DisableCollisions();
            }
            else
            {
                this.GetDoors()[0].EnableCollisions();
                this.GetDoors()[1].EnableCollisions();
            }
            if (this.is_inside || this.Contains(Player.GetInstance()))
            {
                this.inside.EnableCollisions();
            }
            else
            {
                this.inside.DisableCollisions();
            }
            if (!this.is_inside) this.Move(this.speed, 0);
        }

        /// <summary>
        /// Open slightly the doors of the vehicle
        /// </summary>
        /// <returns>true if the doors are opening. false otherwise.</returns>
        public abstract bool OpenDoors();

        /// <summary>
        /// Close a bit the doors of the vehicle.
        /// </summary>
        /// <returns>true if the doors are closing. false otherwise.</returns>
        public abstract bool CloseDoors();

        /// <summary>
        /// Check if the player is inside the vehicle.
        /// </summary>
        /// <returns></returns>
        public bool IsInside()
        {
            return this.is_inside;
        }

        /// <summary>
        /// Return the maximal speed of the vehicle.
        /// </summary>
        /// <returns></returns>
        public int GetMaxSpeed()
        {
            return this.max_speed;
        }

        /// <summary>
        /// Set the speed of the vehicle.
        /// </summary>
        /// <param name="v"></param>
        public void SetSpeed(int v)
        {
            this.speed = v;
        }

        /// <summary>
        /// Change the state of the vehicle.
        /// </summary>
        public void ChangeState()
        {
            this.state++;
        }

        /// <summary>
        /// Return the state of the vehicle.
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
        /// Get the actual speed of the vehicle.
        /// </summary>
        /// <returns></returns>
        public int GetSpeed()
        {
            return this.speed;
        }

        /// <summary>
        /// Set the state of the vehicle.
        /// </summary>
        /// <param name="s"></param>
        public void SetState(int s)
        {
            this.state = s;
        }

        /// <summary>
        /// Update the vehicle then paint it on the screen.
        /// </summary>
        /// <param name="e">The paint environment.</param>
        public override void OnPaint(PaintEventArgs e)
        {
            this.Update();
            base.OnPaint(e);
        }

        /// <summary>
        /// Calculate the distance before the vehicle stop.
        /// </summary>
        /// <returns>The distance before the vehicle stop.</returns>
        private int DistanceBeforeStopping()
        {
            int s = 0;
            for (int i = 1; i <= this.max_speed; i++)
            {
                s += i;
            }
            return s;
        }

        /// <summary>
        /// Get the distance before the vehicle stop.
        /// </summary>
        /// <returns>The distance before the vehicle stop.</returns>
        public int GetDistanceMaxStop()
        {
            return this.max_distance_stop;
        }


        /// <summary>
        /// Get the closest door compared with a x coordinate.
        /// </summary>
        /// <param name="pos_c">The x position of the character.</param>
        /// <returns>The nearest door.</returns>
        public int GetIndexNearestDoor(int pos_c)
        {
            int min = Math.Abs(this.GetPosDoor(0) - pos_c);
            int index = 0;
            for (int i = 1; i < this.GetNumberDoors(); i++)
            {
                if (Math.Abs(this.GetPosDoor(i) - pos_c) < min)
                {
                    min = Math.Abs(this.GetPosDoor(i) - pos_c);
                    index = i;
                }
            }
            return index;
        }

        /// <summary>
        /// Get the position of the door related to the vehicle.
        /// </summary>
        /// <param name="i">The number of the door.</param>
        /// <returns>The position of the door related to the vehicle.</returns>
        public abstract int GetPosDoor(int i);

        /// <summary>
        /// Get the total number of doors.
        /// </summary>
        /// <returns>The number of doors.</returns>
        public abstract int GetNumberDoors();

        /// <summary>
        /// Set a series of objectives for the npc to get on the vehicle.
        /// </summary>
        /// <param name="n">The npc which will enter the vehicle.</param>
        public abstract void SetPathNPCToVehicle(NPC n);

        /// <summary>
        /// Place the npc randomly in the vehicle.
        /// </summary>
        /// <param name="npc">The npc that will be placed randomly in the vehicle.</param>
        public abstract void PlaceNPCRandomlyInVehicle(NPC n);

        /// <summary>
        /// Replace randomly the npcs already inside the vehicle.
        /// </summary>
        public abstract void ShuffleVehicleNPCs();

        public abstract void AddConductor(NPC conductor);

        /// <summary>
        /// Check if the vehicle is colliding with the texture t or its childs
        /// </summary>
        /// <param name="t">The texture that will be tested.</param>
        /// /// <param name="b">Check if t collides with the childs</param>
        /// <returns>true if this hit the hitbox of t or one of its childs, false otherwise.</returns>
        public override bool CollideWith(Texture t, bool b)
        {
            if (!b)
            {
                if (outside.CollideWith(t, false) || inside.CollideWith(t, false)) return true;
            }
            return base.CollideWith(t, b);
        }
    }
}
