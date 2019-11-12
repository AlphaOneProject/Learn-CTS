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
        private Transport vehicule;
        private Background background;
        private Platform platform;
        private int draw_surface_width = 0;
        private int draw_surface_height = 0;
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
        private List<NPC> npcs_leaving_vehicule;
        private int tick_stopped = 0;
        private int NPCsDensity = 100; //max 650


        /// <summary>
        /// Initialize the game window
        /// </summary>

        public GameWindow(string game)
        {
            this.game = game;
            this.game_path = this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            this.Text = game;
            InitializeComponent();
            DoubleBuffered = true;
        }

        public GameWindow(string game, string situation) : this(game)
        {

        }

        /// <summary>
        /// Initialize the timer
        /// </summary>

        private void InitializeTimer()
        {
            timer.Interval = 30;
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
            vehicule = new Tram(-4000, 198);
            background = new Background(0);
            background.DisableCollisions();
            platform = new Platform(0, vehicule.GetY()+vehicule.GetHeight(), vehicule.GetZ() + 2);
            platform.AddChild(player);
            list_textures = new List<Texture>() {
                background,
                vehicule,
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
            if(!vehicule.IsInside() && vehicule.GetState() == 0) MoveTexturesIfPlayerMoves();
            if (vehicule.GetState() == 0)
            {
                tick_stopped += 1;
                NPCLeaveVehicule();
                RemoveNPCsLeavingPlatform();
                if (tick_stopped > 200)
                {
                    vehicule.ChangeState();
                    tick_stopped = 0;
                }
            }
            else if (vehicule.GetState() == 1)
            {
                foreach(Texture n in vehicule.GetListChilds())
                {
                    if(n.GetType().Name == "NPC") ((NPC)n).Animated(false);
                }
            }
            if (vehicule.GetX() < this.Width && !vehicule.IsInside())
            {
                CheckIfCharacterIsEnteringTheVehicule();
                CheckIfCharacterIsLeavingTheVehicule();
                CheckIfTheVehiculeIsArrived();
            }
            else
            {
                if (!vehicule.IsInside() && vehicule.IsPlayerInside())
                {
                    ViewInside();
                }
                else if(vehicule.IsInside()) MoveBackground();
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
            if (player.GetX() + player.GetWidth() > draw_surface_width - 9)
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
            else if(!player.HasObjective())
            {
                player.SetDefaultPose();
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
            this.Focus();
        }

        /// <summary>
        /// Check if the vehicule has to slow down.
        /// </summary>

        private void CheckIfTheVehiculeIsArrived()
        {
            if (vehicule.GetState() == 2 && (vehicule.GetX()+vehicule.GetWidth() > platform.GetX() + platform.GetWidth() - vehicule.GetDistanceMaxStop()) && (vehicule.GetX() + vehicule.GetWidth() < platform.GetX() + platform.GetWidth() - vehicule.GetDistanceMaxStop() + 100))
            {
                vehicule.ChangeState();
            }
        }

        /// <summary>
        /// Check if a character enters the vehicule, add him as a child of the vehicule and remove it from the platform.
        /// </summary>

        private void CheckIfCharacterIsEnteringTheVehicule()
        {
            if(vehicule.GetState() == 0)
            {
                Texture t;
                for (int i = platform.GetListChilds().Count - 1; i >= 0; i--)
                {
                    t = platform.GetListChilds()[i];
                    if (t.GetType().Name == "Player" || t.GetType().Name == "NPC")
                    {
                        if (t.GetZ() <= vehicule.GetY() + vehicule.GetHeight() && t.GetZ() > vehicule.GetY())
                        {
                            vehicule.AddChild(t);
                            platform.RemoveChild(t);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if a character leaves the vehicule, remove it from the vehicule and add it to the platform.
        /// </summary>

        private void CheckIfCharacterIsLeavingTheVehicule()
        {
            if (vehicule.GetState() == 0)
            {
                Texture t;
                for (int i = vehicule.GetListChilds().Count - 1; i >= 0; i--)
                {
                    t = vehicule.GetListChilds()[i];
                    if (t.GetType().Name == "Player" || t.GetType().Name == "NPC")
                    {
                        if (t.GetZ() >= vehicule.GetY() + vehicule.GetHeight())
                        {
                            platform.AddChild(t);
                            vehicule.RemoveChild(t);
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
            if(this.draw_surface_width == 0 && this.draw_surface_height == 0)
            {
                this.draw_surface_width = e.ClipRectangle.Width;
                this.draw_surface_height = e.ClipRectangle.Height;
            }
            if(e.ClipRectangle.Width != this.draw_surface_width || this.draw_surface_height != e.ClipRectangle.Height)
            {
                int diff_x = e.ClipRectangle.Width - this.draw_surface_width;
                int diff_y = e.ClipRectangle.Height - this.draw_surface_height;
                this.draw_surface_width = e.ClipRectangle.Width;
                this.draw_surface_height = e.ClipRectangle.Height;
                foreach (Texture t in list_textures)
                {
                    if (vehicule.IsInside())
                    {
                        PlacePlayerMiddleScreen();
                    }
                    if (diff_y != 0) t.SetY(t.GetY() + diff_y / 2);
                }
            }
            PaintAllTextures(e);
        }

        /// <summary>
        /// Move the background.
        /// </summary>

        private void MoveBackground()
        {
            background.Move(-vehicule.GetMaxSpeed(), 0);
        }

        /// <summary>
        /// Retrieve all the textures, sort them by depth then are paint on the window.
        /// </summary>
        /// <param name="e"></param>

        private void PaintAllTextures(PaintEventArgs e)
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
            if(!this.Controls.Contains(d) && !SearchNPCDialog(list_textures, mouse_x, mouse_y))
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
            if (vehicule.IsInside())
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
            if (!vehicule.IsInside())
            {
                player.Move(a, 0);
                if (IsCharacterCollidingWithTextures(player))
                {
                    player.Move(-a, 0);
                    c_vertical = false;
                }
                player.Move(0, b);
                if (IsCharacterCollidingWithTextures(player) || ((vehicule.GetX() > 0 || vehicule.GetX() + vehicule.GetWidth() < draw_surface_width) && b<0 && player.GetY() <= platform.GetY() - player.GetHeight()))
                {
                    player.Move(0, -b);
                    c_horizontal = false;
                }
            }
            else
            {
                vehicule.Move(-a, 0);
                if (IsCharacterCollidingWithTextures(player))
                {
                    vehicule.Move(a, 0);
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
                c.Move(-a, 0);
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
                case Keys.F: this.StopVehicule(); ; break;
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
            timer.Stop();
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

        private void StopVehicule()
        {
            if (vehicule.GetState() == 2)
            {
                if (vehicule.IsInside())
                {
                    Character.SetM(3);
                    vehicule.ChangeInside();
                    list_textures.Remove(player);
                    vehicule.AddChild(player);
                    platform = new Platform(vehicule.GetX(), vehicule.GetY() + vehicule.GetHeight(), vehicule.GetZ() + 2);
                    vehicule.SetSpeed(vehicule.GetMaxSpeed());
                    list_textures.Add(platform);
                    FillPlatformNPCs();
                }
                vehicule.SetX(-4000);
                vehicule.SetSpeed(vehicule.GetMaxSpeed());
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
                if (vehicule != null && (npc_x > vehicule.GetX() && npc_x < vehicule.GetX() + vehicule.GetWidth()) && (npc_y > vehicule.GetY() && npc_y < vehicule.GetY() + vehicule.GetHeight()))
                {
                    vehicule.AddChild(nm.CreateNPC(npc_name, npc_x, npc_y, npc_quiz, npc_folder, false));
                }
                else
                {
                    platform.AddChild(nm.CreateNPC(npc_name, npc_x, npc_y, npc_quiz, npc_folder, true));
                }
            }
            FillVehiculeNPCs();
            FillPlatformNPCs();
        }

        private void NPCLeaveVehicule()
        {
            if (!leave_npc)
            {
                npcs_leaving_vehicule = SelectionNPCsLeavingVehicule();
                Random r = new Random();
                int i;
                foreach (NPC n in npcs_leaving_vehicule)
                {
                    n.Animated(true);
                    i = vehicule.GetIndexNearestDoor(n.GetX());
                    n.SetObjectiveX(vehicule.GetPosDoor(i));
                    n.SetObjectiveY(vehicule.GetY() + vehicule.GetHeight() + r.Next(0, 100));
                    n.SetObjective(n.GetX() + n.GetWidth() / 2 + r.Next(-1024, 1014), 2000);
                }
                leave_npc = true;
                enter_npc = true;
            }
            else
            {
                if (npcs_leaving_vehicule != null && enter_npc && HasAllNPCsLeavedVehicule(npcs_leaving_vehicule))
                {
                    NPCEnterVehicule();
                }
            }
        }

        private List<NPC> SelectionNPCsLeavingVehicule()
        {
            Random r = new Random();
            List<NPC> list = new List<NPC>();
            for(int i = 0; i< vehicule.GetListChilds().Count; i++)
            {
                if(vehicule.GetListChilds()[i].GetType().Name == "NPC" && ((NPC)vehicule.GetListChilds()[i]).GetQuiz()<1 && r.Next(0,3)!=0) list.Add((NPC)vehicule.GetListChilds()[i]);
            }
            return list;
        }

        private bool HasAllNPCsLeavedVehicule(List<NPC> l)
        {
            bool b = true;
            foreach(NPC n in l)
            {
                if (n.GetY() <= vehicule.GetY() + vehicule.GetHeight()+10)
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
                if(platform.GetListChilds()[i].GetY() > draw_surface_height && platform.GetListChilds()[i].GetType().Name == "NPC")
                {
                    platform.RemoveChild(platform.GetListChilds()[i]);
                }
            }
        }

        private void ViewInside()
        {
            ConsoleAvgFPS();
            platform.RemoveAllChilds();
            list_textures.Remove(platform);
            platform.Dispose();
            vehicule.ChangeInside();
            vehicule.SetState(2);
            vehicule.SetSpeed(0);
            Character.SetM(4);
            PlacePlayerMiddleScreen();
            list_textures.Add(player);
        }

        private void PlacePlayerMiddleScreen()
        {
            if (!vehicule.GetListChilds().Contains(player))
            {
                vehicule.AddChild(player);
            }
            int px = player.GetX() + player.GetWidth() / 2 - vehicule.GetX();
            vehicule.SetX(this.draw_surface_width / 2 - px);
            vehicule.RemoveChild(player);
        }

        private void NPCEnterVehicule()
        {
            Random r = new Random();
            int i;
            NPC n;
            int y;
            foreach (Texture t in platform.GetListChilds())
            {
                if (t.GetType().Name == "NPC" && !((NPC)t).HasObjective())
                {
                    n = (NPC)t;
                    i = vehicule.GetIndexNearestDoor(n.GetX());
                    /*n.SetObjectiveX(vehicule.GetPosDoor(i));
                    n.SetObjectiveY(platform.GetY() - r.Next(10, 25));*/
                    y = vehicule.GetY() + vehicule.GetHeight();
                    n.SetObjective(vehicule.GetPosDoor(i), y);
                    n.SetObjectiveY(platform.GetY() - r.Next(20, 35) + n.GetZ() - y);
                    if (i == 0)
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-64, 256));
                    }
                    else if (i == vehicule.GetNumberDoors() - 1)
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

        private void FillVehiculeNPCs()
        {
            Random r = new Random();
            //int max = r.Next(NPCsDensity / 4, NPCsDensity / 2);
            int max = NPCsDensity / 2;
            int x;
            int y;
            for(int i = 0; i < max; i++)
            {
                x = vehicule.GetX() + r.Next(492, vehicule.GetWidth() - 492);
                if (r.Next(0, 2) == 0) y = vehicule.GetY() + 144 + r.Next(0, 5);
                else y = vehicule.GetY() + 144 + r.Next(15, 20);
                vehicule.AddChild(nm.CreateNPC(x,y,true));
            }
        }

        private void FillPlatformNPCs()
        {
            Random r = new Random();
            //int max = r.Next(NPCsDensity / 4, NPCsDensity / 2);
            int max = NPCsDensity / 2;
            int x;
            int y;
            for (int i = 0; i < max; i++)
            {
                x = platform.GetX() + r.Next(100, platform.GetWidth() - 100);
                y = platform.GetY() + r.Next(20, 192);
                platform.AddChild(nm.CreateNPC(x, y, true));
            }
        }
    }
}
