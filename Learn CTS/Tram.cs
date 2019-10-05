
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Tram : Texture
    {

        private static string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        private List<Character> list_Passengers = new List<Character>();

        private int state = 2;

        private int max_speed = 25;

        private int speed = 25;

        private bool inside = false;

        private String path_image_inside;

        private String path_hitbox_inside;

        public Tram(int x, int y) : base(x, y, -2000, false)
        {
            this.path_image_inside = projectDir + "\\resources\\textures\\" + this.GetType().Name + "Inside.png";
            this.path_hitbox_inside = projectDir + "\\resources\\textures\\" + this.GetType().Name + "InsideHitBox.png";
        }

        public new void Update()
        {
            if (this.GetState() == 1 && this.speed < this.max_speed)
            {
                this.speed++;
                if(this.speed == this.max_speed)
                {
                    this.ChangeState();
                }
            }
            else if(this.GetState() == 3 && this.speed > 0)
            {
                this.speed--;
                if (this.speed == 0)
                {
                    this.ChangeState();
                }
            }
            this.Move(this.speed, 0);
            this.MovePassengers();
        }

        public void ChangeInsideOutside()
        {
            this.inside = !this.inside;
            if (this.inside)
            {
                this.SetImage(this.path_image_inside);
                this.SetHitbox(this.path_hitbox_inside);
            }
            else
            {
                this.SetImage(this.GetPathImage());
                this.SetHitbox(this.GetPathHitBox());
            }
        }

        public bool IsInside()
        {
            return this.inside;
        }

        public void MovePassengers()
        {
            if (this.list_Passengers.Count > 0)
            {
                //this.checkPresencePassengers();
                foreach (Character c in this.list_Passengers)
                {
                    c.Move(this.speed, 0);
                }
            }
        }

        public int GetMaxSpeed()
        {
            return this.max_speed;
        }

        public void SetSpeed(int v)
        {
            this.speed = v;
        }

        public void AddPassenger(Character c)
        {
            this.list_Passengers.Add(c);
        }

        /*private void checkPresencePassengers()
        {
            foreach(Character c in listPassengers)
            {
                if(c.getZ() > this.getY() + this.getImage().Height)
                {
                    this.listPassengers.Remove(c);
                }
            }
        }*/

        public void ChangeState()
        {
            this.state++;
        }

        public int GetState()
        {
            return this.state % 4;
        }

        public string GetStateString()
        {
            if(this.GetState() == 0)
            {
                return "A l'arret";
            }
            else if (this.GetState() == 1)
            {
                return "Demarrage";
            }
            else if (this.GetState() == 2)
            {
                return "En mouvement";
            }
            else
            {
                return "S'arrete";
            }
        }

        public int GetSpeed()
        {
            return this.speed;
        }

        public void SetState(int s)
        {
            this.state = s;
        }

        public override void UpdateGraphic(PaintEventArgs e)
        {
            this.Update();
            Graphics g = e.Graphics;
            g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
        }
    }
}
