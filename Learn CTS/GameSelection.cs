using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Learn_CTS
{
    public partial class GameSelection : Form
    {
        // Attributes.

        private Editor editor;
        private string selected_game = "";

        // Methods.

        /// <summary>
        /// Constructor this Form allowing the selection of a game amongst
        /// the existing ones.
        /// </summary>
        /// <param name="editor">Editor parent of this Form.</param>
        public GameSelection(Editor editor)
        {
            InitializeComponent();
            this.editor = editor;
            this.Size = new Size(600, 500);
        }

        /// <summary>
        /// Load each existing game as a Label.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameSelection_Load(object sender, EventArgs e)
        {
            DirectoryInfo games_path = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "games");
            foreach (DirectoryInfo di in games_path.GetDirectories())
            {
                Label lbl_game = new Label()
                {
                    Name = "lbl_game_" + di.Name,
                    Text = di.Name,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular,
                                                   System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    AutoSize = true,
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.FixedSingle
                };
                lbl_game.Click += new EventHandler(Game_Clicked);
                flp_global.Controls.Add(lbl_game);
                tlt_GameSelection.SetToolTip(lbl_game, "Cliquez pour importer les modèles de ce jeu");
            }
        }

        /// <summary>
        /// Trigger upon click on a Label displayed.
        /// Set it as the "selected_game" and close this Form.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Game_Clicked(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            this.selected_game = lbl.Text;
            this.Close();
        }

        /// <summary>
        /// Bind "Escape" key as closing key for this Form.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Calls the parent Editor to handle the selected game upon form closing.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void GameSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.editor.Import_Selected_Library(this.selected_game);
        }
    }
}
