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
    public partial class EditRecord : Form
    {
        MainForm mf = MainForm.Self;
        Record selectedRecord = null;
        private List<Category> fetchedCatList = new List<Category>();

        public EditRecord(Record currRecord)
        {
            InitializeComponent();
            nameText.Select();
            fetchedCatList = mf.CategoryList;

            this.selectedRecord = currRecord;

            nameText.Text = selectedRecord.Name;
            currentReadText.Text = selectedRecord.CurrentRead.ToString();
            latestReleaseText.Text = selectedRecord.LatestRelease.ToString();

            nextReleaseDate.Value = selectedRecord.NextRelease;
            firstReleaseDate.Value = selectedRecord.FirstRelease;
            nextReleaseDate.Text = selectedRecord.NextRelease.ToString();
            firstReleaseDate.Text = selectedRecord.FirstRelease.ToString();

            releaseScheduleText.Text = selectedRecord.ReleaseSchedule;
            genreText.Text = selectedRecord.Genre;
            publisherText.Text = selectedRecord.Publisher;
            categoryNameText.Text = selectedRecord.CategoryName;
            writerText.Text = selectedRecord.Writer;
            artistText.Text = selectedRecord.Artist;
            isFinishedBox.Checked = selectedRecord.IsFinished;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string n = nameText.Text;
            string startingStr = currentReadText.Text;
            DateTime nr = nextReleaseDate.Value;
            DateTime fr = firstReleaseDate.Value;
            if (firstReleaseDate.Text == "")
            {
                fr = DateTime.Now;
            }
            string rs = releaseScheduleText.Text;
            string g = genreText.Text;
            string p = publisherText.Text;

            string cn = categoryNameText.Text;
            bool foundMatch = false;
            foreach (Category cat in fetchedCatList)
            {
                if (cat.CategoryName == cn)
                {
                    foundMatch = true;
                    break;
                }
            }
            if (foundMatch == false)
            {
                var denyResult = MessageBox.Show("The edited Category name does not exist.",
                    "Category Not Found", MessageBoxButtons.OK);
                return;
            }

            string w = writerText.Text;
            string a = artistText.Text;
            bool isf = isFinishedBox.Checked;

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

            selectedRecord.Name = n;
            selectedRecord.CurrentRead = c;
            selectedRecord.LatestRelease = lr;
            selectedRecord.NextRelease = nr;
            selectedRecord.FirstRelease = fr;
            selectedRecord.ReleaseSchedule = rs;
            selectedRecord.Genre = g;
            selectedRecord.Publisher = p;
            selectedRecord.CategoryName = cn;
            selectedRecord.Writer = w;
            selectedRecord.Artist = a;
            selectedRecord.IsFinished = isf;

            mf.updateRecord(selectedRecord);
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void categoryNameText_TextChanged(object sender, EventArgs e)
        {
            if (categoryNameText.Text != "" && nameText.Text != "" && releaseScheduleText.Text != "")
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
            if (categoryNameText.Text != "" && nameText.Text != "" && releaseScheduleText.Text != "")
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
