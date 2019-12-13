using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_CTS
{
    class Bus : Vehicle
    {

        // Attributes
        private static int[] pos_doors = new int[]{
            152,
            656,
            1416,
            2008
        };

        Texture doors_left;
        Texture doors_right;

        /// <summary>
        /// Constructor of a bus.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Bus(int x, int y) : base("bus", x, y)
        {
            this.doors_left = new Texture("bus_doors_left", "vehicle" + Path.DirectorySeparatorChar + "bus", this.GetX() + 56, this.GetY() + 88, this.GetY() + this.GetHeight() + 1, true);
            this.doors_right = new Texture("bus_doors_right", "vehicle" + Path.DirectorySeparatorChar + "bus", this.GetX() + 144, this.GetY() + 88, this.GetY() + this.GetHeight() + 1, true);
            this.AddChild(doors_left);
            this.AddChild(doors_right);
        }

        /// <summary>
        /// Get the doors of the bus
        /// </summary>
        /// <returns>The first element in the array is the texture of the left doors, the second one is the texture of the right door.</returns>
        public override Texture[] GetDoors()
        {
            return new Texture[] { this.doors_left, this.doors_right };
        }

        /// <summary>
        /// Get the position of the door related to the bus.
        /// </summary>
        /// <param name="i">The number of the door.</param>
        /// <returns>The position of the door related to the bus.</returns>
        public override int GetPosDoor(int i)
        {
            return this.GetX() + pos_doors[i];
        }

        /// <summary>
        /// Get the total number of doors.
        /// </summary>
        /// <returns>The number of doors.</returns>
        public override int GetNumberDoors()
        {
            return pos_doors.Length;
        }

        /// <summary>
        /// Open slightly the doors of the bus
        /// </summary>
        /// <returns>true if the doors are opening. false otherwise.</returns>
        public override bool OpenDoors()
        {
            if (this.doors_left.GetX() >= this.GetX() + 56 - 112)
            {
                this.doors_left.Move(-2, 0);
                this.doors_right.Move(2, 0);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Close a bit the doors of the bus.
        /// </summary>
        /// <returns>true if the doors are closing. false otherwise.</returns>
        public override bool CloseDoors()
        {
            if (this.doors_left.GetX() < this.GetX() + 56)
            {
                this.doors_left.Move(2, 0);
                this.doors_right.Move(-2, 0);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Set a series of objectives for the npc to get on the bus.
        /// </summary>
        /// <param name="n">The npc which will enter the bus.</param>
        public override void SetPathNPCToVehicle(NPC n)
        {
            int i;
            int y;
            Random r = new Random();
            i = this.GetIndexNearestDoor(n.GetX());
            y = this.GetY() + this.GetHeight();
            n.SetObjective(this.GetPosDoor(i), y);
            n.SetObjectiveY(-r.Next(56, 64) + n.GetZ());
            if (i == 0)
            {
                n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(0, 256));
            }
            else if (i == this.GetNumberDoors() - 1)
            {
                n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, -64));
            }
            else if (i % 2 == 0)
            {
                n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-384, 256));
            }
            else
            {
                n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, 384));
            }
        }

        /// <summary>
        /// Place the npc randomly in the bus.
        /// </summary>
        /// <param name="npc">The npc that will be placed randomly in the bus.</param>
        public override void PlaceNPCRandomlyInVehicle(NPC npc)
        {
            Random r = new Random();
            npc.SetX(this.GetX() + r.Next(78, this.GetWidth() - 256));
            npc.SetY(this.GetY() + this.GetHeight() - npc.GetHeight() - r.Next(56, 64));
            this.AddChild(npc);
        }

        /// <summary>
        /// Replace randomly the npcs already inside the vehicle.
        /// </summary>
        public override void ShuffleVehicleNPCs()
        {
            Random r = new Random();
            foreach (Texture t in this.GetListChilds())
            {
                if (t.GetType().Name == "NPC" && ((NPC)t).GetQuiz() < 1 && t.GetName() != "Conducteur")
                {
                    t.SetX(this.GetX() + r.Next(78, this.GetWidth() - 256));
                    t.SetY(this.GetY() + this.GetHeight() - t.GetHeight() - r.Next(56, 64));
                }
            }
        }

        public override void AddConductor(NPC conductor)
        {
            conductor.SetX(this.GetX() + this.GetWidth() - 192 - 10);
            conductor.SetY(this.GetY() + this.GetHeight() - 192 - 60);
            conductor.SetDirection(1);
            conductor.SetDefaultPose();
            conductor.SetZ(this.GetZ() + 1);
            this.AddChild(conductor);
        }
    }
}
