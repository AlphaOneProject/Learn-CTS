using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Learn_CTS
{
    public partial class Dialog : UserControl
    {
        private NPC_Manager nm = NPC_Manager.GetInstance();
        NPC npc;
        private string game_path;
        private JObject data;

        public Dialog(int id, string game)
        {
            InitializeComponent();
            npc = nm.GetNPCByID(id);
            InitializeGamePath(game);
            this.DoubleBuffered = true;
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            this.Focus();
            npc.RemoveInteraction();
            this.Set_Up(npc.GetQuiz().ToString());
            lbl_name.Text = npc.GetName();
            Generate_Buttons_Choices();
            //this.Location = new Point(npc.GetX() + npc.GetWidth() / 2 - this.Width / 2, npc.GetY() - this.Height - 50);
        }

        private void InitializeGamePath(string game)
        {
            this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar;
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

        private void Set_Up(string q)
        {
            JObject dialog = Get_From_JSON("quizzes.json");
            this.data = (JObject)dialog[q];
            txt_dialog_npc.Text = this.data["question"].ToString();
        }

        private void Generate_Buttons_Choices()
        {
            flp_choices.Controls.Clear();
            int nbr_choices = (int)this.data["choices"];
            for (int i = 1; i <= nbr_choices; i++)
            {
                Button btn = new Button();
                btn.Name = "btn_choice";
                btn.AutoSize = true;
                btn.Location = new Point(this.txt_dialog_npc.Location.X + 20 + (2 * btn.Width * i), this.txt_dialog_npc.Location.Y + this.txt_dialog_npc.Height + 20);
                btn.TabIndex = i;
                btn.Cursor = System.Windows.Forms.Cursors.Hand;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.Text = this.data["c" + i.ToString()]["answer"].ToString();
                btn.UseVisualStyleBackColor = true;
                btn.Click += new System.EventHandler(this.Answer_Event);
                flp_choices.Controls.Add(btn);
            }
            flp_choices.Controls.Add(Generate_Button_Leave());
        }

        private Button Generate_Button_Leave()
        {
            Button btn_leave = new Button();
            btn_leave.Cursor = System.Windows.Forms.Cursors.Hand;
            btn_leave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_leave.Location = new System.Drawing.Point(243, 6);
            btn_leave.Name = "btn_leave";
            btn_leave.AutoSize = true;
            btn_leave.TabIndex = 5;
            btn_leave.Text = "Partir";
            btn_leave.UseVisualStyleBackColor = true;
            btn_leave.Click += new System.EventHandler(this.Dialog_Closed);
            return btn_leave;
        }

        public void Answer_Event(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string s = this.data["c" + btn.TabIndex.ToString()]["redirect"].ToString();
            if(s.Length == 0)
            {
                this.Dialog_Closed(sender,e);
            }
            else
            {
                this.Set_Up(s);
                Generate_Buttons_Choices();
                //this.Location = new Point(npc.GetX() + npc.GetWidth() / 2 - this.Width / 2, npc.GetY() - this.Height - 50);
            }
        }

        public void Dialog_Closed(object sender, EventArgs e)
        {
            npc.DisplayInteraction();
            ((GameWindow)this.FindForm()).RemoveDialog();
        }
    }
}
