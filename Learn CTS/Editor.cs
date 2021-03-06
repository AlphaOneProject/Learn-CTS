﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

namespace Learn_CTS
{
    public partial class Editor : Form
    {
        // Constants.

        private const int dialogs_per_page = 5;

        // Attributes.

        private readonly String game;
        private readonly String game_path;
        private JObject game_properties;
        private JObject theme;
        private string old_category = "general";
        private bool saved = true;
        private GameWindow preview = null;
        private PlacementEdition event_placement = null;
        private bool is_loading = false;

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
            if (!this.game_properties["state"].ToString().Equals("Inactif."))
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

            // Set the title and cut it if necessary.
            int char_space = (this.Width - menu.Width - 64) / 24;
            if (char_space < 12) { char_space = 12; } // Avoid a possible substring exception with a ridicularly little window.
            string cut_game = this.game;
            if (cut_game.Length > char_space)
            {
                cut_game = cut_game.Substring(0, char_space - 3) + "...";
            }
            title.Text = "Édition de " + cut_game;
            title.Location = new Point(((this.Width - menu.Width - title.Width) / 2) + menu.Width, title.Location.Y);

            // Set the window size as options size setting.
            string options_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                  Path.DirectorySeparatorChar + "options.json";
            JObject options = Tools.Get_From_JSON(options_path);

            // Load the current color theme.
            this.theme = (JObject)Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal"
                         + Path.DirectorySeparatorChar + "themes.json")[(string)options["theme"]];

            // Apply the current color theme.
            this.BackColor = Color.FromArgb(int.Parse((string)this.theme["0"]["R"]), int.Parse((string)this.theme["0"]["G"]), int.Parse((string)this.theme["0"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            menu.BackColor = Color.FromArgb(int.Parse((string)this.theme["1"]["R"]), int.Parse((string)this.theme["1"]["G"]), int.Parse((string)this.theme["1"]["B"]));
            menu.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            content.BackColor = Color.FromArgb(int.Parse((string)this.theme["1"]["R"]), int.Parse((string)this.theme["1"]["G"]), int.Parse((string)this.theme["1"]["B"]));
            content.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            lbl_path.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            lbl_path.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            if ((bool)options["maximized"])
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.Width = int.Parse((string)options["size"]["x"]);
                this.Height = int.Parse((string)options["size"]["y"]);

                // Place the windows at the center of the screen.
                Rectangle screen = Screen.FromControl(this).Bounds;
                this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2);
            }

            menu.CollapseAll();

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
        /// Allow external Forms or UserControls to access the name of the game edited by
        /// an instance of Editor and thus, the game they are asked to focus on.
        /// </summary>
        /// <returns>Name of the game currently edited.</returns>
        public string Get_Game()
        {
            return this.game;
        }

        /// <summary>
        /// Allow external Forms or UserControls to access the path of the game edited by
        /// an instance of Editor and thus, the game they are asked to focus on.
        /// </summary>
        /// <returns>Path of the game currently edited.</returns>
        public string Get_Game_Path()
        {
            return this.game_path;
        }

        /// <summary>
        /// Transmit the data object already read when starting the Editor and thus
        /// optimizing the I/O memory access.
        /// </summary>
        /// <returns>Object containing the whole pallet of colors to display.</returns>
        public JObject Get_Theme()
        {
            return this.theme;
        }

        /// <summary>
        /// Allow external Forms or UserControls to access the loading state of the game edited by
        /// an instance of Editor and thus, the game they are asked to focus on.
        /// </summary>
        /// <returns>Current loading state of the editor, false if no loading and true if loading.</returns>
        public bool Get_Is_Loading()
        {
            return this.is_loading;
        }

        /// <summary>
        /// Accessor to the local parameter "is_loading",
        /// used to transmit a non-interruption demand.
        /// </summary>
        /// <param name="new_is_loading">New loading state to assign to the editor.</param>
        public void Set_Is_Loading(bool new_is_loading)
        {
            this.is_loading = new_is_loading;

            if (is_loading)
            {
                content.Enabled = false;
            }
            else
            {
                content.Enabled = true;
            }
        }

        /// <summary>
        /// Accessor to the local parameter "saved".
        /// </summary>
        /// <param name="saved">New desired value for "saved".</param>
        public void Set_Saved(bool saved)
        {
            this.saved = saved;
        }

        /// <summary>
        /// Triggers while the Editor began a closing operation, checks first if there is unsaved work
        /// and if there is ask the user to confirm his decision.
        /// Upon confirmation, or without modification, the window parameters will be saved into "/internal/options.json".
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous tout de même quitter ?",
                                "Confirmation de fermeture", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
                return;
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

            // Save current Editor's size.
            string options_path = System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                                  Path.DirectorySeparatorChar + "options.json";
            JObject options = Tools.Get_From_JSON(options_path);
            options["maximized"] = (this.WindowState == FormWindowState.Maximized);
            JObject size = new JObject()
            {
                ["x"] = this.Width,
                ["y"] = this.Height
            };
            options["size"] = size;
            Tools.Set_To_JSON(options_path, options);

            Application.Restart();
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
            TreeView t = (TreeView)sender;
            if (t.SelectedNode == null) { return; }

            Tools.Begin_Control_Update(content);

            string name = t.SelectedNode.Name;
            lbl_path.Text = t.SelectedNode.FullPath;
            List<String> keeping_categories = new List<String>()
            { "npcs", "dialogs", "item_images", "sprites", "backgrounds" };

            if (!(this.old_category.Equals(name) && (keeping_categories.Contains(name) || name.StartsWith("situation") || name.StartsWith("scenario"))))
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
                    case "npcs":
                        Display_NPCs(); break;
                    case "dialogs":
                        Display_Dialogs(); break;
                    case "images":
                        Display_Images(); break;
                    case "item_images":
                        Display_Item_Images(); break;
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
                Text = "Description du jeu",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_desc);

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Général",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            // Creation of textbox containing the current game description.
            TextBox txt_desc = new TextBox()
            {
                Name = "txt_desc",
                Tag = this.game_path + Path.DirectorySeparatorChar + "properties.json",
                Text = (string)this.game_properties["description"],
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ShortcutsEnabled = false
            };
            txt_desc.Width = content.Width - 40;
            txt_desc.KeyPress += new KeyPressEventHandler(this.Txt_Keypress);
            content.Controls.Add(txt_desc);
            tlt_global.SetToolTip(txt_desc, "Description du jeu");

            // Creation of a label used for showing messages regarding the description's modification.
            Label lbl_desc_state = new Label()
            {
                Name = "lbl_desc_state",
                Text = "",
                ForeColor = Color.Red,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_desc_state);

            Label lbl_map_title = new Label()
            {
                Name = "lbl_map_title",
                Text = "Carte du réseau",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_map_title);

            ImageEdition ie_map = new ImageEdition(this, this.game_path + Path.DirectorySeparatorChar + "library" +
                                                   Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                                                   "others" + Path.DirectorySeparatorChar + "map.png", "map")
            {
                Name = "ie_map"
            };
            content.Controls.Add(ie_map);
            tlt_global.SetToolTip(ie_map, "Carte servant au joueur à se repérer dans le réseau de stations que vous souhaitez établir");

