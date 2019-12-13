using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Effects;

namespace Learn_CTS
{
    public partial class Menu : Form
    {
        /**
         * displayed_menu is meant to keep track which part of the menu is currently displaying.
         * "main_menu" is the main menu.
         * "games_menu" is the menu where the games are displayed.
         * "options" means that the options are displayed.
         */
        private string displayed_menu;

        /**
         * Default path of all the created games. 
         */
        private readonly string games_path;

        /**
         * Local copy of the options
         */
        private JObject options;

        /**
         * Local copy of the themes
         */
        private JObject themes;

        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Menu()
        {
            InitializeComponent();
            this.games_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar;
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Fonction called at the start of this form.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Menu_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            this.Activate();

            string options_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "options.json";
            // Set the window size as options size setting.
            if (!new FileInfo(options_path).Exists)
            {
                JObject options_setup = new JObject()
                {
                    ["theme"] = "dark",
                    ["maximized"] = false,
                    ["size"] = new JObject()
                    {
                        ["x"] = 900,
                        ["y"] = 600
                    }
                };
                Tools.Set_To_JSON(options_path, options_setup);
            }

            JObject options = Tools.Get_From_JSON(options_path);

            if ((bool)options["maximized"])
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.Width = int.Parse((string)options["size"]["x"]);
                this.Height = int.Parse((string)options["size"]["y"]);

                // Place the windows at the center of the screen.
                Rectangle pc_screen = Screen.FromControl(this).Bounds;
                this.Location = new Point((pc_screen.Width - this.Width) / 2, (pc_screen.Height - this.Height) / 2);
            }

            // Sets the options file as global variable.
            this.options = options;

            // Sets the themes file as gobal variable
            this.themes = Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "themes.json");

