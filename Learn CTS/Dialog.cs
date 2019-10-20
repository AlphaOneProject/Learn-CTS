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

        public Dialog(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            pbox_head_npc.Image = nm.GetNPCByID(id).GetImage();
            txt_dialog_npc.Text = "Bonjour ! Pourriez-vous me dire à quelle station dois-je m'arrêter pour aller à la place Kléber ?";
            //int nbr_choices = 4;
        }

        /// <summary>
        /// Recover the content of a JSON file at a specified path.
        /// </summary>
        /// <param name="internal_path">Path from the game folder to the targeted JSON file.</param>
        /// <returns>Content of the JSON file under a JObject structure.</returns>
        /*public JObject Get_From_JSON(string internal_path)
        {
            JObject output;
            using (StreamReader stream_r = new StreamReader(@"" + this.game_path + internal_path))
            {
                string json_file = stream_r.ReadToEnd();
                output = JObject.Parse(json_file);
            }
            return output;
        }*/

        private void button4_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_dialog_npc.Text = "Merci beaucoup ! Je vous souhaite une bonne journée !";
            button1.Visible = false;
            button2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txt_dialog_npc.Text = "Vous êtes sûr ? Très bien merci, je vous souhaite une bonne journée !";
            button1.Visible = false;
            button2.Visible = false;
        }
    }
}
