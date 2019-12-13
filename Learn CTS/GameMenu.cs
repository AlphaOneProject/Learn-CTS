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

        //Attributes
        private string game;
        private TextBox txtbox_name_player;
        private string displayed_menu;
        private Panel p_menu;

        /// <summary>
        /// Construct a game menu.
        /// </summary>
        /// <param name="game">The name of the game.</param>
        public GameMenu(string game)
        {
            InitializeComponent();
            this.game = game;
            this.Text = "Menu de "+game;
            this.BackgroundImage = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "mainmenu.png");
            this.pbox_return.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png");
            p_menu = new Panel();
            p_menu.Name = "panel_menu";
            p_menu.BackColor = Color.Black;
            p_menu.Size = new Size(550, this.Height);
            p_menu.Location = new Point(this.Width / 3, 0);
            this.Controls.Add(p_menu);
        }

        /// <summary>
        /// Load the game menu.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameMenu_Load(object sender, EventArgs e)
        {
            SetUpWindow();
            DisplayGameMenu();
        }

        /// <summary>
        /// Display the character menu if clicked.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void btn_launch_scenario_Click(object sender, EventArgs e)
        {
            DisplayCharacterMenu();
        }

        /// <summary>
        /// Close the game menu if clicked.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void button_leave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Display the character menu.
        /// </summary>
        private void DisplayCharacterMenu()
        {
            this.displayed_menu = "character_menu";
            p_menu.Controls.Clear();
            pbox_return.Visible = true;
            Label lbl_name_player = new Label();
            lbl_name_player.Text = "Veuillez entrer votre prénom/pseudo :";
            lbl_name_player.AutoSize = true;
            lbl_name_player.BackColor = Color.Transparent;
            lbl_name_player.ForeColor = Color.White;
            lbl_name_player.Name = "lbl_name_player";
            lbl_name_player.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            p_menu.Controls.Add(lbl_name_player);
            txtbox_name_player = new TextBox();
            txtbox_name_player.Text = "";
            txtbox_name_player.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            txtbox_name_player.Size = new Size(p_menu.Width - 40,p_menu.Height * 1/16);
            txtbox_name_player.Name = "txtbox_name_player";
            p_menu.Controls.Add(txtbox_name_player);
            ChooseCharacter cc = new ChooseCharacter(game);
            cc.Name = "ctrl_selection";
            p_menu.Controls.Add(cc);
            Button btn_confirm = new Button();
            SetupButton(btn_confirm);
            btn_confirm.Text = "Confirmer";
            btn_confirm.Click += new EventHandler(btn_confirm_Click);
            btn_confirm.AutoSize = true;
            btn_confirm.Name = "btn_confirm";
            p_menu.Controls.Add(btn_confirm);
            PerformLayout();
        }

        /// <summary>
        /// Confirm the name of the player and display the scenario menu.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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

        /// <summary>
        /// Display the scenario menu.
        /// </summary>
        private void DisplayScenarioMenu()
        {
            this.displayed_menu = "scenario_menu";
            p_menu.Controls.Clear();
            pbox_return.Visible = true;
            Label lbl_choice_scenario = new Label();
            lbl_choice_scenario.Text = "Quel scénario voulez-vous lancer ?";
            lbl_choice_scenario.Name = "lbl_choice_scenario";
            lbl_choice_scenario.AutoSize = true;
            lbl_choice_scenario.BackColor = Color.Transparent;
            lbl_choice_scenario.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_choice_scenario.ForeColor = Color.White;
            p_menu.Controls.Add(lbl_choice_scenario);
            string game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            string sc_path = game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            FlowLayoutPanel flp = new FlowLayoutPanel();
            Button btn;
            foreach (string dir in Directory.GetDirectories(@"" + sc_path))
            {
                btn = new Button();
                btn.Text = dir.Remove(0, sc_path.Length);
                SetupButton(btn);
                btn.Click += new EventHandler(LaunchGame);
                btn.AutoSize = true;
                flp.Controls.Add(btn);
            }
            flp.AutoSize = true;
            flp.Name = "flp";
            flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            p_menu.Controls.Add(flp);
            PerformLayout();
        }

        /// <summary>
        /// Launch the game.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void LaunchGame(object sender, EventArgs e)
        {
            p_menu.Controls.Clear();
            this.Hide();
            Button b = (Button)sender;
            Form game_window = new GameWindow(game,b.Text);
            game_window.Show();
        }

        /// <summary>
        /// Replace the controls when the form is resized.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameMenu_Resize(object sender, EventArgs e)
        {
            p_menu.Size = new Size(p_menu.Width, this.Height);
            PerformLayout();
        }

        /// <summary>
        /// Replace the controls if the menu displayed is the game menu.
        /// </summary>
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

        /// <summary>
        /// Replace the controls if the menu displayed is the character menu.
        /// </summary>
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
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, 24 + this.Height * 8 / 16);
                        break;
                }
            }
        }

        /// <summary>
        /// Replace the controls if the menu displayed is the scenario menu.
        /// </summary>
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
                        c.Location = new Point(p_menu.Width / 2 - c.Width / 2, this.Height * 5 / 16);
                        break;
                }
            }
        }

        /// <summary>
        /// Return to the main menu.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Visible) { Application.Restart(); }
        }

        /// <summary>
        /// Setup the window according to the options.json file.
        /// </summary>
        private void SetUpWindow()
        {
            JObject options = Tools.Get_From_JSON("internal" + Path.DirectorySeparatorChar + "options.json");
            if ((bool)options["maximized"])
            {
                this.WindowState = FormWindowState.Maximized;
            }
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Width = (int)options["size"]["x"];
            this.Height = (int)options["size"]["y"];
        }

        /// <summary>
        /// Display the game menu.
        /// </summary>
        private void DisplayGameMenu()
        {
            this.displayed_menu = "game_menu";
            p_menu.Controls.Clear();
            p_menu.Focus();
            pbox_return.Visible = false;
            Label lbl_name_game = new Label();
            lbl_name_game.AutoSize = true;
            lbl_name_game.Font = new System.Drawing.Font("Microsoft Sans Serif", 30.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_name_game.Name = "lbl_name_game";
            lbl_name_game.AutoSize = true;
            lbl_name_game.TabIndex = 0;
            lbl_name_game.Text = game;
            lbl_name_game.BackColor = Color.Transparent;
            lbl_name_game.ForeColor = Color.White;
            p_menu.Controls.Add(lbl_name_game);
            Button btn_launch_scenario = new Button();
            SetupButton(btn_launch_scenario);
            btn_launch_scenario.Name = "btn_launch_scenario";
            btn_launch_scenario.AutoSize = true;
            btn_launch_scenario.Text = "Lancer un scénario";
            btn_launch_scenario.Click += new System.EventHandler(this.btn_launch_scenario_Click);
            p_menu.Controls.Add(btn_launch_scenario);
            Button btn_options = new Button();
            SetupButton(btn_options);
            btn_options.Name = "btn_options";
            btn_options.Size = btn_launch_scenario.Size;
            btn_options.Text = "Options";
            p_menu.Controls.Add(btn_options);
            Button btn_leave = new Button();
            SetupButton(btn_leave);
            btn_leave.Name = "btn_leave";
            btn_leave.Size = btn_launch_scenario.Size;
            btn_leave.Text = "Quitter";
            btn_leave.Click += new System.EventHandler(this.button_leave_Click);
            p_menu.Controls.Add(btn_leave);
            PerformLayout();
        }

        /// <summary>
        /// Setup a button.
        /// </summary>
        /// <param name="b">The button that will be set up.</param>
        private void SetupButton(Button b)
        {
            b.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            b.UseVisualStyleBackColor = false;
            b.FlatAppearance.BorderSize = 1;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.ForeColor = Color.White;
            b.BackColor = Color.Black;
            b.MouseEnter += new EventHandler(delegate (object sender, EventArgs e) { this.BackColor = Color.LightSlateGray; });
            b.MouseLeave += new EventHandler(delegate (object sender, EventArgs e) { this.BackColor = Color.Black; });
        }

        /// <summary>
        /// Change the event of the return button according to the menu displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Return(object sender, EventArgs e)
        {
            switch (this.displayed_menu)
            {
                case "character_menu":
                    DisplayGameMenu();
                    break;
                case "scenario_menu":
                    DisplayCharacterMenu();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Display the menu according to the menu that has to be displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
