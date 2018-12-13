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
    public class Record
    {
        private int _pk;
        private int _cat_pk;
        private string name;
        private int currentRead;
        private int latestRelease;
        private DateTime nextRelease;
        private DateTime firstRelease;
        private string releaseSchedule;
        private string genre;
        private string publisher;
        private string categoryName;
        private string writer;
        private string artist;
        private bool isFinished;

        public Record(int pk, int c_pk, string n, int c, int lr, DateTime nr, DateTime fr, string rs,
            string g, string p, string cn, string w, string a, bool isf)
        {
            this._pk = pk;
            this._cat_pk = c_pk;
            this.name = n;
            this.currentRead = c;
            this.latestRelease = lr;
            this.nextRelease = nr;
            this.firstRelease = fr;
            this.releaseSchedule = rs;
            this.genre = g;
            this.publisher = p;
            this.categoryName = cn;
            this.writer = w;
            this.artist = a;
            this.isFinished = isf;
        }

        public int _Pk
        {
            get { return _pk; }
            set { _pk = value; }
        }
        public int _Cat_pk
        {
            get { return _cat_pk; }
            set { _cat_pk = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int CurrentRead
        {
            get { return currentRead; }
            set { currentRead = value; }
        }
        public int LatestRelease
        {
            get { return latestRelease; }
            set { latestRelease = value; }
        }
        public DateTime NextRelease
        {
            get { return nextRelease; }
            set { nextRelease = value; }
        }
        public DateTime FirstRelease
        {
            get { return firstRelease; }
            set { firstRelease = value; }
        }
        public string ReleaseSchedule
        {
            get { return releaseSchedule; }
            set { releaseSchedule = value; }
        }
        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }
        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        public string Writer
        {
            get { return writer; }
            set { writer = value; }
        }
        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }
        public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }
    }
}
