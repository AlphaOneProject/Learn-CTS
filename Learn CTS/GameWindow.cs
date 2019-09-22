using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class GameWindow : Form
    {

        private string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

        private int vitesse = 25;

        private int acc = 0;

        private List<Texture> listeImages = new List<Texture>();

        private Background background = new Background(0);

        private Tram tram = new Tram(-500);

        private Platform platform = new Platform(0);

        public GameWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
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
            gameLoop();
        }

        private void gameLoop()
        {
            acc = 0;
            while (true)
            {
                if (acc < vitesse) { acc++; }
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            background.move(-acc, 0);
            platform.move(-acc, 0);
            background.updateGraphic(g, e);
            tram.updateGraphic(g, e);
            platform.updateGraphic(g, e);
        }
    }
}
