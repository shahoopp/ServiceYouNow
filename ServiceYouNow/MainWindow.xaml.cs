using System.Windows;
using ServiceYouNow.src.automation;
using ServiceYouNow.src.pages.home;

namespace ServiceYouNow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new HomePage());

            //_ = RunPlaywrightOnStartup(); // Fire and forget async call
        }

        private async Task RunPlaywrightOnStartup()
        {
            var test = new Automation();
            await test.LaunchBrowserAsync();
        }

    }
}