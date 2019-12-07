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
        private JObject theme;

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
            lbl_name_player.BackColor = Color.Transparent;
            lbl_name_player.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            this.Controls.Add(lbl_name_player);
            txtbox_name_player = new TextBox();
            txtbox_name_player.Location = new Point(this.Width/2 - txtbox_name_player.Width/2, lbl_name_player.Location.Y + lbl_name_player.Height + 20);
            txtbox_name_player.KeyDown += new KeyEventHandler(txtbox_KeyDown);
            this.Controls.Add(txtbox_name_player);
            ChooseCharacter cc = new ChooseCharacter(game);
            cc.Location = new Point(this.Width / 2 - cc.Width / 2, txtbox_name_player.Location.Y + txtbox_name_player.Height + 10);
            this.Controls.Add(cc);
            Button btn = new Button();
            btn.Text = "Confirmer";
            btn.Click += new EventHandler(btn_confirm_Click);
            btn.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn.Location = new Point(this.Width/2 - btn.Width/2, cc.Location.Y + cc.Height + 10);
            btn.AutoSize = true;
            this.Controls.Add(btn);
        }

            /*flp_char = new FlowLayoutPanel();
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
            this.Controls.Add(flp_char);*/

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            Player.SetName(((TextBox)sender).Text);
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
            Label lbl_choice_scenario = new Label();
            lbl_choice_scenario.Text = "Quel scénario voulez-vous lancer ?";
            lbl_choice_scenario.Location = new Point(this.Width / 2 - lbl_choice_scenario.Width / 2, 10);
            lbl_choice_scenario.AutoSize = true;
            lbl_choice_scenario.BackColor = Color.Transparent;
            lbl_choice_scenario.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            this.Controls.Add(lbl_choice_scenario);
            string game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            string sc_path = game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            FlowLayoutPanel flp = new FlowLayoutPanel();
            Button btn;
            foreach (string dir in Directory.GetDirectories(@"" + sc_path))
            {
                btn = new Button();
                btn.Text = dir.Remove(0, sc_path.Length);
                btn.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
                btn.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
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
            this.Hide();
            Button b = (Button)sender;
            Form game_window = new GameWindow(game,b.Text);
            game_window.Show();
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
            if (this.Visible) { Application.Restart(); }
        }

        private void GameMenu_Load(object sender, EventArgs e)
        {
            SetUpWindow();
            Load_Menu_Buttons();
        }
        private void SetUpWindow()
        {
            JObject options = Tools.Get_From_JSON("internal" + Path.DirectorySeparatorChar + "options.json");
            this.theme = (JObject)Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal"
             + Path.DirectorySeparatorChar + "themes.json")[(string)options["theme"]];
            if ((bool)options["maximized"])
            {
                this.WindowState = FormWindowState.Maximized;
            }
            this.BackColor = Color.FromArgb(int.Parse((string)this.theme["0"]["R"]), int.Parse((string)this.theme["0"]["G"]), int.Parse((string)this.theme["0"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            lbl_name_game.BackColor = Color.Transparent;
            lbl_name_game.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            this.Width = (int)options["size"]["x"];
            this.Height = (int)options["size"]["y"];
        }

        private void Load_Menu_Buttons()
        {
            // 
            // btn_launch_scenario
            // 
            Button btn_launch_scenario = new Button();
            btn_launch_scenario.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_launch_scenario.Name = "btn_launch_scenario";
            btn_launch_scenario.Size = new System.Drawing.Size(179, 48);
            btn_launch_scenario.Location = new System.Drawing.Point(this.Width/2 - btn_launch_scenario.Width/2, lbl_name_game.Location.Y + lbl_name_game.Height + 100);
            btn_launch_scenario.TabIndex = 1;
            btn_launch_scenario.Text = "Lancer un scénario";
            btn_launch_scenario.UseVisualStyleBackColor = false;
            btn_launch_scenario.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_launch_scenario.Click += new System.EventHandler(this.btn_launch_scenario_Click);
            this.Controls.Add(btn_launch_scenario);
            // 
            // btn_options
            // 
            Button btn_options = new Button();
            btn_options.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_options.Location = new System.Drawing.Point(296, 222);
            btn_options.Name = "btn_options";
            btn_options.Size = new System.Drawing.Size(179, 48);
            btn_options.Location = new System.Drawing.Point(this.Width / 2 - btn_options.Width / 2, btn_launch_scenario.Location.Y + btn_launch_scenario.Height + btn_launch_scenario.Height/2);
            btn_options.TabIndex = 4;
            btn_options.Text = "Options";
            btn_options.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_options.UseVisualStyleBackColor = false;
            this.Controls.Add(btn_options);
            // 
            // btn_leave
            // 
            Button btn_leave = new Button();
            btn_leave.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_leave.Name = "btn_leave";
            btn_leave.Size = new System.Drawing.Size(179, 48);
            btn_leave.Location = new System.Drawing.Point(this.Width / 2 - btn_leave.Width / 2, btn_options.Location.Y + btn_options.Height + btn_options.Height / 2);
            btn_leave.TabIndex = 3;
            btn_leave.Text = "Quitter";
            btn_leave.UseVisualStyleBackColor = false;
            btn_leave.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_leave.Click += new System.EventHandler(this.button_leave_Click);
            this.Controls.Add(btn_leave);
        }

    }
}