            // Place the windows at the center of the screen.
            Rectangle screen = Screen.FromControl(this).Bounds;
            this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2);

            // Sets displayed menu variable.
            this.displayed_menu = "main_menu";

            this.BackgroundImage = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "background.png");
        }

        /// <summary>
        /// On click, hides the main menu and displays the editor menu.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Main_menu_btn_edit_Click(object sender, EventArgs e)
        {
            Dispose_Controls(this);
            this.Controls.Clear();
            Display_games_menu();
        }

        /// <summary>
        /// Fetches all existing games into a list of GameCard.
        /// </summary>
        /// <returns>An ArrayList of GameCard</returns>
        private ArrayList Get_Games_List()
        {
            ArrayList game_card_list = new ArrayList();
            foreach (string s in Directory.GetDirectories(this.games_path))
            {
                GameCard gc = new GameCard(this);
                gc.Title = s.Remove(0, games_path.Length);
                gc.Description = Get_Var_From_JSON(s.ToString() + Path.DirectorySeparatorChar + "properties.json", "description");
                switch (Get_Var_From_JSON(s.ToString() + Path.DirectorySeparatorChar + "properties.json", "default"))
                {
                    case "True": gc.IsDefault = true;
                        break;
                    case "False": gc.IsDefault = false;
                        break;
                    default: break;
                }
                game_card_list.Add(gc);
            }
            return game_card_list;
        }

        /// <summary>
        /// Recover the content of a variable in a JSON file at a specified path.
        /// Cast this content as a string before returning it.
        /// </summary>
        /// <param name="internal_path">Path from the game folder to the targeted JSON file.</param>
        /// <param name="var_name">Variable name in the JSON file.</param>
        /// <returns></returns>
        public string Get_Var_From_JSON(string internal_path, string var_name)
        {
            string output = "";
            try
            {
                using (StreamReader stream_r = new StreamReader(internal_path))
                {
                    string json_file = stream_r.ReadToEnd();
                    JObject json_dict = JObject.Parse(json_file);
                    output = (string)json_dict[var_name];
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Le fichier "+internal_path+" est introuvable. Veuillez verifier qu'il existe.");
            }
            
            return output;
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_menu_btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Called when the main_menu_btn_options is clicked.
        /// Displays the options menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_menu_btn_options_Click(object sender, EventArgs e)
        {
            Dispose_Controls(this);
            this.Controls.Clear();
            Display_options_menu();
        }

        /// <summary>
        /// Called when a button to return to the main menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_to_main_menu(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "background.png");
            Dispose_Controls(this);
            this.Controls.Clear();

            Size old_size = this.Size;
            InitializeComponent();
            this.Size = old_size;
            PerformLayout();
            this.displayed_menu = "main_menu";
            Responsive_Resize_Main_Menu();
            this.RecreateHandle();
        }

        /// <summary>
        /// Creates all controls necessary to display the editor menu.
        /// </summary>
        private void Display_games_menu()
        {
            this.displayed_menu = "games_menu";

            // Creation of the panel parent to the the back to menu pb, and create game controls.
            Label games_menu_pnl_topbar = new Label()
            {
                Name = "games_menu_pnl_topbar",
                Location = new Point(-1, 0),
                Size = new Size(this.Width, 80),
                Text = "Mes jeux",
                Tag = 3,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(
                        (int)(this.themes[this.options["theme"].ToString()]["5"]["R"]),
                        (int)(this.themes[this.options["theme"].ToString()]["5"]["G"]),
                        (int)(this.themes[this.options["theme"].ToString()]["5"]["B"])
                    )
            };
            games_menu_pnl_topbar.BackColor = Color.FromArgb(
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pnl_topbar.Tag.ToString()]["R"]),
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pnl_topbar.Tag.ToString()]["G"]),
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pnl_topbar.Tag.ToString()]["B"])
                ); // Forcing theme change


            // Creation of the Button responsible to go back to the main menu.
            PictureBox games_menu_pb_back = new PictureBox()
            {
                Name = "games_menu_pb_back",
                Cursor = Cursors.Hand,
                Size = new Size(42, 42),
                Tag = 3,
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = games_menu_pnl_topbar
            };
            games_menu_pb_back.Click += new EventHandler(this.Back_to_main_menu);
            games_menu_pb_back.BackColor = Color.FromArgb(
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pb_back.Tag.ToString()]["R"]),
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pb_back.Tag.ToString()]["G"]),
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pb_back.Tag.ToString()]["B"])
                );

            // Creation of the Button responsible to create a game.
            PictureBox games_menu_pb_create_game = new PictureBox()
            {
                Name = "games_menu_pb_create_game",
                Cursor = Cursors.Hand,
                Size = new Size(42, 42),
                Tag = 3,
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = games_menu_pnl_topbar
            };
            games_menu_pb_create_game.Location = new Point(this.Width - 68, (games_menu_pnl_topbar.Height - games_menu_pb_create_game.Height) / 2);
            games_menu_pb_create_game.Click += new EventHandler(this.Display_create_game);
            games_menu_pb_create_game.BackColor = Color.FromArgb(
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pb_create_game.Tag.ToString()]["R"]),
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pb_create_game.Tag.ToString()]["G"]),
                    (int)(this.themes[this.options["theme"].ToString()][games_menu_pb_create_game.Tag.ToString()]["B"])
                );

            // Creation of the FlowLayoutPanel in which the games will be displayed as UserControls.
            FlowLayoutPanel games_menu_flp_games = new FlowLayoutPanel()
            {
                Size = new Size(this.Width - SystemInformation.VerticalScrollBarWidth, this.Height - games_menu_pnl_topbar.Height),
                Name = "games_menu_flp_games",
                Anchor = AnchorStyles.Top,
                Tag = 2,
                Padding = new Padding(this.Width / 8, 50, this.Width /8, 0),
                TabIndex = 3,
                AutoScroll = true
            };
            games_menu_flp_games.BackColor = Color.FromArgb(
                        (int)(this.themes[this.options["theme"].ToString()][games_menu_flp_games.Tag.ToString()]["R"]),
                        (int)(this.themes[this.options["theme"].ToString()][games_menu_flp_games.Tag.ToString()]["G"]),
                        (int)(this.themes[this.options["theme"].ToString()][games_menu_flp_games.Tag.ToString()]["B"])
                    );

            this.Controls.AddRange(new Control[] {
                games_menu_pb_back,
                games_menu_pb_create_game,
                games_menu_pnl_topbar,
                games_menu_flp_games
            });

            tlt_menu.SetToolTip(games_menu_pb_back, "Retour au menu principal");
            tlt_menu.SetToolTip(games_menu_pb_create_game, "Créer un nouveau jeu");

            // Fetching the existing games
            ArrayList games_list = Get_Games_List();
            foreach (GameCard gc in games_list)
            {
                games_menu_flp_games.Controls.Add(gc);
            }
            this.ResumeLayout();
        }

        /// <summary>
        /// Creates all controls necessary to display the options menu.
        /// </summary>
        private void Display_options_menu()
        {
            this.displayed_menu = "options_menu";

            this.BackgroundImage = null;

            Label options_menu_pnl_topbar = new Label()
            {
                Name = "options_menu_pnl_topbar",
                Location = new Point(-1, 0),
                Size = new Size(this.Width, 80),
                Text = "Options",
                Tag = 3,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };

            // Creation of the Button responsible to go back to the main menu.
            PictureBox options_menu_pb_back = new PictureBox()
            {
                Name = "options_menu_pb_back",
                Cursor = Cursors.Hand,
                Size = new Size(42, 42),
                Tag = 3,
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = options_menu_pnl_topbar
            };
            options_menu_pb_back.Location = new Point(6, (options_menu_pnl_topbar.Height - options_menu_pb_back.Height) / 2);
            options_menu_pb_back.Click += new EventHandler(this.Back_to_main_menu);

            Label options_menu_lbl_theme = new Label()
            {
                Name = "options_menu_lbl_theme",
                Font = new System.Drawing.Font("Nirmala UI Semilight", 14F),
                Tag = 5,
                Text = "Thème de couleurs : " + this.options["theme"].ToString(),
                AutoSize = true
            };

            JCS.ToggleSwitch options_menu_tgs_theme = new JCS.ToggleSwitch()
            {
                Style = JCS.ToggleSwitch.ToggleSwitchStyle.IOS5,
                TabIndex = 7,
                Tag = 0,
                Name = "options_menu_tgs_theme",
                Size = new Size(80, 30),
                ToggleOnButtonClick = true,
                ToggleOnSideClick = true,
            };
            if (this.options["theme"].ToString() == "dark") options_menu_tgs_theme.Checked = true;
            options_menu_tgs_theme.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(Options_Menu_Tgs_Theme_CheckedChanged);

            this.Controls.AddRange(new Control[] {
                options_menu_pb_back,
                options_menu_pnl_topbar,
                options_menu_tgs_theme,
                options_menu_lbl_theme
            });

            tlt_menu.SetToolTip(options_menu_pb_back, "Retour au menu principal");

            Change_Theme();
        }

        /// <summary>
        /// Called when the ToogleSwitch is changed, to directly update the colors of the current form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_Menu_Tgs_Theme_CheckedChanged(object sender, EventArgs e)
        {
            JCS.ToggleSwitch tgs = (JCS.ToggleSwitch)sender;
            // Récupération du fichier des options
            if (!tgs.Checked)
            {
                this.options["theme"] = "light";
                this.Controls.Find("options_menu_lbl_theme", false)[0].Text = "Thème de couleurs : light";
                tgs.SetRenderer(new JCS.ToggleSwitchIOS5Renderer()
                {

                });
            }
            else
            {
                this.options["theme"] = "dark";
                this.Controls.Find("options_menu_lbl_theme", false)[0].Text = "Thème de couleurs : dark";
                tgs.SetRenderer(new JCS.ToggleSwitchIOS5Renderer()
                {
                });
            }

            this.Change_Theme();

            // Mise à jour du fichier des options
            Tools.Set_To_JSON(AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "options.json", this.options);
        }

        /// <summary>
        /// Displays a custom box to create a game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_create_game(Object sender, EventArgs e)
        {
            GameCreator gc;
            if (Controls.Find("games_menu_game_creator", true).Length == 0)
            {
                gc = new GameCreator(this);
                gc.Name = "games_menu_game_creator";
                gc.Location = new Point(this.Width - gc.Width, this.Controls.Find("games_menu_pnl_topbar", true)[0].Height);
                gc.LostFocus += new EventHandler(GameCreator_Leave);
                this.Controls.Add(gc);
                gc.BringToFront();
            }
            else
            {
                gc = (GameCreator)Controls.Find("games_menu_game_creator", true)[0];
                gc.Dispose();
            }

            Change_Theme();
        }

        /// <summary>
        /// Called when the GameCreator loses focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameCreator_Leave(object sender, EventArgs e)
        {
            if (Controls.Find("games_menu_game_creator", true).Length != 0)
            {
                GameCreator gc = (GameCreator)Controls.Find("games_menu_game_creator", true)[0];
                gc.Dispose();
            }
        }

        /// <summary>
        /// Called when the size of the window has changed.
        /// Is used for responsive behavior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Layout(object sender, LayoutEventArgs e)
        {
            switch (this.displayed_menu)
            {
                case "main_menu":
                    Responsive_Resize_Main_Menu();
                    break;
                case "games_menu":
                    Responsive_Resize_games_menu();
                    break;
                case "options_menu":
                    Responsive_Resize_Options_Menu();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Called by Menu_Layout(), if the user is browsing the Games Menu.
        /// </summary>
        private void Responsive_Resize_games_menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "games_menu_pnl_topbar":
                        c.Width = this.Width;
                        break;
                    case "games_menu_pb_back":
                        c.Location = new Point(6, (this.Controls.Find("games_menu_pnl_topbar", false)[0].Height - c.Height) / 2);
                        break;
                    case "games_menu_pb_create_game":
                        c.Size = new Size(42, 42);
                        c.Location = new Point(this.Width - 68, (this.Controls.Find("games_menu_pnl_topbar", false)[0].Height - c.Height) / 2);
                        break;
                    case "games_menu_game_creator":
                        c.Location = new Point(this.Width - c.Width, this.Controls.Find("games_menu_pnl_topbar", false)[0].Height);
                        break;
                    case "games_menu_flp_games":
                        c.Location = new Point(0, 54);
                        c.Size = new Size(this.Width - SystemInformation.VerticalScrollBarWidth, this.Height - 54);
                        break;
                    case "games_menu_txt_name_game":
                        c.Location = new Point((this.Width / 2) - (c.Width / 2),
                                    (this.Height / 2) - c.Height - (c.Height * 2) - 12);
                        break;
                    case "games_menu_btn_confirm":
                        c.Location = new Point((this.Width / 2) - (c.Width / 2),
                            (this.Height / 2) - c.Height + (this.Controls.Find("games_menu_txt_name_game", false)[0].Height / 2) + 4);
                        break;
                }
            }
        }

        /// <summary>
        /// Called by Menu_SizeChanged(), if the user is browsing the Main Menu.
        /// </summary>
        private void Responsive_Resize_Main_Menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "main_menu_lbl_title1":
                        c.Location = new Point((this.Width / 2) - (c.Width) + 45,
                                    (this.Height / 2) - (c.Height * 2));
                        break;
                    case "main_menu_lbl_title2":
                        c.Location = new Point((this.Width / 2),
                                    (this.Height / 2) - (c.Height * 2) + 26);
                        break;
                    case "main_menu_btn_edit":
                        c.Location = new Point(0, this.Height - (this.Height / 8) * 3 - 38);
                        c.Size = new Size(this.Width, this.Height/8);
                        break;
                    case "main_menu_btn_options":
                        c.Location = new Point(0, this.Height - (this.Height / 8) * 2 - 38);
                        c.Size = new Size(this.Width, this.Height / 8);
                        break;
                    case "main_menu_btn_exit":
                        c.Location = new Point(0, this.Height - (this.Height / 8) - 38);
                        c.Size = new Size(this.Width, this.Height / 8);
                        break;
                }
            }
        }

        /// <summary>
        /// Called by Menu_SizeChanged(), if the user is browsing the Options Menu.
        /// </summary>
        private void Responsive_Resize_Options_Menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "options_menu_lbl_theme":
                        c.Location = new Point((int)((this.Width / 2) - c.Width - 15),
                            this.Height / 2 + c.Height / 2);
                        break;
                    case "options_menu_tgs_theme":
                        c.Location = new Point((this.Width / 2) + (c.Width / 2), (this.Height / 2) + (c.Height / 3));
                        break;
                    case "options_menu_pnl_topbar":
                        c.Width = this.Width;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Called when the form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                Save_Options();
            }
        }

        /// <summary>
        /// Saves the local copy of the options into the options file.
        /// </summary>
        public void Save_Options()
        {
            // Save current Form's size.
            string options_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                  Path.DirectorySeparatorChar + "options.json";
            JObject options = Tools.Get_From_JSON(options_path);
            options["maximized"] = (this.WindowState == FormWindowState.Maximized);
            JObject size = new JObject()
            {
                ["x"] = this.Width,
                ["y"] = this.Height
            };
            options["size"] = size;
            Tools.Set_To_JSON(options_path, options);
        }
    
        /// <summary>
        /// Recursively disposes all the controls of the form.
        /// </summary>
        /// <param name="control"></param>
        public void Dispose_Controls(Control control)
        {
            foreach (Control c in control.Controls)
            {
                Dispose_Controls(c);
            }
        }

        /// <summary>
        /// Change the theme of all the controls of this form, according to the current theme in the options.
        /// </summary>
        public void Change_Theme()
        {
            JObject themes = Tools.Get_From_JSON(AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "themes.json");

            foreach (Control c in this.Controls)
            {
                if (c.Tag != null)
                {
                    if (int.Parse(c.Tag.ToString()) == 5)
                    {
                        c.ForeColor = Color.FromArgb(
                            (int)(themes[this.options["theme"].ToString()][c.Tag.ToString()]["R"]),
                            (int)(themes[this.options["theme"].ToString()][c.Tag.ToString()]["G"]),
                            (int)(themes[this.options["theme"].ToString()][c.Tag.ToString()]["B"])
                        );
                    } else
                    {
                        c.ForeColor = Color.FromArgb(
                            (int)(themes[this.options["theme"].ToString()]["5"]["R"]),
                            (int)(themes[this.options["theme"].ToString()]["5"]["G"]),
                            (int)(themes[this.options["theme"].ToString()]["5"]["B"])
                        );
                        c.BackColor = Color.FromArgb(
                           (int)(themes[this.options["theme"].ToString()][c.Tag.ToString()]["R"]),
                           (int)(themes[this.options["theme"].ToString()][c.Tag.ToString()]["G"]),
                           (int)(themes[this.options["theme"].ToString()][c.Tag.ToString()]["B"])
                       );
                    }                  
                    
                }
            }

            this.BackColor = Color.FromArgb(
                        (int)(themes[this.options["theme"].ToString()][this.Tag.ToString()]["R"]),
                        (int)(themes[this.options["theme"].ToString()][this.Tag.ToString()]["G"]),
                        (int)(themes[this.options["theme"].ToString()][this.Tag.ToString()]["B"])
                    );
        }

        /// <summary>
        /// Returns the current theme of the application.
        /// </summary>
        /// <returns>JObject of the current theme.</returns>
        public JObject GetTheme()
        {
            return (JObject)this.themes[this.options["theme"].ToString()];
        }

    }
}
