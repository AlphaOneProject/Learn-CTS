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

        public GameSelection(Editor editor)
        {
            InitializeComponent();
            this.editor = editor;
            this.Size = new Size(600, 500);
        }

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
            }
        }

        private void Game_Clicked(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            this.selected_game = lbl.Text;
            this.Close();
        }

        private void GameSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void GameSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.editor.Import_Selected_Library(this.selected_game);
        }
    }
}
