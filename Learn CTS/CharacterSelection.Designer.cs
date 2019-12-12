namespace Learn_CTS
{
    partial class CharacterSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterSelection));
            this.pb_right = new System.Windows.Forms.PictureBox();
            this.pb_left = new System.Windows.Forms.PictureBox();
            this.pb_1 = new System.Windows.Forms.PictureBox();
            this.pb_3 = new System.Windows.Forms.PictureBox();
            this.pb_2 = new System.Windows.Forms.PictureBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.pb_delete = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_right
            // 
            this.pb_right.BackColor = System.Drawing.Color.Transparent;
            this.pb_right.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pb_right.BackgroundImage")));
            this.pb_right.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_right.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_right.Location = new System.Drawing.Point(324, 70);
            this.pb_right.Name = "pb_right";
            this.pb_right.Size = new System.Drawing.Size(80, 80);
            this.pb_right.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_right.TabIndex = 17;
            this.pb_right.TabStop = false;
            this.pb_right.Click += new System.EventHandler(this.Pb_right_Click);
            // 
            // pb_left
            // 
            this.pb_left.BackColor = System.Drawing.Color.Transparent;
            this.pb_left.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pb_left.BackgroundImage")));
            this.pb_left.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_left.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_left.Location = new System.Drawing.Point(126, 70);
            this.pb_left.Name = "pb_left";
            this.pb_left.Size = new System.Drawing.Size(80, 80);
            this.pb_left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_left.TabIndex = 16;
            this.pb_left.TabStop = false;
            this.pb_left.Click += new System.EventHandler(this.Pb_left_Click);
            // 
            // pb_1
            // 
            this.pb_1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_1.Location = new System.Drawing.Point(7, 49);
            this.pb_1.Name = "pb_1";
            this.pb_1.Size = new System.Drawing.Size(120, 120);
            this.pb_1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_1.TabIndex = 15;
            this.pb_1.TabStop = false;
            // 
            // pb_3
            // 
            this.pb_3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_3.Location = new System.Drawing.Point(403, 49);
            this.pb_3.Name = "pb_3";
            this.pb_3.Size = new System.Drawing.Size(120, 120);
            this.pb_3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_3.TabIndex = 14;
            this.pb_3.TabStop = false;
            // 
            // pb_2
            // 
            this.pb_2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_2.Location = new System.Drawing.Point(205, 49);
            this.pb_2.Name = "pb_2";
            this.pb_2.Size = new System.Drawing.Size(120, 120);
            this.pb_2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_2.TabIndex = 13;
            this.pb_2.TabStop = false;
            // 
            // txt_name
            // 
            this.txt_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_name.Location = new System.Drawing.Point(142, 14);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(246, 29);
            this.txt_name.TabIndex = 18;
            // 
            // pb_delete
            // 
            this.pb_delete.BackColor = System.Drawing.Color.Transparent;
            this.pb_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete.Image = ((System.Drawing.Image)(resources.GetObject("pb_delete.Image")));
            this.pb_delete.Location = new System.Drawing.Point(491, 3);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(32, 32);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 19;
            this.pb_delete.TabStop = false;
            this.pb_delete.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // CharacterSelection
            // 
            this.Controls.Add(this.pb_delete);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.pb_right);
            this.Controls.Add(this.pb_left);
            this.Controls.Add(this.pb_1);
            this.Controls.Add(this.pb_3);
            this.Controls.Add(this.pb_2);
            this.Name = "CharacterSelection";
            this.Size = new System.Drawing.Size(529, 181);
            this.Load += new System.EventHandler(this.CharacterSelection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pb_right;
        private System.Windows.Forms.PictureBox pb_left;
        private System.Windows.Forms.PictureBox pb_1;
        private System.Windows.Forms.PictureBox pb_3;
        private System.Windows.Forms.PictureBox pb_2;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.PictureBox pb_delete;
    }
}