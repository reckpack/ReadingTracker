﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACS560Project_FrontEnd
{
    public partial class EditCategory : Form
    {
        MainForm mf = MainForm.Self;
        Category selectedCategory = null;

        public EditCategory(Category currCategory)
        {
            InitializeComponent();

            this.selectedCategory = currCategory;
            categoryNameText.Text = selectedCategory.CategoryName;
        }

        // Close AddCategory form without submitting data
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string c = categoryNameText.Text;
            selectedCategory.CategoryName = c;

            mf.updateCategory(selectedCategory);
            this.Close();
        }

        private void categoryNameText_TextChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "")
            {
                submitButton.Enabled = true;
            }
            else
            {
                submitButton.Enabled = false;
            }
        }
    }
}
