﻿namespace Learn_CTS
{
    partial class GameSelection
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
            this.components = new System.ComponentModel.Container();
            this.flp_global = new System.Windows.Forms.FlowLayoutPanel();
            this.tlt_GameSelection = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // flp_global
            // 
            this.flp_global.AutoScroll = true;
            this.flp_global.Location = new System.Drawing.Point(12, 12);
            this.flp_global.Name = "flp_global";
            this.flp_global.Size = new System.Drawing.Size(560, 426);
            this.flp_global.TabIndex = 0;
            // 
            // tlt_GameSelection
            // 
            this.tlt_GameSelection.AutoPopDelay = 5000;
            this.tlt_GameSelection.InitialDelay = 300;
            this.tlt_GameSelection.ReshowDelay = 100;
            // 
            // GameSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.flp_global);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "GameSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choix du jeu à importer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameSelection_FormClosing);
            this.Load += new System.EventHandler(this.GameSelection_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameSelection_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flp_global;
        private System.Windows.Forms.ToolTip tlt_GameSelection;
    }
}