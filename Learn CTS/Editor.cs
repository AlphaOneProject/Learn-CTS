﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Learn_CTS
{
    public partial class Editor : Form
    {

        // Attributes

        private readonly String game;
        private readonly String game_path;

        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Editor(String game)
        {
            InitializeComponent();
            this.game = game;
            this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "games" + Path.DirectorySeparatorChar + game;
            this.Text = "Éditeur : " + game;
            this.DoubleBuffered = false;
        }

        /// <summary>
        /// Restart the application when closing the editor.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Marks the current game as available.
            JObject properties = Get_From_JSON(Path.DirectorySeparatorChar + "properties.json");
            if(properties["state"].ToString().Substring(0, 8).Equals("[DENIED]"))
            {
                properties["state"] = properties["state"].ToString().Substring(8);
            }
            else
            {
                properties["state"] = "Inactif.";
            }
            Set_To_JSON(Path.DirectorySeparatorChar + "properties.json", properties);

            Application.Restart();
        }

        /// <summary>
        /// Synchronize the Form with data from targeted game.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Editor1_Load(object sender, EventArgs e)
        {
            // Marks the current game as in edition so it blocks any concurrent edition or playing.
            JObject properties = Get_From_JSON(Path.DirectorySeparatorChar + "properties.json");
            if(!properties["state"].ToString().Equals("Inactif."))
            {
                if (MessageBox.Show("Le jeu " + '"' + this.game + '"' + " est déjà en cours d'édition ou d'utilisation sur cette machine.\nSouhaitez-vous tout de même y accèder ?",
                                "Jeu en utilisation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                {
                    properties["state"] = "[DENIED]" + properties["state"];
                    Set_To_JSON(Path.DirectorySeparatorChar + "properties.json", properties);
                    this.Close();
                    return;
                }
            }
            properties["state"] = "En cours d'édition...";
            Set_To_JSON(Path.DirectorySeparatorChar + "properties.json", properties);

            // Place the windows at the center of the screen.
            Rectangle screen = Screen.FromControl(this).Bounds;
            this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2);

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
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios";
            foreach (string scenario in Directory.GetDirectories(@"" + sc_path))
            {
                string[] sc_folder = scenario.Remove(0, sc_path.Length + 1).Split('.');
                Add_Scenario(sc_folder[1]);

                // Load existing situations in each scenario.
                foreach (string situation in Directory.GetDirectories(@"" + scenario))
                {
                    string[] si_folder = situation.Remove(0, scenario.Length + 1).Split('.');
                    Add_Situation(menu.Nodes.Find("scenarios", false)[0].LastNode.Name, si_folder[1]);
                }
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
            // Empty the groupbox from precedent controls.
            int nbr_ctrl = content.Controls.Count;
            for (int i = 0; i < nbr_ctrl; i++)
            {
                if(content.Controls[0].GetType() == new PictureBox().GetType())
                {
                    PictureBox pb = (PictureBox)content.Controls[0];
                    pb.Image.Dispose();
                }
                content.Controls[0].Dispose();
            }

            TreeView t = (TreeView) sender;
            String name = t.SelectedNode.Name;
            content.Text = t.SelectedNode.FullPath;

            if(name.StartsWith("scenario") && name != "scenarios")
            {
                Display_Scenario();
            }
            else if (name.StartsWith("situation"))
            {
                Display_Situation();
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
                    case "choices":
                        Display_Choices(); break;
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
            // Creation of controls linking properties from the game to the editor.


            // WIP
        }

        /// <summary>
        /// Load controls for characters' content.
        /// </summary>
        private void Display_Characters()
        {
            // WIP
        }

        /// <summary>
        /// Load controls for NPCs' content.
        /// </summary>
        private void Display_NPCs()
        {
            // WIP
        }

        /// <summary>
        /// Load controls for Choices' content.
        /// </summary>
        private void Display_Choices()
        {
            // WIP
        }

        /// <summary>
        /// Load controls for player's content.
        /// </summary>
        private void Display_Player()
        {
            // WIP
        }

        /// <summary>
        /// Load controls for scenarios' content.
        /// </summary>
        private void Display_Scenarios()
        {
            // Creation of a button allowing the creation of new scenarios.
            Button btn_add_scenario = new Button()
            {
                Name = "btn_add_scenario",
                Text = "Ajouter un nouveau scénario",
                AutoSize = true
            };
            btn_add_scenario.Click += new System.EventHandler(this.Add_Scenario);
            content.Controls.Add(btn_add_scenario);
            btn_add_scenario.Location = new Point((content.Size.Width - btn_add_scenario.Size.Width) / 2, 100);
        }

        /// <summary>
        /// Add a new node to the "scenarios" in the TreeView and initialize it.
        /// Called from the node "scenarios".
        /// </summary>
        /// <param name="sender">Control calling the method, here it will always be the button from "Scenario" content.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Add_Scenario(object sender, EventArgs e)
        {
            // Initialize the name and data structure.
            TreeNode parent = menu.Nodes.Find("scenarios", false)[0];
            string new_scenario = "Nouveau scénario";
            int nbr_new_scenarios = 0;
            int i = 0;
            while (i < parent.Nodes.Count)
            {
                nbr_new_scenarios = i;
                foreach (TreeNode t in parent.Nodes)
                {
                    if (t.Text.Equals("Nouveau scénario (" + i.ToString() + ")"))
                    {
                        nbr_new_scenarios = 0;
                        break;
                    }
                }
                if (nbr_new_scenarios != 0) { break; }
                i++;
            }
            if (nbr_new_scenarios != 0) { new_scenario = "Nouveau scénario (" + nbr_new_scenarios.ToString() + ")"; }
            Add_Scenario(new_scenario);

            // Create the new scenario in the appropriate folder.
            Directory.CreateDirectory(@"" + this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar + parent.LastNode.Name.Substring(8) + "." + new_scenario);

            // Add a "properties.json" to the newly created folder.
            JObject properties_content = new JObject();
            properties_content["description"] = "Description par défaut";
            File.WriteAllText(@"" + this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar + parent.LastNode.Name.Substring(8)
                              + "." + new_scenario + Path.DirectorySeparatorChar + "properties.json",
                              properties_content.ToString());

            // Select the newly created TreeNode.
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
        /// Load a specified scenario into the "content" groupbox.
        /// </summary>
        private void Display_Scenario()
        {
            // Creation of all controls.

            // Creation of a button allowing to discard the scenario.
            Button btn_discard_scenario = new Button()
            {
                Name = "btn_discard_scenario",
                Text = "X",
                AutoSize = true
            };
            btn_discard_scenario.Click += new System.EventHandler(this.Discard_Scenario);
            content.Controls.Add(btn_discard_scenario);

            // Creation of two arrows allowing changement of the scenarios' order.
            PictureBox pb_down_scenario = new PictureBox()
            {
                Name = "pb_down_scenario",
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" + 
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_down.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_down_scenario.Click += new EventHandler(this.Down_Scenario);
            content.Controls.Add(pb_down_scenario);

            PictureBox pb_up_scenario = new PictureBox()
            {
                Name = "pb_up_scenario",
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_up.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_up_scenario.Click += new EventHandler(this.Up_Scenario);
            content.Controls.Add(pb_up_scenario);

            // Creation of a button allowing to rename the scenario.
            PictureBox pb_rename_scenario = new PictureBox()
            {
                Name = "pb_rename_scenario",
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "edit.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_rename_scenario.Click += new EventHandler(this.Ask_Rename_Scenario);
            content.Controls.Add(pb_rename_scenario);

            // Creation of the hidden textbox allowing to enter a new scenario's name.
            TextBox txt_rename_scenario = new TextBox()
            {
                Name = "txt_rename_scenario",
                Text = menu.SelectedNode.Text,
                Width = (menu.SelectedNode.Text.Length * 12) + 20,
                ShortcutsEnabled = false,
                Visible = false
            };
            txt_rename_scenario.KeyPress += new KeyPressEventHandler(this.Rename_Scenario_Txt_Keypress);
            content.Controls.Add(txt_rename_scenario);

            // Creation of the button responsible for the situations' creation.
            Button btn_add_situation = new Button()
            {
                Name = "btn_add_situation",
                Text = "Ajouter une situation",
                AutoSize = true
            };
            btn_add_situation.Click += new EventHandler(this.Add_Situation);
            content.Controls.Add(btn_add_situation);

            // Set the correct location of the controls (responsive with the groupbox's size).
            btn_discard_scenario.Location = new Point(content.Size.Width - btn_discard_scenario.Size.Width, 10);
            pb_rename_scenario.Location = new Point((content.Text.Length * 9) + 20, 0);
            pb_down_scenario.Location = new Point(pb_rename_scenario.Location.X + pb_rename_scenario.Width + 8, 0);
            pb_up_scenario.Location = new Point(pb_down_scenario.Location.X + pb_down_scenario.Width + 2, 0);
            txt_rename_scenario.Location = new Point(((content.Text.Length - menu.SelectedNode.Text.Length) * 10) - 12, 0);

            btn_add_situation.Location = new Point((content.Size.Width - btn_add_situation.Size.Width) / 2, 100);
        }

        private void Down_Scenario(object sender, EventArgs e)
        {
            int index = menu.SelectedNode.Index;
            TreeNode tns = menu.Nodes.Find("scenarios", false)[0];

            // Return if last node.
            if (index == tns.LastNode.Index) { return; }
            TreeNode tn = menu.SelectedNode;
            tns.Nodes.Remove(menu.SelectedNode);
            tns.Nodes.Insert(index + 1, tn);
            menu.SelectedNode = tn;
            Order_Files_Scenarios();
        }

        private void Up_Scenario(object sender, EventArgs e)
        {
            int index = menu.SelectedNode.Index;
            TreeNode tns = menu.Nodes.Find("scenarios", false)[0];

            // Return if fisrt node.
            if (index == 0) { return; }
            TreeNode tn = menu.SelectedNode;
            tns.Nodes.Remove(menu.SelectedNode);
            tns.Nodes.Insert(index - 1, tn);
            menu.SelectedNode = tn;
            Order_Files_Scenarios();
        }

        private void Order_Files_Scenarios()
        {
            TreeNode tns = menu.Nodes.Find("scenarios", false)[0];
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios";
            foreach (string s in Directory.GetDirectories(@"" + sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length + 1).Split('.');

                // Get the corresponding index for the input text from the folder.
                int act_index = 0;
                int i = 0;
                while (i < tns.Nodes.Count)
                {
                    if (tns.Nodes[i].Text == folder[1])
                    {
                        act_index = i;
                        i = tns.Nodes.Count;
                    }
                    i++;
                }

                // Rename the folder if the index changed.
                if (int.Parse(folder[0]) - 1 != act_index)
                {
                    int str_index = act_index + 1;
                    Directory.Move(@"" + s, @"" + sc_path + Path.DirectorySeparatorChar + str_index.ToString() + "." + folder[1]);
                }
            }

            // Reorder nominally the rest of scenarios.
            TreeNode sc = menu.Nodes.Find("scenarios", false)[0];
            for (int i = 0; i < sc.Nodes.Count; i++)
            {
                sc.Nodes[i].Name = "scenario" + (i + 1).ToString();
            }
        }

        /// <summary>
        /// Makes visible the textbox permitting to rename a scenario.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Ask_Rename_Scenario(object sender, EventArgs e)
        {
            // Set the textbox of the name as visible.
            content.Controls.Find("txt_rename_scenario", false)[0].Visible = true;
            content.Controls.Find("pb_rename_scenario", false)[0].Visible = false;
            content.Controls.Find("txt_rename_scenario", false)[0].Focus();
        }

        /// <summary>
        /// Checks each keypress from the textbox responsible of renaming a scenario.
        /// Allows only alphanumerics as content.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method, here details from the keypress.</param>
        private void Rename_Scenario_Txt_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (e.KeyChar == (char) 13) // (char) 13 => Enter.
            {
                // Block the renaming of a scenario if the new name is empty.
                if(t.Text.Equals(string.Empty)) { return; }
                Rename_Scenario(t.Text);
            }
            else if(e.KeyChar == (char) 27) // (char) 27 => Escape.
            {
                t.Visible = false;
                content.Controls.Find("pb_rename_scenario", false)[0].Visible = true;
            }
            else if(!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == (char) 8)) // (char) 8 => Backspace.
            {
                e.Handled = true;
            }
            else if(t.Text.Length > 32)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Rename a scenario with a custom name, rename the scenario's folder and Node.
        /// </summary>
        /// <param name="new_name">Name remplacing the precedent as identifier of the scenario.</param>
        private void Rename_Scenario(string new_name)
        {
            // Exit if name is similar.
            if(menu.SelectedNode.Text == new_name) { return; }

            // Exit and display an error message if the name is already in use.
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios";
            foreach (string s in Directory.GetDirectories(sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length + 1).Split('.');
                if(folder[1] == new_name)
                {
                    MessageBox.Show("Le nom de scénario " + '"' + new_name + '"' + " est déjà utilisé, essayez un autre nom.",
                                    "Nom de scénario invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Rename the scenario's folder.
            Directory.Move(@"" + sc_path + Path.DirectorySeparatorChar + menu.SelectedNode.Name.Substring(8) + "." + menu.SelectedNode.Text,
                           @"" + sc_path + Path.DirectorySeparatorChar + menu.SelectedNode.Name.Substring(8) + "." + new_name);

            // Rename the scenario's Node.
            menu.SelectedNode.Text = new_name;
            content.Text = menu.SelectedNode.FullPath;

            // Repositioning size-sensitives contents.
            TextBox t = (TextBox) content.Controls.Find("txt_rename_scenario", false)[0];
            t.Visible = false;
            t.Width = (menu.SelectedNode.Text.Length * 10) + 20;
            content.Controls.Find("pb_rename_scenario", false)[0].Location = new Point((content.Text.Length * 9) + 20, 0);
            content.Controls.Find("pb_rename_scenario", false)[0].Visible = true;
        }

        /// <summary>
        /// Delete an existing scenario, taking the one currently selected as target.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Discard_Scenario(object sender, EventArgs e)
        {
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios";

            // Ask for confirmation before suppression of the scenario.
            if ((MessageBox.Show("Confirmer la suppression du scénario " + menu.SelectedNode.Text + " ?", "Confirmation de suppression", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)) {
                return;
            }

            // Remove the scenario's folder.
            Directory.Delete(@"" + sc_path + Path.DirectorySeparatorChar + menu.SelectedNode.Name.Substring(8) + "." + menu.SelectedNode.Text, true);

            // Remove the scenario's Node.
            menu.Nodes.Remove(menu.SelectedNode);

            TreeNode sc = menu.Nodes.Find("scenarios", false)[0];
            menu.SelectedNode = sc;

            // Reorder nominally the rest of scenarios.
            for(int i = 0; i < sc.Nodes.Count; i++)
            {
                sc.Nodes[i].Name = "scenario" + (i + 1).ToString();
            }
            int j = 1;
            foreach (string s in Directory.GetDirectories(@"" + sc_path))
            {
                string folder = s.Remove(0, sc_path.Length + 1);
                if (j.ToString() != folder.Split('.')[0])
                {
                    Directory.Move(@"" + sc_path + Path.DirectorySeparatorChar + folder, @"" + sc_path + Path.DirectorySeparatorChar + j.ToString() + "." + folder.Split('.')[1]);
                }
                j++;
            }
        }

        public void Add_Situation(object sender, EventArgs e)
        {
            // Initialize the name and data structure.
            TreeNode parent = menu.SelectedNode;
            string new_situation = "Nouvelle situation";
            int nbr_new_situations = 0;
            int i = 1;
            while (i < parent.Nodes.Count + 1)
            {
                nbr_new_situations = i;
                foreach (TreeNode t in parent.Nodes)
                {
                    if (t.Text.Equals("Nouvelle situation (" + i.ToString() + ")"))
                    {
                        nbr_new_situations = 0;
                        break;
                    }
                }
                if (nbr_new_situations != 0) { break; }
                i++;
            }
            if (nbr_new_situations != 0) { new_situation = "Nouvelle situation (" + nbr_new_situations.ToString() + ")"; }
            Add_Situation(parent.Name, new_situation);

            String access_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                                 (parent.Index + 1).ToString() + "." + parent.Text + Path.DirectorySeparatorChar;

            // Create the new scenario in the appropriate folder.
            Directory.CreateDirectory(@"" + access_path + parent.Nodes.Count.ToString() + "." + new_situation);

            // Add a "dialogs.json" to the newly created folder.
            JObject dialogs_content = new JObject();
            File.WriteAllText(@"" + access_path + parent.Nodes.Count.ToString() + "." + new_situation + Path.DirectorySeparatorChar + "dialogs.json",
                              dialogs_content.ToString());

            // Add a "environment.json" to the folder.
            JObject environment_content = new JObject();
            environment_content["background"] = "default";
            environment_content["scene_type"] = "tram_entrance";
            File.WriteAllText(@"" + access_path + parent.Nodes.Count.ToString() + "." + new_situation + Path.DirectorySeparatorChar + "environment.json",
                              environment_content.ToString());

            // Select the newly created TreeNode.
            menu.SelectedNode = parent.LastNode;
        }

        private void Add_Situation(string scenario, string new_situation)
        {
            // Initialize necessary variables.
            TreeNode tn_new_situation = new TreeNode();
            TreeNode tn_parent = menu.Nodes.Find(scenario, true)[0];
            int nbr_existing_scenarios = tn_parent.Nodes.Count + 1;

            // Configure the TreeNode associated to the new scenario.
            tn_new_situation.Name = "situation" + (tn_parent.Index + 1) + "_" + nbr_existing_scenarios.ToString();
            tn_new_situation.Text = new_situation;
            tn_parent.Nodes.Add(tn_new_situation);
        }

        public void Display_Situation()
        {
            // Creation of all controls.

            // Creation of a button allowing to discard the situation.
            Button btn_discard_situation = new Button()
            {
                Name = "btn_discard_situation",
                Text = "X",
                AutoSize = true
            };
            btn_discard_situation.Click += new System.EventHandler(this.Discard_Situation);
            content.Controls.Add(btn_discard_situation);

            // Creation of two arrows allowing changement of the scenarios' order.
            PictureBox pb_down_situation = new PictureBox()
            {
                Name = "pb_down_situation",
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_down.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_down_situation.Click += new EventHandler(this.Down_Situation);
            content.Controls.Add(pb_down_situation);

            PictureBox pb_up_situation = new PictureBox()
            {
                Name = "pb_up_situation",
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_up.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_up_situation.Click += new EventHandler(this.Up_Situation);
            content.Controls.Add(pb_up_situation);

            // Creation of a button allowing to rename the scenario.
            PictureBox pb_rename_situation = new PictureBox()
            {
                Name = "pb_rename_situation",
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "edit.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_rename_situation.Click += new EventHandler(this.Ask_Rename_Situation);
            content.Controls.Add(pb_rename_situation);

            // Creation of the hidden textbox allowing to enter a new scenario's name.
            TextBox txt_rename_situation = new TextBox()
            {
                Name = "txt_rename_situation",
                Text = menu.SelectedNode.Text,
                Width = (menu.SelectedNode.Text.Length * 12) + 20,
                ShortcutsEnabled = false,
                Visible = false
            };
            txt_rename_situation.KeyPress += new KeyPressEventHandler(this.Rename_Situation_Txt_Keypress);
            content.Controls.Add(txt_rename_situation);

            // Set the correct location of the controls (responsive with the groupbox's size).
            btn_discard_situation.Location = new Point(content.Size.Width - btn_discard_situation.Size.Width, 10);
            pb_rename_situation.Location = new Point((content.Text.Length * 9) + 20, 0);
            pb_down_situation.Location = new Point(pb_rename_situation.Location.X + pb_rename_situation.Width + 8, 0);
            pb_up_situation.Location = new Point(pb_down_situation.Location.X + pb_down_situation.Width + 2, 0);
            txt_rename_situation.Location = new Point(((content.Text.Length - menu.SelectedNode.Text.Length) * 10) - 48, 0);
        }

        private void Down_Situation(object sender, EventArgs e)
        {
            int index = menu.SelectedNode.Index;
            TreeNode tns = menu.SelectedNode.Parent;

            // Return if last node.
            if (index == tns.LastNode.Index) { return; }
            TreeNode tn = menu.SelectedNode;
            tns.Nodes.Remove(menu.SelectedNode);
            tns.Nodes.Insert(index + 1, tn);
            menu.SelectedNode = tn;
            Order_Files_Situations();
        }

        private void Up_Situation(object sender, EventArgs e)
        {
            int index = menu.SelectedNode.Index;
            TreeNode tns = menu.SelectedNode.Parent;

            // Return if fisrt node.
            if (index == 0) { return; }
            TreeNode tn = menu.SelectedNode;
            tns.Nodes.Remove(menu.SelectedNode);
            tns.Nodes.Insert(index - 1, tn);
            menu.SelectedNode = tn;
            Order_Files_Situations();
        }

        private void Order_Files_Situations()
        {
            TreeNode tns = menu.SelectedNode.Parent;
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                             +(menu.SelectedNode.Parent.Index + 1) + "." + menu.SelectedNode.Parent.Text;
            foreach (string s in Directory.GetDirectories(@"" + sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length + 1).Split('.');

                // Get the corresponding index for the input text from the folder.
                int act_index = 0;
                int i = 0;
                while (i < tns.Nodes.Count)
                {
                    if (tns.Nodes[i].Text == folder[1])
                    {
                        act_index = i;
                        i = tns.Nodes.Count;
                    }
                    i++;
                }

                // Rename the folder if the index changed.
                if (int.Parse(folder[0]) - 1 != act_index)
                {
                    int str_index = act_index + 1;
                    Directory.Move(@"" + s, @"" + sc_path + Path.DirectorySeparatorChar + str_index.ToString() + "." + folder[1]);
                }
            }

            // Reorder nominally the rest of scenarios.
            TreeNode sc = menu.Nodes.Find("scenarios", false)[0];
            for (int i = 0; i < sc.Nodes.Count; i++)
            {
                sc.Nodes[i].Name = "scenario" + (i + 1).ToString();
            }
        }

        private void Ask_Rename_Situation(object sender, EventArgs e)
        {
            // Set the textbox of the name as visible.
            content.Controls.Find("txt_rename_situation", false)[0].Visible = true;
            content.Controls.Find("pb_rename_situation", false)[0].Visible = false;
            content.Controls.Find("txt_rename_situation", false)[0].Focus();
        }

        private void Rename_Situation_Txt_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (e.KeyChar == (char)13) // (char) 13 => Enter.
            {
                // Block the renaming of a situation if the new name is empty.
                if (t.Text.Equals(string.Empty)) { return; }
                Rename_Situation(t.Text);
            }
            else if (e.KeyChar == (char)27) // (char) 27 => Escape.
            {
                t.Visible = false;
                content.Controls.Find("pb_rename_situation", false)[0].Visible = true;
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == (char)8)) // (char) 8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 32)
            {
                e.Handled = true;
            }
        }

        private void Rename_Situation(string new_name)
        {
            // Exit if name is similar.
            if (menu.SelectedNode.Text == new_name) { return; }

            // Exit and display an error message if the name is already in use.
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                             +(menu.SelectedNode.Parent.Index + 1) + "." + menu.SelectedNode.Parent.Text;
            foreach (string s in Directory.GetDirectories(sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length + 1).Split('.');
                if (folder[1] == new_name)
                {
                    MessageBox.Show("Le nom de la situation " + '"' + new_name + '"' + " est déjà utilisé, essayez un autre nom.",
                                    "Nom de scénario invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Rename the situation's folder.
            Directory.Move(@"" + sc_path + Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text,
                           @"" + sc_path + Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + new_name);

            // Rename the situation's Node.
            menu.SelectedNode.Text = new_name;
            content.Text = menu.SelectedNode.FullPath;

            // Repositioning size-sensitives contents.
            TextBox t = (TextBox)content.Controls.Find("txt_rename_situation", false)[0];
            t.Visible = false;
            t.Width = (menu.SelectedNode.Text.Length * 10) + 20;
            content.Controls.Find("pb_rename_situation", false)[0].Location = new Point((content.Text.Length * 9) + 20, 0);
            content.Controls.Find("pb_down_situation", false)[0].Location = new Point(content.Controls.Find("pb_rename_situation",
                                  false)[0].Location.X + content.Controls.Find("pb_rename_situation", false)[0].Width + 8, 0);
            content.Controls.Find("pb_up_situation", false)[0].Location = new Point(content.Controls.Find("pb_down_situation",
                                  false)[0].Location.X + content.Controls.Find("pb_down_situation", false)[0].Width + 2, 0);
            content.Controls.Find("pb_rename_situation", false)[0].Visible = true;
        }

        public void Discard_Situation(object sender, EventArgs e)
        {
            TreeNode select_node = menu.SelectedNode;

            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                             +(select_node.Parent.Index + 1) + "." + select_node.Parent.Text;

            // Ask for confirmation before suppression of the scenario.
            if ((MessageBox.Show("Confirmer la suppression de la situation " + select_node.Text + " ?", "Confirmation de suppression",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No))
            {
                return;
            }

            // Remove the situation's folder.
            Directory.Delete(@"" + sc_path + Path.DirectorySeparatorChar + (select_node.Index + 1) + "." + select_node.Text, true);

            TreeNode sc = select_node.Parent;
            menu.SelectedNode = sc;

            // Remove the situation's Node.
            menu.Nodes.Remove(select_node);

            // Reorder nominally the rest of scenarios.
            for (int i = 0; i < sc.Nodes.Count; i++)
            {
                sc.Nodes[i].Name = "situation" + (i + 1).ToString();
            }
            int j = 1;
            foreach (string s in Directory.GetDirectories(@"" + sc_path))
            {
                string folder = s.Remove(0, sc_path.Length + 1);
                if (j.ToString() != folder.Split('.')[0])
                {
                    Directory.Move(@"" + sc_path + Path.DirectorySeparatorChar + folder, @"" + sc_path + Path.DirectorySeparatorChar + j.ToString() + "." + folder.Split('.')[1]);
                }
                j++;
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
        /// Recover the content of a JSON file at a specified path.
        /// </summary>
        /// <param name="internal_path">Path from the game folder to the targeted JSON file.</param>
        /// <returns>Content of the JSON file under a JObject structure.</returns>
        public JObject Get_From_JSON(string internal_path)
        {
            JObject output;
            using (StreamReader stream_r = new StreamReader(@"" + this.game_path + internal_path))
            {
                string json_file = stream_r.ReadToEnd();
                output = JObject.Parse(json_file);
            }
            return output;
        }

        /// <summary>
        /// Set the content of the JSON file at the specified path.
        /// </summary>
        /// <param name="internal_path">Path from the game folder to the targeted JSON file.</param>
        /// <param name="new_content">JObject containing the variables needed in the file.</param>
        public void Set_To_JSON(string internal_path, JObject new_content)
        {
            File.WriteAllText(@"" + this.game_path + internal_path, new_content.ToString());

            // Write JSON directly to the specified file.
            using (StreamWriter file = File.CreateText(@"" + this.game_path + internal_path))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                new_content.WriteTo(writer);
            }
        }
    }
}
