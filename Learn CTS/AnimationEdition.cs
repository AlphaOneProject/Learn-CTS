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
    public partial class AnimationEdition : UserControl
    {

        // Attributes.

        private Editor editor;
        private string folder_path;
        private int upper_cursor = 2;
        private int lower_cursor = 2;

        // Methods.

        /// <summary>
        /// Constructor of the UserControl, setup the necessary arguments as parameters.
        /// </summary>
        /// <param name="editor">Parent instance of Editor.</param>
        /// <param name="folder_path">Sprite folder's full path.</param>
        public AnimationEdition(Editor editor, string folder_path)
        {
            InitializeComponent();

            this.editor = editor;
            this.folder_path = @"" + folder_path + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Apply the global theme to the UserControl and set the name.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void AnimationEdition_Load(object sender, EventArgs e)
        {
            JObject theme = this.editor.Get_Theme();
            lbl_name.BackColor = Color.FromArgb(int.Parse((string)theme["1"]["R"]), int.Parse((string)theme["1"]["G"]), int.Parse((string)theme["1"]["B"]));
            lbl_name.ForeColor = Color.FromArgb(int.Parse((string)theme["5"]["R"]), int.Parse((string)theme["5"]["G"]), int.Parse((string)theme["5"]["B"]));
            lbl_name.Text = folder_path.Substring(0, folder_path.Length - 1).Split(Path.DirectorySeparatorChar).Last();
            Load_Images();
        }

        /// <summary>
        /// Load images depending upon values of "upper_cursor" and "lower_cursor".
        /// Then checks validity of the folder.
        /// </summary>
        private void Load_Images()
        {
            pb_up1.Tag = folder_path + "1_" + (((upper_cursor % 9) + 9) % 9).ToString() + ".png";
            pb_up2.Tag = folder_path + "1_" + ((((upper_cursor + 1) % 9) + 9) % 9).ToString() + ".png";
            pb_up3.Tag = folder_path + "1_" + ((((upper_cursor + 2) % 9) + 9) % 9).ToString() + ".png";

            pb_down1.Tag = folder_path + "3_" + (((lower_cursor % 9) + 9) % 9).ToString() + ".png";
            pb_down2.Tag = folder_path + "3_" + ((((lower_cursor + 1) % 9) + 9) % 9).ToString() + ".png";
            pb_down3.Tag = folder_path + "3_" + ((((lower_cursor + 2) % 9) + 9) % 9).ToString() + ".png";

            pb_up1.Image = Tools.Image_From_File(folder_path + "1_" + (((upper_cursor % 9) + 9) % 9).ToString() + ".png");
            pb_up2.Image = Tools.Image_From_File(folder_path + "1_" + ((((upper_cursor + 1) % 9) + 9) % 9).ToString() + ".png");
            pb_up3.Image = Tools.Image_From_File(folder_path + "1_" + ((((upper_cursor + 2) % 9) + 9) % 9).ToString() + ".png");

            pb_down1.Image = Tools.Image_From_File(folder_path + "3_" + (((lower_cursor % 9) + 9) % 9).ToString() + ".png");
            pb_down2.Image = Tools.Image_From_File(folder_path + "3_" + ((((lower_cursor + 1) % 9) + 9) % 9).ToString() + ".png");
            pb_down3.Image = Tools.Image_From_File(folder_path + "3_" + ((((lower_cursor + 2) % 9) + 9) % 9).ToString() + ".png");

            if (Tools.Is_Valid(folder_path))
            {
                pb_valid.Image = Tools.Image_From_File(System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                                       Path.DirectorySeparatorChar + "images" +
                                                       Path.DirectorySeparatorChar + "done.png");
            }
            else
            {
                pb_valid.Image = Tools.Image_From_File(System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                                       Path.DirectorySeparatorChar + "images" +
                                                       Path.DirectorySeparatorChar + "wrong.png");
            }
        }

        /// <summary>
        /// Slides the upper bar to the left.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_up_left_Click(object sender, EventArgs e)
        {
            this.upper_cursor--;
            Load_Images();
        }

        /// <summary>
        /// Slides the upper bar to the right.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_up_right_Click(object sender, EventArgs e)
        {
            this.upper_cursor++;
            Load_Images();
        }

        /// <summary>
        /// Slides the lower bar to the left.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_down_left_Click(object sender, EventArgs e)
        {
            this.lower_cursor--;
            Load_Images();
        }

        /// <summary>
        /// Slides the lower bar to the right.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_down_right_Click(object sender, EventArgs e)
        {
            this.lower_cursor++;
            Load_Images();
        }

        /// <summary>
        /// Set or modify the assigned sprite with one specified by the user through OpenFileDialog.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Click_Sprite(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (ofd_sprites.ShowDialog() == DialogResult.Cancel) { return; }

            string extension = ofd_sprites.FileName.Split(Path.DirectorySeparatorChar).Last().Split('.').Last();
            if (!(extension == "png" || extension == "jpeg"))
            {
                MessageBox.Show("L'image doit être soit de format .png soit .jpeg pour être utilisée.", "Type invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (@"" + ofd_sprites.FileName == @"" + pb.Tag)
            {
                MessageBox.Show("Vous ne pouvez importer un modèle déjà présent.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            File.Copy(@"" + ofd_sprites.FileName, @"" + pb.Tag, true);

            pb.Image = Tools.Image_From_File(@"" + pb.Tag);
        }

        /// <summary>
        /// After checking for any usage in NPCs folders,
        /// deletes this sprite folder.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_delete_Click(object sender, EventArgs e)
        {
            // Checks if it is used in a situation.
            string npcs_path = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "npcs";
            JObject data;
            foreach (string npc in Directory.GetFiles(npcs_path))
            {
                data = Tools.Get_From_JSON(npc);
                if (data["folder"].ToString() == this.folder_path.Substring(0, this.folder_path.Length - 1).Split(Path.DirectorySeparatorChar).Last())
                {
                    MessageBox.Show("Ces images sont utilisées par un ou plusieurs figurants.\n" +
                                    "Remplacez-le pour ces figurants puis réessayez.\n\n" +
                                    "Figurant en faisant usage : " + data["name"].ToString(),
                                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            // Asks for confirmation before suppression.
            if (MessageBox.Show("Confirmez-vous la suppression de ce décor ?", "Suppression de décor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Directory.Delete(folder_path, true);
                this.Dispose();
            }
        }
    }
}
