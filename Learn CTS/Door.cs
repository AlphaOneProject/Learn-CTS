using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Door : Texture
    {
        private int etat = 0;

        private bool right;

        public Door(int x, int y, bool r) : base(x, y, -1999, false)
        {
            this.right = r;
        }

        public new void Update()
        {
            if (this.right && this.etat == 1)
            {

            }
            else
            {

            }
        }

        private int GetEtat()
        {
            return etat % 4;
        }

        public void ChangeEtat()
        {
            this.etat++;
        }

        public override void UpdateGraphic(PaintEventArgs e)
        {
            this.Update();
            Graphics g = e.Graphics;
            g.DrawImage(this.GetImage(), new Point(this.GetX(), this.GetY()));
        }
    }
}
