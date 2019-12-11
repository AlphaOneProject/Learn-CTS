﻿using System;
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

        // Methods.

        public ImageEdition(Editor editor, string image_path)
        {
            InitializeComponent();

            this.editor = editor;
            this.image_path = image_path;
        }

        private void ImageEdition_Load(object sender, EventArgs e)
        {
            Image img;
            using (var bmpTemp = new Bitmap(image_path))
            {
                img = new Bitmap(bmpTemp);
            }
            pb_img.BackgroundImage = img;

            lbl_title.Text = image_path.Split(Path.DirectorySeparatorChar).Last();

            JObject theme = editor.Get_Theme();

            this.BackColor = Color.FromArgb(int.Parse((string)theme["2"]["R"]), int.Parse((string)theme["2"]["G"]), int.Parse((string)theme["2"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)theme["5"]["R"]), int.Parse((string)theme["5"]["G"]), int.Parse((string)theme["5"]["B"]));
        }

        private void Pb_delete_Click(object sender, EventArgs e)
        {
            // Checks if it is used in a situation.
            string scenarios_path = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            JObject data;
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

            // Asks for confirmation before suppression.
            if (MessageBox.Show("Confirmez-vous la suppression de ce décor ?", "Suppression de décor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                File.Delete(image_path);
                this.Dispose();
            }
        }
    }
}