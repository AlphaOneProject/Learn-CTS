using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Learn_CTS
{
    class Platform : Texture
    {

        /// <summary>
        /// Constructor of platform.
        /// </summary>
        /// <param name="x">The x coordinate.</param>

        public Platform(int x, int y, int z) : base("Platform", x, y, z)
        {
        }

        public override void OnPaint(PaintEventArgs e)
        {
            CompositingMode c = e.Graphics.CompositingMode;
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            base.OnPaint(e);
            e.Graphics.CompositingMode = c;
        }
    }
}
