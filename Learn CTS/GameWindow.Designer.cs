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
            this.lbl_score = new System.Windows.Forms.Label();
            this.pbox_backpack = new System.Windows.Forms.PictureBox();
            this.lbl_nfps = new System.Windows.Forms.Label();
            this.pbox_egg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_backpack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_egg)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_score
            // 
            this.lbl_score.AutoSize = true;
            this.lbl_score.BackColor = System.Drawing.Color.Black;
            this.lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_score.ForeColor = System.Drawing.Color.White;
            this.lbl_score.Location = new System.Drawing.Point(12, 111);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_score.Size = new System.Drawing.Size(96, 31);
            this.lbl_score.TabIndex = 0;
            this.lbl_score.Text = "default";
            this.lbl_score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbox_backpack
            // 
            this.pbox_backpack.BackColor = System.Drawing.Color.Silver;
            this.pbox_backpack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbox_backpack.Location = new System.Drawing.Point(12, 145);
            this.pbox_backpack.Name = "pbox_backpack";
            this.pbox_backpack.Size = new System.Drawing.Size(96, 96);
            this.pbox_backpack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_backpack.TabIndex = 2;
            this.pbox_backpack.TabStop = false;
            this.pbox_backpack.Click += new System.EventHandler(this.pbox_backpack_Click);
            // 
            // lbl_nfps
            // 
            this.lbl_nfps.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_nfps.AutoEllipsis = true;
            this.lbl_nfps.AutoSize = true;
            this.lbl_nfps.BackColor = System.Drawing.Color.Black;
            this.lbl_nfps.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_nfps.ForeColor = System.Drawing.Color.White;
            this.lbl_nfps.Location = new System.Drawing.Point(1170, 9);
            this.lbl_nfps.Name = "lbl_nfps";
            this.lbl_nfps.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_nfps.Size = new System.Drawing.Size(82, 31);
            this.lbl_nfps.TabIndex = 4;
            this.lbl_nfps.Text = "0,000";
            this.lbl_nfps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_nfps.Visible = false;
            // 
            // pbox_egg
            // 
            this.pbox_egg.Location = new System.Drawing.Point(12, 12);
            this.pbox_egg.Name = "pbox_egg";
            this.pbox_egg.Size = new System.Drawing.Size(96, 96);
            this.pbox_egg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_egg.TabIndex = 5;
            this.pbox_egg.TabStop = false;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pbox_egg);
            this.Controls.Add(this.lbl_nfps);
            this.Controls.Add(this.pbox_backpack);
            this.Controls.Add(this.lbl_score);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(320, 540);
            this.Name = "GameWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameWindow_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameWindow_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameWindow_MouseDown);
            this.Resize += new System.EventHandler(this.GameWindow_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbox_backpack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_egg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_score;
        private System.Windows.Forms.PictureBox pbox_backpack;
        private System.Windows.Forms.Label lbl_nfps;
        private System.Windows.Forms.PictureBox pbox_egg;
    }
}

