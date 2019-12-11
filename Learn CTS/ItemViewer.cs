using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;

namespace Learn_CTS
{
    /// <summary>
    /// Control used to view the question and actions choices for a specific item in the point and click game mode,
    /// Allows to interact with the item.
    /// </summary>
    partial class ItemViewer : UserControl
    {
        private Item item;
        private ItemManager manager;
        SpeechSynthesizer s;
        private Thread t_audio;
        private JObject actions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item_id">ID of the item viewed.</param>
        /// <param name="manager">Manager of the situation, which contains the item.</param>
        public ItemViewer(int item_id, ItemManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.item = manager.GetItemByID(item_id);
        }

        /// <summary>
        /// Called when the ItemViewer is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemViewer_Load(object sender, EventArgs e)
        {
            // Properties of the item image picturebox
            pb_item.BackgroundImage = item.GetImage();

            // Properties of the description label
            lbl_desc.Text = item.GetDescription();
            lbl_desc.BackColor = Color.Transparent;
            lbl_desc.Size = new Size(this.Width - pb_item.Width - 6, this.Height - 6);

            // Properties of the hide button
            btn_exit.Location = new Point(this.Width - btn_exit.Width - 24 , this.Height - 6 - btn_exit.Height);
            pb_audio.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "speaker.png");

            this.t_audio = new Thread(new ThreadStart(Listen));
            this.actions = item.GetActions();
            DisplayActions();
        }

        /// <summary>
        /// Hides the form when the exit button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Called when the size of the window changes.
        /// Is used for responsive behavior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemViewer_ClientSizeChanged(object sender, EventArgs e)
        {
            lbl_desc.Width = this.Width - pb_item.Width - 6;
            btn_exit.Location = new Point(this.Width - btn_exit.Width - 24, this.Height - 6 - btn_exit.Height);
            pb_audio.Location = new Point(this.Width - pb_audio.Width - 24, 6);
        }

        /// <summary>
        /// Displays one button for each actions possible with the item.
        /// </summary>
        private void DisplayActions()
        {
            flp_actions.Controls.Clear();
            int nbr_choices = (int)actions["choices"];
            for (int i = 1; i <= nbr_choices; i++)
            {
                Button btn = new Button();
                btn.Name = "btn_action";
                btn.AutoSize = true;
                btn.TabIndex = i;
                btn.Cursor = Cursors.Hand;
                btn.Text = actions["c" + i.ToString()]["answer"].ToString();
                btn.UseVisualStyleBackColor = true;
                btn.Click += new System.EventHandler(this.Action_Event);
                flp_actions.Controls.Add(btn);
            }
        }

        private JObject GetActionsFromItem()
        {
            JObject actions = item.GetActions();
            this.actions = actions;
            return actions;
        }

        /// <summary>
        /// Called when a button defining an action of the item is clicked.
        /// Processes the action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Action_Event(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string s = item.GetActions()["c" + btn.TabIndex.ToString()]["redirect"].ToString();
            int score = (int)item.GetActions()["c" + btn.TabIndex.ToString()]["score"];
            GameWindow.GetInstance().SetScore(score);
            if (s.Length == 0 || s == "0")
            {
                PNCWindow window = ((PNCWindow)this.FindForm());
                window.setItemsLeft(window.getItemsLeft() - 1);
            }
            else if (s == "-1")
            {
                Exit();
            }
            else
            {
                this.actions = Tools.Get_From_JSON(manager.GetLibraryPath() + Path.DirectorySeparatorChar +
                    "dialogs" + Path.DirectorySeparatorChar + s + ".json");
                DisplayActions();
            }
        }

        /// <summary>
        /// Called when the audiodescription PictureBox is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_audio_Click(object sender, EventArgs e)
        {
            t_audio.Start();
        }

        /// <summary>
        /// Audiodescription for the viewed items quesion and choices of actions.
        /// </summary>
        private void Listen()
        {
            JObject actions = item.GetActions();
            int nbr_choices = (int)actions["choices"];
            s = new SpeechSynthesizer();
            s.SetOutputToDefaultAudioDevice();
            if (s.State.ToString() == "Ready")
            {
                s.Volume = 100;
                s.Rate = -1;
                s.Speak(actions["question"].ToString());
                string se;
                for (int i = 1; i <= nbr_choices; i++)
                {
                    if (i == 1) se = "er";
                    else if (i == 2) se = "nd";
                    else se = "ème";
                    s.Speak(i + se + " choix " + actions["c" + i.ToString()]["answer"].ToString());
                }
            }
            t_audio.Abort();
        }

        /// <summary>
        /// Called when the exit button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_exit_MouseDown(object sender, MouseEventArgs e)
        {
            Exit();
        }

        /// <summary>
        /// Called when the ItemViewer loses focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemViewer_Leave(object sender, EventArgs e)
        {
            Exit();
        }

        /// <summary>
        /// Called to exit the ItemViewer.
        /// Aborts the audiodescription thread if it exists and disposes the control.
        /// </summary>
        public void Exit()
        {
            if (t_audio != null && t_audio.IsAlive) t_audio.Abort();
            this.Dispose();
        }
    }
}
