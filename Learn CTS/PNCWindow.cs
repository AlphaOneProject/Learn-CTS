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
    public partial class PNCWindow : Form
    {

        private int DrawSurfaceWidth;
        private int DrawSurfaceHeight;
        private List<Texture> list_textures;
        private String game;
        private String game_path;
        private ItemManager item_manager;

        public PNCWindow(String game)
        {
            this.game = game;
            this.game_path = this.game_path = System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game + Path.DirectorySeparatorChar;
            this.Text = game;
            item_manager = ItemManager.GetInstance();
            DoubleBuffered = true;
            InitializeComponent();
        }

        private void PNCWindow_Load(object sender, EventArgs e)
        {
            InitializeListTextures();
            Refresh();
        }

        private void InitializeListTextures()
        {
            Texture.InitializePath(game);
            list_textures = item_manager.GetItemsFromSituation(Tools.Get_From_JSON(game_path + "item_test.json"));
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
        /// <returns></returns>
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

        private void PNCWindow_MouseDown(object sender, MouseEventArgs e)
        {
            int mouse_x = e.Location.X;
            int mouse_y = e.Location.Y;
            foreach (Item t in list_textures)
            {
                if (t.IsHitboxHit(mouse_x, mouse_y)) HighlightItem(t);
            }
        }

        private void HighlightItem(Item item)
        {
            ItemViewer iv = new ItemViewer(item.GetID())
            {
                Name = "iv",
                Width = this.Width
            };
            iv.Location = new Point(0, this.Height - 165);
            this.Controls.Add(iv);
        }

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
    }
}
