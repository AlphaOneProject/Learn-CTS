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
        /// Attributes

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
        private bool debug = false;
        private bool god = false;
        private bool enter_npc = true;
        private bool leave_npc = false;
        private List<NPC> npcs_leaving_tram;
        private int tick_tram_stopped = 0;


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
            Character.SetM(3);
            player = new Player("Moi",600, 504);
            tram = new Tram(-4000, 198);
            background = new Background(0);
            background.DisableCollisions();
            platform = new Platform(0, tram.GetZ() + 2);
            platform.AddChild(player);
            list_textures = new List<Texture>() {
                background,
                tram,
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
            InitializeListTextures();
            InitializeTimer();
            Show();
            timer.Enabled = true;
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
        /// Instructions executed every 15ms
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>

        private void Timer_Tick(object Sender, EventArgs e)
        {
            ticks += 1;
            if(!tram.IsPlayerInside()) MoveTexturesIfPlayerMoves();
            if (tram.GetState() == 0)
            {
                tick_tram_stopped += 1;
                NPCLeaveTram();
                RemoveNPCsLeavingPlatform();
                if (tick_tram_stopped > 200)
                {
                    tram.ChangeState();
                    tick_tram_stopped = 0;
                }
            }
            else if (tram.GetState() == 1)
            {
                foreach(Texture n in tram.GetListChilds())
                {
                    if(n.GetType().Name == "NPC") ((NPC)n).Animated(false);
                }
            }
            if (tram.GetX() < this.Width && !tram.IsInside())
            {
                CheckIfCharacterIsEnteringTheTram();
                CheckIfCharacterIsLeavingTheTram();
                CheckIfTheTramIsArrived();
            }
            else
            {
                if (!tram.IsInside() && tram.IsPlayerInside())
                {
                    ConsoleAvgFPS();
                    platform.RemoveAllChilds();
                    list_textures.Remove(platform);
                    platform.Dispose();
                    tram.ChangeInside();
                    tram.SetState(2);
                    tram.SetSpeed(0);
                    //Character.SetM(6);
                    PlacePlayerMiddleScreen();
                }
                else if(tram.IsInside()) MoveBackground();
            }
            foreach(Texture t in GetAllTextures(list_textures))
            {
                if ((t.GetType().Name == "Player" || t.GetType().Name == "NPC") && ((Character)t).HasObjective())
                {
                    MoveCharacterToObjective((Character)t);
                }
            }
            ArrowsPressed();
            Refresh();
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

        private void MoveCharacterToObjective(Character c)
        {
            int a = 0;
            int b = 0;
            if (!c.ReachedObjX())
            {
                if (c.GetObjX() > 8)
                {
                    a = 8;
                }
                else if (c.GetObjX() < -8)
                {
                    a = -8;
                }
                c.UpdateObjX(-a);
            }
            if (!c.ReachedObjY())
            {
                if (c.GetObjY() > 8)
                {
                    b = 8;
                }
                else if (c.GetObjY() < -8)
                {
                    b = -8;
                }
                c.UpdateObjY(-b);
            }
            if(c == player)
            {
                MovePlayer(a, b);
            }
            else
            {
                MoveCharacter(c, a, b);
            }
            if (c.ReachedObjective())
            {
                c.RemoveObjective();
            }
        }

        public void RemoveDialog()
        {
            this.Controls.Remove(d);
            d.Dispose();
            d = null;
            this.Focus();
        }

        /// <summary>
        /// Place the player at the middle of the user's screen
        /// </summary>

        private void PlacePlayerMiddleScreen()
        {
            int px = player.GetX() + player.GetWidth()/2 - tram.GetX();
            tram.SetX(this.DrawSurfaceWidth / 2 - px);
            tram.RemoveChild(player);
            list_textures.Add(player);
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
        /// Check if a character enters the tram, add him as a child of the tram and remove it from the platform.
        /// </summary>

        private void CheckIfCharacterIsEnteringTheTram()
        {
            if(tram.GetState() == 0)
            {
                Texture t;
                for (int i = platform.GetListChilds().Count - 1; i >= 0; i--)
                {
                    t = platform.GetListChilds()[i];
                    if (t.GetType().Name == "Player" || t.GetType().Name == "NPC")
                    {
                        if (t.GetZ() <= tram.GetY() + tram.GetHeight() && t.GetZ() > tram.GetY())
                        {
                            tram.AddChild(t);
                            platform.RemoveChild(t);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if a character leaves the tram, remove it from the tram and add it to the platform.
        /// </summary>

        private void CheckIfCharacterIsLeavingTheTram()
        {
            if (tram.GetState() == 0)
            {
                Texture t;
                for (int i = tram.GetListChilds().Count - 1; i >= 0; i--)
                {
                    t = tram.GetListChilds()[i];
                    if (t.GetType().Name == "Player" || t.GetType().Name == "NPC")
                    {
                        if (t.GetZ() >= tram.GetY() + tram.GetHeight())
                        {
                            platform.AddChild(t);
                            tram.RemoveChild(t);
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
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            foreach (Texture t in list_all_textures)
            {
                t.OnPaint(e);
                if (debug)
                {
                    t.Debug(e);
                }
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
                player.SetObjective(mouse_x, mouse_y);
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
                foreach (NPC t in nm.GetList())
                {
                    if (t.GetQuiz() > 0 && t.IsHitboxHit(mx, my))
                    {
                        if(Math.Abs((t.GetX()+t.GetWidth()/2 - (player.GetX()+player.GetWidth()/2))) < 256 && Math.Abs((t.GetY() - player.GetY())) < 256)
                        {
                            d = new Dialog(t.GetID(), game);
                            this.Controls.Add(d);
                        }
                        else
                        {
                            MessageBox.Show("Vous devez vous rapprocher pour parler à cette personne.");
                        }
                        return true;
                    }
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

        private bool IsCharacterCollidingWithTextures(Character c)
        {
            foreach(Texture t in list_textures)
            {
                if (t.CollideWith(c))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Move the player if there are no collisions.
        /// </summary>
        /// <param name="a">Move the player of a</param>
        /// <param name="b"></param>
        /// <returns>true if the player has moved, false otherwise.</returns>

        private void MovePlayer(int a, int b)
        {
            bool c_vertical = true;
            bool c_horizontal = true;
            if (this.Controls.Contains(d)) return;
            player.UpdateMovement(a, b);
            if (!tram.IsInside())
            {
                player.Move(a, 0);
                if (IsCharacterCollidingWithTextures(player))
                {
                    player.Move(-a, 0);
                    c_vertical = false;
                }
                player.Move(0, b);
                if (IsCharacterCollidingWithTextures(player) || ((tram.GetX() > 0 || tram.GetX() + tram.GetWidth() < DrawSurfaceWidth) && player.GetY() < tram.GetY() + 200))
                {
                    player.Move(0, -b);
                    c_horizontal = false;
                }
            }
            else
            {
                tram.Move(-a, 0);
                if (IsCharacterCollidingWithTextures(player))
                {
                    tram.Move(a, 0);
                    c_vertical = false;
                }
                player.Move(0, b);
                if (IsCharacterCollidingWithTextures(player))
                {
                    player.Move(0, -b);
                    c_horizontal = false;
                }
            }
            if(!c_vertical && !c_horizontal)
            {
                player.RemoveObjective();
            }
        }

        /// <summary>
        /// Move the character
        /// </summary>
        /// <param name="c">The character</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        private void MoveCharacter(Character c, int a, int b)
        {
            c.UpdateMovement(a, b);
            c.Move(a, 0);
            if (c.CollideWith(player))
            {
                player.Move(-a, 0);
            }
            c.Move(0, b);
            if (c.CollideWith(player))
            {
                c.Move(0, -b);
            }
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
                case Keys.D: this.debug = !debug; break;
                case Keys.G: this.god = !god; if (god) player.DisableCollisions(); else player.EnableCollisions(); break;
                case Keys.F: this.StopTram(); ; break;
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
            foreach(Texture t in list_textures)
            {
                t.Dispose();
            }
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

        private void StopTram()
        {
            if (tram.GetState() == 2)
            {
                if (tram.IsInside())
                {
                    //Character.SetM(3);
                    tram.ChangeInside();
                    list_textures.Remove(player);
                    tram.AddChild(player);
                    platform = new Platform(tram.GetX(), tram.GetZ() + 2);
                    tram.SetSpeed(tram.GetMaxSpeed());
                    list_textures.Add(platform);
                }
                tram.SetX(-4000);
                tram.SetSpeed(tram.GetMaxSpeed());
                leave_npc = false;
            }
        }

        public void InitializeNPCs()
        {
            JObject npcs = Get_From_JSON("library" + Path.DirectorySeparatorChar + "dialogs.json");
            int npc_x;
            int npc_y;
            string npc_name;
            string npc_folder;
            int npc_quiz;
            foreach (KeyValuePair<string, JToken> line in npcs)
            {
                npc_x = line.Value["x"].ToObject<int>();
                npc_y = line.Value["y"].ToObject<int>();
                npc_name = npcs[line.Key]["npc"]["name"].ToString();
                npc_folder = npcs[line.Key]["npc"]["folder"].ToString();
                npc_quiz = line.Value["quizz"].ToObject<int>();
                if (tram != null && (npc_x > tram.GetX() && npc_x < tram.GetX() + tram.GetWidth()) && (npc_y > tram.GetY() && npc_y < tram.GetY() + tram.GetHeight()))
                {
                    tram.AddChild(nm.CreateNPC(npc_name, npc_x, npc_y, npc_quiz, npc_folder, false));
                }
                else
                {
                    platform.AddChild(nm.CreateNPC(npc_name, npc_x, npc_y, npc_quiz, npc_folder, true));
                }
            }
            FillGameNPCs();
        }

        private void NPCLeaveTram()
        {
            if (!leave_npc)
            {
                npcs_leaving_tram = SelectionNPCsLeavingTram();
                Random r = new Random();
                int i;
                foreach (NPC n in npcs_leaving_tram)
                {
                    n.Animated(true);
                    i = tram.GetIndexNearestDoor(n.GetX());
                    n.SetObjectiveX(tram.GetPosDoor(i));
                    n.SetObjectiveY(tram.GetY() + tram.GetHeight() + r.Next(0, 100));
                    n.SetObjective(n.GetX() + n.GetWidth() / 2 + r.Next(-1024, 1014), 2000);
                }
                leave_npc = true;
                enter_npc = true;
            }
            else
            {
                if (npcs_leaving_tram != null && enter_npc && HasAllNPCsLeavedTram(npcs_leaving_tram))
                {
                    NPCEnterTram();
                }
            }
        }

        private List<NPC> SelectionNPCsLeavingTram()
        {
            Random r = new Random();
            List<NPC> list = new List<NPC>();
            for(int i = 0; i< tram.GetListChilds().Count; i++)
            {
                if(tram.GetListChilds()[i].GetType().Name == "NPC" && ((NPC)tram.GetListChilds()[i]).GetQuiz()<1 && r.Next(0,3)!=0) list.Add((NPC)tram.GetListChilds()[i]);
            }
            return list;
        }

        private bool HasAllNPCsLeavedTram(List<NPC> l)
        {
            bool b = true;
            foreach(NPC n in l)
            {
                if (n.GetY() <= tram.GetY() + tram.GetHeight()+10)
                {
                    b = false;
                }
            }
            return b;
        }

        private void RemoveNPCsLeavingPlatform()
        {
            for(int i = platform.GetListChilds().Count - 1; i>=0; i--)
            {
                if(platform.GetListChilds()[i].GetY() > platform.GetY() + platform.GetHeight() && platform.GetListChilds()[i].GetType().Name == "NPC")
                {
                    platform.RemoveChild(platform.GetListChilds()[i]);
                }
            }
        }

        private void NPCEnterTram()
        {
            Random r = new Random();
            int i;
            foreach (Texture t in platform.GetListChilds())
            {
                if (t.GetType().Name == "NPC" && !((NPC)t).HasObjective())
                {
                    NPC n = (NPC)t;
                    i = tram.GetIndexNearestDoor(n.GetX());
                    n.SetObjectiveX(tram.GetPosDoor(i));
                    n.SetObjectiveY(tram.GetY() + 336 + r.Next(0, 20));
                    if (i == 0)
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-64, 256));
                    }
                    else if (i == tram.GetNumberDoors() - 1)
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, 64));
                    }
                    else
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, 256));
                    }
                }
            }
            enter_npc = false;
        }

        private void FillGameNPCs()
        {
            Random r = new Random();
            int max = r.Next(5, 10);
            int x;
            int y;
            for(int i = 0; i < max; i++)
            {
                x = tram.GetX() + r.Next(460, tram.GetWidth() - 460);
                y = tram.GetY() + 144 + r.Next(0, 20);
                tram.AddChild(nm.CreateNPC(x,y,true));
            }
            max = r.Next(7, 12);
            for (int i = 0; i < max; i++)
            {
                x = platform.GetX() + r.Next(100, platform.GetWidth() - 100);
                y = platform.GetY() + r.Next(20, 192);
                platform.AddChild(nm.CreateNPC(x, y, true));
            }
        }
    }
}
