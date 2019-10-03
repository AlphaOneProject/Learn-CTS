namespace Learn_CTS
{
    partial class Editor1
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
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Général");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Non-joueurs");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Joueur");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Personnages", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Scénarios");
            this.menu = new System.Windows.Forms.TreeView();
            this.content = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.FullRowSelect = true;
            this.menu.ItemHeight = 32;
            this.menu.Location = new System.Drawing.Point(12, 12);
            this.menu.Name = "menu";
            treeNode6.Name = "global";
            treeNode6.Text = "Général";
            treeNode7.Name = "npcs";
            treeNode7.Text = "Non-joueurs";
            treeNode8.Name = "player";
            treeNode8.Text = "Joueur";
            treeNode9.Name = "characters";
            treeNode9.Text = "Personnages";
            treeNode10.Name = "scenarios";
            treeNode10.Text = "Scénarios";
            this.menu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode9,
            treeNode10});
            this.menu.PathSeparator = " / ";
            this.menu.ShowLines = false;
            this.menu.ShowNodeToolTips = true;
            this.menu.Size = new System.Drawing.Size(202, 618);
            this.menu.TabIndex = 0;
            this.menu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Menu_AfterSelect);
            // 
            // content
            // 
            this.content.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.content.Location = new System.Drawing.Point(236, 12);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(754, 617);
            this.content.TabIndex = 1;
            this.content.TabStop = false;
            this.content.Text = "Général";
            // 
            // Editor1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 642);
            this.Controls.Add(this.content);
            this.Controls.Add(this.menu);
            this.MaximizeBox = false;
            this.Name = "Editor1";
            this.Opacity = 0.5D;
            this.Text = "Éditeur";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView menu;
        private System.Windows.Forms.GroupBox content;
    }
}