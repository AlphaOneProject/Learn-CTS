
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Tram : Vehicule
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

        public Tram(int x, int y) : base("tram", x, y, pos_doors)
        {
            this.doors_left = new Texture("tram" + "DoorsLeft", "vehicule" + Path.DirectorySeparatorChar + "tram", this.GetX() + 424, this.GetY() + 112, this.GetY() + this.GetHeight() + 1, true);
            this.doors_right = new Texture("tram" + "DoorsRight", "vehicule" + Path.DirectorySeparatorChar + "tram", this.GetX() + 512, this.GetY() + 112, this.GetY() + this.GetHeight() + 1, true);
            this.AddChild(new Texture("tram" + "Interior", "vehicule" + Path.DirectorySeparatorChar + "tram", this.GetX() + 480, this.GetY() + 208, true));
            this.AddChild(doors_left);
            this.AddChild(doors_right);
            NPC conductor = NPC_Manager.GetInstance().CreateNPC("Conducteur", this.GetX() + this.GetWidth() - 192 - 100, this.GetY() + this.GetHeight() - 192 - 10);
            conductor.SetDirection(1);
            conductor.SetDefaultPose();
            conductor.SetZ(this.GetZ() - 1);
            this.AddChild(conductor);
        }

        public override Texture[] GetDoors()
        {
            return new Texture[] { this.doors_left, this.doors_right };
        }

        public override int GetPosDoor(int i)
        {
            return this.GetX() + pos_doors[i];
        }

        public override int GetNumberDoors()
        {
            return pos_doors.Length;
        }


        public override int GetIndexNearestDoor(int pos_c)
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

        public override void SetPathNPCToVehicule(NPC n)
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

        public override void PlaceNPCRandomlyInVehicle(NPC npc)
        {
            Random r = new Random();
            npc.SetX(this.GetX() + r.Next(492, this.GetWidth() - 492));
            npc.SetY(this.GetY() + this.GetHeight() - npc.GetHeight() - r.Next(16, 30));
            this.AddChild(npc);
        }

        public override void ShuffleVehiculeNPCs()
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
    }
}