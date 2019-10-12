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
            this.SuspendLayout();
            // 
            // main_menu_btn_edit
            // 
            this.main_menu_btn_edit.Location = new System.Drawing.Point(75, 111);
            this.main_menu_btn_edit.Name = "main_menu_btn_edit";
            this.main_menu_btn_edit.Size = new System.Drawing.Size(75, 23);
            this.main_menu_btn_edit.TabIndex = 1;
            this.main_menu_btn_edit.Tag = "main_menu";
            this.main_menu_btn_edit.Text = "Editer";
            this.main_menu_btn_edit.UseVisualStyleBackColor = true;
            this.main_menu_btn_edit.Click += new System.EventHandler(this.Main_menu_btn_edit_Click);
            // 
            // main_menu_btn_exit
            // 
            this.main_menu_btn_exit.Location = new System.Drawing.Point(75, 336);
            this.main_menu_btn_exit.Name = "main_menu_btn_exit";
            this.main_menu_btn_exit.Size = new System.Drawing.Size(75, 23);
            this.main_menu_btn_exit.TabIndex = 2;
            this.main_menu_btn_exit.Tag = "main_menu";
            this.main_menu_btn_exit.Text = "Quitter";
            this.main_menu_btn_exit.UseVisualStyleBackColor = true;
            this.main_menu_btn_exit.Click += new System.EventHandler(this.Main_menu_btn_exit_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.main_menu_btn_exit);
            this.Controls.Add(this.main_menu_btn_edit);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Learn CTS";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.SizeChanged += new System.EventHandler(this.Menu_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button main_menu_btn_edit;
        private System.Windows.Forms.Button main_menu_btn_exit;
    }
}