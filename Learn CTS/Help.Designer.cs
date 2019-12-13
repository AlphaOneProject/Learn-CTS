namespace Learn_CTS
{
    partial class Help
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
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Général");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Figurants");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Dialogues");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Objets");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Personnages");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Décors");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Images", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Modèles", new System.Windows.Forms.TreeNode[] {
            treeNode22,
            treeNode23,
            treeNode27});
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Situations");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Scénarios", new System.Windows.Forms.TreeNode[] {
            treeNode29});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
            this.tvw_help = new System.Windows.Forms.TreeView();
            this.pnl_bg = new System.Windows.Forms.Panel();
            this.pb_help = new System.Windows.Forms.PictureBox();
            this.lbl_help = new System.Windows.Forms.Label();
            this.pnl_bg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_help)).BeginInit();
            this.SuspendLayout();
            // 
            // tvw_help
            // 
            this.tvw_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tvw_help.Location = new System.Drawing.Point(12, 12);
            this.tvw_help.Name = "tvw_help";
            treeNode21.Name = "nd_gen";
            treeNode21.Text = "Général";
            treeNode22.Name = "nd_fig";
            treeNode22.Text = "Figurants";
            treeNode23.Name = "Nœud4";
            treeNode23.Text = "Dialogues";
            treeNode24.Name = "nd_obj";
            treeNode24.Text = "Objets";
            treeNode25.Name = "nd_per";
            treeNode25.Text = "Personnages";
            treeNode26.Name = "nd_dec";
            treeNode26.Text = "Décors";
            treeNode27.Name = "nd_img";
            treeNode27.Text = "Images";
            treeNode28.Name = "nd_mod";
            treeNode28.Text = "Modèles";
            treeNode29.Name = "nd_sit";
            treeNode29.Text = "Situations";
            treeNode30.Name = "nd_sce";
            treeNode30.Text = "Scénarios";
            this.tvw_help.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode21,
            treeNode28,
            treeNode30});
            this.tvw_help.Size = new System.Drawing.Size(165, 426);
            this.tvw_help.TabIndex = 0;
            this.tvw_help.Tag = "1";
            this.tvw_help.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Tvw_help_AfterSelect);
            // 
            // pnl_bg
            // 
            this.pnl_bg.Controls.Add(this.pb_help);
            this.pnl_bg.Controls.Add(this.lbl_help);
            this.pnl_bg.Location = new System.Drawing.Point(183, 12);
            this.pnl_bg.Name = "pnl_bg";
            this.pnl_bg.Size = new System.Drawing.Size(605, 426);
            this.pnl_bg.TabIndex = 1;
            this.pnl_bg.Tag = "2";
            // 
            // pb_help
            // 
            this.pb_help.BackColor = System.Drawing.Color.Transparent;
            this.pb_help.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_help.Location = new System.Drawing.Point(0, -5);
            this.pb_help.Name = "pb_help";
            this.pb_help.Size = new System.Drawing.Size(570, 359);
            this.pb_help.TabIndex = 1;
            this.pb_help.TabStop = false;
            // 
            // lbl_help
            // 
            this.lbl_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_help.Location = new System.Drawing.Point(17, 34);
            this.lbl_help.Name = "lbl_help";
            this.lbl_help.Size = new System.Drawing.Size(574, 359);
            this.lbl_help.TabIndex = 0;
            this.lbl_help.Text = resources.GetString("lbl_help.Text");
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnl_bg);
            this.Controls.Add(this.tvw_help);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Help";
            this.Tag = "0";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.Help_Load);
            this.pnl_bg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_help)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvw_help;
        private System.Windows.Forms.Panel pnl_bg;
        private System.Windows.Forms.Label lbl_help;
        private System.Windows.Forms.PictureBox pb_help;
    }
}