﻿using System.Windows.Forms;

namespace Learn_CTS
{
    public class NumericUpDownFix : System.Windows.Forms.NumericUpDown
    {
        /// <summary>
        /// Disable or recalculates the action of the scroll upon the Control.
        /// </summary>
        /// <param name="e">Arguments of the action performed.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            return; // Disable scroll value control.
        }
    }
}
