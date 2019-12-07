namespace Learn_CTS
{
    partial class GameWindow
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_fps = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_fps
            // 
            this.lbl_fps.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_fps.AutoEllipsis = true;
            this.lbl_fps.AutoSize = true;
            this.lbl_fps.BackColor = System.Drawing.Color.Black;
            this.lbl_fps.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_fps.ForeColor = System.Drawing.Color.White;
            this.lbl_fps.Location = new System.Drawing.Point(1170, 9);
            this.lbl_fps.Name = "lbl_fps";
            this.lbl_fps.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_fps.Size = new System.Drawing.Size(82, 31);
            this.lbl_fps.TabIndex = 4;
            this.lbl_fps.Text = "0,000";
            this.lbl_fps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_fps.Visible = false;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.lbl_fps);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(320, 540);
            this.Name = "GameWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameWindow_FormClosed);
            this.Load += new System.EventHandler(this.GameWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameWindow_MouseDown);
            this.Resize += new System.EventHandler(this.GameWindow_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_fps;
    }
}

