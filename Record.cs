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
    public class Record
    {
        //hello best friends
        //sorry in advance for all the parameters
        //but the methods needed to be flexible lol
        //also edit this all u want cuz im not used to using get set
        //oh also the Record.txt is gpt except for the 1st 2 :P
        //i got lazy to add :P


        public int id { get; set; }
        public string title { get; set; }
        public int pageStatus { get; set; }
        public int pageCount { get; set; }
        public string readingStatus { get; set; }
        public string startDate { get; set; }
        public string lastDate { get; set; }
        public string fav { get; set; }

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

        public Record(string newTitle, int pStat, int pCount, string readStat, string sDate, string lDate)
        {
            id = GenerateNewID();
            title = newTitle;
            pageStatus = pStat;
            pageCount = pCount;
            readingStatus = readStat;
            startDate = sDate;
            lastDate = lDate;
            fav = "-";
        }

        //method for adding record
        //the parameters will correspond with content of the record (minus the id and the favorites)
        public void AddRecord(Record current)
        {
            List<Record> allRecords = FileManage.GetAllRecords();
            allRecords.Add(current);

            //not exactly sure how OrderBy works, it's copy/paste from StackOverflow
            allRecords = allRecords.OrderBy(r => r.id).ToList();
            FileManage.SaveFile(allRecords);
        }

        //method for editing record
        public void EditRecord(int inputID, string cont2, string cont3, string cont4, string cont5, string cont6, string cont7, string cont8)
        {
            List<Record> allRecords = FileManage.GetAllRecords();
            Record record = FileManage.GetRecord(inputID, allRecords);

            record.title = cont2;
            record.pageStatus = int.Parse(cont3);
            record.pageCount = int.Parse(cont4);
            record.readingStatus = cont5;
            record.startDate = cont6;
            record.lastDate = cont7;
            record.fav = cont8;

            FileManage.SaveFile(allRecords);
        }

        //method for deleting record
        public void DeleteRecord(int inputID)
        {
            List<Record> allRecords = FileManage.GetAllRecords();
            List<Record> filteredRecords = new List<Record>();

            foreach (Record record in allRecords)
            {
                if (record.id == inputID)
                    continue;

                filteredRecords.Add(record); // keep the rest
            }

            FileManage.SaveFile(filteredRecords);
        }

        //in case a record is deleted
        private int GenerateNewID()
        {
            List<Record> allRecords = FileManage.GetAllRecords();
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
