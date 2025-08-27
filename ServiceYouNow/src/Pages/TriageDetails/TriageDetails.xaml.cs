using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.src.pages.home;
using ServiceYouNow.Helpers;


namespace ServiceYouNow.src.pages.triagedetails
{
    public partial class TriageDetails : Page
    {
        public TriageDetails()
        {
            InitializeComponent();
            var prefs = PreferencesStorage.Load();
            WorkItemNumber.Text = prefs.WorkItemNumber;
            YourFullName.Text = prefs.FullName;
            Notes.Text = prefs.Notes;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new HomePage());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var prefs = PreferencesStorage.Load();
            prefs.WorkItemNumber = WorkItemNumber.Text;
            prefs.FullName = YourFullName.Text;
            prefs.Notes = Notes.Text;
            PreferencesStorage.Save(prefs);
            MessageBox.Show("Details saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
