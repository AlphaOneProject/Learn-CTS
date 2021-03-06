﻿namespace Learn_CTS
{
    partial class EventEdition
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventEdition));
            this.pb_delete = new System.Windows.Forms.PictureBox();
            this.cbo_npcs = new Learn_CTS.ComboBoxFix();
            this.cbo_dialogs = new Learn_CTS.ComboBoxFix();
            this.tlt_Event_Edition = new System.Windows.Forms.ToolTip(this.components);
            this.btn_placement = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_delete
            // 
            this.pb_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete.Image = ((System.Drawing.Image)(resources.GetObject("pb_delete.Image")));
            this.pb_delete.Location = new System.Drawing.Point(555, 15);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(30, 30);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 0;
            this.pb_delete.TabStop = false;
            this.tlt_Event_Edition.SetToolTip(this.pb_delete, "Supprime l\'évènement.");
            this.pb_delete.Click += new System.EventHandler(this.Discard);
            // 
            // cbo_npcs
            // 
            this.cbo_npcs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.cbo_npcs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_npcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_npcs.ForeColor = System.Drawing.Color.White;
            this.cbo_npcs.FormattingEnabled = true;
            this.cbo_npcs.Location = new System.Drawing.Point(105, 14);
            this.cbo_npcs.Name = "cbo_npcs";
            this.cbo_npcs.Size = new System.Drawing.Size(217, 32);
            this.cbo_npcs.TabIndex = 1;
            this.tlt_Event_Edition.SetToolTip(this.cbo_npcs, "Permet le choix du personnage.");
            this.cbo_npcs.SelectedIndexChanged += new System.EventHandler(this.Cbo_npcs_SelectedIndexChanged);
            // 
            // cbo_dialogs
            // 
            this.cbo_dialogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.cbo_dialogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_dialogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_dialogs.ForeColor = System.Drawing.Color.White;
            this.cbo_dialogs.FormattingEnabled = true;
            this.cbo_dialogs.Location = new System.Drawing.Point(356, 14);
            this.cbo_dialogs.Name = "cbo_dialogs";
            this.cbo_dialogs.Size = new System.Drawing.Size(189, 32);
            this.cbo_dialogs.TabIndex = 2;
            this.tlt_Event_Edition.SetToolTip(this.cbo_dialogs, "Permet le choix du dialogue.");
            this.cbo_dialogs.SelectedIndexChanged += new System.EventHandler(this.Cbo_dialogs_SelectedIndexChanged);
            // 
            // tlt_Event_Edition
            // 
            this.tlt_Event_Edition.AutoPopDelay = 5000;
            this.tlt_Event_Edition.InitialDelay = 300;
            this.tlt_Event_Edition.ReshowDelay = 100;
            this.tlt_Event_Edition.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tlt_Event_Edition.ToolTipTitle = "Information";
            // 
            // btn_placement
            // 
            this.btn_placement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_placement.ForeColor = System.Drawing.Color.White;
            this.btn_placement.Location = new System.Drawing.Point(15, 14);
            this.btn_placement.Name = "btn_placement";
            this.btn_placement.Size = new System.Drawing.Size(80, 32);
            this.btn_placement.TabIndex = 3;
            this.btn_placement.Text = "Placer";
            this.tlt_Event_Edition.SetToolTip(this.btn_placement, "Place le figurant dans le décor");
            this.btn_placement.Click += new System.EventHandler(this.Btn_placement_Click);
            // 
            // EventEdition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btn_placement);
            this.Controls.Add(this.cbo_dialogs);
            this.Controls.Add(this.cbo_npcs);
            this.Controls.Add(this.pb_delete);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "EventEdition";
            this.Size = new System.Drawing.Size(600, 60);
            this.Load += new System.EventHandler(this.EventEdition_Load);
            this.SizeChanged += new System.EventHandler(this.EventEdition_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_delete;
        private ComboBoxFix cbo_npcs;
        private ComboBoxFix cbo_dialogs;
        private System.Windows.Forms.ToolTip tlt_Event_Edition;
        private System.Windows.Forms.Button btn_placement;
    }
}
