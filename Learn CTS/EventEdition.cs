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

        private Editor editor;
        private string file_path;
        private int event_id;
        private JObject theme;
        private string type;
        private bool loading;

        // Methods.

        /// <summary>
        /// Constructor of the UserControl, setup the necessary arguments as parameters.
        /// </summary>
        /// <param name="editor">Parent instance of Editor.</param>
        /// <param name="file_path">Path of the "dialogs.json" file of the situation.</param>
        /// <param name="event_id">ID of the event to manage inside the file.</param>
        public EventEdition(Editor editor, string file_path, int event_id)
        {
            InitializeComponent();

            this.editor = editor;
            this.file_path = file_path;
            this.event_id = event_id;
            this.theme = editor.Get_Theme();
            this.type = file_path.Split(Path.DirectorySeparatorChar).Last().Split('.')[0];
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Accessor to the file path.
        /// </summary>
        /// <returns>File full path.</returns>
        public string Get_File_Path()
        {
            return this.file_path;
        }

        /// <summary>
        /// Accessor to the ID of the event managed.
        /// </summary>
        /// <returns>ID of the event.</returns>
        public int Get_Event_Id()
        {
            return this.event_id;
        }

        /// <summary>
        /// Accessor to the type of the event managed.
        /// </summary>
        /// <returns>Type of the event.</returns>
        public string Get_Type()
        {
            return this.type;
        }

        /// <summary>
        /// Allow access from the Editor to the path of the aimed file (modified after renaming the situation).
        /// </summary>
        /// <param name="new_file_path">Path of the "dialogs.json" file of the situation.</param>
        public void Set_File_Path(string new_file_path)
        {
            this.file_path = new_file_path;
        }

        /// <summary>
        /// Load all NPCs and Dialogs into the ComboBoxFix which will allow their selection.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void EventEdition_Load(object sender, EventArgs e)
        {
            List<string> cbo_npcs_list = new List<string>();
            List<string> cbo_dialogs_list = new List<string>();
            DirectoryInfo game_dir = Directory.GetParent(this.file_path).Parent.Parent.Parent;
            DirectoryInfo npcs_dir = new DirectoryInfo(game_dir.FullName + Path.DirectorySeparatorChar + "library" +
                                                      Path.DirectorySeparatorChar + "npcs");
            DirectoryInfo dialogs_dir = new DirectoryInfo(game_dir.FullName + Path.DirectorySeparatorChar + "library" +
                                                      Path.DirectorySeparatorChar + "dialogs");
            DirectoryInfo items_dir = new DirectoryInfo(game_dir.FullName + Path.DirectorySeparatorChar + "library" +
                                                      Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar +
                                                      "items");
            JObject file_data = Tools.Get_From_JSON(file_path);
            JObject temp_data;
            DirectoryInfo choosen_dir;
            int nbr_files;

            switch (this.type)
            {
                case "dialogs":
                    choosen_dir = npcs_dir;
                    break;
                case "items":
                    choosen_dir = items_dir;
                    break;
                default:
                    throw new ArgumentException("Invalid argument in EventEdition!");
            }

            int i = 1;
            string displayed_name = "";
            nbr_files = choosen_dir.GetFiles().Length;
            foreach (FileInfo file in choosen_dir.GetFiles())
            {
                switch (this.type)
                {
                    case "dialogs":
                        displayed_name = Tools.Get_From_JSON(file.FullName)["name"].ToString();
                        break;
                    case "items":
                        displayed_name = file.Name.Split('.')[0];
                        break;
                    default:
                        throw new ArgumentException("Invalid argument in EventEdition!");
                }

                
                if (nbr_files < 10)
                {
                    cbo_npcs_list.Add("[" + i.ToString() + "] > " + displayed_name);
                }
                else if (nbr_files < 100)
                {
                    if (i.ToString().Length == 1)
                    {
                        cbo_npcs_list.Add("[0" + i.ToString() + "] > " + displayed_name);
                    }
                    else
                    {
                        cbo_npcs_list.Add("[" + i.ToString() + "] > " + displayed_name);
                    }
                }
                else
                {
                    if (i.ToString().Length == 1)
                    {
                        cbo_npcs_list.Add("[00" + i.ToString() + "] > " + displayed_name);
                    }
                    else if (i.ToString().Length == 2)
                    {
                        cbo_npcs_list.Add("[0" + i.ToString() + "] > " + displayed_name);
                    }
                    else
                    {
                        cbo_npcs_list.Add("[" + i.ToString() + "] > " + displayed_name);
                    }
                }
                i++;
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
            switch (this.type)
            {
                case "dialogs":
                    cbo_npcs.SelectedIndex = int.Parse(file_data[this.event_id.ToString()]["npc"]["id"].ToString()) - 1;
                    break;
                case "items":
                    cbo_npcs.SelectedIndex = int.Parse(file_data[this.event_id.ToString()]["item"]["id"].ToString()) - 1;
                    break;
                default:
                    throw new ArgumentException("Invalid argument in EventEdition!");
            }
            
            cbo_dialogs.SelectedIndex = int.Parse((string)file_data[this.event_id.ToString()]["quizz"]) - 1;
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

        /// <summary>
        /// Placement and sizing upon resize (from the editor).
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
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

        /// <summary>
        /// Saves the new selected NPC into the event.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Cbo_npcs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editor.Get_Is_Loading() || this.loading) { return; }
            editor.Set_Is_Loading(true);

            ComboBoxFix cbo = (ComboBoxFix)sender;
            JObject file_data = Tools.Get_From_JSON(this.file_path);
            int file_id = cbo.SelectedIndex + 1;
            switch (this.type)
            {
                case "dialogs":
                    file_data[event_id.ToString()]["npc"]["id"] = file_id;
                    break;
                case "items":
                    file_data[event_id.ToString()]["item"]["name"] = cbo.SelectedItem.ToString().Split('>')[1].Substring(1);
                    file_data[event_id.ToString()]["item"]["id"] = file_id;
                    break;
                default:
                    throw new ArgumentException("Invalid argument in EventEdition!");
            }
            Tools.Set_To_JSON(this.file_path, file_data);
            editor.Set_Is_Loading(false);
        }

        /// <summary>
        /// Saves the new selected dialog into the event.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Cbo_dialogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editor.Get_Is_Loading() || this.loading) { return; }
            editor.Set_Is_Loading(true);

            ComboBoxFix cbo = (ComboBoxFix)sender;
            JObject file_data = Tools.Get_From_JSON(this.file_path);
            file_data[event_id.ToString()]["quizz"] = cbo.SelectedIndex + 1;
            Tools.Set_To_JSON(this.file_path, file_data);
            editor.Set_Is_Loading(false);
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

        /// <summary>
        /// Sends a placement request to the Editor.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Btn_placement_Click(object sender, EventArgs e)
        {
            ((Editor)this.ParentForm).Place_Event(this);
        }
    }
}
