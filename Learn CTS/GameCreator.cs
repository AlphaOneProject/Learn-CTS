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

            this.Dock = DockStyle.Fill;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            pnl_greyout.BackColor = Color.FromArgb(80, 0, 0, 0);
            pnl_greyout.Location = new Point(0, 0);
            pnl_greyout.Size = new Size(this.Width, this.Height);

            pnl_bg.Location = new Point(this.Width / 2 - pnl_bg.Width / 2,
                this.Height / 2 - pnl_bg.Height / 2);

            lbl_create.Width = pnl_bg.Width;
            lbl_create.TextAlign = ContentAlignment.MiddleCenter;
            lbl_create.Location = new Point(pnl_bg.Width / 2 - lbl_create.Width / 2,
                24);

            pb_back_create.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_left.png");

            pb_confirm.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "gamecard-play-btn-x128.png");

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

        private void Create_game(String nom)
        {

            //TODO : verifier si le nom est unique
            //Creation of the directories used by the game.
            Directory.CreateDirectory(@"" + this.games_path + nom);
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "library");
            Directory.CreateDirectory(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "scenarios");

            // Add a "properties.json" to the newly created folder.
            JObject properties_content = new JObject();
            properties_content["default"] = false;
            properties_content["state"] = "Inactif.";
            properties_content["description"] = "Description par défaut";
            File.WriteAllText(@"" + this.games_path + nom + Path.DirectorySeparatorChar + "properties.json",
                              properties_content.ToString());
        }

        private void Pb_back_create_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Pnl_greyout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void GameCreator_SizeChanged(object sender, EventArgs e)
        {
            pnl_greyout.Size = new Size(this.Width, this.Height);
            pnl_bg.Location = new Point(this.Width / 2 - pnl_bg.Width / 2,
                this.Height / 2 - pnl_bg.Height / 2);
            lbl_create.Location = new Point(pnl_bg.Width / 2 - lbl_create.Width / 2, 24);
        }

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
    }
}