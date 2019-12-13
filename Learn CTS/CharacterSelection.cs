using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Learn_CTS
{
    public partial class CharacterSelection : UserControl
    {

        // Attributes.

        private Editor editor;
        private JObject theme;
        private string npc_path;
        private int id;
        private int cursor = 0;
        private List<Image> valid_sprites = new List<Image>();
        private List<string> valid_folders = new List<string>();

        // Methods.

        public CharacterSelection(Editor editor, string npc_path)
        {
            InitializeComponent();
            this.editor = editor;
            this.theme = editor.Get_Theme();
            this.npc_path = npc_path;
            this.id = int.Parse(npc_path.Split(Path.DirectorySeparatorChar).Last().Split('.')[0]);
        }

        private void CharacterSelection_Load(object sender, EventArgs e)
        {
            JObject theme = editor.Get_Theme();

            this.BackColor = Color.FromArgb(int.Parse((string)theme["2"]["R"]), int.Parse((string)theme["2"]["G"]), int.Parse((string)theme["2"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)theme["5"]["R"]), int.Parse((string)theme["5"]["G"]), int.Parse((string)theme["5"]["B"]));

            txt_name.BackColor = Color.FromArgb(int.Parse((string)theme["4"]["R"]), int.Parse((string)theme["4"]["G"]), int.Parse((string)theme["4"]["B"]));
            txt_name.ForeColor = Color.FromArgb(int.Parse((string)theme["5"]["R"]), int.Parse((string)theme["5"]["G"]), int.Parse((string)theme["5"]["B"]));

            string characters_images = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "library" +
                                       Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                                       "characters";
            foreach (string sprite_dir in Directory.GetDirectories(characters_images))
            {
                if (Tools.Is_Valid(sprite_dir))
                {
                    valid_folders.Add(sprite_dir);
                    valid_sprites.Add(Tools.Image_From_File(sprite_dir + Path.DirectorySeparatorChar + "1_0.png"));
                }
            }

            JObject data = Tools.Get_From_JSON(npc_path);
            string folder = valid_folders.Find(x => x == data["folder"].ToString());
            cursor = valid_folders.IndexOf(folder);

            txt_name.Text = data["name"].ToString();
            txt_name.TextAlign = HorizontalAlignment.Center;
            txt_name.KeyPress += new KeyPressEventHandler(TextBox1_TextChanged);

            Load_Images();
        }

        private void Load_Images()
        {
            pb_1.Image = valid_sprites[(((cursor - 1) % valid_sprites.Count) + valid_sprites.Count) % valid_sprites.Count];
            pb_2.Image = valid_sprites[((cursor % valid_sprites.Count) + valid_sprites.Count) % valid_sprites.Count];
            pb_3.Image = valid_sprites[(((cursor + 1) % valid_sprites.Count) + valid_sprites.Count) % valid_sprites.Count];

            JObject data = Tools.Get_From_JSON(npc_path);
            data["folder"] = valid_folders[((cursor % valid_sprites.Count) + valid_sprites.Count) % valid_sprites.Count].Split(Path.DirectorySeparatorChar).Last();
            Tools.Set_To_JSON(npc_path, data);
        }

        private void Pb_left_Click(object sender, EventArgs e)
        {
            this.cursor--;
            Load_Images();
        }

        private void Pb_right_Click(object sender, EventArgs e)
        {
            this.cursor++;
            Load_Images();
        }

        private void TextBox1_TextChanged(object sender, KeyPressEventArgs e)
        {
            TextBox t = (TextBox)sender;
            List<char> autorized_chars = new List<char>() { ' ', '?', '!', '-', '(', ')', ':' };
            if (e.KeyChar == (char)13) // (char)13 => Enter.
            {
                string txt_path = Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                                  "npcs" + Path.DirectorySeparatorChar + id + ".json";
                JObject data_npc = Tools.Get_From_JSON(editor.Get_Game_Path() + txt_path);
                data_npc["name"] = t.Text;
                Tools.Set_To_JSON(editor.Get_Game_Path() + txt_path, data_npc); // Set the entered setting as a stored blueprint for npc.
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                editor.Set_Saved(true);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)27) // (char)27 => Escape.
            {
                string txt_path = Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar +
                                  "npcs" + Path.DirectorySeparatorChar + id + ".json";
                JObject data_npc = Tools.Get_From_JSON(editor.Get_Game_Path() + txt_path);
                t.Text = (string)data_npc["name"];
                t.SelectionStart = t.Text.Length;
                t.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
                editor.Set_Saved(true);
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
                    t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                    editor.Set_Saved(false);
                    return; // Let you erase regardless of the length.
                }
                e.Handled = true;
            }
            else
            {
                t.BackColor = Color.FromArgb(255, (int)(int.Parse((string)this.theme["4"]["G"]) * 0.4), (int)(int.Parse((string)this.theme["4"]["B"]) * 0.4));
                editor.Set_Saved(false);
            }
        }

        private void Delete_This(object sender, EventArgs e)
        {
            // Prevent the user from suppressing the last NPC.
            string npcs_folder_path = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "library" +
                                                        Path.DirectorySeparatorChar + "npcs";
            if (Directory.GetFiles(npcs_folder_path).Count() <= 1)
            {
                MessageBox.Show("Vous ne pouvez pas supprimer l'intégralité des personnages non-joueurs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Checks if it is used in a situation.
            string scenarios_path = @"" + editor.Get_Game_Path() + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            JObject data;
            foreach (string scenario in Directory.GetDirectories(scenarios_path))
            {
                foreach (string situation in Directory.GetDirectories(scenario))
                {
                    data = Tools.Get_From_JSON(situation + Path.DirectorySeparatorChar + "dialogs.json");
                    for (int i = 1; i <= int.Parse(data["events"].ToString()); i++)
                    {
                        if (int.Parse(data[i.ToString()]["npc"]["id"].ToString()) == this.id)
                        {
                            MessageBox.Show("Ce figurant est utilisé dans une ou plusieurs situations.\n" +
                                            "Remplacez-le dans ces situations puis réessayez.\n\n" +
                                            "Situation en faisant usage : " +
                                            situation.Split(Path.DirectorySeparatorChar).Last().Split('.').Last(),
                                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Removing the involved file.
            File.Delete(npcs_folder_path + Path.DirectorySeparatorChar + this.id + ".json");

            // Reordering the others files.
            for (int i = this.id; i <= Directory.GetFiles(@"" + npcs_folder_path).Length; i++)
            {
                if (this.id <= i)
                {
                    Directory.Move(@"" + npcs_folder_path + Path.DirectorySeparatorChar + (i + 1) + ".json",
                                   @"" + npcs_folder_path + Path.DirectorySeparatorChar + i + ".json");
                }
            }

            this.Dispose();
        }
    }
}
