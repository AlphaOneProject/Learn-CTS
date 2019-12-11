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
            this.pbox_return = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_return)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox_return
            // 
            this.pbox_return.BackColor = System.Drawing.Color.Black;
            this.pbox_return.Location = new System.Drawing.Point(12, 12);
            this.pbox_return.Name = "pbox_return";
            this.pbox_return.Size = new System.Drawing.Size(76, 75);
            this.pbox_return.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_return.TabIndex = 0;
            this.pbox_return.TabStop = false;
            this.pbox_return.Visible = false;
            this.pbox_return.Click += new System.EventHandler(this.Button_Return);
            // 
            // GameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbox_return);
            this.Name = "GameMenu";
            this.Text = "GameMenu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameMenu_FormClosed);
            this.Load += new System.EventHandler(this.GameMenu_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.GameMenu_Layout);
            this.Resize += new System.EventHandler(this.GameMenu_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbox_return)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox_return;
    }
}