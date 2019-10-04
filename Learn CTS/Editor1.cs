using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Learn_CTS
{
    public partial class Editor1 : Form
    {

        // Attributes

        private String game;
        private String game_path;

        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Editor1(String game)
        {
            InitializeComponent();
            this.game = game;
            this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "\\games\\" + game;
            this.DoubleBuffered = false;
        }

        /// <summary>
        /// Synchronize the Form with data from targeted game.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Editor1_Load(object sender, EventArgs e)
        {
            // Set the title and cut it if necessary.
            int char_space = (this.Width - menu.Width - 64) / 24;
            if (char_space < 12) { char_space = 12; } // Avoid a possible substring exception with a ridicularly little window.
            String cut_game = this.game;
            if (cut_game.Length > char_space)
            {
                cut_game = cut_game.Substring(0, char_space - 3) + "...";
            }
            title.Text = "Édition de " + cut_game;
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Height);

            // Load already existing scenarios.
            string sc_path = this.game_path + "\\scenarios";
            foreach (string s in Directory.GetDirectories(sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length).Split('.');
                Add_Scenario(folder[1]);
            }

            menu.ExpandAll();
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
            Button btn_content_add_scenario = new Button();
            btn_content_add_scenario.Name = "btn_btn_content_add_scenario";
            btn_content_add_scenario.Text = "Ajouter un nouveau scénario";
            btn_content_add_scenario.AutoSize = true;
            btn_content_add_scenario.Click += new System.EventHandler(this.Add_Scenario);
            content.Controls.Add(btn_content_add_scenario);
            btn_content_add_scenario.Location = new Point((content.Size.Width - btn_content_add_scenario.Size.Width) / 2, 100);
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
            Button btn_content_discard_scenario = new Button();
            btn_content_discard_scenario.Name = "btn_btn_content_discard_scenario";
            btn_content_discard_scenario.Text = "Supprimer ce scénario";
            btn_content_discard_scenario.AutoSize = true;
            btn_content_discard_scenario.Click += new System.EventHandler(this.Discard_Scenario);
            content.Controls.Add(btn_content_discard_scenario);
            btn_content_discard_scenario.Location = new Point((content.Size.Width - btn_content_discard_scenario.Size.Width) / 2, 100);

            // MessageBox.Show("Scénario n°" + scenario_id.ToString());
        }

        /// <summary>
        /// Add a new node to the "scenarios" in the TreeView and initialize it.
        /// Called from the node "scenarios".
        /// </summary>
        /// <param name="sender">Control calling the method, here it will always be the button from "Scenario" content.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Add_Scenario(object sender, EventArgs e)
        {
            // Initialize necessary variables and call .
            TreeNode parent = menu.Nodes.Find("scenarios", false)[0];
            int nbr_existing_scenarios = parent.Nodes.Count + 1;
            Add_Scenario("Scénario " + nbr_existing_scenarios.ToString());

            // Select the newly created TreeNode.
            menu.SelectedNode.Expand();
            menu.SelectedNode = parent.LastNode;
        }

        /// <summary>
        /// Add a new node to the "scenarios" in the TreeView and initialize it.
        /// Called with any string as the scenario's name.
        /// </summary>
        /// <param name="new_scenario">Name of the scenario to create.</param>
        private void Add_Scenario(string new_scenario)
        {
            // Initialize necessary variables.
            TreeNode tn_new_scenario = new TreeNode();
            TreeNode tn_parent = menu.Nodes.Find("scenarios", false)[0];
            int nbr_existing_scenarios = tn_parent.Nodes.Count + 1;

            // Configure the TreeNode associated to the new scenario.
            tn_new_scenario.Name = "scenario" + nbr_existing_scenarios.ToString();
            tn_new_scenario.Text = new_scenario;
            tn_parent.Nodes.Add(tn_new_scenario);
        }

        /// <summary>
        /// Delete an existing scenario, taking the one currently selected as target.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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
                // sc.Nodes[i].Text = "Scénario " + (i + 1).ToString();
            }
        }

        /// <summary>
        /// Adapt the size and positions of elements in the Form during a resize.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Editor1_SizeChanged(object sender, EventArgs e)
        {
            // Resize and cut the title if too long.
            int char_space = (this.Width - menu.Width - 64) / 24;
            if (char_space < 12) { char_space = 12; }
            String cut_name = this.game;
            if (cut_name.Length > char_space)
            {
                cut_name = cut_name.Substring(0, char_space - 3) + "...";
            }
            title.Text = "Édition de " + cut_name;
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Height);

            // Resize generals controls from the Editor.
            menu.Size = new Size(menu.Width, this.Height - menu.Location.Y - 51);
            content.Size = new Size(this.Width - content.Location.X - 28, this.Height - content.Location.Y - 51);

            // Resize all controls inside "content".
            Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
        }

        /// <summary>
        /// Recover the content of a variable in a JSON file at a specified path.
        /// Cast this content as a string before returning it.
        /// </summary>
        /// <param name="internal_path">Path from the game folder to the targeted JSON file.</param>
        /// <param name="var_name">Variable name in the JSON file.</param>
        /// <returns></returns>
        public string Get_Var_From_JSON(string internal_path, string var_name)
        {
            string output = "";
            using (StreamReader stream_r = new StreamReader(this.game_path + internal_path))
            {
                var json_file = stream_r.ReadToEnd();
                var json_dict = JObject.Parse(json_file);
                output = (string) json_dict[var_name];
            }
            return output;
        }
    }
}
