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

        private void Btn_Back_To_Main_Menu_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Display_Main_Menu();
        }

        private void Display_Main_Menu()
        {
            displayed_menu = "main_menu";
            this.SuspendLayout();

            // 
            // Creation of the button that will show the selection of available levels to edit.
            // 
            Button main_menu_btn_edit = new Button();
            main_menu_btn_edit.Location = new Point(75, (int)(this.Height * 0.3));
            main_menu_btn_edit.Name = "btn_edit";
            main_menu_btn_edit.Size = new Size(75, 23);
            main_menu_btn_edit.TabIndex = 1;
            main_menu_btn_edit.Text = "Editer";
            main_menu_btn_edit.UseVisualStyleBackColor = true;
            main_menu_btn_edit.Click += new System.EventHandler(this.Main_menu_btn_edit_Click);

            // 
            // Creation of the button that will exit the application.
            // 
            Button main_menu_btn_exit = new Button();
            main_menu_btn_exit.Location = new Point(75, (int)(this.Height * 0.7));
            main_menu_btn_exit.Name = "btn_exit";
            main_menu_btn_exit.Size = new Size(75, 23);
            main_menu_btn_exit.TabIndex = 2;
            main_menu_btn_exit.Text = "Quitter";
            main_menu_btn_exit.UseVisualStyleBackColor = true;
            main_menu_btn_exit.Click += new System.EventHandler(this.Main_menu_btn_exit_Click);

            this.Controls.AddRange(new Control[] { main_menu_btn_edit, main_menu_btn_exit });

            this.ResumeLayout();
        }

        /// <summary>
        /// Creates all controls necessary to display the editor menu.
        /// Every controls created here have the tag "editor_menu".
        /// </summary>
        private void Display_editor_menu()
        {
            this.displayed_menu = "editor_menu";

            this.SuspendLayout();

            // Creation of the FlowLayoutPanel in which the games will be displayed as UserControls.
            FlowLayoutPanel flp_editor_menu = new FlowLayoutPanel();
            flp_editor_menu.Size = new Size((int)(this.Width * 0.8), this.Height);
            flp_editor_menu.Location = new Point((int)((this.Width / 2) - (flp_editor_menu.Width / 2)), 0);
            flp_editor_menu.Name = "flp_editor_menu";
            flp_editor_menu.TabIndex = 3;

            // Creation of the Button responsible to go back to the main menu.
            Button btn_back_to_main_menu = new Button();
            btn_back_to_main_menu.Size = new Size((int)(this.Width * 0.05), (int)(this.Width * 0.05));
            btn_back_to_main_menu.Location = new Point(10, 10);
            btn_back_to_main_menu.Name = "btn_back_to_main_menu";
            btn_back_to_main_menu.Text = "<-";
            btn_back_to_main_menu.UseVisualStyleBackColor = true;
            btn_back_to_main_menu.Click += new EventHandler(this.Btn_Back_To_Main_Menu_Click);

            this.Controls.AddRange(new Control[] { flp_editor_menu, btn_back_to_main_menu });

            // Fetching the existing games
            ArrayList games_list = Get_Games_List();
            foreach (GameCard gc in games_list)
            {
                flp_editor_menu.Controls.Add(gc);
            }

            this.ResumeLayout();
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
                    case "flp_editor_menu":
                        c.Size = new Size((int)(this.Width * 0.8), this.Height);
                        c.Location = new Point((int)((this.Width / 2) - (c.Width / 2)), 0);
                        break;
                    case "btn_back_to_main_menu":
                        c.Size = new Size((int)(this.Width * 0.05), (int)(this.Width * 0.05));
                        c.Location = new Point(10, 10);
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
                    case "main_menu_btn_edit":
                        c.Location = new Point(75, (int)(this.Height * 0.3));
                        break;
                    case "main_menu_btn_exit":
                        c.Location = new Point(75, (int)(this.Height*0.7));
                        break;
                }
            }
        }
    }
}
