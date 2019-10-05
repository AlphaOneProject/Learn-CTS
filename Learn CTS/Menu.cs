using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        String displayed_menu;

        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Menu()
        {
            InitializeComponent();
            this.DoubleBuffered = false;
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
        private void Btn_edit_Click(object sender, EventArgs e)
        {
            Remove_Menu_Controls_By_Tag(displayed_menu);
            Display_editor_menu();
        }

        private void Display_Flp_Scenes_List(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Back_To_Main_Menu_Click(object sender, EventArgs e)
        {
            Remove_Menu_Controls_By_Tag(displayed_menu);
            Display_Main_Menu();
        }

        private void Display_Main_Menu()
        {
            displayed_menu = "main_menu";
            this.SuspendLayout();
            //
            // Creation of the button that will show the selection of available levels to play.
            //
            Button btn_play = new Button();
            btn_play.Location = new Point(75, 55);
            btn_play.Name = "btn_play";
            btn_play.Size = new Size(75, 23);
            btn_play.TabIndex = 0;
            btn_play.Text = "Jouer";
            btn_play.UseVisualStyleBackColor = true;
            btn_play.Tag = "main_menu";

            // 
            // Creation of the button that will show the selection of available levels to edit.
            // 
            Button btn_edit = new Button();
            btn_edit.Location = new Point(75, 111);
            btn_edit.Name = "btn_edit";
            btn_edit.Size = new Size(75, 23);
            btn_edit.TabIndex = 1;
            btn_edit.Text = "Editer";
            btn_edit.UseVisualStyleBackColor = true;
            btn_play.Tag = "main_menu";
            btn_edit.Click += new System.EventHandler(this.Btn_edit_Click);

            // 
            // Creation of the button that will exit the application.
            // 
            Button btn_exit = new Button();
            btn_exit.Location = new Point(75, 336);
            btn_exit.Name = "btn_exit";
            btn_exit.Size = new Size(75, 23);
            btn_exit.TabIndex = 2;
            btn_exit.Text = "Quitter";
            btn_exit.UseVisualStyleBackColor = true;
            btn_exit.Tag = "main_menu";
            btn_exit.Click += new System.EventHandler(this.Btn_exit_Click);

            this.Controls.AddRange(new Control[] { btn_play, btn_edit, btn_exit });

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
            //
            //Creation of the FlowLayoutPanel in which the games will be displayed as UserControls.
            //
            FlowLayoutPanel flp_editor_menu = new FlowLayoutPanel();
            flp_editor_menu.Location = new Point(0, 0);
            flp_editor_menu.Name = "flp_editor_menu";
            flp_editor_menu.Size = new Size((int)(this.Width * 0.8), (int)(this.Height * 0.8));
            flp_editor_menu.BackColor = Color.ForestGreen;
            flp_editor_menu.TabIndex = 3;
            flp_editor_menu.Tag = "editor_menu";

            //
            //Creation of the Button responsible to go back to the main menu.
            //
            Button btn_back_to_main_menu = new Button();
            btn_back_to_main_menu.Location = new Point(0, this.Height - btn_back_to_main_menu.Height);
            btn_back_to_main_menu.Name = "btn_back_to_main_menu";
            btn_back_to_main_menu.Size = new Size(150, 23);
            btn_back_to_main_menu.Text = "Retour au menu principal";
            btn_back_to_main_menu.UseVisualStyleBackColor = true;
            btn_back_to_main_menu.Tag = "editor_menu";
            btn_back_to_main_menu.Click += new EventHandler(this.Btn_Back_To_Main_Menu_Click);

            this.Controls.AddRange(new Control[] { flp_editor_menu, btn_back_to_main_menu });
            flp_editor_menu.Controls.Add(btn_back_to_main_menu);

            this.ResumeLayout();
        }

        /// <summary>
        /// Removes every controls with the parameter tag from the form.
        /// </summary>
        /// <param name="ctag">Tag of the controls to remove</param>
        private void Remove_Menu_Controls_By_Tag(String ctag)
        {
            //MessageBox.Show(ctag);
            foreach(Control c in this.Controls)
            {
                if (ctag.Equals((String)c.Tag))
                {
                    this.Controls.Remove(c);
                }
            }
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

        }

        private void Responsive_Resize_Main_Menu()
        {
            
        }
    }
}
