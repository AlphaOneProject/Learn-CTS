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
        private bool isDefaultGame;
        private readonly string img_path;
        private string gameFullName;

        public GameCard()
        {
            InitializeComponent();
            this.img_path = AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            PictureBox pb_play_parent = new PictureBox()
            {
                BackColor = Color.Transparent
            };
            pb_play.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-play-btn-x128.png"), 0.5f);
            pb_play.BackColor = Color.Transparent;
            pb_edit.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-edit-btn-x64.png"), 0.5f);
            pb_edit.BackColor = Color.Transparent;
            pb_delete.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + "gamecard-delete-btn-x64.png"), 0.5f);
            pb_delete.BackColor = Color.Transparent;
        }

        public string Title
        {
            get
            {
                return this.lbl_title.Text;
            }

            set
            {
                this.gameFullName = value;
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

        public string Description
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

        public void Show_Thumbnail(string game)
        {
            string thumbnail_path = AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game
                + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images"
                + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + "thumbnail.png";
            try
            {
                pb_thumbnail.BackgroundImage = Image.FromFile(thumbnail_path);
            }
            catch (Exception)
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
                pb_delete.Parent = pb_thumbnail;
                pb_delete.Location = new Point(128 - pb_delete.Width, 128 - pb_delete.Height);
                pb_edit.BringToFront();
                pb_delete.BringToFront();
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.isDefaultGame;
            }
            set => this.isDefaultGame = value;
        }

        private void Btn_edit_Click(object sender, EventArgs e)
        {
            ((Menu)this.ParentForm).Save_Options();
            Editor editor = new Editor(Title);
            editor.Show();
            this.ParentForm.Hide();
        }

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            try
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
            catch (FileNotFoundException)
            {
                MessageBox.Show("L'image " + img.ToString() + " est introuvable. Vérifiez qu'elle existe.");
                return null;
            }
        }

        private void Pb_play_Click(object sender, EventArgs e)
        {
            Form game = new GameWindow(this.gameFullName);
            game.Show();
            this.Parent.Parent.Hide();
        }

        private void Pb_edit_Click(object sender, EventArgs e)
        {
            Form editor = new Editor(this.gameFullName);
            editor.Show();
            this.Parent.Parent.Hide();
        }

        private void Pb_delete_Click(object sender, EventArgs e)
        {
            if (IsDefault)
            {
                MessageBox.Show("Vous ne pouvez pas supprimer un jeu démo.");
            }
            else
            {
                if ((MessageBox.Show("Confirmer la suppression du jeu " + this.gameFullName + " ?", "Confirmation de suppression",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes))
                {
                    Delete_Game();
                    this.Parent.Controls.Remove(this);
                }
            }            
        }

        private void Delete_Game()
        {
            Directory.Delete(@"" + System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + this.gameFullName, true);
        }

        private void Pb_Btn_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            try
            {
                if (pb.BackgroundImage != null) pb.BackgroundImage.Dispose();
                pb.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + pb.ImageLocation), 1);
            }
            catch (FileNotFoundException)
            {
                pb.BackgroundImage = null;
            }
        }

        private void Pb_Btn_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            try
            {
                if (pb.BackgroundImage != null) pb.BackgroundImage.Dispose();
                pb.BackgroundImage = ChangeOpacity(Image.FromFile(img_path + pb.ImageLocation), 0.7f);
            }
            catch (FileNotFoundException)
            {
                pb.BackgroundImage = null;
            }
        }
    }
}
