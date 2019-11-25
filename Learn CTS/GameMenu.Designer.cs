namespace Learn_CTS
{
    partial class GameMenu
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
            this.lbl_name_game = new System.Windows.Forms.Label();
            this.btn_launch_scenario = new System.Windows.Forms.Button();
            this.btn_leave = new System.Windows.Forms.Button();
            this.btn_options = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_name_game
            // 
            this.lbl_name_game.AutoSize = true;
            this.lbl_name_game.Location = new System.Drawing.Point(350, 99);
            this.lbl_name_game.Name = "lbl_name_game";
            this.lbl_name_game.Size = new System.Drawing.Size(67, 13);
            this.lbl_name_game.TabIndex = 0;
            this.lbl_name_game.Text = "LE JEUUUU";
            // 
            // btn_launch_scenario
            // 
            this.btn_launch_scenario.Location = new System.Drawing.Point(296, 193);
            this.btn_launch_scenario.Name = "btn_launch_scenario";
            this.btn_launch_scenario.Size = new System.Drawing.Size(179, 23);
            this.btn_launch_scenario.TabIndex = 1;
            this.btn_launch_scenario.Text = "Lancer un scénario";
            this.btn_launch_scenario.UseVisualStyleBackColor = true;
            this.btn_launch_scenario.Click += new System.EventHandler(this.btn_launch_scenario_Click);
            // 
            // btn_leave
            // 
            this.btn_leave.Location = new System.Drawing.Point(296, 251);
            this.btn_leave.Name = "btn_leave";
            this.btn_leave.Size = new System.Drawing.Size(179, 23);
            this.btn_leave.TabIndex = 3;
            this.btn_leave.Text = "Quitter";
            this.btn_leave.UseVisualStyleBackColor = true;
            this.btn_leave.Click += new System.EventHandler(this.button_leave_Click);
            // 
            // btn_options
            // 
            this.btn_options.Location = new System.Drawing.Point(296, 222);
            this.btn_options.Name = "btn_options";
            this.btn_options.Size = new System.Drawing.Size(179, 23);
            this.btn_options.TabIndex = 4;
            this.btn_options.Text = "Options";
            this.btn_options.UseVisualStyleBackColor = true;
            // 
            // GameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_options);
            this.Controls.Add(this.btn_leave);
            this.Controls.Add(this.btn_launch_scenario);
            this.Controls.Add(this.lbl_name_game);
            this.Name = "GameMenu";
            this.Text = "GameMenu";
            this.Resize += new System.EventHandler(this.GameMenu_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_name_game;
        private System.Windows.Forms.Button btn_launch_scenario;
        private System.Windows.Forms.Button btn_leave;
        private System.Windows.Forms.Button btn_options;
    }
}