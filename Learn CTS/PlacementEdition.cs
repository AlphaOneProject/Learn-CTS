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
    public partial class PlacementEdition : Form
    {
        // Attributes.

        private Editor editor;
        private PictureBox pb;
        private Point dragPoint = Point.Empty;
        private bool dragging = false;
        private int event_id;
        private int scene_type;

        // Methods.

        /// <summary>
        /// Constructor of the Form, setup the necessary arguments as parameters.
        /// </summary>
        /// <param name="sender">Instance of the Editor parent of the Form.</param>
        /// <param name="pb_placing">PictureBox to place.</param>
        /// <param name="pbs_placed">Others PictureBoxes already placed.</param>
        /// <param name="pbs_placed_points">Positions from the placed PictureBoxes.</param>
        /// <param name="pb_environment">PictureBox where the others will be placed.</param>
        public PlacementEdition(Editor sender, PictureBox pb_placing, List<PictureBox> pbs_placed, List<Point> pbs_placed_points, PictureBox pb_environment, int scene_type)
        {
            InitializeComponent();

            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            this.editor = sender;
            this.event_id = (int)pb_placing.Tag;
            this.scene_type = scene_type;
            this.Height = Tools.Min_Int(pb_environment.Height, 800) + 56; // 56 = Border + Scrollbar. (by deduction)
            this.Width = Tools.Min_Int(pb_environment.Width, 1200);
            pan_global.Controls.Add(pb_environment);
            pb_environment.Location = new Point(0, 0);

            for (int i = 0; i < pbs_placed.Count; i++)
            {
                pan_global.Controls.Add(pbs_placed[i]);
                pbs_placed[i].Location = pbs_placed_points[i];
                //pbs_placed[i].BackColor = Color.Transparent;
            }

            pan_global.Controls.Add(pb_placing);
            pb_placing.MouseDown += new MouseEventHandler(Pb_Placing_MouseDown);
            pb_placing.MouseMove += new MouseEventHandler(Pb_Placing_MouseMove);
            pb_placing.MouseUp += new MouseEventHandler(Pb_Placing_MouseUp);
            //pb_placing.BorderStyle = BorderStyle.Fixed3D;
            pb_placing.Cursor = Cursors.SizeAll;
            //pb_placing.BackColor = Color.Transparent;
            this.pb = pb_placing;

            pb_environment.SendToBack();
            pb_placing.BringToFront();
            
        }

        /// <summary>
        /// Activates the dragging.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_Placing_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// Modify the location for the PictureBox while it is dragged.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_Placing_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (this.dragging)
            {
                pb.Location = new Point(pb.Location.X + e.X - dragPoint.X, pb.Location.Y + e.Y - dragPoint.Y);
            }
        }

        /// <summary>
        /// Toggles the dragging off.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void Pb_Placing_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// Transmits to the Editor the new position selected.
        /// </summary>
        /// <param name="sender">Control calling the method.</param>
        /// <param name="e">Arguments from the action whose caused the call of this method.</param>
        private void PlacementEdition_FormClosing(object sender, FormClosingEventArgs e)
        {
            Point current_pos = new Point(this.pb.Location.X + pan_global.HorizontalScroll.Value,
                                                                   this.pb.Location.Y + pan_global.VerticalScroll.Value);
            Texture.InitializePath(editor.Get_Game());
            bool isColliding;
            switch (this.scene_type)
            {
                case int n when (n > 3 && n < 8):
                    isColliding = Tools.IsCollidingWithVehicule(new Bus(0, 0), current_pos);
                    break;
                case int n when (n > 7):
                    isColliding = false;
                    break;
                default:
                    isColliding = Tools.IsCollidingWithVehicule(new Tram(0, 0), current_pos);
                    break;
            }

            if (isColliding)
            {
                if (MessageBox.Show("L'évènement rentre en collision avec la scène, rendant sa position invalide.\n" +
                                    "Souhaitez-vous valider tout de même ? (Il sera alors placé aléatoirement.)", "Position invalide",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            this.editor.Reset_Place_Event(this.event_id, current_pos);
        }
    }
}
