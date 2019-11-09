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
            this.txt_dialog_npc = new System.Windows.Forms.RichTextBox();
            this.lbl_name = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // txt_dialog_npc
            // 
            this.txt_dialog_npc.BackColor = System.Drawing.Color.White;
            this.txt_dialog_npc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_dialog_npc.ForeColor = System.Drawing.Color.Black;
            this.txt_dialog_npc.Location = new System.Drawing.Point(27, 36);
            this.txt_dialog_npc.Name = "txt_dialog_npc";
            this.txt_dialog_npc.ReadOnly = true;
            this.txt_dialog_npc.Size = new System.Drawing.Size(241, 60);
            this.txt_dialog_npc.TabIndex = 1;
            this.txt_dialog_npc.Text = "";
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Location = new System.Drawing.Point(15, 11);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(39, 13);
            this.lbl_name.TabIndex = 6;
            this.lbl_name.Text = "default";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(243, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Partir";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Dialog_Closed);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(18, 124);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(275, 35);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txt_dialog_npc);
            this.Name = "Dialog";
            this.Size = new System.Drawing.Size(306, 175);
            this.Load += new System.EventHandler(this.Dialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox txt_dialog_npc;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
