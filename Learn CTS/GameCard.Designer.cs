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
            this.components = new System.ComponentModel.Container();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_description = new System.Windows.Forms.Label();
            this.pb_edit = new System.Windows.Forms.PictureBox();
            this.pb_play = new System.Windows.Forms.PictureBox();
            this.pb_thumbnail = new System.Windows.Forms.PictureBox();
            this.pb_delete = new System.Windows.Forms.PictureBox();
            this.pnl_border = new System.Windows.Forms.Panel();
            this.pb_copy = new System.Windows.Forms.PictureBox();
            this.tlt_gc = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pb_edit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_play)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_thumbnail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).BeginInit();
            this.pnl_border.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_copy)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_title
            // 
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Location = new System.Drawing.Point(153, 6);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(158, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Tag = "3";
            this.lbl_title.Text = "Titre";
            // 
            // lbl_description
            // 
            this.lbl_description.Location = new System.Drawing.Point(153, 34);
            this.lbl_description.Name = "lbl_description";
            this.lbl_description.Size = new System.Drawing.Size(158, 53);
            this.lbl_description.TabIndex = 2;
            this.lbl_description.Tag = "3";
            this.lbl_description.Text = "Description";
            // 
            // pb_edit
            // 
            this.pb_edit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_edit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_edit.ErrorImage = null;
            this.pb_edit.ImageLocation = "gamecard-edit-btn-x64.png";
            this.pb_edit.Location = new System.Drawing.Point(207, 90);
            this.pb_edit.Name = "pb_edit";
            this.pb_edit.Size = new System.Drawing.Size(42, 42);
            this.pb_edit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_edit.TabIndex = 5;
            this.pb_edit.TabStop = false;
            this.pb_edit.Tag = "3";
            this.pb_edit.Click += new System.EventHandler(this.Pb_edit_Click);
            this.pb_edit.MouseEnter += new System.EventHandler(this.Pb_Btn_MouseHover);
            this.pb_edit.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
            // 
            // pb_play
            // 
            this.pb_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_play.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_play.ErrorImage = null;
            this.pb_play.ImageLocation = "gamecard-play-btn-x128.png";
            this.pb_play.Location = new System.Drawing.Point(24, 24);
            this.pb_play.Name = "pb_play";
            this.pb_play.Size = new System.Drawing.Size(91, 92);
            this.pb_play.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_play.TabIndex = 4;
            this.pb_play.TabStop = false;
            this.pb_play.Tag = "";
            this.pb_play.Click += new System.EventHandler(this.Pb_play_Click);
            this.pb_play.MouseEnter += new System.EventHandler(this.Pb_Btn_MouseHover);
            this.pb_play.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
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
            this.pb_delete.Location = new System.Drawing.Point(259, 90);
            this.pb_delete.Name = "pb_delete";
            this.pb_delete.Size = new System.Drawing.Size(42, 42);
            this.pb_delete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_delete.TabIndex = 6;
            this.pb_delete.TabStop = false;
            this.pb_delete.Tag = "3";
            this.pb_delete.Click += new System.EventHandler(this.Pb_delete_Click);
            this.pb_delete.MouseEnter += new System.EventHandler(this.Pb_Btn_MouseHover);
            this.pb_delete.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
            // 
            // pnl_border
            // 
            this.pnl_border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.pnl_border.Controls.Add(this.pb_play);
            this.pnl_border.Controls.Add(this.pb_thumbnail);
            this.pnl_border.Location = new System.Drawing.Point(0, 0);
            this.pnl_border.Name = "pnl_border";
            this.pnl_border.Size = new System.Drawing.Size(140, 140);
            this.pnl_border.TabIndex = 7;
            this.pnl_border.Tag = "4";
            // 
            // pb_copy
            // 
            this.pb_copy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_copy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pb_copy.ErrorImage = null;
            this.pb_copy.ImageLocation = "gamecard-copy-btn-x64.png";
            this.pb_copy.Location = new System.Drawing.Point(155, 90);
            this.pb_copy.Name = "pb_copy";
            this.pb_copy.Size = new System.Drawing.Size(42, 42);
            this.pb_copy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_copy.TabIndex = 8;
            this.pb_copy.TabStop = false;
            this.pb_copy.Tag = "3";
            this.pb_copy.Click += new System.EventHandler(this.Pb_copy_Click);
            this.pb_copy.MouseEnter += new System.EventHandler(this.Pb_Btn_MouseHover);
            this.pb_copy.MouseLeave += new System.EventHandler(this.Pb_Btn_MouseLeave);
            // 
            // GameCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Controls.Add(this.pb_delete);
            this.Controls.Add(this.pb_copy);
            this.Controls.Add(this.lbl_description);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.pb_edit);
            this.Controls.Add(this.pnl_border);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "GameCard";
            this.Size = new System.Drawing.Size(314, 140);
            this.Tag = "3";
            this.Load += new System.EventHandler(this.GameCard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_edit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_play)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_thumbnail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_delete)).EndInit();
            this.pnl_border.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_copy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_description;
        private System.Windows.Forms.PictureBox pb_thumbnail;
        private System.Windows.Forms.PictureBox pb_play;
        private System.Windows.Forms.PictureBox pb_edit;
        private System.Windows.Forms.PictureBox pb_delete;
        private System.Windows.Forms.Panel pnl_border;
        private PictureBox pb_copy;
        private ToolTip tlt_gc;
    }
}
