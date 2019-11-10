﻿namespace Learn_CTS
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
            this.main_menu_btn_launch_engine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // main_menu_btn_edit
            // 
            this.main_menu_btn_edit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.main_menu_btn_edit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.main_menu_btn_edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.main_menu_btn_edit.ForeColor = System.Drawing.Color.White;
            this.main_menu_btn_edit.Location = new System.Drawing.Point(278, 154);
            this.main_menu_btn_edit.Name = "main_menu_btn_edit";
            this.main_menu_btn_edit.Size = new System.Drawing.Size(230, 60);
            this.main_menu_btn_edit.TabIndex = 1;
            this.main_menu_btn_edit.Tag = "main_menu";
            this.main_menu_btn_edit.Text = "Mes jeux";
            this.main_menu_btn_edit.UseVisualStyleBackColor = false;
            this.main_menu_btn_edit.Click += new System.EventHandler(this.Main_menu_btn_edit_Click);
            // 
            // main_menu_btn_exit
            // 
            this.main_menu_btn_exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.main_menu_btn_exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.main_menu_btn_exit.ForeColor = System.Drawing.Color.White;
            this.main_menu_btn_exit.Location = new System.Drawing.Point(402, 286);
            this.main_menu_btn_exit.Name = "main_menu_btn_exit";
            this.main_menu_btn_exit.Size = new System.Drawing.Size(109, 30);
            this.main_menu_btn_exit.TabIndex = 4;
            this.main_menu_btn_exit.Tag = "main_menu";
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
            this.main_menu_btn_options.Tag = "main_menu";
            this.main_menu_btn_options.Text = "Options";
            this.main_menu_btn_options.UseVisualStyleBackColor = false;
            this.main_menu_btn_options.Click += new System.EventHandler(this.Main_menu_btn_options_Click);
            // 
            // main_menu_btn_launch_engine
            // 
            this.main_menu_btn_launch_engine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.main_menu_btn_launch_engine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.main_menu_btn_launch_engine.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.main_menu_btn_launch_engine.ForeColor = System.Drawing.Color.White;
            this.main_menu_btn_launch_engine.Location = new System.Drawing.Point(278, 88);
            this.main_menu_btn_launch_engine.Name = "main_menu_btn_launch_engine";
            this.main_menu_btn_launch_engine.Size = new System.Drawing.Size(230, 60);
            this.main_menu_btn_launch_engine.TabIndex = 0;
            this.main_menu_btn_launch_engine.Tag = "main_menu";
            this.main_menu_btn_launch_engine.Text = "Tester le moteur";
            this.main_menu_btn_launch_engine.UseVisualStyleBackColor = false;
            this.main_menu_btn_launch_engine.Click += new System.EventHandler(this.Demo_Engine);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.main_menu_btn_launch_engine);
            this.Controls.Add(this.main_menu_btn_options);
            this.Controls.Add(this.main_menu_btn_exit);
            this.Controls.Add(this.main_menu_btn_edit);
            this.ForeColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(400, 342);
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
        private System.Windows.Forms.Button main_menu_btn_options;
        private System.Windows.Forms.Button main_menu_btn_launch_engine;
    }
}