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

        private List<Texture> listeImages = new List<Texture>();

        private Background background = new Background(0);

        private Tram tram = new Tram(-500);

        public GameWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            this.gameLoop();
        }

        private void gameLoop()
        {
            while (true)
            {
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            background.move(-vitesse, 0);
            background.updateGraphic(g, e);
            tram.updateGraphic(g, e);
        }
    }
}
