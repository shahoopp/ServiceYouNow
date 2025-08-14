
using System.Windows;
using System.Windows.Controls;
using ServiceYouNow.src.catalog;
using ServiceYouNow.Helpers;   // Corrected
using ServiceYouNow.Models;    // Corrected

namespace ServiceYouNow.src.pages.catalog
{
    public partial class CatalogItem : Page
    {
        private readonly CatalogItemConfig _config;

        public CatalogItem(string itemKey)
        {
            InitializeComponent();

            if (!CatalogItemRegistry.Items.TryGetValue(itemKey, out var cfg) || cfg is null)
            {
                MessageBox.Show("Invalid catalog item key.");

                if (NavigationService?.CanGoBack == true)
                {
                    NavigationService.GoBack();
                    return;
                }

                throw new System.ArgumentException($"Invalid catalog item key: {itemKey}", nameof(itemKey));
            }

            _config = cfg; // ✅ Assigned here

            SetupUI();
            HookEvents();
        }

        private void SetupUI()
        {
            TitleTextBlock.Text = _config.Name;

            ActionTypeComboBox.ItemsSource = _config.ActionTypes;
            EnvironmentComboBox.ItemsSource = _config.Environments;
            AccessTypeComboBox.ItemsSource = _config.AccessTypes;

            ServerNameComboBox.ItemsSource = null;
            ServerNamePanel.Visibility = Visibility.Collapsed;

            WorkItemPanel.Visibility = _config.AssociatedWorkItemRequired ? Visibility.Visible : Visibility.Collapsed;
            BusinessCasePanel.Visibility = _config.BusinessCaseRequired ? Visibility.Visible : Visibility.Collapsed;
        }

        private void HookEvents()
        {
            EnvironmentComboBox.SelectionChanged += EnvironmentComboBox_SelectionChanged;
        }

        private void EnvironmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var env = EnvironmentComboBox.SelectedItem as string;

            if (!_config.ServerName || string.IsNullOrWhiteSpace(env))
            {
                ServerNameComboBox.ItemsSource = null;
                ServerNamePanel.Visibility = Visibility.Collapsed;
                return;
            }

            if (_config.ServerNamesByEnvironment.TryGetValue(env, out var servers) && servers is { Count: > 0 })
            {
                ServerNameComboBox.ItemsSource = servers;
                ServerNamePanel.Visibility = Visibility.Visible;
            }
            else
            {
                ServerNameComboBox.ItemsSource = null;
                ServerNamePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new CatalogPage());
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            var requestData = new PSGDatabaseAccessRequestData
            {
                Environment = EnvironmentComboBox.SelectedItem?.ToString() ?? "",
                ServerName = ServerNameComboBox.SelectedItem?.ToString() ?? "",
                AccessLevel = AccessTypeComboBox.SelectedItem?.ToString() ?? "",
                WorkItemNumber = WorkItemTextBox.Text,
                BusinessCase = BusinessCaseTextBox.Text
            };

            var prefs = PreferencesStorage.Load() ?? new UserPreferences();
            prefs.SavedRequests.Add(requestData);
            PreferencesStorage.Save(prefs);

            MessageBox.Show("Template saved successfully!");
        }
    }
}
