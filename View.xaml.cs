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
using static System.Reflection.Metadata.BlobBuilder;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        MainWindow main;
        Record current;
        string mode;

        //constructor for current entries
        public View(Record selectedBook, MainWindow manWin)
        {
            InitializeComponent();
            current = selectedBook;
            main = manWin;

            mode = "Edit";
            btn_Edit.Content = "Edit";
            btn_Delete.Content = "Cancel";

            AddStatusList();
            UpdateTextBoxesForRecord(current);
        }

        //constructor for new entries
        public View(MainWindow manWin)
        {
            InitializeComponent();
            current = new Record("", 0, 0, "", "--/--/----", "--/--/----");
            main = manWin;

            mode = "Add";
            btn_Edit.Content = "Add";
            btn_Delete.Content = "Cancel";

            AddStatusList();
            UpdateTextBoxesForRecord(current);
        }

        //EXAMPLE USE CASES FOR THE METHODS
        private void UpdateTextBoxesForRecord(Record record)
        {
            lbl_ID.Content = record.id.ToString();
            tbx_Title.Text = record.title;
            tbx_PageStatus.Text = record.pageStatus.ToString();
            tbx_PageCount.Text = record.pageCount.ToString();
            tbx_Status.SelectedValue = record.readingStatus;
            SeparateDate(record.startDate,tbx_SD_Month, tbx_SD_Day, tbx_SD_Year);
            SeparateDate(record.lastDate, tbx_LD_Month, tbx_LD_Day, tbx_LD_Year);
            lbl_Heart.Foreground = record.fav == "+" ? Brushes.Red : Brushes.Black;
        }

        private void btn_Favorite_Click(object sender, RoutedEventArgs e)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");

            int id = int.Parse(lbl_ID.Content.ToString());
            List<Record> allRecords = FileManage.GetAllRecords();
            Record record = FileManage.GetRecord(id, allRecords);

            record.fav = record.fav == "-" ? "+" : "-";

            lbl_Heart.Foreground = record.fav == "+" ? Brushes.Red : Brushes.Black;

            temp.EditRecord(record.id, record.title, record.pageStatus.ToString(), record.pageCount.ToString(), record.readingStatus, record.startDate, record.lastDate, record.fav);
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(lbl_ID.Content.ToString());
            string title = tbx_Title.Text;
            string pageStatus = tbx_PageStatus.Text;
            string pageCount = tbx_PageCount.Text;
            string readingStatus = tbx_Status.SelectedItem.ToString();
            string startDate = MergeDate(tbx_SD_Month, tbx_SD_Day, tbx_SD_Year);
            string lastDate = MergeDate(tbx_SD_Month, tbx_SD_Day, tbx_SD_Year);
            string fav = lbl_Heart.Content.ToString();

            //EDIT MODE

            if (mode == "Edit")
            {
                MessageBoxResult result = MessageBox.Show("Save changes made to the entry?", "Save Changes",MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    current.EditRecord(id, title, pageStatus,pageCount,readingStatus,startDate,lastDate,fav);
                    MessageBox.Show("Record edited.");

                    main.RefreshPage();
                    main.MainGrid.Children.Remove(this);
                }
            }

            //ADD MODE

            else if (mode == "Add")
            {
                MessageBoxResult result = MessageBox.Show("Add the entry to the list?", "Add Entry", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    current.AddRecord(current);
                    MessageBox.Show("Record added.");

                    main.RefreshPage();
                    main.MainGrid.Children.Remove(this);
                }
            }

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Delete the entry? This action cannot be undone", "Delete", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Yes)
            {
                Record temp = new Record(0, "", 0, 0, "", "", "", "");
                int id = int.Parse(lbl_ID.Content.ToString());

                temp.DeleteRecord(id);

                MessageBox.Show("Record deleted.");
                main.MainGrid.Children.Remove(this);
            }
            
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
            lbl_Heart.Content = "♡";
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            main.MainGrid.Children.Remove(this);
        }

        private void AddStatusList()
        {
            tbx_Status.Items.Add("");
            tbx_Status.Items.Add("Ongoing");
            tbx_Status.Items.Add("Completed");
            tbx_Status.Items.Add("On-Hold");
            tbx_Status.Items.Add("Dropped");
            tbx_Status.Items.Add("Plan to Read");
        }

        private void SeparateDate(string date, TextBox month, TextBox day, TextBox year)
        {
            string[] part = date.Split('/');

            month.Text = part[0];
            day.Text = part[1];
            year.Text = part[2];
        }

        private string MergeDate(TextBox month, TextBox day, TextBox year)
        {
            string updatedDate = $"{month.Text}/{day.Text}/{year.Text}";
            return updatedDate;
        }
    }
}
