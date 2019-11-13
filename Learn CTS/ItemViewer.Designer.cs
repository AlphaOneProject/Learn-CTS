namespace Learn_CTS
{
    partial class ItemViewer
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
            this.pb_item = new System.Windows.Forms.PictureBox();
            this.lbl_desc = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Button();
            this.lbl_color = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_item)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_item
            // 
            this.pb_item.Location = new System.Drawing.Point(64, 64);
            this.pb_item.Name = "pb_item";
            this.pb_item.Size = new System.Drawing.Size(102, 91);
            this.pb_item.TabIndex = 0;
            this.pb_item.TabStop = false;
            // 
            // lbl_desc
            // 
            this.lbl_desc.AutoSize = true;
            this.lbl_desc.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F);
            this.lbl_desc.Location = new System.Drawing.Point(296, 81);
            this.lbl_desc.Name = "lbl_desc";
            this.lbl_desc.Size = new System.Drawing.Size(107, 29);
            this.lbl_desc.TabIndex = 1;
            this.lbl_desc.Text = "lbl_desc";
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(301, 210);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 2;
            this.btn_exit.Text = "OK";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.Btn_exit_Click);
            // 
            // lbl_color
            // 
            this.lbl_color.Location = new System.Drawing.Point(0, 0);
            this.lbl_color.Name = "lbl_color";
            this.lbl_color.Size = new System.Drawing.Size(42, 39);
            this.lbl_color.TabIndex = 3;
            this.lbl_color.Click += new System.EventHandler(this.Lbl_color_Click);
            // 
            // ItemViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.lbl_desc);
            this.Controls.Add(this.pb_item);
            this.Controls.Add(this.lbl_color);
            this.Name = "ItemViewer";
            this.Size = new System.Drawing.Size(431, 281);
            this.ClientSizeChanged += new System.EventHandler(this.ItemViewer_ClientSizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pb_item)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_item;
        private System.Windows.Forms.Label lbl_desc;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Label lbl_color;
    }
}
