using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Learn_CTS
{
    public partial class Editor : Form
    {

        // Attributes.

        private readonly String game;
        private readonly String game_path;
        private JObject game_properties;
        private string old_category = "general";
        private bool saved;

        // Methods.

        /// <summary>
        /// Initialize the whole Form, as a constructor should.
        /// </summary>
        public Editor(String game)
        {
            InitializeComponent();
            this.game = game;
            this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game;
            this.Text = "Éditeur : " + game;
            this.DoubleBuffered = false;
        }

        public void Set_Saved(bool saved)
        {
            this.saved = saved;
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous tout de même quitter ?",
                                "Confirmation de fermeture", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Restart the application when closing the editor.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Marks the current game as available.
            JObject properties = Tools.Get_From_JSON(this.game_path + Path.DirectorySeparatorChar + "properties.json");
            if(properties["state"].ToString().Substring(0, 8).Equals("[DENIED]"))
            {
                properties["state"] = properties["state"].ToString().Substring(8);
            }
            else
            {
                properties["state"] = "Inactif.";
            }
            Tools.Set_To_JSON(this.game_path + Path.DirectorySeparatorChar + "properties.json", properties);

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
            this.game_properties = Tools.Get_From_JSON(this.game_path + Path.DirectorySeparatorChar + "properties.json");
            this.saved = true;
            if(!this.game_properties["state"].ToString().Equals("Inactif."))
            {
                if (MessageBox.Show("Le jeu " + '"' + this.game + '"' + " est déjà en cours d'édition ou d'utilisation sur cette machine.\nSouhaitez-vous tout de même y accéder ?",
                                "Jeu en utilisation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                {
                    this.game_properties["state"] = "[DENIED]" + this.game_properties["state"];
                    Tools.Set_To_JSON(this.game_path + Path.DirectorySeparatorChar + "properties.json", this.game_properties);
                    this.Close();
                    return;
                }
            }
            else
            {
                this.game_properties["state"] = "En cours d'édition...";
                Tools.Set_To_JSON(this.game_path + Path.DirectorySeparatorChar + "properties.json", this.game_properties);
            }

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
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Location.Y);

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
            menu.SelectedNode = menu.Nodes[0];
        }

        /// <summary>
        /// Ask for confirmation before switching from selected item if the current dataset was modified.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Menu_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous les abandonner ?",
                                "Confirmation d'abandon de modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                this.saved = true;
            }
        }

        /// <summary>
        /// Change the content display when the user select another category from the menu.
        /// </summary>
        /// <param name="sender">Control calling the method, here it will always be the menu.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Menu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Tools.Begin_Control_Update(content);

            TreeView t = (TreeView)sender;
            string name = t.SelectedNode.Name;
            lbl_path.Text = t.SelectedNode.FullPath;
            List<String> keeping_categories = new List<String>()
            { "npcs", "dialogs" };

            if (!(this.old_category.Equals(name) && keeping_categories.Contains(name)))
            {
                // Empty the groupbox from precedent controls.
                int nbr_ctrl = content.Controls.Count;
                for (int i = 0; i < nbr_ctrl; i++)
                {
                    if (content.Controls[0].GetType() == new PictureBox().GetType())
                    {
                        PictureBox pb = (PictureBox)content.Controls[0];
                        pb.Image.Dispose();
                    }
                    content.Controls[0].Dispose();
                }
            }

            if (name.StartsWith("scenario") && name != "scenarios")
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
                    case "models":
                        Display_Models(); break;
                    case "player":
                        Display_Player(); break;
                    case "npcs":
                        Display_NPCs(); break;
                    case "dialogs":
                        Display_Dialogs(); break;
                    case "images":
                        Display_Images(); break;
                    case "sprites":
                        Display_Sprites(); break;
                    case "backgrounds":
                        Display_Backgrounds(); break;
                    case "scenarios":
                        Display_Scenarios(); break;
                    default:
                        throw new ArgumentException("Category not yet implemented: " + t.SelectedNode.Text);
                }
            }
            this.old_category = name;

            Tools.End_Control_Update(content);
        }

        /// <summary>
        /// Load controls for general content.
        /// </summary>
        private void Display_Global()
        {
            // Creation of controls linking properties from the game to the editor.

            // Creation of the "Description" label.
            Label lbl_desc = new Label()
            {
                Name = "lbl_desc",
                Text = "Description",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_desc);

            // Creation of textbox containing the current game description.
            TextBox txt_desc = new TextBox()
            {
                Name = "txt_desc",
                Text = (string)this.game_properties["description"],
                Cursor = Cursors.IBeam,
                Multiline = true,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(56, 56, 56),
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(8, 8, 8, 8),
                ShortcutsEnabled = false
            };
            txt_desc.Width = content.Width - 100;
            txt_desc.Height = ((int)((txt_desc.Text.Length * 12) / txt_desc.Width) + 1) * 40;
            txt_desc.KeyPress += new KeyPressEventHandler(this.Desc_Txt_Keypress);
            content.Controls.Add(txt_desc);

            // Creation of a label used for showing messages regarding the description's modification.
            Label lbl_desc_state = new Label()
            {
                Name = "lbl_desc_state",
                Text = "",
                ForeColor = Color.Red,
                AutoSize = true
            };
            content.Controls.Add(lbl_desc_state);

            // Set the correct location of the controls (responsive with the groupbox's size).
            lbl_desc.Location = new Point(20, 20);
            txt_desc.Location = new Point(20, lbl_desc.Location.Y + lbl_desc.Height + 20);
            lbl_desc_state.Location = new Point(20, txt_desc.Location.Y + txt_desc.Height + 8);
        }

        /// <summary>
        /// Handle all keypresses and validate, cancel or even forbid the action.
        /// Upon validation will save the new description in the "properties.json" file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Desc_Txt_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '.', ',', '\'', '?', '!', '-', '°', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                // Block the renaming if the new name is empty.
                if (t.Text.Equals(string.Empty)) { return; }
                this.game_properties["description"] = t.Text;
                Tools.Set_To_JSON(this.game_path + Path.DirectorySeparatorChar + "properties.json", this.game_properties); // Set the entered description as valid description.
                t.Height = ((int)((t.Text.Length * 12) / t.Width) + 1) * 40;
                t.BackColor = Color.FromArgb(56, 56, 56);
                this.saved = true;
                content.Controls.Find("lbl_desc_state", false)[0].Text = "";

                // Resize all controls inside "content".
                Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)this.game_properties["description"];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(56, 56, 56);
                this.saved = true;
                content.Controls.Find("lbl_desc_state", false)[0].Text = "";
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || autorized_chars.Contains(e.KeyChar) || e.KeyChar == (char)8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 512) // Avoid endless descriptions.
            {
                if (e.KeyChar == (char)8) // Still backspace.
                {
                    t.BackColor = Color.FromArgb(56, 32, 32);
                    content.Controls.Find("lbl_desc_state", false)[0].Text = "Sauvegardez en appuyant sur 'Entrée' ou annulez avec 'Echap'";
                    this.saved = false;
                    return; // Let you erase regardless of the length.
                }
                content.Controls.Find("lbl_desc_state", false)[0].Text = "Limite de caractères atteinte !";
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(56, 32, 32);
                content.Controls.Find("lbl_desc_state", false)[0].Text = "Sauvegardez en appuyant sur 'Entrée' ou annulez avec 'Echap'";
                this.saved = false;
            }
        }

        /// <summary>
        /// Load controls for models' content.
        /// </summary>
        private void Display_Models()
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
        /// Load controls for NPCs' content.
        /// </summary>
        private void Display_NPCs()
        {
            if (this.old_category.Equals("npcs")) // Trigger only on form resize.
            {
                TableLayoutPanel tlp = (TableLayoutPanel)content.Controls.Find("tlp_npcs", false)[0];
                Button btn_add = (Button)content.Controls.Find("btn_add_lib_npc", false)[0];

                tlp.Width = content.Width - 80;
                btn_add.Location = new Point((content.Width - btn_add.Width) / 2, tlp.Location.Y + tlp.Height + 8);
                return;
            }

            // Creation of controls linking NPCs files to the editor.

            // Label annoncing the following NPCs management.
            Label lbl_npcs = new Label()
            {
                Name = "lbl_npcs",
                Text = "Gestion des figurants",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_npcs);

            PictureBox pb_add_lib_npc = new PictureBox()
            {
                Name = "pb_add_lib_npc",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_lib_npc.Click += new EventHandler(this.Add_Lib_NPC);
            content.Controls.Add(pb_add_lib_npc);

            // TableLayoutPanel, keeping the generated elements in rows.
            TableLayoutPanel tlp_npcs = new TableLayoutPanel()
            {
                Name = "tlp_npcs",
                AutoScroll = true,
                Width = content.Width - 80,
                Height = 2,
                RowCount = 0,
                ColumnCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                Visible = false
            };
            content.Controls.Add(tlp_npcs);

            // Button allowing the creation of a new NPC.
            Button btn_add_lib_npc = new Button()
            {
                Name = "btn_add_lib_npc",
                Text = "Ajouter un personnage",
                AutoSize = true
            };
            btn_add_lib_npc.Click += new EventHandler(this.Add_Lib_NPC);
            content.Controls.Add(btn_add_lib_npc);

            // Set the correct location of the controls (responsive with the groupbox's size).
            lbl_npcs.Location = new Point(20, 20);
            pb_add_lib_npc.Location = new Point(lbl_npcs.Location.X + lbl_npcs.Width + 20, 20);
            tlp_npcs.Location = new Point(40, lbl_npcs.Location.Y + lbl_npcs.Height + 20);

            // Setup the columns sizes.
            tlp_npcs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.47));
            tlp_npcs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.47));
            tlp_npcs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)0.06));

            // Generates all data in tlp_npcs.
            int i = 0;
            JObject data_pnj;
            string npcs_folder_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                                        Path.DirectorySeparatorChar + "npcs";
            foreach (string file_npc in Directory.GetFiles(npcs_folder_path))
            {
                data_pnj = Tools.Get_From_JSON(npcs_folder_path + Path.DirectorySeparatorChar + (i + 1) + ".json");
                
                tlp_npcs.Height += 42;
                tlp_npcs.RowCount++;
                tlp_npcs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

                TextBox npc_name = new TextBox()
                {
                    Name = "npc_name" + (i + 1),
                    Text = (string)data_pnj["name"],
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(56, 56, 56),
                    Dock = DockStyle.Fill,
                    ShortcutsEnabled = false,
                    AutoSize = true
                };
                npc_name.KeyPress += new KeyPressEventHandler(Text_Changed_Lib_NPC);
                tlp_npcs.Controls.Add(npc_name, 0, i);

                TextBox npc_folder = new TextBox()
                {
                    Name = "npc_folder" + (i + 1),
                    Text = (string)data_pnj["folder"],
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(56, 56, 56),
                    Dock = DockStyle.Fill,
                    ShortcutsEnabled = false,
                    AutoSize = true
                };
                npc_folder.KeyPress += new KeyPressEventHandler(Text_Changed_Lib_NPC);
                tlp_npcs.Controls.Add(npc_folder, 1, i);

                PictureBox npc_discard = new PictureBox()
                {
                    Name = "npc_discard" + (i + 1),
                    Cursor = Cursors.Hand,
                    Dock = DockStyle.Fill,
                    Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                           Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "gamecard-delete-btn-x64.png"),
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                npc_discard.Click += new EventHandler(this.Discard_Lib_NPC);
                tlp_npcs.Controls.Add(npc_discard, 2, i);

                i++;
            }

            tlp_npcs.Visible = true;

            // Must takes the final size of "tlp_npcs" in order to be under it.
            btn_add_lib_npc.Location = new Point((content.Width - btn_add_lib_npc.Width) / 2, tlp_npcs.Location.Y + tlp_npcs.Height + 8);

            /*if (content.VerticalScroll.Enabled)
            {
                // Add some space under the whole structure.
                Panel pan_spacing = new Panel()
                {
                    Name = "pan_spacing",
                    Size = new Size(content.Width - 40, 10),
                    Location = new Point(20, content.VerticalScroll.Maximum + 30)
                };
                content.Controls.Add(pan_spacing);
            }*/
        }

        public void Add_Lib_NPC(object sender, EventArgs e)
        {
            // Generates the new NPC's id.
            string npcs_folder_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                      Path.DirectorySeparatorChar + "npcs";
            int new_id = Directory.GetFiles(npcs_folder_path).Length + 1;

            // Creating a new file for the NPC's data.
            JObject npc_content = new JObject()
            {
                ["name"] = "Nom",
                ["folder"] = ""
            };
            File.WriteAllText(@"" + npcs_folder_path + Path.DirectorySeparatorChar + new_id + ".json",
                              npc_content.ToString());

            // Updating the display.
            TableLayoutPanel tlp_npcs = (TableLayoutPanel)content.Controls.Find("tlp_npcs", false)[0];
            Button btn_add_lib_npc = (Button)content.Controls.Find("btn_add_lib_npc", false)[0];

            Tools.Begin_Control_Update(tlp_npcs);

            tlp_npcs.Height += 42;
            tlp_npcs.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            TextBox npc_name = new TextBox()
            {
                Name = "npc_name" + new_id,
                Text = "Nom",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(56, 56, 56),
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            npc_name.KeyPress += new KeyPressEventHandler(Text_Changed_Lib_NPC);
            tlp_npcs.Controls.Add(npc_name, 0, new_id - 1);

            TextBox npc_folder = new TextBox()
            {
                Name = "npc_folder" + new_id,
                Text = "",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(56, 56, 56),
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            npc_folder.KeyPress += new KeyPressEventHandler(Text_Changed_Lib_NPC);
            tlp_npcs.Controls.Add(npc_folder, 1, new_id - 1);

            PictureBox npc_discard = new PictureBox()
            {
                Name = "npc_discard" + new_id,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "gamecard-delete-btn-x64.png"),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            npc_discard.Click += new EventHandler(this.Discard_Lib_NPC);
            tlp_npcs.Controls.Add(npc_discard, 2, new_id - 1);

            tlp_npcs.RowCount++;

            Tools.End_Control_Update(tlp_npcs);

            btn_add_lib_npc.Location = new Point((content.Width - btn_add_lib_npc.Width) / 2, tlp_npcs.Location.Y + tlp_npcs.Height + 8);
        }

        public void Text_Changed_Lib_NPC(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '?', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                // Recovering the id of the involved file.
                TableLayoutPanel flp = (TableLayoutPanel)t.Parent;
                int txt_id = flp.GetRow(t) + 1;
                int txt_type = flp.GetColumn(t);

                string txt_path = Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                                  "npcs" + Path.DirectorySeparatorChar + txt_id + ".json";
                JObject data_npc = Tools.Get_From_JSON(this.game_path + txt_path);
                switch (txt_type)
                {
                    case 0:
                        data_npc["name"] = t.Text;
                        break;
                    case 1:
                        data_npc["folder"] = t.Text;
                        break;
                }
                Tools.Set_To_JSON(this.game_path + txt_path, data_npc); // Set the entered setting as a stored blueprint for npc.
                t.BackColor = Color.FromArgb(56, 56, 56);
                this.saved = true;
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                // Recovering the id of the involved file.
                TableLayoutPanel flp = (TableLayoutPanel)t.Parent;
                int txt_id = flp.GetRow(t) + 1;
                int txt_type = flp.GetColumn(t);

                string txt_path = Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                                  "npcs" + Path.DirectorySeparatorChar + txt_id + ".json";
                JObject data_npc = Tools.Get_From_JSON(this.game_path + txt_path);
                switch (txt_type)
                {
                    case 0:
                        t.Text = (string)data_npc["name"];
                        break;
                    case 1:
                        t.Text = (string)data_npc["folder"];
                        break;
                }
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(56, 56, 56);
                this.saved = true;
                e.Handled = true;
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || autorized_chars.Contains(e.KeyChar) || e.KeyChar == (char)8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 32) // Avoid endless names.
            {
                if (e.KeyChar == (char)8) // Still backspace.
                {
                    t.BackColor = Color.FromArgb(56, 32, 32);
                    this.saved = false;
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(56, 32, 32);
                this.saved = false;
            }
        }

        public void Discard_Lib_NPC(object sender, EventArgs e)
        {
            // Recovering the id of the involved file.
            Control ctrl = (Control)sender;
            TableLayoutPanel tlp = (TableLayoutPanel)ctrl.Parent;
            int del_id = tlp.GetRow(ctrl) + 1;

            // Removing the involved file.
            string npcs_folder_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                      Path.DirectorySeparatorChar + "npcs";
            File.Delete(npcs_folder_path + Path.DirectorySeparatorChar + del_id + ".json");

            // Reordering the others files.
            for (int i = del_id; i <= Directory.GetFiles(@"" + npcs_folder_path).Length; i++)
            {
                if (del_id <= i)
                {
                    Directory.Move(@"" + npcs_folder_path + Path.DirectorySeparatorChar + (i + 1) + ".json",
                                   @"" + npcs_folder_path + Path.DirectorySeparatorChar + i + ".json");
                }
            }

            Tools.Begin_Control_Update(tlp);

            // Updating the display.
            Button btn_add_lib_npc = (Button)content.Controls.Find("btn_add_lib_npc", false)[0];

            tlp.GetControlFromPosition(2, del_id - 1).Dispose();
            tlp.GetControlFromPosition(1, del_id - 1).Dispose();
            tlp.GetControlFromPosition(0, del_id - 1).Dispose();

            for(int i = del_id; i < tlp.RowCount; i++)
            {
                tlp.SetCellPosition(tlp.GetControlFromPosition(2, i), new TableLayoutPanelCellPosition(2, i - 1));
                tlp.SetCellPosition(tlp.GetControlFromPosition(1, i), new TableLayoutPanelCellPosition(1, i - 1));
                tlp.SetCellPosition(tlp.GetControlFromPosition(0, i), new TableLayoutPanelCellPosition(0, i - 1));
            }
            tlp.RowCount--;

            Tools.End_Control_Update(tlp);

            tlp.Height -= 42;
            btn_add_lib_npc.Location = new Point((content.Width - btn_add_lib_npc.Width) / 2, tlp.Location.Y + tlp.Height + 8);
        }

        /// <summary>
        /// Load controls for Choices' content.
        /// </summary>
        private void Display_Dialogs()
        {
            if (this.old_category == "dialogs") // Trigger only on form resize.
            {
                foreach (QuizzEdition qe in content.Controls.OfType<QuizzEdition>())
                {
                    qe.Width = content.Width - 80;
                }
                return;
            }

            // Generating basic Label & add PictureBox in the upper part of "content".
            Label lbl_dialogs = new Label()
            {
                Name = "lbl_dialogs",
                Text = "Dialogues",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_dialogs);

            PictureBox pb_add_dialog = new PictureBox()
            {
                Name = "pb_add_dialog",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_dialog.Click += new EventHandler(this.Add_Dialog);
            content.Controls.Add(pb_add_dialog);

            // Place the controls just created.
            lbl_dialogs.Location = new Point(20, 20);
            pb_add_dialog.Location = new Point(lbl_dialogs.Location.X + lbl_dialogs.Width + 20, 20);

            // Generating to all files' a QuizzEdition UserControl.
            int i = 1;
            int last_pos = 0;
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            foreach (string file in Directory.GetFiles(dialogs_path))
            {
                // Creating the UserControl responsible for the internal edition of the JSON file.
                QuizzEdition QEdition = new QuizzEdition(@"" + dialogs_path + i + ".json")
                {
                    Name = "QuizzEdition" + i,
                    Width = content.Width - 80,
                    Location = new Point(40, 100 + last_pos)
                };
                content.Controls.Add(QEdition);
                last_pos += QEdition.Height + 40;

                // Label over the UserControl, displaying dialog's id.
                Label lbl_dialog_id = new Label()
                {
                    Name = "lbl_dialog_id" + i,
                    Text = "N°" + i,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    AutoSize = true,
                    BackColor = Color.FromArgb(46, 46, 46),
                    BorderStyle = BorderStyle.FixedSingle
                };
                content.Controls.Add(lbl_dialog_id);

                lbl_dialog_id.Location = new Point(QEdition.Location.X, QEdition.Location.Y - lbl_dialog_id.Height + 1);
                i++;
            }
        }

        public void Add_Dialog(object sender, EventArgs e)
        {
            // Generates the new dialog's id.
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            int new_id = Directory.GetFiles(dialogs_path).Length + 1;

            // Creating a new file for the dialog's data.
            JObject dialog_content = new JObject()
            {
                ["question"] = "Quelle question placer ici ?",
                ["choices"] = 2,
                ["c1"] = new JObject()
                {
                    ["answer"] = "Réponse 1",
                    ["score"] = 0,
                    ["redirect"] = 0
                },
                ["c2"] = new JObject()
                {
                    ["answer"] = "Réponse 2",
                    ["score"] = 0,
                    ["redirect"] = 0
                }
            };
            File.WriteAllText(@"" + dialogs_path + Path.DirectorySeparatorChar + new_id + ".json",
                              dialog_content.ToString());

            // Creating the UserControl responsible for the internal edition of the JSON file.
            QuizzEdition QEdition = new QuizzEdition(@"" + dialogs_path + new_id + ".json")
            {
                Name = "QuizzEdition" + new_id,
                Width = content.Width - 80,
                Location = new Point(40, 100)
            };
            content.Controls.Add(QEdition);

            // Label over the UserControl, displaying dialog's id.
            Label lbl_dialog_id = new Label()
            {
                Name = "lbl_dialog_id" + new_id,
                Text = "N°" + new_id,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true,
                BackColor = Color.FromArgb(46, 46, 46),
                BorderStyle = BorderStyle.FixedSingle
            };
            content.Controls.Add(lbl_dialog_id);

            // Placing the controls created in the right place.
            Update_Dialogs();
        }

        /// <summary>
        /// Update positions of all "QuizzEdition" UserControls in content based on their heights.
        /// Called when height of one contained UserControl is changed through "Parent" attribute.
        /// </summary>
        public void Update_Dialogs()
        {
            int i = 1;
            int last_pos = 0;
            foreach (QuizzEdition qe in content.Controls.OfType<QuizzEdition>())
            {
                qe.Location = new Point(40, 100 + last_pos - content.VerticalScroll.Value);
                content.Controls.Find("lbl_dialog_id" + i, false)[0].Location = new Point(40, 100 + last_pos - content.VerticalScroll.Value -
                                                                                          content.Controls.Find("lbl_dialog_id" + i, false)[0].Height + 1);
                last_pos += qe.Height + 40;
                i++;
            }
        }

        public void Discard_Dialog(QuizzEdition sender)
        {
            // Ask for confirmation before suppression of the scenario.
            if ((MessageBox.Show("Confirmer la suppression du dialogue N°" + sender.Get_Id() + " ?", "Confirmation de suppression",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No))
            {
                return;
            }

            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            // Deleting sender file.
            File.Delete(@"" + dialogs_path + sender.Get_Id() + ".json");

            // Reordering files.
            for (int i = sender.Get_Id(); i <= Directory.GetFiles(@"" + dialogs_path).Length; i++)
            {
                if (sender.Get_Id() <= i)
                {
                    Directory.Move(@"" + dialogs_path + Path.DirectorySeparatorChar + (i + 1) + ".json",
                                   @"" + dialogs_path + Path.DirectorySeparatorChar + i + ".json");
                }
            }

            // Rerendering the whole content panel.
            this.old_category = "reload_injection";
            Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
        }

        private void Display_Images()
        {
            // WIP
        }

        private void Display_Sprites()
        {
            // WIP
        }

        private void Display_Backgrounds()
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
                Cursor = Cursors.Hand,
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
            JObject properties_content = new JObject()
            {
                ["description"] = "Description par défaut"
            };
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
        /// Load the selected scenario into the "content" groupbox.
        /// </summary>
        private void Display_Scenario()
        {
            // Creation of all controls.

            // Creation of two arrows allowing changement of the scenarios' order.
            PictureBox pb_down_scenario = new PictureBox()
            {
                Name = "pb_down_scenario",
                Cursor = Cursors.Hand,
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
                Cursor = Cursors.Hand,
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_up.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_up_scenario.Click += new EventHandler(this.Up_Scenario);
            content.Controls.Add(pb_up_scenario);

            // Creation of a label reminding the scenario's name.
            Label lbl_name_scenario = new Label()
            {
                Name = "lbl_name_scenario",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.Hand,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            lbl_name_scenario.Click += new EventHandler(this.Ask_Rename_Scenario);
            content.Controls.Add(lbl_name_scenario);
            
            // Creation of the hidden textbox allowing to enter a new scenario's name.
            TextBox txt_rename_scenario = new TextBox()
            {
                Name = "txt_rename_scenario",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.IBeam,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(56, 56, 56),
                ForeColor = Color.White,
                Width = Tools.Get_Text_Width(this, menu.SelectedNode.Text, 16) + 24,
                ShortcutsEnabled = false,
                Visible = false
            };
            txt_rename_scenario.KeyPress += new KeyPressEventHandler(this.Rename_Scenario_Txt_Keypress);
            content.Controls.Add(txt_rename_scenario);

            // Creation of a button allowing to rename the scenario.
            PictureBox pb_rename_scenario = new PictureBox()
            {
                Name = "pb_rename_scenario",
                Cursor = Cursors.Hand,
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "edit.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_rename_scenario.Click += new EventHandler(this.Ask_Rename_Scenario);
            content.Controls.Add(pb_rename_scenario);

            // Creation of a button allowing to discard the scenario.
            PictureBox pb_discard_scenario = new PictureBox()
            {
                Name = "pb_discard_scenario",
                Cursor = Cursors.Hand,
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "delete.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_discard_scenario.Click += new System.EventHandler(this.Discard_Scenario);
            content.Controls.Add(pb_discard_scenario);

            // Creation of the button responsible for the situations' creation.
            Button btn_add_situation = new Button()
            {
                Name = "btn_add_situation",
                Text = "Ajouter une situation",
                Cursor = Cursors.Hand,
                AutoSize = true
            };
            btn_add_situation.Click += new EventHandler(this.Add_Situation);
            content.Controls.Add(btn_add_situation);

            // Set the correct location of the controls (responsive with the groupbox's size).
            pb_down_scenario.Location = new Point(8, 0);
            pb_up_scenario.Location = new Point(pb_down_scenario.Location.X + pb_down_scenario.Width + 2, 0);
            lbl_name_scenario.Location = new Point(pb_up_scenario.Location.X + pb_up_scenario.Width + 8, 0);
            txt_rename_scenario.Location = new Point(lbl_name_scenario.Location.X, 0);
            pb_rename_scenario.Location = new Point(lbl_name_scenario.Location.X + lbl_name_scenario.Width, 0);
            pb_discard_scenario.Location = new Point(pb_rename_scenario.Location.X + pb_rename_scenario.Width + 2, 0);

            btn_add_situation.Location = new Point((content.Width - btn_add_situation.Width) / 2, 100);
        }

        /// <summary>
        /// Switch the selected scenario with the one upside it.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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

        /// <summary>
        /// Switch the selected scenario with the one upside it.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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

        /// <summary>
        /// Technical function re-ordering all scenarios after a switch, both in the menu and in the folder.
        /// </summary>
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
            content.Controls.Find("lbl_name_scenario", false)[0].Visible = false;
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
            if (e.KeyChar == (char) 13) // (char)13 => Enter.
            {
                // Block the renaming of a scenario if the new name is empty.
                if(t.Text.Equals(string.Empty)) { return; }
                Rename_Scenario(t.Text);
            }
            else if(e.KeyChar == (char) 27) // (char)27 => Escape.
            {
                t.Visible = false;
                content.Controls.Find("pb_rename_scenario", false)[0].Visible = true;
                content.Controls.Find("lbl_name_scenario", false)[0].Visible = true;
                e.Handled = true;
            }
            else if(!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == (char) 8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if(t.Text.Length > 32)
            {
                if (e.KeyChar == (char)8) { return; } // Let you erase even with length limit reached.
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
            if(menu.SelectedNode.Text == new_name.Trim()) { return; }

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
            lbl_path.Text = menu.SelectedNode.FullPath;

            // Repositioning size-sensitives contents.
            TextBox t = (TextBox) content.Controls.Find("txt_rename_scenario", false)[0];
            t.Visible = false;
            t.Width = (menu.SelectedNode.Text.Length * 10) + 20;
            content.Controls.Find("lbl_name_scenario", false)[0].Text = menu.SelectedNode.Text;
            content.Controls.Find("pb_rename_scenario", false)[0].Location = new Point(content.Controls.Find("lbl_name_scenario", false)[0].Location.X +
                                                                                       content.Controls.Find("lbl_name_scenario", false)[0].Width, 0);
            content.Controls.Find("pb_discard_scenario", false)[0].Location = new Point(content.Controls.Find("pb_rename_scenario", false)[0].Location.X +
                                                                                       content.Controls.Find("pb_rename_scenario", false)[0].Width + 2, 0);
            content.Controls.Find("pb_rename_scenario", false)[0].Visible = true;
            content.Controls.Find("lbl_name_scenario", false)[0].Visible = true;
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

        /// <summary>
        /// This function has to be launched with the corresponding scenario selected.
        /// It will create an entire situation linked to the scenario and set the values to default settings.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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
            JObject environment_content = new JObject()
            {
                ["background"] = "default",
                ["scene_type"] = "tram_entrance"
            };
            File.WriteAllText(@"" + access_path + parent.Nodes.Count.ToString() + "." + new_situation + Path.DirectorySeparatorChar + "environment.json",
                              environment_content.ToString());

            // Select the newly created TreeNode.
            menu.SelectedNode = parent.LastNode;
        }

        /// <summary>
        /// Add a new situation to an existing scenario, only manages the menu, 
        /// the files have to be created through "Add_Situation(object sender, EventArgs e)".
        /// This function will then trigger this one.
        /// </summary>
        /// <param name="scenario">Name of the scenario in which the situation will be added.</param>
        /// <param name="new_situation">Name of the new situation to create.</param>
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

        /// <summary>
        /// Generates all controls necessary to a situation depending on which situation is selected.
        /// </summary>
        public void Display_Situation()
        {
            // Creation of all controls.

            // Creation of two arrows allowing changement of the situations' order.
            PictureBox pb_down_situation = new PictureBox()
            {
                Name = "pb_down_situation",
                Cursor = Cursors.Hand,
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
                Cursor = Cursors.Hand,
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_up.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_up_situation.Click += new EventHandler(this.Up_Situation);
            content.Controls.Add(pb_up_situation);

            // Creation of a label reminding the situation's name.
            Label lbl_name_situation = new Label()
            {
                Name = "lbl_name_situation",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.Hand,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            lbl_name_situation.Click += new EventHandler(this.Ask_Rename_Situation);
            content.Controls.Add(lbl_name_situation);

            // Creation of the hidden textbox allowing to enter a new situation's name.
            TextBox txt_rename_situation = new TextBox()
            {
                Name = "txt_rename_situation",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.IBeam,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(56, 56, 56),
                ForeColor = Color.White,
                Width = Tools.Get_Text_Width(this, menu.SelectedNode.Text, 16) + 24,
                ShortcutsEnabled = false,
                Visible = false
            };
            txt_rename_situation.KeyPress += new KeyPressEventHandler(this.Rename_Situation_Txt_Keypress);
            content.Controls.Add(txt_rename_situation);

            // Creation of a picturebox allowing to rename the situation.
            PictureBox pb_rename_situation = new PictureBox()
            {
                Name = "pb_rename_situation",
                Cursor = Cursors.Hand,
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "edit.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_rename_situation.Click += new EventHandler(this.Ask_Rename_Situation);
            content.Controls.Add(pb_rename_situation);

            // Creation of a button allowing to discard the situation.
            PictureBox pb_discard_situation = new PictureBox()
            {
                Name = "pb_discard_situation",
                Cursor = Cursors.Hand,
                Size = new Size(24, 24),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "delete.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_discard_situation.Click += new EventHandler(this.Discard_Situation);
            content.Controls.Add(pb_discard_situation);

            // Set the correct location of the controls (responsive with the groupbox's size).
            pb_down_situation.Location = new Point(8, 0);
            pb_up_situation.Location = new Point(pb_down_situation.Location.X + pb_down_situation.Width + 2, 0);
            lbl_name_situation.Location = new Point(pb_up_situation.Location.X + pb_up_situation.Width + 8, 0);
            txt_rename_situation.Location = new Point(lbl_name_situation.Location.X, 0);
            pb_rename_situation.Location = new Point(lbl_name_situation.Location.X + lbl_name_situation.Width, 0);
            pb_discard_situation.Location = new Point(pb_rename_situation.Location.X + pb_rename_situation.Width + 2, 0);
        }

        /// <summary>
        /// Switch the select situation with the one bellow it.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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

        /// <summary>
        /// Switch the selected situation with the one upside it.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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

        /// <summary>
        /// Technical function re-ordering all situations after a switch, both in the menu and in the folder.
        /// </summary>
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

        /// <summary>
        /// Display the renaming textbox and hides the picturebox while editing the name.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Ask_Rename_Situation(object sender, EventArgs e)
        {
            // Set the textbox of the name as visible.
            content.Controls.Find("txt_rename_situation", false)[0].Visible = true;
            content.Controls.Find("pb_rename_situation", false)[0].Visible = false;
            content.Controls.Find("lbl_name_situation", false)[0].Visible = false;
            content.Controls.Find("txt_rename_situation", false)[0].Focus();
        }

        /// <summary>
        /// Handle all keypresses and validate, cancel or even forbid the action.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Rename_Situation_Txt_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                // Block the renaming of a situation if the new name is empty.
                if (t.Text.Equals(string.Empty)) { return; }
                Rename_Situation(t.Text);
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Visible = false;
                content.Controls.Find("pb_rename_situation", false)[0].Visible = true;
                content.Controls.Find("lbl_name_situation", false)[0].Visible = true;
                e.Handled = true;
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == (char)8)) // (char) 8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 32)
            {
                if (e.KeyChar == (char)8) { return; } // Let you erase even with length limit reached.
                e.Handled = true;
            }
        }

        /// <summary>
        /// Set the given name as the one of the selected situation.
        /// </summary>
        /// <param name="new_name">New name to be assigned to the situation.</param>
        private void Rename_Situation(string new_name)
        {
            // Exit if name is similar.
            if (menu.SelectedNode.Text == new_name.Trim()) { return; }

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
            lbl_path.Text = menu.SelectedNode.FullPath;

            // Repositioning size-sensitives contents.
            TextBox t = (TextBox)content.Controls.Find("txt_rename_situation", false)[0];
            t.Visible = false;
            t.Width = (menu.SelectedNode.Text.Length * 10) + 20;
            content.Controls.Find("lbl_name_situation", false)[0].Text = menu.SelectedNode.Text;
            content.Controls.Find("pb_rename_situation", false)[0].Location = new Point(content.Controls.Find("lbl_name_situation", false)[0].Location.X +
                                                                                       content.Controls.Find("lbl_name_situation", false)[0].Width, 0);
            content.Controls.Find("pb_discard_situation", false)[0].Location = new Point(content.Controls.Find("pb_rename_situation", false)[0].Location.X +
                                                                                       content.Controls.Find("pb_rename_situation", false)[0].Width + 2, 0);
            content.Controls.Find("pb_rename_situation", false)[0].Visible = true;
            content.Controls.Find("lbl_name_situation", false)[0].Visible = true;
        }

        /// <summary>
        /// Discard the currently selected situation.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Location.Y);

            // Resize generals controls from the Editor.
            menu.Size = new Size(menu.Width, this.Height - menu.Location.Y - 51);
            content.Size = new Size(this.Width - content.Location.X - 28, this.Height - content.Location.Y - 51);

            // Resize all controls inside "content".
            Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
        }
    }
}
