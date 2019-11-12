using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
        private String displayed_menu;

        /**
         * Default path of all the created games. 
         */
        private readonly String games_path;

        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Menu()
        {
            InitializeComponent();
            this.games_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar;
            this.DoubleBuffered = false;
            this.Activate();
        }

        /// <summary>
        /// Fonction called at the start of this form.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Menu_Load(object sender, EventArgs e)
        {
            // Set the window size as options size setting.
            string options_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                  Path.DirectorySeparatorChar + "options.json";
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

            // Place the windows at the center of the screen.
            Rectangle screen = Screen.FromControl(this).Bounds;
            this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2);

            this.displayed_menu = "main_menu";

            // Dynamically places the controls of the main menu.
            main_menu_lbl_title1.Location = new Point((this.Width / 2) - (main_menu_lbl_title1.Width) + 15,
                                    (this.Height / 2) - (main_menu_lbl_title1.Height*2));

            main_menu_lbl_title2.Location = new Point(main_menu_lbl_title1.Location.X + main_menu_lbl_title1.Width - 20,
                                    main_menu_lbl_title1.Location.Y - 10);

            main_menu_btn_edit.Location = new Point((this.Width / 2) - (main_menu_btn_edit.Width / 2),
                            (this.Height / 2) - main_menu_btn_edit.Height + (main_menu_lbl_title1.Height / 2) + 2);

            main_menu_btn_options.Location = new Point(main_menu_btn_edit.Location.X,
                (this.Height / 2) + (main_menu_btn_edit.Height / 2) + 10);

            main_menu_btn_exit.Location = new Point((this.Width / 2) + 6, (this.Height / 2) + (main_menu_btn_edit.Height / 2) + 10);
        }

        /// <summary>
        /// On click, hides the main menu and displays the editor menu.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Main_menu_btn_edit_Click(object sender, EventArgs e)
        {
            this.Dispose_Images();
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
                GameCard gc = new GameCard();
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
            this.Dispose_Images();
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
            this.Dispose_Images();
            this.Controls.Clear();
            Display_Main_Menu();
        }

        /// <summary>
        /// Creates all controls necessary to display the main menu.
        /// </summary>
        private void Display_Main_Menu()
        {
            displayed_menu = "main_menu";

            // Creation of the first part of the title
            Label main_menu_lbl_title1_dyna = new Label()
            {
                Font = new System.Drawing.Font("Nirmala UI Semilight", 36F),
                Location = new System.Drawing.Point(276, 31),
                Name = "main_menu_lbl_title1",
                Size = new System.Drawing.Size(141, 65),
                Text = "Learn"
            };
            main_menu_lbl_title1_dyna.Location = new Point((this.Width / 2) - (main_menu_lbl_title1_dyna.Width) + 15,
                                (this.Height / 2) - (main_menu_lbl_title1_dyna.Height*2));

            // Creation of the second part of the title
            Label main_menu_lbl_title2_dyna = new Label()
            {
                Font = new Font("Nirmala UI", 44F, System.Drawing.FontStyle.Bold),
                ForeColor = Color.Red,
                Name = "main_menu_lbl_title2",
                Size = new Size(138, 78),
                Text = "CTS"
            };
            main_menu_lbl_title2_dyna.Location = new Point(main_menu_lbl_title1_dyna.Location.X + main_menu_lbl_title1_dyna.Width - 20,
                            main_menu_lbl_title1_dyna.Location.Y - 10);

            // Creation of the button that will show the selection of available levels to edit.
            Button main_menu_btn_edit_dyna = new Button()
            {
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Name = "main_menu_btn_edit",
                Cursor = Cursors.Hand,
                Size = new Size(230, 60),
                TabIndex = 2,
                Text = "Mes jeux"
            };
            main_menu_btn_edit_dyna.Location = new Point((this.Width / 2) - (main_menu_btn_edit_dyna.Width / 2),
                    (this.Height / 2) - main_menu_btn_edit_dyna.Height + (main_menu_lbl_title1_dyna.Height / 2) + 2);
            main_menu_btn_edit_dyna.Click += new EventHandler(this.Main_menu_btn_edit_Click);

            // Creation of the button that will exit the application.
            Button main_menu_btn_exit_dyna = new Button() {
                Location = new Point((this.Width / 2) + 6, (this.Height / 2) + (main_menu_btn_edit_dyna.Height / 2) + 10),
                Name = "main_menu_btn_exit",
                Cursor = Cursors.Hand,
                Size = new Size(109, 30),
                TabIndex = 4,
                Text = "Quitter"
            };
            main_menu_btn_exit_dyna.Click += new EventHandler(this.Main_menu_btn_exit_Click);

            // Creation of the button that displays options.
            Button main_menu_btn_options_dyna = new Button()
            {
                Name = "main_menu_btn_options",
                Cursor = Cursors.Hand,
                Size = new Size(109, 30),
                TabIndex = 3,
                Text = "Options"
            };
            main_menu_btn_options_dyna.Location = new Point(main_menu_btn_edit_dyna.Location.X,
                (this.Height / 2) + (main_menu_btn_edit_dyna.Height / 2) + 10);
            main_menu_btn_options_dyna.Click += new EventHandler(this.Main_menu_btn_options_Click);

            this.Controls.AddRange(new Control[] { main_menu_lbl_title2_dyna, main_menu_lbl_title1_dyna, main_menu_btn_edit_dyna, main_menu_btn_exit_dyna, main_menu_btn_options_dyna });
        }

        /// <summary>
        /// Creates all controls necessary to display the editor menu.
        /// </summary>
        private void Display_games_menu()
        {
            this.displayed_menu = "games_menu";

            this.SuspendLayout();

            // Creation of the panel parent to the the back to menu pb, and create game controls.
            Label games_menu_pnl_topbar = new Label()
            {
                Name = "games_menu_pnl_topbar",
                Location = new Point(0, 0),
                Size = new Size(this.Width, 54),
                Text = "Mes jeux",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0)
            };

            // Creation of the Button responsible to go back to the main menu.
            PictureBox games_menu_pb_back_to_main_menu = new PictureBox()
            {
                Name = "games_menu_pb_back_to_main_menu",
                Cursor = Cursors.Hand,
                Size = new Size(42, 42),
                Location = new Point(6, 6),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = games_menu_pnl_topbar
            };
            games_menu_pb_back_to_main_menu.Click += new EventHandler(this.Back_to_main_menu);

            // Creation of the Button responsible to create a game.
            PictureBox games_menu_pb_create_game = new PictureBox()
            {
                Name = "games_menu_pb_create_game",
                Cursor = Cursors.Hand,
                Size = new Size(42, 42),
                Location = new Point(this.Width - 68, 6),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = games_menu_pnl_topbar
            };
            games_menu_pb_create_game.Click += new EventHandler(this.Display_create_game);

            // Creation of the FlowLayoutPanel in which the games will be displayed as UserControls.
            FlowLayoutPanel games_menu_flp_games = new FlowLayoutPanel()
            {
                Size = new Size(this.Width - SystemInformation.VerticalScrollBarWidth, this.Height - games_menu_pnl_topbar.Height),
                Location = new Point(0, games_menu_pnl_topbar.Height),
                Name = "games_menu_flp_games",
                TabIndex = 3,
                AutoScroll = true
            };

            this.Controls.AddRange(new Control[] {
                games_menu_pb_back_to_main_menu,
                games_menu_pb_create_game,
                games_menu_pnl_topbar,
                games_menu_flp_games
            });

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

            // Creation of the Button responsible to go back to the main menu.
            PictureBox options_menu_pb_back_to_main_menu = new PictureBox()
            {
                Name = "options_menu_pb_back_to_main_menu",
                Cursor = Cursors.Hand,
                Size = new Size(42, 42),
                Location = new Point(6, 6),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            options_menu_pb_back_to_main_menu.Click += new EventHandler(this.Back_to_main_menu);

            this.Controls.Add(options_menu_pb_back_to_main_menu);
            // Amuse toi bien Antoine :-*

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
                gc = new GameCreator();
                gc.Name = "games_menu_game_creator";
                gc.Location = new Point(this.Width - gc.Width, this.Controls.Find("games_menu_pnl_topbar", true)[0].Height);
                this.Controls.Add(gc);
                gc.BringToFront();
            }
            else
            {
                gc = (GameCreator)Controls.Find("games_menu_game_creator", true)[0];
                gc.Dispose();
            }
            
        }

        private void Menu_SizeChanged(object sender, EventArgs e)
        {
            switch (this.displayed_menu)
            {
                case "main_menu": Responsive_Resize_Main_Menu();
                    break;
                case "games_menu": Responsive_Resize_games_menu();
                    break;
                case "options_menu": Responsive_Resize_Options_Menu();
                    break;
                default:
                    break;
            }
        }

        private void Responsive_Resize_games_menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "games_menu_pnl_topbar":
                        c.Width = this.Width;
                        break;
                    case "games_menu_pb_create_game":
                        c.Size = new Size(42, 42);
                        c.Location = new Point(this.Width - 68, 6);
                        break;
                    case "games_menu_game_creator":
                        c.Location = new Point(this.Width - c.Width, this.Controls.Find("games_menu_pnl_topbar", true)[0].Height);
                        break;
                    case "games_menu_flp_games":
                        c.Location = new Point(0, this.Controls.Find("games_menu_pnl_topbar", false)[0].Height);
                        c.Size = new Size(this.Width - SystemInformation.VerticalScrollBarWidth, this.Height - this.Controls.Find("games_menu_pnl_topbar", false)[0].Height);
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

        private void Responsive_Resize_Main_Menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "main_menu_lbl_title1":
                        c.Location = new Point((this.Width / 2) - (c.Width) + 14,
                                    (this.Height / 2) - (c.Height * 2));
                        break;
                    case "main_menu_lbl_title2":
                        c.Location = new Point((this.Width / 2) - 5,
                                    (this.Height / 2) - (c.Height * 2) + 16);
                        break;
                    case "main_menu_btn_edit":
                        c.Location = new Point((this.Width / 2) - (c.Width / 2),
                            (this.Height / 2) - c.Height + (this.Controls.Find("main_menu_lbl_title1", false)[0].Height / 2) + 2);
                        break;
                    case "main_menu_btn_options":
                        c.Location = new Point((this.Width / 2) - c.Width - 6,
                            (this.Height / 2) + (this.Controls.Find("main_menu_btn_edit", false)[0].Height / 2) + 10);
                        break;
                    case "main_menu_btn_exit":
                        c.Location = new Point((this.Width / 2) + 6,
                            (this.Height / 2) + (this.Controls.Find("main_menu_btn_edit", false)[0].Height / 2) + 10);
                        break;
                }
            }
        }

        private void Responsive_Resize_Options_Menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "salut":
                        // Salut Antoine
                        // Salut !
                        // Cool comme fonction dis-donc !
                        break;
                    default:
                        break;
                }
            }
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible)
            {
                Save_Options();
            }
        }

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
    
        public void Dispose_Images()
        {
            foreach (PictureBox p in this.Controls)
            {
                p.Image.Dispose();
            }
        }
    }
}
