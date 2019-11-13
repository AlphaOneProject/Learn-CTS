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
            this.lbl_sc = new System.Windows.Forms.Label();
            this.lbl_score = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_sc
            // 
            this.lbl_sc.AutoSize = true;
            this.lbl_sc.BackColor = System.Drawing.Color.Black;
            this.lbl_sc.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_sc.ForeColor = System.Drawing.Color.White;
            this.lbl_sc.Location = new System.Drawing.Point(12, 9);
            this.lbl_sc.Name = "lbl_sc";
            this.lbl_sc.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_sc.Size = new System.Drawing.Size(100, 31);
            this.lbl_sc.TabIndex = 1;
            this.lbl_sc.Text = "Score :";
            this.lbl_sc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_score
            // 
            this.lbl_score.AutoSize = true;
            this.lbl_score.BackColor = System.Drawing.Color.Black;
            this.lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_score.ForeColor = System.Drawing.Color.White;
            this.lbl_score.Location = new System.Drawing.Point(118, 9);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_score.Size = new System.Drawing.Size(96, 31);
            this.lbl_score.TabIndex = 0;
            this.lbl_score.Text = "default";
            this.lbl_score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.lbl_sc);
            this.Controls.Add(this.lbl_score);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "GameWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameWindow_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameWindow_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_sc;
        private System.Windows.Forms.Label lbl_score;
    }
}

