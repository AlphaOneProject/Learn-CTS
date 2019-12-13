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
            this.lbl_nscore = new System.Windows.Forms.Label();
            this.lbl_name_place = new System.Windows.Forms.Label();
            this.lbl_nfps = new System.Windows.Forms.Label();
            this.lbl_score = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_nscore
            // 
            this.lbl_nscore.AutoSize = true;
            this.lbl_nscore.BackColor = System.Drawing.Color.Black;
            this.lbl_nscore.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F);
            this.lbl_nscore.ForeColor = System.Drawing.Color.White;
            this.lbl_nscore.Location = new System.Drawing.Point(668, 9);
            this.lbl_nscore.Name = "lbl_nscore";
            this.lbl_nscore.Size = new System.Drawing.Size(93, 39);
            this.lbl_nscore.TabIndex = 0;
            this.lbl_nscore.Tag = "0";
            this.lbl_nscore.Text = "0000";
            this.lbl_nscore.Visible = false;
            // 
            // lbl_name_place
            // 
            this.lbl_name_place.AutoSize = true;
            this.lbl_name_place.BackColor = System.Drawing.Color.Black;
            this.lbl_name_place.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.lbl_name_place.ForeColor = System.Drawing.Color.White;
            this.lbl_name_place.Location = new System.Drawing.Point(1156, 9);
            this.lbl_name_place.Name = "lbl_name_place";
            this.lbl_name_place.Size = new System.Drawing.Size(96, 31);
            this.lbl_name_place.TabIndex = 1;
            this.lbl_name_place.Tag = "0";
            this.lbl_name_place.Text = "default";
            this.lbl_name_place.Visible = false;
            // 
            // lbl_nfps
            // 
            this.lbl_nfps.AutoSize = true;
            this.lbl_nfps.BackColor = System.Drawing.Color.Black;
            this.lbl_nfps.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.lbl_nfps.ForeColor = System.Drawing.Color.White;
            this.lbl_nfps.Location = new System.Drawing.Point(1170, 57);
            this.lbl_nfps.Name = "lbl_nfps";
            this.lbl_nfps.Size = new System.Drawing.Size(82, 31);
            this.lbl_nfps.TabIndex = 2;
            this.lbl_nfps.Tag = "0";
            this.lbl_nfps.Text = "0,000";
            this.lbl_nfps.Visible = false;
            // 
            // lbl_score
            // 
            this.lbl_score.AutoSize = true;
            this.lbl_score.BackColor = System.Drawing.Color.Black;
            this.lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.25F);
            this.lbl_score.ForeColor = System.Drawing.Color.White;
            this.lbl_score.Location = new System.Drawing.Point(527, 9);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.Size = new System.Drawing.Size(124, 39);
            this.lbl_score.TabIndex = 3;
            this.lbl_score.Tag = "0";
            this.lbl_score.Text = "Score :";
            this.lbl_score.Visible = false;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 581);
            this.Controls.Add(this.lbl_score);
            this.Controls.Add(this.lbl_nfps);
            this.Controls.Add(this.lbl_name_place);
            this.Controls.Add(this.lbl_nscore);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(320, 458);
            this.Name = "GameWindow";
            this.Tag = "x";
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

        private System.Windows.Forms.Label lbl_nscore;
        private System.Windows.Forms.Label lbl_name_place;
        private System.Windows.Forms.Label lbl_nfps;
        private System.Windows.Forms.Label lbl_score;
    }
}

