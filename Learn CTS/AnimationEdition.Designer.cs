namespace Learn_CTS
{
    partial class AnimationEdition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationEdition));
            this.pb_down2 = new System.Windows.Forms.PictureBox();
            this.pb_down3 = new System.Windows.Forms.PictureBox();
            this.pb_down1 = new System.Windows.Forms.PictureBox();
            this.pb_down_left = new System.Windows.Forms.PictureBox();
            this.pb_down_right = new System.Windows.Forms.PictureBox();
            this.pb_up_right = new System.Windows.Forms.PictureBox();
            this.pb_up_left = new System.Windows.Forms.PictureBox();
            this.pb_up1 = new System.Windows.Forms.PictureBox();
            this.pb_up3 = new System.Windows.Forms.PictureBox();
            this.pb_up2 = new System.Windows.Forms.PictureBox();
            this.lbl_name = new System.Windows.Forms.Label();
            this.pb_valid = new System.Windows.Forms.PictureBox();
            this.ofd_sprites = new System.Windows.Forms.OpenFileDialog();
            this.pb_delete = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_valid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_down2
            // 
            this.pb_down2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_down2.Location = new System.Drawing.Point(128, 95);
            this.pb_down2.Name = "pb_down2";
            this.pb_down2.Size = new System.Drawing.Size(80, 80);
            this.pb_down2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_down2.TabIndex = 0;
            this.pb_down2.TabStop = false;
            this.pb_down2.Click += new System.EventHandler(this.Click_Sprite);
            // 
            // pb_down3
            // 
            this.pb_down3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_down3.Location = new System.Drawing.Point(214, 95);
            this.pb_down3.Name = "pb_down3";
            this.pb_down3.Size = new System.Drawing.Size(80, 80);
            this.pb_down3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_down3.TabIndex = 1;
            this.pb_down3.TabStop = false;
            this.pb_down3.Click += new System.EventHandler(this.Click_Sprite);
            // 
            // pb_down1
            // 
            this.pb_down1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_down1.Location = new System.Drawing.Point(42, 95);
            this.pb_down1.Name = "pb_down1";
            this.pb_down1.Size = new System.Drawing.Size(80, 80);
            this.pb_down1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_down1.TabIndex = 2;
            this.pb_down1.TabStop = false;
            this.pb_down1.Click += new System.EventHandler(this.Click_Sprite);
            // 
            // pb_down_left
            // 
            this.pb_down_left.BackColor = System.Drawing.Color.Transparent;
            this.pb_down_left.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_down_left.Image = ((System.Drawing.Image)(resources.GetObject("pb_down_left.Image")));
            this.pb_down_left.Location = new System.Drawing.Point(4, 121);
            this.pb_down_left.Name = "pb_down_left";
            this.pb_down_left.Size = new System.Drawing.Size(32, 32);
            this.pb_down_left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_down_left.TabIndex = 3;
            this.pb_down_left.TabStop = false;
            this.pb_down_left.Click += new System.EventHandler(this.Pb_down_left_Click);
            // 
            // pb_down_right
            // 
            this.pb_down_right.BackColor = System.Drawing.Color.Transparent;
            this.pb_down_right.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_down_right.Image = ((System.Drawing.Image)(resources.GetObject("pb_down_right.Image")));
            this.pb_down_right.Location = new System.Drawing.Point(300, 121);
            this.pb_down_right.Name = "pb_down_right";
            this.pb_down_right.Size = new System.Drawing.Size(32, 32);
            this.pb_down_right.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_down_right.TabIndex = 4;
            this.pb_down_right.TabStop = false;
            this.pb_down_right.Click += new System.EventHandler(this.Pb_down_right_Click);
            // 
            // pb_up_right
            // 
            this.pb_up_right.BackColor = System.Drawing.Color.Transparent;
            this.pb_up_right.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_up_right.Image = ((System.Drawing.Image)(resources.GetObject("pb_up_right.Image")));
            this.pb_up_right.Location = new System.Drawing.Point(300, 35);
            this.pb_up_right.Name = "pb_up_right";
            this.pb_up_right.Size = new System.Drawing.Size(32, 32);
            this.pb_up_right.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_up_right.TabIndex = 9;
            this.pb_up_right.TabStop = false;
            this.pb_up_right.Click += new System.EventHandler(this.Pb_up_right_Click);
            // 
            // pb_up_left
            // 
            this.pb_up_left.BackColor = System.Drawing.Color.Transparent;
            this.pb_up_left.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_up_left.Image = ((System.Drawing.Image)(resources.GetObject("pb_up_left.Image")));
            this.pb_up_left.Location = new System.Drawing.Point(4, 35);
            this.pb_up_left.Name = "pb_up_left";
            this.pb_up_left.Size = new System.Drawing.Size(32, 32);
            this.pb_up_left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_up_left.TabIndex = 8;
            this.pb_up_left.TabStop = false;
            this.pb_up_left.Click += new System.EventHandler(this.Pb_up_left_Click);
            // 
            // pb_up1
            // 
            this.pb_up1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_up1.Location = new System.Drawing.Point(42, 9);
            this.pb_up1.Name = "pb_up1";
            this.pb_up1.Size = new System.Drawing.Size(80, 80);
            this.pb_up1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_up1.TabIndex = 7;
            this.pb_up1.TabStop = false;
            this.pb_up1.Click += new System.EventHandler(this.Click_Sprite);
            // 
            // pb_up3
            // 
            this.pb_up3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_up3.Location = new System.Drawing.Point(214, 9);
            this.pb_up3.Name = "pb_up3";
            this.pb_up3.Size = new System.Drawing.Size(80, 80);
            this.pb_up3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_up3.TabIndex = 6;
            this.pb_up3.TabStop = false;
            this.pb_up3.Click += new System.EventHandler(this.Click_Sprite);
            // 
            // pb_up2
            // 
            this.pb_up2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_up2.Location = new System.Drawing.Point(128, 9);
            this.pb_up2.Name = "pb_up2";
            this.pb_up2.Size = new System.Drawing.Size(80, 80);
            this.pb_up2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_up2.TabIndex = 5;
            this.pb_up2.TabStop = false;
            this.pb_up2.Click += new System.EventHandler(this.Click_Sprite);
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.Location = new System.Drawing.Point(37, 191);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(107, 26);
            this.lbl_name.TabIndex = 10;
            this.lbl_name.Text = "Loading...";
            // 
            // pb_valid
            // 
            this.pb_valid.BackColor = System.Drawing.Color.Transparent;
            this.pb_valid.Image = ((System.Drawing.Image)(resources.GetObject("pb_valid.Image")));
            this.pb_valid.Location = new System.Drawing.Point(4, 188);
            this.pb_valid.Name = "pb_valid";
            this.pb_valid.Size = new System.Drawing.Size(32, 32);
            this.pb_valid.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_valid.TabIndex = 11;
            this.pb_valid.TabStop = false;
            // 
            // ofd_sprites
            // 
            this.ofd_sprites.Title = "Nouvelle image";
            // 
            // pb_delete
            // 
            this.pb_delete.BackColor = System.Drawing.Color.Transparent;
            this.pb_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete.Image = ((System.Drawing.Image)(resources.GetObject("pb_delete.Image")));
            this.pb_delete.Location = new System.Drawing.Point(301, -1);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(32, 32);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 12;
            this.pb_delete.TabStop = false;
            this.pb_delete.Click += new System.EventHandler(this.Pb_delete_Click);
            // 
            // AnimationEdition
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pb_delete);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.pb_valid);
            this.Controls.Add(this.pb_up_right);
            this.Controls.Add(this.pb_up_left);
            this.Controls.Add(this.pb_up1);
            this.Controls.Add(this.pb_up3);
            this.Controls.Add(this.pb_up2);
            this.Controls.Add(this.pb_down_right);
            this.Controls.Add(this.pb_down_left);
            this.Controls.Add(this.pb_down1);
            this.Controls.Add(this.pb_down3);
            this.Controls.Add(this.pb_down2);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "AnimationEdition";
            this.Size = new System.Drawing.Size(336, 223);
            this.Load += new System.EventHandler(this.AnimationEdition_Load);
            this.Click += new System.EventHandler(this.Click_Sprite);
            ((System.ComponentModel.ISupportInitialize)(this.pb_down2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_down_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_up2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_valid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_down2;
        private System.Windows.Forms.PictureBox pb_down3;
        private System.Windows.Forms.PictureBox pb_down1;
        private System.Windows.Forms.PictureBox pb_down_left;
        private System.Windows.Forms.PictureBox pb_down_right;
        private System.Windows.Forms.PictureBox pb_up_right;
        private System.Windows.Forms.PictureBox pb_up_left;
        private System.Windows.Forms.PictureBox pb_up1;
        private System.Windows.Forms.PictureBox pb_up3;
        private System.Windows.Forms.PictureBox pb_up2;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.PictureBox pb_valid;
        private System.Windows.Forms.OpenFileDialog ofd_sprites;
        private System.Windows.Forms.PictureBox pb_delete;
    }
}
