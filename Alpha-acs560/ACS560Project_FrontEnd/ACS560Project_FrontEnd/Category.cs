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
    public class Category
    {
        private int _pk;
        private string categoryName;
        private string apiLink;
        private int indexEditor;

        public Category(int pk, string cn, string api, int ie)
        {
            this._pk = pk;
            this.categoryName = cn;
            this.apiLink = api;
            this.indexEditor = ie;
        }

        public int _Pk
        {
            get { return _pk; }
            set { _pk = value; }
        }
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        public string ApiLink
        {
            get { return apiLink; }
            set { apiLink = value; }
        }
        public int IndexEditor
        {
            get { return indexEditor; }
            set { indexEditor = value; }
        }

    }
}
