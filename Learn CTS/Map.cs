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
    public partial class Map : UserControl
    {
        public Map(string game)
        {
            InitializeComponent();
            try
            {
                pbox_map.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory +"games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + "map.png");
            }
            catch (Exception) { MessageBox.Show("Aucune map n'a été trouvée."); }
        }

        private void Map_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
        }
    }
}
