using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class GameCard : UserControl
    {
        /**
         * True if this is the card of a default game
         * False otherwise
         */
        private bool defaultGame;
        private readonly String img_path;

        public GameCard()
        {
            InitializeComponent();
            this.img_path = AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            PictureBox pb_play_parent = new PictureBox()
            {
                //Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            pb_play.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-play-btn-x128.png"), 0.5f);
            pb_play.BackColor = Color.Transparent;
            pb_edit.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-edit-btn-x64.png"), 0.5f);
            pb_edit.BackColor = Color.Transparent;
        }

        public String Title
        {
            get
            {
                return this.lbl_title.Text;
            }

            set
            {
                int char_space = 14; // Number of characters that can be seen in the label
                if (value.Length > char_space)
                {
                    this.lbl_title.Text = value.Substring(0, char_space - 3) + "...";
                }
                else
                {
                    this.lbl_title.Text = value;
                }
                // We need to know the game title in order to retrieve the thumbnail.
                Show_Thumbnail(value);
            }
        }

        public String Description
        {
            get
            {
                return this.lbl_description.Text;
            }

            set
            {
                int char_space = 164; // Number of characters that can be seen in the label
                if (value.Length > char_space)
                {
                    this.lbl_description.Text = value.Substring(0, char_space - 3) + "...";
                }
                else
                {
                    this.lbl_description.Text = value;
                }
            }
        }

        public void Show_Thumbnail(String game)
        {
            String thumbnail_path = AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game
                + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images"
                + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + "thumbnail.png";
            try
            {
                pb_thumbnail.BackgroundImage = Image.FromFile(thumbnail_path);
            }
            catch (Exception ex)
            {
                pb_thumbnail.BackgroundImage = null;
            }
            finally
            {
                // Setting the parent of the icons in order to make them transparent.
                pb_play.Parent = pb_thumbnail;
                pb_play.Location = new Point(64 - pb_play.Width/2, 64 - pb_play.Height/2);
                pb_edit.Parent = pb_thumbnail;
                pb_edit.Location = new Point(128 - pb_edit.Width, 0);
                pb_edit.BringToFront();
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.defaultGame;
            }
            set => this.defaultGame = value;
        }

        private void Btn_edit_Click(object sender, EventArgs e)
        {
            Form editor = new Editor(Title);
            editor.Show();
            this.Parent.Parent.Hide();
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

        private void Pb_play_Click(object sender, EventArgs e)
        {

        }

        private void Pb_edit_Click(object sender, EventArgs e)
        {
            Form editor = new Editor(Title);
            editor.Show();
            this.Parent.Parent.Hide();
        }

        private void Pb_play_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (pb_play.BackgroundImage != null) pb_play.BackgroundImage.Dispose();
                pb_play.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-play-btn-x128.png"), 1);
            }
            catch (Exception ex)
            {
                pb_play.BackgroundImage = null;
            }
        }

        private void Pb_play_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (pb_play.BackgroundImage != null) pb_play.BackgroundImage.Dispose();
                pb_play.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-play-btn-x128.png"), 0.7f);
            }
            catch (Exception ex)
            {
                pb_play.BackgroundImage = null;
            }
        }

        private void Pb_edit_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (pb_edit.BackgroundImage != null) pb_edit.BackgroundImage.Dispose();
                pb_edit.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-edit-btn-x64.png"), 1);
            }
            catch (Exception ex)
            {
                pb_edit.BackgroundImage = null;
            }
        }

        private void Pb_edit_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (pb_edit.BackgroundImage != null) pb_edit.BackgroundImage.Dispose();
                pb_edit.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-edit-btn-x64.png"), 0.7f);
            }
            catch (Exception ex)
            {
                pb_edit.BackgroundImage = null;
            }
        }
    }
}
