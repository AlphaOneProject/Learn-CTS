using System.Windows.Forms;

namespace Learn_CTS
{
    partial class GameCard
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
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_description = new System.Windows.Forms.Label();
            this.pb_edit = new System.Windows.Forms.PictureBox();
            this.pb_play = new System.Windows.Forms.PictureBox();
            this.pb_thumbnail = new System.Windows.Forms.PictureBox();
            this.pb_delete = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pb_edit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_play)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_thumbnail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Location = new System.Drawing.Point(153, 6);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(158, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Titre";
            // 
            // lbl_description
            // 
            this.lbl_description.Location = new System.Drawing.Point(153, 34);
            this.lbl_description.Name = "lbl_description";
            this.lbl_description.Size = new System.Drawing.Size(158, 100);
            this.lbl_description.TabIndex = 2;
            this.lbl_description.Text = "Description";
            // 
            // pb_edit
            // 
            this.pb_edit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_edit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_edit.ErrorImage = null;
            this.pb_edit.ImageLocation = "gamecard-edit-btn-x64.png";
            this.pb_edit.Location = new System.Drawing.Point(93, 7);
            this.pb_edit.Name = "pb_edit";
            this.pb_edit.Size = new System.Drawing.Size(42, 42);
            this.pb_edit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_edit.TabIndex = 5;
            this.pb_edit.TabStop = false;
            this.pb_edit.Tag = "";
            this.pb_edit.Click += new System.EventHandler(this.Pb_edit_Click);
            this.pb_edit.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
            this.pb_edit.MouseHover += new System.EventHandler(this.Pb_Btn_MouseHover);
            // 
            // pb_play
            // 
            this.pb_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_play.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_play.ErrorImage = null;
            this.pb_play.ImageLocation = "gamecard-play-btn-x128.png";
            this.pb_play.Location = new System.Drawing.Point(28, 28);
            this.pb_play.Name = "pb_play";
            this.pb_play.Size = new System.Drawing.Size(86, 86);
            this.pb_play.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_play.TabIndex = 4;
            this.pb_play.TabStop = false;
            this.pb_play.Tag = "";
            this.pb_play.Click += new System.EventHandler(this.Pb_play_Click);
            this.pb_play.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
            this.pb_play.MouseHover += new System.EventHandler(this.Pb_Btn_MouseHover);
            // 
            // pb_thumbnail
            // 
            this.pb_thumbnail.Location = new System.Drawing.Point(6, 6);
            this.pb_thumbnail.Name = "pb_thumbnail";
            this.pb_thumbnail.Size = new System.Drawing.Size(128, 128);
            this.pb_thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_thumbnail.TabIndex = 3;
            this.pb_thumbnail.TabStop = false;
            // 
            // pb_delete
            // 
            this.pb_delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_delete.ErrorImage = null;
            this.pb_delete.ImageLocation = "gamecard-delete-btn-x64.png";
            this.pb_delete.Location = new System.Drawing.Point(93, 92);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(42, 42);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 6;
            this.pb_delete.TabStop = false;
            this.pb_delete.Tag = "";
            this.pb_delete.Click += new System.EventHandler(this.Pb_delete_Click);
            this.pb_delete.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
            this.pb_delete.MouseHover += new System.EventHandler(this.Pb_Btn_MouseHover);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.panel1.Controls.Add(this.pb_edit);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 140);
            this.panel1.TabIndex = 7;
            // 
            // GameCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Controls.Add(this.pb_delete);
            this.Controls.Add(this.pb_play);
            this.Controls.Add(this.pb_thumbnail);
            this.Controls.Add(this.lbl_description);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "GameCard";
            this.Size = new System.Drawing.Size(314, 140);
            ((System.ComponentModel.ISupportInitialize)(this.pb_edit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_play)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_thumbnail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_description;
        private System.Windows.Forms.PictureBox pb_thumbnail;
        private System.Windows.Forms.PictureBox pb_play;
        private System.Windows.Forms.PictureBox pb_edit;
        private System.Windows.Forms.PictureBox pb_delete;
        private System.Windows.Forms.Panel panel1;
    }
}
