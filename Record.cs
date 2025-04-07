using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace FileManager
{
    internal class Record
    {
        //hello best friends
        //sorry in advance for all the parameters
        //but the methods needed to be flexible lol
        //also edit this all u want cuz im not used to using get set

        //static string tablePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Tables");
        //string bookPath = System.IO.Path.Combine(tablePath, "Book.txt");
        //string recordPath = System.IO.Path.Combine(tablePath, "Record.txt");

        string bookPath = "D:\\Visual Studio\\Visual Studio repos\\FileManager\\Tables\\Book.txt";
        string recordPath = "D:\\Visual Studio\\Visual Studio repos\\FileManager\\Tables\\Record.txt";

        bool addingBook = false; //true = adding, false = editing, deleting; this will be used for appending

        public int id;
        public string title;
        public int pageStatus;
        public int pageCount;
        public string readingStatus;
        public string startDate;
        public string lastDate;
        public string fav;

        public Record(int newID, string newTitle, int pStat, int pCount, string readStat, string sDate, string lDate, string favorite)
        {
            id = newID;
            title = newTitle;
            pageStatus = pStat;
            pageCount = pCount;
            readingStatus = readStat;
            startDate = sDate;
            lastDate = lDate;
            fav = favorite;
        }

        public List<Record> GetAllRecords()
        {
            List<Record> allRecords = new List<Record>();

            using (StreamReader sr = new StreamReader(recordPath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] content = line.Split('|');

                    int id = int.Parse(content[0]);
                    string title = content[1];
                    int pageStatus = int.Parse(content[2]);
                    int pageCount = int.Parse(content[3]);
                    string readingStatus = content[4];
                    string startDate = content[5];
                    string lastDate = content[6];
                    string fav = content[7];

                    Record record = new Record(id, title, pageStatus, pageCount, readingStatus, startDate, lastDate, fav);

                    allRecords.Add(record);
                }
            }

            return allRecords;
        }

        public Record GetRecord(int inputID, List<Record> allRecords)
        {
            foreach (Record record in allRecords)
            {
                if (inputID == record.id)
                {
                    return record;
                }
            }

            return null; //null will never be returned
        }

        //method for adding record
        //the parameters will correspond with content of the record (minus the id and the favorites)
        public void AddRecord(string cont2, string cont3, string cont4, string cont5, string cont6, string cont7)
        {
            addingBook = true;

            //the book is not favorited? at first
            string line = $"|{cont2}|{cont3}|{cont4}|{cont5}|{cont6}|{cont7}|-";

            SaveFile(line);
        }

        //method for editing record
        public void EditRecord(int inputID, string cont2, string cont3, string cont4, string cont5, string cont6, string cont7, string cont8)
        {
            Record record = GetRecord(inputID, GetAllRecords());

            if (record != null)
            {
                string title = cont2;
                string pageStatus = cont3;
                string pageCount = cont4;
                string readingStatus = cont5;
                string startDate = cont6;
                string lastDate = cont7;
                string fav = cont8;

                string line = $"{inputID}|{cont2}|{cont3}|{cont4}|{cont5}|{cont6}|{cont7}|{cont8}";

                SaveFile(line);
            }
            else
            {
                MessageBox.Show("Record not found.");
            }
        }

        //method for deleting record
        public void DeleteRecord(int inputID)
        {
            List<string> allRecords = new List<string>();

            foreach (Record record in GetAllRecords())
            {
                if (record.id != inputID)
                    continue;

                string cont1 = record.id.ToString();
                string cont2 = record.title;
                string cont3 = record.pageStatus.ToString();
                string cont4 = record.pageCount.ToString();
                string cont5 = record.readingStatus;
                string cont6 = record.startDate;
                string cont7 = record.lastDate;
                string cont8 = record.fav;

                string line = $"{cont1}|{cont2}|{cont3}|{cont4}|{cont5}|{cont6}|{cont7}|{cont8}";
            }

            SaveFile(allRecords);
        }

        //FIX THIS PART
        //saves changes to the file
        private void SaveFile(string line) //line = the record you changed
        {
            using (StreamWriter sw = new StreamWriter(recordPath, addingBook))
            {
                if (addingBook) //add
                {
                    string newID = GenerateNewID();
                    sw.WriteLine(newID + line);
                }
                else //edit
                {
                    
                }
            }

            MessageBox.Show("Record added successfully!"); //u can remove this if u want

            addingBook = false; //prolly a better way to do this but I'm lazy lol
        }

        private void SaveFile(List<string> allRecords)
        {
            using (StreamWriter sw = new StreamWriter(recordPath))
            {
                foreach (string record in allRecords)
                {
                    sw.WriteLine(record);
                }
            }
        }

        //in case a record is deleted
        private string GenerateNewID()
        {
            string newID = "0";

            List<Record> allRecords = GetAllRecords();
            int lowestNum = 0;

            foreach (Record record in allRecords)
            {
                if (record.id == lowestNum + 1)
                    lowestNum = record.id;
            }

            newID = lowestNum + 1.ToString();

            return newID;
        }
    }
}
