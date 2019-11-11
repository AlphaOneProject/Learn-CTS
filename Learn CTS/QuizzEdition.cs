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
        private int prev_line_loc = 50;
        private List<string> cbo_redirect_list;

        // Methods.
        
        public QuizzEdition(string file_path)
        {
            InitializeComponent();
            string file_name = file_path.Split(Path.DirectorySeparatorChar).Last();
            this.id = int.Parse(file_name.Split('.')[0]);
            this.file_path = file_path;
            this.data = Tools.Get_From_JSON(file_path);

            Reload_Redirections();
        }

        private void QuizzEdition_Load(object sender, EventArgs e)
        {
            // Add the question.
            txt_question.Text = (string)this.data["question"];

            // Add existing choices to display.
            for (int i = 1; i <= int.Parse((string)data["choices"]); i++)
            {
                Add_Choice(i);
            }
        }

        public void Reload_Redirections()
        {
            this.cbo_redirect_list = new List<string>() { "[FIN]" };
            JObject file_data;
            DirectoryInfo file_dir = Directory.GetParent(this.file_path);
            foreach (FileInfo f in file_dir.GetFiles())
            {
                file_data = Tools.Get_From_JSON(f.FullName);
                this.cbo_redirect_list.Add("[" + f.Name.Split('.')[0] + "] > " + file_data["question"]);
            }
        }

        public int Get_Id()
        {
            return this.id;
        }

        private void Txt_Question_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '?', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                this.data["question"] = t.Text.Trim();
                Tools.Set_To_JSON(this.file_path, this.data);
                t.BackColor = Color.FromArgb(56, 56, 56);
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)this.data["question"];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(56, 56, 56);
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
                    t.BackColor = Color.FromArgb(56, 32, 32);
                    ((Editor)this.ParentForm).Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(56, 32, 32);
                ((Editor)this.ParentForm).Set_Saved(false);
            }

            // Adapt the TextBox's size.
            t.Width = Tools.Min_Int(Tools.Get_Text_Width(this, t.Text, 20) + 12,
                                    this.Width - 10 - 30 - 8 - 10 - 8 - 30 - 10);
            pb_add.Location = new Point(txt_question.Location.X + txt_question.Width + 8, 10);
        }

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
        }

        public void Add_Choice(int id)
        {
            // Generates choice's management controls.

            // Panel containing all following controls.
            Panel pan_choice = new Panel()
            {
                Name = "pan_choice" + id,
                BackColor = Color.FromArgb(46, 46, 46),
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
                BackColor = Color.FromArgb(56, 56, 56),
                ForeColor = Color.White,
                ShortcutsEnabled = false,
                Tag = id
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
                BackColor = Color.FromArgb(56, 56, 56),
                ForeColor = Color.White,
                Tag = id
            };
            nud_score.KeyPress += new KeyPressEventHandler(this.Nud_Score_KeyPress);
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
                BackColor = Color.FromArgb(56, 56, 56),
                ForeColor = Color.White,
                Tag = id
            };
            cbo_redirect.SelectedIndexChanged += new EventHandler(this.Cbo_Redirect_SelectedIndexChanged);
            pan_choice.Controls.Add(cbo_redirect);
            toolTip.SetToolTip(cbo_redirect, "Dialogue vers lequel le joueur sera redirigé lors de ce choix." +
                               "\nLa redirection vers [FIN] indique la fin de la situation.");
            try
            {
                cbo_redirect.SelectedIndex = int.Parse(((JObject)data["c" + id])["redirect"].ToString());
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

        public void Txt_Answer_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '.', ',', '\'', '?', '!', '-', '°', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                ((JObject)this.data["c" + t.Tag])["answer"] = t.Text.Trim();
                Tools.Set_To_JSON(this.file_path, this.data); // Set the entered setting as a stored blueprint for npc.
                t.BackColor = Color.FromArgb(56, 56, 56);
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)((JObject)this.data["c" + t.Tag])["answer"];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(56, 56, 56);
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
                    t.BackColor = Color.FromArgb(56, 32, 32);
                    ((Editor)this.ParentForm).Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(56, 32, 32);
                ((Editor)this.ParentForm).Set_Saved(false);
            }
        }

        public void Nud_Score_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumericUpDown t = (NumericUpDown)sender;
            List<char> autorized_chars = new List<char>() { ' ', ',', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                ((JObject)this.data["c" + t.Tag])["score"] = int.Parse(t.Text.Trim());
                Tools.Set_To_JSON(this.file_path, this.data);
                t.BackColor = Color.FromArgb(56, 56, 56);
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)((JObject)this.data["c" + t.Tag])["score"];
                t.BackColor = Color.FromArgb(56, 56, 56);
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
                    t.BackColor = Color.FromArgb(56, 32, 32);
                    ((Editor)this.ParentForm).Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(56, 32, 32);
                ((Editor)this.ParentForm).Set_Saved(false);
            }
        }

        private void Cbo_Redirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            ((JObject)this.data["c" + cbo.Tag])["redirect"] = cbo.SelectedIndex;
            Tools.Set_To_JSON(this.file_path, this.data);
        }

        public void Discard_Choice(object sender, EventArgs e)
        {
            int nbr_choices = int.Parse((string)this.data["choices"]);
            if(nbr_choices < 2)
            {
                MessageBox.Show("Vous allez supprimer l'intégralité des choix du dialogue.\nSi vous souhaitez supprimer le dialogue," +
                                "merci de cliquer sur la croix en haut à droite.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Delete the choice from the JSON file.
            this.data["choices"] = nbr_choices - 1;
            data.Property("c" + ((PictureBox)sender).Tag).Remove();
            Tools.Set_To_JSON(this.file_path, this.data);

            // Delete the panel linked to the choice.
            this.prev_line_loc -= ((PictureBox)sender).Parent.Height + 10;
            this.Controls.Remove(((PictureBox)sender).Parent);
            this.Height = this.prev_line_loc;
        }

        private void QuizzEdition_Resize(object sender, EventArgs e)
        {
            txt_question.Width = Tools.Min_Int(Tools.Get_Text_Width(this, txt_question.Text, 20) + 12,
                                    this.Width - 10 - 30 - 8 - 10 - 8 - 30 - 10);
            pb_add.Location = new Point(txt_question.Location.X + txt_question.Width + 8, 10);
            pb_delete_all.Location = new Point(this.Width - pb_delete_all.Width - 10, 10);
            this.Height = this.prev_line_loc;

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
                txt_answer.Location = new Point(10, 10);
                nud_score.Location = new Point(txt_answer.Location.X + txt_answer.Width + 8, 10);
                cbo_redirect.Location = new Point(nud_score.Location.X + nud_score.Width + 8, 10);
                pb_discard_choice.Location = new Point(cbo_redirect.Location.X + cbo_redirect.Width + 8, 10);
            }
        }

        public void Delete_All(object sender, EventArgs e)
        {
            ((Editor)this.ParentForm).Discard_Dialog(this);
        }
    }
}
