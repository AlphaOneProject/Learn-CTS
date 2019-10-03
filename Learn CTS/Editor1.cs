using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class Editor1 : Form
    {
        public Editor1()
        {
            InitializeComponent();
            this.DoubleBuffered = false;
            menu.ExpandAll();
            this.Activate();
        }

        /// <summary>
        /// Change the content display when the user select another category from the menu.
        /// </summary>
        /// <param name="sender">Control calling the method, here it will always be the menu.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Menu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView t = (TreeView) sender;
            content.Text = t.SelectedNode.FullPath;

            switch(t.SelectedNode.Name)
            {
                case "global":
                    Display_Global(); break;
                case "characters":
                    Display_Characters(); break;
                case "npcs":
                    Display_NPCs(); break;
                case "player":
                    Display_Player(); break;
                case "scenarios":
                    Display_Scenarios(); break;
                default:
                    throw new ArgumentException("Category not yet implemented: " + t.SelectedNode.Text);
            }
        }

        private void Display_Global()
        {
            content.Controls.Clear();
            MessageBox.Show("Global");
        }

        private void Display_Characters()
        {
            content.Controls.Clear();
            MessageBox.Show("Characters");
        }

        private void Display_NPCs()
        {
            content.Controls.Clear();
            MessageBox.Show("NPCs");
        }

        private void Display_Player()
        {
            content.Controls.Clear();
            MessageBox.Show("Player");
        }

        private void Display_Scenarios()
        {
            content.Controls.Clear();
            MessageBox.Show("Scenarios");
        }
    }
}
