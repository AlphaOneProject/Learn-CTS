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

        //TODO: Gérer le SizeChanged

        public ItemViewer(int item_id)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.manager = ItemManager.GetInstance();
            this.item = manager.GetItemByID(item_id);

            //Proprietes du panel pour la couleur
            lbl_color.Width = this.Width;
            lbl_color.Height = this.Height;
            lbl_color.BackColor = Color.FromArgb(80, 0, 0, 0);

            //Proprietes de la picturebox pour l'image de l'objet
            pb_item.BackgroundImage = item.GetImage();
            pb_item.Height = item.GetImage().Height;
            pb_item.Width = item.GetImage().Width;
            pb_item.Location = new Point((this.Width / 2) - (pb_item.Width * 2), (this.Height / 2) - (pb_item.Height / 2));

            //Proprietes du label de la description
            lbl_desc.Text = this.item.GetDescription();
            lbl_desc.BackColor = Color.Transparent;
            lbl_desc.Parent = lbl_color;
            lbl_desc.AutoSize = true;
            lbl_desc.MaximumSize = new Size(this.Width - ((this.Width / 2) + (this.Width / 20)), this.Height - pb_item.Location.Y);
            lbl_desc.Location = new Point((this.Width / 2) + (this.Width / 20), (this.Height / 2) + (lbl_desc.Height / 2));

            //Proprietes du bouton pour quitter
            btn_exit.Location = new Point(lbl_desc.Location.X, lbl_desc.Location.Y + 20 + lbl_desc.Height);
        }

        private void Btn_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Lbl_color_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ItemViewer_ClientSizeChanged(object sender, EventArgs e)
        {
            lbl_color.Width = this.Width;
            lbl_color.Height = this.Height;
            pb_item.Location = new Point((this.Width / 2) - (pb_item.Width * 2), (this.Height / 2) - (pb_item.Height / 2));
            lbl_desc.Location = new Point((this.Width / 2) + (this.Width / 20), (this.Height / 2) - (lbl_desc.Height / 2));
            btn_exit.Location = new Point(lbl_desc.Location.X, lbl_desc.Location.Y + 20 + lbl_desc.Height);
        }
    }
}
