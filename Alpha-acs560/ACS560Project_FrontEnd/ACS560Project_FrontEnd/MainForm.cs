using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// "104.197.234.76" (PFW ip address)
// "10.100.248.176" (Laptop ip address - Sammy)
// "192.168.1.27" (Home ip address - James)
// "104.196.107.34" (google cloud vm ip)

namespace ACS560Project_FrontEnd
{
    // Struct built to pass messages via socket to server/database. Must be created and sent in exact order
    public struct Message
    {
        public string Operation; // For determinating create, read, update, delected (CRUD)
        public string Object; // rec for Record, cat for Category, all for everything
        public int    ObjectID; // The ID of the object being added on
        public string Action; // Default blank, but used for updating Record on what is being changeds

        public Message(string opt, string obj, int objID, string act)
        {
            Operation = opt;
            Object = obj;
            Action = act;
            ObjectID = objID;
        }
    }

    public partial class MainForm : Form
    {
        // Static accessor
        public static MainForm Self;

        // Lists that contain all Record and Category objects
        private List<Record> recordList = new List<Record>();
        private List<Category> categoryList = new List<Category>();

        // constant ip and port variable so we only have to make changes in one place instead of wherever the ip string ends up
        //private const string IP = "104.196.107.34";//;
        private const string IP = "127.0.0.1";
        private const int PORT = 8080;

        // For reating a TCP/IP socket 
        static IPAddress ipAddress = IPAddress.Parse(IP);
        static IPEndPoint remoteEP = new IPEndPoint(ipAddress, PORT);
        static Socket sender = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Data buffer for incoming data.  
        byte[] bytes = new byte[8192];

        // Determines if the startClient() method has run first or not yet
        private bool isAfterLaunch = false;

        public MainForm()
        {
            Self = this;

            // Initialize components and Handlers
            InitializeComponent();
            categoryTabs.MouseDown += new MouseEventHandler(categoryTabs_MouseDown);
            categoryTabs.Selecting += new TabControlCancelEventHandler(categoryTabs_Selecting);
            categoryTabs.MouseDoubleClick += new MouseEventHandler(categoryTabs_MouseDoubleClick);

            // Set up Socket(s)
            Self.startClient();
        }

        // Connects to server/database
        public void startClient()
        {
            // Connect the socket to the remote endpoint
            try
            {
                resetConnection();
                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Pull all existing data from server/database, starting with Categories, then Records
                Message servMessage = new Message("r", "CatAll", -1, "");
                Self.sendMessageToServer(servMessage);
                receiveCategoryFromServer();

                foreach(Category c in categoryList)
                {
                    resetConnection();
                    servMessage = new Message("r", "RecAll", c._Pk, "");
                    Self.sendMessageToServer(servMessage);
                    receiveRecordsFromServer();

                }

                // Append "+" tab to end of tab list
                this.categoryTabs.SelectedIndex = 0;
                TabPage firstTab = categoryTabs.SelectedTab;
                categoryTabs.TabPages.Remove(firstTab);
                categoryTabs.TabPages.Add(firstTab);

                // Retrieve and update reminder and help bools
                loadConfigData();

                isAfterLaunch = true;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        // Resets socket connection before each use of it
        public void resetConnection()
        {
            if (sender.Connected != false)
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }

            ipAddress = IPAddress.Parse(IP);
            remoteEP = new IPEndPoint(ipAddress, PORT);
            sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);
        }

        // Load information from config.txt file
        public void loadConfigData()
        {
            var fs = new FileStream(Directory.GetCurrentDirectory() + "\\config.txt", 
                FileMode.Open, FileAccess.Read);
            var settings = new List<string>();

            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line = "";
                while((line = sr.ReadLine()) != null)
                {
                    settings.Add(line);
                }
                sr.Close();
            }
            fs.Close();

            foreach (string s in settings)
            {
                string[] line = s.Split(' ');//.Last();
                if(line[0] == "help")
                {
                    if(line[1] == "True")
                        startupHelpActive.Checked = true;
                    else
                        startupHelpActive.Checked = false;
                }
                else if(line[0] == "reminder")
                {
                    if (line[1] == "True")
                        remindersActive.Checked = true;
                    else
                        remindersActive.Checked = false;
                }
            }
        }

