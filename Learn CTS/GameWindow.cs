using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
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

        private static GameWindow instance;
        private NPC_Manager nm = NPC_Manager.GetInstance();
        private List<Texture> list_game_textures;
        private List<Texture> list_hud_textures;
        private List<string> list_comments;
        private System.Windows.Forms.Timer timer_game;
        private Player player;
        private Vehicule vehicule;
        private Background background;
        private Platform platform;
        private int draw_surface_width = 0;
        private int draw_surface_height = 0;
        private bool[] movement = { false, false, false, false};
        private int ticks = 0;
        private int ticks_temp = 0;
        private double start_milliseconds = (DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds;
        private int NPCsDensity = 0;
        private int score = -1;
        private int step_score = -1;
        private string sc_path;
        private string scenario;
        private string situation;
        private Random r;
        private bool preview = false;
        private int n_situation = 0;
        private Thread t_fps;
        private float n_fps;
        private bool ticket_valid = false;
        private Transition tr;
        private Texture backpack;
        private Egg egg;
        private EventHandler current_scene_tick;
        // development
        private bool showhitbox = false;
        private bool god = false;

        /// <summary>
        /// Initialize the game window
        /// </summary>

        private GameWindow(string game)
        {
            InitializeComponent();
            if (instance == null)
            {
                instance = this;
                Texture.InitializePath(game);
                this.Text = game;
                string game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
                sc_path = game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            }
            else
            {
                MessageBox.Show("Vous ne pouvez avoir qu'une seule fenêtre de jeu ouverte en même temps.");
            }
        }

        /// <summary>
        /// Launch the game at a scenario
        /// </summary>
        /// <param name="game"></param>
        /// <param name="scenario"></param>

        public GameWindow(string game, string scenario) : this(game)
        {
            this.scenario = scenario;
            if (this.situation == null) this.situation = Directory.GetDirectories(@"" + sc_path + this.scenario)[n_situation].Remove(0, sc_path.Length + this.scenario.Length + 1);
        }

        /// <summary>
        /// Launch a game, at a situation in a scenario
        /// Used exclusively to preview a situation
        /// </summary>
        /// <param name="game"></param>
        /// <param name="scenario"></param>
        /// <param name="situation"></param>

        public GameWindow(string game, string scenario, string situation) : this(game,scenario)
        {
            this.preview = true;
            this.situation = situation;
        }

        /// <summary>
        /// Get the instance of the current game window.
        /// </summary>
        /// <returns></returns>

        public static GameWindow GetInstance()
        {
            if (instance == null) throw new Exception();
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_Load(object sender, EventArgs e)
        {
            if (instance == null) throw new InvalidOperationException();
            SetUpWindow();
            lbl_nfps.Location = new Point((int)(this.Width * 0.98) - lbl_nfps.Width - 10, this.Height * 3 / 32);
            lbl_score.Location = new Point(this.Width / 2 - lbl_score.Width - 5, this.Height * 1 / 32);
            lbl_nscore.Location = new Point(this.Width / 2 + 5, this.Height * 1 / 32);
            lbl_name_place.Location = new Point((int)(this.Width * 0.98) - lbl_name_place.Width - 10, this.Height * 1 / 32);
            lbl_place.Location = new Point((int)(this.Width * 0.98) - lbl_place.Width - lbl_name_place.Width - 10, this.Height * 1 / 32);
            InitializeFPSThread();
            InitializeHUD();
            tr.SetD(10);
            Load_Game();
        }

        private void DisplayLoading()
        {
            Label lbl_loading = new Label();
            lbl_loading.AutoSize = true;
            lbl_loading.BackColor = System.Drawing.Color.Black;
            lbl_loading.ForeColor = System.Drawing.Color.White;
            lbl_loading.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_loading.Name = "lbl_loading";
            lbl_loading.TabIndex = 0;
            lbl_loading.Tag = 1;
            lbl_loading.Text = "Chargement...";
            lbl_loading.Location = new System.Drawing.Point(this.Width / 2 - lbl_loading.Width / 2, this.Height / 2 - lbl_loading.Height / 2);
            lbl_loading.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Controls.Add(lbl_loading);
            Refresh();
        }

        private void DisplayIntro(string scene_name, string scene_intro)
        {
            RemoveControlsToSuppress();
            if(scene_name != "" && scene_intro != "")
            {
                Label label_intro = new Label();
                label_intro.BackColor = System.Drawing.Color.Black;
                label_intro.ForeColor = System.Drawing.Color.White;
                label_intro.Size = new System.Drawing.Size(384, 20);
                label_intro.TabIndex = 2;
                label_intro.Text = scene_intro;
                label_intro.AutoSize = true;
                label_intro.BorderStyle = BorderStyle.None;
                label_intro.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label_intro.Location = new Point(this.Width / 2 - label_intro.Width / 2, this.Height / 2 - label_intro.Height / 2);
                Button btn_continue = new Button();
                btn_continue.Click += new EventHandler(delegate (object sender, EventArgs e) { 
                    RemoveAllControls();
                    this.Focus();
                    this.BackColor = Color.White;
                    lbl_nfps.Visible = true;
                    lbl_nscore.Visible = true;
                    lbl_score.Visible = true;
                    lbl_name_place.Visible = true;
                    lbl_place.Visible = true;
                    tr.EndTransition();
                    timer_game.Start();
                });
                btn_continue.Name = "btn_continue";
                btn_continue.Size = new Size(100, 30);
                btn_continue.Location = new Point(this.Width/2 - btn_continue.Width/2, label_intro.Location.Y + label_intro.Height + 64);
                btn_continue.BackColor = System.Drawing.Color.Black;
                btn_continue.ForeColor = System.Drawing.Color.White;
                btn_continue.UseVisualStyleBackColor = false;
                btn_continue.Text = "Continuer";
                this.Controls.Add(label_intro);
                this.Controls.Add(btn_continue);
            }
            else
            {
                timer_game.Start();
                lbl_nfps.Visible = true;
                lbl_nscore.Visible = true;
                lbl_score.Visible = true;
                lbl_name_place.Visible = false;
                lbl_place.Visible = false;
                this.Focus();
                this.BackColor = Color.White;
                tr.EndTransition();
            }
            Refresh();
        }

        /// <summary>
        /// Load every objects needed 
        /// </summary>

        private void Load_Game()
        {
            if (instance == null) return;
            this.BackColor = Color.Black;
            RemoveAllControls();
            this.Focus();
            StartTransition();
            JObject environment = Tools.Get_From_JSON(this.sc_path + scenario + Path.DirectorySeparatorChar + situation + Path.DirectorySeparatorChar + "environment.json");
            lbl_name_place.Text = environment["scene_name"].ToString();
            int scene_type = (int)environment["scene_type"];
            string background_name = (string)environment["background"];
            if(scene_type < 0 || scene_type > 10)
            {
                MessageBox.Show("Erreur : Le type de scène n'a pas été reconnu.");
                this.Close();
            }
            if(scene_type != 10)
            {
                SetupScene(scene_type);
                DisplayLoading();
                /*while (!tr.HasFinished())
                {
                    Refresh();
                }*/
                list_comments = new List<string>();
                SetScore(800);
                r = new Random();
                InitializeTimer(scene_type);
                InitializeGameTextures(scene_type, background_name);
                DisplayIntro(environment["scene_name"].ToString(), environment["scene_intro"].ToString());
            }
            else
            {
                Form pandc = new PNCWindow(this.Text, scenario, situation);
                //this.Hide();
                pandc.Show();
            }
        }

        private void SetupScene(int scene_type)
        {
            if (scene_type == 0 || scene_type == 4)
            {
                this.MouseDown += new System.Windows.Forms.MouseEventHandler(delegate (object sender, System.Windows.Forms.MouseEventArgs e)
                {
                    if (!ticket_valid && platform.IsTerminalHit(e.Location.X, e.Location.Y))
                    {
                        ticket_valid = true;
                        this.Controls.Add(new GameMessage("Vous avez bien validé votre ticket !"));
                    }
                });
            }
            else if (scene_type == 4) { }//todo : placer bornes dans bus
        }

        /// <summary>
        /// Initialize the thread which calculate the fps.
        /// </summary>

        private void InitializeFPSThread()
        {
            t_fps = new Thread(new ThreadStart(CalculateFPS));
            t_fps.Start();
        }

        /// <summary>
        /// Initialize the transition.
        /// </summary>

        private void InitializeTransition()
        {
            if(tr != null)
            {
                tr.Dispose();
            }
            tr = new Transition(draw_surface_width, draw_surface_height);
        }

        /// <summary>
        /// Start the transition.
        /// </summary>

        private void StartTransition()
        {
            if (tr != null && !list_hud_textures.Contains(tr)) list_hud_textures.Add(tr);
        }

        /// <summary>
        /// Remove the transition on screen.
        /// </summary>

        public void RemoveTransition()
        {
            if (list_hud_textures.Contains(tr)) list_hud_textures.Remove(tr);
        }

        /// <summary>
        /// Initialize the game timer_game.
        /// </summary>

        private void InitializeTimer(int scene_type)
        {
            if (timer_game == null && current_scene_tick == null)
            {
                timer_game = new System.Windows.Forms.Timer();
                timer_game.Tick += new EventHandler(GameTick);
            }
            else
            {
                timer_game.Stop();
                timer_game.Tick -= new EventHandler(current_scene_tick);
            }
            if (scene_type == 0 || scene_type == 4) current_scene_tick = OnPlatform_Tick;
            else if (scene_type == 1 || scene_type == 5) current_scene_tick = InsideVehicule_Tick;
            else if (scene_type == 2 || scene_type == 6) current_scene_tick = VehiculeArrive_Tick;
            else if (scene_type == 3 || scene_type == 7) current_scene_tick = VehiculeCrashed_Tick;
            else if (scene_type == 8) current_scene_tick = Park_Tick;
            else if (scene_type == 9) current_scene_tick = Street_Tick;
            timer_game.Tick += new EventHandler(current_scene_tick);
            timer_game.Interval = 30;
        }

        /// <summary>
        /// Initialize all the textures of the game.
        /// </summary>

        private void InitializeGameTextures(int scene_type, string background_name)
        {
            if (list_game_textures != null && list_game_textures.Count > 0)
            {
                foreach (Texture t in list_game_textures)
                {
                    t.Dispose();
                }
            }
            player = new Player(this.Width/2-96, 650);
            background = new Background(background_name, 0, -372);
            list_game_textures = new List<Texture>();
            if (scene_type == 0 || scene_type == 4)
            {
                if(scene_type == 0) vehicule = new Tram(-4000, 298 + 80);
                else vehicule = new Bus(-4000, 298 + 80);
                platform = new Platform(-100, vehicule.GetY() + vehicule.GetHeight(), vehicule.GetZ() + 2);
                platform.AddChild(player);
                list_game_textures.Add(vehicule);
                list_game_textures.Add(platform);
            }
            else if (scene_type == 1 || scene_type == 5)
            {
                if (scene_type == 0) vehicule = new Tram(-4000, 298 + 80);
                else vehicule = new Bus(-4000, 298 + 80);
                vehicule.AddChild(player);
                player.SetX(vehicule.GetX() + vehicule.GetWidth() / 2 - player.GetWidth() / 2);
                player.SetY(vehicule.GetY() + vehicule.GetHeight() - 202);
                list_game_textures.Add(vehicule);
                vehicule.ChangeInside();
                vehicule.SetState(2);
                PlacePlayerMiddleScreen();
            }
            else if (scene_type == 2 || scene_type == 6)
            {
                if (scene_type == 0) vehicule = new Tram(-4000, 298 + 80);
                else vehicule = new Bus(-4000, 298 + 80);
                platform = new Platform(-100, vehicule.GetY() + vehicule.GetHeight(), vehicule.GetZ() + 2);
                vehicule.AddChild(player);
                player.SetX(vehicule.GetX() + vehicule.GetWidth() / 2 - player.GetWidth() / 2);
                list_game_textures.Add(vehicule);
                list_game_textures.Add(platform);
            }
            else if (scene_type == 3 || scene_type == 7)
            {
                if (scene_type == 0) vehicule = new Tram(-4000, 298 + 80);
                else vehicule = new Bus(-4000, 298 + 80);
                vehicule.AddChild(player);
                player.SetX(vehicule.GetX() + vehicule.GetWidth() / 2 - player.GetWidth() / 2);
                player.SetY(vehicule.GetY() + vehicule.GetHeight() - 202);
                list_game_textures.Add(vehicule);
                vehicule.ChangeInside();
                vehicule.SetState(2);
                PlacePlayerMiddleScreen();
            }
            else if (scene_type == 8)
            {
                background.EnableCollisions();
                list_game_textures.Add(player);
            }
            else if (scene_type == 9)
            {
                player.SetY(838 - 372);
                background.EnableCollisions();
                list_game_textures.Add(player);
            }
            list_game_textures.Add(background);
            InitializeNPCs(scene_type);
        }

        /// <summary>
        /// Initialize the textures of the hud, like the egg and the backpack icons.
        /// </summary>

        private void InitializeHUD()
        {
            egg = new Egg(12, 10);
            backpack = new Texture("Backpack", 12, 145);
            list_hud_textures = new List<Texture>(){
                backpack,
                egg
            };
        }

        /// <summary>
        /// Setup the window according to the general settings of the application stored in the options.json
        /// </summary>

        private void SetUpWindow()
        {
            JObject options = Tools.Get_From_JSON("internal" + Path.DirectorySeparatorChar + "options.json");
            if ((bool)options["maximized"])
            {
                this.WindowState = FormWindowState.Maximized;
            }
            this.Width = (int)options["size"]["x"];
            this.Height = (int)options["size"]["y"];
        }

        /// <summary>
        /// Show the fps average at this moment.
        /// </summary>
        private void CalculateFPS()
        {
            while (true)
            {
                Thread.Sleep(1000);
                double diff = ((DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds - start_milliseconds)/1000;
                if (ticks > 0 && diff > 0) n_fps = (float)(ticks / diff);
                else Console.WriteLine("Erreur fps");
                ticks = 0;
                start_milliseconds = (DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Instructions executed every tick
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>

        private void GameTick(object sender, EventArgs e)
        {
            ticks++;
            if (ticks % 3 == 0) PassThroughNPCs();
            if (n_fps.ToString().Length > 4) lbl_nfps.Text = n_fps.ToString().Substring(0,4);
            MoveAllCharactersToObjective();
            CheckArrowsPressed();
            RemoveControlsToSuppress();
            Refresh();
        }

        private void OnPlatform_Tick(object sender, EventArgs e)
        {
            PreventPlayerEnteringVehicule();
            ChangeStateIfVehiculeArrived();
            if (vehicule.GetState() == 0)
            {
                ticks_temp++;
                if (ticks_temp == 1)
                {
                    NPCLeaveVehicule(false);
                }
                if (ticks_temp == 35)
                {
                    NPCEnteringVehicule(true);
                }
                if (ticks_temp > 200)
                {
                    vehicule.ChangeState();
                    ticks_temp = 0;
                }
            }
            else if (vehicule.Contains(player) && vehicule.GetX() >= draw_surface_width && tr.HasFinished()) ViewInsideVehicule();
            else if (platform.Contains(player) && vehicule.GetX() >= draw_surface_width)
            {
                ticks_temp++;
                if (ticks_temp == 1)
                {
                    NPCsComingPlatform();
                }
                else if (ticks_temp > 100)
                {
                    ResetVehicule();
                    ticks_temp = 0;
                }
            }
            if (vehicule.GetState() != 2)
            {
                CheckIfCharacterIsEnteringTheVehicule();
                CheckIfCharacterIsLeavingTheVehicule();
            }
            else if (vehicule.Contains(player) && !list_game_textures.Contains(tr) && !IsOnScreen(vehicule))
            {
                StartTransition();
            }
        }

        private void VehiculeArrive_Tick(object sender, EventArgs e)
        {
            ChangeStateIfVehiculeArrived();
            if (vehicule.GetState() == 0)
            {
                ticks_temp++;
                if (ticks_temp == 1)
                {
                    NPCLeaveVehicule(true);
                }
                if (ticks_temp == 35)
                {
                    NPCEnteringVehicule(false);
                }
                if (ticks_temp > 200)
                {
                    vehicule.ChangeState();
                    ticks_temp = 0;
                }
            }
            else if (platform.Contains(player) && vehicule.GetX() >= draw_surface_width)
            {
                ticks_temp++;
                if (ticks_temp == 1)
                {
                    NPCsComingPlatform();
                }
                else if (ticks_temp > 100)
                {
                    ResetVehicule();
                    ticks_temp = 0;
                }
            }
            if (vehicule.GetState() != 2)
            {
                CheckIfCharacterIsEnteringTheVehicule();
                CheckIfCharacterIsLeavingTheVehicule();
            }
            else if (vehicule.Contains(player) && !list_game_textures.Contains(tr) && !IsOnScreen(vehicule))
            {
                StartTransition();
            }
        }

        private void Normal_Tick(object sender, EventArgs e)
        {

        }

        private void InsideVehicule_Tick(object sender, EventArgs e)
        {
            background.Move(-vehicule.GetSpeed(), 0);
        }

        private void Park_Tick(object sender, EventArgs e)
        {
            RespawnNPCParkOutsideScreen();
        }

        private void Street_Tick(object sender, EventArgs e)
        {
            RespawnNPCStreetOutsideScreen();
            RespawnCarsOutsideScreen();
        }

        /// <summary>
        /// Move all the the characters towards its objective if he has any.
        /// </summary>

        private void MoveAllCharactersToObjective()
        {
            foreach (Texture t in GetAllTextures(list_game_textures))
            {
                if ((t.GetType().Name == "Player" || t.GetType().Name == "NPC") && ((Character)t).HasObjective())
                {
                    MoveCharacterToObjective((Character)t);
                }
            }
        }

        /// <summary>
        /// Move the textures if the player reaches the borders of the screen.
        /// </summary>

        private void MoveTexturesIfPlayerMoves()
        {
            if (player.GetX() + player.GetWidth() > draw_surface_width - 9)
            {
                foreach (Texture t in list_game_textures)
                {
                    t.Move(-Character.GetCharacterSpeed(), 0);
                }
            }
            else if (player.GetX() <= 9)
            {
                foreach (Texture t in list_game_textures)
                {
                    t.Move(Character.GetCharacterSpeed(), 0);
                }
            }
        }

        /// <summary>
        /// Check if arrows are pressed
        /// </summary>

        private void CheckArrowsPressed()
        {
            int a = 0;
            int b = 0;

            if (movement[0])
            {
                a -= Character.GetCharacterSpeed();
            }
            if (movement[1])
            {
                a += Character.GetCharacterSpeed();
            }
            if (movement[2])
            {
                b -= Character.GetCharacterSpeed();
            }
            if (movement[3])
            {
                b += Character.GetCharacterSpeed();
            }
            if (a != 0 || b != 0)
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
        /// Move the character toward his objective
        /// </summary>

        private void MoveCharacterToObjective(Character c)
        {
            int a = 0;
            int b = 0;
            if (!c.ReachedObjX())
            {
                if (c.GetObjX() > Character.GetCharacterSpeed())
                {
                    a = Character.GetCharacterSpeed();
                }
                else if (c.GetObjX() < -Character.GetCharacterSpeed())
                {
                    a = -Character.GetCharacterSpeed();
                }
                c.UpdateObjX(-a);
            }
            if (!c.ReachedObjY())
            {
                if (c.GetObjY() > Character.GetCharacterSpeed())
                {
                    b = Character.GetCharacterSpeed();
                }
                else if (c.GetObjY() < -Character.GetCharacterSpeed())
                {
                    b = -Character.GetCharacterSpeed();
                }
                c.UpdateObjY(-b);
            }
            if (c.GetType().Name == "Player") MovePlayer(a, b);
            else
            {
                MoveNPC((NPC)c, a, b);
            }
            if (c.ReachedObjective()) c.RemoveObjective();
        }

        /// <summary>
        /// Check if the vehicule has to slow down.
        /// </summary>

        private void ChangeStateIfVehiculeArrived()
        {
            if(vehicule.GetState() == 2 && (vehicule.GetX()+vehicule.GetWidth() > platform.GetX()+platform.GetWidth()/2+vehicule.GetWidth()/2 - vehicule.GetDistanceMaxStop() && vehicule.GetX() + vehicule.GetWidth() < platform.GetX() + platform.GetWidth() / 2 + vehicule.GetWidth() / 2 - vehicule.GetDistanceMaxStop() + vehicule.GetMaxSpeed()))
            {
                vehicule.ChangeState();
            }
        }

        /// <summary>
        /// Check if a character enters the vehicule, add him as a child of the vehicule and remove it from the platform.
        /// </summary>

        private void CheckIfCharacterIsEnteringTheVehicule()
        {
            Texture t;
            for (int i = platform.GetListChilds().Count - 1; i >= 0; i--)
            {
                t = platform.GetListChilds()[i];
                if (t.GetType().Name == "Player" || t.GetType().Name == "NPC")
                {
                    if (t.GetZ() <= vehicule.GetY() + vehicule.GetHeight() && t.GetZ() >= vehicule.GetY())
                    {
                        vehicule.AddChild(t);
                        platform.RemoveChild(t);
                    }
                }
            }
        }

        /// <summary>
        /// Check if a character leaves the vehicule, remove it from the vehicule and add it to the platform.
        /// </summary>

        private void CheckIfCharacterIsLeavingTheVehicule()
        {
            Texture t;
            for (int i = vehicule.GetListChilds().Count - 1; i >= 0; i--)
            {
                t = vehicule.GetListChilds()[i];
                if (t.GetType().Name == "Player" || t.GetType().Name == "NPC")
                {
                    if (t.GetZ() > vehicule.GetY() + vehicule.GetHeight())
                    {
                        platform.AddChild(t);
                        vehicule.RemoveChild(t);
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
            OptimizeGraphics(e);
            //Initialize the draw_surface_width and draw_surface_height variable
            if (this.draw_surface_width == 0 && this.draw_surface_height == 0)
            {
                this.draw_surface_width = e.ClipRectangle.Width;
                this.draw_surface_height = e.ClipRectangle.Height;
                InitializeTransition();
            }
            if(e.ClipRectangle.Width != this.draw_surface_width || e.ClipRectangle.Height != this.draw_surface_height)
            {
                ResizeGameWindow(e);
            }
            PaintAllTextures(e);
        }

        private void ResizeGameWindow(PaintEventArgs e)
        {
            int diff_y = e.ClipRectangle.Height - this.draw_surface_height;
            this.draw_surface_width = e.ClipRectangle.Width;
            this.draw_surface_height = e.ClipRectangle.Height;
            InitializeTransition();
            foreach (Texture t in list_game_textures)
            {
                PlacePlayerMiddleScreen();
                if (diff_y != 0) t.SetY(t.GetY() + diff_y / 2);
            }
        }

        /// <summary>
        /// Retrieve all the textures, sort them by depth then are paint on the window.
        /// </summary>
        /// <param name="e"></param>

        private void PaintAllTextures(PaintEventArgs e)
        {
            List<Texture> list_all_textures = GetAllTextures(list_game_textures);
            list_all_textures.Sort(Texture.Compare);
            if(list_all_textures != null)
            foreach (Texture t in list_all_textures)
            {
                t.OnPaint(e);
                if (showhitbox)
                {
                    t.Debug(e);
                }
            }
            if(list_hud_textures != null)
            {
                List<Texture> list_hud_temp = new List<Texture>(list_hud_textures);
                foreach (Texture t in list_hud_temp)
                {
                    t.OnPaint(e);
                }
            }
        }

        private void OptimizeGraphics(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
        }

        /// <summary>
        /// Retrieve all the textures and the childs of each textures.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>

        private List<Texture> GetAllTextures(List<Texture> list)
        {
            List<Texture> list_temp = new List<Texture>();
            if(list != null)
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
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>

        private void GameWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int mouse_x = e.Location.X;
            int mouse_y = e.Location.Y;
            if(timer_game.Enabled)
            foreach (Control c in this.Controls)
            {
                if ((mouse_x < c.Location.X || mouse_x > c.Location.X + c.Width) || (mouse_y < c.Location.Y || mouse_y > c.Location.Y + c.Height))
                {
                    RemoveAllControls();
                }
            }
            if (backpack.IsHitboxHit(mouse_x, mouse_y)) OpenClose_Backpack();
            else if (this.Focused && !SearchNPCDialog(mouse_x, mouse_y))
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

        private bool SearchNPCDialog(int mx, int my)
        {
            foreach (NPC t in nm.GetList())
            {
                if (t.IsInteractive() && t.IsHitboxHit(mx, my))
                {
                    if (Math.Abs((t.GetX() + t.GetWidth() / 2 - (player.GetX() + player.GetWidth() / 2))) < 256 && Math.Abs((t.GetY() - player.GetY())) < 256)
                    {
                        t.RemoveAllObjectives();
                        t.SetDefaultPose();
                        this.Controls.Add(new Dialog(t.GetID(), this.Text));
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

        /// <summary>
        /// Check if a character is colliding with the other textures in the game.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private bool IsCharacterCollidingWithTextures(Character c)
        {
            foreach(Texture t in list_game_textures)
            {
                if (t.CollideWith(c,true))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Move the player if there are no collisions.
        /// </summary>
        /// <param name="a"</param>
        /// <param name="b"></param>

        private void MovePlayer(int a, int b)
        {
            bool c_vertical = true;
            bool c_horizontal = true;
            if (!this.Focused) return;
            player.UpdateMovement(a, b);
            if (a != 0)
            {
                MoveAllTexturesExceptPlayer(-a, 0);
                if (IsCharacterCollidingWithTextures(player))
                {
                    MoveAllTexturesExceptPlayer(a, 0);
                    c_vertical = false;
                }
            }
            if (b != 0)
            {
                player.Move(0, b);
                if (IsCharacterCollidingWithTextures(player))
                {
                    player.Move(0, -b);
                    c_horizontal = false;
                }
            }
            if (!c_vertical && !c_horizontal)
            {
                player.RemoveObjective();
            }
        }

        private void PreventPlayerEnteringVehicule()
        {
            if (vehicule != null && platform != null
                    && player.GetY() <= platform.GetY() - player.GetHeight() + 2
                    && ((!ticket_valid && !god)
                    || (player.GetX() < vehicule.GetX() || player.GetX() + player.GetWidth() > vehicule.GetX() + vehicule.GetWidth())))
            {
                player.Move(0, Character.GetCharacterSpeed());
                if (!ticket_valid && this.Controls.Find("GameMessage", true).Length == 0 && IsOnScreen(vehicule))
                {
                    movement[0] = false; movement[1] = false; movement[2] = false; movement[3] = false;
                    this.Controls.Add(new GameMessage("Vous devez d'abord valider votre ticket !"));
                }
            }
        }

        private void MoveAllTexturesExceptPlayer(int a, int b)
        {
            foreach (Texture t in GetAllTextures(list_game_textures))
            {
                if (t != player)
                {
                    t.Move(a, b, false);
                }
                if(t == background && vehicule != null && vehicule.IsInside() && vehicule.GetSpeed()>0)
                {
                    t.Move(-a, -b, false);
                }
            }
        }

        private void MoveNPC(NPC n, int a, int b)
        {
            n.UpdateMovement(a, b);
            n.Move(a, b);
        }

        /// <summary>
        /// Check if the player presses down the keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left : movement[0] = true; break;
                case Keys.Q: movement[0] = true; break;
                case Keys.Right : movement[1] = true; break;
                case Keys.D: movement[1] = true; break;
                case Keys.Up : movement[2] = true; break;
                case Keys.Z: movement[2] = true; break;
                case Keys.Down : movement[3] = true; break;
                case Keys.S: movement[3] = true; break;
                case Keys.B: OpenClose_Backpack(); break;
                case Keys.Escape: this.Close(); break;
            }
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F1: this.god = !god; if (god) player.DisableCollisions(); else player.EnableCollisions(); break;
                    case Keys.F2: this.showhitbox = !showhitbox; break;
                    case Keys.F3: SwitchSituation(); break;
                }
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
                case Keys.Left: movement[0] = false; break;
                case Keys.Q: movement[0] = false; break;
                case Keys.Right: movement[1] = false; break;
                case Keys.D: movement[1] = false; break;
                case Keys.Up: movement[2] = false; break;
                case Keys.Z: movement[2] = false; break;
                case Keys.Down: movement[3] = false; break;
                case Keys.S: movement[3] = false; break;
            }
        }

        /// <summary>
        /// Display the average FPS during the game session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(list_game_textures!=null)
            foreach (Texture t in list_game_textures)
            {
                t.Dispose();
            }
            if (list_hud_textures != null)
            foreach (Texture t in list_hud_textures)
            {
                t.Dispose();
            }
            nm.Clear();
            if(timer_game != null) timer_game.Stop();
            if (t_fps != null && t_fps.IsAlive) t_fps.Abort();
            instance = null;
            if (!preview)
            {
                Application.Restart();
            }
        }

        private void ResetVehicule()
        {
            if (vehicule.GetState() == 2)
            {
                if (vehicule.IsInside())
                {
                    vehicule.ChangeInside();
                    list_game_textures.Remove(player);
                    vehicule.AddChild(player);
                    platform = new Platform(vehicule.GetX(), vehicule.GetY() + vehicule.GetHeight(), vehicule.GetZ() + 2);
                    vehicule.SetSpeed(vehicule.GetMaxSpeed());
                    list_game_textures.Add(platform);
                    FillPlatformNPCs(NPCsDensity/2);
                }
                else vehicule.ShuffleVehiculeNPCs();
                vehicule.SetX(-4000);
                vehicule.SetSpeed(vehicule.GetMaxSpeed());
            }
        }

        public void InitializeNPCs(int scene_type)
        {
            nm.Clear();
            JObject npcs = Tools.Get_From_JSON(this.sc_path + scenario + Path.DirectorySeparatorChar + situation + Path.DirectorySeparatorChar + "dialogs.json");
            NPCsDensity = (int)Tools.Get_From_JSON(this.sc_path + scenario + Path.DirectorySeparatorChar + situation + Path.DirectorySeparatorChar + "environment.json")["npc_density"];
            int npc_x;
            int npc_y;
            string npc_name;
            string npc_folder;
            int npc_quiz;
            NPC n;
            for (int i = 1; i <= int.Parse((string)npcs["events"]); i++)
            {
                npc_x = (int)npcs[i.ToString()]["x"];
                npc_y = (int)npcs[i.ToString()]["y"];
                npc_name = npcs[i.ToString()]["npc"]["name"].ToString();
                npc_folder = npcs[i.ToString()]["npc"]["folder"].ToString();
                npc_quiz = (int)npcs[i.ToString()]["quizz"];
                n = nm.CreateNPC(npc_name, npc_x, npc_y, npc_quiz, npc_folder);
                if (scene_type == 0 || scene_type == 2 || scene_type == 4 || scene_type == 6)
                {
                    if (vehicule.CollideWith(n, false))
                    {
                        n.SetX(vehicule.GetX() + npc_x);
                        n.SetY(vehicule.GetY() + npc_y);
                        vehicule.AddChild(n);
                    }
                    else
                    {
                        n.SetX(platform.GetX() - 192 + r.Next(100, platform.GetWidth() - 100));
                        n.SetY(platform.GetY() - 192 + r.Next(10, platform.GetHeight()));
                        platform.AddChild(n);
                    }
                }
                else if(scene_type == 1 || scene_type == 3 || scene_type == 5 || scene_type == 7)
                {
                    if (vehicule.CollideWith(n, false))
                    {
                        n.SetX(vehicule.GetX() + npc_x);
                        n.SetY(vehicule.GetY() + npc_y);
                        vehicule.AddChild(n);
                    }
                    else
                    {
                        vehicule.PlaceNPCRandomlyInVehicle(n);
                    }
                }
                else if (scene_type == 8 || scene_type == 9)
                {
                    list_game_textures.Add(n);
                }
            }
            if(scene_type == 0 || scene_type == 2 || scene_type == 4 || scene_type == 6)
            {
                FillVehiculeNPCs(NPCsDensity / 2);
                FillPlatformNPCs(NPCsDensity / 2);
            }
            else if (scene_type == 1 || scene_type == 5)
            {
                FillVehiculeNPCs(NPCsDensity);
                nm.MakeAllNPCsInteractives();
            }
            else if (scene_type == 3 || scene_type == 7)
            {
                FillVehiculeNPCs(NPCsDensity);
            }
            else if(scene_type == 8)
            {
                FillParkNPCs(NPCsDensity);
                nm.MakeAllNPCsInteractives();
            }
            else if (scene_type == 9)
            {
                FillStreetNPCs(NPCsDensity);
                FillStreetCars(4);
                nm.MakeAllNPCsInteractives();
            }
        }

        private void NPCLeaveVehicule(bool affect_interactive_npcs)
        {
            int i;
            NPC n;
            foreach (Texture t in vehicule.GetListChilds())
            {
                if(t.GetType().Name == "NPC" && t.GetName() != "Conducteur")
                {
                    n = (NPC)t;
                    if((affect_interactive_npcs && n.GetQuiz() >= 1))
                    {
                        i = vehicule.GetIndexNearestDoor(n.GetX());
                        n.SetObjectiveX(vehicule.GetPosDoor(i));
                        n.SetObjectiveY(vehicule.GetY() + vehicule.GetHeight());
                        n.SetObjective(n.GetX() + n.GetWidth() / 2 + r.Next(-1024, 1014), r.Next(0, platform.GetHeight()-n.GetHeight()-20));
                    }
                    else if (r.Next(0, 3) != 0 && n.GetQuiz() < 1)
                    {
                        i = vehicule.GetIndexNearestDoor(n.GetX());
                        n.SetObjectiveX(vehicule.GetPosDoor(i));
                        n.SetObjectiveY(vehicule.GetY() + vehicule.GetHeight() + r.Next(0, 100));
                        n.SetObjective(n.GetX() + n.GetWidth() / 2 + r.Next(-1024, 1014), 5000);
                    }
                }
            }
        }

        private void ViewInsideVehicule()
        {
            ChangeCurrentTick(InsideVehicule_Tick);
            platform.Dispose();
            list_game_textures.Remove(platform);
            vehicule.ChangeInside();
            vehicule.SetState(2);
            PlacePlayerMiddleScreen();
            tr.EndTransition();
        }

        private void ChangeCurrentTick(EventHandler eh)
        {
            timer_game.Tick -= new EventHandler(current_scene_tick);
            timer_game.Tick += new EventHandler(eh);
            current_scene_tick = eh;
        }

        private void PlacePlayerMiddleScreen()
        {
            int px = player.GetX() + player.GetWidth() / 2 - draw_surface_width/2;
            player.SetX(draw_surface_width / 2 - player.GetWidth() / 2);
            MoveAllTexturesExceptPlayer(-px,0);
        }

        private void NPCEnteringVehicule(bool affect_interactive_npcs)
        {
            foreach (Texture t in platform.GetListChilds())
            {
                if (t.GetType().Name == "NPC" && !((NPC)t).HasObjective())
                {
                    if (!affect_interactive_npcs && ((NPC)t).GetQuiz() >= 1) break;
                    vehicule.SetPathNPCToVehicule((NPC)t);
                }
            }
        }

        private void FillVehiculeNPCs(int density)
        {
            int max = density;
            for(int i = 0; i < max; i++)
            {
                vehicule.PlaceNPCRandomlyInVehicle(NPC_Manager.GetInstance().CreateNPC(0, 0));
            }
        }

        private void FillPlatformNPCs(int density)
        {
            int max = density;
            int x;
            int y;
            for (int i = 0; i < max; i++)
            {
                x = platform.GetX()-192 + r.Next(100, platform.GetWidth()-100);
                y = platform.GetY()-192 + r.Next(10, platform.GetHeight());
                platform.AddChild(nm.CreateNPC(x, y));
            }
        }

        private void FillParkNPCs(int density)
        {
            int max = density;
            int x;
            int y;
            NPC n;
            for (int i = 0; i < max; i++)
            {
                x = r.Next(0, draw_surface_width);
                y = r.Next(draw_surface_height * 2/3, draw_surface_height);
                n = nm.CreateNPC(x, y);
                if (r.Next(0, 2) == 0) n.SetObjectiveX(5000);
                else n.SetObjectiveX(-5000);
                list_game_textures.Add(n);
            }
        }

        private void FillStreetNPCs(int density)
        {
            int max = density;
            int x;
            int y;
            NPC n;
            for (int i = 0; i < max; i++)
            {
                x = r.Next(0, draw_surface_width);
                if (i%2 == 0) y = r.Next(1248-192, 1440-192) - 372;
                else y = r.Next(992-192, 1062-192) - 372;
                n = nm.CreateNPC(x, y);
                if (r.Next(0, 2) == 0) n.SetObjectiveX(5000);
                else n.SetObjectiveX(-5000);
                list_game_textures.Add(n);
            }
        }

        private void FillStreetCars(int number)
        {
            int max = number;
            int x;
            int y;
            NPC n;
            for (int i = 0; i < max; i++)
            {
                x = r.Next(0, draw_surface_width);
                if (i % 2 == 0)
                {
                    y = 960 - 372;
                    n = nm.CreateNPC(x, y);
                    n.SetVisible(false);
                    n.AddChild(new Texture("car_red_3","vehicule/cars",x,y));
                    while (IsCharacterCollidingWithTextures(n))
                    {
                        nm.RemoveNPC(n.GetID());
                        n = nm.CreateNPC(x, y);
                        n.SetVisible(false);
                        n.AddChild(new Texture("car_red_3", "vehicule/cars", x, y));
                    }
                    n.SetObjectiveX(-500000);
                }
                else
                {
                    y = 1060 - 372;
                    n = nm.CreateNPC(x, y);
                    n.SetVisible(false);
                    n.AddChild(new Texture("car_red_1", "vehicule/cars", x, y));
                    while (IsCharacterCollidingWithTextures(n))
                    {
                        nm.RemoveNPC(n.GetID());
                        n = nm.CreateNPC(x, y);
                        n.SetVisible(false);
                        n.AddChild(new Texture("car_red_1", "vehicule/cars", x, y));
                    }
                    n.SetObjectiveX(500000);
                }
                list_game_textures.Add(n);
            }
        }

        private void RespawnNPCParkOutsideScreen()
        {
            foreach(NPC n in nm.GetList())
            {
                if (!IsOnScreen(n))
                {
                    n.RemoveAllObjectives();
                    if (n.GetX()+n.GetWidth() <= 0)
                    {
                        n.SetX(draw_surface_width - 1);
                        n.SetY(r.Next(draw_surface_height * 2 / 3, draw_surface_height));
                        n.SetObjectiveX(-500000);
                    }
                    else if(n.GetX() > draw_surface_width)
                    {
                        n.SetX(-n.GetWidth() + 1);
                        n.SetY(r.Next(draw_surface_height * 2 / 3, draw_surface_height));
                        n.SetObjectiveX(500000);
                    }
                }
            }
        }

        private void RespawnNPCStreetOutsideScreen()
        {
            foreach (NPC n in nm.GetList())
            {
                if (!IsOnScreen(n) && n.GetListChilds().Find(x => x.GetName().Contains("car")) == null)
                {
                    n.RemoveAllObjectives();
                    if (n.GetX() + n.GetWidth() <= 0)
                    {
                        n.SetX(draw_surface_width - 1);
                        if (r.Next(0, 2) == 0) n.SetY(r.Next(1248 - 192, 1440 - 192) - 372);
                        else n.SetY(r.Next(992 - 192, 1062 - 192) - 372);
                        n.SetObjectiveX(-500000);
                    }
                    else if (n.GetX() > draw_surface_width)
                    {
                        n.SetX(-n.GetWidth() + 1);
                        if (r.Next(0, 2) == 0) n.SetY(r.Next(1248 - 192, 1440 - 192) - 372);
                        else n.SetY(r.Next(992 - 192, 1062 - 192) - 372);
                        n.SetObjectiveX(500000);
                    }
                }
            }
        }

        private void RespawnCarsOutsideScreen()
        {
            Texture car;
            foreach (NPC n in nm.GetList())
            {
                if (!IsOnScreen(n) && n.GetListChilds().Find(x => x.GetName().Contains("car")) != null)
                {
                    car = n.GetListChilds().Find(x => x.GetName().Contains("car"));
                    n.RemoveAllObjectives();
                    if (car.GetX() + car.GetWidth() <= 0)
                    {
                        n.SetX(draw_surface_width - 1);
                        n.SetY(960 - 372);
                        n.SetObjectiveX(-500000);
                    }
                    else if (car.GetX() > draw_surface_width)
                    {
                        n.SetX(-n.GetWidth() + 1);
                        n.SetY(1060 - 372);
                        n.SetObjectiveX(500000);
                    }
                }
            }
        }

        private void NPCsComingPlatform()
        {
            int x;
            int y;
            NPC n;
            foreach(Texture t in platform.GetListChilds())
            {
                if(t.GetType().Name == "NPC" && t.GetZ() > draw_surface_height)
                {
                    n = (NPC)t;
                    n.RemoveAllObjectives();
                    x = platform.GetX() + r.Next(400, platform.GetWidth() - 400);
                    y = draw_surface_height + r.Next(0, 500);
                    n.SetX(x);
                    n.SetY(y);
                    n.SetObjective(n.GetX() + r.Next(-400, 400), platform.GetY() + r.Next(30, platform.GetHeight() - 30));
                }
            }
        }

        public void SetScore(int s)
        {
            if (step_score < 0)
            {
                step_score = s / 5;
                score = s;
            }
            else
            {
                score += s;
            }
            this.Controls.Find("lbl_nscore", true)[0].Text = score.ToString();
            if (egg != null && score / step_score >= 0 && score / step_score < 6)
            {
                egg.SetD(5 - score / step_score);
            }
            else if (score / step_score < 0) egg.SetD(5);
        }

        public void OpenClose_Backpack()
        {
            if (this.Controls.Find("Backpack", true).Length == 0)
            {
                this.Controls.Add(new Backpack());
            }
            else
            {
                RemoveAllControls();
                this.Focus();
            }
        }

        public void PassThroughNPCs()
        {
            int d = player.GetDirection();
            foreach(NPC n in nm.GetList())
            {
                if (Math.Abs(n.GetZ() - player.GetZ()) <= 8 && d == 1
                    && n.GetX() + n.GetWidth() / 2 > player.GetX() + player.GetWidth() / 2
                    && n.GetX() + n.GetWidth() / 2 < player.GetX() + player.GetWidth() / 2 + 50)
                {
                    int m;
                    if (r.Next(0, 2) == 0)
                    {
                        m = -(int)(1.5 * Character.GetCharacterSpeed());
                    }
                    else
                    {
                        m = (int)(1.5 * Character.GetCharacterSpeed());
                    }
                    n.Move(0, m);
                    if (vehicule != null && vehicule.CollideWith(n, false))
                    {
                        n.Move(0, -2 * m);
                        if (vehicule.CollideWith(n, false))
                        {
                            n.Move(0, m);
                        }
                    }
                }
                else if (Math.Abs(n.GetZ() - player.GetZ()) <= 8 && d == 3
                && n.GetX() + n.GetWidth() / 2 < player.GetX() + player.GetWidth() / 2
                && n.GetX() + n.GetWidth() / 2 > player.GetX() + player.GetWidth() / 2 - 50)
                {
                    int m;
                    if (r.Next(0, 2) == 0)
                    {
                        m = -(int)(1.5 * Character.GetCharacterSpeed());
                    }
                    else
                    {
                        m = (int)(1.5 * Character.GetCharacterSpeed());
                    }
                    n.Move(0, m);
                    if (vehicule != null && vehicule.CollideWith(n, false))
                    {
                        n.Move(0, -2 * m);
                        if (vehicule.CollideWith(n, false))
                        {
                            n.Move(0, m);
                        }
                    }
                }
            }
        }

        public void SwitchSituation()
        {
            if (preview) this.Close();
            else
            {
                n_situation++;
                if (Directory.GetDirectories(@"" + sc_path + scenario).Length == n_situation)
                {
                    StartTransition();
                    timer_game.Tick += new EventHandler(EndTimer);
                }
                else
                {
                    situation = Directory.GetDirectories(@"" + sc_path + scenario)[n_situation].Remove(0, sc_path.Length + scenario.Length + 1);
                    Load_Game();
                }
            }
        }

        public void EndTimer(object sender, EventArgs e)
        {
            //todo : afficher commentaires
            if (tr.HasFinished())
            {
                RemoveAllControls();
                //this.Controls.Add();
                this.Close();
            }
        }

        private void GameWindow_Resize(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "lbl_nfps":
                        c.Location = new Point((int)(this.Width * 0.93), this.Height * 3 / 32);
                        break;
                    case "lbl_score":
                        c.Location = new Point(this.Width / 2 - c.Width - 5, this.Height * 1 / 32);
                        break;
                    case "lbl_nscore":
                        c.Location = new Point(this.Width / 2 + 5, this.Height * 1 / 32);
                        break;
                    case "lbl_place":
                        c.Location = new Point((int)(this.Width * 0.93) - c.Width, this.Height * 1 / 32);
                        break;
                    case "lbl_name_place":
                        c.Location = new Point((int)(this.Width * 0.93), this.Height * 1 / 32);
                        break;
                    default:
                        break;
                }
            }
            Refresh();
        }

        private void VehiculeCrashed_Tick(object sender, EventArgs e)
        {
            ticks_temp++;
            int b;
            if(ticks_temp > 150)
            {
                if (ticks_temp % 2 == 0)
                {
                    b = 16;
                }
                else
                {
                    b = -16;
                }
                foreach (Texture t in list_game_textures)
                {
                    if (t.GetType().Name != "Transition") t.Move(0, b);
                }
                if (ticks_temp == 150 + 16)
                {
                    tr.SetD(10);
                    StartTransition();
                }
                else if (ticks_temp == 150 + 64)
                {
                    vehicule.SetSpeed(0);
                    vehicule.SetState(0);
                    MessageBox.Show("Tout le monde va bien ?");
                    ChangeCurrentTick(Normal_Tick);
                    nm.MakeAllNPCsInteractives();
                    tr.EndTransition();
                }
            }
            background.Move(-vehicule.GetSpeed(), 0);
        }

        private bool IsOnScreen(Texture t)
        {
            return (t.GetX() + t.GetWidth() >= 0 && t.GetX() < draw_surface_width && t.GetY() + t.GetHeight() >= 0 && t.GetY() < draw_surface_height);
        }
        public void RemoveAllControls()
        {
            foreach(Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "lbl_nfps":
                        break;
                    case "lbl_score":
                        break;
                    case "lbl_nscore":
                        break;
                    case "lbl_place":
                        break;
                    case "lbl_name_place":
                        break;
                    default:
                        c.Tag = 1;
                        break;
                }
            }
        }

        private void RemoveControlsToSuppress()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                Control c = this.Controls[i];
                if(int.Parse(c.Tag.ToString()) == 1)
                {
                    this.Controls.Remove(c);
                    this.Focus();
                }
            }
        }

        public void AddComment(string comment)
        {
            list_comments.Add(comment);
        }
    }
}
