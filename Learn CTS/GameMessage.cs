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
    public partial class GameMessage : UserControl
    {
        //List<string> list_messages = new List<string>();

        private GameMessage()
        {
            InitializeComponent();
            this.Tag = 0;
        }

        public GameMessage(string msg) : this()
        {
            lbl_msg.Text = msg;
        }

        public GameMessage(int x, int y, string msg) : this()
        {
            lbl_msg.Text = msg;
            this.Location = new Point(x, y);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            GameWindow.GetInstance().Controls.Remove(this);
            GameWindow.GetInstance().Focus();
            GameWindow.GetInstance().Refresh();
        }

        private void GameMessage_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
        }
    }
}
