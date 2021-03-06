﻿using System;
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

        // Attributes
        private bool t = true;
        private int state = 0;
        private List<Image> list_transitions = new List<Image>();

        /// <summary>
        /// Construct a transition
        /// </summary>
        /// <param name="w">The width of the transition.</param>
        /// <param name="h">The height of the transition.</param>
        public Transition(int w, int h) : base(0, 0, 5000)
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

        /// <summary>
        /// Update the transition to fade in or fade out.
        /// </summary>
        public void Update()
        {
            if (t)
            {
                if (state < 10) state++;
            }
            else
            {
                if (state == 1)
                {
                    t = true;
                    GameWindow.GetInstance().RemoveTransition();
                }
                else
                {
                    state--;
                }
            }
        }

        /// <summary>
        /// Check if the transition has fade in totally.
        /// </summary>
        /// <returns></returns>
        public bool HasFinished()
        {
            return state>=10;
        }

        /// <summary>
        /// Set the transition to fade out.
        /// </summary>
        public void EndTransition()
        {
            t = !t;
        }

        /// <summary>
        /// Get the current image of the transition.
        /// </summary>
        /// <returns>The current image of the transition.</returns>
        public override Image GetImage()
        {
            if (list_transitions.Count == 0) return base.GetImage();
            return list_transitions[state];
        }

        /// <summary>
        /// Set the state of the transition.
        /// </summary>
        /// <param name="d">The state of the transition.</param>
        public void SetState(int state)
        {
            this.state = state;
        }

        /// <summary>
        /// Dispose the transition.
        /// </summary>
        public override void Dispose()
        {
            foreach (Image i in list_transitions) i.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Update the transition and paint it.
        /// </summary>
        /// <param name="e"></param>
        public override void OnPaint(PaintEventArgs e)
        {
            this.Update();
            base.OnPaint(e);
        }
    }
}
