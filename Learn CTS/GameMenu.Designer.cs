namespace Learn_CTS
{
    partial class GameMenu
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
            this.SuspendLayout();
            // 
            // GameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "GameMenu";
            this.Text = "GameMenu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameMenu_FormClosed);
            this.Load += new System.EventHandler(this.GameMenu_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.GameMenu_Layout);
            this.Resize += new System.EventHandler(this.GameMenu_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}