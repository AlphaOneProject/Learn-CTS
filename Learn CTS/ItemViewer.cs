using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Learn_CTS
{
    public partial class ItemViewer : UserControl
    {
        private Item item;
        private ItemManager manager;

        public ItemViewer(int item_id, int parent_height, int parent_width)
        {
            InitializeComponent();
            this.Height = parent_height;
            this.Width = parent_width;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.manager = ItemManager.GetInstance();
            this.item = manager.GetItemByID(item_id);
            //Proprietes du label de la description
            lbl_desc.Text = this.item.GetDescription();

            //Proprietes du panel pour la couleur
            lbl_color.Width = parent_height;
            lbl_color.Height = parent_width;
            lbl_color.BackColor = Color.FromArgb(70, 0, 0, 0);

            //Proprietes de la picturebox pour l'image de l'objet
            pb_item.BackgroundImage = item.GetImage();
            pb_item.Height = item.GetImage().Height;
            pb_item.Width = item.GetImage().Width;
        }

        private void Btn_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
