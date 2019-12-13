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
    public partial class ImageEdition : UserControl
    {

        // Attributes.

        private Editor editor;
        private string image_path;
        private string type;

        // Methods.

        /// <summary>
        /// Constructor of the UserControl, setup the necessary arguments as parameters.
        /// </summary>
        /// <param name="editor">Parent of the UserControl, used for game path and theme.</param>
        /// <param name="image_path">Path of the image to edit.</param>
        public ImageEdition(Editor editor, string image_path, string type)
        {
            InitializeComponent();

            this.editor = editor;
            this.image_path = image_path;
            this.type = type;
        }

        /// <summary>
        /// Load the image, its title and the global theme.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void ImageEdition_Load(object sender, EventArgs e)
        {
            pb_img.BackgroundImage = Tools.Image_From_File(this.image_path);

            lbl_title.Text = image_path.Split(Path.DirectorySeparatorChar).Last().Split('.')[0];

            if (this.type == "map") lbl_title.Text = "Carte accessible en jeu";

            JObject theme = editor.Get_Theme();

            this.BackColor = Color.FromArgb(int.Parse((string)theme["2"]["R"]), int.Parse((string)theme["2"]["G"]), int.Parse((string)theme["2"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)theme["5"]["R"]), int.Parse((string)theme["5"]["G"]), int.Parse((string)theme["5"]["B"]));
        }

        /// <summary>
        /// Allow the user to override the current image.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_img_Click(object sender, EventArgs e)
        {
            if (ofd_img.ShowDialog() == DialogResult.Cancel) { return; }

            string extension = ofd_img.FileName.Split(Path.DirectorySeparatorChar).Last().Split('.').Last();
            if (extension != "png")
            {
                MessageBox.Show("L'image doit être de format .png pour être utilisée.", "Type invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string new_path;
            switch (this.type)
            {
                case "background":
                    new_path = editor.Get_Game_Path() + Path.DirectorySeparatorChar + "library" +
                               Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "background" +
                               Path.DirectorySeparatorChar + this.image_path.Split(Path.DirectorySeparatorChar).Last();
                    break;
                case "item":
                    new_path = editor.Get_Game_Path() + Path.DirectorySeparatorChar + "library" +
                               Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "items" +
                               Path.DirectorySeparatorChar + this.image_path.Split(Path.DirectorySeparatorChar).Last();
                    break;
                case "map":
                    new_path = editor.Get_Game_Path() + Path.DirectorySeparatorChar + "library" +
                               Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "others" +
                               Path.DirectorySeparatorChar + this.image_path.Split(Path.DirectorySeparatorChar).Last();
                    break;
                default:
                    throw new ArgumentException("Type given to ImageEdition was invalid!");
            }

            if (@"" + ofd_img.FileName == new_path)
            {
                MessageBox.Show("Vous ne pouvez importer un modèle déjà présent.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                File.Copy(@"" + ofd_img.FileName, @"" + new_path, true);
            }
            catch (Exception)
            {
                MessageBox.Show("L'image est en cours d'utilisation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                return;
            }

            ImageEdition_Load(this, new EventArgs());
        }

        /// <summary>
        /// Checks the uses of the image, if not used delete it and then "Dispose()" himself.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_delete_Click(object sender, EventArgs e)
        {
            // Checks if it is used somewhere.
            JObject data;
            string scenarios_path;
            switch (this.type)
            {
                case "background":
                    scenarios_path = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
                    foreach (string scenario in Directory.GetDirectories(scenarios_path))
                    {
                        foreach (string situation in Directory.GetDirectories(scenario))
                        {
                            data = Tools.Get_From_JSON(situation + Path.DirectorySeparatorChar + "environment.json");
                            if (data["background"].ToString() == this.image_path.Split(Path.DirectorySeparatorChar).Last())
                            {
                                MessageBox.Show("Cette image est utilisée dans une ou plusieurs situations.\n" +
                                                "Remplacez-la dans ces situations puis réessayez.\n\n" +
                                                "Situation en faisant usage : " +
                                                situation.Split(Path.DirectorySeparatorChar).Last().Split('.').Last(),
                                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    break;
                case "item":
                    scenarios_path = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
                    foreach (string scenario in Directory.GetDirectories(scenarios_path))
                    {
                        foreach (string situation in Directory.GetDirectories(scenario))
                        {
                            data = Tools.Get_From_JSON(situation + Path.DirectorySeparatorChar + "items.json");
                            for (int i = 1; i <= int.Parse(data["events"].ToString()); i++)
                            {
                                if (data[i.ToString()]["item"]["name"].ToString() == this.image_path.Split(Path.DirectorySeparatorChar).Last().Split('.')[0])
                                {
                                    MessageBox.Show("Cet objet est utilisé dans une ou plusieurs situations.\n" +
                                                    "Remplacez-le dans ces situations puis réessayez.\n\n" +
                                                    "Situation en faisant usage : " +
                                                    situation.Split(Path.DirectorySeparatorChar).Last().Split('.').Last(),
                                                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                    break;
                case "map":
                    break;
                default:
                    throw new ArgumentException("Type given to ImageEdition was invalid!");
            }

            // Asks for confirmation before suppression.
            if (MessageBox.Show("Confirmez-vous la suppression de cette image ?", "Suppression d'image", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                File.Delete(image_path);
                if (this.type != "map") this.Dispose();
            }
        }

    }
}
