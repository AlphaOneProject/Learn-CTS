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

        private int d = 0;

        private List<Image> list_transitions = new List<Image>();

        public Transition(int w, int h) : base(0, 0)
        {
            Bitmap bmp = new Bitmap(w, h);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.FillRectangle(Brushes.Black, new Rectangle(0, 0, w, h));
            }
            this.SetImage((Image)bmp);
            this.SetZ(500000);
            list_transitions.Add(this.GetImage());
            for(int i = 1; i<=10; i++)
            {
                list_transitions.Add(Tools.ChangeOpacity(this.GetImage(), (float)i/10));
            }
        }

        public void Update()
        {
            if (t)
            {
                if (d < 10) d++;
            }
            else
            {
                if (d == 1)
                {
                    t = true;
                    GameWindow.GetInstance().RemoveTransition();
                }
                else
                {
                    d--;
                }
            }
        }

        public bool HasFinished()
        {
            return d>=10;
        }

        public void EndTransition()
        {
            t = !t;
        }

        public override Image GetImage()
        {
            if (list_transitions.Count == 0) return base.GetImage();
            return list_transitions[d];
        }

        public void SetD(int d)
        {
            this.d = d;
        }

        public override void Dispose()
        {
            foreach (Image i in list_transitions) i.Dispose();
            base.Dispose();
        }

        public override void OnPaint(PaintEventArgs e)
        {
            this.Update();
            base.OnPaint(e);
        }
    }
}
