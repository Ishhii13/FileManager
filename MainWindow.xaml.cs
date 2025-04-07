using System.IO;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //EXAMPLE USE CASES FOR THE METHODS
        private void UpdateTextBoxesForRecord(int recordID)
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");

            // Get all records
            List<Record> allRecords = temp.GetAllRecords(); // Adjust as needed

            // Find the record with the given ID
            Record record = allRecords.FirstOrDefault(r => r.id == recordID);

            if (record != null)
            {
                // Update the textboxes with the record's details
                lbl_ID.Content = record.id.ToString();
                tbx_Title.Text = record.title;
                tbx_PageStatus.Text = record.pageStatus.ToString();
                tbx_PageCount.Text = record.pageCount.ToString();
                tbx_Status.Text = record.readingStatus;
                tbx_StartDate.Text = record.startDate;
                tbx_LastDate.Text = record.lastDate;
                // If you have any logic for the favorite button, update it as well
                lbl_Heart.Content = record.fav == "-" ? "♡" : "❤️"; // Adjust based on the favorite logic
            }
            else
            {
                MessageBox.Show("Record with ID " + recordID + " not found.");
            }
        }

        // Example usage: update the textboxes when the window is loaded or a button is clicked
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btn_book1_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxesForRecord(1);
        }

        private void btn_book2_Click(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxesForRecord(2);
        }
    }
}