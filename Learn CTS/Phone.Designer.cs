namespace Learn_CTS
{
    partial class Phone
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_answer = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flp_talk = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "NPC Name";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.flp_talk);
            this.panel1.Controls.Add(this.txt_answer);
            this.panel1.Location = new System.Drawing.Point(15, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 590);
            this.panel1.TabIndex = 1;
            // 
            // txt_answer
            // 
            this.txt_answer.Location = new System.Drawing.Point(24, 556);
            this.txt_answer.Name = "txt_answer";
            this.txt_answer.Size = new System.Drawing.Size(292, 20);
            this.txt_answer.TabIndex = 3;
            this.txt_answer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_answer_KeyDown);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.ForestGreen;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(15, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(343, 53);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(15, 38);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(343, 21);
            this.panel3.TabIndex = 1;
            // 
            // flp_talk
            // 
            this.flp_talk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flp_talk.AutoScroll = true;
            this.flp_talk.BackColor = System.Drawing.Color.LightGray;
            this.flp_talk.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flp_talk.Location = new System.Drawing.Point(24, 88);
            this.flp_talk.Name = "flp_talk";
            this.flp_talk.Size = new System.Drawing.Size(292, 451);
            this.flp_talk.TabIndex = 0;
            this.flp_talk.WrapContents = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(343, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Remettre téléphone dans le sac";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Phone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Phone";
            this.Size = new System.Drawing.Size(372, 643);
            this.Load += new System.EventHandler(this.Phone_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txt_answer;
        private System.Windows.Forms.FlowLayoutPanel flp_talk;
        private System.Windows.Forms.Button button1;
    }
}
