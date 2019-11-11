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
            this.flp_choices = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // txt_dialog_npc
            // 
            this.txt_dialog_npc.BackColor = System.Drawing.Color.White;
            this.txt_dialog_npc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_dialog_npc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_dialog_npc.ForeColor = System.Drawing.Color.Black;
            this.txt_dialog_npc.Location = new System.Drawing.Point(28, 52);
            this.txt_dialog_npc.Name = "txt_dialog_npc";
            this.txt_dialog_npc.ReadOnly = true;
            this.txt_dialog_npc.Size = new System.Drawing.Size(241, 60);
            this.txt_dialog_npc.TabIndex = 1;
            this.txt_dialog_npc.Text = "blablabla";
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.Location = new System.Drawing.Point(15, 11);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(71, 22);
            this.lbl_name.TabIndex = 6;
            this.lbl_name.Text = "default";
            // 
            // flp_choices
            // 
            this.flp_choices.AutoSize = true;
            this.flp_choices.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flp_choices.Location = new System.Drawing.Point(18, 124);
            this.flp_choices.Name = "flp_choices";
            this.flp_choices.Size = new System.Drawing.Size(0, 0);
            this.flp_choices.TabIndex = 8;
            // 
            // Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.flp_choices);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.txt_dialog_npc);
            this.Name = "Dialog";
            this.Size = new System.Drawing.Size(302, 171);
            this.Load += new System.EventHandler(this.Dialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox txt_dialog_npc;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.FlowLayoutPanel flp_choices;
    }
}
