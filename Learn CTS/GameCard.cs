using System;
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

        public GameCard()
        {
            InitializeComponent();
        }

        public String Title
        {
            get
            {
                return this.lbl_title.Text;
            }

            set => this.lbl_title.Text = value;
        }

        public String Description
        {
            get
            {
                return this.lbl_description.Text;
            }

            set => this.lbl_description.Text = value;
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
        }
    }
}
