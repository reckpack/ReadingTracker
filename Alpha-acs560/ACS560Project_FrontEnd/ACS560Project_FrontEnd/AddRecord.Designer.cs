namespace ACS560Project_FrontEnd
{
    partial class AddRecord
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
            this.nameText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.genreText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.currentReadText = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.categoryNameText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.publisherText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.writerText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.artistText = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.isFinishedBox = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.nextReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.firstReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.latestReleaseText = new System.Windows.Forms.TextBox();
            this.releaseScheduleText = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // nameText
            // 
            this.nameText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.nameText.Location = new System.Drawing.Point(114, 49);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(152, 20);
            this.nameText.TabIndex = 37;
            this.nameText.TextChanged += new System.EventHandler(this.nameText_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(43, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "*Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "*Current Read:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "*Latest Release:";
            // 
            // genreText
            // 
            this.genreText.Location = new System.Drawing.Point(397, 86);
            this.genreText.Name = "genreText";
            this.genreText.Size = new System.Drawing.Size(152, 20);
            this.genreText.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(346, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Genre:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(21, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Next Release:";
            // 
            // currentReadText
            // 
            this.currentReadText.Location = new System.Drawing.Point(114, 86);
            this.currentReadText.Name = "currentReadText";
            this.currentReadText.Size = new System.Drawing.Size(81, 20);
            this.currentReadText.TabIndex = 29;
            this.currentReadText.TextChanged += new System.EventHandler(this.currentReadText_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(23, 227);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "First Release:";
            // 
            // submitButton
            // 
            this.submitButton.Enabled = false;
            this.submitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submitButton.Location = new System.Drawing.Point(454, 256);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(95, 23);
            this.submitButton.TabIndex = 38;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(353, 256);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(95, 23);
            this.cancelButton.TabIndex = 39;
            this.cancelButton.Text = " Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(298, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 41;
            this.label1.Text = "*Category:";
            // 
            // categoryNameText
            // 
            this.categoryNameText.Location = new System.Drawing.Point(397, 49);
            this.categoryNameText.Name = "categoryNameText";
            this.categoryNameText.Size = new System.Drawing.Size(152, 20);
            this.categoryNameText.TabIndex = 42;
            this.categoryNameText.TextChanged += new System.EventHandler(this.categoryNameText_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(328, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Publisher:";
            // 
            // publisherText
            // 
            this.publisherText.Location = new System.Drawing.Point(397, 120);
            this.publisherText.Name = "publisherText";
            this.publisherText.Size = new System.Drawing.Size(152, 20);
            this.publisherText.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(346, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Writer:";
            // 
            // writerText
            // 
            this.writerText.Location = new System.Drawing.Point(397, 156);
            this.writerText.Name = "writerText";
            this.writerText.Size = new System.Drawing.Size(152, 20);
            this.writerText.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(351, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 47;
            this.label4.Text = "Artist:";
            // 
            // artistText
            // 
            this.artistText.Location = new System.Drawing.Point(397, 191);
            this.artistText.Name = "artistText";
            this.artistText.Size = new System.Drawing.Size(152, 20);
            this.artistText.TabIndex = 48;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(394, 227);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
            this.label11.TabIndex = 49;
            this.label11.Text = "Is Finished:";
            // 
            // isFinishedBox
            // 
            this.isFinishedBox.AutoSize = true;
            this.isFinishedBox.Location = new System.Drawing.Point(472, 227);
            this.isFinishedBox.Name = "isFinishedBox";
            this.isFinishedBox.Size = new System.Drawing.Size(15, 14);
            this.isFinishedBox.TabIndex = 50;
            this.isFinishedBox.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(2, 159);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 13);
            this.label13.TabIndex = 81;
            this.label13.Text = "*Release Sched.:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(38, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 13);
            this.label14.TabIndex = 82;
            this.label14.Text = "*Required Field";
            // 
            // nextReleaseDate
            // 
            this.nextReleaseDate.Location = new System.Drawing.Point(114, 191);
            this.nextReleaseDate.Name = "nextReleaseDate";
            this.nextReleaseDate.Size = new System.Drawing.Size(200, 20);
            this.nextReleaseDate.TabIndex = 120;
            // 
            // firstReleaseDate
            // 
            this.firstReleaseDate.Location = new System.Drawing.Point(114, 224);
            this.firstReleaseDate.Name = "firstReleaseDate";
            this.firstReleaseDate.Size = new System.Drawing.Size(200, 20);
            this.firstReleaseDate.TabIndex = 121;
            // 
            // latestReleaseText
            // 
            this.latestReleaseText.Location = new System.Drawing.Point(114, 120);
            this.latestReleaseText.Name = "latestReleaseText";
            this.latestReleaseText.Size = new System.Drawing.Size(81, 20);
            this.latestReleaseText.TabIndex = 122;
            this.latestReleaseText.TextChanged += new System.EventHandler(this.latestReleaseText_TextChanged);
            // 
            // releaseScheduleText
            // 
            this.releaseScheduleText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.releaseScheduleText.FormattingEnabled = true;
            this.releaseScheduleText.Items.AddRange(new object[] {
            "Weekly",
            "Biweekly",
            "Monthly"});
            this.releaseScheduleText.Location = new System.Drawing.Point(114, 156);
            this.releaseScheduleText.Name = "releaseScheduleText";
            this.releaseScheduleText.Size = new System.Drawing.Size(81, 21);
            this.releaseScheduleText.TabIndex = 123;
            this.releaseScheduleText.SelectedIndexChanged += new System.EventHandler(this.releaseScheduleText_SelectedIndexChanged);
            // 
            // AddRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(568, 294);
            this.Controls.Add(this.releaseScheduleText);
            this.Controls.Add(this.latestReleaseText);
            this.Controls.Add(this.firstReleaseDate);
            this.Controls.Add(this.nextReleaseDate);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.isFinishedBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.artistText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.writerText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.publisherText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.categoryNameText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.genreText);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.currentReadText);
            this.Controls.Add(this.label10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddRecord";
            this.Text = "New Record";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox genreText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox currentReadText;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox categoryNameText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox publisherText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox writerText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox artistText;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox isFinishedBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker nextReleaseDate;
        private System.Windows.Forms.DateTimePicker firstReleaseDate;
        private System.Windows.Forms.TextBox latestReleaseText;
        private System.Windows.Forms.ComboBox releaseScheduleText;
    }
}