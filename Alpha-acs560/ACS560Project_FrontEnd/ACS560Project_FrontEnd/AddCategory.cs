using System;
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
    public partial class AddCategory : Form
    {
        MainForm mf = MainForm.Self;

        public AddCategory()
        {
            InitializeComponent();
        }

        // Close AddCategory form without submitting data
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string cn = categoryNameText.Text;

            // Create new Category object and add it to list of records
            Category newCategory = new Category(0,cn,"",0);
            mf.addCategory(newCategory);
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
