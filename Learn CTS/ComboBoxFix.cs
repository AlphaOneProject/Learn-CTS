using System;
using System.Windows.Forms;

namespace Learn_CTS
{
    class ComboBoxFix : ComboBox
    {
        /// <summary>
        /// Disable or recalculates the action of the scroll upon the Control.
        /// </summary>
        /// <param name="e">Arguments of the action performed.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
            return;
        }
    }
}
