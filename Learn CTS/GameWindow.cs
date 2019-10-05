using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Learn_CTS
{
    public partial class GameWindow : Form
    {

        private List<Texture> listTextures;

        private Timer timer = new Timer();

        private Player player = new Player(600, 502);

        private Tram tram = new Tram(-4000,200);

        private Background background = new Background(0);

        private NPC npc = new NPC(-3400, 340);

        private int DrawSurfaceHeight;

        private int DrawSurfaceWidth;

        public GameWindow()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void InitializeTimer()
        {
            timer.Interval = 15;
            timer.Tick += new EventHandler(Timer_Tick);
        }

        private void InitializeListTextures()
        {
            listTextures = new List<Texture>() {
                background,
                new Platform(0),
                tram,
                player,
                npc
            };
            tram.AddPassenger(npc);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button btn_launch = new Button();
            btn_launch.Location = new Point(5,5);
            btn_launch.Size = new Size(150, 50);
            btn_launch.Text = "Launch Game";
            btn_launch.Click += new EventHandler(Btn_Launch_Click);
            Controls.Add(btn_launch);
            Show();
            InitializeListTextures();
            InitializeTimer();
        }

        private void Btn_Launch_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            Refresh();
            Button btnLaunch = sender as Button;
            btnLaunch.Enabled = false;
            btnLaunch.Visible = false;
        }

        private void Timer_Tick(object Sender, EventArgs e)
        {
            CheckKeyboard();
            if (tram.GetX() < this.Width && !tram.IsInside())
            {
                CheckTram();
            }
            else
            {
                if (!tram.IsInside())
                {
                    tram.ChangeInsideOutside();
                    tram.SetState(0);
                    tram.SetSpeed(0);
                    PlacePlayerMiddleScreen();
                }
                MoveBackground();
            }
            Refresh();
        }

        private void PlacePlayerMiddleScreen()
        {
            int px = player.GetX() - tram.GetX();
            player.SetX(this.DrawSurfaceWidth / 2 - (player.GetImage().Width / 2));
            tram.SetX(this.DrawSurfaceWidth / 2 - px);
        }

        private void CheckTram()
        {
            if (player.GetZ() < tram.GetY() + tram.GetImage().Height && player.GetZ() > tram.GetY() && tram.GetState() == 0)
            {
                tram.AddPassenger(player);
                tram.ChangeState();
            }
            if (tram.GetX() >= -200 && tram.GetX() <= -100 && tram.GetState() == 2)
            {
                tram.ChangeState();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawSurfaceHeight = e.ClipRectangle.Height;
            this.DrawSurfaceWidth = e.ClipRectangle.Width;
            UpdateTextures(e);
        }

        private void MoveBackground()
        {
            foreach (Texture t in listTextures)
            {
                if (t.IsLocked())
                {
                    t.Move(-tram.GetMaxSpeed(), 0);
                }
            }
        }

        private void UpdateTextures(PaintEventArgs e)
        {
            listTextures.Sort(Texture.Compare);
            foreach (Texture t in listTextures)
            {
                t.UpdateGraphic(e);
                //t.debug(e);
            }
        }

        private void GameWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach(Texture t in listTextures)
            {
                if (t.GetType().Name == "NPC" && t.IsHitboxHit(e.Location.X, e.Location.Y))
                {
                    MessageBox.Show("Dialogue");
                }
            }
        }

        private void CheckKeyboard()
        {
            int a = 0;
            int b = 0;
            if (Keyboard.IsKeyDown(Key.Left))
            {
                a -= 8;
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                a += 8;
            }
            if (Keyboard.IsKeyDown(Key.Up))
            {
                b -= 8;
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                b += 8;
            }
            if (!tram.IsInside())
            {
                player.Move(a, b);
                if (IsCollidingWithTextures(player))
                {
                    player.Move(-a, -b);
                }
            }
            else
            {
                tram.Move(-a, -b);
                if (IsCollidingWithTextures(player))
                {
                    tram.Move(a, b);
                }
            }
        }

        private bool IsCollidingWithTextures(Character p)
        {
            bool b = false;
            foreach(Texture t in listTextures)
            {
                if (p.CollideWith(t))
                {
                    b = true;
                }
            }
            return b;
        }
    }
}
