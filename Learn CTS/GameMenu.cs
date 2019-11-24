using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class GameMenu : Form
    {
        private string game;

        public GameMenu(string game)
        {
            InitializeComponent();
            this.game = game;
            this.Text = "Menu de "+game;
            this.lbl_name_game.Text = game;
        }

        private void btn_launch_scenario_Click(object sender, EventArgs e)
        {
            DisplayNamePlayer();
        }

        private void button_leave_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Restart();
        }

        private void DisplayNamePlayer()
        {
            this.Controls.Clear();
            Label lbl_name_player = new Label();
            lbl_name_player.Text = "Veuillez entrer votre prénom";
            lbl_name_player.Location = new Point(10, 10);
            lbl_name_player.AutoSize = true;
            this.Controls.Add(lbl_name_player);
            TextBox txtbox_name_player = new TextBox();
            txtbox_name_player.Location = new Point(lbl_name_player.Location.X + lbl_name_player.Width + 10, lbl_name_player.Location.Y);
            txtbox_name_player.KeyDown += new KeyEventHandler(txtbox_KeyDown);
            this.Controls.Add(txtbox_name_player);
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (((TextBox)sender).Text != null && e.KeyCode == Keys.Enter)
            {
                DisplayScenarioMenu(((TextBox)sender).Text);
            }
        }

        private void DisplayScenarioMenu(string player_name)
        {
            this.Controls.Clear();
            Player.SetName(player_name);
            string game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            string sc_path = game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            FlowLayoutPanel flp = new FlowLayoutPanel();
            foreach (string dir in Directory.GetDirectories(@"" + sc_path))
            {
                Button btn = new Button();
                btn.Text = dir.Remove(0, sc_path.Length);
                btn.Click += new EventHandler(LaunchGame);
                btn.AutoSize = true;
                flp.Controls.Add(btn);
            }
            flp.AutoSize = true;
            flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flp.Location = new Point(this.Width/2 - flp.Width/2, this.Height/2 - flp.Height/2);
            this.Controls.Add(flp);
        }

        private void LaunchGame(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Form game_window = new GameWindow(game,b.Text);
            game_window.Show();
            this.Hide();
        }

        private void GameMenu_Resize(object sender, EventArgs e)
        {

        }
    }
}
