namespace Learn_CTS
{
    partial class Editor
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Non-joueurs");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Dialogues");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Joueur");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Personnages", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Scénarios");
            this.menu = new System.Windows.Forms.TreeView();
            this.content = new System.Windows.Forms.GroupBox();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.FullRowSelect = true;
            this.menu.ItemHeight = 32;
            this.menu.Location = new System.Drawing.Point(12, 12);
            this.menu.Name = "menu";
            treeNode1.Name = "global";
            treeNode1.Text = "Général";
            treeNode2.Name = "npcs";
            treeNode2.Text = "Non-joueurs";
            treeNode3.Name = "choices";
            treeNode3.Text = "Dialogues";
            treeNode4.Name = "player";
            treeNode4.Text = "Joueur";
            treeNode5.Name = "characters";
            treeNode5.Text = "Personnages";
            treeNode6.Name = "scenarios";
            treeNode6.Text = "Scénarios";
            this.menu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode5,
            treeNode6});
            this.menu.PathSeparator = " / ";
            this.menu.ShowLines = false;
            this.menu.ShowNodeToolTips = true;
            this.menu.Size = new System.Drawing.Size(225, 618);
            this.menu.TabIndex = 0;
            this.menu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Menu_AfterSelect);
            // 
            // content
            // 
            this.content.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.content.Location = new System.Drawing.Point(250, 85);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(740, 544);
            this.content.TabIndex = 1;
            this.content.TabStop = false;
            this.content.Text = "Général";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(243, 28);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(115, 37);
            this.title.TabIndex = 2;
            this.title.Text = "Édition";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 642);
            this.Controls.Add(this.title);
            this.Controls.Add(this.content);
            this.Controls.Add(this.menu);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Editor";
            this.Text = "Éditeur";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Editor_FormClosed);
            this.Load += new System.EventHandler(this.Editor1_Load);
            this.SizeChanged += new System.EventHandler(this.Editor1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView menu;
        private System.Windows.Forms.GroupBox content;
        private System.Windows.Forms.Label title;
    }
}