using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    /// <summary>
    /// Window responsible for a situation of the point and click game mode.
    /// </summary>
    public partial class PNCWindow : Form
    {

        private int DrawSurfaceWidth;
        private int DrawSurfaceHeight;
        private List<Texture> list_textures = new List<Texture>();
        private string game;
        private string game_path;
        private string scenario;
        private string scenario_path;
        private string situation;
        private string situation_path;
        private string library_path;
        private ItemManager item_manager;

        /* Number of items that the player need to valid in order to finish the situation. */
        private int items_left;

        /// <summary>
        /// Constructor of the window.
        /// </summary>
        /// <param name="game">Name of the current game</param>
        /// <param name="scenario">Name of the scenario</param>
        /// <param name="situation">Name of the situation</param>
        public PNCWindow(string game, string scenario ,string situation)
        {
            this.game = game;
            this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            this.Text = game;
            this.library_path = this.game_path + Path.DirectorySeparatorChar + "library";
            this.scenario = scenario;
            this.scenario_path = this.game_path + Path.DirectorySeparatorChar + "scenarios" + Path.DirectorySeparatorChar + scenario;
            this.situation = situation;
            this.situation_path = this.scenario_path + Path.DirectorySeparatorChar + situation;

            item_manager = new ItemManager(game, situation_path);
            DoubleBuffered = true;
            InitializeComponent();
        }

        /// <summary>
        /// Setter for the situation. Updates the path to the situation
        /// </summary>
        /// <param name="situation">Full situation name, with the number</param>
        public void SetSituation(string situation)
        {
            this.situation = situation;
            this.situation_path = this.scenario_path + Path.DirectorySeparatorChar + situation;
        }

        /// <summary>
        /// Initializes the textures on load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PNCWindow_Load(object sender, EventArgs e)
        {
            InitializeListTextures();
            Refresh();
        }

        /// <summary>
        /// Get the items from the situation.
        /// </summary>
        private void InitializeListTextures()
        {
            Texture.InitializePath(game);
            Texture background = FetchBackground();
            list_textures.Add(background); // Adding the background first

            item_manager.GetItemsFromSituation();
            list_textures.AddRange(item_manager.GetList()); // Adding the items from the situation
            Show();
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
        /// Retrieve all the textures, sort them by depth then are paint on the window.
        /// </summary>
        /// <param name="e"></param>
        private void UpdateTextures(PaintEventArgs e)
        {
            List<Texture> list_all_textures = GetAllTextures(list_textures);
            list_all_textures.Sort(Texture.Compare);
            foreach (Texture t in list_all_textures)
            {
                t.OnPaint(e);
            }
        }

        /// <summary>
        /// Retrieve all the textures and the childs of each textures.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>The textures from the list of textures.</returns>
        private List<Texture> GetAllTextures(List<Texture> list)
        {
            List<Texture> list_temp = new List<Texture>();
            foreach (Texture t in list)
            {
                list_temp.Add(t);
                list_temp.AddRange(GetAllTextures(t.GetListChilds()));
            }
            return list_temp;
        }

        /// <summary>
        /// Called when the window is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PNCWindow_MouseDown(object sender, MouseEventArgs e)
        {
            // Checks if the item is clicked, then highlights it. 
            // The items need a hitbox.
            int mouse_x = e.Location.X;
            int mouse_y = e.Location.Y;
            Boolean found = false;
            try
            {
                ItemViewer[] iv_list = Controls.Find("iv", false).OfType<ItemViewer>().ToArray();
                iv_list[0].Exit();
            }
            catch (IndexOutOfRangeException)
            {
                
            } 
            finally
            {
                foreach (Texture t in list_textures)
                {
                    if (t.GetType().Equals(new Item(0, 0, 0).GetType()))
                    {
                        if (!found)
                        {
                            if (t.IsHitboxHit(mouse_x, mouse_y))
                            {
                                HighlightItem((Item)t);
                                found = true;
                            }
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// Creates an ItemViewer to view the item in details.
        /// </summary>
        /// <param name="item">The item to be highlighted.</param>
        private void HighlightItem(Item item)
        {
            ItemViewer iv = new ItemViewer(item.GetID(), item_manager)
            {
                Name = "iv",
                Width = this.Width
            };
            iv.Location = new Point(0, this.Height - 165);
            this.Controls.Add(iv);
        }

        /// <summary>
        /// Called when the window resizes, to have a responsive behavior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PNCWindow_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "iv":
                        c.Width = this.Width;
                        c.Location = new Point(0, this.Height - 165);
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the number of items that needs to be validated in order to complete the situation.
        /// Calls the GameWindow to switch situation if all the items are validated.
        /// </summary>
        /// <param name="n">Number of items</param>
        public void setItemsLeft(int n)
        {
            this.items_left = n;
            if (n == 0)
            {
                GameWindow.GetInstance().SwitchSituation();
            }
        }

        /// <summary>
        /// Returns the number of items that needs to be validated in order to complete the situation.
        /// </summary>
        /// <returns>Number of items</returns>
        public int getItemsLeft()
        {
            return items_left;
        }

        /// <summary>
        /// Finds which background is used in the environnement.json file, then gets it in the library.
        /// </summary>
        private Texture FetchBackground()
        {
            string bg_name = Tools.Get_From_JSON(situation_path + Path.DirectorySeparatorChar + "environment.json")["background"].ToString();
            Texture bg = new Texture(bg_name, "background", 0, 0, -100);
            return bg;
        }
    }
}
