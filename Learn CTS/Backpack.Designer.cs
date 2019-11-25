namespace Learn_CTS
{
    partial class Backpack
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
            this.pbox_ticket = new System.Windows.Forms.PictureBox();
            this.pbox_map = new System.Windows.Forms.PictureBox();
            this.pbox_phone = new System.Windows.Forms.PictureBox();
            this.lbl_backpack = new System.Windows.Forms.Label();
            this.pbox_backpack = new System.Windows.Forms.PictureBox();
            this.pbox_close = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_ticket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_map)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_phone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_backpack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_close)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox_ticket
            // 
            this.pbox_ticket.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbox_ticket.Location = new System.Drawing.Point(27, 90);
            this.pbox_ticket.Name = "pbox_ticket";
            this.pbox_ticket.Size = new System.Drawing.Size(128, 128);
            this.pbox_ticket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_ticket.TabIndex = 0;
            this.pbox_ticket.TabStop = false;
            // 
            // pbox_map
            // 
            this.pbox_map.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbox_map.Location = new System.Drawing.Point(176, 90);
            this.pbox_map.Name = "pbox_map";
            this.pbox_map.Size = new System.Drawing.Size(128, 128);
            this.pbox_map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_map.TabIndex = 1;
            this.pbox_map.TabStop = false;
            // 
            // pbox_phone
            // 
            this.pbox_phone.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbox_phone.Location = new System.Drawing.Point(329, 90);
            this.pbox_phone.Name = "pbox_phone";
            this.pbox_phone.Size = new System.Drawing.Size(128, 128);
            this.pbox_phone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_phone.TabIndex = 2;
            this.pbox_phone.TabStop = false;
            this.pbox_phone.Click += new System.EventHandler(this.pbox_phone_Click);
            // 
            // lbl_backpack
            // 
            this.lbl_backpack.AutoSize = true;
            this.lbl_backpack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_backpack.Location = new System.Drawing.Point(73, 23);
            this.lbl_backpack.Name = "lbl_backpack";
            this.lbl_backpack.Size = new System.Drawing.Size(93, 24);
            this.lbl_backpack.TabIndex = 3;
            this.lbl_backpack.Text = "Sac à dos";
            // 
            // pbox_backpack
            // 
            this.pbox_backpack.Location = new System.Drawing.Point(3, 3);
            this.pbox_backpack.Name = "pbox_backpack";
            this.pbox_backpack.Size = new System.Drawing.Size(64, 64);
            this.pbox_backpack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_backpack.TabIndex = 4;
            this.pbox_backpack.TabStop = false;
            // 
            // pbox_close
            // 
            this.pbox_close.Location = new System.Drawing.Point(454, 3);
            this.pbox_close.Name = "pbox_close";
            this.pbox_close.Size = new System.Drawing.Size(32, 32);
            this.pbox_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_close.TabIndex = 5;
            this.pbox_close.TabStop = false;
            // 
            // Backpack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbox_close);
            this.Controls.Add(this.pbox_backpack);
            this.Controls.Add(this.lbl_backpack);
            this.Controls.Add(this.pbox_phone);
            this.Controls.Add(this.pbox_map);
            this.Controls.Add(this.pbox_ticket);
            this.Name = "Backpack";
            this.Size = new System.Drawing.Size(489, 309);
            this.Load += new System.EventHandler(this.Backpack_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Backpack_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbox_ticket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_map)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_phone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_backpack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_close)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox_ticket;
        private System.Windows.Forms.PictureBox pbox_map;
        private System.Windows.Forms.PictureBox pbox_phone;
        private System.Windows.Forms.Label lbl_backpack;
        private System.Windows.Forms.PictureBox pbox_backpack;
        private System.Windows.Forms.PictureBox pbox_close;
    }
}
