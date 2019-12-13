using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class Phone : UserControl
    {

        /// <summary>
        /// Construct a phone control.
        /// </summary>
        public Phone()
        {
            InitializeComponent();
            flp_talk.Width = flp_talk.Width + SystemInformation.VerticalScrollBarWidth;
            flp_talk.BorderStyle = BorderStyle.FixedSingle;
            this.Tag = 0;
        }

        /// <summary>
        /// Add an answer to the phone.
        /// </summary>
        /// <param name="text">The message of the answer.</param>
        public void AddAnswer(string text)
        {
            RichTextBox r = new RichTextBox();
            r.BackColor = System.Drawing.Color.ForestGreen;
            r.Size = new System.Drawing.Size(flp_talk.Width / 2, 96);
            r.TabIndex = 1;
            r.Text = text;
            r.BorderStyle = BorderStyle.FixedSingle;
            using (Graphics g = CreateGraphics())
            {
                r.Height = (int)g.MeasureString(r.Text,
                    r.Font, r.Width).Height + 10;
            }
            flp_talk.Controls.Add(r);
            flp_talk.ScrollControlIntoView(r);
            flp_talk.Controls.SetChildIndex(r, 0);
        }

        /// <summary>
        /// Add the answer of the player.
        /// </summary>
        /// <param name="text">The message of the player answer.</param>
        public void AddMyAnswer(string text)
        {
            RichTextBox r = new RichTextBox();
            r.BackColor = System.Drawing.Color.White;
            r.Size = new System.Drawing.Size(flp_talk.Width/2, 96);
            r.TabIndex = 2;
            r.Text = text;
            r.BorderStyle = BorderStyle.FixedSingle;
            using (Graphics g = CreateGraphics())
            {
                r.Height = (int)g.MeasureString(r.Text,
                    r.Font, r.Width).Height + 10;
            }
            FlowLayoutPanel p = new FlowLayoutPanel();
            p.Controls.Add(r);
            p.Size = new System.Drawing.Size(flp_talk.Width-SystemInformation.VerticalScrollBarWidth-10, r.Height);
            p.BackColor = System.Drawing.Color.LightGray;
            p.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flp_talk.Controls.Add(p);
            flp_talk.Controls.SetChildIndex(p, 0);
            flp_talk.ScrollControlIntoView(p);
        }

        /// <summary>
        /// Load the phone.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Phone_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
            AddAnswer("Bonjour !");
        }

        /// <summary>
        /// Add the answer of the player.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void txt_answer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && ((TextBox)sender).Text != "")
            {
                AddMyAnswer(((TextBox)sender).Text);
                ((TextBox)sender).Text = "";
            }
        }
    }
}
