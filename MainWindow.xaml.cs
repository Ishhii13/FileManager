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
        List<Record> books;
        List<Button> buttons;

        public MainWindow()
        {
            InitializeComponent();
            books = FileManage.GetAllRecords();
            buttons = GatherBTN();
            List.ItemsSource = books;
            ListTitle.Content = "All";
            CurrentFilter(btn_All);
        }

        // view an existing entry
        private void ViewEntry(object sender, MouseButtonEventArgs e)
        {
            if (List.SelectedItem != null)
            {
                Record selectedBook = (Record)List.SelectedItem;
                MainGrid.Children.Add(new View(selectedBook, this));
            }
        }

        //method to add new entry
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Add(new View(this));
        }

        //Methods that changes the list based on the reading status
        private void btn_All_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current, 25, 65);
            UpdateList(books);
            CurrentFilter(btn_All);
            ListTitle.Content = "All";
        }
        private void btn_Favorite_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current,80, 145);
            UpdateList(FileManage.FilteredList(books));
            CurrentFilter(btn_Favorite);
            ListTitle.Content = "Favorite";
        }

        private void btn_Ongoing_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current, 163, 243);
            UpdateList(FileManage.FilteredList(btn_Ongoing.Content.ToString(), books));
            CurrentFilter(btn_Ongoing);
            ListTitle.Content = "Ongoing";
        }

        private void btn_Completed_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current, 260, 363);
            UpdateList(FileManage.FilteredList(btn_Completed.Content.ToString(), books));
            CurrentFilter(btn_Completed);
            ListTitle.Content = "Completed";
        }

        private void btn_OnHold_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current, 377, 465);
            UpdateList(FileManage.FilteredList(btn_OnHold.Content.ToString(), books));
            CurrentFilter(btn_OnHold);
            ListTitle.Content = "On-Hold";
        }

        private void btn_Dropped_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current, 480, 567);
            UpdateList(FileManage.FilteredList(btn_Dropped.Content.ToString(), books));
            CurrentFilter(btn_Dropped);
            ListTitle.Content = "Dropped";
        }

        private void btn_PtR_Click(object sender, RoutedEventArgs e)
        {
            ChangeLineLocation(line_Current, 580, 675);
            UpdateList(FileManage.FilteredList(btn_PtR.Content.ToString(),books));
            CurrentFilter(btn_PtR);
            ListTitle.Content = "Plan to Read";
        }

        //method to return to main
        public void RefreshPage()
        {
            books = FileManage.GetAllRecords();
            UpdateList(books);
            ChangeLineLocation(line_Current, 25, 65);
            ListTitle.Content = "All";
        }

        //methods that change the elements and such
        private void ChangeLineLocation(Line line, int x1, int x2)
        {
            line.X1 = x1; line.X2 = x2;
            line.Y1 = 10; line.Y2 = 10;
        }

        private void UpdateList(List<Record> booklist)
        {
            List.ItemsSource = null;
            List.ItemsSource = booklist;
        }

        private List<Button> GatherBTN()
        { 
            List<Button> btns = new List<Button>();

            btns.Add(btn_All);
            btns.Add(btn_Favorite);
            btns.Add(btn_Ongoing);
            btns.Add(btn_Completed);
            btns.Add(btn_OnHold);
            btns.Add(btn_Dropped);
            btns.Add(btn_PtR);

            return btns;
        }

        private void CurrentFilter(Button currentBTN)
        { 
            foreach(Button btn in buttons)
            {
                if(btn == currentBTN)
                {
                    btn.Foreground = Brushes.SeaGreen;
                    btn.FontWeight = FontWeights.Medium;
                }

                else
                {
                    btn.Foreground = Brushes.Gray;
                    btn.FontWeight = FontWeights.Light;
                }
            }
        }


    }
}