        // Receive Category data from server/database
        public void receiveCategoryFromServer()
        {
            int bytesRec = sender.Receive(bytes);

            string resultString = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            int index_of_closing_breaket = resultString.IndexOf(']');
            string sub = resultString.Substring(0, index_of_closing_breaket + 1);
            var deserializedResult = JsonConvert.DeserializeObject<JObject[]>(sub);

            foreach (JObject r in deserializedResult)
            {
                int pk = r["id"].Value<int>();
                string cn = r["name"].Value<string>().Trim('\'');
                string api = r["api_link"].Value<string>().Trim('\'');
                int ie = r["IndexInEditor"].Value<int>();

                Category receivedCategory = new Category(pk, cn, api, ie);
                addCategory(receivedCategory);
            }

            categoryList = categoryList.OrderBy(o => o.IndexEditor).ToList();
            foreach (Category c in categoryList)
            {
                fillTab(c);
            }
        }

        // Receive Record data from server/database
        public void receiveRecordsFromServer()
        {
            int bytesRec = sender.Receive(bytes);

            string resultString = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            
            int index_of_closing_breaket = resultString.IndexOf(']');
            if (index_of_closing_breaket == -1)
                return;

            string sub = resultString.Substring(0, index_of_closing_breaket + 1);
            var deserializedResult = JsonConvert.DeserializeObject<JObject[]>(sub);

            foreach (JObject r in deserializedResult)
            {
                int pk = r["id"].Value<int>();
                int c_pk = r["category_id"].Value<int>();
                string n = r["title"].Value<string>().Trim('\''); ;
                int c = r["current_issue"].Value<int>();
                int lr = r["latest_released"].Value<int>();
                DateTime nr = r["next_release_date"].Value<DateTime>();
                string g = r["genre"].Value<string>().Trim('\''); ;
                string p = r["publisher"].Value<string>().Trim('\''); ;
                string cn = "";
                DateTime fr = r["initial_release_date"].Value<DateTime>();
                string w = r["writer"].Value<string>().Trim('\''); w = w.Replace(";", ", ");
                string a = r["artist"].Value<string>().Trim('\''); a = a.Replace(";", ", ");
                bool isf = r["is_finished"].Value<bool>();
                string rs = r["release_schedule"].Value<string>().Trim('\''); ;

                for (int i = 0; i <= categoryList.Count; i++)
                {
                    Category currCategory = categoryList[i];
                    if (currCategory._Pk == c_pk)
                    {
                        cn = currCategory.CategoryName;
                        break;
                    }
                }

                Record receivedRecord = new Record(pk, c_pk, n, c, lr, nr, fr, rs, g, p, cn, w, a, isf);
                addRecord(receivedRecord);
            }
        }

        // Receives message from server/database determining if sent message failed or passed
        public string receiveResponseFromServer()
        {
            int bytesRec = sender.Receive(bytes);
            string resultString = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            if (resultString.Contains("ok"))
                return "ok";
            else if (resultString.Contains("fail"))
                return "fail";

            return resultString;
        }

        // Messages server/database with a Message struct
        public void sendMessageToServer(Message servMessage)
        {
            var serializedResult = JsonConvert.SerializeObject(servMessage);

            // Encode the data string into a byte array
            byte[] msg = Encoding.ASCII.GetBytes(serializedResult);

            // Send the data through the socket
            int bytesSent = sender.Send(msg);
        }

