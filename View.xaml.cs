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
            btn_Delete.Content = "Delete";

            AddStatusList();
            ToggleEdit(false);
            UpdateTextBoxesForRecord(current);
        }

        //constructor for new entries
        public View(MainWindow manWin)
        {
            InitializeComponent();
            current = new Record("Book Title", 0, 0, "", "--/--/----", "--/--/----");
            current.AddRecord(current);
            main = manWin;


            mode = "Add";
            btn_Edit.Content = "Add";
            btn_Delete.Content = "Cancel";

            AddStatusList();
            ToggleEdit(true);
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

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            //EDIT MODE

            if (mode == "Edit")
            {
                if (btn.Content.ToString() == "Edit")
                {
                    btn_Edit.Content = "Save";
                    btn_Delete.Content = "Cancel";

                    ToggleEdit(true);
                }

                else if (btn.Content.ToString() == "Save")
                {
                    MessageBoxResult result = MessageBox.Show("Save changes made to the entry?", "Save Changes", MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.OK)
                    {
                        TransferContentToRecord();
                        MessageBox.Show("Record edited.");

                        main.RefreshPage();
                        main.MainGrid.Children.Remove(this);
                    }
                }
            }

            //ADD MODE

            else if (mode == "Add")
            {
                MessageBoxResult result = MessageBox.Show("Add the entry to the list?", "Add Entry", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    TransferContentToRecord();
                    MessageBox.Show("Record added.");

                    main.RefreshPage();
                    main.MainGrid.Children.Remove(this);
                }
            }

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn.Content.ToString() == "Delete")
            {
                MessageBoxResult result = MessageBox.Show("Delete the entry? This action cannot be undone", "Delete", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    CancelRecord();
                    MessageBox.Show("Record deleted.");

                    main.RefreshPage();
                    main.MainGrid.Children.Remove(this);
                }
            }

            else if (btn.Content.ToString() == "Cancel")
            {
                MessageBoxResult result = MessageBox.Show("Discard the changes made?", "Cancel Changes", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Canceled changes.");

                    if (mode == "Edit")
                    {
                        UpdateTextBoxesForRecord(current);
                        ToggleEdit(false);

                        btn_Edit.Content = "Edit";
                        btn_Delete.Content = "Delete";
                    }

                    else if(mode == "Add")
                    {
                        CancelRecord();
                        main.MainGrid.Children.Remove(this);
                    }
                }
            }
            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (mode == "Add")
            {
                CancelRecord();
            }
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

        private void TransferContentToRecord()
        {
            int id = int.Parse(lbl_ID.Content.ToString());
            string title = tbx_Title.Text;
            string pageStatus = tbx_PageStatus.Text;
            string pageCount = tbx_PageCount.Text;
            string readingStatus = tbx_Status.SelectedItem.ToString();
            string startDate = MergeDate(tbx_SD_Month, tbx_SD_Day, tbx_SD_Year);
            string lastDate = MergeDate(tbx_LD_Month, tbx_LD_Day, tbx_LD_Year);
            string fav = lbl_Heart.Content.ToString();

            current.EditRecord(id, title, pageStatus, pageCount, readingStatus, startDate, lastDate, fav);
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

        private void lbl_Heart_Click(object sender, RoutedEventArgs e)
        {
            current.fav = current.fav == "-" ? "+" : "-";
            lbl_Heart.Foreground = current.fav == "+" ? Brushes.Red : Brushes.Black;

            current.EditRecord(current.id, current.title, current.pageStatus.ToString(), current.pageCount.ToString(), current.readingStatus, current.startDate, current.lastDate, current.fav);
        }

        private void TodayDate(object sender, RoutedEventArgs e)
        { 
            string today = DateTime.Today.ToString("MM/dd/yyyy");
            
            if (sender is Button btn)
            {
                SetDate(btn, today, true);
            }
        }

        private void ToggleEdit(bool set)
        {
            tbx_Title.IsEnabled = set;
            tbx_Status.IsEnabled = set;
            tbx_PageStatus.IsEnabled = set;
            tbx_PageCount.IsEnabled = set;

            btn_SD_Today.IsEnabled = set;
            btn_SD_Unknown.IsEnabled = set;
            tbx_SD_Month.IsEnabled = set;
            tbx_SD_Day.IsEnabled = set;
            tbx_SD_Year.IsEnabled = set;

            btn_LD_Today.IsEnabled = set;
            btn_LD_Unknown.IsEnabled = set;
            tbx_LD_Month.IsEnabled = set;
            tbx_LD_Day.IsEnabled = set;
            tbx_LD_Year.IsEnabled = set;
        }

        private void UnknownDate(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Content.ToString() == "Set Date")
                {
                    SetDate(btn, "--/--/----", true);
                }
                else if (btn.Content.ToString() == "Date Unknown")
                {
                    SetDate(btn, "--/--/----", false);
                }
            }
        }

        private void SetDate(Button btn, string date, bool set)
        {
            if (btn.Name == "btn_SD_Unknown" || btn.Name == "btn_SD_Today")
            {
                SeparateDate(date, tbx_SD_Month, tbx_SD_Day, tbx_SD_Year);
                tbx_SD_Month.IsEnabled = set; tbx_SD_Day.IsEnabled = set; tbx_SD_Year.IsEnabled = set;
            }

            else if (btn.Name == "btn_LD_Unknown" || btn.Name == "btn_LD_Today")
            {
                SeparateDate(date, tbx_LD_Month, tbx_LD_Day, tbx_LD_Year);
                tbx_LD_Month.IsEnabled = set; tbx_LD_Day.IsEnabled = set; tbx_LD_Year.IsEnabled = set;
            }
        }

        private void CancelRecord()
        {
            Record temp = new Record(0, "", 0, 0, "", "", "", "");
            int id = int.Parse(lbl_ID.Content.ToString());

            temp.DeleteRecord(id);
        }
    }
}
