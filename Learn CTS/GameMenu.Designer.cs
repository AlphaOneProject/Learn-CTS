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
            this.SuspendLayout();
            // 
            // lbl_name_game
            // 
            this.lbl_name_game.AutoSize = true;
            this.lbl_name_game.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name_game.Location = new System.Drawing.Point(300, 89);
            this.lbl_name_game.Name = "lbl_name_game";
            this.lbl_name_game.Size = new System.Drawing.Size(166, 31);
            this.lbl_name_game.TabIndex = 0;
            this.lbl_name_game.Text = "LE JEUUUU";
            // 
            // GameMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_name_game);
            this.Name = "GameMenu";
            this.Text = "GameMenu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameMenu_FormClosed);
            this.Load += new System.EventHandler(this.GameMenu_Load);
            this.Resize += new System.EventHandler(this.GameMenu_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_name_game;
    }
}