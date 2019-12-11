namespace Learn_CTS
{
    partial class ChooseCharacter
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
            this.pbox_char = new System.Windows.Forms.PictureBox();
            this.pbox_arrow_left = new System.Windows.Forms.PictureBox();
            this.pbox_arrow_right = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_char)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_arrow_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_arrow_right)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox_char
            // 
            this.pbox_char.Location = new System.Drawing.Point(118, 14);
            this.pbox_char.Name = "pbox_char";
            this.pbox_char.Size = new System.Drawing.Size(91, 91);
            this.pbox_char.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_char.TabIndex = 0;
            this.pbox_char.TabStop = false;
            // 
            // pbox_arrow_left
            // 
            this.pbox_arrow_left.Location = new System.Drawing.Point(52, 33);
            this.pbox_arrow_left.Name = "pbox_arrow_left";
            this.pbox_arrow_left.Size = new System.Drawing.Size(60, 58);
            this.pbox_arrow_left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_arrow_left.TabIndex = 2;
            this.pbox_arrow_left.TabStop = false;
            this.pbox_arrow_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbox_arrow_left_MouseDown);
            // 
            // pbox_arrow_right
            // 
            this.pbox_arrow_right.Location = new System.Drawing.Point(215, 33);
            this.pbox_arrow_right.Name = "pbox_arrow_right";
            this.pbox_arrow_right.Size = new System.Drawing.Size(60, 58);
            this.pbox_arrow_right.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox_arrow_right.TabIndex = 3;
            this.pbox_arrow_right.TabStop = false;
            this.pbox_arrow_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbox_arrow_right_MouseDown);
            // 
            // ChooseCharacter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pbox_arrow_right);
            this.Controls.Add(this.pbox_arrow_left);
            this.Controls.Add(this.pbox_char);
            this.Name = "ChooseCharacter";
            this.Size = new System.Drawing.Size(338, 126);
            this.Load += new System.EventHandler(this.ChooseCharacter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbox_char)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_arrow_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_arrow_right)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox_char;
        private System.Windows.Forms.PictureBox pbox_arrow_left;
        private System.Windows.Forms.PictureBox pbox_arrow_right;
    }
}
