using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Learn_CTS
{
    public partial class IncrementEdition : UserControl
    {

        // Attributes.

        private Editor editor;
        private string file_path;
        private string id;
        private JObject theme;

        // Methods.

        public IncrementEdition(Editor editor, string file_path, string increment_id)
        {
            InitializeComponent();
            string file_name = file_path.Split(Path.DirectorySeparatorChar).Last();
            this.editor = editor;
            this.id = increment_id;
            this.file_path = file_path;
            this.theme = editor.Get_Theme();
            this.DoubleBuffered = true;
        }

        public string Get_File_Path()
        {
            return this.file_path;
        }
        
        public void Set_File_Path(string new_path)
        {
            this.file_path = new_path;
        }

        public int Get_Id()
        {
            return int.Parse(this.id);
        }

        private void IncrementEdition_Load(object sender, EventArgs e)
        {
            JObject data = Tools.Get_From_JSON(this.file_path);

            nud_score.Value = int.Parse(data[id.ToString()]["score"].ToString());
            txt_message.Text = data[id.ToString()]["comment"].ToString();

            editor.Set_Saved(true);

            this.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            nud_score.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
            nud_score.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            txt_message.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
            txt_message.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
        }

        private void IncrementEdition_SizeChanged(object sender, EventArgs e)
        {
            pb_delete.Location = new Point(this.Width - pb_delete.Width - 10, pb_delete.Location.Y);
            txt_message.Width = this.Width - lbl_message.Width - 20;
        }

        private void Pb_delete_Click(object sender, EventArgs e)
        {
            this.editor.Discard_Increment(this);
        }

        private void Txt_message_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            JObject data = Tools.Get_From_JSON(this.file_path);
            List<char> autorized_chars = new List<char>() { ' ', '.', ',', '\'', '?', '!', '-', '°', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                ((JObject)data[id])["comment"] = t.Text.Trim();
                Tools.Set_To_JSON(this.file_path, data); // Set the entered setting as a stored blueprint for npc.
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)((JObject)data[id])["comment"];
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

        private void Nud_score_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDownFix nud = (NumericUpDownFix)sender;
            nud.BackColor = Color.FromArgb(56, 32, 32);
            ((Editor)this.ParentForm).Set_Saved(false);
        }

        private void Nud_score_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumericUpDownFix t = (NumericUpDownFix)sender;
            JObject data = Tools.Get_From_JSON(this.file_path);
            List<char> autorized_chars = new List<char>() { ' ', ',', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                ((JObject)data[id])["score"] = int.Parse(t.Text.Trim());
                Tools.Set_To_JSON(this.file_path, data);
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                ((Editor)this.ParentForm).Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                t.Text = (string)((JObject)data[id])["score"];
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
    }
}
