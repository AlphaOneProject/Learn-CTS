namespace Learn_CTS
{
    partial class ImageEdition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEdition));
            this.pb_img = new System.Windows.Forms.PictureBox();
            this.lbl_title = new System.Windows.Forms.Label();
            this.pb_delete = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_img
            // 
            this.pb_img.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pb_img.Location = new System.Drawing.Point(-1, -1);
            this.pb_img.Name = "pb_img";
            this.pb_img.Size = new System.Drawing.Size(200, 200);
            this.pb_img.TabIndex = 0;
            this.pb_img.TabStop = false;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Location = new System.Drawing.Point(3, 206);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(93, 24);
            this.lbl_title.TabIndex = 1;
            this.lbl_title.Text = "Loading...";
            // 
            // pb_delete
            // 
            this.pb_delete.BackColor = System.Drawing.Color.Transparent;
            this.pb_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete.Image = ((System.Drawing.Image)(resources.GetObject("pb_delete.Image")));
            this.pb_delete.Location = new System.Drawing.Point(171, 3);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(24, 24);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 2;
            this.pb_delete.TabStop = false;
            this.pb_delete.Click += new System.EventHandler(this.Pb_delete_Click);
            // 
            // ImageEdition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pb_delete);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.pb_img);
            this.Name = "ImageEdition";
            this.Size = new System.Drawing.Size(198, 238);
            this.Load += new System.EventHandler(this.ImageEdition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_img;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.PictureBox pb_delete;
    }
}