        // Fills new DataGridView with necessary information upon generating a new tab
        private void fillTab(Category sentCategory)
        {
            var lastIndex = 0;
            if (isAfterLaunch)
            {
                lastIndex = categoryTabs.TabCount - 1;
                categoryTabs.TabPages.Insert(lastIndex, sentCategory.CategoryName);
            }
            else
            {
                categoryTabs.TabPages.Add(sentCategory.CategoryName);
                lastIndex = categoryTabs.TabCount - 1;
            }

            categoryTabs.SelectedIndex = lastIndex;
            categoryTabs.SelectedTab.Name = sentCategory.CategoryName;
            categoryTabs.SelectedTab.Text = sentCategory.CategoryName;
            categoryTabs.TabPages[lastIndex].Controls.Add(new DataGridView() { Name = sentCategory.CategoryName });

            int dataGridViewIndex = categoryTabs.TabPages[lastIndex].Controls.IndexOfKey(sentCategory.CategoryName);
            if (dataGridViewIndex >= 0)
            {
                DataGridView currGrid = categoryTabs.TabPages[lastIndex].Controls[dataGridViewIndex] as DataGridView;
                if (currGrid != null)
                {
                    currGrid.Columns.Add("Record #", "Record #");
                    currGrid.Columns["Record #"].Visible = false;
                    currGrid.Columns.Add("Name", "Name");
                    currGrid.Columns.Add("Current Read", "Current Read");
                    currGrid.Columns.Add("Latest Release", "Latest Release");
                    currGrid.Columns.Add("Next Release", "Next Release");
                    currGrid.Columns.Add("First Release", "First Release");
                    currGrid.Columns.Add("Release Schedule", "Release Schedule");
                    currGrid.Columns.Add("Genre", "Genre");
                    currGrid.Columns.Add("Publisher", "Publisher");
                    currGrid.Columns.Add("Writer", "Writer");
                    currGrid.Columns.Add("Artist", "Artist");
                    currGrid.Columns.Add("Finished", "Finished");

                    currGrid.SetBounds(3, 5, 793, 397);
                    currGrid.ReadOnly = true;
                    currGrid.AllowUserToAddRows = false;
                    currGrid.AllowUserToDeleteRows = false;
                    currGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    currGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    currGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    currGrid.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                   
                    currGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(currGrid_CellDoubleClick);

                    Button newButton = new Button();
                    newButton.Click += new EventHandler(newRecordButton_Click);
                    newButton.SetBounds(3, 412, 101, 23);
                    newButton.Text = "New Record";
                    newButton.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    categoryTabs.TabPages[lastIndex].Controls.Add(newButton);

                    Button deleteRecordButton = new Button();
                    deleteRecordButton.Click += new EventHandler(deleteRecordButton_Click);
                    deleteRecordButton.SetBounds(110, 412, 101, 23);
                    deleteRecordButton.Text = "Delete Record";
                    deleteRecordButton.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    categoryTabs.TabPages[lastIndex].Controls.Add(deleteRecordButton);

                    Button deleteCategoryButton = new Button();
                    deleteCategoryButton.Click += new EventHandler(deleteCategoryButton_Click);
                    deleteCategoryButton.SetBounds(217, 412, 101, 23);
                    deleteCategoryButton.Text = "Delete Category";
                    deleteCategoryButton.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    categoryTabs.TabPages[lastIndex].Controls.Add(deleteCategoryButton);
                }
            }
        }

