using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class EventEdition : UserControl
    {
        // Attributes.

        private string file_path;
        private int event_id;
        private JObject file_data;
        private JObject theme;
        private bool loading;

        // Methods.

        public EventEdition(Editor editor, string file_path, int event_id)
        {
            InitializeComponent();
            this.file_path = file_path;
            this.event_id = event_id;
            this.file_data = Tools.Get_From_JSON(file_path);
            this.theme = editor.Get_Theme();
            this.DoubleBuffered = true;
        }

        public string Get_File_Path()
        {
            return this.file_path;
        }

        public int Get_Event_Id()
        {
            return this.event_id;
        }

        public void Set_File_Path(string new_file_path)
        {
            this.file_path = new_file_path;
        }

        private void EventEdition_Load(object sender, EventArgs e)
        {
            List<string> cbo_npcs_list = new List<string>();
            List<string> cbo_dialogs_list = new List<string>();
            DirectoryInfo game_dir = Directory.GetParent(this.file_path).Parent.Parent.Parent;
            DirectoryInfo npcs_dir = new DirectoryInfo(game_dir.FullName + Path.DirectorySeparatorChar + "library" +
                                                      Path.DirectorySeparatorChar + "npcs");
            DirectoryInfo dialogs_dir = new DirectoryInfo(game_dir.FullName + Path.DirectorySeparatorChar + "library" +
                                                      Path.DirectorySeparatorChar + "dialogs");
            JObject temp_data;
            int nbr_files = npcs_dir.GetFiles().Length;
            foreach (FileInfo npc in npcs_dir.GetFiles())
            {
                temp_data = Tools.Get_From_JSON(npc.FullName);
                if (nbr_files < 10)
                {
                    cbo_npcs_list.Add("[" + npc.Name.Split('.')[0] + "] > " + temp_data["name"]);
                }
                else if (nbr_files < 100)
                {
                    if (npc.Name.Split('.')[0].Length == 1)
                    {
                        cbo_npcs_list.Add("[0" + npc.Name.Split('.')[0] + "] > " + temp_data["name"]);
                    }
                    else
                    {
                        cbo_npcs_list.Add("[" + npc.Name.Split('.')[0] + "] > " + temp_data["name"]);
                    }
                }
                else
                {
                    if (npc.Name.Split('.')[0].Length == 1)
                    {
                        cbo_npcs_list.Add("[00" + npc.Name.Split('.')[0] + "] > " + temp_data["name"]);
                    }
                    else if (npc.Name.Split('.')[0].Length == 2)
                    {
                        cbo_npcs_list.Add("[0" + npc.Name.Split('.')[0] + "] > " + temp_data["name"]);
                    }
                    else
                    {
                        cbo_npcs_list.Add("[" + npc.Name.Split('.')[0] + "] > " + temp_data["name"]);
                    }
                }
            }
            cbo_npcs_list.Sort();

            nbr_files = dialogs_dir.GetFiles().Length;
            foreach (FileInfo dialog in dialogs_dir.GetFiles())
            {
                temp_data = Tools.Get_From_JSON(dialog.FullName);
                if (nbr_files < 10)
                {
                    cbo_dialogs_list.Add("[" + dialog.Name.Split('.')[0] + "] > " + temp_data["question"]);
                }
                else if (nbr_files < 100)
                {
                    if (dialog.Name.Split('.')[0].Length == 1)
                    {
                        cbo_dialogs_list.Add("[0" + dialog.Name.Split('.')[0] + "] > " + temp_data["question"]);
                    }
                    else
                    {
                        cbo_dialogs_list.Add("[" + dialog.Name.Split('.')[0] + "] > " + temp_data["question"]);
                    }
                }
                else
                {
                    if (dialog.Name.Split('.')[0].Length == 1)
                    {
                        cbo_dialogs_list.Add("[00" + dialog.Name.Split('.')[0] + "] > " + temp_data["question"]);
                    }
                    else if (dialog.Name.Split('.')[0].Length == 2)
                    {
                        cbo_dialogs_list.Add("[0" + dialog.Name.Split('.')[0] + "] > " + temp_data["question"]);
                    }
                    else
                    {
                        cbo_dialogs_list.Add("[" + dialog.Name.Split('.')[0] + "] > " + temp_data["question"]);
                    }
                }
            }
            cbo_dialogs_list.Sort();

            this.loading = true;
            cbo_npcs.DataSource = cbo_npcs_list;
            cbo_dialogs.DataSource = cbo_dialogs_list;

            cbo_npcs.SelectedIndex = int.Parse((string)this.file_data[this.event_id.ToString()]["npc"]["id"]) - 1;
            cbo_dialogs.SelectedIndex = int.Parse((string)this.file_data[this.event_id.ToString()]["quizz"]) - 1;
            this.loading = false;

            // Setup the theme to match the one of the editor.
            this.BackColor = Color.FromArgb(int.Parse((string)this.theme["2"]["R"]), int.Parse((string)this.theme["2"]["G"]), int.Parse((string)this.theme["2"]["B"]));
            this.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            cbo_dialogs.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
            cbo_dialogs.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            cbo_npcs.BackColor = Color.FromArgb(int.Parse((string)this.theme["4"]["R"]), int.Parse((string)this.theme["4"]["G"]), int.Parse((string)this.theme["4"]["B"]));
            cbo_npcs.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));

            btn_placement.BackColor = Color.FromArgb(int.Parse((string)this.theme["0"]["R"]), int.Parse((string)this.theme["0"]["G"]), int.Parse((string)this.theme["0"]["B"]));
            btn_placement.ForeColor = Color.FromArgb(int.Parse((string)this.theme["5"]["R"]), int.Parse((string)this.theme["5"]["G"]), int.Parse((string)this.theme["5"]["B"]));
        }

        private void EventEdition_SizeChanged(object sender, EventArgs e)
        {
            // Sizing.
            int size_left = this.Width - 15 - btn_placement.Width - 10 - 10 - 10 - pb_delete.Width - 15;
            cbo_dialogs.Width = size_left / 2;
            cbo_npcs.Width = size_left / 2;

            // Placing.
            cbo_dialogs.Location = new Point(cbo_npcs.Location.X + cbo_npcs.Width + 10, 14);
            pb_delete.Location = new Point(this.Width - pb_delete.Width - 15, 15);
        }

        private void Cbo_npcs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.loading) { return; }
            DirectoryInfo game_dir = Directory.GetParent(this.file_path).Parent.Parent.Parent;
            DirectoryInfo npcs_dir = new DirectoryInfo(game_dir.FullName + Path.DirectorySeparatorChar + "library" +
                                                      Path.DirectorySeparatorChar + "npcs");
            ComboBoxFix cbo = (ComboBoxFix)sender;
            int file_id = cbo.SelectedIndex + 1;
            Console.WriteLine("Access: " + npcs_dir.FullName + Path.DirectorySeparatorChar + file_id + ".json" + "\nChoice: " + this.event_id + "\n");
            this.file_data[event_id.ToString()]["npc"] = Tools.Get_From_JSON(npcs_dir.FullName + Path.DirectorySeparatorChar +
                                                                        file_id + ".json");
            this.file_data[event_id.ToString()]["npc"]["id"] = file_id;
            Tools.Set_To_JSON(this.file_path, this.file_data);
        }

        private void Cbo_dialogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.loading) { return; }
            ComboBoxFix cbo = (ComboBoxFix)sender;
            this.file_data[event_id.ToString()]["quizz"] = cbo.SelectedIndex + 1;
            Tools.Set_To_JSON(this.file_path, this.file_data);
        }

        /// <summary>
        /// Asks to the Editor to delete both this EventEdition and his linked event in the JSON file.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        public void Discard(object sender, EventArgs e)
        {
            ((Editor)this.ParentForm).Discard_Event(this);
        }

        private void Btn_placement_Click(object sender, EventArgs e)
        {
            ((Editor)this.ParentForm).Place_Event(this);
        }
    }
}
