using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    static public class FileManage
    {
        static string recordPath = @"C:\Users\Krizia\Source\Repos\FileManager\Tables\Record.txt";

        static public List<Record> GetAllRecords()
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

        static public Record GetRecord(int inputID, List<Record> allRecords)
        {
            foreach (Record record in allRecords)
            {
                if (inputID == record.id)
                    return record;
            }

            return null; //null will never be returned
        }

        static public List<Record> FilteredList(string filter,List<Record> allRecords)
        { 
            List<Record> records = new List<Record>();
            
            foreach (Record record in allRecords)
            { 
                if(record.readingStatus == filter)
                { records.Add(record); }
            }

            return records;
        }

        static public List<Record> FilteredList(List<Record> allRecords)
        {
            List<Record> records = new List<Record>();

            foreach (Record record in allRecords)
            {
                if (record.fav == "+")
                { records.Add(record); }
            }

            return records;
        }

        static public void SaveFile(List<Record> allRecords)
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

    }
}
