using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Learn_CTS
{
    /// <summary>
    /// UserControl used for displaying a game.
    /// Allow user to launch the game, start the editor of the game, copying it or deleting it.
    /// </summary>
    public partial class GameCard : UserControl
    {
        /**
         * True if this is the card of a default game
         * False otherwise
         */
        private bool isDefaultGame;
        private readonly string img_path;
        private string gameFullName;
        private Menu menu;
        private JObject theme;

        private static Image icon_play;
        private static Image icon_edit;
        private static Image icon_copy;
        private static Image icon_delete;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menu">Menu calling the GameCard.</param>
        public GameCard(Menu menu)
        {
            InitializeComponent();
            this.img_path = AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar;
            this.menu = menu;
            this.theme = menu.GetTheme();

            // Fetching icons and saving them in a static variable.
            FetchIcons();

            // Setting the icons to 50% opacity by default.
            pb_play.BackgroundImage = Tools.ChangeOpacity(icon_play, 0.5f);
            pb_play.BackColor = Color.Transparent;
            pb_edit.BackgroundImage = Tools.ChangeOpacity(icon_edit, 0.5f);
            pb_edit.BackColor = Color.Transparent;
            pb_delete.BackgroundImage = Tools.ChangeOpacity(icon_delete, 0.5f);
            pb_delete.BackColor = Color.Transparent;
            pb_copy.BackgroundImage = Tools.ChangeOpacity(icon_copy, 0.5f);
            pb_copy.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Called when the gamecard is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameCard_Load(object sender, EventArgs e)
        {
            //Setting the colors for the controls accoring to the theme
            Change_Theme();
        }

        /// <summary>
        /// Gets the images of the icons from the library and stores them in local variables.
        /// </summary>
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

        /// <summary>
        /// Title of the game, truncated if necessary.
        /// </summary>
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

        /// <summary>
        /// Description of the game, truncated if necessary.
        /// </summary>
        public string Description
        {
            get
            {
                return this.lbl_description.Text;
            }

            set
            {
                int char_space = 134; // Number of characters that can be seen in the label
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

        /// <summary>
        /// Fetches the game thumbnail in the library.
        /// </summary>
        /// <param name="game">Name of the game of the thumbnail.</param>
        public void Show_Thumbnail(string game)
        {
            string thumbnail_path = AppDomain.CurrentDomain.BaseDirectory + "games" + Path.DirectorySeparatorChar + game
                + Path.DirectorySeparatorChar + "library" + Path.DirectorySeparatorChar + "images"
                + Path.DirectorySeparatorChar + "others" + Path.DirectorySeparatorChar + "thumbnail.png";
            try
            {
                Image img;
                using (var bmpTemp = new Bitmap(thumbnail_path))
                {
                    img = new Bitmap(bmpTemp);
                }
                pb_thumbnail.BackgroundImage = img;
            }
            catch (Exception)
            {
                pb_thumbnail.BackgroundImage = null;
            }
            finally
            {
                // Setting the parent of the icons in order to make them transparent.
                pb_play.Parent = pb_thumbnail;

                // Bringing the controls to the front
                pb_copy.BringToFront();
                pb_edit.BringToFront();
                pb_delete.BringToFront();
            }
        }

        /// <summary>
        /// Is the game a default game.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return this.isDefaultGame;
            }
            set => this.isDefaultGame = value;
        }

        /// <summary>
        /// Called when the button to edit the game is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_edit_Click(object sender, EventArgs e)
        {
            ((Menu)this.ParentForm).Save_Options();
            Editor editor = new Editor(Title);
            editor.Show();
            this.ParentForm.Hide();
        }

        /// <summary>
        /// Called when the PictureBox to launch the game is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_play_Click(object sender, EventArgs e)
        {
            bool isGameBusy = (Tools.Get_From_JSON(AppDomain.CurrentDomain.BaseDirectory + "games"
                + Path.DirectorySeparatorChar + this.gameFullName + 
                Path.DirectorySeparatorChar + "properties.json")["state"].ToString()).Equals("En cours d'édition...");
            if (!isGameBusy)
            {
                Form game_menu = new GameMenu(this.gameFullName);
                game_menu.Show();
                this.Parent.Parent.Hide();
            }
            else
            {
                MessageBox.Show("Ce jeu est en cours d'édition sur votre ordinateur, vous ne pouvez pas le lancer.");
            }
        }

        /// <summary>
        /// Called when the PictureBox to edit the game is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_edit_Click(object sender, EventArgs e)
        {
            Form editor = new Editor(this.gameFullName);
            editor.Show();
            this.Parent.Parent.Hide();
        }

        /// <summary>
        /// Called when the PictureBox to delete the game is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    this.Parent.Controls.Remove(this);
                    Delete_Game();
                }
            }            
        }

        /// <summary>
        /// Deletes the game from the file system.
        /// </summary>
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
        /// Creates a copy of the game in the fil system and launches the editor of the newly created game.
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
                    // We try to create the new directory
                    DirectoryInfo dir_target = new DirectoryInfo(target_path + copy_number.ToString());

                    while (dir_target.Exists)
                    {
                        copy_number++;
                        dir_target = new DirectoryInfo(target_path + copy_number.ToString());
                    }
                    valid_name = true;
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

        /// <summary>
        /// Called when the mouse hovers a PictureBox used to interact with the card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when the mouse leaves a PictureBox used to interact with the card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when the PictureBox to copy a game is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_copy_Click(object sender, EventArgs e)
        {
            Copy_Game();
        }

        /// <summary>
        /// Change the theme of all the controls of this form, according to the current theme in the options.
        /// </summary>
        public void Change_Theme()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Tag != null)
                {
                    if (int.Parse(c.Tag.ToString()) == 5)
                    {
                        c.ForeColor = Color.FromArgb(
                            int.Parse((string)this.theme[c.Tag.ToString()]["R"]),
                            int.Parse((string)this.theme[c.Tag.ToString()]["G"]),
                            int.Parse((string)this.theme[c.Tag.ToString()]["B"])
                        );
                    }
                    else
                    {
                        c.ForeColor = Color.FromArgb(
                            int.Parse((string)this.theme["5"]["R"]),
                            int.Parse((string)this.theme["5"]["G"]),
                            int.Parse((string)this.theme["5"]["B"])
                        );
                        c.BackColor = Color.FromArgb(
                           int.Parse((string)this.theme[c.Tag.ToString()]["R"]),
                           int.Parse((string)this.theme[c.Tag.ToString()]["G"]),
                           int.Parse((string)this.theme[c.Tag.ToString()]["B"])
                       );
                    }

                }
            }

            this.BackColor = Color.FromArgb(
                        int.Parse((string)this.theme[this.Tag.ToString()]["R"]),
                        int.Parse((string)this.theme[this.Tag.ToString()]["G"]),
                        int.Parse((string)this.theme[this.Tag.ToString()]["B"])
                    );
        }
    }
}
