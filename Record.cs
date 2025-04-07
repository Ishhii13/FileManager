using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal class Record
    {
        static string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string resourcesFolderPath = Path.Combine(Directory.GetParent(appFolderPath).Parent.FullName, "Book.txt");

        //Uri recordPath = new Uri("C:\\Users\\22-0371c\\source\\repos\\FileManager\\Tables\\Record.txt");
        //var resourceStream = ApplicationException.GetResourceStream(recordPath);
        //Uri bookPath = new Uri(@"Book.txt");

        int id;
        string title;
        int pageStatus;
        int pageCount;
        string readingStatus;
        string startDate;
        string lastDate;
        char fav;

        public Record(int newID, string newTitle, int pStat, int pCount, string readStat, string sDate, string lDate, char favorite)
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

        //method for deleting record (ensure error handling)

        //method for adding record
        //public Record AddRecord()
        //{
        //    using (StreamReader sr = new StreamReader(resourcesFolderPath))
        //    {

        //    }
        //}

        //method for editing record
    }
}
