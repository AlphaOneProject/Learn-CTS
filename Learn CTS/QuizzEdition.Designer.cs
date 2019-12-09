namespace Learn_CTS
{
    partial class QuizzEdition
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuizzEdition));
            this.txt_question = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pb_add = new System.Windows.Forms.PictureBox();
            this.pb_delete_all = new System.Windows.Forms.PictureBox();
            this.cbo_audio = new Learn_CTS.ComboBoxFix();
            ((System.ComponentModel.ISupportInitialize)(this.pb_add)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete_all)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_question
            // 
            this.txt_question.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.txt_question.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_question.ForeColor = System.Drawing.Color.White;
            this.txt_question.Location = new System.Drawing.Point(10, 10);
            this.txt_question.MaximumSize = new System.Drawing.Size(920, 30);
            this.txt_question.MinimumSize = new System.Drawing.Size(60, 30);
            this.txt_question.Name = "txt_question";
            this.txt_question.ShortcutsEnabled = false;
            this.txt_question.Size = new System.Drawing.Size(420, 30);
            this.txt_question.TabIndex = 0;
            this.toolTip.SetToolTip(this.txt_question, "Question à poser.");
            this.txt_question.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_Question_KeyPress);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 300;
            this.toolTip.IsBalloon = true;
            this.toolTip.OwnerDraw = true;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Information";
            // 
            // pb_add
            // 
            this.pb_add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_add.Image = ((System.Drawing.Image)(resources.GetObject("pb_add.Image")));
            this.pb_add.Location = new System.Drawing.Point(438, 10);
            this.pb_add.Name = "pb_add";
            this.pb_add.Size = new System.Drawing.Size(30, 30);
            this.pb_add.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_add.TabIndex = 1;
            this.pb_add.TabStop = false;
            this.toolTip.SetToolTip(this.pb_add, "Ajouter une réponse.");
            this.pb_add.Click += new System.EventHandler(this.Add_Choice);
            // 
            // pb_delete_all
            // 
            this.pb_delete_all.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete_all.Image = ((System.Drawing.Image)(resources.GetObject("pb_delete_all.Image")));
            this.pb_delete_all.Location = new System.Drawing.Point(785, 10);
            this.pb_delete_all.Name = "pb_delete_all";
            this.pb_delete_all.Size = new System.Drawing.Size(30, 30);
            this.pb_delete_all.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete_all.TabIndex = 2;
            this.pb_delete_all.TabStop = false;
            this.toolTip.SetToolTip(this.pb_delete_all, "Supprimer ce dialogue.");
            this.pb_delete_all.Click += new System.EventHandler(this.Delete_All);
            // 
            // cbo_audio
            // 
            this.cbo_audio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.cbo_audio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_audio.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_audio.ForeColor = System.Drawing.Color.White;
            this.cbo_audio.FormattingEnabled = true;
            this.cbo_audio.Items.AddRange(new object[] {
            "Texte uniquement",
            "Texte et audio",
            "Audio uniquement"});
            this.cbo_audio.Location = new System.Drawing.Point(492, 9);
            this.cbo_audio.Name = "cbo_audio";
            this.cbo_audio.Size = new System.Drawing.Size(180, 32);
            this.cbo_audio.TabIndex = 3;
            this.toolTip.SetToolTip(this.cbo_audio, "Gestion de l\'audio du dialogue.");
            this.cbo_audio.SelectedIndexChanged += new System.EventHandler(this.Cbo_audio_SelectedIndexChanged);
            // 
            // QuizzEdition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbo_audio);
            this.Controls.Add(this.pb_delete_all);
            this.Controls.Add(this.pb_add);
            this.Controls.Add(this.txt_question);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "QuizzEdition";
            this.Size = new System.Drawing.Size(820, 60);
            this.Load += new System.EventHandler(this.QuizzEdition_Load);
            this.Resize += new System.EventHandler(this.QuizzEdition_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pb_add)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete_all)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_question;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pb_add;
        private System.Windows.Forms.PictureBox pb_delete_all;
        private ComboBoxFix cbo_audio;
    }
}
