namespace ACS560Project_FrontEnd
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.newCategoryTab = new System.Windows.Forms.TabPage();
            this.categoryTabs = new System.Windows.Forms.TabControl();
            this.remindersActive = new System.Windows.Forms.CheckBox();
            this.startupHelpActive = new System.Windows.Forms.CheckBox();
            this.categoryTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // newCategoryTab
            // 
            this.newCategoryTab.AutoScroll = true;
            this.newCategoryTab.BackColor = System.Drawing.Color.Transparent;
            this.newCategoryTab.Location = new System.Drawing.Point(4, 22);
            this.newCategoryTab.Name = "newCategoryTab";
            this.newCategoryTab.Padding = new System.Windows.Forms.Padding(3);
            this.newCategoryTab.Size = new System.Drawing.Size(804, 442);
            this.newCategoryTab.TabIndex = 4;
            this.newCategoryTab.Text = "+";
            // 
            // categoryTabs
            // 
            this.categoryTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryTabs.Controls.Add(this.newCategoryTab);
            this.categoryTabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryTabs.Location = new System.Drawing.Point(0, 39);
            this.categoryTabs.Name = "categoryTabs";
            this.categoryTabs.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.categoryTabs.SelectedIndex = 0;
            this.categoryTabs.Size = new System.Drawing.Size(812, 468);
            this.categoryTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.categoryTabs.TabIndex = 4;
            // 
            // remindersActive
            // 
            this.remindersActive.AutoSize = true;
            this.remindersActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remindersActive.Location = new System.Drawing.Point(138, 12);
            this.remindersActive.Name = "remindersActive";
            this.remindersActive.Size = new System.Drawing.Size(194, 20);
            this.remindersActive.TabIndex = 5;
            this.remindersActive.Text = "Have reminders on startup?:";
            this.remindersActive.UseVisualStyleBackColor = true;
            this.remindersActive.Click += new System.EventHandler(this.remindersActive_CheckedChanged);
            // 
            // startupHelpActive
            // 
            this.startupHelpActive.AutoSize = true;
            this.startupHelpActive.Checked = true;
            this.startupHelpActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startupHelpActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startupHelpActive.Location = new System.Drawing.Point(458, 12);
            this.startupHelpActive.Name = "startupHelpActive";
            this.startupHelpActive.Size = new System.Drawing.Size(173, 20);
            this.startupHelpActive.TabIndex = 7;
            this.startupHelpActive.Text = "Display help on startup?:";
            this.startupHelpActive.UseVisualStyleBackColor = true;
            this.startupHelpActive.Click += new System.EventHandler(this.startupHelpActive_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(809, 511);
            this.Controls.Add(this.startupHelpActive);
            this.Controls.Add(this.remindersActive);
            this.Controls.Add(this.categoryTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(825, 550);
            this.Name = "MainForm";
            this.Text = "Reading Tracker";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.categoryTabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabPage newCategoryTab;
        private System.Windows.Forms.TabControl categoryTabs;
        private System.Windows.Forms.CheckBox remindersActive;
        private System.Windows.Forms.CheckBox startupHelpActive;
    }
}

