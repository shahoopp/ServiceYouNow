using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.Helpers;
using ServiceYouNow.src.pages.home;

namespace ServiceYouNow.src.pages.login
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();

            // Load saved preferences
            var prefs = PreferencesStorage.Load();
            UsernameBox.Text = prefs.Username;
            PasswordBox.Password = prefs.Password;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new HomePage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var prefs = PreferencesStorage.Load();
            prefs.Username = UsernameBox.Text;
            prefs.Password = PasswordBox.Password;
            PreferencesStorage.Save(prefs);
            MessageBox.Show("Login info saved.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /*private void ShowPassWordButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            VisiblePasswordBox.Text = PasswordBox.Password;
            VisiblePasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPassWordButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            VisiblePasswordBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;
        }*/
    }
}
