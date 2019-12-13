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
        /// Constructor.
        /// </summary>
        /// <param name="anchor">Optionnal anchor to get to a category directly. If the category is not found, displays the first one.</param>
        public Help(string anchor)
        {
            InitializeComponent();
            this.theme = Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "options.json")["theme"].ToString();
            this.themes = Tools.Get_From_JSON(System.AppDomain.CurrentDomain.BaseDirectory + "internal" + Path.DirectorySeparatorChar + "themes.json");

            help_texts.Add("Général", "Dans cette rubrique, vous pourrez modifier le titre et la description du jeu. Comme tous les textes changeables du jeu, il faut valider le changement en appuyant sur la touche 'Entrée'. Vous pouvez aussi annuler le changement avec la touche 'Echap'.");
            help_texts.Add("Modèles", "Dans cette rubrique, il est possible de modifier et de créer des figurants, des dialogues, ainsi que les images des personnages et des décors.");
            help_texts.Add("Figurants", "Ajoutez un figurant en cliquant sur le bouton          ou supprimez-en en cliquant sur la croix       . Choisissez l'apparence de votre figurant avec les flèches, renommez-le en cliquant sur son nom.");
            help_texts.Add("Dialogues", "Dans ce menu, vous pouvez créer des dialogues, une question à la fois, et les assigner à des personnages.\nVous pouvez créer une nouvelle question en cliquant sur le bouton       .\nChaque question, numérotée dans un onglet en haut à gauche du cadre du dialogue, est composée de un ou plusieurs choix de réponse.\nChangez le texte de la question en cliquant dessus, ajoutez un choix de réponse avec le bouton       , et choisissez comment la question sera présentée au joueur: uniquement sous forme de texte, d'audio, ou les deux.\nPour chaque choix de réponse, personnalisable en cliquant sur le texte, vous pouvez assigner un score positif ou négatif. Il est possible de choisir une question sur laquelle rediriger le joueur s'il choisit cette réponse.\nCette redirection permet des dialogues complexes et riches, qui augmentent la personnalisation du jeu et l'immersion du joueur.\nSupprimez une question(et ses choix de réponse avec, attention!) en cliquant sur la croix       , ou supprimez un choix de réponse unique en cliquant sur la poubelle       .");
            help_texts.Add("Images", "La rubrique Images permet d'ajouter ou des supprimer des images utilisables directement dans votre éditeur, et donc dans votre jeu !");
            help_texts.Add("Objets", "Cette rubrique vous donne accès aux images des objets utilisables directement dans l'éditeur et votre jeu, dans le mode de jeu vision à la première personne.\nAjoutez une nouvelle image d'objet en cliquant sur le bouton          . Cette action va ouvrir l'explorateur de fichiers.\nUne fois une image ajoutée, vous pouvez la supprimer en cliquant sur la croix    .");
            help_texts.Add("Personnages", "Ce menu donne l'accès aux images des personnages, et de leur animation. Vous pouvez ajouter un nouvel ensemble d'images pour un personnage en cliquant sur le bouton          .\nChaque ensemble d'images est placé dans un cadre, qui représente l'animation d'un personnage à chaque fois. La croix          permet de supprimer l'ensemble d'images. La première ligne d'images contient les animations de déplacement vers la droite du personnage, et la deuxième ligne les animations vers la gauche.\nEn cliquant sur une image, l'explorateur de fichier de votre ordinateur s'ouvre. Vous pouvez alors choisir une image, qui sera uilisée pour une étape de l'animation. L'icone en bas à gauche du cadre vous indique si les images sont valides ou non.");
            help_texts.Add("Décors", "Cette rubrique vous donne accès aux images des décors utilisables directement dans l'éditeur et votre jeu.\nAjoutez une nouvelle image de décor en cliquant sur le bouton        . Cette action va ouvrir l'explorateur de fichiers.\nUne fois une image ajoutée, vous pouvez la supprimer en cliquant sur la croix          .");
            help_texts.Add("Scénarios", "Cette rubrique vous permet d'ajouter des scénarios. Quand vous ajoutez un scénario, il est créé vide. Pour ajouter une nouvelle situation dans ce scénario, il suffit de cliquer sur 'Ajouter une nouvelle situation'.\nVous pouvez voir le nom complet du scénario, avec son arborescence, dans l'onglet en haut à gauche du cadre. Juste en dessous, vous pouvez modifier l'ordre d'apparition du scénario avec les flèches pointant vers le haut et vers le bas. Pour renommer le scénario, il suffit de cliquer sur son nom ou sur le crayon à coté. Vous pouvez supprimmer le scénario en cliquant sur la poubelle          à coté.");
            help_texts.Add("Situations", "La situation est le niveau de jeu dans lequel va évoluer le joueur. Vous pouvez, comme pour les scénarios, vous pouvez modifier l'ordre d'apparition de la situation. Notez que celui-ci est limité au scénario de la situation.\nLa situation est introduite par un écran d'introduction. Vous pouvez y renseigner le lieu de déroulement de la situation, et un texte d'introduction.\nVous pouvez choisir un décor pour la situation en le séléctionnant par son nom, qui est le nom du fichier de l'image.\nLe type de scène est le moteur de jeu utilisé pour la situation. Choisissez celui qui convient le mieux à vos envies !\nLa rubrique Evènements permet de mettre en lien un Figurant avec un dialogue.\nSi vous choisissez le mode de jeu vision à la première personne, vous pouvez mettre en lien des objets avec un dialogue, et non un figurant.\nVous pouvez ajouter un évènement par figurant avec le bouton          .La liste déroulante de gauche permet le choix du figurant voulu, et la liste déroulante de droite permet le choix du dialogue qui lui sera associé pendant la partie.\nPour jouer un aperçu de la situation, cliquez sur le bouton          .");

            tvw_help.SelectedNode = tvw_help.Nodes[anchor];
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

            pb_help.Parent = lbl_help;

            tvw_help.ExpandAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tvw_help_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pb_help.Image = null;
            try
            {
                lbl_help.Text = help_texts[tvw_help.SelectedNode.Text];
                pb_help.Image = Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "internal" +
                    Path.DirectorySeparatorChar + "images" + Path.DirectorySeparatorChar + "help" +
                    Path.DirectorySeparatorChar + tvw_help.SelectedNode.Text + ".png");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(tvw_help.SelectedNode.Text + " not found in help_texts");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