        // Update selected Category data and respective Records when Edit Category form submitted
        public void updateCategory(Category selectedCategory)
        {
            int catNameCount = 0;
            foreach (Category cat in categoryList)
            {
                if (cat.CategoryName == selectedCategory.CategoryName)
                {
                    catNameCount++;
                    if(catNameCount > 1)
                    {
                        var denyResult = MessageBox.Show("Program cannot have more than one Category of the same name.",
                            "Duplicate Category Found", MessageBoxButtons.OK);
                        return;
                    }
                }
            }

            foreach (Category c in categoryList) {
                if (c._Pk == selectedCategory._Pk)
                {
                    resetConnection();

                    string toGo = "'" + selectedCategory.CategoryName + "','" + selectedCategory.ApiLink + "','" + selectedCategory.IndexEditor + "'";
                    Message servMessage = new Message("u", "Cat", selectedCategory._Pk, toGo);
                    Self.sendMessageToServer(servMessage);

                    string resp = receiveResponseFromServer();
                    if (resp == "ok")
                    {
                        var currTab = categoryTabs.SelectedTab;
                        currTab.Name = selectedCategory.CategoryName;
                        currTab.Text = selectedCategory.CategoryName;

                        for (int i = 0; i <= recordList.Count - 1; i++)
                        {
                            Record currRecord = recordList[i];
                            if (currRecord.CategoryName == selectedCategory.CategoryName)
                            {
                                recordList[i].CategoryName = selectedCategory.CategoryName;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Failed to edit Category. Category name: " + selectedCategory.CategoryName);
                    }
                    return;
                }
            }
        }

        // Update selected Record data when Edit Record form submitted
        public void updateRecord(Record selectedRecord)
        {
            for (int i = 0; i <= recordList.Count - 1; i++)
            {
                Record currRecord = recordList[i];
                if (currRecord._Pk == selectedRecord._Pk)
                {
                    resetConnection();
                    selectedRecord.Writer = selectedRecord.Writer.Replace(", ", ";");
                    selectedRecord.Artist = selectedRecord.Artist.Replace(", ", ";");

                    string toGo = "'" + selectedRecord.Name + "'," + selectedRecord.CurrentRead +
                         "," + selectedRecord.LatestRelease + ",'" + selectedRecord.Genre + "','" + selectedRecord.NextRelease +
                         "','" + selectedRecord.Writer + "','" + selectedRecord.Artist + "'," + selectedRecord._Cat_pk +",'" + selectedRecord.Publisher +
                         "','" + selectedRecord.FirstRelease + "','" + selectedRecord.IsFinished + "','" + selectedRecord.ReleaseSchedule +
                         "'";
                    Message servMessage = new Message("u", "Rec", selectedRecord._Pk, toGo);
                    Self.sendMessageToServer(servMessage);

                    string resp = receiveResponseFromServer();
                    if (resp == "ok")
                    {
                        var currTab = categoryTabs.SelectedTab;
                        int dataGridViewIndex = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls.IndexOfKey(currTab.Name);
                        DataGridView currDGV = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls[dataGridViewIndex] as DataGridView;
                        int row = currDGV.CurrentCell.RowIndex;

                        string checkmarkCheck = "";
                        if (selectedRecord.IsFinished == true)
                        {
                            checkmarkCheck = "\u221A";
                        }

                        if (selectedRecord.CategoryName != currTab.Name) {
                            currDGV.Rows.RemoveAt(row);

                            for (int j = 0; j < categoryTabs.TabCount; j++)
                            {
                                this.categoryTabs.SelectTab(j);
                                currTab = categoryTabs.SelectedTab;
                                dataGridViewIndex = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls.IndexOfKey(currTab.Name);
                                currDGV = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls[dataGridViewIndex] as DataGridView;

                                if (currTab.Name == selectedRecord.CategoryName)
                                {
                                    Category fetchedCategory = categoryList[j];
                                    recordList[i]._Cat_pk = fetchedCategory._Pk;

                                    currDGV.Rows.Add(selectedRecord._Pk, selectedRecord.Name, selectedRecord.CurrentRead, selectedRecord.LatestRelease, selectedRecord.NextRelease.ToShortDateString(), 
                                        selectedRecord.FirstRelease.ToShortDateString(), selectedRecord.ReleaseSchedule, selectedRecord.Genre, selectedRecord.Publisher, selectedRecord.Writer, selectedRecord.Artist, checkmarkCheck);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            currDGV = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls[dataGridViewIndex] as DataGridView;

                            currDGV.Rows[row].Cells[1].Value = selectedRecord.Name;
                            currDGV.Rows[row].Cells[2].Value = selectedRecord.CurrentRead;
                            currDGV.Rows[row].Cells[3].Value = selectedRecord.LatestRelease;
                            currDGV.Rows[row].Cells[4].Value = selectedRecord.NextRelease.ToString("M/d/yyyy");
                            currDGV.Rows[row].Cells[5].Value = selectedRecord.FirstRelease.ToString("M/d/yyyy");
                            currDGV.Rows[row].Cells[6].Value = selectedRecord.ReleaseSchedule;
                            currDGV.Rows[row].Cells[7].Value = selectedRecord.Genre;
                            currDGV.Rows[row].Cells[8].Value = selectedRecord.Publisher;
                            currDGV.Rows[row].Cells[9].Value = selectedRecord.Writer;
                            currDGV.Rows[row].Cells[10].Value = selectedRecord.Artist;
                            currDGV.Rows[row].Cells[11].Value = checkmarkCheck;
                        }
                        recordList[i] = selectedRecord;

                    } else {
                        Console.WriteLine("ERROR: Failed to edit Record at index: " + i + ". Record name: " + selectedRecord.Name);
                    }
                    return;
                }
            }
        }

        // Add Category created in AddCategory form or server/database to List, and create new Category tab 
        // on Main Form with columns set up
        public void addCategory(Category newCategory)
        {
            if(isAfterLaunch)
            {
                newCategory.IndexEditor = categoryList.Last().IndexEditor + 1;
                fillTab(newCategory);
                resetConnection();

                string toGo = "'" + newCategory.CategoryName + "','" + newCategory.ApiLink + "','" + newCategory.IndexEditor + "'";
                Message servMessage = new Message("c", "Cat", -1, toGo);
                Self.sendMessageToServer(servMessage);

                string resp = receiveResponseFromServer();
                int newID = Int32.Parse(resp);
                if (newID > 0)
                {
                    newCategory._Pk = newID;
                    categoryList.Add(newCategory);
                    return;
                } else {
                    Console.WriteLine("ERROR: Failed to add new Category. Category name: " + newCategory.CategoryName);
                }
            } else {
                categoryList.Add(newCategory);
            }
        }

        // Add Record created in AddRecord form or from server/database to List
        public void addRecord(Record newRecord)
        {
            var currTab = categoryTabs.SelectedTab;
            var currTabIndex = categoryTabs.SelectedIndex;
            int dataGridViewIndex = categoryTabs.TabPages[currTabIndex].Controls.IndexOfKey(newRecord.CategoryName);
            bool categoryLocated = false;

            if (dataGridViewIndex < 0)
            {
                // Checks if Record should be added to another Category besides currently selected one
                for (int i = 0; i < categoryTabs.TabCount; i++)
                {
                    this.categoryTabs.SelectTab(i);

                    currTab = categoryTabs.SelectedTab;
                    currTabIndex = categoryTabs.SelectedIndex;
                    dataGridViewIndex = categoryTabs.TabPages[currTabIndex].Controls.IndexOfKey(newRecord.CategoryName);

                    if (dataGridViewIndex >= 0)
                    {
                        categoryLocated = true;
                        break;
                    }
                }
                if (categoryLocated == false)
                {
                    var denyResult = MessageBox.Show("Must include an existing Category the new Record is from.", "Missing Category for Record",
                        MessageBoxButtons.OK);
                    return;
                }
            }

            string checkmarkCheck = "";
            if(newRecord.IsFinished == true)
            {
                checkmarkCheck = "\u221A";
            }
           
            // Updates new Record's Category pk
            foreach (Category c in categoryList)
            {
                if (c.CategoryName == currTab.Name)
                {
                    newRecord._Cat_pk = c._Pk;
                }
            }
            
            if (isAfterLaunch)
            {
                resetConnection();
                newRecord.Writer = newRecord.Writer.Replace(", ", ";");
                newRecord.Artist = newRecord.Artist.Replace(", ", ";");

                string toGo = newRecord._Cat_pk + ",'" + newRecord.Name + "','" + newRecord.CurrentRead +
                     "','" + newRecord.LatestRelease + "','" + newRecord.NextRelease + "','" + newRecord.Genre +
                     "','" + newRecord.Publisher + "','" + newRecord.FirstRelease + "','" + newRecord.Writer + 
                     "','" + newRecord.Artist + "'," + newRecord.IsFinished + ",'" + newRecord.ReleaseSchedule+
                     "'";
                Message servMessage = new Message("c", "Rec", -1, toGo);
                Self.sendMessageToServer(servMessage);

                string resp = receiveResponseFromServer();
                int newID = Int32.Parse(resp);
                if (newID > 0)
                {
                    newRecord._Pk = newID;
                    DataGridView currDGV = categoryTabs.TabPages[currTabIndex].Controls[dataGridViewIndex] as DataGridView;
                    currDGV.Rows.Add(newRecord._Pk, newRecord.Name, newRecord.CurrentRead, newRecord.LatestRelease, newRecord.NextRelease.ToShortDateString(),
                        newRecord.FirstRelease.ToShortDateString(), newRecord.ReleaseSchedule, newRecord.Genre, newRecord.Publisher, newRecord.Writer, newRecord.Artist, checkmarkCheck);

                    recordList.Add(newRecord);
                    return;
                }
                else
                {
                    Console.WriteLine("ERROR: Failed to add new Record. Record name: " + newRecord.Name);
                }
            }
            else
            {
                DataGridView currDGV = categoryTabs.TabPages[currTabIndex].Controls[dataGridViewIndex] as DataGridView;
                currDGV.Rows.Add(newRecord._Pk, newRecord.Name, newRecord.CurrentRead, newRecord.LatestRelease, newRecord.NextRelease.ToShortDateString(),
                    newRecord.FirstRelease.ToShortDateString(), newRecord.ReleaseSchedule, newRecord.Genre, newRecord.Publisher, newRecord.Writer, newRecord.Artist, checkmarkCheck);

                recordList.Add(newRecord);
            }
        }

        // Open EditCategory form if a tab is double-clicked
        private void categoryTabs_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            for (int i = 0; i <= categoryList.Count; i++)
            {
                Category currCategory = categoryList[i];
                if (currCategory.CategoryName == categoryTabs.SelectedTab.Name)
                {
                    EditCategory categoryAccessor = new EditCategory(currCategory);
                    categoryAccessor.ShowDialog();
                    return;
                }
            }
        }

        // Open EditRecord form if a Record is double-clicked
        private void currGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pass selected Record to EditRecord form/class as it is generated before ShowDialog
            var currTab = categoryTabs.SelectedTab;

            for (int i = 0; i <= recordList.Count - 1; i++)
            {
                Record currRecord = recordList[i];
                if ((currRecord.CategoryName == currTab.Name) && currRecord != null)
                {
                    int dataGridViewIndex = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls.IndexOfKey(currRecord.CategoryName);
                    DataGridView currDGV = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls[dataGridViewIndex] as DataGridView;

                    int row = currDGV.CurrentCell.RowIndex;
                    DataGridViewCell currCell = currDGV.Rows[row].Cells[0];
                    string testCase = currCell.Value.ToString();

                    for (int j = 0; j <= recordList.Count - 1; j++)
                    {
                        Record secondRecordCheck = recordList[j];
                        if (testCase == secondRecordCheck._Pk.ToString())
                        {
                            EditRecord recordAccessor = new EditRecord(secondRecordCheck);
                            recordAccessor.ShowDialog();
                            return;
                        }
                    }
                }
            }
        }

        // Do not allow "+" tab to be selectable
        private void categoryTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == this.categoryTabs.TabCount - 1)
                e.Cancel = true;
        }

