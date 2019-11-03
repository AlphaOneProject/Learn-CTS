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
    public partial class Dialog : UserControl
    {
        private NPC_Manager nm = NPC_Manager.GetInstance();
        private int id;
        private string game_path;
        private JObject data;

        public Dialog(int id, string game)
        {
            InitializeComponent();
            this.id = id;
            InitializeGamePath(game);
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            this.SetUp();
            pbox_head_npc.Image = GetHeadNPC(nm.GetNPCByID(id).GetImage());
            lbl_name.Text = nm.GetNPCByID(id).GetName();
            txt_dialog_npc.Text = this.data["question"].ToString();
            int nbr_choices = (int)this.data["choices"];
            for(int i = 1; i<=nbr_choices; i++)
            {
                Button btn = new Button();
                btn.Name = "btn";
                btn.Size = new System.Drawing.Size(92, 23);
                btn.Location = new System.Drawing.Point(this.pbox_head_npc.Location.X + this.pbox_head_npc.Width + 20 + (2*btn.Width*i), this.txt_dialog_npc.Location.Y + this.txt_dialog_npc.Height + 20);
                btn.TabIndex = 2;
                btn.Text = this.data["c" + i.ToString()]["answer"].ToString();
                btn.UseVisualStyleBackColor = true;
                //btn.Click += new System.EventHandler(this.button1_Click);
                this.Controls.Add(btn);
            }
        }

        private Image GetHeadNPC(Image img)
        {
            Bitmap bmpImage = new Bitmap(img);
            Rectangle rect = new Rectangle((img.Width-pbox_head_npc.Width)/2,0,pbox_head_npc.Width, pbox_head_npc.Height);
            Bitmap bmpCrop = bmpImage.Clone(rect, bmpImage.PixelFormat);
            return (Image)bmpCrop;
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

        private void SetUp()
        {
            JObject dialog = Get_From_JSON("quizzes.json");
            this.data = (JObject)dialog[nm.GetNPCByID(this.id).GetQuiz().ToString()];
        }

        public void Dialog_Closed(object sender, EventArgs e)
        {
            ((GameWindow)this.FindForm()).RemoveDialog();
        }
    }
}
