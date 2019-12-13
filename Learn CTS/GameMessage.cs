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

        /// <summary>
        /// Construct a game message.
        /// </summary>
        private GameMessage()
        {
            InitializeComponent();
            this.Tag = 0;
        }

        /// <summary>
        /// Construct a game message with a message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public GameMessage(string msg) : this()
        {
            lbl_msg.Text = msg;
        }

        /// <summary>
        /// Remove the game message in the game window.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            GameWindow.GetInstance().Controls.Remove(this);
            GameWindow.GetInstance().Focus();
            GameWindow.GetInstance().Refresh();
        }

        /// <summary>
        /// Load the game message.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameMessage_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
        }
    }
}