        // Have "+" tab open AddCategory form
        private void categoryTabs_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.categoryTabs.GetTabRect(this.categoryTabs.TabCount - 1).Contains(e.Location))
            {
                AddCategory categoryAccessor = new AddCategory();
                categoryAccessor.ShowDialog();
            }
        }

        // Open AddRecord form
        private void newRecordButton_Click(object sender, EventArgs e)
        {
            AddRecord recordAccessor = new AddRecord(categoryTabs.SelectedTab.Name);
            recordAccessor.ShowDialog();
        }

        // Prompts message if Category should be deleted; if yes, it does so and removes associated Records
        private void deleteCategoryButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you wish to delete this Category? All associated Records will be removed as well.",
                "Confirm Category Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (categoryTabs.TabPages.Count == 1)
                {
                    var denyResult = MessageBox.Show("Cannot delete Category for now if there is only one left.",
                        "Cannot remove last Category", MessageBoxButtons.OK);
                    return;
                }
                var removedCategoryName = categoryTabs.SelectedTab.Name;
                for (int i = 0; i <= categoryList.Count - 1; i++)
                {
                    Category currCategory = categoryList[i];
                    if (currCategory.CategoryName == removedCategoryName)
                    {
                        resetConnection();
                        Message servMessage = new Message("d", "cat", currCategory._Pk, "");
                        Self.sendMessageToServer(servMessage);

                        string resp = receiveResponseFromServer();
                        if (resp == "ok")
                        {
                            categoryList.RemoveAt(i);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Failed to remove Category at index: " + i + ". Category name: " + currCategory.CategoryName);
                            return;
                        }
                    }
                }
                categoryTabs.TabPages.Remove(categoryTabs.SelectedTab);

                for (int j = 0; j <= recordList.Count - 1; j++)
                {
                    Record currRecord = recordList[j];
                    if (currRecord.CategoryName == removedCategoryName)
                    {
                        recordList.RemoveAt(j);
                    }
                }
            }
        }

        // Prompts message if Record should be deleted; if yes, does so
        private void deleteRecordButton_Click(object sender, EventArgs e)
        {
            resetConnection();
            int dataGridViewIndex = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls.IndexOfKey(categoryTabs.SelectedTab.Name);
            DataGridView currDGV = categoryTabs.TabPages[categoryTabs.SelectedIndex].Controls[dataGridViewIndex]
                as DataGridView;

            // Check if at least one Record exists before proceeding
            if (recordList.Count == 0 || currDGV.CurrentCell == null)
            {
                var denyResult = MessageBox.Show("There are no Records to delete in this Category.",
                    "No Records Available", MessageBoxButtons.OK);
                return;
            }

            // Check if a Record is being selected before proceeding 
            var confirmResult = MessageBox.Show("Are you sure you wish to delete this Record?",
                "Confirm Record Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                int row = currDGV.CurrentCell.RowIndex;
                DataGridViewCell currCell = currDGV.Rows[row].Cells[0];
                string testCase = currCell.Value.ToString();

                for (int i = 0; i <= recordList.Count - 1; i++)
                {
                    Record currRecord = recordList[i];
                    if (testCase == currRecord._Pk.ToString())
                    {
                        Message servMessage = new Message("d", "rec", currRecord._Pk, "");
                        Self.sendMessageToServer(servMessage);

                        string resp = receiveResponseFromServer();
                        if (resp == "ok")
                        {
                            currDGV.Rows.RemoveAt(row);
                            recordList.RemoveAt(i);
                            return;
                        } else {
                            Console.WriteLine("ERROR: Failed to remove Record at index: " + i + ". Record name: " + currRecord.Name);
                        }
                    }
                }
            }
        }

        // Once MainForm loads, opens separate windows and their functions
        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Handle optional help window display
            if (startupHelpActive.Checked == true)
            {
                HelpWindow windowAccessor = new HelpWindow();
                windowAccessor.TopMost = true;
                windowAccessor.Show();
            }

            // Handle optional reminders display
            if (remindersActive.Checked == true)
            {
                runReminders();
            }
        }

        // Fetches and displays currently relevant reminders according to nearing next release dates
        private void runReminders()
        {
            DateTime currDate = DateTime.Now;
            List<string> displayList = new List<string>();

            foreach (Record r in recordList)
            {
                if((((r.NextRelease - currDate).TotalDays <= 7 && r.ReleaseSchedule == "Monthly")
                    || ((r.NextRelease - currDate).TotalDays <= 5 && r.ReleaseSchedule == "Biweekly")
                    || ((r.NextRelease - currDate).TotalDays <= 2 && r.ReleaseSchedule == "Weekly"))
                    && ((r.NextRelease - currDate).TotalDays >= 0))
                {
                    displayList.Add(r.Name + " at " + r.NextRelease.ToString("M/d/yyyy"));
                }
            }

            string formattedResult = "";
            foreach(string str in displayList)
            {
                formattedResult += str + "\n";
            }

            MessageBox.Show("Here is a list of your approaching release dates!" + "\n\n" + formattedResult, "Current Reminders", MessageBoxButtons.OK);
        }

        // Checks if reminders should be displayed up program startup or not
        private void remindersActive_CheckedChanged(object sender, EventArgs e)
        {
            checkboxTweaks();
        }

        // Checks if help window should be displayed up program startup or not
        private void startupHelpActive_CheckedChanged(object sender, EventArgs e)
        {
            checkboxTweaks();
        }

        // Writes over config.txt with changed checkbox input
        private void checkboxTweaks()
        {
            if (isAfterLaunch)
            {
                string path = Directory.GetCurrentDirectory() + "\\config.txt";
                string oldText = File.ReadAllText(path);
                string newText = "";
                string[] lines = oldText.Split('\n');

                foreach (string l in lines)
                {
                    string[] set = l.Split(' ');
                    if (set[0] == "help")
                        newText += set[0] + " " + startupHelpActive.Checked + Environment.NewLine;
                    else if (set[0] == "reminder")
                        newText += set[0] + " " + remindersActive.Checked + Environment.NewLine;

                }
                File.WriteAllText(path, newText);
            }
        }

        // Getter/setter for categoryList
        public List<Category> CategoryList
        {
            get { return categoryList; }
            set { categoryList = value; }
        }
    }
}
