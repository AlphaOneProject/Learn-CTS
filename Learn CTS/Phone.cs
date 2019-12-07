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
        public Phone()
        {
            this.Location = new Point(200, 200);
            InitializeComponent();
            flp_talk.Width = flp_talk.Width + SystemInformation.VerticalScrollBarWidth;
        }

        public void AddAnswer(string text)
        {
            RichTextBox r = new RichTextBox();
            r.BackColor = System.Drawing.Color.ForestGreen;
            r.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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

        public void Addjsp(string text)
        {
            RichTextBox r = new RichTextBox();
            r.BackColor = System.Drawing.Color.White;
            r.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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

        private void Phone_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
            AddAnswer("coucou");
            Addjsp("hey");
            AddAnswer("comment ça va ?");
            Addjsp("ouais ça va et toi ?");
            AddAnswer("bieng merci");
            AddAnswer("blablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablablabla");
        }

        private void txt_answer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && ((TextBox)sender).Text != "")
            {
                Addjsp(((TextBox)sender).Text);
                ((TextBox)sender).Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((GameWindow)this.FindForm()).OpenClose_Backpack();
            this.FindForm().Controls.Remove(this);
        }
    }
}
