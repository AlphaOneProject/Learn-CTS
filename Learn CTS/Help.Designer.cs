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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Général");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Figurants");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Dialogues");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Objets");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Personnages");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Décors");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Images", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Modèles", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Situations");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Scénarios", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
            this.tvw_help = new System.Windows.Forms.TreeView();
            this.pnl_bg = new System.Windows.Forms.Panel();
            this.lbl_help = new System.Windows.Forms.Label();
            this.pnl_bg.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvw_help
            // 
            this.tvw_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tvw_help.Location = new System.Drawing.Point(12, 12);
            this.tvw_help.Name = "tvw_help";
            treeNode1.Name = "nd_gen";
            treeNode1.Text = "Général";
            treeNode2.Name = "nd_fig";
            treeNode2.Text = "Figurants";
            treeNode3.Name = "Nœud4";
            treeNode3.Text = "Dialogues";
            treeNode4.Name = "nd_obj";
            treeNode4.Text = "Objets";
            treeNode5.Name = "nd_per";
            treeNode5.Text = "Personnages";
            treeNode6.Name = "nd_dec";
            treeNode6.Text = "Décors";
            treeNode7.Name = "nd_img";
            treeNode7.Text = "Images";
            treeNode8.Name = "nd_mod";
            treeNode8.Text = "Modèles";
            treeNode9.Name = "nd_sit";
            treeNode9.Text = "Situations";
            treeNode10.Name = "nd_sce";
            treeNode10.Text = "Scénarios";
            this.tvw_help.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode8,
            treeNode10});
            this.tvw_help.Size = new System.Drawing.Size(165, 426);
            this.tvw_help.TabIndex = 0;
            this.tvw_help.Tag = "1";
            this.tvw_help.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Tvw_help_AfterSelect);
            // 
            // pnl_bg
            // 
            this.pnl_bg.Controls.Add(this.lbl_help);
            this.pnl_bg.Location = new System.Drawing.Point(183, 12);
            this.pnl_bg.Name = "pnl_bg";
            this.pnl_bg.Size = new System.Drawing.Size(605, 426);
            this.pnl_bg.TabIndex = 1;
            this.pnl_bg.Tag = "2";
            // 
            // lbl_help
            // 
            this.lbl_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_help.Location = new System.Drawing.Point(14, 34);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvw_help;
        private System.Windows.Forms.Panel pnl_bg;
        private System.Windows.Forms.Label lbl_help;
    }
}