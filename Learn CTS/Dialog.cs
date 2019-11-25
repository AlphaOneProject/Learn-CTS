using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;

namespace Learn_CTS
{
    public partial class Dialog : UserControl
    {
        private NPC_Manager nm = NPC_Manager.GetInstance();
        NPC npc;
        private string game_path;
        private JObject data;
        SpeechSynthesizer s;
        private int audio;
        private Thread t_audio;
        private System.Windows.Forms.Timer timer_text;
        private string question = "";

        public Dialog(int id, string game)
        {
            InitializeComponent();
            npc = nm.GetNPCByID(id);
            InitializeGamePath(game);
            this.DoubleBuffered = true;
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            t_audio = new Thread(new ThreadStart(Listen));
            InitializeTimerDisplayText();
            this.Set_Up(npc.GetQuiz().ToString());
            Generate_Buttons_Choices();
            this.Focus();
            npc.RemoveInteraction();
            lbl_name.Text = npc.GetName();
            this.pbox_audio.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "speaker.png");
            this.Location = new Point(npc.GetX() + npc.GetWidth() / 2 - this.Width / 2, npc.GetY() - this.Height - 50);
            if(audio != 2) timer_text.Start();
        }

        private void InitializeGamePath(string game)
        {
            this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "dialogs" + Path.DirectorySeparatorChar;
        }

        private void Set_Up(string q)
        {
            data = Tools.Get_From_JSON(this.game_path + q + ".json");
            audio = (int)this.data["audio"];
            question = this.data["question"].ToString().Replace("<Nom>", Player.GetInstance().GetName());
            if (audio == 2)
            {
                txt_dialog_npc.Text = "";
                t_audio.Start();
            }
        }

        private void InitializeTimerDisplayText()
        {
            timer_text = new System.Windows.Forms.Timer();
            timer_text.Tick += new EventHandler(Question_Tick);
            timer_text.Interval = 50;
        }

        private void Question_Tick(object sender, EventArgs e)
        {
            if(txt_dialog_npc.Text.Length < question.Length)
            {
                txt_dialog_npc.Text += question.Substring(txt_dialog_npc.Text.Length, 1);
            }
            else
            {
                timer_text.Stop();
            }
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
            btn_leave.TabIndex = 0;
            btn_leave.Text = "Partir";
            btn_leave.UseVisualStyleBackColor = true;
            btn_leave.Click += new System.EventHandler(this.Dialog_Closed);
            return btn_leave;
        }

        private void Listen()
        {
            s = new SpeechSynthesizer();
            s.SetOutputToDefaultAudioDevice();
            if (s.State.ToString() == "Ready")
            {
                s.Volume = 100;
                s.Rate = -1;
                s.Speak(question);
                int nbr_choices = (int)this.data["choices"];
                string se;
                for (int i = 1; i <= nbr_choices; i++)
                {
                    if (i == 1) se = "er";
                    else if (i == 2) se = "nd";
                    else se = "ème";
                    s.Speak(i + se + " choix "+ this.data["c" + i.ToString()]["answer"].ToString());
                }
            }
            t_audio.Abort();
        }

        public void Answer_Event(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string s = this.data["c" + btn.TabIndex.ToString()]["redirect"].ToString();
            int score = (int)this.data["c" + btn.TabIndex.ToString()]["score"];
            ((GameWindow)this.FindForm()).SetScore(score);
            if (s.Length == 0 || s == "0")
            {
                ((GameWindow)this.FindForm()).SwitchSituation();
            }
            else if(s == "-1")
            {
                this.Dialog_Closed(sender, e);
            }
            else
            {
                this.Set_Up(s);
                Generate_Buttons_Choices();
                this.Location = new Point(npc.GetX() + npc.GetWidth() / 2 - this.Width / 2, npc.GetY() - this.Height - 50);
                timer_text.Start();
            }
        }

        public void Dialog_Closed(object sender, EventArgs e)
        {
            if (npc.GetQuiz() > 0) npc.DisplayInteraction();
            if (t_audio != null && t_audio.IsAlive) t_audio.Abort();
            ((GameWindow)this.FindForm()).RemoveDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (audio == 0)
            {
                MessageBox.Show("Le son est désactivé pour ce dialogue.");
            }
            else
            {
                t_audio.Start();
            }
        }

        private void Dialog_Resize(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            pbox_audio.Location = new Point(c.Width - pbox_audio.Width - 10, 10);
        }
    }
}
