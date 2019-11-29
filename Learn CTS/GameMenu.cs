using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class GameMenu : Form
    {
        private string game;
        private TextBox txtbox_name_player;
        private FlowLayoutPanel flp_char;

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
            lbl_name_player.Location = new Point(this.Width / 2 - lbl_name_player.Width / 2, 10);
            lbl_name_player.AutoSize = true;
            this.Controls.Add(lbl_name_player);
            txtbox_name_player = new TextBox();
            txtbox_name_player.Location = new Point(this.Width/2 - txtbox_name_player.Width/2, lbl_name_player.Location.Y + lbl_name_player.Height + 20);
            txtbox_name_player.KeyDown += new KeyEventHandler(txtbox_KeyDown);
            this.Controls.Add(txtbox_name_player);
            string character_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar;
            flp_char = new FlowLayoutPanel();
            PictureBox pbox;
            foreach (string dir in Directory.GetDirectories(@"" + character_path))
            {
                pbox = new PictureBox();
                pbox.Size = new Size(128, 128);
                pbox.SizeMode = PictureBoxSizeMode.StretchImage;
                pbox.Image = Image.FromFile(dir + Path.DirectorySeparatorChar + "1_0.png");
                pbox.Name = dir.Substring(character_path.Length, dir.Length-character_path.Length);
                pbox.Click += new EventHandler(SelectFolderChar);
                flp_char.Controls.Add(pbox);
            }
            flp_char.AutoSize = true;
            flp_char.AutoScroll = true;
            flp_char.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            flp_char.WrapContents = false;
            flp_char.Location = new Point(this.Width / 2 - flp_char.Width / 2, txtbox_name_player.Location.Y + txtbox_name_player.Height + 10);
            this.Controls.Add(flp_char);
            Button btn = new Button();
            btn.Text = "Confirmer";
            btn.Click += new EventHandler(btn_confirm_Click);
            btn.Location = new Point(this.Width/2 - btn.Width/2, flp_char.Location.Y + flp_char.Height + 10);
            btn.AutoSize = true;
            this.Controls.Add(btn);
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            Player.SetName(((TextBox)sender).Text);
        }

        private void SelectFolderChar(object sender, EventArgs e)
        {
            foreach(Control p in flp_char.Controls){
                ((PictureBox)p).BorderStyle = BorderStyle.None;
            }
            ((PictureBox)sender).BorderStyle = BorderStyle.Fixed3D;
            Player.SetFolder(((PictureBox)sender).Name);
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if(txtbox_name_player.Text == "")
            {
                MessageBox.Show("Vous n'avez pas choisi de prénom !");
            }
            else
            {
                Player.SetName(txtbox_name_player.Text);
                DisplayScenarioMenu();
            }
        }

        private void DisplayScenarioMenu()
        {
            this.Controls.Clear();
            string game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            string sc_path = game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            FlowLayoutPanel flp = new FlowLayoutPanel();
            Button btn;
            foreach (string dir in Directory.GetDirectories(@"" + sc_path))
            {
                btn = new Button();
                btn.Text = dir.Remove(0, sc_path.Length);
                btn.Click += new EventHandler(LaunchGame);
                btn.AutoSize = true;
                flp.Controls.Add(btn);
            }
            flp.AutoSize = true;
            flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flp.Location = new Point(this.Width/2 - flp.Width/2, (this.Height/2 - flp.Height/2)/2);
            this.Controls.Add(flp);
        }

        private void LaunchGame(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Label lbl_loading = new Label();
            lbl_loading.Text = "Chargement du jeu en cours";
            lbl_loading.AutoSize = true;
            lbl_loading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_loading.Location = new Point(this.Width / 2 - lbl_loading.Width / 2, this.Height / 2 - lbl_loading.Height / 2);
            this.Controls.Add(lbl_loading);
            Refresh();
            Button b = (Button)sender;
            Form game_window = new GameWindow(game,b.Text);
            game_window.Show();
            this.Hide();
        }

        private void GameMenu_Resize(object sender, EventArgs e)
        {
            foreach(Control c in this.Controls)
            {
                c.Location = new Point(this.Width / 2 - c.Width / 2, c.Location.Y);
            }
        }

        private void GameMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(GameWindow.GetInstance() == null) Application.Restart();
        }

        private void GameMenu_Load(object sender, EventArgs e)
        {
            SetUpWindow();
        }
        private void SetUpWindow()
        {
            JObject options = Tools.Get_From_JSON("internal" + Path.DirectorySeparatorChar + "options.json");
            if ((bool)options["maximized"])
            {
                this.WindowState = FormWindowState.Maximized;
            }
            this.Width = (int)options["size"]["x"];
            this.Height = (int)options["size"]["y"];
        }

    }
}
