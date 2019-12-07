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
            this.flp_actions = new System.Windows.Forms.FlowLayoutPanel();
            this.pb_audio = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_item)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_audio)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_item
            // 
            this.pb_item.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pb_item.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pb_item.Location = new System.Drawing.Point(3, 3);
            this.pb_item.Name = "pb_item";
            this.pb_item.Size = new System.Drawing.Size(125, 121);
            this.pb_item.TabIndex = 0;
            this.pb_item.TabStop = false;
            // 
            // lbl_desc
            // 
            this.lbl_desc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lbl_desc.Location = new System.Drawing.Point(134, 3);
            this.lbl_desc.Name = "lbl_desc";
            this.lbl_desc.Size = new System.Drawing.Size(585, 90);
            this.lbl_desc.TabIndex = 1;
            this.lbl_desc.Text = "lbl_desc";
            // 
            // btn_exit
            // 
            this.btn_exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_exit.Location = new System.Drawing.Point(682, 101);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 2;
            this.btn_exit.Text = "OK";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_exit_MouseDown);
            // 
            // flp_actions
            // 
            this.flp_actions.Location = new System.Drawing.Point(134, 96);
            this.flp_actions.Name = "flp_actions";
            this.flp_actions.Size = new System.Drawing.Size(542, 28);
            this.flp_actions.TabIndex = 3;
            // 
            // pb_audio
            // 
            this.pb_audio.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pb_audio.Location = new System.Drawing.Point(725, 3);
            this.pb_audio.Name = "pb_audio";
            this.pb_audio.Size = new System.Drawing.Size(32, 32);
            this.pb_audio.TabIndex = 10;
            this.pb_audio.TabStop = false;
            // 
            // ItemViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.pb_audio);
            this.Controls.Add(this.flp_actions);
            this.Controls.Add(this.lbl_desc);
            this.Controls.Add(this.pb_item);
            this.Name = "ItemViewer";
            this.Size = new System.Drawing.Size(760, 128);
            this.Load += new System.EventHandler(this.ItemViewer_Load);
            this.ClientSizeChanged += new System.EventHandler(this.ItemViewer_ClientSizeChanged);
            this.Leave += new System.EventHandler(this.ItemViewer_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.pb_item)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_audio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_item;
        private System.Windows.Forms.Label lbl_desc;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.FlowLayoutPanel flp_actions;
        private System.Windows.Forms.PictureBox pb_audio;
    }
}
