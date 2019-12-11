using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Learn_CTS
{
    public partial class Backpack : UserControl
    {

        public Backpack()
        {
            InitializeComponent();
            this.Tag = 0;
        }

        private void Backpack_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            pbox_backpack.Image = Image.FromFile(path + "backpack.png");
            pbox_ticket.Image = Image.FromFile(path + "tickets.png");
            pbox_map.Image = Image.FromFile(path + "map.png");
            pbox_phone.Image = Image.FromFile(path + "smartphone.png");
            pbox_close.Image = Image.FromFile(path + "gamecard-delete-btn-x64.png");
            pbox_close.Click += new EventHandler(Backpack_Closed);
            this.Focus();
        }

        private void Backpack_Closed(object sender, EventArgs e)
        {
            ((GameWindow)this.FindForm()).OpenClose_Backpack();
        }

        private void Backpack_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.B) Backpack_Closed(sender, e);
        }

        private void pbox_phone_Click(object sender, EventArgs e)
        {
            this.FindForm().Controls.Add(new Phone());
            ((GameWindow)this.FindForm()).OpenClose_Backpack();
        }
    }
}
