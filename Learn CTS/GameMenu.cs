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
        private string displayed_menu;
        private Panel p_menu;

        public GameMenu(string game)
        {
            InitializeComponent();
            this.game = game;
            this.Text = "Menu de "+game;
            this.BackgroundImage = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "menu" + Path.DirectorySeparatorChar + "mainmenu.png");
            p_menu = new Panel();
            p_menu.Name = "panel_black";
            p_menu.BackColor = Color.Black;
            p_menu.Size = new Size(this.Width / 5, this.Height);
            p_menu.Location = new Point(this.Width / 5, 0);
            this.Controls.Add(p_menu);
        }

        private void btn_launch_scenario_Click(object sender, EventArgs e)
        {
            DisplayCharacterMenu();
        }

        private void button_leave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayCharacterMenu()
        {
            this.displayed_menu = "character_menu";
            p_menu.Controls.Clear();
            Label lbl_name_player = new Label();
            lbl_name_player.Text = "Veuillez entrer votre prénom/pseudo :";
            lbl_name_player.AutoSize = true;
            lbl_name_player.BackColor = Color.Transparent;
            lbl_name_player.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            lbl_name_player.Name = "lbl_name_player";
            lbl_name_player.Location = new Point(p_menu.Width / 4, 10);
            p_menu.Controls.Add(lbl_name_player);
            txtbox_name_player = new TextBox();
            txtbox_name_player.Location = new Point(p_menu.Width / 4, lbl_name_player.Location.Y + lbl_name_player.Height + 20);
            txtbox_name_player.KeyDown += new KeyEventHandler(txtbox_KeyDown);
            txtbox_name_player.Name = "txtbox_name_player";
            p_menu.Controls.Add(txtbox_name_player);
            ChooseCharacter cc = new ChooseCharacter(game);
            cc.Location = new Point(p_menu.Width / 4, txtbox_name_player.Location.Y + txtbox_name_player.Height + 10);
            cc.Name = "ctrl_selection";
            p_menu.Controls.Add(cc);
            Button btn_confirm = new Button();
            btn_confirm.Text = "Confirmer";
            btn_confirm.Click += new EventHandler(btn_confirm_Click);
            btn_confirm.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_confirm.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_confirm.Location = new Point(p_menu.Width / 4, cc.Location.Y + cc.Height + 10);
            btn_confirm.AutoSize = true;
            btn_confirm.Name = "btn_confirm";
            p_menu.Controls.Add(btn_confirm);
            PerformLayout();
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            Player.SetName(((TextBox)sender).Text);
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if(txtbox_name_player.Text == "")
            {
                MessageBox.Show("Vous n'avez pas choisi de prénom/pseudo !");
            }
            else
            {
                Player.SetName(txtbox_name_player.Text);
                DisplayScenarioMenu();
            }
        }

        private void DisplayScenarioMenu()
        {
            this.displayed_menu = "scenario_menu";
            p_menu.Controls.Clear();
            Label lbl_choice_scenario = new Label();
            lbl_choice_scenario.Text = "Quel scénario voulez-vous lancer ?";
            lbl_choice_scenario.Name = "lbl_choice_scenario";
            lbl_choice_scenario.Location = new Point(p_menu.Width / 4, 10);
            lbl_choice_scenario.AutoSize = true;
            lbl_choice_scenario.BackColor = Color.Transparent;
            lbl_choice_scenario.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            p_menu.Controls.Add(lbl_choice_scenario);
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
            flp.Name = "flp";
            flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flp.Location = new Point(this.Width/6, (this.Height/2 - flp.Height/2)/2);
            p_menu.Controls.Add(flp);
            PerformLayout();
        }

        private void LaunchGame(object sender, EventArgs e)
        {
            p_menu.Controls.Clear();
            this.Hide();
            Button b = (Button)sender;
            Form game_window = new GameWindow(game,b.Text);
            game_window.Show();
        }

        private void GameMenu_Resize(object sender, EventArgs e)
        {
            p_menu.Size = new Size(this.Width / 5, this.Height);
            PerformLayout();
        }

        private void Responsive_Resize_Game_Menu()
        {
            foreach(Control c in p_menu.Controls)
            {
                switch (c.Name)
                {
                    case "lbl_name_game":
                        c.Location = new System.Drawing.Point((p_menu.Width - c.Width)/2, this.Height * 3 / 16);
                        break;
                    case "btn_launch_scenario":
                        c.Location = new System.Drawing.Point((p_menu.Width - c.Width) / 2, this.Height * 6 / 16);
                        break;
                    case "btn_options":
                        c.Location = new System.Drawing.Point((p_menu.Width - c.Width) / 2, this.Height * 8 / 16);
                        break;
                    case "btn_leave":
                        c.Location = new System.Drawing.Point((p_menu.Width - c.Width) / 2, this.Height * 10 / 16);
                        break;
                }
            }
        }

        private void Responsive_Resize_Character_Menu()
        {
            foreach (Control c in p_menu.Controls)
            {
                switch (c.Name)
                {
                    case "lbl_name_player":
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, this.Height * 3 / 16);
                        break;
                    case "txtbox_name_player":
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, this.Height * 4 / 16);
                        break;
                    case "ctrl_selection":
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, this.Height * 5 / 16);
                        break;
                    case "btn_confirm":
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, 24 + this.Height * 7 / 16);
                        break;
                }
            }
        }

        private void Responsive_Resize_Scenario_Menu()
        {
            foreach (Control c in p_menu.Controls)
            {
                switch (c.Name)
                {
                    case "lbl_choice_scenario":
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, this.Height * 3 / 16);
                        break;
                    case "flp":
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, this.Height * 4 / 16);
                        break;
                }
            }
        }

        private void GameMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Visible) { Application.Restart(); }
        }

        private void GameMenu_Load(object sender, EventArgs e)
        {
            SetUpWindow();
            DisplayGameMenu();
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
            this.Width = (int)options["size"]["x"];
            this.Height = (int)options["size"]["y"];
        }

        private void DisplayGameMenu()
        {
            this.displayed_menu = "game_menu";
            // 
            // lbl_name_game
            // 
            Label lbl_name_game = new Label();
            lbl_name_game.AutoSize = true;
            lbl_name_game.Font = new System.Drawing.Font("Microsoft Sans Serif", 30.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_name_game.Name = "lbl_name_game";
            lbl_name_game.AutoSize = true;
            lbl_name_game.TabIndex = 0;
            lbl_name_game.Text = game;
            lbl_name_game.BackColor = Color.FromArgb(int.Parse((string)this.theme["1"]["R"]), int.Parse((string)this.theme["1"]["G"]), int.Parse((string)this.theme["1"]["B"]));
            lbl_name_game.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            lbl_name_game.Location = new System.Drawing.Point(p_menu.Width / 4, this.Height * 1 / 6);
            p_menu.Controls.Add(lbl_name_game);
            // 
            // btn_launch_scenario
            // 
            Button btn_launch_scenario = new Button();
            btn_launch_scenario.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn_launch_scenario.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_launch_scenario.Name = "btn_launch_scenario";
            btn_launch_scenario.AutoSize = true;
            btn_launch_scenario.TabIndex = 1;
            btn_launch_scenario.Text = "Lancer un scénario";
            btn_launch_scenario.UseVisualStyleBackColor = false;
            btn_launch_scenario.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_launch_scenario.Click += new System.EventHandler(this.btn_launch_scenario_Click);
            btn_launch_scenario.Location = new System.Drawing.Point(p_menu.Width / 4, lbl_name_game.Location.Y + lbl_name_game.Height + this.Height * 2 / 16);
            p_menu.Controls.Add(btn_launch_scenario);
            // 
            // btn_options
            // 
            Button btn_options = new Button();
            btn_options.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn_options.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_options.Name = "btn_options";
            btn_options.Size = btn_launch_scenario.Size;
            btn_options.TabIndex = 4;
            btn_options.Text = "Options";
            btn_options.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_options.UseVisualStyleBackColor = false;
            btn_options.Location = new System.Drawing.Point(p_menu.Width / 4, btn_launch_scenario.Location.Y + btn_launch_scenario.Height + this.Height * 1 / 16);
            p_menu.Controls.Add(btn_options);
            // 
            // btn_leave
            // 
            Button btn_leave = new Button();
            btn_leave.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn_leave.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            btn_leave.Name = "btn_leave";
            btn_leave.Size = btn_launch_scenario.Size;
            btn_leave.TabIndex = 3;
            btn_leave.Text = "Quitter";
            btn_leave.UseVisualStyleBackColor = false;
            btn_leave.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
            btn_leave.Click += new System.EventHandler(this.button_leave_Click);
            btn_leave.Location = new System.Drawing.Point(p_menu.Width / 4, btn_options.Location.Y + btn_options.Height + this.Height * 1 / 16);
            p_menu.Controls.Add(btn_leave);
            PerformLayout();
        }

        private void GameMenu_Layout(object sender, LayoutEventArgs e)
        {
            switch (this.displayed_menu)
            {
                case "game_menu":
                    Responsive_Resize_Game_Menu();
                    break;
                case "character_menu":
                    Responsive_Resize_Character_Menu();
                    break;
                case "scenario_menu":
                    Responsive_Resize_Scenario_Menu();
                    break;
                default:
                    break;
            }
        }
    }
}
