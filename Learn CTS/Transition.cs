using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Transition : Texture
    {

        private bool t = true;

        private float opacity = 0;

        public Transition(int w, int h) : base(0, 0)
        {
            Bitmap bmp = new Bitmap(w, h);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.FillRectangle(Brushes.Black, new Rectangle(0, 0, w, h));
            }
            this.SetImage((Image)bmp);
            this.SetZ(500000);
        }

        public void Update()
        {
            if (t)
            {
                this.SetImage(Tools.ChangeOpacity(this.GetImage(), opacity));
                opacity += (float)0.01;
                if (opacity >= 1) t = !t;
            }
            else
            {
                this.SetImage(Tools.ChangeOpacity(this.GetImage(), opacity));
                opacity -= (float)0.01;
            }
        }

        public bool HasFinished()
        {
            return opacity <= 0;
        }

        public override void OnPaint(PaintEventArgs e)
        {
            this.Update();
            base.OnPaint(e);
        }
    }
}
