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
         * "editor_menu" is the menu where the games are displayed.
         * "create_menu" is the menu where you choose the name of the newly created game.
         * "credits" means that the credits are displayed.
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
            this.displayed_menu = "main_menu";

            // Dynamically places the controls of the main menu.
            main_menu_btn_create.Location = new Point((this.Width / 2) - (main_menu_btn_create.Width / 2),
                                            (this.Height / 2) - main_menu_btn_create.Height - (main_menu_btn_create.Height / 2) - 4);

            main_menu_btn_edit.Location = new Point((this.Width / 2) - (main_menu_btn_edit.Width / 2),
                            (this.Height / 2) - main_menu_btn_edit.Height + (main_menu_btn_create.Height / 2) + 4);

            main_menu_btn_credits.Location = new Point((this.Width / 2) - main_menu_btn_credits.Width - 6,
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
            this.Controls.Clear();
            Display_editor_menu();
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
                    case "true": gc.IsDefault = true;
                        break;
                    case "false": gc.IsDefault = false;
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
            using (StreamReader stream_r = new StreamReader(internal_path))
            {
                string json_file = stream_r.ReadToEnd();
                JObject json_dict = JObject.Parse(json_file);
                output = (string)json_dict[var_name];
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
        /// Called when the main_menu_btn_create is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_menu_btn_create_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Display_Create_Menu();
        }

        /// <summary>
        /// Called when the editor_menu_btn_back_to_main_menu in the editor is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_to_main_menu(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Display_Main_Menu();
        }

        /// <summary>
        /// Creates all controls necessary to display the main menu.
        /// </summary>
        private void Display_Main_Menu()
        {
            displayed_menu = "main_menu";

            // Creation of the button that will create a game.
            Button main_menu_btn_create_dyna = new Button()
            {
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Name = "main_menu_btn_create",
                Size = new Size(230, 60),
                TabIndex = 1,
                Text = "Créer un jeu",
                UseVisualStyleBackColor = true
            };
            main_menu_btn_create_dyna.Location = new Point((this.Width / 2) - (main_menu_btn_create_dyna.Width / 2),
                                    (this.Height / 2) - main_menu_btn_create_dyna.Height - (main_menu_btn_create_dyna.Height / 2) - 4);
            main_menu_btn_create_dyna.Click += new EventHandler(this.Main_menu_btn_create_Click);

            // Creation of the button that will show the selection of available levels to edit.
            Button main_menu_btn_edit_dyna = new Button()
            {
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Name = "main_menu_btn_edit",
                Size = new Size(230, 60),
                TabIndex = 1,
                Text = "Mes jeux",
                UseVisualStyleBackColor = true
            };
            main_menu_btn_edit_dyna.Location = new Point((this.Width / 2) - (main_menu_btn_edit_dyna.Width / 2),
                            (this.Height / 2) - main_menu_btn_edit_dyna.Height + (main_menu_btn_create_dyna.Height / 2) + 4);
            main_menu_btn_edit_dyna.Click += new EventHandler(this.Main_menu_btn_edit_Click);

            // Creation of the button that will exit the application.
            Button main_menu_btn_exit_dyna = new Button() {
                Location = new Point((this.Width / 2) + 6, (this.Height / 2) + (main_menu_btn_edit.Height / 2) + 10),
                Name = "main_menu_btn_exit",
                Size = new Size(109, 30),
                TabIndex = 5,
                Text = "Quitter",
                UseVisualStyleBackColor = true
            };
            main_menu_btn_exit_dyna.Click += new EventHandler(this.Main_menu_btn_exit_Click);

            // Creation of the button that displays credits.
            Button main_menu_btn_credits_dyna = new Button()
            {
                Name = "main_menu_btn_credits_dyna",
                Size = new Size(109, 30),
                TabIndex = 5,
                Text = "Credits",
                UseVisualStyleBackColor = true
            };
            main_menu_btn_credits_dyna.Location = new Point((this.Width / 2) - main_menu_btn_credits.Width - 6,
                (this.Height / 2) + (main_menu_btn_edit.Height / 2) + 10);

            this.Controls.AddRange(new Control[] { main_menu_btn_create_dyna, main_menu_btn_edit_dyna, main_menu_btn_exit_dyna, main_menu_btn_credits_dyna });
        }

        /// <summary>
        /// Creates all controls necessary to display the editor menu.
        /// </summary>
        private void Display_editor_menu()
        {
            this.displayed_menu = "editor_menu";

            this.SuspendLayout();

            // Creation of the Button responsible to go back to the main menu.
            Button editor_menu_btn_back_to_main_menu = new Button() {
                Size = new Size((int)(this.Width * 0.05), (int)(this.Width * 0.05)),
                Location = new Point(10, 10),
                Name = "editor_menu_btn_back_to_main_menu",
                Text = "<-",
                UseVisualStyleBackColor = true
            };
            editor_menu_btn_back_to_main_menu.Click += new EventHandler(this.Back_to_main_menu);


            // Creation of the FlowLayoutPanel in which the games will be displayed as UserControls.
            FlowLayoutPanel flp_editor_menu = new FlowLayoutPanel()
            {
                Size = new Size(this.Width - (editor_menu_btn_back_to_main_menu.Location.X + editor_menu_btn_back_to_main_menu.Width - 8),
                        this.Height),
                Name = "flp_editor_menu",
                TabIndex = 3
            };
            flp_editor_menu.Location = new Point(editor_menu_btn_back_to_main_menu.Location.X + editor_menu_btn_back_to_main_menu.Width + 8, 0);

            this.Controls.AddRange(new Control[] { flp_editor_menu, editor_menu_btn_back_to_main_menu });

            // Fetching the existing games
            ArrayList games_list = Get_Games_List();
            foreach (GameCard gc in games_list)
            {
                flp_editor_menu.Controls.Add(gc);
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Creates all controls necessary to display the creator menu.
        /// </summary>
        private void Display_Create_Menu()
        {
            this.displayed_menu = "create_menu";

            // Creation of the text box to enter the game name.
            TextBox create_menu_txt_name_game = new TextBox() {
                Name = "create_menu_txt_name_game",
                Size = new Size(300, 60)
            };
            create_menu_txt_name_game.Location = new Point((this.Width / 2) - (create_menu_txt_name_game.Width / 2),
                                    (this.Height / 2) - create_menu_txt_name_game.Height - (create_menu_txt_name_game.Height / 2) - 4);

            // Creation of the button that will create the game and go to the editor.
            Button create_menu_btn_confirm = new Button()
            {
                Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Name = "create_menu_btn_confirm",
                Size = new Size(230, 60),
                TabIndex = 1,
                Text = "Créer",
                UseVisualStyleBackColor = true
            };
            create_menu_btn_confirm.Location = new Point((this.Width / 2) - (create_menu_btn_confirm.Width / 2),
                            (this.Height / 2) - create_menu_btn_confirm.Height + (create_menu_txt_name_game.Height / 2) + 4);
            create_menu_btn_confirm.Click += new EventHandler(this.Main_menu_btn_edit_Click);

            this.Controls.AddRange(new Control[] { create_menu_txt_name_game, create_menu_btn_confirm });
        }

        private void Menu_SizeChanged(object sender, EventArgs e)
        {
            switch (this.displayed_menu)
            {
                case "main_menu": Responsive_Resize_Main_Menu();
                    break;
                case "editor_menu": Responsive_Resize_Editor_Menu();
                    break;
                default:
                    break;
            }
        }

        private void Responsive_Resize_Editor_Menu()
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "editor_menu_btn_back_to_main_menu":
                        c.Size = new Size((int)(this.Width * 0.05), (int)(this.Width * 0.05));
                        c.Location = new Point(10, 10);
                        break;
                    case "flp_editor_menu":
                        c.Location = new Point(this.Controls.Find("editor_menu_btn_back_to_main_menu", false)[0].Location.X +
                            this.Controls.Find("editor_menu_btn_back_to_main_menu", false)[0].Width + 8, 0);
                        c.Size = new Size(this.Width - (this.Controls.Find("editor_menu_btn_back_to_main_menu", false)[0].Location.X +
                            this.Controls.Find("editor_menu_btn_back_to_main_menu", false)[0].Width - 8) - c.Location.X,
                            this.Height);
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
                    case "main_menu_btn_create":
                        c.Location = new Point((this.Width / 2) - (c.Width / 2), (this.Height / 2) - c.Height - (c.Height / 2) - 4);
                        break;
                    case "main_menu_btn_edit":
                        c.Location = new Point((this.Width / 2) - (c.Width / 2), 
                            (this.Height / 2) - c.Height + (this.Controls.Find("main_menu_btn_create",false)[0].Height / 2) + 4);
                        break;
                    case "main_menu_btn_credits_dyna":
                        c.Location = new Point((this.Width / 2) - c.Width - 6,
                            (this.Height / 2) + (this.Controls.Find("main_menu_btn_edit", false)[0].Height / 2) + 10);
                        break;
                    case "main_menu_btn_exit":
                        c.Location = new Point((this.Width / 2) + 6, (this.Height / 2) + (main_menu_btn_edit.Height / 2) + 10);
                        break;
                }
            }
        }
    }
}
