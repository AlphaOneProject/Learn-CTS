namespace Learn_CTS
{
    partial class IncrementEdition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncrementEdition));
            this.lbl_base = new System.Windows.Forms.Label();
            this.pb_delete = new System.Windows.Forms.PictureBox();
            this.txt_message = new System.Windows.Forms.TextBox();
            this.lbl_message = new System.Windows.Forms.Label();
            this.nud_score = new Learn_CTS.NumericUpDownFix();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_score)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_base
            // 
            this.lbl_base.AutoSize = true;
            this.lbl_base.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_base.Location = new System.Drawing.Point(3, 10);
            this.lbl_base.Name = "lbl_base";
            this.lbl_base.Size = new System.Drawing.Size(303, 26);
            this.lbl_base.TabIndex = 0;
            this.lbl_base.Text = "Si le score final est inférieur à ";
            // 
            // pb_delete
            // 
            this.pb_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete.Image = ((System.Drawing.Image)(resources.GetObject("pb_delete.Image")));
            this.pb_delete.Location = new System.Drawing.Point(458, 8);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(30, 30);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 3;
            this.pb_delete.TabStop = false;
            this.pb_delete.Click += new System.EventHandler(this.Pb_delete_Click);
            // 
            // txt_message
            // 
            this.txt_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_message.Location = new System.Drawing.Point(109, 54);
            this.txt_message.Name = "txt_message";
            this.txt_message.Size = new System.Drawing.Size(379, 32);
            this.txt_message.TabIndex = 5;
            this.txt_message.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_message_KeyPress);
            // 
            // lbl_message
            // 
            this.lbl_message.AutoSize = true;
            this.lbl_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_message.Location = new System.Drawing.Point(3, 57);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(100, 26);
            this.lbl_message.TabIndex = 6;
            this.lbl_message.Text = "Message";
            // 
            // nud_score
            // 
            this.nud_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_score.Location = new System.Drawing.Point(312, 8);
            this.nud_score.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_score.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_score.Name = "nud_score";
            this.nud_score.Size = new System.Drawing.Size(120, 32);
            this.nud_score.TabIndex = 4;
            this.nud_score.ValueChanged += new System.EventHandler(this.Nud_score_ValueChanged);
            this.nud_score.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Nud_score_KeyPress);
            // 
            // IncrementEdition
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.txt_message);
            this.Controls.Add(this.nud_score);
            this.Controls.Add(this.pb_delete);
            this.Controls.Add(this.lbl_base);
            this.Name = "IncrementEdition";
            this.Size = new System.Drawing.Size(495, 94);
            this.Load += new System.EventHandler(this.IncrementEdition_Load);
            this.SizeChanged += new System.EventHandler(this.IncrementEdition_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_score)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_base;
        private System.Windows.Forms.PictureBox pb_delete;
        private NumericUpDownFix nud_score;
        private System.Windows.Forms.TextBox txt_message;
        private System.Windows.Forms.Label lbl_message;
    }
}
