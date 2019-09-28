using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class GameWindow : Form
    {

        private int vitessemax = 25;

        private int vitesse = 0;

        private List<Texture> listeTextures;

        private Timer timer = new Timer();

        public GameWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.listeTextures = new List<Texture>() {
                new Background(0),
                new Platform(0),
                new Tram(-500)
            };
            this.timer.Interval = 15;
            this.timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button btnLaunch = new Button();
            btnLaunch.Location = new Point(5,5);
            btnLaunch.Size = new Size(150, 50);
            btnLaunch.Text = "lancement";
            btnLaunch.Click += new EventHandler(btnLaunch_Click);
            this.Controls.Add(btnLaunch);
            this.Show();
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Button btnLaunch = sender as Button;
            btnLaunch.Enabled = false;
            btnLaunch.Visible = false;
            this.timer.Enabled = true;
        }

        private void Timer_Tick(object Sender, EventArgs e)
        {
            if(this.vitesse < this.vitessemax) { vitesse++; }
            this.Refresh();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            this.moveBackground();
            this.updateTextures(e);
        }

        private void moveBackground()
        {
            foreach (Texture t in listeTextures)
            {
                if (t.isLocked())
                {
                    t.move(-vitesse, 0);
                }
            }
        }

        private void updateTextures(PaintEventArgs e)
        {
            foreach (Texture t in listeTextures)
            {
                t.updateGraphic(e);
            }
        }

        private void GameWindow_MouseDown(object sender, MouseEventArgs e)
        {
            foreach(Texture t in listeTextures)
            {
                if (t.isHitboxHit(e.Location.X, e.Location.Y))
                {
                    MessageBox.Show("OK");
                }
            }
        }
    }
}
