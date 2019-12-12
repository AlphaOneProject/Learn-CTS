using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    /// <summary>
    /// Form used as help for the user. Contains documentation to learn to use the editor.
    /// </summary>
    public partial class Help : Form
    {

        private string theme;

        private JObject themes;

        private Dictionary<string, string> help_texts = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public Help()
        {
            InitializeComponent();
            this.theme = Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "options.json")["theme"].ToString();
            this.themes = Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "themes.json");

            help_texts.Add("Général", "Dans cette rubrique, vous pourrez modifier le titre et la description du jeu. Comme tous les textes changeables du jeu, il faut valider le changement en appuyant sur la touche 'Entrée'. Vous pouvez aussi annuler le changement avec la touche 'Echap'.");
            help_texts.Add("Figurants", "Dans cette rubrique, il est possible de modifier et de créer des figurants, des dialogues, ainsi que les images des personnages et des décors.\n Ajoutez un figurant en cliquant sur le bouton  ou supprimez-en en cliquant sur la croix. Choisissez l'apparence de votre figurant avec les flèches, renommez-le en cliquant sur son nom.");
        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(
                        (int)(this.themes[this.theme][this.Tag.ToString()]["R"]),
                        (int)(this.themes[this.theme][this.Tag.ToString()]["G"]),
                        (int)(this.themes[this.theme][this.Tag.ToString()]["B"])
                    );
            tvw_help.BackColor = Color.FromArgb(
                        (int)(this.themes[this.theme][tvw_help.Tag.ToString()]["R"]),
                        (int)(this.themes[this.theme][tvw_help.Tag.ToString()]["G"]),
                        (int)(this.themes[this.theme][tvw_help.Tag.ToString()]["B"])
                    );
            tvw_help.ForeColor = Color.FromArgb(
                        (int)(this.themes[this.theme]["5"]["R"]),
                        (int)(this.themes[this.theme]["5"]["G"]),
                        (int)(this.themes[this.theme]["5"]["B"])
                    );
            pnl_bg.BackColor = Color.FromArgb(
                        (int)(this.themes[this.theme][pnl_bg.Tag.ToString()]["R"]),
                        (int)(this.themes[this.theme][pnl_bg.Tag.ToString()]["G"]),
                        (int)(this.themes[this.theme][pnl_bg.Tag.ToString()]["B"])
                    );
            lbl_help.ForeColor = Color.FromArgb(
                        (int)(this.themes[this.theme]["5"]["R"]),
                        (int)(this.themes[this.theme]["5"]["G"]),
                        (int)(this.themes[this.theme]["5"]["B"])
                    );

            tvw_help.ExpandAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tvw_help_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                lbl_help.Text = help_texts[tvw_help.SelectedNode.Text];
            }
            catch(KeyNotFoundException)
            {
                Console.WriteLine(tvw_help.SelectedNode.Text + " not found in help_texts");
            }
        }
    }
}
