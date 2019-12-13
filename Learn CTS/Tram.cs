
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Tram : Vehicle
    {

        // Attributes

        Texture doors_left;
        Texture doors_right;

        private static int[] pos_doors = new int[]{
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
        public Tram(int x, int y) : base("tram", x, y)
        {
            this.doors_left = new Texture("tram_doors_left", "vehicle" + Path.DirectorySeparatorChar + "tram", this.GetX() + 424, this.GetY() + 112, this.GetY() + this.GetHeight() + 1, true);
            this.doors_right = new Texture("tram_doors_right", "vehicle" + Path.DirectorySeparatorChar + "tram", this.GetX() + 512, this.GetY() + 112, this.GetY() + this.GetHeight() + 1, true);
            this.AddChild(new Texture("tram_interior", "vehicle" + Path.DirectorySeparatorChar + "tram", this.GetX() + 480, this.GetY() + 208, true));
            this.AddChild(doors_left);
            this.AddChild(doors_right);
        }

        /// <summary>
        /// Get the doors of the tram
        /// </summary>
        /// <returns>The first element in the array is the texture of the left doors, the second one is the texture of the right door.</returns>
        public override Texture[] GetDoors()
        {
            return new Texture[] { this.doors_left, this.doors_right };
        }

        /// <summary>
        /// Get the position of the door related to the tram.
        /// </summary>
        /// <param name="i">The number of the door.</param>
        /// <returns>The position of the door related to the tram.</returns>
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
        /// Open slightly the doors of the tram
        /// </summary>
        /// <returns>true if the doors are opening. false otherwise.</returns>
        public override bool OpenDoors()
        {
            if (this.GetDoors()[0].GetX() >= this.GetX() + 348)
            {
                this.GetDoors()[0].Move(-2, 0);
                this.GetDoors()[1].Move(2, 0);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Close a bit the doors of the tram.
        /// </summary>
        /// <returns>true if the doors are closing. false otherwise.</returns>
        public override bool CloseDoors()
        {
            if (this.GetDoors()[0].GetX() < this.GetX() + 424)
            {
                this.GetDoors()[0].Move(2, 0);
                this.GetDoors()[1].Move(-2, 0);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Set a series of objectives for the npc to get on the tram.
        /// </summary>
        /// <param name="n">The npc which will enter the tram.</param>
        public override void SetPathNPCToVehicle(NPC n)
        {
            int i;
            int y;
            Random r = new Random();
            i = this.GetIndexNearestDoor(n.GetX());
            y = this.GetY() + this.GetHeight();
            n.SetObjective(this.GetPosDoor(i), y);
            n.SetObjectiveY(- r.Next(16, 30) + n.GetZ());
            if (i == 0)
            {
                n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-64, 256));
            }
            else if (i == this.GetNumberDoors() - 1)
            {
                n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, 64));
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
        /// Place the npc randomly in the tram.
        /// </summary>
        /// <param name="npc">The npc that will be placed randomly in the tram.</param>
        public override void PlaceNPCRandomlyInVehicle(NPC npc)
        {
            Random r = new Random();
            npc.SetX(this.GetX() + r.Next(492, this.GetWidth() - 492));
            npc.SetY(this.GetY() + this.GetHeight() - npc.GetHeight() - r.Next(16, 30));
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
                if (t.GetType().Name == "NPC" && ((NPC)t).GetQuiz()<1 && t.GetName() != "Conducteur")
                {
                    t.SetX(this.GetX() + r.Next(492, this.GetWidth() - 492));
                    t.SetY(this.GetY() + this.GetHeight() - t.GetHeight() - r.Next(16, 30));
                }
            }
        }

        public override void AddConductor(NPC conductor)
        {
            conductor.SetX(this.GetX() + this.GetWidth() - 192 - 100);
            conductor.SetY(this.GetY() + this.GetHeight() - 192 - 10);
            conductor.SetDirection(1);
            conductor.SetDefaultPose();
            conductor.SetZ(this.GetZ() - 1);
            this.AddChild(conductor);
        }
    }
}