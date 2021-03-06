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

namespace Learn_CTS
{
    public partial class Backpack : UserControl
    {
        /// <summary>
        /// Constructor of the backpack control
        /// </summary>
        public Backpack()
        {
            InitializeComponent();
            this.Name = "backpack";
            this.Tag = 0;
        }

        /// <summary>
        /// Load the backpack control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Backpack_Load(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            this.Location = new Point(f.Width / 2 - this.Width / 2, f.Height / 2 - this.Height / 2);
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            pbox_backpack.Image = Image.FromFile(path + "backpack.png");
            pbox_ticket.Image = Image.FromFile(path + "tickets.png");
            pbox_map.Image = Image.FromFile(path + "map.png");
            pbox_phone.Image = Image.FromFile(path + "smartphone.png");
            pbox_close.Image = Image.FromFile(path + "gamecard-delete-btn-x64.png");
            pbox_close.Click += new EventHandler(Backpack_Closed);
            this.Focus();
        }
        
        /// <summary>
        /// Close the backpack.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Backpack_Closed(object sender, EventArgs e)
        {
            ((GameWindow)this.FindForm()).OpenClose_Backpack();
        }

        /// <summary>
        /// Close the backpack if the user press b.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Backpack_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.B) Backpack_Closed(sender, e);
        }

        /// <summary>
        /// Open the phone if the user clicks on the phone icon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbox_phone_Click(object sender, EventArgs e)
        {
            GameWindow.GetInstance().RemoveAllControls();
            this.FindForm().Controls.Add(new Phone());
        }

        /// <summary>
        /// Open the map of the user clicks on the map icon.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void pbox_map_Click(object sender, EventArgs e)
        {
            GameWindow.GetInstance().RemoveAllControls();
            this.FindForm().Controls.Add(new Map(this.FindForm().Text));
        }
    }
}
