namespace Learn_CTS
{
    partial class PlacementEdition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pan_global = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pan_global
            // 
            this.pan_global.AutoScroll = true;
            this.pan_global.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pan_global.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_global.Location = new System.Drawing.Point(0, 0);
            this.pan_global.Name = "pan_global";
            this.pan_global.Size = new System.Drawing.Size(800, 450);
            this.pan_global.TabIndex = 0;
            // 
            // PlacementEdition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pan_global);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlacementEdition";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Placement de l\'évènement";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlacementEdition_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_global;
    }
}