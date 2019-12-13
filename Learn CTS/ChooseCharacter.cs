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

namespace Learn_CTS
{
    public partial class ChooseCharacter : UserControl
    {

        // Attributes
        private string game;
        private List<Image> list_images_char;
        private int i_current_img = 0;

        /// <summary>
        /// Construct a user control which let the user to choose the appearence of the player.
        /// </summary>
        /// <param name="game">The current game where the control will search the characters images folders.</param>
        public ChooseCharacter(string game)
        {
            InitializeComponent();
            this.game = game;
        }

        /// <summary>
        /// Load and display the appearence that the user can choose.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void ChooseCharacter_Load(object sender, EventArgs e)
        {
            list_images_char = new List<Image>();
            string images_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            string character_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar;
            pbox_arrow_left.Image = Image.FromFile(images_path +"arrow_left.png");
            pbox_arrow_right.Image = Image.FromFile(images_path + "arrow_right.png");
            foreach (string dir in Directory.GetDirectories(@"" + character_path))
            {
                if(Tools.Is_Valid(dir)) list_images_char.Add(Image.FromFile(dir + Path.DirectorySeparatorChar + "1_0.png"));
            }
            if (list_images_char.Count == 0) MessageBox.Show("Aucunes textures de personnage n'a été trouvée !");
            else
            {
                UpdateFolder();
            }
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void pbox_arrow_right_MouseDown(object sender, MouseEventArgs e)
        {
            this.i_current_img++;
            UpdateFolder();
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void pbox_arrow_left_MouseDown(object sender, MouseEventArgs e)
        {
            this.i_current_img--;
            UpdateFolder();
        }

        /// <summary>
        /// Update the character image displayed by the character image before and update the player folder.
        /// </summary>
        private void UpdateFolder()
        {
            pbox_char.Image = list_images_char[((((i_current_img % list_images_char.Count) + list_images_char.Count) % list_images_char.Count))];
            Player.SetFolder(((((i_current_img % list_images_char.Count) + list_images_char.Count) % list_images_char.Count) + 1).ToString());
        }
    }
}
