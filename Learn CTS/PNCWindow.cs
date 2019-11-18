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

        //TODO : Ajouter un endroit par scénario et le remplir de personnages selon le scénario

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
            DoubleBuffered = false;
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
            Item h1 = item_manager.CreateItem("Door", 23, 24);
            list_textures = new List<Texture>
            {
                h1
            };
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
            if (item_manager.GetItemOnPoint(mouse_x, mouse_y) != null)
            {
                HighlightItem(item_manager.GetItemOnPoint(mouse_x, mouse_y));
            }
        }

        private void HighlightItem(Item item)
        {
            // Panel used to darken the background elements
            Panel hg_pnl_color = new Panel()
            {
                Location = new Point(0, 0),
                Width = this.Width,
                Height = this.Height,
                Name = "hg_pnl_color",
                BackColor = Color.FromArgb(80, 0, 0, 0)
            };
            hg_pnl_color.MouseDown += new MouseEventHandler(DisableHighlight);

            // PictureBox to preview the item
            PictureBox hg_pb_item = new PictureBox()
            {
                BackgroundImage = item.GetImage(),
                Parent = hg_pnl_color,
                Name = "hg_pb_item",
                Height = item.GetImage().Height,
                Width = item.GetImage().Width
            };
            hg_pb_item.Location = new Point((this.Width / 2) - (hg_pb_item.Width * 2), (this.Height / 2) - (hg_pb_item.Height / 2));

            // Label to view the item description
            Label hg_lbl_desc = new Label()
            {
                Text = item.GetDescription(),
                BackColor = Color.Transparent,
                Name = "hg_lbl_desc",
                Parent = hg_pnl_color,
                AutoSize = true,
                MaximumSize = new Size(this.Width - ((this.Width / 2) + (this.Width / 20)), this.Height - hg_pb_item.Location.Y),
            };
            hg_lbl_desc.Location = new Point((this.Width / 2) + (this.Width / 20), (this.Height / 2) + (hg_lbl_desc.Height / 2));
            hg_lbl_desc.MouseDown += new MouseEventHandler(DisableHighlight);

            this.Controls.AddRange(new Control[] { hg_pb_item, hg_lbl_desc, hg_pnl_color });

        }

        private void DisableHighlight(object sender, MouseEventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "hg_pnl_color":
                        c.Dispose();
                        break;
                    case "hg_pb_item":
                        c.Dispose();
                        break;
                    case "hg_lbl_desc":
                        c.Dispose();
                        break;
                }
            }
        }

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        private void PNCWindow_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                switch (c.Name)
                {
                    case "hg_pnl_color":
                        c.Width = this.Width;
                        c.Height = this.Height;
                        break;
                    case "hg_pb_item":
                        c.Location = new Point((this.Width / 2) - (c.Width * 2), (this.Height / 2) - (c.Height / 2));
                        break;
                    case "hg_lbl_desc":
                        c.MaximumSize = new Size(this.Width - ((this.Width / 2) + (this.Width / 20)), this.Height - Controls.Find("hg_pb_item", true)[0].Location.Y);
                        break;
                }
            }
        }
    }
}
