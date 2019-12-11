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

        public AnimationEdition(Editor editor, string folder_path)
        {
            InitializeComponent();

            this.editor = editor;
            this.folder_path = @"" + folder_path + Path.DirectorySeparatorChar;
            Console.WriteLine(this.folder_path);
        }

        private void AnimationEdition_Load(object sender, EventArgs e)
        {
            JObject theme = this.editor.Get_Theme();
            lbl_name.BackColor = Color.FromArgb(int.Parse((string)theme["2"]["R"]), int.Parse((string)theme["2"]["G"]), int.Parse((string)theme["2"]["B"]));
            lbl_name.ForeColor = Color.FromArgb(int.Parse((string)theme["5"]["R"]), int.Parse((string)theme["5"]["G"]), int.Parse((string)theme["5"]["B"]));
            lbl_name.Text = folder_path.Split(Path.DirectorySeparatorChar).Last();
            Load_Images();
        }

        private void Load_Images()
        {
            pb_up1.Image = Tools.Image_From_File(folder_path + "1_" + (upper_cursor % 9).ToString() + ".png");
            pb_up2.Image = Tools.Image_From_File(folder_path + "1_" + ((upper_cursor + 1) % 9).ToString() + ".png");
            pb_up3.Image = Tools.Image_From_File(folder_path + "1_" + ((upper_cursor + 2) % 9).ToString() + ".png");

            pb_down1.Image = Tools.Image_From_File(folder_path + "3_" + (lower_cursor % 9).ToString() + ".png");
            pb_down2.Image = Tools.Image_From_File(folder_path + "3_" + ((lower_cursor + 1) % 9).ToString() + ".png");
            pb_down3.Image = Tools.Image_From_File(folder_path + "3_" + ((lower_cursor + 2) % 9).ToString() + ".png");

            if (this.Is_Valid())
            {
                pb_valid.Image = Tools.Image_From_File(System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                                       Path.DirectorySeparatorChar + "images" +
                                                       Path.DirectorySeparatorChar + "done.png");
            }
            else
            {
                pb_valid.Image = Tools.Image_From_File(System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                                       Path.DirectorySeparatorChar + "images" +
                                                       Path.DirectorySeparatorChar + "gamecard-delete-btn-x64.png");
            }
        }

        public bool Is_Valid()
        {
            bool valid = true;
            List<string> required_files = new List<string>()
            {
                "1_0.png", "1_1.png", "1_2.png", "1_3.png", "1_4.png",
                "1_5.png", "1_6.png", "1_7.png", "1_8.png",
                "3_0.png", "3_1.png", "3_2.png", "3_3.png", "3_4.png",
                "3_5.png", "3_6.png", "3_7.png", "3_8.png"
            };
            bool found = false;
            foreach (string required_file in required_files)
            {
                found = false;
                foreach (string actual_file in Directory.GetFiles(folder_path))
                {
                    if (actual_file.Split(Path.DirectorySeparatorChar).Last().Equals(required_file))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }

        private void Pb_up_left_Click(object sender, EventArgs e)
        {
            this.upper_cursor--;
            Load_Images();
        }

        private void Pb_up_right_Click(object sender, EventArgs e)
        {
            this.upper_cursor++;
            Load_Images();
        }

        private void Pb_down_left_Click(object sender, EventArgs e)
        {
            this.lower_cursor--;
            Load_Images();
        }

        private void Pb_down_right_Click(object sender, EventArgs e)
        {
            this.lower_cursor++;
            Load_Images();
        }
    }
}
