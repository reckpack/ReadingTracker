namespace ACS560Project_FrontEnd
{
    partial class HelpWindow
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
            this.helpPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.helpPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // helpPicture
            // 
            this.helpPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpPicture.ErrorImage = null;
            this.helpPicture.Image = global::ACS560Project_FrontEnd.Properties.Resources.Help_Screen;
            this.helpPicture.Location = new System.Drawing.Point(0, 0);
            this.helpPicture.Name = "helpPicture";
            this.helpPicture.Size = new System.Drawing.Size(811, 508);
            this.helpPicture.TabIndex = 0;
            this.helpPicture.TabStop = false;
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 508);
            this.Controls.Add(this.helpPicture);
            this.Cursor = System.Windows.Forms.Cursors.Help;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "HelpWindow";
            this.Text = "Startup Help Screen";
            ((System.ComponentModel.ISupportInitialize)(this.helpPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox helpPicture;
    }
}