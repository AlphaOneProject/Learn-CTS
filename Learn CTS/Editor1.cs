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
        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Editor1(String game)
        {
            InitializeComponent();
            this.DoubleBuffered = false;

            menu.ExpandAll();

            // Set the title and cut it if necessary.
            if(game.Length > 38)
            {
                game = game.Substring(0, 36) + "...";
            }
            title.Text = "Édition de '" + game + "'";
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Height);
        }

        /// <summary>
        /// Change the content display when the user select another category from the menu.
        /// </summary>
        /// <param name="sender">Control calling the method, here it will always be the menu.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Menu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView t = (TreeView) sender;
            String name = t.SelectedNode.Name;
            content.Text = t.SelectedNode.FullPath;

            if(name.StartsWith("scenario") && name != "scenarios")
            {
                Display_Scenario(int.Parse(name.Substring(8)));
            }
            else
            {
                switch (name)
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
        }

        /// <summary>
        /// Load controls for general content.
        /// </summary>
        private void Display_Global()
        {
            // Empty the groupbox from precedent controls.
            content.Controls.Clear();

            // WIP
        }

        /// <summary>
        /// Load controls for characters' content.
        /// </summary>
        private void Display_Characters()
        {
            // Empty the groupbox from precedent controls.
            content.Controls.Clear();

            // WIP
        }

        /// <summary>
        /// Load controls for NPCs' content.
        /// </summary>
        private void Display_NPCs()
        {
            // Empty the groupbox from precedent controls.
            content.Controls.Clear();

            // WIP
        }

        /// <summary>
        /// Load controls for player's content.
        /// </summary>
        private void Display_Player()
        {
            // Empty the groupbox from precedent controls.
            content.Controls.Clear();

            // WIP
        }

        /// <summary>
        /// Load controls for scenarios' content.
        /// </summary>
        private void Display_Scenarios()
        {
            // Empty the groupbox from precedent controls.
            content.Controls.Clear();

            // Creation of a button allowing the creation of new scenarios.
            Button content_add_scenario = new Button();
            content_add_scenario.Text = "Ajouter un nouveau scénario";
            content_add_scenario.AutoSize = true;
            content_add_scenario.Click += new System.EventHandler(this.Add_Scenario);
            content.Controls.Add(content_add_scenario);
            content_add_scenario.Location = new Point((content.Size.Width - content_add_scenario.Size.Width) / 2, 100);
        }

        /// <summary>
        /// Load a specified scenario into the "content" groupbox.
        /// </summary>
        /// <param name="scenario_id">ID of the scenario to load.</param>
        private void Display_Scenario(int scenario_id)
        {
            // Empty the groupbox from precedent controls.
            content.Controls.Clear();

            // Creation of a button allowing to discard the scenario.
            Button content_discard_scenario = new Button();
            content_discard_scenario.Text = "Supprimer ce scénario";
            content_discard_scenario.AutoSize = true;
            content_discard_scenario.Click += new System.EventHandler(this.Discard_Scenario);
            content.Controls.Add(content_discard_scenario);
            content_discard_scenario.Location = new Point((content.Size.Width - content_discard_scenario.Size.Width) / 2, 100);

            // MessageBox.Show("Scénario n°" + scenario_id.ToString());
        }

        /// <summary>
        /// Add a new node to the "menu" and initialize it.
        /// </summary>
        /// <param name="sender">Control calling the method, here it will always be the button from "Scenario" content.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Add_Scenario(object sender, EventArgs e)
        {
            // Initialize necessary variables.
            TreeNode new_scenario = new TreeNode();
            TreeNode parent = menu.Nodes.Find("scenarios", false)[0];
            int nbr_existing_scenarios = parent.Nodes.Count + 1;

            // Configure the TreeNode associated to the new scenario.
            new_scenario.Name = "scenario" + nbr_existing_scenarios.ToString();
            new_scenario.Text = "Scénario " + nbr_existing_scenarios.ToString();
            parent.Nodes.Add(new_scenario);

            // Select the newly created TreeNode.
            menu.SelectedNode.Expand();
            menu.SelectedNode = new_scenario;
        }

        private void Discard_Scenario(object sender, EventArgs e)
        {
            // Ask for confirmation before suppression of the scenario.
            if((MessageBox.Show("Confirmer la suppression du " + menu.SelectedNode.Text + " ?", "Confirmation de suppression", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)) {
                return;
            }

            menu.Nodes.Remove(menu.SelectedNode);

            TreeNode sc = menu.Nodes.Find("scenarios", false)[0];
            menu.SelectedNode = sc;

            // Reorder visually and nominally the rest of scenarios.
            for(int i = 0; i < sc.Nodes.Count; i++)
            {
                sc.Nodes[i].Name = "scenario" + (i + 1).ToString();
                sc.Nodes[i].Text = "Scénario " + (i + 1).ToString();
            }
        }

        private void Editor1_SizeChanged(object sender, EventArgs e)
        {
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Height);
            menu.Size = new Size(menu.Width, this.Height - menu.Location.Y - 51);
            content.Size = new Size(this.Width - content.Location.X - 28, this.Height - content.Location.Y - 51);
            Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
        }
    }
}
