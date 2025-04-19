using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        public View()
        {
            InitializeComponent();
        }

        //EXAMPLE USE CASES FOR THE METHODS
        private void UpdateTextBoxesForRecord(int recordID)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");

            List<Record> allRecords = temp.GetAllRecords();
            Record record = temp.GetRecord(recordID, allRecords);

            lbl_ID.Content = record.id.ToString();
            tbx_Title.Text = record.title;
            tbx_PageStatus.Text = record.pageStatus.ToString();
            tbx_PageCount.Text = record.pageCount.ToString();
            tbx_Status.Text = record.readingStatus;
            tbx_StartDate.Text = record.startDate;
            tbx_LastDate.Text = record.lastDate;
            lbl_Heart.Foreground = record.fav == "+" ? Brushes.Red : Brushes.Black;
        }

        private void btn_book1_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxesForRecord(1);
        }

        private void btn_book2_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxesForRecord(2);
        }

        private void btn_Favorite_Click(object sender, RoutedEventArgs e)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");

            int id = int.Parse(lbl_ID.Content.ToString());
            List<Record> allRecords = temp.GetAllRecords();
            Record record = temp.GetRecord(id, allRecords);

            record.fav = record.fav == "-" ? "+" : "-";

            lbl_Heart.Foreground = record.fav == "+" ? Brushes.Red : Brushes.Black;

            temp.EditRecord(record.id, record.title, record.pageStatus.ToString(), record.pageCount.ToString(), record.readingStatus, record.startDate, record.lastDate, record.fav);
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");

            string title = tbx_Title.Text;
            string pageStatus = tbx_PageStatus.Text;
            string pageCount = tbx_PageCount.Text;
            string readingStatus = tbx_Status.Text;
            string startDate = tbx_StartDate.Text;
            string lastDate = tbx_LastDate.Text;

            temp.AddRecord(title, pageStatus, pageCount, readingStatus, startDate, lastDate);

            MessageBox.Show("Record added.");
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");

            int id = int.Parse(lbl_ID.Content.ToString());
            string title = tbx_Title.Text;
            string pageStatus = tbx_PageStatus.Text;
            string pageCount = tbx_PageCount.Text;
            string readingStatus = tbx_Status.Text;
            string startDate = tbx_StartDate.Text;
            string lastDate = tbx_LastDate.Text;
            string fav = lbl_Heart.Content.ToString();

            temp.EditRecord(id, title, pageStatus, pageCount, readingStatus, startDate, lastDate, fav);

            MessageBox.Show("Record edited.");
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");
            int id = int.Parse(lbl_ID.Content.ToString());

            temp.DeleteRecord(id);

            MessageBox.Show("Record deleted.");
            ClearUI();
        }

        //Extra methods u can use

        //the status parameter should relate to the xaml
        //like if u use a button like in MAL for the statuses
        //write the method like GetGenreCountByStatus(btn.Content.ToString(),...)
        private Dictionary<string, int> GetGenreCountByStatus(string status, List<Record> records, List<Book> books)
        {
            Dictionary<string, int> genreCount = new Dictionary<string, int>();

            Dictionary<string, Book> bookByGenre = new Dictionary<string, Book>();
            foreach (Book book in books)
                bookByGenre.Add(book.title, book);

            foreach (Record record in records)
            {
                if (record.readingStatus == status)
                {
                    Book recordedBook = bookByGenre[record.title];

                    foreach (string genre in recordedBook.genres)
                    {
                        if (genreCount.ContainsKey(genre))
                            genreCount[genre]++;
                        else
                            genreCount[genre] = 1;
                    }
                }
            }

            return genreCount;
        }

        private int PagesByTime(DateTime startDate, DateTime endDate, List<Record> records)
        {
            int pageCount = 0;

            foreach (Record record in records)
            {
                if (DateTime.Parse(record.startDate) < startDate && (DateTime.Parse(record.lastDate) < endDate))
                    pageCount += record.pageCount;
            }

            return pageCount;
        }

        private void ClearUI()
        {
            lbl_ID.Content = "";
            tbx_Title.Text = "";
            tbx_PageStatus.Text = "";
            tbx_PageCount.Text = "";
            tbx_Status.Text = "";
            tbx_StartDate.Text = "";
            tbx_LastDate.Text = "";
            lbl_Heart.Content = "♡";
        }
    }
}
