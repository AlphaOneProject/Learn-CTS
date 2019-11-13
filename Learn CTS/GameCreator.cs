using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Learn_CTS
{
    public partial class GameCreator : UserControl
    {

        /**
         * Default path of all the created games. 
         */
        private readonly String games_path;

        public GameCreator()
        {
            InitializeComponent();

            this.games_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar;

            lbl_create.Width = pnl_bg.Width;
            lbl_create.TextAlign = ContentAlignment.MiddleCenter;
            lbl_create.Location = new Point(pnl_bg.Width / 2 - lbl_create.Width / 2,
                24);

            pb_back_create.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png");

            pb_confirm.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "gamecard-play-btn-x128.png");

            
        }

        private void GameCreator_Load(object sender, EventArgs e)
        {
            txt_create.Focus();
        }

        /// <summary>
        /// Character filtering for the game name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_create_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)13 && !t.Text.Equals(String.Empty) && Is_Game_Unique(t.Text))
            {
                games_menu_confirm_box(t.Text);
            }
            else if (!Is_Game_Unique(t.Text))
            {
                MessageBox.Show("Ce jeu existe déjà.");
            }
        }

        /// <summary>
        /// Checks if the given name is already taken by an existing game.
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        private Boolean Is_Game_Unique(String nom)
        {
            Boolean res = true;
            foreach (String dir in Directory.GetDirectories(@"" + games_path))
            {
                if (nom.Equals(dir))
                {
                    res = false;
                }
            }
            return res;
        }

        /// <summary>
        /// Displays a confirmation MessageBox
        /// </summary>
        /// <param name="nom"></param>
        private void games_menu_confirm_box(String nom)
        {
            if (MessageBox.Show("Confirmer la creation du jeu " + nom + " ?", "Confirmation de création",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Create_game(nom);
                Form g = new Editor(nom);
                this.Hide();
                g.Show();
            }
        }

        /// <summary>
        /// Creates the directories used by the game.
        /// </summary>
        /// <param name="nom"></param>
        private void Create_game(String nom)
        {
            // ./Game
            Directory.CreateDirectory(@"" + this.games_path + nom);
            // ./Game/library
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "library");
            // ./Game/library/dialogs
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "dialogs");
            // ./Game/library/images
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images");
            // ./Game/library/npcs
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "npcs");
            // ./Game/scenarios
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "scenarios");

            // Add a "properties.json" to the newly created folder.
            JObject properties_content = new JObject();
            properties_content["default"] = false;
            properties_content["state"] = "Inactif.";
            properties_content["description"] = "Description par défaut";
            File.WriteAllText(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "properties.json",
                              properties_content.ToString());
        }

        /// <summary>
        /// Called when the back arrow is clicked.
        /// Hides the UC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_back_create_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Called when the background is clicked.
        /// Hides the UC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pnl_greyout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Called when the Size of the UC to have a responsive behavior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameCreator_SizeChanged(object sender, EventArgs e)
        {
            lbl_create.Location = new Point(pnl_bg.Width / 2 - lbl_create.Width / 2, 24);
        }

        /// <summary>
        /// Verifies the validity of the name given before calling the confirmation box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_game_verify(object sender, MouseEventArgs e)
        {
            if (!txt_create.Text.Equals(String.Empty) && Is_Game_Unique(txt_create.Text))
            {
                games_menu_confirm_box(txt_create.Text);
            }
            else if (!Is_Game_Unique(txt_create.Text))
            {
                MessageBox.Show("Ce jeu existe déjà.");
            }
        }

        private void GameCreator_Leave(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}