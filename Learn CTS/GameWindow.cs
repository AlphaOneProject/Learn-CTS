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

        private List<Texture> list_game_textures;
        private List<Texture> list_hud_textures;
        private NPC_Manager nm = NPC_Manager.GetInstance();
        private System.Windows.Forms.Timer timer;
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
        private int ticks = 0;
        private double start_milliseconds = (DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds;
        private Dialog d;
        private Backpack bp;
        private Label lbl_score;
        private Label lbl_nfps;
        private string game;
        private bool showhitbox = false;
        private bool god = false;
        private int ticks_stopped = 0;
        private int NPCsDensity = 0;
        private int score = 0;
        private string sc_path;
        private string scenario;
        private string situation;
        private Random r;
        private bool preview = false;
        private int speed_character = 8;
        private int n_situation = 0;
        private Thread t_fps;
        private float n_fps;
        private bool ticket_valid = false;
        private static GameWindow instance;
        private Transition tr;
        private Texture backpack;
        private Egg egg;

        /// <summary>
        /// Initialize the game window
        /// </summary>

        private GameWindow(string game)
        {
            if (instance == null) instance = this;
            else
            {
                MessageBox.Show("Vous ne pouvez avoir qu'une seule fenêtre de jeu ouverte en même temps.");
                this.Close();
            }
            this.game = game;
            string game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            this.Text = game;
            InitializeComponent();
            DoubleBuffered = true;
            sc_path = game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar;
            if(scenario == null) scenario = Directory.GetDirectories(@"" + sc_path)[0].Remove(0, sc_path.Length);
            if (situation == null) situation = Directory.GetDirectories(@"" + sc_path + scenario)[n_situation].Remove(0, sc_path.Length + scenario.Length + 1);
        }

        /// <summary>
        /// Launch the game with at a scenario
        /// </summary>
        /// <param name="game"></param>
        /// <param name="scenario"></param>

        public GameWindow(string game, string scenario) : this(game)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Launch a game, at a situation in a scenario
        /// </summary>
        /// <param name="game"></param>
        /// <param name="scenario"></param>
        /// <param name="situation"></param>

        public GameWindow(string game, string scenario, string situation) : this(game)
        {
            this.preview = true;
            this.scenario = scenario;
            this.situation = situation;
        }

        /// <summary>
        /// Get the instance of the current game window.
        /// </summary>
        /// <returns></returns>

        public static GameWindow GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// When the form is loading, create a button "Launch", 
        /// initialize the timer and the textures,
        /// then show the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_Load(object sender, EventArgs e)
        {
            SetUpWindow();
            this.BackColor = Color.Black;
            DisplayLoading();
            Texture.InitializePath(game);
            Load_Game();
            this.Controls.Clear();
            InitializeHUD();
            InitializeFPSThread();
            InitializeTimer();
            SetScore(1000);
            this.BackColor = Color.White;
        }

        private void DisplayLoading()
        {
            Label lbl_loading = new Label();
            lbl_loading.AutoSize = true;
            lbl_loading.BackColor = System.Drawing.Color.Black;
            lbl_loading.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_loading.ForeColor = System.Drawing.Color.White;
            lbl_loading.Name = "lbl_score";
            lbl_loading.RightToLeft = System.Windows.Forms.RightToLeft.No;
            lbl_loading.TabIndex = 0;
            lbl_loading.Text = "Chargement...";
            lbl_loading.Location = new System.Drawing.Point(this.Width / 2 - lbl_loading.Width / 2, this.Height / 2 - lbl_loading.Height / 2);
            lbl_loading.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Controls.Add(lbl_loading);
            this.Show();
            Refresh();
        }

        /// <summary>
        /// Load every objects needed 
        /// </summary>

        private void Load_Game()
        {
            r = new Random();
            InitializeListTextures();
        }

        /// <summary>
        /// Initialize the thread which calculate the fps.
        /// </summary>

        private void InitializeFPSThread()
        {
            t_fps = new Thread(new ThreadStart(CalculFPSandM));
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
            if (tr != null && !list_game_textures.Contains(tr)) list_game_textures.Add(tr);
        }

        /// <summary>
        /// Remove the transition on screen.
        /// </summary>

        public void RemoveTransition()
        {
            if (list_game_textures.Contains(tr)) list_game_textures.Remove(tr);
        }

        /// <summary>
        /// Initialize the game timer.
        /// </summary>

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(GameTick);
            timer.Interval = 30;
            timer.Start();
        }

        /// <summary>
        /// Initialize all the textures of the game.
        /// </summary>

        private void InitializeListTextures()
        {
            if (list_game_textures != null && list_game_textures.Count > 0)
            {
                foreach (Texture t in list_game_textures)
                {
                    t.Dispose();
                }
            }
            vehicule = new Tram(-4000, 298+80);
            background = new Background(0, -372);
            background.DisableCollisions();
            player = Player.Construct(600, 604);
            platform = new Platform(-100, vehicule.GetY() + vehicule.GetHeight(), vehicule.GetZ() + 2);
            platform.AddChild(player);
            list_game_textures = new List<Texture>() {
                background,
                vehicule,
                platform
            };
            InitializeNPCs();
        }

        /// <summary>
        /// Initialize the textures of the hud, like the egg and the backpack icons.
        /// </summary>

        private void InitializeHUD()
        {
            Load_Controls_HUD();
            egg = new Egg(12, 10);
            backpack = new Texture("Backpack", 12, 145);
            list_hud_textures = new List<Texture>(){
                backpack,
                egg
            };
        }

        private void Load_Controls_HUD()
        {
            this.Controls.Clear();
            bp = new Backpack();
            lbl_score = new System.Windows.Forms.Label();
            lbl_nfps = new System.Windows.Forms.Label();
            lbl_score.AutoSize = true;
            lbl_score.BackColor = System.Drawing.Color.Black;
            lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_score.ForeColor = System.Drawing.Color.White;
            lbl_score.Location = new System.Drawing.Point(12, 111);
            lbl_score.Name = "lbl_score";
            lbl_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            lbl_score.Size = new System.Drawing.Size(96, 31);
            lbl_score.TabIndex = 0;
            lbl_score.Text = "default";
            lbl_score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lbl_nfps.Anchor = System.Windows.Forms.AnchorStyles.None;
            lbl_nfps.AutoEllipsis = true;
            lbl_nfps.AutoSize = true;
            lbl_nfps.BackColor = System.Drawing.Color.Black;
            lbl_nfps.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_nfps.ForeColor = System.Drawing.Color.White;
            lbl_nfps.Location = new System.Drawing.Point(1170, 9);
            lbl_nfps.Name = "lbl_nfps";
            lbl_nfps.RightToLeft = System.Windows.Forms.RightToLeft.No;
            lbl_nfps.Size = new System.Drawing.Size(82, 31);
            lbl_nfps.TabIndex = 4;
            lbl_nfps.Text = "0,000";
            lbl_nfps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lbl_nfps.Visible = false;
            this.Controls.Add(lbl_nfps);
            this.Controls.Add(lbl_score);
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
        private void CalculFPSandM()
        {
            while (true)
            {
                Thread.Sleep(250);
                double diff = ((DateTime.Now - new DateTime(2019, 1, 1)).TotalMilliseconds - start_milliseconds) / 250;
                if (ticks > 0 && diff > 0) n_fps = (float)(ticks / diff)*4;
                else Console.WriteLine("Erreur fps");
                ticks = 0;
                if((int)Math.Round(n_fps / 8)>1) Character.SetM((int)Math.Round(n_fps/8));
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
            if (ticks % 3 == 0) Moise();
            if (!vehicule.IsInside())
            {
                CheckIfTheVehiculeIsArrived();
                if (player.GetX()>=8 && player.GetX()+player.GetWidth()<=draw_surface_width - 8 && !vehicule.Contains(player)) MoveTexturesIfPlayerMoves();
                if (vehicule.GetState() == 0)
                {
                    ticks_stopped++;
                    if (ticks_stopped == 1)
                    {
                        NPCLeaveVehicule();
                    }
                    if (ticks_stopped == 35)
                    {
                        NPCEnteringVehicule();
                    }
                    if (ticks_stopped > 200)
                    {
                        vehicule.ChangeState();
                        ticks_stopped = 0;
                    }
                }
                else if (vehicule.Contains(player) && vehicule.GetX() >= draw_surface_width && tr.HasFinished()) ViewInside();
                else if (platform.Contains(player) && vehicule.GetX() >= draw_surface_width)
                {
                    ticks_stopped++;
                    if (ticks_stopped == 1)
                    {
                        NPCsComingPlatform();
                    }
                    else if (ticks_stopped > 100)
                    {
                        StopVehicule();
                        ticks_stopped = 0;
                    }
                }
                if (vehicule.GetState() != 2)
                {
                    CheckIfCharacterIsEnteringTheVehicule();
                    CheckIfCharacterIsLeavingTheVehicule();
                }
                else if (vehicule.Contains(player) && !list_game_textures.Contains(tr))
                {
                    StartTransition();
                }
            }
            else
            {
                MoveBackground();
            }
            MoveAllCharactersToObjective();
            CheckArrowsPressed();
            Refresh();
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
                    t.Move(-speed_character, 0);
                }
            }
            else if (player.GetX() <= 9)
            {
                foreach (Texture t in list_game_textures)
                {
                    t.Move(speed_character, 0);
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

            if (go_left)
            {
                a -= speed_character;
            }
            else if (go_right)
            {
                a += speed_character;
            }
            if (go_up)
            {
                b -= speed_character;
            }
            if (go_down)
            {
                b += speed_character;
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
        /// Move the character toward his objective
        /// </summary>

        private void MoveCharacterToObjective(Character c)
        {
            int a = 0;
            int b = 0;
            if (!c.ReachedObjX())
            {
                if (c.GetObjX() > speed_character)
                {
                    a = speed_character;
                }
                else if (c.GetObjX() < -speed_character)
                {
                    a = -speed_character;
                }
                c.UpdateObjX(-a);
            }
            if (!c.ReachedObjY())
            {
                if (c.GetObjY() > speed_character)
                {
                    b = speed_character;
                }
                else if (c.GetObjY() < -speed_character)
                {
                    b = -speed_character;
                }
                c.UpdateObjY(-b);
            }
            if(c.GetType().Name == "Player") MovePlayer(a, b);
            else MoveNPC((NPC)c, a, b);
            if (c.ReachedObjective()) c.RemoveObjective();
        }

        /// <summary>
        /// Open or close the dialog if its already on screen.
        /// </summary>

        public void OpenCloseDialog()
        {
            Refresh();
            if (this.Controls.Contains(d))
            {
                this.Controls.Remove(d);
                this.Focus();
            }
            else
            {
                if(d!=null) this.Controls.Add(d);
            }
            Refresh();
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
            if(n_fps>0) lbl_nfps.Text = n_fps.ToString().Substring(0, 4);
            if (this.draw_surface_width == 0 && this.draw_surface_height == 0)
            {
                this.draw_surface_width = e.ClipRectangle.Width;
                this.draw_surface_height = e.ClipRectangle.Height;
                InitializeTransition();
            }
            if(e.ClipRectangle.Width != this.draw_surface_width || e.ClipRectangle.Height != this.draw_surface_height)
            {
                int diff_y = e.ClipRectangle.Height - this.draw_surface_height;
                this.draw_surface_width = e.ClipRectangle.Width;
                this.draw_surface_height = e.ClipRectangle.Height;
                InitializeTransition();
                foreach (Texture t in list_game_textures)
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
            background.Move(-vehicule.GetSpeed(), 0);
        }

        /// <summary>
        /// Retrieve all the textures, sort them by depth then are paint on the window.
        /// </summary>
        /// <param name="e"></param>

        private void PaintAllTextures(PaintEventArgs e)
        {
            List<Texture> list_all_textures = GetAllTextures(list_game_textures);
            list_all_textures.Sort(Texture.Compare);
            //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
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
            foreach(Texture t in list_hud_textures)
            {
                t.OnPaint(e);
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
            if (!ticket_valid && platform.IsTerminalHit(mouse_x, mouse_y))
            {
                ticket_valid = true;
                MessageBox.Show("Vous avez bien validé votre ticket !");
            }
            if (backpack.IsHitboxHit(mouse_x, mouse_y)) OpenClose_Backpack();
            else if (!this.Controls.Contains(d) && !SearchNPCDialog(mouse_x, mouse_y))
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
            if (vehicule.IsInside())
            {
                foreach (NPC t in nm.GetList())
                {
                    if (t.GetQuiz() > 0 && t.IsHitboxHit(mx, my))
                    {
                        if(Math.Abs((t.GetX()+t.GetWidth()/2 - (player.GetX()+player.GetWidth()/2))) < 256 && Math.Abs((t.GetY() - player.GetY())) < 256)
                        {
                            d = new Dialog(t.GetID(), game);
                            OpenCloseDialog();
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
            foreach(Texture t in list_game_textures)
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
            if (this.Controls.Contains(d) || this.Controls.Contains(bp)) return;
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
                if (IsCharacterCollidingWithTextures(player)
                    || 
                    (b<0
                    && player.GetY() <= platform.GetY() - player.GetHeight() + 2
                    && ((!ticket_valid && !god)
                    || (player.GetX() < vehicule.GetX() || player.GetX()+player.GetWidth() > vehicule.GetX()+vehicule.GetWidth()))))
                {
                    player.Move(0, -b);
                    c_horizontal = false;
                }
            }
            else
            {
                vehicule.Move(-a, 0);
                if(vehicule.GetState()==0) background.Move(-a, 0);
                if (IsCharacterCollidingWithTextures(player))
                {
                    if (vehicule.GetState() == 0) background.Move(a, 0);
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

        private void MoveNPC(NPC c, int a, int b)
        {
            c.UpdateMovement(a, b);
            c.Move(a, b);
            /*c.Move(a, 0);
            if (c.GetQuiz() < 0 && IsOnScreen(c) && (c.CollideWith(player) || c.CollideWith(vehicule)))
            {
                c.Move(-a, 0);
            }
            c.Move(0, b);
            if (c.GetQuiz() < 0 && IsOnScreen(c) && (c.CollideWith(player) || c.CollideWith(vehicule)))
            {
                c.Move(0, -b);
            }*/
        }

        private bool IsOnScreen(Texture t)
        {
            return (t.GetX() + t.GetWidth() > 0 && t.GetX() < draw_surface_width && t.GetY() + t.GetHeight() > 0 && t.GetY() < draw_surface_height);
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
                case Keys.Left : go_left = true; break;
                case Keys.Q: go_left = true; break;
                case Keys.Right : go_right = true; break;
                case Keys.D: go_right = true; break;
                case Keys.Up : go_up = true; break;
                case Keys.Z: go_up = true; break;
                case Keys.Down : go_down = true; break;
                case Keys.S: go_down = true; break;
                case Keys.B: OpenClose_Backpack(); break;
                case Keys.Escape: this.Close(); break;
            }
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F1: this.god = !god; if (god) player.DisableCollisions(); else player.EnableCollisions(); break;
                    case Keys.F2: this.showhitbox = !showhitbox; break;
                    case Keys.F3: this.lbl_nfps.Visible = !this.lbl_nfps.Visible; break;
                    case Keys.F4: this.StartVehiculeCrash(); break;
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
                case Keys.Left: go_left = false; break;
                case Keys.Q: go_left = false; break;
                case Keys.Right: go_right = false; break;
                case Keys.D: go_right = false; break;
                case Keys.Up: go_up = false; break;
                case Keys.Z: go_up = false; break;
                case Keys.Down: go_down = false; break;
                case Keys.S: go_down = false; break;
            }
        }

        /// <summary>
        /// Display the average FPS during the game session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GameWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Texture t in list_game_textures)
            {
                t.Dispose();
            }
            nm.Clear();
            instance = null;
            timer.Stop();
            t_fps.Abort();
            if (!preview)
            {
                Application.Restart();
            }
        }

        private void StopVehicule()
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
                    FillPlatformNPCs();
                }
                else ShuffleVehiculeNPCs();
                vehicule.SetX(-4000);
                vehicule.SetSpeed(vehicule.GetMaxSpeed());
            }
        }

        public void InitializeNPCs()
        {
            JObject npcs = Tools.Get_From_JSON(this.sc_path + scenario + Path.DirectorySeparatorChar + situation + Path.DirectorySeparatorChar + "dialogs.json");
            NPCsDensity = (int)Tools.Get_From_JSON(this.sc_path + scenario + Path.DirectorySeparatorChar + situation + Path.DirectorySeparatorChar + "environment.json")["npc_density"];
            int npc_x;
            int npc_y;
            string npc_name;
            string npc_folder;
            int npc_quiz;
            for (int i = 1; i <= int.Parse((string)npcs["events"]); i++)
            {
                npc_x = (int)npcs[i.ToString()]["x"];
                npc_y = (int)npcs[i.ToString()]["y"];
                npc_name = npcs[i.ToString()]["npc"]["name"].ToString();
                npc_folder = npcs[i.ToString()]["npc"]["folder"].ToString();
                npc_quiz = (int)npcs[i.ToString()]["quizz"];
                if(Tools.Get_From_JSON(this.sc_path + scenario + Path.DirectorySeparatorChar + situation + Path.DirectorySeparatorChar + "environment.json")["scene_type"].ToString() == "tram_entrance"
                    && vehicule != null & vehicule.GetX() + npc_x >= vehicule.GetX() + 492 && vehicule.GetX() + npc_x < vehicule.GetX() + vehicule.GetWidth() - 492 && vehicule.GetY() + npc_y >= vehicule.GetY() + 144 && vehicule.GetY() + npc_y <= vehicule.GetY() + 164)
                {
                    vehicule.AddChild(nm.CreateNPC(npc_name, vehicule.GetX() + npc_x, vehicule.GetY() + npc_y, npc_quiz, npc_folder));
                }
                else
                {
                    npc_x = platform.GetX() - 192 + r.Next(100, platform.GetWidth() - 100);
                    npc_y = platform.GetY() - 192 + r.Next(10, platform.GetHeight());
                    platform.AddChild(nm.CreateNPC(npc_name, npc_x, npc_y, npc_quiz, npc_folder));
                }
            }
            FillVehiculeNPCs();
            FillPlatformNPCs();
        }

        private void NPCLeaveVehicule()
        {
            List<NPC> l = SelectionNPCsLeavingVehicule();
            int i;
            foreach (NPC n in l)
            {
                i = vehicule.GetIndexNearestDoor(n.GetX());
                n.SetObjectiveX(vehicule.GetPosDoor(i));
                n.SetObjectiveY(vehicule.GetY() + vehicule.GetHeight() + r.Next(0, 100));
                n.SetObjective(n.GetX() + n.GetWidth() / 2 + r.Next(-1024, 1014), 5000);
            }
        }

        private List<NPC> SelectionNPCsLeavingVehicule()
        {
            List<NPC> list = new List<NPC>();
            for(int i = 0; i< vehicule.GetListChilds().Count; i++)
            {
                if(vehicule.GetListChilds()[i].GetType().Name == "NPC" && ((NPC)vehicule.GetListChilds()[i]).GetQuiz()<1 && r.Next(0, 3) != 0 && vehicule.GetListChilds()[i].GetX()<vehicule.GetX()+vehicule.GetWidth() - 192 - 200)
                {
                    vehicule.GetListChilds()[i].DisableCollisions();
                    list.Add((NPC)vehicule.GetListChilds()[i]);
                } 
            }
            return list;
        }

        private void ViewInside()
        {
            platform.Dispose();
            list_game_textures.Remove(platform);
            vehicule.ChangeInside();
            vehicule.SetState(2);
            PlacePlayerMiddleScreen();
            list_game_textures.Add(player);
            tr.EndTransition();
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

        private void NPCEnteringVehicule()
        {
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
                    n.SetObjectiveY(platform.GetY() - r.Next(24, 30) + n.GetZ() - y);
                    if (i == 0)
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-64, 256));
                    }
                    else if (i == vehicule.GetNumberDoors() - 1)
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, 64));
                    }
                    else if (i%2 == 0)
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-384, 256));
                    }
                    else
                    {
                        n.SetObjectiveX(n.GetX() + n.GetWidth() / 2 + r.Next(-256, 384));
                    }
                }
            }
        }

        private void FillVehiculeNPCs()
        {
            int max = NPCsDensity / 2;
            int x;
            int y;
            for(int i = 0; i < max; i++)
            {
                x = vehicule.GetX() + r.Next(492, vehicule.GetWidth() - 492);
                y = vehicule.GetY() + 144 + r.Next(0, 19);
                vehicule.AddChild(nm.CreateNPC(x,y));
            }
            NPC conductor = nm.CreateNPC(vehicule.GetX() + vehicule.GetWidth() - 192 - 100, vehicule.GetY() + vehicule.GetHeight() - 192 - 10);
            conductor.SetDirection(1);
            conductor.SetDefaultPose();
            conductor.SetZ(vehicule.GetZ()-1);
            vehicule.AddChild(conductor);
        }

        private void FillPlatformNPCs()
        {
            int max = NPCsDensity / 2;
            int x;
            int y;
            for (int i = 0; i < max; i++)
            {
                x = platform.GetX()-192 + r.Next(100, platform.GetWidth()-100);
                y = platform.GetY()-192 + r.Next(10, platform.GetHeight());
                platform.AddChild(nm.CreateNPC(x, y));
            }
        }

        private void ShuffleVehiculeNPCs()
        {
            int x;
            int y;
            NPC n;
            foreach (Texture t in vehicule.GetListChilds())
            {
                if(t.GetType().Name == "NPC" && t.GetY() < vehicule.GetY() + vehicule.GetHeight() - 192 - 20)
                {
                    n = (NPC)t;
                    x = vehicule.GetX() + r.Next(492, vehicule.GetWidth() - 492);
                    y = vehicule.GetY() + 144 + r.Next(0, 19);
                    n.SetX(x);
                    n.SetY(y);
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
            score += s;
            lbl_score.Text = score.ToString();
            if(egg != null && score/200>=0 && score / 200 < 6)
            {
                egg.SetD(5 - score / 200);
            }
        }

        public void OpenClose_Backpack()
        {
            Refresh();
            if (!this.Controls.Contains(bp))
            {
                this.Controls.Add(bp);
            }
            else
            {
                this.Controls.Remove(bp);
                this.Focus();
                Refresh();
            }
        }

        public void Moise()
        {
            int d = player.GetDirection();
            NPC n;
            int e = 8;
            foreach(Texture t in nm.GetList())
            {
                if (t.GetType().Name == "NPC" )
                {
                    n = (NPC)t;
                    if(d == 1 
                    && n.GetX() + n.GetWidth()/2 > player.GetX() + player.GetWidth()/2
                    && n.GetX() + n.GetWidth() / 2 < player.GetX() + player.GetWidth()/2 + 50
                    && Math.Abs(n.GetZ() - player.GetZ()) <= 8)
                    {
                        n.Move(0, -e);
                        if (n.CollideWith(vehicule))
                        {
                            n.Move(0, 2 * e);
                            if (n.CollideWith(vehicule))
                            {
                                n.Move(0, -2 * e);
                            }
                            else
                            {
                                n.Move(0, -e);
                            }
                        }
                        /*if(n.GetZ() >= vehicule.GetY()+vehicule.GetHeight() - 12 || n.GetZ() <= vehicule.GetY() + vehicule.GetHeight() - 8)
                        {
                            n.SetObjectiveY(n.GetZ() - e);
                        }
                        else if (n.GetZ() <= vehicule.GetY() + vehicule.GetHeight() - 32)
                        {
                            n.SetObjectiveY(n.GetZ() + e);
                        }
                        else
                        {
                            if (r.Next(0, 2) == 0) n.SetObjectiveY(n.GetZ() - e);
                            else n.SetObjectiveY(n.GetZ() + e);
                        }*/
                    }
                    else if (d == 3
                    && n.GetX() + n.GetWidth() / 2 < player.GetX() + player.GetWidth() / 2
                    && n.GetX() + n.GetWidth() / 2 > player.GetX() + player.GetWidth() / 2 - 50
                    && Math.Abs(n.GetZ() - player.GetZ()) < 8)
                    {
                        n.Move(0, -e);
                        if (n.CollideWith(vehicule))
                        {
                            n.Move(0, 2 * e);
                            if (n.CollideWith(vehicule))
                            {
                                n.Move(0, -2*e);
                            }
                            else
                            {
                                n.Move(0, -e);
                            }
                        }
                        /*if (n.GetZ() >= vehicule.GetY() + vehicule.GetHeight() - 12 || n.GetZ() <= vehicule.GetY() + vehicule.GetHeight() - 8)
                        {
                            n.SetObjectiveY(n.GetZ() - e);
                        }
                        else if (n.GetZ() <= vehicule.GetY() + vehicule.GetHeight() - 32)
                        {
                            n.SetObjectiveY(n.GetZ() + e);
                        }
                        else
                        {
                            if (r.Next(0, 2) == 0) n.SetObjectiveY(n.GetZ() - e);
                            else n.SetObjectiveY(n.GetZ() + e);
                        }*/
                    }
                }
            }
        }

        public void SwitchSituation()
        {
            n_situation++;
            if(Directory.GetDirectories(@"" + sc_path + scenario).Length == n_situation)
            {
                this.Close();
            }
            else
            {
                situation = Directory.GetDirectories(@"" + sc_path + scenario)[n_situation].Remove(0, sc_path.Length + scenario.Length + 1);
                Load_Game();
            }
        }

        private void GameWindow_Resize(object sender, EventArgs e)
        {
            if (this.Controls.Contains(lbl_nfps)) lbl_nfps.Location = new Point(Width - lbl_nfps.Width - 30, 10);
            if (this.Controls.Contains(bp)) bp.Location = new Point(this.Width / 2 - bp.Width / 2, this.Height / 2 - bp.Height / 2);
            Refresh();
        }

        private void StartVehiculeCrash()
        {
            if (!vehicule.IsInside()) return;
            timer.Stop();
            timer.Tick -= new EventHandler(GameTick);
            timer.Tick += new EventHandler(TimerCrash);
            timer.Start();
        }

        private void TimerCrash(object sender, EventArgs e)
        {
            ticks_stopped++;
            int b;
            if (ticks_stopped % 2 == 0)
            {
                b = 16;
            }
            else
            {
                b = -16;
            }
            foreach(Texture t in list_game_textures)
            {
                if(t.GetType().Name != "Transition") t.Move(0, b);
            }
            if(ticks_stopped >= 16)
            {
                tr.SetD(10);
                StartTransition();
            }
            if (ticks_stopped ==64)
            {
                vehicule.SetSpeed(0);
                vehicule.SetState(0);
                MessageBox.Show("Tout le monde va bien ?");
                timer.Tick += new EventHandler(GameTick);
                timer.Tick -= new EventHandler(TimerCrash);
                tr.EndTransition();
            }
            MoveBackground();
            Refresh();
        }

        private void DisplayTutorial()
        {

        }

        private void DisplaySettings()
        {
            timer.Stop();
            ControlsSettings();
        }

        private void ControlsSettings()
        {
            this.SuspendLayout();
            Panel p_settings = new Panel();
            p_settings.BackColor = Color.Transparent;
            p_settings.Size = new Size(this.Width, this.Height);
            p_settings.Resize += new EventHandler(delegate (object sender, EventArgs e) { this.Size = new Size(this.Width, this.Height); });
            Button btn_continue = new Button();
            btn_continue.Click += new EventHandler(delegate(object sender, EventArgs e) { this.Controls.Remove(p_settings); timer.Start(); });
            btn_continue.Size = new Size(100, 30);
            btn_continue.Location = new Point(200, 300);
            btn_continue.BackColor = Color.White;
            Button btn_settings = new Button();
            btn_settings.Size = btn_continue.Size;
            btn_settings.Location = new Point(btn_continue.Location.X, btn_continue.Location.Y + btn_continue.Height + 20);
            btn_settings.BackColor = Color.White;
            Button btn_leave = new Button();
            btn_leave.Size = btn_continue.Size;
            btn_leave.Location = new Point(btn_continue.Location.X, btn_settings.Location.Y + btn_settings.Height + 20);
            btn_leave.Click += new EventHandler(delegate(object sender, EventArgs e) { this.Close(); });
            btn_leave.BackColor = Color.White;
            p_settings.Controls.AddRange(new Control[3] { btn_continue, btn_settings, btn_leave });
            this.Controls.Add(p_settings);
            this.ResumeLayout();
        }
    }
}
