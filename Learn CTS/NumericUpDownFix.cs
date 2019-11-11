using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn_CTS
{
    public class NumericUpDownFix : System.Windows.Forms.NumericUpDown
    {
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            return; // Disable scroll value control.
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
                hme.Handled = true;

            if (e.Delta > 0)
                this.Value += this.Increment;
            else if (e.Delta < 0)
                this.Value -= this.Increment;
        }
    }
}
