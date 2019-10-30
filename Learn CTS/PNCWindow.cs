using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public partial class PNCWindow : Form
    {

        private List<Texture> list_textures;
        private String game;
        private String game_path;

        public PNCWindow(String game)
        {
            this.game = game;
            this.Text = game;
            DoubleBuffered = false;
            InitializeComponent();
        }

        private void PNCWindow_Load(object sender, EventArgs e)
        {

        }
        
        private void InitializeListTextures()
        {
            Texture.InitializePath(game);
            Hint h1 = new Hint("n1", 32, 34);
            list_textures = new List<Texture>
            {
                h1
            };
        }
    }
}
