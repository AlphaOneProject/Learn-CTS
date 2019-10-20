namespace Learn_CTS
{
    partial class Dialog
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
            this.pbox_head_npc = new System.Windows.Forms.PictureBox();
            this.txt_dialog_npc = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbox_head_npc)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox_head_npc
            // 
            this.pbox_head_npc.Location = new System.Drawing.Point(25, 20);
            this.pbox_head_npc.Name = "pbox_head_npc";
            this.pbox_head_npc.Size = new System.Drawing.Size(110, 116);
            this.pbox_head_npc.TabIndex = 0;
            this.pbox_head_npc.TabStop = false;
            // 
            // txt_dialog_npc
            // 
            this.txt_dialog_npc.Location = new System.Drawing.Point(173, 20);
            this.txt_dialog_npc.Name = "txt_dialog_npc";
            this.txt_dialog_npc.ReadOnly = true;
            this.txt_dialog_npc.Size = new System.Drawing.Size(1047, 116);
            this.txt_dialog_npc.TabIndex = 1;
            this.txt_dialog_npc.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Homme de fer.";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(320, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Colonne.";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(449, 172);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Partir";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_dialog_npc);
            this.Controls.Add(this.pbox_head_npc);
            this.Name = "Dialog";
            this.Size = new System.Drawing.Size(1280, 240);
            this.Load += new System.EventHandler(this.Dialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbox_head_npc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox_head_npc;
        private System.Windows.Forms.RichTextBox txt_dialog_npc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
    }
}
