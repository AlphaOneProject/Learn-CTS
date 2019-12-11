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

        private string game;
        private List<Image> list_images_char;
        private int i_current_img = 0;

        public ChooseCharacter(string game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void ChooseCharacter_Load(object sender, EventArgs e)
        {
            list_images_char = new List<Image>();
            string images_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            string character_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar;
            pbox_arrow_left.Image = Image.FromFile(images_path +"arrow_left.png");
            pbox_arrow_right.Image = Image.FromFile(images_path + "arrow_right.png");
            foreach (string dir in Directory.GetDirectories(@"" + character_path))
            {
                list_images_char.Add(Image.FromFile(dir + Path.DirectorySeparatorChar + "1_0.png"));
            }
            if (list_images_char.Count == 0) MessageBox.Show("Aucunes textures de personnage n'a été trouvée !");
            else
            {
                UpdateFolder();
            }
        }

        private void pbox_arrow_right_MouseDown(object sender, MouseEventArgs e)
        {
            this.i_current_img--;
            UpdateFolder();
        }

        private void pbox_arrow_left_MouseDown(object sender, MouseEventArgs e)
        {
            this.i_current_img++;
            UpdateFolder();
        }

        private void UpdateFolder()
        {
            pbox_char.Image = list_images_char[Math.Abs(i_current_img) % list_images_char.Count];
            Player.SetFolder((Math.Abs(i_current_img) % list_images_char.Count + 1).ToString());
        }
    }
}
