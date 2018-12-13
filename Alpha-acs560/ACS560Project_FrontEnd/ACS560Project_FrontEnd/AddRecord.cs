using System;
using System.Collections;
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
    public partial class AddRecord : Form
    {
        MainForm mf = MainForm.Self;

        public AddRecord(string preCategoryName)
        {
            InitializeComponent();
            categoryNameText.Text = preCategoryName;
            nameText.Select();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string n = nameText.Text;
            string startingStr = currentReadText.Text;
            foreach (char character in startingStr)
            {
                if (character < '0' || character > '9')
                {
                    var denyResult = MessageBox.Show("Current Read info must contain only numbers.",
                        "Submission Error", MessageBoxButtons.OK);
                    return;
                }
            }
            int c = Int32.Parse(startingStr);

            startingStr = latestReleaseText.Text;
            foreach (char character in startingStr)
            {
                if (character < '0' || character > '9')
                {
                    var denyResult = MessageBox.Show("Latest Release info must contain only numbers.",
                        "Submission Error", MessageBoxButtons.OK);
                    return;
                }
            }
            int lr = Int32.Parse(startingStr);

            DateTime nr = nextReleaseDate.Value;
            DateTime fr = firstReleaseDate.Value;
            if(firstReleaseDate.Text == "")
            {
                fr = DateTime.Now;
            }
            string rs = releaseScheduleText.Text;
            string g = genreText.Text;
            string p = publisherText.Text;
            string cn = categoryNameText.Text;
            string w = writerText.Text;
            string a = artistText.Text;

            bool isf = isFinishedBox.Checked;

            // Create new Record object and add it to list of records
            Record newRecord = new Record(0,0,n,c,lr,nr,fr,rs,g,p,cn,w,a,isf);
            mf.addRecord(newRecord);
            this.Close();
        }

        private void categoryNameText_TextChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "" && nameText.Text != "" && currentReadText.Text != "" && 
                latestReleaseText.Text != "" && releaseScheduleText.Text != "")
            {
                submitButton.Enabled = true;
            }
            else
            {
                submitButton.Enabled = false;
            }
        }

        private void nameText_TextChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "" && nameText.Text != "" && currentReadText.Text != "" &&
                latestReleaseText.Text != "" && releaseScheduleText.Text != "")
            {
                submitButton.Enabled = true;
            }
            else
            {
                submitButton.Enabled = false;
            }
        }

        private void currentReadText_TextChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "" && nameText.Text != "" && currentReadText.Text != "" &&
                latestReleaseText.Text != "" && releaseScheduleText.Text != "")
            {
                submitButton.Enabled = true;
            }
            else
            {
                submitButton.Enabled = false;
            }
         }

        private void latestReleaseText_TextChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "" && nameText.Text != "" && currentReadText.Text != "" &&
                latestReleaseText.Text != "" && releaseScheduleText.Text != "")
            {
                submitButton.Enabled = true;
            }
            else
            {
                submitButton.Enabled = false;
            }
        }

        private void releaseScheduleText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "" && nameText.Text != "" && currentReadText.Text != "" &&
                latestReleaseText.Text != "" && releaseScheduleText.Text != "")
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
