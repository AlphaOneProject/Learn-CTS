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
using Newtonsoft.Json.Linq;

namespace Learn_CTS
{
    public partial class ItemViewer : UserControl
    {
        private Item item;
        private ItemManager manager;

        public ItemViewer(int item_id)
        {
            InitializeComponent();
            this.manager = ItemManager.GetInstance();
            this.item = manager.GetItemByID(item_id);
        }

        private void ItemViewer_Load(object sender, EventArgs e)
        {
            // Properties of the item image picturebox
            pb_item.BackgroundImage = item.GetImage();

            // Properties of the description label
            lbl_desc.Text = item.GetDescription();
            lbl_desc.BackColor = Color.Transparent;
            lbl_desc.Size = new Size(this.Width - pb_item.Width - 6, this.Height - 6);

            // Properties of the hide button
            btn_exit.Location = new Point(this.Width - btn_exit.Width - 24 , this.Height - 6 - btn_exit.Height);
        }

        private void Btn_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ItemViewer_ClientSizeChanged(object sender, EventArgs e)
        {
            lbl_desc.Width = this.Width - pb_item.Width - 6;
            btn_exit.Location = new Point(this.Width - btn_exit.Width - 24, this.Height - 6 - btn_exit.Height);
        }

        private void DisplayActions()
        {
            JObject actions = item.GetActions();
            int nbr_choices = (int)actions["choices"];
            for (int i = 1; i <= nbr_choices; i++)
            {
                Button btn = new Button();
                btn.Name = "btn_action";
                btn.AutoSize = true;
                btn.TabIndex = i;
                btn.Cursor = Cursors.Hand;
                btn.Text = actions["c" + i.ToString()]["answer"].ToString();
                btn.UseVisualStyleBackColor = true;
                //btn.Click += new System.EventHandler(this.Answer_Event);
                flp_actions.Controls.Add(btn);
            }
        }
    }
}
