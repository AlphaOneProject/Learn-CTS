﻿namespace Learn_CTS
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
            this.components = new System.ComponentModel.Container();
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
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Scénarios");
            this.menu = new System.Windows.Forms.TreeView();
            this.content = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.lbl_path = new System.Windows.Forms.Label();
            this.tlt_global = new System.Windows.Forms.ToolTip(this.components);
            this.ofd_global = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.menu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.ForeColor = System.Drawing.Color.White;
            this.menu.FullRowSelect = true;
            this.menu.ItemHeight = 32;
            this.menu.Location = new System.Drawing.Point(12, 12);
            this.menu.Name = "menu";
            treeNode1.Name = "global";
            treeNode1.Text = "Général";
            treeNode1.ToolTipText = "Paramètres du jeu";
            treeNode2.Name = "npcs";
            treeNode2.Text = "Figurants";
            treeNode2.ToolTipText = "Personnages non joueurs s\'adressant au joueur par le moyen de dialogues";
            treeNode3.Name = "dialogs";
            treeNode3.Text = "Dialogues";
            treeNode3.ToolTipText = "Questions et réponses proposables au joueur";
            treeNode4.Name = "item_images";
            treeNode4.Text = "Objets";
            treeNode4.ToolTipText = "Images associables à des objets";
            treeNode5.Name = "sprites";
            treeNode5.Text = "Personnages";
            treeNode5.ToolTipText = "Combinaisons d\'images permettant d\'animer les personnages";
            treeNode6.Name = "backgrounds";
            treeNode6.Text = "Décors";
            treeNode6.ToolTipText = "Images servant d\'illustration de fond à une situation";
            treeNode7.Name = "images";
            treeNode7.Text = "Images";
            treeNode7.ToolTipText = "Représentations visuelles des différents éléments du jeu";
            treeNode8.Name = "models";
            treeNode8.Text = "Modèles";
            treeNode8.ToolTipText = "Gestion des modèles du jeu";
            treeNode9.Name = "scenarios";
            treeNode9.Text = "Scénarios";
            treeNode9.ToolTipText = "Gestion des scénarios qui seront disponibles au joueur";
            this.menu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode8,
            treeNode9});
            this.menu.PathSeparator = " / ";
            this.menu.ShowLines = false;
            this.menu.ShowNodeToolTips = true;
            this.menu.Size = new System.Drawing.Size(285, 618);
            this.menu.TabIndex = 0;
            this.menu.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.Menu_BeforeSelect);
            this.menu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Menu_AfterSelect);
            // 
            // content
            // 
            this.content.AutoScroll = true;
            this.content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.content.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.content.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.content.Location = new System.Drawing.Point(314, 85);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(676, 544);
            this.content.TabIndex = 1;
            this.content.Text = "Général";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(307, 15);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(115, 37);
            this.title.TabIndex = 2;
            this.title.Text = "Édition";
            this.tlt_global.SetToolTip(this.title, "Titre du jeu en cours d\'édition");
            // 
            // lbl_path
            // 
            this.lbl_path.AutoSize = true;
            this.lbl_path.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.lbl_path.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_path.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_path.Location = new System.Drawing.Point(314, 58);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(91, 28);
            this.lbl_path.TabIndex = 3;
            this.lbl_path.Text = "Général";
            // 
            // tlt_global
            // 
            this.tlt_global.AutoPopDelay = 5000;
            this.tlt_global.InitialDelay = 300;
            this.tlt_global.ReshowDelay = 100;
            // 
            // ofd_global
            // 
            this.ofd_global.AddExtension = false;
            this.ofd_global.InitialDirectory = "this.game_path";
            this.ofd_global.ReadOnlyChecked = true;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1002, 642);
            this.Controls.Add(this.lbl_path);
            this.Controls.Add(this.title);
            this.Controls.Add(this.content);
            this.Controls.Add(this.menu);
            this.ForeColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Editor";
            this.Text = "Éditeur";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Editor_FormClosed);
            this.Load += new System.EventHandler(this.Editor1_Load);
            this.SizeChanged += new System.EventHandler(this.Editor1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView menu;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Panel content;
        private System.Windows.Forms.Label lbl_path;
        private System.Windows.Forms.ToolTip tlt_global;
        private System.Windows.Forms.OpenFileDialog ofd_global;
    }
}