            // Set the correct location of the controls (responsive with the groupbox's size).
            lbl_desc.Location = new Point(20, 20);
            pb_help.Location = new Point(lbl_desc.Location.X + lbl_desc.Width + 20, 20);
            txt_desc.Location = new Point(20, lbl_desc.Location.Y + lbl_desc.Height + 20);
            lbl_desc_state.Location = new Point(20, txt_desc.Location.Y + txt_desc.Height + 8);
            lbl_map_title.Location = new Point(20, lbl_desc_state.Location.Y + lbl_desc_state.Height + 40);
            ie_map.Location = new Point(20, lbl_map_title.Location.Y + lbl_map_title.Height + 20);
        }

        /// <summary>
        /// Handle all keypresses and validate, cancel or even forbid the action.
        /// Upon validation will save the new data piece in the file Tagged by the TextBox.
        /// </summary>
        /// <param name="sender">Control calling the method, carrying the path of the
        /// relevant file in Tag.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Txt_Keypress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string data_path = (string)t.Tag;
            string data_piece = "";
            JObject data = Tools.Get_From_JSON(data_path);

            switch(t.Name)
            {
                case "txt_desc":
                    data_piece = "description";
                    break;
                case "txt_scene_name":
                    data_piece = "scene_name";
                    break;
                case "txt_scene_intro":
                    data_piece = "scene_intro";
                    break;
                default:
                    throw new InvalidDataException("Data wasn't found for Txt_Keypress function in the Editor.cs file!");
            }

            List<char> autorized_chars = new List<char>() { ' ', '.', ',', '\'', '?', '!', '-', '°', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                data[data_piece] = t.Text;
                Tools.Set_To_JSON(data_path, data); // Set the entered value as new file's value.
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                this.saved = true;
                if (t.Name == "txt_desc")
                {
                    content.Controls.Find("lbl_desc_state", false)[0].Text = "";
                }
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)data[data_piece];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                this.saved = true;
                if (t.Name == "txt_desc")
                {
                    content.Controls.Find("lbl_desc_state", false)[0].Text = "";
                }
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || autorized_chars.Contains(e.KeyChar) || e.KeyChar == (char)8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 512) // Avoid endless descriptions.
            {
                if (e.KeyChar == (char)8) // Still backspace.
                {
                    t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"])*0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                    if (t.Name == "txt_desc")
                    {
                        content.Controls.Find("lbl_desc_state", false)[0].Text = "Sauvegardez en appuyant sur 'Entrée' ou annulez avec 'Echap'";
                    }
                    e.Handled = true;
                    this.saved = false;
                    return; // Let you erase regardless of the length.
                }
                if (t.Name == "txt_desc")
                {
                    content.Controls.Find("lbl_desc_state", false)[0].Text = "Limite de caractères atteinte !";
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                if (t.Name == "txt_desc")
                {
                    content.Controls.Find("lbl_desc_state", false)[0].Text = "Sauvegardez en appuyant sur 'Entrée' ou annulez avec 'Echap'";
                }
                this.saved = false;
            }
        }

        /// <summary>
        /// Load controls for models' content.
        /// </summary>
        private void Display_Models()
        {
            // Creation of the PictureBox responsible for copying game's library from another game.
            PictureBox pb_import_lib = new PictureBox()
            {
                Name = "pb_import_lib",
                Cursor = Cursors.Hand,
                Size = new Size(64, 64),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "gamecard-copy-btn-x64.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_import_lib.Click += new EventHandler(this.Import_Library);
            content.Controls.Add(pb_import_lib);
            tlt_global.SetToolTip(pb_import_lib, "Permet d'importer les modèles d'un jeu quelconque dans celui-ci");

            Label lbl_import_lib = new Label()
            {
                Name = "lbl_import_lib",
                Text = "Importer les modèles d'un autre jeu",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            lbl_import_lib.Click += new EventHandler(this.Import_Library);
            content.Controls.Add(lbl_import_lib);
            tlt_global.SetToolTip(lbl_import_lib, "Permet d'importer les modèles d'un jeu quelconque dans celui-ci");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Modèles",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            pb_import_lib.Location = new Point((content.Width - pb_import_lib.Width - 8 - lbl_import_lib.Width) / 2, 20);
            lbl_import_lib.Location = new Point(pb_import_lib.Location.X + pb_import_lib.Width + 8, pb_import_lib.Location.Y +
                                                (pb_import_lib.Height - lbl_import_lib.Height) / 2);
            pb_help.Location = new Point(lbl_import_lib.Location.X + lbl_import_lib.Width + 20, 20);
        }

        /// <summary>
        /// Launch the Form "GameSelection" with full focus to get the selected game
        /// upon its end.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Import_Library(object sender, EventArgs e)
        {
            GameSelection gs = new GameSelection(this);
            gs.ShowDialog();
        }

        /// <summary>
        /// Recover and process the name of the selected game.
        /// If the name is not empty then the library of the game will
        /// be copied in the current game.
        /// </summary>
        /// <param name="game_name">Legacy parameter from the "GameSelection" Form,
        /// name of the selected game.</param>
        public void Import_Selected_Library(string game_name)
        {
            if (game_name.Length == 0) { return; }

            DirectoryInfo source_library = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar +
                                         "games" + Path.DirectorySeparatorChar + game_name + Path.DirectorySeparatorChar +
                                         "library" + Path.DirectorySeparatorChar);
            DirectoryInfo dest_library = new DirectoryInfo(this.game_path + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar);

            int npcs_nbr = new DirectoryInfo(dest_library.FullName + "npcs").GetFiles().Length;
            int i = 1;
            foreach (FileInfo fi in new DirectoryInfo(source_library + "npcs").GetFiles())
            {
                fi.CopyTo(dest_library.FullName + "npcs" + Path.DirectorySeparatorChar + (npcs_nbr + i).ToString() +
                          ".json");
                i++;
            }

            int dialogs_nbr = new DirectoryInfo(dest_library.FullName + "dialogs").GetFiles().Length;
            i = 1;
            foreach (FileInfo fi in new DirectoryInfo(source_library + "dialogs").GetFiles())
            {
                fi.CopyTo(dest_library.FullName + "dialogs" + Path.DirectorySeparatorChar + (dialogs_nbr + i).ToString() +
                          ".json");
                i++;
            }

            Tools.DirectoryCopy(new DirectoryInfo(source_library.FullName + "images"), new DirectoryInfo(dest_library.FullName + "images"));

            MessageBox.Show("La copie des modèles du jeu <" + game_name + "> a bien été effectuée !", "Copie réussie",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Load controls for NPCs' content.
        /// </summary>
        private void Display_NPCs()
        {
            if (this.old_category.Equals("npcs")) // Trigger only on form resize.
            {
                FlowLayoutPanel flp = (FlowLayoutPanel)content.Controls.Find("flp_npcs", false)[0];

                flp.Width = content.Width - 40;
                flp.Height = content.Height - 120;
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
            tlt_global.SetToolTip(pb_add_lib_npc, "Ajoute un nouveau figurant");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Figurants",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            // FlowLayoutPanel, keeping the generated UserControls in well displayed.
            FlowLayoutPanel flp_npcs = new FlowLayoutPanel()
            {
                Name = "flp_npcs",
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Width = content.Width - 40,
                Height = content.Height - 120
            };
            content.Controls.Add(flp_npcs);

            // Set the correct location of the controls (responsive with the groupbox's size).
            lbl_npcs.Location = new Point(20, 20);
            pb_add_lib_npc.Location = new Point(lbl_npcs.Location.X + lbl_npcs.Width + 20, 20);
            pb_help.Location = new Point(pb_add_lib_npc.Location.X + pb_add_lib_npc.Width + 20, 20);
            flp_npcs.Location = new Point(20, 100);

            // Generates all CharacterSelection UserControls.
            int i = 0;
            JObject data_pnj;
            string npcs_folder_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                                        Path.DirectorySeparatorChar + "npcs";
            foreach (string file_npc in Directory.GetFiles(npcs_folder_path))
            {
                data_pnj = Tools.Get_From_JSON(npcs_folder_path + Path.DirectorySeparatorChar + (i + 1) + ".json");

                CharacterSelection cs = new CharacterSelection(this, file_npc);
                flp_npcs.Controls.Add(cs);

                i++;
            }
        }

        /// <summary>
        /// Add a new NPC to the library, both in the Editor and in the dedicated folder.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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
            CharacterSelection cs = new CharacterSelection(this, @"" + npcs_folder_path + Path.DirectorySeparatorChar + new_id + ".json");
            content.Controls.Find("flp_npcs", true)[0].Controls.Add(cs);
        }

        /// <summary>
        /// Load controls for Choices' content.
        /// </summary>
        private void Display_Dialogs()
        {
            if (this.old_category == "dialogs") // Trigger only on form resize.
            {
                Button btn_ff = (Button)content.Controls.Find("btn_fast_forward", true)[0];
                Button btn_f = (Button)content.Controls.Find("btn_forward", true)[0];
                Label lbl_pn = (Label)content.Controls.Find("lbl_page_number", true)[0];
                Button btn_b = (Button)content.Controls.Find("btn_backward", true)[0];
                Button btn_fb = (Button)content.Controls.Find("btn_fast_backward", true)[0];

                btn_ff.Location = new Point(content.Width - btn_ff.Width - 40, 20);
                btn_f.Location = new Point(btn_ff.Location.X - btn_f.Width - 4, 20);
                lbl_pn.Location = new Point(btn_f.Location.X - lbl_pn.Width - 10,
                                                     20 + ((btn_ff.Height - lbl_pn.Height) / 2));
                btn_b.Location = new Point(lbl_pn.Location.X - btn_b.Width - 10, 20);
                btn_fb.Location = new Point(btn_b.Location.X - btn_fb.Width - 4, 20);

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
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
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
            tlt_global.SetToolTip(pb_add_dialog, "Ajouter un nouveau dialogue");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Dialogues",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            // Place the controls just created.
            lbl_dialogs.Location = new Point(20, 20);
            pb_add_dialog.Location = new Point(lbl_dialogs.Location.X + lbl_dialogs.Width + 20, 20);
            pb_help.Location = new Point(pb_add_dialog.Location.X + pb_add_dialog.Width + 20, 20);

            // Generating Controls associated to the pages.
            // Choosen pattern: << < P > >>
            Button btn_fast_backward = new Button()
            {
                Name = "btn_fast_backward",
                Text = "<<",
                AutoSize = true
            };
            btn_fast_backward.Click += new EventHandler(Dialog_Fast_Backward);
            content.Controls.Add(btn_fast_backward);
            tlt_global.SetToolTip(btn_fast_backward, "Retourner à la première page");

            Button btn_backward = new Button()
            {
                Name = "btn_backward",
                Text = "<",
                AutoSize = true
            };
            btn_backward.Click += new EventHandler(Dialog_Backward);
            content.Controls.Add(btn_backward);
            tlt_global.SetToolTip(btn_backward, "Aller à la page précédente");

            Label lbl_page_number = new Label()
            {
                Name = "lbl_page_number",
                Text = "1/1",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            lbl_page_number.TextChanged += new EventHandler(Dialog_Page_Update);
            content.Controls.Add(lbl_page_number);
            tlt_global.SetToolTip(lbl_page_number, "Page actuelle");

            Button btn_forward = new Button()
            {
                Name = "btn_forward",
                Text = ">",
                AutoSize = true
            };
            btn_forward.Click += new EventHandler(Dialog_Forward);
            content.Controls.Add(btn_forward);
            tlt_global.SetToolTip(btn_forward, "Aller à la page suivante");

            Button btn_fast_forward = new Button()
            {
                Name = "btn_fast_forward",
                Text = ">>",
                AutoSize = true
            };
            btn_fast_forward.Click += new EventHandler(Dialog_Fast_Forward);
            content.Controls.Add(btn_fast_forward);
            tlt_global.SetToolTip(btn_fast_forward, "Aller à la dernière page");

            // Placement of said Controls.
            btn_fast_forward.Location = new Point(content.Width - btn_fast_forward.Width - 40, 20);
            btn_forward.Location = new Point(btn_fast_forward.Location.X - btn_forward.Width - 4, 20);
            lbl_page_number.Location = new Point(btn_forward.Location.X - lbl_page_number.Width - 10,
                                                 20 + ((btn_fast_forward.Height - lbl_page_number.Height) / 2));
            btn_backward.Location = new Point(lbl_page_number.Location.X - btn_backward.Width - 10, 20);
            btn_fast_backward.Location = new Point(btn_backward.Location.X - btn_fast_backward.Width - 4, 20);

            // Generating to all files' a QuizzEdition UserControl.
            int i = 1;
            int last_pos = 0;
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            int total_pages = Tools.Round_Up((double)Directory.GetFiles(dialogs_path).Length / dialogs_per_page);
            lbl_page_number.Text = "1/" + total_pages.ToString();

            foreach (string file in Directory.GetFiles(dialogs_path))
            {
                // Creating the UserControl responsible for the internal edition of the JSON file.
                QuizzEdition QEdition = new QuizzEdition(this, @"" + dialogs_path + i + ".json")
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
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    AutoSize = true,
                    BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"])),
                    ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                    BorderStyle = BorderStyle.FixedSingle
                };
                content.Controls.Add(lbl_dialog_id);

                lbl_dialog_id.Location = new Point(QEdition.Location.X, QEdition.Location.Y - lbl_dialog_id.Height + 1);
                i++;

                if (i > dialogs_per_page)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Refresh the display of QuizzEdition inside the content depending on the new dialogs' page.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Dialog_Page_Update(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            int current_page = int.Parse(lbl.Text.Split('/')[0]);
            int max_page = Tools.Round_Up((double)Directory.GetFiles(dialogs_path).Length / dialogs_per_page);

            List<int> ids = new List<int>();
            foreach(QuizzEdition qe in content.Controls.OfType<QuizzEdition>())
            {
                ids.Add(qe.Get_Id());
                content.Controls.Remove(qe);
            }
            foreach (int id in ids)
            {
                    content.Controls.Remove(content.Controls.Find("lbl_dialog_id" + id, true)[0]);
            }

            Control fast_backward = content.Controls.Find("btn_fast_backward", true)[0];
            Control backward = content.Controls.Find("btn_backward", true)[0];
            Control forward = content.Controls.Find("btn_forward", true)[0];
            Control fast_forward = content.Controls.Find("btn_fast_forward", true)[0];

            fast_backward.Enabled = true;
            backward.Enabled = true;
            forward.Enabled = true;
            fast_forward.Enabled = true;

            if (current_page == 1)
            {
                fast_backward.Enabled = false;
                backward.Enabled = false;
            }
            if (current_page == max_page)
            {
                forward.Enabled = false;
                fast_forward.Enabled = false;
            }

            int i = 1;
            int last_pos = 0;

            foreach (string file in Directory.GetFiles(dialogs_path))
            {
                if (i < ((current_page - 1) * dialogs_per_page) + 1)
                {
                    i++;
                    continue;
                }

                // Creating the UserControl responsible for the internal edition of the JSON file.
                QuizzEdition QEdition = new QuizzEdition(this, @"" + dialogs_path + i + ".json")
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
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    AutoSize = true,
                    BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"])),
                    ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                    BorderStyle = BorderStyle.FixedSingle
                };
                content.Controls.Add(lbl_dialog_id);

                lbl_dialog_id.Location = new Point(QEdition.Location.X, QEdition.Location.Y - lbl_dialog_id.Height + 1);
                i++;

                if (i > (dialogs_per_page * current_page))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Go to the first page of the dialogs.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Dialog_Fast_Backward(object sender, EventArgs e)
        {
            if (!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous les abandonner ?",
                                "Confirmation d'abandon de modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                this.saved = true;
            }

            Label lbl = (Label)content.Controls.Find("lbl_page_number", true)[0];
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            lbl.Text = "1/" + Tools.Round_Up((double)Directory.GetFiles(dialogs_path).Length / dialogs_per_page).ToString();
        }

        /// <summary>
        /// Go to the precedent page of the dialogs.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Dialog_Backward(object sender, EventArgs e)
        {
            if (!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous les abandonner ?",
                                "Confirmation d'abandon de modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                this.saved = true;
            }

            Label lbl = (Label)content.Controls.Find("lbl_page_number", true)[0];
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            lbl.Text = (int.Parse(lbl.Text.Split('/')[0]) - 1).ToString() + "/" +
                       Tools.Round_Up((double)Directory.GetFiles(dialogs_path).Length / dialogs_per_page).ToString();
        }

        /// <summary>
        /// Go to the next page of the dialogs.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Dialog_Forward(object sender, EventArgs e)
        {
            if (!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous les abandonner ?",
                                "Confirmation d'abandon de modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                this.saved = true;
            }

            Label lbl = (Label)content.Controls.Find("lbl_page_number", true)[0];
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            lbl.Text = (int.Parse(lbl.Text.Split('/')[0]) + 1).ToString() + "/" +
                       Tools.Round_Up((double)Directory.GetFiles(dialogs_path).Length / dialogs_per_page).ToString();
        }

        /// <summary>
        /// Go to the last page of the dialogs.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Dialog_Fast_Forward(object sender, EventArgs e)
        {
            if (!this.saved && MessageBox.Show("Vous avez des modifications non enregistrées.\nSouhaitez-vous les abandonner ?",
                                "Confirmation d'abandon de modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                this.saved = true;
            }

            Label lbl = (Label)content.Controls.Find("lbl_page_number", true)[0];
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
            int max_page = Tools.Round_Up((double)Directory.GetFiles(dialogs_path).Length / dialogs_per_page);
            lbl.Text = max_page.ToString() + "/" + max_page.ToString();
        }

        /// <summary>
        /// Creates a new dialog file and it's Controls.
        /// Then call Update_Dialogs in order to place them.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Add_Dialog(object sender, EventArgs e)
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
                },
                ["audio"] = 1
            };
            File.WriteAllText(@"" + dialogs_path + Path.DirectorySeparatorChar + new_id + ".json",
                              dialog_content.ToString());

            // Creating the UserControl responsible for the internal edition of the JSON file.
            QuizzEdition QEdition = new QuizzEdition(this, @"" + dialogs_path + new_id + ".json")
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
                BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
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
            int last_pos = 0;
            foreach (QuizzEdition qe in content.Controls.OfType<QuizzEdition>())
            {
                qe.Location = new Point(40, 100 + last_pos - content.VerticalScroll.Value);
                content.Controls.Find("lbl_dialog_id" +
                                      qe.Get_Id(), false)[0].Location = new Point(40, 100 + last_pos - content.VerticalScroll.Value -
                                                             content.Controls.Find("lbl_dialog_id" + qe.Get_Id(), false)[0].Height + 1);
                last_pos += qe.Height + 40;
                qe.Reload_Redirections();
            }

            // Reload page count and go to the last page.
            Dialog_Fast_Forward(this, new EventArgs());
        }

        /// <summary>
        /// Delete a specified instance of QuizzEdition and his reference's file.
        /// </summary>
        /// <param name="sender">Instance of QuizzEdition calling the function, permits to identify which file delete.</param>
        public void Discard_Dialog(QuizzEdition sender)
        {
            string dialogs_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                                  Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;

            // Disables suppression of all dialogs.
            if (Directory.GetFiles(@"" + dialogs_path).Length <= 1)
            {
                MessageBox.Show("Vous ne pouvez pas supprimer l'intégralité des dialogues.", "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Checks if it is used in another dialog.
            JObject data;
            foreach (string dialog in Directory.GetFiles(dialogs_path))
            {
                data = Tools.Get_From_JSON(dialog);
                for (int i = 1; i <= int.Parse(data["choices"].ToString()); i++)
                {
                    if (int.Parse(data["c" + i.ToString()]["redirect"].ToString()) == sender.Get_Id())
                    {
                        MessageBox.Show("Ce dialogue est utilisé par un ou plusieurs autres dialogues.\n" +
                                        "Remplacez-le pour ces dialogues puis réessayez.\n\n" +
                                        "Dialogue en faisant usage : " + data["question"].ToString(),
                                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Checks if it is used in a situation.
            string scenarios_path = @"" + this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            foreach (string scenario in Directory.GetDirectories(scenarios_path))
            {
                foreach (string situation in Directory.GetDirectories(scenario))
                {
                    data = Tools.Get_From_JSON(situation + Path.DirectorySeparatorChar + "dialogs.json");
                    for (int i = 1; i <= int.Parse(data["events"].ToString()); i++)
                    {
                        if (int.Parse(data[i.ToString()]["quizz"].ToString()) == sender.Get_Id())
                        {
                            MessageBox.Show("Ce dialogue est utilisé dans une ou plusieurs situations.\n" +
                                            "Remplacez-le dans ces situations puis réessayez.\n\n" +
                                            "Situation en faisant usage : " +
                                            situation.Split(Path.DirectorySeparatorChar).Last().Split('.').Last(),
                                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Asks for confirmation before suppression of the dialog.
            if ((MessageBox.Show("Confirmer la suppression du dialogue N°" + sender.Get_Id() + " ?", "Confirmation de suppression",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No))
            {
                return;
            }

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

        /// <summary>
        /// Load controls for images' content.
        /// </summary>
        private void Display_Images()
        {
            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Images",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            pb_help.Location = new Point(20, 20);
        }

        /// <summary>
        /// Load controls for item images' content.
        /// </summary>
        private void Display_Item_Images()
        {
            if (this.old_category == "item_images") // Activates upon resize from the Editor.
            {
                content.Controls.Find("flp_item_images", true)[0].Width = content.Width - 40;
                content.Controls.Find("flp_item_images", true)[0].Height = content.Height - 120;
                return;
            }

            // Controls 
            Label lbl_item_images = new Label()
            {
                Name = "lbl_item_images",
                Text = "Images des objets",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_item_images);

            PictureBox pb_add_item_image = new PictureBox()
            {
                Name = "pb_add_item_image",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_item_image.Click += new EventHandler(Add_Item_Image);
            content.Controls.Add(pb_add_item_image);
            tlt_global.SetToolTip(pb_add_item_image, "Ajoute un nouveau modèle de décor depuis un fichier");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Objets",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            FlowLayoutPanel flp_item_images = new FlowLayoutPanel()
            {
                Name = "flp_item_images",
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Width = content.Width - 40,
                Height = content.Height - 120
            };
            content.Controls.Add(flp_item_images);

            // Placement of those Controls.
            lbl_item_images.Location = new Point(20, 40);
            pb_add_item_image.Location = new Point(lbl_item_images.Location.X + lbl_item_images.Width + 20, 40);
            pb_help.Location = new Point(pb_add_item_image.Location.X + pb_add_item_image.Width + 20, 40);
            flp_item_images.Location = new Point(20, 100);

            // Add all existing backgrounds to the FlowLayoutPanel.
            string items_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                                      Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                                      "items" + Path.DirectorySeparatorChar;
            foreach (string file_name in Directory.GetFiles(items_path))
            {
                ImageEdition ie = new ImageEdition(this, file_name, "item");
                flp_item_images.Controls.Add(ie);
            }
        }

        /// <summary>
        /// Asks a new Image from the user then copy it and add it
        /// to the display.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Add_Item_Image(object sender, EventArgs e)
        {
            if (ofd_global.ShowDialog() == DialogResult.Cancel) { return; }

            string extension = ofd_global.FileName.Split(Path.DirectorySeparatorChar).Last().Split('.').Last();
            if (extension != "png")
            {
                MessageBox.Show("L'image doit être de format .png pour être utilisée.", "Type invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (@"" + ofd_global.FileName == @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "items" +
                Path.DirectorySeparatorChar + ofd_global.FileName.Split(Path.DirectorySeparatorChar).Last())
            {
                MessageBox.Show("Vous ne pouvez importer un modèle déjà présent.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string new_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                          Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "items" +
                          Path.DirectorySeparatorChar + ofd_global.FileName.Split(Path.DirectorySeparatorChar).Last();
            try
            {
                File.Copy(@"" + ofd_global.FileName, @"" + new_path, false);
            }
            catch (IOException except)
            {
                if (MessageBox.Show("Un fichier du même nom existe déjà dans votre liste de modèles.\n" +
                                    "Souhaitez-vous le remplacer ?", "Remplacement de fichier",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Copy(@"" + ofd_global.FileName, @"" + new_path, true);
                }
                else
                {
                    return;
                }
            }

            ImageEdition ie = new ImageEdition(this, @"" + new_path, "item");
            content.Controls.Find("flp_item_images", true)[0].Controls.Add(ie);
        }

        /// <summary>
        /// Load controls for sprites' content.
        /// </summary>
        private void Display_Sprites()
        {
            if (this.old_category == "sprites") // Activates upon resize from the Editor.
            {
                content.Controls.Find("flp_sprites", true)[0].Width = content.Width - 40;
                content.Controls.Find("flp_sprites", true)[0].Height = content.Height - 120;
                return;
            }

            // Controls 
            Label lbl_sprites = new Label()
            {
                Name = "lbl_sprites",
                Text = "Personnages",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_sprites);

            PictureBox pb_add_sprite = new PictureBox()
            {
                Name = "pb_add_sprite",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_sprite.Click += new EventHandler(Add_Sprite);
            content.Controls.Add(pb_add_sprite);
            tlt_global.SetToolTip(pb_add_sprite, "Ajoute un nouveau modèle de personnage et de ses animations");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Personnages",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            FlowLayoutPanel flp_sprites = new FlowLayoutPanel()
            {
                Name = "flp_sprites",
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Width = content.Width - 40,
                Height = content.Height - 120
            };
            content.Controls.Add(flp_sprites);

            // Placement of those Controls.
            lbl_sprites.Location = new Point(20, 40);
            pb_add_sprite.Location = new Point(lbl_sprites.Location.X + lbl_sprites.Width + 20, 40);
            pb_help.Location = new Point(pb_add_sprite.Location.X + pb_add_sprite.Width + 20, 40);
            flp_sprites.Location = new Point(20, 100);

            // Add all existing sprites to the display.
            string characters_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                                      Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                                      "characters" + Path.DirectorySeparatorChar;
            foreach (string folder_name in Directory.GetDirectories(characters_path))
            {
                AnimationEdition ae = new AnimationEdition(this, folder_name);
                flp_sprites.Controls.Add(ae);
            }
        }

        /// <summary>
        /// Creates and display a new sprite.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Add_Sprite(object sender, EventArgs e)
        {
            string characters_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                          Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                          "characters" + Path.DirectorySeparatorChar;
            string new_folder = characters_path + (Directory.GetDirectories(characters_path).Count() + 1).ToString();
            Directory.CreateDirectory(new_folder);

            AnimationEdition ae = new AnimationEdition(this, new_folder);
            content.Controls.Find("flp_sprites", true)[0].Controls.Add(ae);
        }

        /// <summary>
        /// Load controls for backgrounds' content.
        /// </summary>
        private void Display_Backgrounds()
        {
            if (this.old_category == "backgrounds") // Activates upon resize from the Editor.
            {
                content.Controls.Find("flp_backgrounds", true)[0].Width = content.Width - 40;
                content.Controls.Find("flp_backgrounds", true)[0].Height = content.Height - 120;
                return;
            }

            // Controls 
            Label lbl_backgrounds = new Label()
            {
                Name = "lbl_backgrounds",
                Text = "Décors",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_backgrounds);

            PictureBox pb_add_background = new PictureBox()
            {
                Name = "pb_add_background",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_background.Click += new EventHandler(Add_Background);
            content.Controls.Add(pb_add_background);
            tlt_global.SetToolTip(pb_add_background, "Ajoute un nouveau modèle de décor depuis un fichier");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Décors",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            FlowLayoutPanel flp_backgrounds = new FlowLayoutPanel()
            {
                Name = "flp_backgrounds",
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Width = content.Width - 40,
                Height = content.Height - 120
            };
            content.Controls.Add(flp_backgrounds);

            // Placement of those Controls.
            lbl_backgrounds.Location = new Point(20, 40);
            pb_add_background.Location = new Point(lbl_backgrounds.Location.X + lbl_backgrounds.Width + 20, 40);
            pb_help.Location = new Point(pb_add_background.Location.X + pb_add_background.Width + 20, 40);
            flp_backgrounds.Location = new Point(20, 100);

            // Add all existing backgrounds to the FlowLayoutPanel.
            string backgrounds_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                                      Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                                      "background" + Path.DirectorySeparatorChar;
            foreach(string file_name in Directory.GetFiles(backgrounds_path))
            {
                if (file_name.Contains("_hitbox")) { continue; }
                ImageEdition ie = new ImageEdition(this, file_name, "background");
                flp_backgrounds.Controls.Add(ie);
            }
        }

        /// <summary>
        /// Creates and display a new background from a selected image file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Add_Background(object sender, EventArgs e)
        {
            if (ofd_global.ShowDialog() == DialogResult.Cancel) { return; }

            string extension = ofd_global.FileName.Split(Path.DirectorySeparatorChar).Last().Split('.').Last();
            if (extension != "png") {
                MessageBox.Show("L'image doit être de format .png pour être utilisée.", "Type invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (@"" + ofd_global.FileName == @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "background" +
                Path.DirectorySeparatorChar + ofd_global.FileName.Split(Path.DirectorySeparatorChar).Last())
            {
                MessageBox.Show("Vous ne pouvez importer un modèle déjà présent.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string new_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                          Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "background" +
                          Path.DirectorySeparatorChar + ofd_global.FileName.Split(Path.DirectorySeparatorChar).Last();
            try
            {
                File.Copy(@"" + ofd_global.FileName, @"" + new_path, false);
            }
            catch (IOException except)
            {
                if (MessageBox.Show("Un fichier du même nom existe déjà dans votre liste de modèles.\n" +
                                    "Souhaitez-vous le remplacer ?", "Remplacement de fichier",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Copy(@"" + ofd_global.FileName, @"" + new_path, true);
                }
                else
                {
                    return;
                }
            }

            ImageEdition ie = new ImageEdition(this, @"" + new_path, "background");
            content.Controls.Find("flp_backgrounds", true)[0].Controls.Add(ie);
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
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Cursor = Cursors.Hand,
                AutoSize = true
            };
            btn_add_scenario.Click += new System.EventHandler(this.Add_Scenario);
            content.Controls.Add(btn_add_scenario);

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Scénarios",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            btn_add_scenario.Location = new Point((content.Size.Width - btn_add_scenario.Size.Width) / 2, 100);
            pb_help.Location = new Point(20, 20);
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
            int i = 1;
            while (i < parent.Nodes.Count + 1)
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
                ["increments"] = 0
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
            if (this.old_category == menu.SelectedNode.Name)
            {
                FlowLayoutPanel flp = (FlowLayoutPanel)content.Controls.Find("flp_increments", true)[0];
                Button btn = (Button)content.Controls.Find("btn_add_situation", true)[0];

                btn.Location = new Point((content.Width - btn.Width) / 2, 100);
                flp.Width = content.Width - 40;
                flp.Height = content.Height - 260;

                foreach(IncrementEdition ie in flp.Controls.OfType<IncrementEdition>())
                {
                    ie.Width = flp.Width - 10;
                }
                return;
            }

            // Creation of all controls.

            // Creation of two arrows allowing changement of the scenarios' order.
            PictureBox pb_down_scenario = new PictureBox()
            {
                Name = "pb_down_scenario",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" + 
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_down.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_down_scenario.Click += new EventHandler(this.Down_Scenario);
            content.Controls.Add(pb_down_scenario);
            tlt_global.SetToolTip(pb_down_scenario, "Inverse la position de ce scénario avec le suivant");

            PictureBox pb_up_scenario = new PictureBox()
            {
                Name = "pb_up_scenario",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_up.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_up_scenario.Click += new EventHandler(this.Up_Scenario);
            content.Controls.Add(pb_up_scenario);
            tlt_global.SetToolTip(pb_up_scenario, "Inverse la position de ce scénario avec le précédent");

            // Creation of a label reminding the scenario's name.
            Label lbl_name_scenario = new Label()
            {
                Name = "lbl_name_scenario",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.Hand,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            lbl_name_scenario.Click += new EventHandler(this.Ask_Rename_Scenario);
            content.Controls.Add(lbl_name_scenario);
            tlt_global.SetToolTip(lbl_name_scenario, "Renomme le scénario");
            
            // Creation of the hidden textbox allowing to enter a new scenario's name.
            TextBox txt_rename_scenario = new TextBox()
            {
                Name = "txt_rename_scenario",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.IBeam,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                Width = Tools.Get_Text_Width(this, menu.SelectedNode.Text, 20) + 24,
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
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "edit.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_rename_scenario.Click += new EventHandler(this.Ask_Rename_Scenario);
            content.Controls.Add(pb_rename_scenario);
            tlt_global.SetToolTip(pb_rename_scenario, "Renomme le scénario");

            // Creation of a button allowing to discard the scenario.
            PictureBox pb_discard_scenario = new PictureBox()
            {
                Name = "pb_discard_scenario",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "delete.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_discard_scenario.Click += new System.EventHandler(this.Discard_Scenario);
            content.Controls.Add(pb_discard_scenario);
            tlt_global.SetToolTip(pb_discard_scenario, "Supprime le scénario et ses situations");

            // Set the correct location of the controls (responsive with the groupbox's size).
            pb_down_scenario.Location = new Point(8, 0);
            pb_up_scenario.Location = new Point(pb_down_scenario.Location.X + pb_down_scenario.Width + 2, 0);
            lbl_name_scenario.Location = new Point(pb_up_scenario.Location.X + pb_up_scenario.Width + 8, 0);
            txt_rename_scenario.Location = new Point(lbl_name_scenario.Location.X, 0);
            pb_rename_scenario.Location = new Point(lbl_name_scenario.Location.X + lbl_name_scenario.Width, 0);
            pb_discard_scenario.Location = new Point(pb_rename_scenario.Location.X + pb_rename_scenario.Width + 2, 0);

            // Creation of the button responsible for the situations' creation.
            Button btn_add_situation = new Button()
            {
                Name = "btn_add_situation",
                Text = "Ajouter une situation",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Cursor = Cursors.Hand,
                AutoSize = true
            };
            btn_add_situation.Click += new EventHandler(this.Add_Situation);
            content.Controls.Add(btn_add_situation);

            Label lbl_increments = new Label()
            {
                Name = "lbl_increments",
                Text = "Paliers de score",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_increments);

            PictureBox pb_add_increment = new PictureBox()
            {
                Name = "pb_add_increment",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_increment.Click += new EventHandler(Add_Increment);
            content.Controls.Add(pb_add_increment);

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Scénario",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            FlowLayoutPanel flp_increments = new FlowLayoutPanel()
            {
                Name = "flp_increments",
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Width = content.Width - 40,
                Height = content.Height - 260
            };
            content.Controls.Add(flp_increments);

            // Second area placement.
            btn_add_situation.Location = new Point((content.Width - btn_add_situation.Width) / 2, 100);
            lbl_increments.Location = new Point(20, 180);
            pb_add_increment.Location = new Point(lbl_increments.Location.X + lbl_increments.Width + 20, 180);
            pb_help.Location = new Point(pb_add_increment.Location.X + pb_add_increment.Width + 20, 180);
            flp_increments.Location = new Point(20, 240);

            // Adds the increments already in place.
            string scenario_path = @"" + this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                                   menu.SelectedNode.Name.Substring(8) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject scenario_data = Tools.Get_From_JSON(scenario_path + "properties.json");
            for (int i = 1; i <= int.Parse(scenario_data["increments"].ToString()); i++)
            {
                IncrementEdition ie = new IncrementEdition(this, scenario_path + "properties.json", i.ToString())
                {
                    Width = flp_increments.Width - 10
                };
                flp_increments.Controls.Add(ie);
            }
        }

        private void Add_Increment(object sender, EventArgs e)
        {
            // Filling the matching file.
            string scenario_path = @"" + this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                                   menu.SelectedNode.Name.Substring(8) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject properties_data = Tools.Get_From_JSON(scenario_path + "properties.json");

            int new_event_id = int.Parse((string)properties_data["increments"]) + 1;
            properties_data["increments"] = new_event_id;
            properties_data[new_event_id.ToString()] = new JObject()
            {
                ["score"] = 0,
                ["comment"] = ""
            };

            Tools.Set_To_JSON(scenario_path + "properties.json", properties_data);

            // Creating the UserControl responsible for the internal edition of the JSON file.
            IncrementEdition ie = new IncrementEdition(this, scenario_path + "properties.json", new_event_id.ToString())
            {
                Name = "EventEdition" + new_event_id,
                Width = content.Controls.Find("flp_increments", true)[0].Width - 40,
                Location = new Point(40, new_event_id)
            };
            content.Controls.Find("flp_increments", true)[0].Controls.Add(ie);
        }

        public void Discard_Increment(IncrementEdition sender)
        {
            string scenario_path = @"" + this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                                   menu.SelectedNode.Name.Substring(8) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject properties_data = Tools.Get_From_JSON(scenario_path + "properties.json");

            int nbr_increments = int.Parse((string)properties_data["increments"]);

            // Delete the increment then reoder the others in the JSON file.
            properties_data["increments"] = nbr_increments - 1;
            for (int i = sender.Get_Id(); i < nbr_increments; i++)
            {
                properties_data[i.ToString()] = properties_data[(i + 1).ToString()];
            }
            properties_data.Property(nbr_increments.ToString()).Remove();
            Tools.Set_To_JSON(sender.Get_File_Path(), properties_data);

            // Delete the UserControl linked to the increment.
            this.Controls.Remove(sender);
            sender.Dispose();

            // Reload all others Controls.
            this.old_category = "reload_needed";
            Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
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
            string secured_new_name = new_name.Trim();

            // Exit if name is similar.
            if(menu.SelectedNode.Text == secured_new_name) { return; }

            // Exit and display an error message if the name is already in use.
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios";
            foreach (string s in Directory.GetDirectories(sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length + 1).Split('.');
                if(folder[1] == secured_new_name)
                {
                    MessageBox.Show("Le nom de scénario " + '"' + secured_new_name + '"' + " est déjà utilisé, essayez un autre nom.",
                                    "Nom de scénario invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Rename the scenario's folder.
            Directory.Move(@"" + sc_path + Path.DirectorySeparatorChar + menu.SelectedNode.Name.Substring(8) + "." + menu.SelectedNode.Text,
                           @"" + sc_path + Path.DirectorySeparatorChar + menu.SelectedNode.Name.Substring(8) + "." + secured_new_name);

            // Rename the scenario's Node.
            menu.SelectedNode.Text = secured_new_name;
            lbl_path.Text = menu.SelectedNode.FullPath;

            // Re-configuring path-sensitives UserControls.
            foreach (IncrementEdition ie in content.Controls.OfType<IncrementEdition>())
            {
                ie.Set_File_Path(@"" + sc_path + Path.DirectorySeparatorChar + menu.SelectedNode.Name.Substring(8) + "." + secured_new_name +
                                 Path.DirectorySeparatorChar + "properties.json");
            }

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
            JObject dialogs_content = new JObject()
            {
                ["events"] = 1,
                ["1"] = new JObject()
                {
                    ["x"] = 0,
                    ["y"] = 0,
                    ["npc"] = new JObject()
                    {
                        ["id"] = 1,
                        ["name"] = "Gérard",
                        ["folder"] = 1
                    },
                    ["quizz"] = 1
                }
            };
            File.WriteAllText(@"" + access_path + parent.Nodes.Count.ToString() + "." + new_situation + Path.DirectorySeparatorChar + "dialogs.json",
                              dialogs_content.ToString());

            // Add a "items.json" to the newly created folder.
            string items_path = this.game_path + Path.DirectorySeparatorChar + "library" +
                          Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "items" +
                          Path.DirectorySeparatorChar;
            JObject items_content = new JObject()
            {
                ["events"] = 1,
                ["1"] = new JObject()
                {
                    ["x"] = 0,
                    ["y"] = 0,
                    ["item"] = new JObject()
                    {
                        ["id"] = 1,
                        ["name"] = Directory.GetFiles(items_path)[0].Split(Path.DirectorySeparatorChar).Last().Split('.')[0]
                    },
                    ["quizz"] = 1
                }
            };
            File.WriteAllText(@"" + access_path + parent.Nodes.Count.ToString() + "." + new_situation + Path.DirectorySeparatorChar + "items.json",
                              items_content.ToString());

            // Add a "environment.json" to the folder.
            string bg_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                             "images" + Path.DirectorySeparatorChar + "background" + Path.DirectorySeparatorChar;
            JObject environment_content = new JObject()
            {
                ["scene_name"] = "",
                ["scene_intro"] = "",
                ["npc_density"] = 10,
                ["background"] = Directory.GetFiles(bg_path)[0].Split(Path.DirectorySeparatorChar).Last().Split('.')[0],
                ["scene_type"] = 0
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
            if (this.old_category == menu.SelectedNode.Name) // Trigger only on form resize.
            {
                Label l1 = (Label)content.Controls.Find("lbl_scene_name", true)[0];
                Label l2 = (Label)content.Controls.Find("lbl_scene_intro", true)[0];
                TextBox t1 = (TextBox)content.Controls.Find("txt_scene_name", true)[0];
                TextBox t2 = (TextBox)content.Controls.Find("txt_scene_intro", true)[0];
                Label lbl = (Label)content.Controls.Find("lbl_tb", true)[0];
                TrackBar tb = (TrackBar)content.Controls.Find("tb_npc_density", true)[0];
                Label l3 = (Label)content.Controls.Find("lbl_background", true)[0];
                Label l4 = (Label)content.Controls.Find("lbl_scene_type", true)[0];
                ComboBox cbo1 = (ComboBox)content.Controls.Find("cbo_background", true)[0];
                ComboBox cbo2 = (ComboBox)content.Controls.Find("cbo_scene_type", true)[0];

                t1.Width = content.Width - 40 - 10 - Tools.Max_Int(l1.Width, l2.Width);
                t2.Width = content.Width - 40 - 10 - Tools.Max_Int(l1.Width, l2.Width);
                tb.Width = content.Width - lbl.Width - 10 - 80;
                cbo1.Width = content.Width - Tools.Max_Int(l3.Width, l4.Width) - 40 - 10;
                cbo2.Width = content.Width - Tools.Max_Int(l3.Width, l4.Width) - 40 - 10;

                foreach (EventEdition ee in content.Controls.OfType<EventEdition>())
                {
                    ee.Width = content.Width - 80;
                }
                return;
            }

            // Creation of all controls.

            // Creation of two arrows allowing changement of the situations' order.
            PictureBox pb_down_situation = new PictureBox()
            {
                Name = "pb_down_situation",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_down.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_down_situation.Click += new EventHandler(Down_Situation);
            content.Controls.Add(pb_down_situation);
            tlt_global.SetToolTip(pb_down_situation, "Inverse la position de cette situation avec la suivante");

            PictureBox pb_up_situation = new PictureBox()
            {
                Name = "pb_up_situation",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "arrow_up.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_up_situation.Click += new EventHandler(Up_Situation);
            content.Controls.Add(pb_up_situation);
            tlt_global.SetToolTip(pb_up_situation, "Inverse la position de cette situation avec la précédente");

            // Creation of a label reminding the situation's name.
            Label lbl_name_situation = new Label()
            {
                Name = "lbl_name_situation",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.Hand,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            lbl_name_situation.Click += new EventHandler(Ask_Rename_Situation);
            content.Controls.Add(lbl_name_situation);
            tlt_global.SetToolTip(lbl_name_situation, "Renomme la situation");

            // Creation of the hidden textbox allowing to enter a new situation's name.
            TextBox txt_rename_situation = new TextBox()
            {
                Name = "txt_rename_situation",
                Text = menu.SelectedNode.Text,
                Cursor = Cursors.IBeam,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                Width = Tools.Get_Text_Width(this, menu.SelectedNode.Text, 20) + 24,
                ShortcutsEnabled = false,
                Visible = false
            };
            txt_rename_situation.KeyPress += new KeyPressEventHandler(Rename_Situation_Txt_Keypress);
            content.Controls.Add(txt_rename_situation);

            // Creation of a PictureBox allowing to rename the situation.
            PictureBox pb_rename_situation = new PictureBox()
            {
                Name = "pb_rename_situation",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "edit.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_rename_situation.Click += new EventHandler(Ask_Rename_Situation);
            content.Controls.Add(pb_rename_situation);
            tlt_global.SetToolTip(pb_rename_situation, "Renomme la situation");

            // Creation of a PictureBox allowing to discard the situation.
            PictureBox pb_discard_situation = new PictureBox()
            {
                Name = "pb_discard_situation",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "delete.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_discard_situation.Click += new EventHandler(Discard_Situation);
            content.Controls.Add(pb_discard_situation);
            tlt_global.SetToolTip(pb_discard_situation, "Supprime la situation");

            // Creation of a PictureBox responsible for the preview display.
            PictureBox pb_preview_situation = new PictureBox()
            {
                Name = "pb_preview_situation",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "eye.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_preview_situation.Click += new EventHandler(this.Preview_Situation);
            content.Controls.Add(pb_preview_situation);
            tlt_global.SetToolTip(pb_preview_situation, "Affiche un aperçu de la situation");

            // Set the correct location of the controls (responsive with the groupbox's size).
            pb_down_situation.Location = new Point(8, 0);
            pb_up_situation.Location = new Point(pb_down_situation.Location.X + pb_down_situation.Width + 2, 0);
            lbl_name_situation.Location = new Point(pb_up_situation.Location.X + pb_up_situation.Width + 8, 0);
            txt_rename_situation.Location = new Point(lbl_name_situation.Location.X, 0);
            pb_rename_situation.Location = new Point(lbl_name_situation.Location.X + lbl_name_situation.Width, 0);
            pb_discard_situation.Location = new Point(pb_rename_situation.Location.X + pb_rename_situation.Width + 2, 0);
            pb_preview_situation.Location = new Point(pb_discard_situation.Location.X + pb_discard_situation.Width + 2, 0);



            // Recovering data from the JSON files.
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                                  Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                                  Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject situ_data = Tools.Get_From_JSON(situation_path + "dialogs.json");
            JObject item_data = Tools.Get_From_JSON(situation_path + "items.json");
            JObject envi_data = Tools.Get_From_JSON(situation_path + "environment.json");

            // Creates primary Controls of the situation.

            Label lbl_scene_name = new Label()
            {
                Name = "lbl_scene_name",
                Text = "Nom du lieu   ",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_scene_name);

            Label lbl_scene_intro = new Label()
            {
                Name = "lbl_scene_intro",
                Text = "Texte d'introduction   ",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_scene_intro);

            TextBox txt_scene_name = new TextBox()
            {
                Name = "txt_scene_name",
                Text = (string)envi_data["scene_name"],
                Tag = situation_path + "environment.json",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(8, 8, 8, 8),
                ShortcutsEnabled = false,
                Width = content.Width - Tools.Max_Int(lbl_scene_name.Width, lbl_scene_intro.Width) - 10 - 40
            };
            txt_scene_name.KeyPress += new KeyPressEventHandler(Txt_Keypress);
            content.Controls.Add(txt_scene_name);
            tlt_global.SetToolTip(txt_scene_name, "Nom du lieu où se déroule la scène, affiché en haut à droite de l'écran");

            TextBox txt_scene_intro = new TextBox()
            {
                Name = "txt_scene_intro",
                Tag = situation_path + "environment.json",
                Text = (string)envi_data["scene_intro"],
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                BorderStyle = BorderStyle.Fixed3D,
                Margin = new Padding(8, 8, 8, 8),
                ShortcutsEnabled = false,
                Width = content.Width - Tools.Max_Int(lbl_scene_name.Width, lbl_scene_intro.Width) - 10 - 40
            };
            txt_scene_intro.KeyPress += new KeyPressEventHandler(Txt_Keypress);
            content.Controls.Add(txt_scene_intro);
            tlt_global.SetToolTip(txt_scene_intro, "Texte d'introdution à la situation, affiché avant celle-ci");

            Label lbl_tb = new Label()
            {
                Name = "lbl_tb",
                Text = "Densité de PNJs : " + (string)envi_data["npc_density"],
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_tb);

            TrackBar tb_npc_density = new TrackBar()
            {
                Name = "tb_npc_density",
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                Minimum = 0,
                Maximum = 275,
                TickFrequency = 5,
                TickStyle = TickStyle.Both,
                Value = int.Parse((string)envi_data["npc_density"]),
                Width = content.Width - lbl_tb.Width - 10 - 80
            };
            tb_npc_density.ValueChanged += new EventHandler(Npc_Density_Update);
            content.Controls.Add(tb_npc_density);
            tlt_global.SetToolTip(tb_npc_density, "Modifie la densité des personnages non joueurs sans dialogue disposés aléatoirement");

            // Filling with the existing backgrounds.
            string bg_path = this.game_path + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                             "images" + Path.DirectorySeparatorChar + "background" + Path.DirectorySeparatorChar;
            List<string> list_bg = new List<string>();
            foreach (string fi in Directory.GetFiles(@"" + bg_path))
            {
                string file_name = fi.Split(Path.DirectorySeparatorChar).Last().Split('.')[0];
                if (!file_name.Contains("_hitbox"))
                {
                    list_bg.Add(file_name);
                }
            }

            Label lbl_background = new Label()
            {
                Name = "lbl_background",
                Text = "Décor   ",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_background);

            Label lbl_scene_type = new Label()
            {
                Name = "lbl_scene_type",
                Text = "Type de scène   ",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_scene_type);

            ComboBoxFix cbo_background = new ComboBoxFix()
            {
                Name = "cbo_background",
                DataSource = list_bg,
                Tag = situation_path + "environment.json",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Width = content.Width - Tools.Max_Int(lbl_background.Width, lbl_scene_type.Width) - 40 - 10,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]))
            };
            cbo_background.SelectedIndexChanged += new EventHandler(Situation_Background_Changed);
            content.Controls.Add(cbo_background);
            cbo_background.SelectedItem = (string)envi_data["background"];
            tlt_global.SetToolTip(cbo_background, "Décor présent à l'arrière-plan");

            List<string> list_scenes = new List<string>()
            {
                "Montée dans le tram", "Trajet en tram", "Descente du tram", "Accident de tram", "Montée dans le bus", "Trajet en bus",
                "Descente du bus", "Accident de bus", "Marche dans un parc", "Marche dans la ville", "Vision à la première personne"
            };
            ComboBoxFix cbo_scene_type = new ComboBoxFix()
            {
                Name = "cbo_scene_type",
                DataSource = list_scenes,
                Tag = situation_path + "environment.json",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Width = content.Width - Tools.Max_Int(lbl_background.Width, lbl_scene_type.Width) - 40 - 10,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]))
            };
            cbo_scene_type.SelectedIndexChanged += new EventHandler(Scene_Type_Changed);
            content.Controls.Add(cbo_scene_type);
            cbo_scene_type.SelectedIndex = int.Parse((string)envi_data["scene_type"]);
            tlt_global.SetToolTip(cbo_scene_type, "Type de situation se présentant au joueur");

            // Places Controls just created.
            int sync_loc = Tools.Max_Int(lbl_scene_name.Width, lbl_scene_intro.Width) + 20 + 10;
            lbl_scene_name.Location = new Point(20, 75);
            txt_scene_name.Location = new Point(sync_loc, 75);
            lbl_scene_intro.Location = new Point(20, txt_scene_name.Location.Y + txt_scene_name.Height + 10);
            txt_scene_intro.Location = new Point(sync_loc, txt_scene_name.Location.Y + txt_scene_name.Height + 10);
            lbl_tb.Location = new Point(20, txt_scene_intro.Location.Y + txt_scene_intro.Height + 15 +
                                            ((tb_npc_density.Height - lbl_tb.Height) / 2));
            tb_npc_density.Location = new Point(lbl_tb.Location.X + lbl_tb.Width + 50, txt_scene_intro.Location.Y + 
                                                txt_scene_intro.Height + 15);

            sync_loc = Tools.Max_Int(lbl_background.Width, lbl_scene_type.Width) + 20 + 10;
            lbl_background.Location = new Point(20, tb_npc_density.Location.Y + tb_npc_density.Height + 15);
            cbo_background.Location = new Point(sync_loc, lbl_background.Location.Y);
            lbl_scene_type.Location = new Point(20, cbo_background.Location.Y + cbo_background.Height + 10);
            cbo_scene_type.Location = new Point(sync_loc, lbl_scene_type.Location.Y);



            // Generates basic Label & add PictureBox bellow the previous Controls in the content panel.

            // Label displaying the area items.
            Label lbl_events = new Label()
            {
                Name = "lbl_events",
                Text = "Évènements",
                Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                AutoSize = true
            };
            content.Controls.Add(lbl_events);

            // PictureBox allowing the addition of new events.
            PictureBox pb_add_event = new PictureBox()
            {
                Name = "pb_add_event",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "add.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_add_event.Click += new EventHandler(Add_Event);
            content.Controls.Add(pb_add_event);
            tlt_global.SetToolTip(pb_add_event, "Ajoute un nouvel évènement");

            // PictureBox of help.
            PictureBox pb_help = new PictureBox()
            {
                Name = "pb_help",
                Tag = "Situations",
                Cursor = Cursors.Hand,
                Size = new Size(32, 32),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            pb_help.Click += new EventHandler(Click_Help);
            content.Controls.Add(pb_help);

            // Places the Controls just created.
            lbl_events.Location = new Point(20, 360);
            pb_add_event.Location = new Point(lbl_events.Location.X + lbl_events.Width + 20, 360);
            pb_help.Location = new Point(pb_add_event.Location.X + pb_add_event.Width + 20, 360);

            // Generating to all files an EventEdition UserControl.
            int data_size;
            switch (cbo_scene_type.SelectedIndex)
            {
                case 10:
                    data_size = int.Parse((string)item_data["events"]);
                    break;
                default:
                    data_size = int.Parse((string)situ_data["events"]);
                    break;
            }
            int last_pos = 420;
            for (int i = 1; i <= data_size; i++)
            {
                // Creating the UserControl responsible for the internal edition of the JSON file.
                EventEdition EEdition;
                switch (cbo_scene_type.SelectedIndex)
                {
                    case 10:
                        EEdition = new EventEdition(this, @"" + situation_path + "items.json", i)
                        {
                            Name = "EventEdition" + i,
                            Width = content.Width - 80,
                            Location = new Point(40, last_pos)
                        };
                        break;
                    default:
                        EEdition = new EventEdition(this, @"" + situation_path + "dialogs.json", i)
                        {
                            Name = "EventEdition" + i,
                            Width = content.Width - 80,
                            Location = new Point(40, last_pos)
                        };
                        break;
                }
                content.Controls.Add(EEdition);
                last_pos += EEdition.Height + 20;
            }
        }

        /// <summary>
        /// Adjust value of NPC's density in the environment.json file
        /// of the current situation.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Npc_Density_Update(object sender, EventArgs e)
        {
            // Diplay the new value.
            TrackBar tb = (TrackBar)sender;
            Label lbl = (Label)content.Controls.Find("lbl_tb", true)[0];
            lbl.Text = "Densité de PNJs : " + tb.Value.ToString();

            // Save the new value.
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                                  Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                                  Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject envi_data = Tools.Get_From_JSON(situation_path + "environment.json");
            envi_data["npc_density"] = tb.Value;
            try { Tools.Set_To_JSON(situation_path + "environment.json", envi_data); } catch (Exception except) { return; }
        }

        /// <summary>
        /// Synchronize value of active background in the environment.json file
        /// of the current situation.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Situation_Background_Changed(object sender, EventArgs e)
        {
            ComboBoxFix cbo = (ComboBoxFix)sender;

            JObject data = Tools.Get_From_JSON((string)cbo.Tag);
            data["background"] = cbo.Text;
            Tools.Set_To_JSON((string)cbo.Tag, data);
        }

        /// <summary>
        /// Synchronize value of scene_type in the environment.json file
        /// of the current situation.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Scene_Type_Changed(object sender, EventArgs e)
        {
            ComboBoxFix cbo = (ComboBoxFix)sender;

            JObject data = Tools.Get_From_JSON((string)cbo.Tag);
            int old_index = int.Parse(data["scene_type"].ToString());
            if (old_index == cbo.SelectedIndex) { return; }

            data["scene_type"] = cbo.SelectedIndex;
            Tools.Set_To_JSON((string)cbo.Tag, data);

            if (old_index != 10 & cbo.SelectedIndex != 10) { return; }

            List<Control> temp_ctrls = new List<Control>();
            foreach (EventEdition ee in content.Controls.OfType<EventEdition>())
            {
                temp_ctrls.Add(ee);
            }
            foreach (Control temp_ctrl in temp_ctrls)
            {
                content.Controls.Remove(temp_ctrl);
            }
            
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                    Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                    Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject situ_data = Tools.Get_From_JSON(situation_path + "dialogs.json");
            JObject item_data = Tools.Get_From_JSON(situation_path + "items.json");
            JObject envi_data = Tools.Get_From_JSON(situation_path + "environment.json");
            int data_size;
            switch (cbo.SelectedIndex)
            {
                case 10:
                    data_size = int.Parse((string)item_data["events"]);
                    break;
                default:
                    data_size = int.Parse((string)situ_data["events"]);
                    break;
            }
            int last_pos = 420;
            for (int i = 1; i <= data_size; i++)
            {
                // Creating the UserControl responsible for the internal edition of the JSON file.
                EventEdition EEdition;
                switch (cbo.SelectedIndex)
                {
                    case 10:
                        EEdition = new EventEdition(this, @"" + situation_path + "items.json", i)
                        {
                            Name = "EventEdition" + i,
                            Width = content.Width - 80,
                            Location = new Point(40, last_pos)
                        };
                        break;
                    default:
                        EEdition = new EventEdition(this, @"" + situation_path + "dialogs.json", i)
                        {
                            Name = "EventEdition" + i,
                            Width = content.Width - 80,
                            Location = new Point(40, last_pos)
                        };
                        break;
                }
                content.Controls.Add(EEdition);
                last_pos += EEdition.Height + 20;
            }
        }

        /// <summary>
        /// Creates a new event in the current situation.
        /// Both displayed and created on hard drive.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Add_Event(object sender, EventArgs e)
        {
            // Filling the matching file.
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                                  Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                                  Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject situ_data = Tools.Get_From_JSON(situation_path + "dialogs.json");
            JObject item_data = Tools.Get_From_JSON(situation_path + "items.json");
            int cbo_index = ((ComboBox)content.Controls.Find("cbo_scene_type", true)[0]).SelectedIndex;
            JObject data;
            string kind;
            string file;
            switch (cbo_index)
            {
                case 10:
                    data = item_data;
                    kind = "item";
                    file = "items.json";
                    break;
                default:
                    data = situ_data;
                    kind = "npc";
                    file = "dialogs.json";
                    break;
            }

            int new_event_id = int.Parse((string)data["events"]) + 1;
            data["events"] = new_event_id;
            data[new_event_id.ToString()] = new JObject()
            {
                ["x"] = 0,
                ["y"] = 0,
                [kind] = new JObject()
                {
                    ["id"] = 1
                },
                ["quizz"] = 1
            };

            Tools.Set_To_JSON(situation_path + file, data);

            // Creating the UserControl responsible for the internal edition of the JSON file.
            EventEdition EEdition = new EventEdition(this, @"" + situation_path + file, new_event_id)
            {
                Name = "EventEdition" + new_event_id,
                Width = content.Width - 80,
                Location = new Point(40, new_event_id)
            };
            content.Controls.Add(EEdition);

            // Replacing all EventEdition UserControls in the right place.
            int i = 1;
            int last_pos = 420;
            foreach (EventEdition ee in content.Controls.OfType<EventEdition>())
            {
                ee.Location = new Point(40, last_pos - content.VerticalScroll.Value);
                last_pos += ee.Height + 20;
                i++;
            }
        }

        /// <summary>
        /// Deletes the event inside the situation's "dialogs.json" file.
        /// Then the display of the sender ( EventEdition ). 
        /// </summary>
        /// <param name="sender">Instance of EventEdition UserControl sending a self-destruction request.</param>
        public void Discard_Event(EventEdition sender)
        {
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                                  Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                                  Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject situ_data = Tools.Get_From_JSON(situation_path + "dialogs.json");
            JObject item_data = Tools.Get_From_JSON(situation_path + "items.json");
            int cbo_index = ((ComboBox)content.Controls.Find("cbo_scene_type", true)[0]).SelectedIndex;
            JObject data;
            switch (cbo_index)
            {
                case 10:
                    data = item_data;
                    break;
                default:
                    data = situ_data;
                    break;
            }

            int nbr_events = int.Parse((string)data["events"]);
            if (nbr_events < 2)
            {
                MessageBox.Show("Vous allez supprimer l'intégralité des évènements de la situation.\nSi vous souhaitez supprimer la situation," +
                                "merci de cliquer sur l'icône en à la droite du titre.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Delete the event then reoder the others in the JSON file.
            data["events"] = nbr_events - 1;
            for (int i = sender.Get_Event_Id(); i < nbr_events; i++)
            {
                data[i.ToString()] = data[(i + 1).ToString()];
            }
            data.Property(nbr_events.ToString()).Remove();
            Tools.Set_To_JSON(sender.Get_File_Path(), data);

            // Delete the UserControl linked to the event.
            this.Controls.Remove(sender);
            sender.Dispose();

            // Reload all others Controls.
            this.old_category = "reload_needed";
            Menu_AfterSelect(menu, new TreeViewEventArgs(new TreeNode()));
        }

        /// <summary>
        /// Starts a PlacementEdition UserControl to place the event.
        /// </summary>
        /// <param name="sender">Instance of EventEdition UserControl sending a placement request.</param>
        public void Place_Event(EventEdition sender)
        {
            if (this.event_placement != null) { return; }

            string images_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                                "images" + Path.DirectorySeparatorChar;
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                                  Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                                  Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject envi_data = Tools.Get_From_JSON(situation_path + "environment.json");
            JObject situ_data = Tools.Get_From_JSON(situation_path + "dialogs.json");
            JObject item_data = Tools.Get_From_JSON(situation_path + "items.json");
            int cbo_index = ((ComboBox)content.Controls.Find("cbo_scene_type", true)[0]).SelectedIndex;
            JObject data;
            string kind;
            switch (cbo_index)
            {
                case 10:
                    data = item_data;
                    kind = "item";
                    break;
                default:
                    data = situ_data;
                    kind = "npc";
                    break;
            }
            Image img;

            PictureBox placing_npc = new PictureBox();
            List<PictureBox> list_placed_npcs = new List<PictureBox>();
            List<Point> list_placed_npcs_points = new List<Point>();
            for (int i = 1; i <= int.Parse((string)data["events"]); i++)
            {
                switch (cbo_index)
                {
                    case 10:
                        img = Tools.Image_From_File(images_path + "items" + Path.DirectorySeparatorChar +
                                                    data[i.ToString()]["item"]["name"].ToString() + ".png");
                        break;
                    default:
                        string npcs_folder_path = @"" + this.game_path + Path.DirectorySeparatorChar + "library" +
                                      Path.DirectorySeparatorChar + "npcs" + Path.DirectorySeparatorChar;
                        data = Tools.Get_From_JSON(npcs_folder_path + i.ToString() + ".json");
                        img = Tools.Image_From_File(this.game_path + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                                "images" + Path.DirectorySeparatorChar + "characters" + Path.DirectorySeparatorChar +
                                data["folder"].ToString() + Path.DirectorySeparatorChar + "1_0.png");
                        break;
                }

                if (i == sender.Get_Event_Id() || int.Parse((string)data[i.ToString()]["x"]) != 0 ||
                    int.Parse((string)data[i.ToString()]["y"]) != 0)
                {
                    PictureBox pb_temp = new PictureBox()
                    {
                        Name = "pb_temp" + i,
                        Tag = i,
                        Image = img,
                        SizeMode = PictureBoxSizeMode.AutoSize
                    };
                    if (i == sender.Get_Event_Id())
                    {
                        placing_npc = pb_temp;
                    }
                    else
                    {
                        list_placed_npcs.Add(pb_temp);
                        list_placed_npcs_points.Add(new Point(int.Parse((string)data[i.ToString()]["x"]),
                                                              int.Parse((string)data[i.ToString()]["y"])));
                    }
                }
            }

            int scene_type = int.Parse(envi_data["scene_type"].ToString());
            switch (scene_type)
            {
                case int n when (n == 2 || n == 6):
                    img = Tools.Image_From_File(images_path + "platform" + Path.DirectorySeparatorChar + "Platform.png");
                    break;
                case int n when (n == 4 || n == 5 || n == 7):
                    img = Tools.Image_From_File(images_path + "vehicle" + Path.DirectorySeparatorChar + "bus" +
                                                Path.DirectorySeparatorChar + "bus_inside.png");
                    break;
                case int n when (n > 7):
                    img = Tools.Image_From_File(images_path + "background" + Path.DirectorySeparatorChar +
                                                envi_data["background"].ToString() + ".png");
                    break;
                default:
                    img = Tools.Image_From_File(images_path + "vehicle" + Path.DirectorySeparatorChar + "tram" +
                                                Path.DirectorySeparatorChar + "tram_inside.png");
                    break;
            }

            PictureBox pb_background = new PictureBox()
            {
                Name = "pb_background",
                Image = img,
                SizeMode = PictureBoxSizeMode.AutoSize,
                Location = new Point(0, 0)
            };

            this.event_placement = new PlacementEdition(this, placing_npc, list_placed_npcs, list_placed_npcs_points, pb_background, scene_type);
            this.event_placement.ShowDialog();
        }

        /// <summary>
        /// Is called by PlacementEdition to transmit the new event position.
        /// </summary>
        /// <param name="id">ID of the event.</param>
        /// <param name="new_pos">New position of the event.</param>
        public void Reset_Place_Event(int id, Point new_pos)
        {
            // Reset of the PlacementEdition local Form.
            this.event_placement.Dispose();
            this.event_placement = null;

            // Saving new event Point.
            string situation_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" +
                                  Path.DirectorySeparatorChar + menu.SelectedNode.Parent.Name.Remove(0, "scenario".Length) + "." + menu.SelectedNode.Parent.Text +
                                  Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text + Path.DirectorySeparatorChar;
            JObject situ_data = Tools.Get_From_JSON(situation_path + "dialogs.json");
            JObject item_data = Tools.Get_From_JSON(situation_path + "items.json");
            int cbo_index = ((ComboBox)content.Controls.Find("cbo_scene_type", true)[0]).SelectedIndex;
            JObject data;
            string file;
            switch (cbo_index)
            {
                case 10:
                    data = item_data;
                    file = "items.json";
                    break;
                default:
                    data = situ_data;
                    file = "dialogs.json";
                    break;
            }
            data[id.ToString()]["x"] = new_pos.X;
            data[id.ToString()]["y"] = new_pos.Y;
            Tools.Set_To_JSON(situation_path + file, data);
        }

        /// <summary>
        /// Switch the select situation with the one bellow it.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Down_Situation(object sender, EventArgs e)
        {
            TreeNode tn = menu.SelectedNode;
            TreeNode tns = tn.Parent;
            int index = tn.Index;
            menu.SelectedNode = tns;

            // Return if last node.
            if (index == tns.LastNode.Index) { return; }
            tns.Nodes.Remove(tn);
            tns.Nodes.Insert(index + 1, tn);
            
            Order_Files_Situations(tn);
        }

        /// <summary>
        /// Switch the selected situation with the one upside it.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Up_Situation(object sender, EventArgs e)
        {
            TreeNode tn = menu.SelectedNode;
            TreeNode tns = tn.Parent;
            int index = tn.Index;
            menu.SelectedNode = tns;

            // Return if fisrt node.
            if (index == 0) { return; }
            tns.Nodes.Remove(tn);
            tns.Nodes.Insert(index - 1, tn);

            Order_Files_Situations(tn);
        }

        /// <summary>
        /// Technical function re-ordering all situations after a switch, both in the menu and in the folder.
        /// </summary>
        private void Order_Files_Situations(TreeNode tn)
        {
            TreeNode tns = tn.Parent;
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                             + (tns.Index + 1) + "." + tns.Text;
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
            menu.SelectedNode = tn;
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
            string secured_new_name = new_name.Trim();

            // Exit if name is similar.
            if (menu.SelectedNode.Text == secured_new_name) { return; }

            // Exit and display an error message if the name is already in use.
            string sc_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar +
                             +(menu.SelectedNode.Parent.Index + 1) + "." + menu.SelectedNode.Parent.Text;
            foreach (string s in Directory.GetDirectories(sc_path))
            {
                string[] folder = s.Remove(0, sc_path.Length + 1).Split('.');
                if (folder[1] == secured_new_name)
                {
                    MessageBox.Show("Le nom de la situation " + '"' + secured_new_name + '"' + " est déjà utilisé, essayez un autre nom.",
                                    "Nom de scénario invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Rename the situation's folder.
            Directory.Move(@"" + sc_path + Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text,
                           @"" + sc_path + Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + secured_new_name);

            // Rename the situation's Node.
            menu.SelectedNode.Text = secured_new_name;
            lbl_path.Text = menu.SelectedNode.FullPath;

            // Re-configuring path-sensitives UserControls.
            foreach (EventEdition ee in content.Controls.OfType<EventEdition>())
            {
                ee.Set_File_Path(sc_path + Path.DirectorySeparatorChar + (menu.SelectedNode.Index + 1) + "." + secured_new_name +
                                 Path.DirectorySeparatorChar + "dialogs.json");
            }
            content.Controls.Find("txt_scene_name", true)[0].Tag = sc_path + Path.DirectorySeparatorChar +
                (menu.SelectedNode.Index + 1) + "." + secured_new_name + Path.DirectorySeparatorChar + "environment.json";
            content.Controls.Find("txt_scene_intro", true)[0].Tag = sc_path + Path.DirectorySeparatorChar + 
                (menu.SelectedNode.Index + 1) + "." + secured_new_name + Path.DirectorySeparatorChar + "environment.json";

            content.Controls.Find("cbo_background", true)[0].Tag = sc_path + Path.DirectorySeparatorChar +
                (menu.SelectedNode.Index + 1) + "." + secured_new_name + Path.DirectorySeparatorChar + "environment.json";
            content.Controls.Find("cbo_scene_type", true)[0].Tag = sc_path + Path.DirectorySeparatorChar +
                (menu.SelectedNode.Index + 1) + "." + secured_new_name + Path.DirectorySeparatorChar + "environment.json";

            // Repositioning size-sensitives contents.
            TextBox t = (TextBox)content.Controls.Find("txt_rename_situation", false)[0];
            t.Visible = false;
            t.Width = (menu.SelectedNode.Text.Length * 10) + 20;
            content.Controls.Find("lbl_name_situation", false)[0].Text = menu.SelectedNode.Text;
            content.Controls.Find("pb_rename_situation", false)[0].Location = new Point(content.Controls.Find("lbl_name_situation", false)[0].Location.X +
                                                                                        content.Controls.Find("lbl_name_situation", false)[0].Width, 0);
            content.Controls.Find("pb_discard_situation", false)[0].Location = new Point(content.Controls.Find("pb_rename_situation", false)[0].Location.X +
                                                                                         content.Controls.Find("pb_rename_situation", false)[0].Width + 2, 0);
            content.Controls.Find("pb_preview_situation", false)[0].Location = new Point(content.Controls.Find("pb_discard_situation", false)[0].Location.X +
                                                      content.Controls.Find("pb_discard_situation", false)[0].Width + 2, 0);
            content.Controls.Find("pb_rename_situation", false)[0].Visible = true;
            content.Controls.Find("lbl_name_situation", false)[0].Visible = true;
        }

        /// <summary>
        /// Discard the currently selected situation.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Discard_Situation(object sender, EventArgs e)
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
        /// Display a preview of the current situation.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Preview_Situation(object sender, EventArgs e)
        {
            if (this.preview != null)
            {
                this.preview.Close();
                this.preview = null;
            }
            this.preview = new GameWindow(this.game, (menu.SelectedNode.Parent.Index + 1) + "." + menu.SelectedNode.Parent.Text,
                                            (menu.SelectedNode.Index + 1) + "." + menu.SelectedNode.Text);
            this.preview.Show();
            this.preview.Size = new Size(600, 400);
            Rectangle screen = Screen.FromControl(this).Bounds;
            this.preview.Location = new Point((screen.Width - this.preview.Width) / 2, (screen.Height - this.preview.Height) / 2);
        }

        private void Click_Help(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            Help new_help = new Help(ctrl.Tag.ToString());
            new_help.Show();
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
