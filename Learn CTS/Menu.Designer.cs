namespace Learn_CTS
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.main_menu_btn_edit = new System.Windows.Forms.Button();
            this.main_menu_btn_exit = new System.Windows.Forms.Button();
            this.main_menu_btn_options = new System.Windows.Forms.Button();
            this.main_menu_lbl_title1 = new System.Windows.Forms.Label();
            this.main_menu_lbl_title2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // main_menu_btn_edit
            // 
            this.main_menu_btn_edit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.main_menu_btn_edit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.main_menu_btn_edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.main_menu_btn_edit.ForeColor = System.Drawing.Color.White;
            this.main_menu_btn_edit.Location = new System.Drawing.Point(278, 220);
            this.main_menu_btn_edit.Name = "main_menu_btn_edit";
            this.main_menu_btn_edit.Size = new System.Drawing.Size(230, 60);
            this.main_menu_btn_edit.TabIndex = 1;
            this.main_menu_btn_edit.Tag = "3";
            this.main_menu_btn_edit.Text = "Mes jeux";
            this.main_menu_btn_edit.UseVisualStyleBackColor = false;
            this.main_menu_btn_edit.Click += new System.EventHandler(this.Main_menu_btn_edit_Click);
            // 
            // main_menu_btn_exit
            // 
            this.main_menu_btn_exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.main_menu_btn_exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.main_menu_btn_exit.ForeColor = System.Drawing.Color.White;
            this.main_menu_btn_exit.Location = new System.Drawing.Point(399, 286);
            this.main_menu_btn_exit.Name = "main_menu_btn_exit";
            this.main_menu_btn_exit.Size = new System.Drawing.Size(109, 30);
            this.main_menu_btn_exit.TabIndex = 4;
            this.main_menu_btn_exit.Tag = "3";
            this.main_menu_btn_exit.Text = "Quitter";
            this.main_menu_btn_exit.UseVisualStyleBackColor = false;
            this.main_menu_btn_exit.Click += new System.EventHandler(this.Main_menu_btn_exit_Click);
            // 
            // main_menu_btn_options
            // 
            this.main_menu_btn_options.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.main_menu_btn_options.Cursor = System.Windows.Forms.Cursors.Hand;
            this.main_menu_btn_options.ForeColor = System.Drawing.Color.White;
            this.main_menu_btn_options.Location = new System.Drawing.Point(278, 286);
            this.main_menu_btn_options.Name = "main_menu_btn_options";
            this.main_menu_btn_options.Size = new System.Drawing.Size(109, 30);
            this.main_menu_btn_options.TabIndex = 3;
            this.main_menu_btn_options.Tag = "3";
            this.main_menu_btn_options.Text = "Options";
            this.main_menu_btn_options.UseVisualStyleBackColor = false;
            this.main_menu_btn_options.Click += new System.EventHandler(this.Main_menu_btn_options_Click);
            // 
            // main_menu_lbl_title1
            // 
            this.main_menu_lbl_title1.AutoSize = true;
            this.main_menu_lbl_title1.Font = new System.Drawing.Font("Nirmala UI Semilight", 36F);
            this.main_menu_lbl_title1.Location = new System.Drawing.Point(276, 31);
            this.main_menu_lbl_title1.Name = "main_menu_lbl_title1";
            this.main_menu_lbl_title1.Size = new System.Drawing.Size(141, 65);
            this.main_menu_lbl_title1.TabIndex = 5;
            this.main_menu_lbl_title1.Text = "Learn";
            // 
            // main_menu_lbl_title2
            // 
            this.main_menu_lbl_title2.AutoSize = true;
            this.main_menu_lbl_title2.Font = new System.Drawing.Font("Nirmala UI", 44F, System.Drawing.FontStyle.Bold);
            this.main_menu_lbl_title2.ForeColor = System.Drawing.Color.Red;
            this.main_menu_lbl_title2.Location = new System.Drawing.Point(398, 20);
            this.main_menu_lbl_title2.Name = "main_menu_lbl_title2";
            this.main_menu_lbl_title2.Size = new System.Drawing.Size(138, 78);
            this.main_menu_lbl_title2.TabIndex = 6;
            this.main_menu_lbl_title2.Text = "CTS";
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(800, 461);
            this.Controls.Add(this.main_menu_lbl_title2);
            this.Controls.Add(this.main_menu_lbl_title1);
            this.Controls.Add(this.main_menu_btn_options);
            this.Controls.Add(this.main_menu_btn_exit);
            this.Controls.Add(this.main_menu_btn_edit);
            this.ForeColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Learn CTS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Menu_FormClosing);
            this.Load += new System.EventHandler(this.Menu_Load);
            this.SizeChanged += new System.EventHandler(this.Menu_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button main_menu_btn_edit;
        private System.Windows.Forms.Button main_menu_btn_exit;
        private System.Windows.Forms.Button main_menu_btn_options;
        private System.Windows.Forms.Label main_menu_lbl_title1;
        private System.Windows.Forms.Label main_menu_lbl_title2;
    }
}