namespace Learn_CTS
{
    partial class GameCard
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_description = new System.Windows.Forms.Label();
            this.pb_edit = new System.Windows.Forms.PictureBox();
            this.pb_play = new System.Windows.Forms.PictureBox();
            this.pb_thumbnail = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_edit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_play)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_thumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Location = new System.Drawing.Point(142, 6);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(158, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Titre";
            // 
            // lbl_description
            // 
            this.lbl_description.Location = new System.Drawing.Point(142, 34);
            this.lbl_description.Name = "lbl_description";
            this.lbl_description.Size = new System.Drawing.Size(158, 100);
            this.lbl_description.TabIndex = 2;
            this.lbl_description.Text = "Description";
            // 
            // pb_edit
            // 
            this.pb_edit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_edit.Location = new System.Drawing.Point(95, 7);
            this.pb_edit.Name = "pb_edit";
            this.pb_edit.Size = new System.Drawing.Size(42, 42);
            this.pb_edit.TabIndex = 5;
            this.pb_edit.TabStop = false;
            this.pb_edit.Click += new System.EventHandler(this.Pb_edit_Click);
            this.pb_edit.MouseLeave += new System.EventHandler(this.Pb_edit_MouseLeave);
            this.pb_edit.MouseHover += new System.EventHandler(this.Pb_edit_MouseHover);
            // 
            // pb_play
            // 
            this.pb_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_play.Location = new System.Drawing.Point(28, 28);
            this.pb_play.Name = "pb_play";
            this.pb_play.Size = new System.Drawing.Size(86, 86);
            this.pb_play.TabIndex = 4;
            this.pb_play.TabStop = false;
            this.pb_play.Click += new System.EventHandler(this.Pb_play_Click);
            this.pb_play.MouseLeave += new System.EventHandler(this.Pb_play_MouseLeave);
            this.pb_play.MouseHover += new System.EventHandler(this.Pb_play_MouseHover);
            // 
            // pb_thumbnail
            // 
            this.pb_thumbnail.Location = new System.Drawing.Point(7, 7);
            this.pb_thumbnail.Name = "pb_thumbnail";
            this.pb_thumbnail.Size = new System.Drawing.Size(128, 128);
            this.pb_thumbnail.TabIndex = 3;
            this.pb_thumbnail.TabStop = false;
            // 
            // GameCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.pb_edit);
            this.Controls.Add(this.pb_play);
            this.Controls.Add(this.pb_thumbnail);
            this.Controls.Add(this.lbl_description);
            this.Controls.Add(this.lbl_title);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "GameCard";
            this.Size = new System.Drawing.Size(314, 140);
            ((System.ComponentModel.ISupportInitialize)(this.pb_edit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_play)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_thumbnail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_description;
        private System.Windows.Forms.PictureBox pb_thumbnail;
        private System.Windows.Forms.PictureBox pb_play;
        private System.Windows.Forms.PictureBox pb_edit;
    }
}
