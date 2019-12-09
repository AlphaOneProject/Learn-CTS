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
    }
}
