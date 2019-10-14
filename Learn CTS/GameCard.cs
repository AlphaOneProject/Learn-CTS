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

            set
            {
                int char_space = 24; // Number of characters that can be seen in the label
                if (value.Length > char_space)
                {
                    this.lbl_title.Text = value.Substring(0, char_space - 3) + "...";
                }
                else
                {
                    this.lbl_title.Text = value;
                }
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
                int char_space = 64; // Number of characters that can be seen in the label
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
    }
}
