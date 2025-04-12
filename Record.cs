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
        //oh also the Record.txt is gpt except for the 1st 2 :P
        //i got lazy to add :P

        string recordPath = "D:\\Visual Studio\\Visual Studio repos\\FileManager\\Tables\\Record.txt";

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

                    Record record = new Record(int.Parse(content[0]), content[1], int.Parse(content[2]), int.Parse(content[3]), content[4], content[5], content[6], content[7]);

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
                    return record;
            }

            return null; //null will never be returned
        }

        //method for adding record
        //the parameters will correspond with content of the record (minus the id and the favorites)
        public void AddRecord(string cont2, string cont3, string cont4, string cont5, string cont6, string cont7)
        {
            int newID = GenerateNewID();

            //the book is not favorited?? at first
            Record newRecord = new Record(newID, cont2, int.Parse(cont3), int.Parse(cont4), cont5, cont6, cont7, "-");

            List<Record> allRecords = GetAllRecords();
            allRecords.Add(newRecord);

            //not exactly sure how OrderBy works, it's copy/paste from StackOverflow
            allRecords = allRecords.OrderBy(r => r.id).ToList();

            SaveFile(allRecords);
        }

        //method for editing record
        public void EditRecord(int inputID, string cont2, string cont3, string cont4, string cont5, string cont6, string cont7, string cont8)
        {
            List<Record> allRecords = GetAllRecords();
            Record record = GetRecord(inputID, allRecords);

            record.title = cont2;
            record.pageStatus = int.Parse(cont3);
            record.pageCount = int.Parse(cont4);
            record.readingStatus = cont5;
            record.startDate = cont6;
            record.lastDate = cont7;
            record.fav = cont8;

            SaveFile(allRecords);
        }

        //method for deleting record
        public void DeleteRecord(int inputID)
        {
            List<Record> allRecords = GetAllRecords();
            List<Record> filteredRecords = new List<Record>();

            foreach (Record record in allRecords)
            {
                if (record.id == inputID)
                    continue;

                filteredRecords.Add(record); // keep the rest
            }

            SaveFile(filteredRecords);
        }

        private void SaveFile(List<Record> allRecords)
        {
            using (StreamWriter sw = new StreamWriter(recordPath))
            {
                foreach (Record record in allRecords)
                {
                    string line = $"{record.id}|{record.title}|{record.pageStatus}|{record.pageCount}|{record.readingStatus}|{record.startDate}|{record.lastDate}|{record.fav}";
                    sw.WriteLine(line);
                }
            }
        }

        //in case a record is deleted
        private int GenerateNewID()
        {
            List<Record> allRecords = GetAllRecords();
            List<int> existingIDs = new List<int>();

            foreach (Record record in allRecords)
                existingIDs.Add(record.id);

            int newID = 1;

            while (existingIDs.Contains(newID))
                newID++;

            return newID;
        }
    }
}
