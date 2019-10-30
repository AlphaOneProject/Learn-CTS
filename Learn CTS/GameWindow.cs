using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Learn_CTS
{
    /// <summary>
    /// Window where the game is displayed
    /// </summary>

    public partial class GameWindow : Form
    {
        /// <summary>
        /// Attributes
        /// </summary>

        private List<Texture> list_textures;
        private NPC_Manager nm = NPC_Manager.GetInstance();
        private Timer timer = new Timer();
        private Player player;
        private Tram tram;
        private Background background;
        private Platform platform;
        private int DrawSurfaceWidth;
        private int DrawSurfaceHeight;
        private bool go_up = false;
        private bool go_down = false;
        private bool go_left = false;
        private bool go_right = false;
        private int ticks = 1;
        private double start_milliseconds = (DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds;
        private Dialog d;
        private string game;
        private string game_path;


        /// <summary>
        /// Initialize the game window
        /// </summary>

        public GameWindow(String game)
        {
            this.game = game;
            this.game_path = this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            this.Text = game;
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Initialize the timer
        /// </summary>

        private void InitializeTimer()
        {
            timer.Interval = 20;
            timer.Tick += new EventHandler(Timer_Tick);
        }

        /// <summary>
        /// Initialize all the textures
        /// </summary>

        private void InitializeListTextures()
        {
            Texture.InitializePath(game);
            player = new Player(600, 504);
            tram = new Tram(-4000, 198);
            background = new Background(0);
            background.DisableCollisions();
            platform = new Platform(-300, tram.GetZ() + 2);
            list_textures = new List<Texture>() {
                background,
                tram,
                player,
                platform
            };
            InitializeNPCs();
        }

        /// <summary>
        /// When the form is loading, create a button "Launch", 
        /// initialize the timer and the textures,
        /// then show the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Form1_Load(object sender, EventArgs e)
        {
            Button btn_launch = new Button();
            btn_launch.Location = new Point(5,5);
            btn_launch.Size = new Size(150, 50);
            btn_launch.Text = "Lancer le jeu";
            btn_launch.Click += new EventHandler(Btn_Launch_Click);
            Controls.Add(btn_launch);
            InitializeListTextures();
            InitializeTimer();
            Character.SetM(3);
            Show();
        }

        /// <summary>
        /// Show the fps average at this moment.
        /// </summary>
        private void ConsoleAvgFPS()
        {
            double diff = ((DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds - start_milliseconds) / 1000;
            Console.WriteLine("Fps moyen : " + (ticks / diff).ToString().Substring(0, 4));
            ticks = 0;
            start_milliseconds = (DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds;
        }

        /// <summary>
        /// Executed when the user press the button "Launch"
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Btn_Launch_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            Refresh();
            Button btnLaunch = sender as Button;
            btnLaunch.Enabled = false;
            btnLaunch.Visible = false;
        }

        /// <summary>
        /// Instructions executed every 15ms
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>

        private void Timer_Tick(object Sender, EventArgs e)
        {
            ticks += 1;
            if (tram.GetState() != 1)
            {
                MoveTexturesIfPlayerMoves();
            }
            if (tram.GetX() < this.Width && !tram.IsInside())
            {
                CheckIfCharacterIsEnteringTheTram();
                CheckIfTheTramIsArrived();
            }
            else
            {
                if (!tram.IsInside())
                {
                    ConsoleAvgFPS();
                    list_textures.Remove(platform);
                    tram.ChangeInside();
                    tram.SetState(2);
                    tram.SetSpeed(0);
                    Character.SetM(9);
                    PlacePlayerMiddleScreen();
                }
                MoveBackground();
            }
            if (player.HasObjective())
            {
                MovePlayerToObjective();
            }
            ArrowsPressed();
            Refresh();
            //Update();
        }

        /// <summary>
        /// Move the textures if the player reaches the borders of the screen.
        /// </summary>

        private void MoveTexturesIfPlayerMoves()
        {
            if (player.GetX() + player.GetWidth() > DrawSurfaceWidth - 9)
            {
                foreach (Texture t in list_textures)
                {
                    t.Move(-8, 0);
                }
            }
            else if (player.GetX() <= 9)
            {
                foreach (Texture t in list_textures)
                {
                    t.Move(8, 0);
                }
            }
            //WIP
            /*if (player.GetY() + player.GetHeight() < DrawSurfaceHeight - 17  && player.GetY() + player.GetHeight() > DrawSurfaceHeight - 25)
            {
                foreach (Texture t in list_textures)
                {
                    t.Move(0, 8);
                }
            }
            else if (player.GetY() + player.GetHeight() > DrawSurfaceHeight - 9)
            {
                foreach (Texture t in list_textures)
                {
                    t.Move(0, -8);
                }
            }*/
        }

        /// <summary>
        /// Check if arrows are pressed
        /// </summary>

        private void ArrowsPressed()
        {
            int a = 0;
            int b = 0;

            if (go_left)
            {
                a -= 8;
            }
            else if (go_right)
            {
                a += 8;
            }
            if (go_up)
            {
                b -= 8;
            }
            if (go_down)
            {
                b += 8;
            }
            if(a != 0 || b != 0)
            {
                player.RemoveObjective();
                MovePlayer(a, b);
            }
        }

        /// <summary>
        /// Move the player toward his objective
        /// </summary>

        private void MovePlayerToObjective()
        {
            int a = 0;
            int b = 0;
            if (!player.ReachedObjX())
            {
                if (player.GetX() + player.GetWidth() / 2 > player.GetObjX() + 8)
                {
                    a = -8;
                }
                else if (player.GetX() + player.GetWidth() / 2 < player.GetObjX() - 8)
                {
                    a = 8;
                }
            }
            if (!player.ReachedObjY())
            {
                if (player.GetY() + player.GetHeight() > player.GetObjY() - 16)
                {
                    b = -8;
                }
                else if (player.GetY() + player.GetHeight() < player.GetObjY() + 16)
                {
                    b = 8;
                }
            }
            if (!MovePlayer(a, b) || player.ReachedObjective())
            {
                player.RemoveObjective();
            }
        }

        /// <summary>
        /// Place the player at the middle of the user's screen
        /// </summary>

        private void PlacePlayerMiddleScreen()
        {
            tram.RemoveChild(player);
            list_textures.Add(player);
            int px = player.GetX() - tram.GetX();
            player.SetX(this.DrawSurfaceWidth / 2 - (player.GetWidth() / 2));
            tram.SetX(this.DrawSurfaceWidth / 2 - px);
        }

        /// <summary>
        /// Check if the tram has to slow down.
        /// </summary>

        private void CheckIfTheTramIsArrived()
        {
            if (tram.GetState() == 2 && (tram.GetX()+tram.GetWidth() > platform.GetX() + platform.GetWidth() - tram.GetDistanceMaxStop()) && (tram.GetX() + tram.GetWidth() < platform.GetX() + platform.GetWidth() - tram.GetDistanceMaxStop() + 100))
            {
                tram.ChangeState();
            }
        }

        /// <summary>
        /// Check if a player or a npc enter the tram and add him as a passenger of the latter and remove him for the list of the textures
        /// </summary>

        private void CheckIfCharacterIsEnteringTheTram()
        {
            if(tram.GetState() == 0)
            {
                for (int i = list_textures.Count - 1; i >= 0; i--)
                {
                    if (list_textures[i].GetType().Name == "Player" || list_textures[i].GetType().Name == "NPC")
                    {
                        if (list_textures[i].GetZ() < tram.GetY() + tram.GetHeight() && list_textures[i].GetZ() > tram.GetY())
                        {
                            tram.AddChild(list_textures[i]);
                            if (list_textures[i].GetType().Name == "Player")
                            {
                                tram.ChangeState();
                            }
                            list_textures.Remove(list_textures[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Paint the textures at the user's screen.
        /// </summary>
        /// <param name="e"></param>


        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawSurfaceWidth = e.ClipRectangle.Width;
            this.DrawSurfaceHeight = e.ClipRectangle.Height;
            UpdateTextures(e);
        }

        /// <summary>
        /// Move the background.
        /// </summary>

        private void MoveBackground()
        {
            background.Move(-tram.GetMaxSpeed(), 0);
        }

        /// <summary>
        /// Retrieve all the textures, sort them by depth then are paint on the window.
        /// </summary>
        /// <param name="e"></param>

        private void UpdateTextures(PaintEventArgs e)
        {
            List<Texture> list_all_textures = GetAllTextures(list_textures);
            list_all_textures.Sort(Texture.Compare);
            foreach (Texture t in list_all_textures)
            {
                t.UpdateGraphic(e);
                //t.Debug(e);
            }
        }

        /// <summary>
        /// Retrieve all the textures and the childs of each textures.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>

        private List<Texture> GetAllTextures(List<Texture> list)
        {
            List<Texture> list_temp = new List<Texture>();
            foreach(Texture t in list)
            {
                list_temp.Add(t);
                list_temp.AddRange(GetAllTextures(t.GetListChilds()));
            }
            return list_temp;
        }

        /// <summary>
        /// When the player clicks on the mouse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int mouse_x = e.Location.X;
            int mouse_y = e.Location.Y;
            if(!SearchNPCDialog(list_textures, mouse_x, mouse_y))
            {
                player.GoTo(mouse_x, mouse_y);
            }
        }

        /// <summary>
        /// Activate a dialog if the player clicks on a NPC.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <returns></returns>

        private bool SearchNPCDialog(List<Texture> list, int mx, int my)
        {
            if (tram.IsInside())
            {
                foreach (Texture t in list)
                {
                    if (t.GetType().Name == "NPC" && t.IsHitboxHit(mx, my))
                    {
                        d = new Dialog(((NPC)t).GetID(),game);
                        this.Controls.Add(d);
                        return true;
                    }
                    SearchNPCDialog(t.GetListChilds(), mx, my);
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Check if a character is colliding with the other textures in the game.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private bool IsCollidingWithTextures(Character p)
        {
            foreach(Texture t in list_textures)
            {
                if (t.CollideWith(p))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Move the player if there are no collisions.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if the player has moved, false otherwise.</returns>

        private bool MovePlayer(int a, int b)
        {
            bool boo = true;
            if (this.Controls.Contains(d))
            {
                return false;
            }
            if (!tram.IsInside())
            {
                player.Move(a, 0);
                player.UpdateMovement(a, b);
                if (IsCollidingWithTextures(player))
                {
                    player.Move(-a, 0);
                    boo = false;
                }
                player.Move(0, b);
                if (IsCollidingWithTextures(player))
                {
                    player.Move(0, -b);
                    boo = false;
                }
            }
            else
            {
                tram.Move(-a, 0);
                player.UpdateMovement(a, b);
                if (IsCollidingWithTextures(player))
                {
                    tram.Move(a, 0);
                    boo = false;
                }
                player.Move(0, b);
                player.UpdateMovement(a, b);
                if (IsCollidingWithTextures(player))
                {
                    player.Move(0, -b);
                    boo = false;
                }
            }
            return boo;
        }

        /// <summary>
        /// Check if the player presses down the arrows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left : go_left = true; break;
                case Keys.Right : go_right = true; break;
                case Keys.Up : go_up = true; break;
                case Keys.Down : go_down = true; break;
            }
        }

        /// <summary>
        /// Check if the player presses up the arrows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: go_left = false; break;
                case Keys.Right: go_right = false; break;
                case Keys.Up: go_up = false; break;
                case Keys.Down: go_down = false; break;
            }
        }

        /// <summary>
        /// Display the average FPS during the game session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConsoleAvgFPS();
            nm.Clear();
        }

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

        public void InitializeNPCs()
        {
            /*tram.AddChild(nm.CreateNPC("1", -3200, 400, 1));
            tram.AddChild(nm.CreateNPC("2", -2400, 430, 2));
            tram.AddChild(nm.CreateNPC("3", -1800, 420, 3));*/
            JObject npcs = Get_From_JSON("library" + Path.DirectorySeparatorChar + "dialogs_test.json");
            JObject npcs2 = Get_From_JSON("library" + Path.DirectorySeparatorChar + "npcs.json");
            foreach (KeyValuePair<string, JToken> line in npcs)
            {
                tram.AddChild(nm.CreateNPC(npcs2[line.Key]["name"].ToString(), line.Value["x"].ToObject<int>(), line.Value["y"].ToObject<int>(), line.Value["quizz"].ToObject<int>()));
            }
        }
    }
}
