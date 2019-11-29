using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class QuizzEdition : UserControl
    {
        // Attributes.

        private int id;
        private string file_path;
        private JObject data;
        private JObject theme;
        private int prev_line_loc = 50;
        private List<string> cbo_redirect_list;
        private bool loading = false;

        // Methods.
        
        /// <summary>
        /// Setup the parameters necessary to the UserControl.
        /// </summary>
        /// <param name="file_path">Absolute path of the linked file.</param>
        public QuizzEdition(Editor editor, string file_path)
        {
            InitializeComponent();
            string file_name = file_path.Split(Path.DirectorySeparatorChar).Last();
            this.id = int.Parse(file_name.Split('.')[0]);
            this.file_path = file_path;
            this.data = Tools.Get_From_JSON(file_path);
            this.DoubleBuffered = true;
            this.theme = editor.Get_Theme();

            Reload_Redirections();
        }

        /// <summary>
        /// Load and display the data of the linked file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void QuizzEdition_Load(object sender, EventArgs e)
        {
            // Setup the theme to match the one of the editor.
            this.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            txt_question.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
            txt_question.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            cbo_audio.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
            cbo_audio.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            // Add the question.
            txt_question.Text = (string)this.data["question"];
            cbo_audio.SelectedIndex = int.Parse((string)this.data["audio"]);

            // Add existing choices to display.
            for (int i = 1; i <= int.Parse((string)data["choices"]); i++)
            {
                Add_Choice(i);
            }
        }

        /// <summary>
        /// Reload the existing dialogs upon signal from the Editor.
        /// </summary>
        public void Reload_Redirections()
        {
            this.cbo_redirect_list = new List<string>() { "[_CONTINUE_]", "[_FIN_]" };
            JObject file_data;
            DirectoryInfo file_dir = Directory.GetParent(this.file_path);
            int nbr_files = file_dir.GetFiles().Length;
            foreach (FileInfo f in file_dir.GetFiles())
            {
                file_data = Tools.Get_From_JSON(f.FullName);
                if (nbr_files < 10)
                {
                    this.cbo_redirect_list.Add("[" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
                }
                else if (nbr_files < 100)
                {
                    if (f.Name.Split('.')[0].Length == 1)
                    {
                        this.cbo_redirect_list.Add("[0" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
                    }
                    else
                    {
                        this.cbo_redirect_list.Add("[" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
                    }
                }
                else
                {
                    if (f.Name.Split('.')[0].Length == 1)
                    {
                        this.cbo_redirect_list.Add("[00" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
                    }
                    else if (f.Name.Split('.')[0].Length == 2)
                    {
                        this.cbo_redirect_list.Add("[0" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
                    }
                    else
                    {
                        this.cbo_redirect_list.Add("[" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
                    }
                }
            }
            cbo_redirect_list.Sort();
            this.loading = true;
            int i = 1;
            foreach (Panel pan in this.Controls.OfType<Panel>())
            {
                pan.Width = this.Width - 20;
                int id = int.Parse(pan.Name.Remove(0, "pan_choice".Length));

                ComboBox cbo_redirect = (ComboBox)pan.Controls.Find("cbo_redirect" + id, false)[0];
                cbo_redirect.DataSource = new List<string>(cbo_redirect_list);
                cbo_redirect.Name = "cbo_redirect" + i;
                cbo_redirect.SelectedIndex = int.Parse((string)this.data["c" + i]["redirect"]) + 1;
                i++;
            }
            this.loading = false;
        }

        /// <summary>
        /// Get method for the QuizzEdition's id.
        /// </summary>
        /// <returns>Instance's id.</returns>
        public int Get_Id()
        {
            return this.id;
        }

        /// <summary>
        /// Verify, save or cancel the input data to the linked file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Txt_Question_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '\'', '.', ',', '?', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                this.data["question"] = t.Text.Trim();
                Tools.Set_To_JSON(this.file_path, this.data);
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)this.data["question"];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == '<')
            {
                t.Text += "<Nom>";
                e.Handled = true;
                t.Focus();
                t.SelectionStart = t.Text.Length;
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || autorized_chars.Contains(e.KeyChar) || e.KeyChar == (char)8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 128) // Avoid endless names.
            {
                if (e.KeyChar == (char)8) // Still backspace.
                {
                    t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                    ((Editor)this.ParentForm).Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                ((Editor)this.ParentForm).Set_Saved(false);
            }
            Console.WriteLine("Hehe");

            // Adapt the TextBox's size.
            t.Width = Tools.Min_Int(Tools.Get_Text_Width(this, txt_question.Text, 20) + 12,
                                    this.Width - 10 - 30 - 10 - cbo_audio.Width - 10 - 30 - 10);
            pb_add.Location = new Point(txt_question.Location.X + txt_question.Width + 8, 10);
            cbo_audio.Location = new Point(pb_add.Location.X + pb_add.Width + 8, 9);
        }

        /// <summary>
        /// Triggers upon click on the PictureBox.
        /// Add a new choice to the file through a new JObject inserted into the file's data.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Add_Choice(object sender, EventArgs e)
        {
            int nbr_choices = int.Parse((string)this.data["choices"]);
            this.data["choices"] = nbr_choices + 1;
            JObject new_choice = new JObject()
            {
                ["answer"] = "Réponse " + (nbr_choices + 1).ToString(),
                ["score"] = 0,
                ["redirect"] = 0
            };
            this.data["c" + (nbr_choices + 1).ToString()] = new_choice;
            Tools.Set_To_JSON(this.file_path, this.data);
            Add_Choice(nbr_choices + 1);
            ((Editor)this.ParentForm).Update_Dialogs();
        }

        /// <summary>
        /// Creates and adds all necessary Controls to the QuizzEdition for displaying a choice of the specified id.
        /// </summary>
        /// <param name="id">Id of the choice which need Controls management.</param>
        public void Add_Choice(int id)
        {
            // Generates choice's management controls.

            // Panel containing all following controls.
            Panel pan_choice = new Panel()
            {
                Name = "pan_choice" + id,
                BackColor = Color.FromArgb(int.Parse((string)this.theme["1"]["R"]), int.Parse((string)this.theme["1"]["G"]), int.Parse((string)this.theme["1"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                Width = this.Width - 20,
                Height = 50
            };
            this.Controls.Add(pan_choice);

            // Textbox for the answer.
            TextBox txt_answer = new TextBox()
            {
                Name = "txt_answer" + id,
                Text = (string)((JObject)data["c" + id])["answer"],
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                ShortcutsEnabled = false,
                Tag = id,
                BorderStyle = BorderStyle.FixedSingle
            };
            txt_answer.KeyPress += new KeyPressEventHandler(this.Txt_Answer_KeyPress);
            pan_choice.Controls.Add(txt_answer);
            toolTip.SetToolTip(txt_answer, "Réponse proposée.");

            // NumericUpDown accepting only numbers for the score.
            NumericUpDownFix nud_score = new NumericUpDownFix()
            {
                Name = "nud_score" + id,
                Maximum = 1000000M,
                Minimum = -1000000M,
                Increment = 1,
                Text = int.Parse(((JObject)data["c" + id])["score"].ToString()).ToString(),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                Tag = id,
                BorderStyle = BorderStyle.FixedSingle
            };
            nud_score.KeyPress += new KeyPressEventHandler(this.Nud_Score_KeyPress);
            nud_score.ValueChanged += new EventHandler(this.Nud_Score_ValueChanged);
            pan_choice.Controls.Add(nud_score);
            toolTip.SetToolTip(nud_score, "Score donné ou retiré au choix de cette réponse, négatif " +
                               "\npour une réponse fausse et positif pour réponse juste.");

            // ComboBox for the redirection (or not) to another existing dialog.
            ComboBox cbo_redirect = new ComboBox()
            {
                Name = "cbo_redirect" + id,
                DataSource = new List<string>(cbo_redirect_list),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                                               System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"])),
                ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"])),
                Tag = id
            };
            cbo_redirect.SelectedIndexChanged += new EventHandler(this.Cbo_Redirect_SelectedIndexChanged);
            pan_choice.Controls.Add(cbo_redirect);
            toolTip.SetToolTip(cbo_redirect, "Dialogue vers lequel le joueur sera redirigé lors de ce choix." +
                               "\nLa redirection vers [FIN] indique la fin de la situation.");
            try
            {
                cbo_redirect.SelectedIndex = int.Parse(((JObject)data["c" + id])["redirect"].ToString()) + 1;
            }
            catch (System.FormatException e)
            {
                cbo_redirect.SelectedIndex = 0;
            }

            // PictureBox allowing to delete this choice.
            PictureBox pb_discard_choice = new PictureBox()
            {
                Name = "pb_discard_choice" + id,
                Cursor = Cursors.Hand,
                Size = new Size(30, 30),
                Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "internal" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "delete.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = id
            };
            pb_discard_choice.Click += new EventHandler(this.Discard_Choice);
            pan_choice.Controls.Add(pb_discard_choice);

            // Sizing of all created controls.
            int size_taken = 10*2 + 8*3 + nud_score.Width + pb_discard_choice.Width;
            int size_left = pan_choice.Width - size_taken;

            txt_answer.Width = size_left / 2;
            cbo_redirect.Width = size_left / 2;

            // Placement of all created controls.
            pan_choice.Location = new Point(10, this.prev_line_loc);
            txt_answer.Location = new Point(10, 10);
            nud_score.Location = new Point(txt_answer.Location.X + txt_answer.Width + 8, 10);
            cbo_redirect.Location = new Point(nud_score.Location.X + nud_score.Width + 8, 10);
            pb_discard_choice.Location = new Point(cbo_redirect.Location.X + cbo_redirect.Width + 8, 10);

            this.prev_line_loc += pan_choice.Height + 10;
            this.Height = this.prev_line_loc;
        }

        /// <summary>
        /// Verify, save or cancel the input data to the linked file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Txt_Answer_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '.', ',', '\'', '?', '!', '-', '°', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                ((JObject)this.data["c" + t.Tag])["answer"] = t.Text.Trim();
                Tools.Set_To_JSON(this.file_path, this.data); // Set the entered setting as a stored blueprint for npc.
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)((JObject)this.data["c" + t.Tag])["answer"];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || autorized_chars.Contains(e.KeyChar) || e.KeyChar == (char)8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 128) // Avoid endless names.
            {
                if (e.KeyChar == (char)8) // Still backspace.
                {
                    t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                    ((Editor)this.ParentForm).Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                ((Editor)this.ParentForm).Set_Saved(false);
            }
        }

        /// <summary>
        /// Verify, save or cancel the input data to the linked file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Nud_Score_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumericUpDownFix t = (NumericUpDownFix)sender;
            List<char> autorized_chars = new List<char>() { ' ', ',', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                ((JObject)this.data["c" + t.Tag])["score"] = int.Parse(t.Text.Trim());
                Tools.Set_To_JSON(this.file_path, this.data);
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)((JObject)this.data["c" + t.Tag])["score"];
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (!(Char.IsLetterOrDigit(e.KeyChar) || autorized_chars.Contains(e.KeyChar) || e.KeyChar == (char)8)) // (char)8 => Backspace.
            {
                e.Handled = true;
            }
            else if (t.Text.Length > 128) // Avoid endless names.
            {
                if (e.KeyChar == (char)8) // Still backspace.
                {
                    t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                    ((Editor)this.ParentForm).Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                ((Editor)this.ParentForm).Set_Saved(false);
            }
        }

        private void Nud_Score_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDownFix nud = (NumericUpDownFix)sender;
            nud.BackColor = Color.FromArgb(56, 32, 32);
            ((Editor)this.ParentForm).Set_Saved(false);
        }

        /// <summary>
        /// Save the input data to the linked file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Cbo_Redirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.loading) { return; }
            ComboBox cbo = (ComboBox)sender;
            ((JObject)this.data["c" + cbo.Tag])["redirect"] = cbo.SelectedIndex - 1;
            Tools.Set_To_JSON(this.file_path, this.data);
        }

        /// <summary>
        /// Discard a choice identified by the sender's Tag.
        /// Both in the JSON file and on the display.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Discard_Choice(object sender, EventArgs e)
        {
            int nbr_choices = int.Parse((string)this.data["choices"]);
            if(nbr_choices < 2)
            {
                MessageBox.Show("Vous allez supprimer l'intégralité des choix du dialogue.\nSi vous souhaitez supprimer le dialogue," +
                                "merci de cliquer sur la croix en haut à droite.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Delete the choice then reoder the others in the JSON file.
            this.data["choices"] = nbr_choices - 1;
            for(int i = (int)((PictureBox)sender).Tag; i < nbr_choices; i++)
            {
                this.data["c" + i] = this.data["c" + (i + 1)];
            }
            data.Property("c" + nbr_choices).Remove();
            Tools.Set_To_JSON(this.file_path, this.data);

            // Delete the panel linked to the choice.
            this.prev_line_loc -= ((PictureBox)sender).Parent.Height + 10;
            this.Controls.Remove(((PictureBox)sender).Parent);
            this.Height = this.prev_line_loc;
            QuizzEdition_Resize(this, new EventArgs());
            ((Editor)this.ParentForm).Update_Dialogs();
        }

        /// <summary>
        /// Resize the Controls in order to match the new size of the UserControl.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void QuizzEdition_Resize(object sender, EventArgs e)
        {
            txt_question.Width = Tools.Min_Int(Tools.Get_Text_Width(this, txt_question.Text, 20) + 12,
                                    this.Width - 10 - 30 - 10 - cbo_audio.Width - 10 - 30 - 10);
            pb_add.Location = new Point(txt_question.Location.X + txt_question.Width + 8, 10);
            cbo_audio.Location = new Point(pb_add.Location.X + pb_add.Width + 8, 9);
            pb_delete_all.Location = new Point(this.Width - pb_delete_all.Width - 10, 10);
            this.Height = this.prev_line_loc;

            this.prev_line_loc = 50;
            foreach(Panel pan in this.Controls.OfType<Panel>())
            {
                pan.Width = this.Width - 20;
                int id = int.Parse(pan.Name.Remove(0, "pan_choice".Length));

                // Sizing of all contained controls.
                TextBox txt_answer = (TextBox)pan.Controls.Find("txt_answer" + id, false)[0];
                NumericUpDownFix nud_score = (NumericUpDownFix)pan.Controls.Find("nud_score" + id, false)[0];
                ComboBox cbo_redirect = (ComboBox)pan.Controls.Find("cbo_redirect" + id, false)[0];
                PictureBox pb_discard_choice = (PictureBox)pan.Controls.Find("pb_discard_choice" + id, false)[0];

                int size_taken = 10 * 2 + 8 * 3 + nud_score.Width + pb_discard_choice.Width;
                int size_left = pan.Width - size_taken;

                txt_answer.Width = size_left / 2;
                cbo_redirect.Width = size_left / 2;

                // Placement of all created controls.
                pan.Location = new Point(10, this.prev_line_loc);
                txt_answer.Location = new Point(10, 10);
                nud_score.Location = new Point(txt_answer.Location.X + txt_answer.Width + 8, 10);
                cbo_redirect.Location = new Point(nud_score.Location.X + nud_score.Width + 8, 10);
                pb_discard_choice.Location = new Point(cbo_redirect.Location.X + cbo_redirect.Width + 8, 10);

                this.prev_line_loc += pan.Height + 10;
            }
        }

        /// <summary>
        /// Asks to the Editor to delete both this QuizzEdition and his linked file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Delete_All(object sender, EventArgs e)
        {
            ((Editor)this.ParentForm).Discard_Dialog(this);
        }

        private void Cbo_audio_SelectedIndexChanged(object sender, EventArgs e)
        {
            data["audio"] = cbo_audio.SelectedIndex;
            Tools.Set_To_JSON(this.file_path, data);
        }
    }
}
