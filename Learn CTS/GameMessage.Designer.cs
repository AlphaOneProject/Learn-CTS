namespace Learn_CTS
{
    partial class GameMessage
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
            this.rtxtbox_msg = new System.Windows.Forms.RichTextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxtbox_msg
            // 
            this.rtxtbox_msg.Location = new System.Drawing.Point(16, 13);
            this.rtxtbox_msg.Name = "rtxtbox_msg";
            this.rtxtbox_msg.Size = new System.Drawing.Size(282, 112);
            this.rtxtbox_msg.TabIndex = 1;
            this.rtxtbox_msg.Text = "";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(121, 131);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 27);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "Compris";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // GameMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.rtxtbox_msg);
            this.Name = "GameMessage";
            this.Size = new System.Drawing.Size(315, 161);
            this.Load += new System.EventHandler(this.GameMessage_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtxtbox_msg;
        private System.Windows.Forms.Button btn_ok;
    }
}
