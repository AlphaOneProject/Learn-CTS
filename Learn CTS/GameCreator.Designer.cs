namespace Learn_CTS
{
    partial class GameCreator
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
            this.pb_confirm = new System.Windows.Forms.PictureBox();
            this.txt_create = new System.Windows.Forms.TextBox();
            this.pb_back_create = new System.Windows.Forms.PictureBox();
            this.lbl_create = new System.Windows.Forms.Label();
            this.pnl_bg = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pb_confirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_back_create)).BeginInit();
            this.pnl_bg.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb_confirm
            // 
            this.pb_confirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pb_confirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_confirm.Location = new System.Drawing.Point(299, 92);
            this.pb_confirm.Name = "pb_confirm";
            this.pb_confirm.Size = new System.Drawing.Size(60, 61);
            this.pb_confirm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_confirm.TabIndex = 3;
            this.pb_confirm.TabStop = false;
            this.pb_confirm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Create_game_verify);
            // 
            // txt_create
            // 
            this.txt_create.Location = new System.Drawing.Point(69, 57);
            this.txt_create.Name = "txt_create";
            this.txt_create.Size = new System.Drawing.Size(231, 20);
            this.txt_create.TabIndex = 2;
            this.txt_create.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_create_KeyPress);
            // 
            // pb_back_create
            // 
            this.pb_back_create.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pb_back_create.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_back_create.Location = new System.Drawing.Point(11, 12);
            this.pb_back_create.Name = "pb_back_create";
            this.pb_back_create.Size = new System.Drawing.Size(36, 36);
            this.pb_back_create.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_back_create.TabIndex = 1;
            this.pb_back_create.TabStop = false;
            this.pb_back_create.Click += new System.EventHandler(this.Pb_back_create_Click);
            // 
            // lbl_create
            // 
            this.lbl_create.AutoSize = true;
            this.lbl_create.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lbl_create.ForeColor = System.Drawing.Color.White;
            this.lbl_create.Location = new System.Drawing.Point(86, 24);
            this.lbl_create.Name = "lbl_create";
            this.lbl_create.Size = new System.Drawing.Size(194, 24);
            this.lbl_create.TabIndex = 4;
            this.lbl_create.Text = "Créer un nouveau jeu";
            // 
            // pnl_bg
            // 
            this.pnl_bg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pnl_bg.Controls.Add(this.lbl_create);
            this.pnl_bg.Controls.Add(this.pb_back_create);
            this.pnl_bg.Controls.Add(this.txt_create);
            this.pnl_bg.Controls.Add(this.pb_confirm);
            this.pnl_bg.Location = new System.Drawing.Point(0, 0);
            this.pnl_bg.Name = "pnl_bg";
            this.pnl_bg.Size = new System.Drawing.Size(372, 157);
            this.pnl_bg.TabIndex = 5;
            // 
            // GameCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnl_bg);
            this.Name = "GameCreator";
            this.Size = new System.Drawing.Size(372, 160);
            this.SizeChanged += new System.EventHandler(this.GameCreator_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pb_confirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_back_create)).EndInit();
            this.pnl_bg.ResumeLayout(false);
            this.pnl_bg.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_confirm;
        private System.Windows.Forms.TextBox txt_create;
        private System.Windows.Forms.PictureBox pb_back_create;
        private System.Windows.Forms.Label lbl_create;
        private System.Windows.Forms.Panel pnl_bg;
    }
}
