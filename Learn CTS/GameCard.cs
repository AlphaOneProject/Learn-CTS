using Newtonsoft.Json.Linq;
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

        private static Image icon_play;
        private static Image icon_edit;
        private static Image icon_copy;
        private static Image icon_delete;

        public GameCard()
        {
            InitializeComponent();
            this.img_path = AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;

            // Fetching icons and saving them in a static variable.
            FetchIcons();

            // Setting the icons to 50% opacity by default.
            pb_play.BackgroundImage = Tools.ChangeOpacity(icon_play, 0.5f);
            pb_play.BackColor = Color.Transparent;
            pb_edit.BackgroundImage = Tools.ChangeOpacity(icon_edit, 0.5f);
            pb_edit.BackColor = Color.Transparent;
            pb_delete.BackgroundImage = Tools.ChangeOpacity(icon_delete, 0.5f);
            pb_delete.BackColor = Color.Transparent;

            pb_copy.Hide();
            //pb_copy.BackgroundImage = Tools.ChangeOpacity(icon_copy, 0.5f);
            //pb_copy.BackColor = Color.Transparent;
        }

        private void FetchIcons()
        {
            try
            {
                if (icon_play == null)
                {
                    icon_play = Image.FromFile(img_path + "gamecard-play-btn-x128.png");
                }
                if (icon_edit == null)
                {
                    icon_edit = Image.FromFile(img_path + "gamecard-edit-btn-x64.png");
                }
                if (icon_delete == null)
                {
                    icon_delete = Image.FromFile(img_path + "gamecard-delete-btn-x64.png");
                }
                if (icon_copy == null)
                {
                    icon_copy = Image.FromFile(img_path + "gamecard-copy-btn-x64.png");
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                ToolTip tp_title = new ToolTip();
                tp_title.SetToolTip(lbl_title, value);
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
                pb_copy.Parent = pb_thumbnail;
                pb_copy.Location = new Point(6, 128 - pb_copy.Height - 2);
                pb_edit.Parent = pb_thumbnail;
                pb_edit.Location = new Point(128 - pb_edit.Width, 0);
                pb_delete.Parent = pb_thumbnail;
                pb_delete.Location = new Point(128 - pb_delete.Width, 128 - pb_delete.Height);
                pb_copy.BringToFront();
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

        private void Pb_play_Click(object sender, EventArgs e)
        {
            Form game_menu = new GameMenu(this.gameFullName);
            game_menu.Show();
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
            try
            {
                Directory.Delete(@"" + System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + this.gameFullName, true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// marche pa
        /// </summary>
        private void Copy_Game()
        {
            String source_path = @"" + System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + this.gameFullName;
            String target_path = @"" + System.AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + this.gameFullName + "-Copie";
            DirectoryInfo dir_source = new DirectoryInfo(source_path);
            
            Boolean valid_name = false;
            int copy_number = 1;
            while (!valid_name)
            {
                try
                {
                    DirectoryInfo dir_target = new DirectoryInfo(target_path + copy_number.ToString());

                    while (dir_target.Exists)
                    {
                        copy_number++;
                        dir_target = new DirectoryInfo(target_path + copy_number.ToString());
                    }
                    Tools.DirectoryCopy(dir_source, dir_target);
                }
                catch (IOException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            JObject properties_content = Tools.Get_From_JSON(target_path + copy_number.ToString() +
                Path.DirectorySeparatorChar + "properties.json");
            properties_content["default"] = false;
            File.WriteAllText(@"" + target_path + copy_number.ToString() + Path.DirectorySeparatorChar + "properties.json",
                                properties_content.ToString());

            Form g = new Editor(this.gameFullName + "-Copie" + copy_number.ToString());
            this.Hide();
            g.Show();
        }

        private void Pb_Btn_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            try
            {
                if (pb.BackgroundImage != null) pb.BackgroundImage.Dispose();
                pb.BackgroundImage = Tools.ChangeOpacity(Image.FromFile(img_path + pb.ImageLocation), 1);
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
                pb.BackgroundImage = Tools.ChangeOpacity(Image.FromFile(img_path + pb.ImageLocation), 0.5f);
            }
            catch (FileNotFoundException)
            {
                pb.BackgroundImage = null;
            }
        }

        private void Pb_copy_Click(object sender, EventArgs e)
        {
            Copy_Game();
        }
    }
}
