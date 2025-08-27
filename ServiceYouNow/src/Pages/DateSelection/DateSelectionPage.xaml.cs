using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ServiceYouNow.src.pages.home;
using ServiceYouNow.Helpers;
using ServiceYouNow.Models;


namespace ServiceYouNow.src.pages.dateselection
{
    public partial class DateSelectionPage : Page
    {
        public DateSelectionPage()
        {
            InitializeComponent();

            var sprintInfo = SprintHelper.GetSprintInfo(DateTime.Today);
            PopulateSprintUI(sprintInfo);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new HomePage());
        }




        private void PopulateSprintUI(SprintInfo sprintInfo)
        {
            // Load saved preferences
            var prefs = PreferencesStorage.Load();
            var savedDates = prefs.SelectedSprintDates ?? new List<DateTime>();

            MyStackPanel.Children.Clear();

            // Sprint Header
            TextBlock header = new TextBlock
            {
                Text = $"Sprint {sprintInfo.SprintNumber} (Day {sprintInfo.DayNumber} of 10)",
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            MyStackPanel.Children.Add(header);

            for (int row = 0; row < 2; row++)
            {
                StackPanel rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                List<CheckBox> rowCheckBoxes = new List<CheckBox>();

                for (int i = 0; i < 5; i++)
                {
                    int index = row * 5 + i;
                    var date = sprintInfo.SprintDates[index];

                    StackPanel dayPanel = new StackPanel
                    {
                        Margin = new Thickness(10),
                        Width = 150
                    };

                    TextBlock dayText = new TextBlock
                    {
                        Text = $"Day {index + 1}",
                        FontSize = 25,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    TextBlock dateText = new TextBlock
                    {
                        Text = date.ToString("MMM dd, yyyy"),
                        FontSize = 25,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    CheckBox checkBox = new CheckBox
                    {
                        Name = $"Day{index + 1}CheckBox",
                        LayoutTransform = new ScaleTransform(1.75, 1.75),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    // ✅ Restore selection if date matches saved preferences
                    if (savedDates.Any(d => d.Date == date.Date))
                    {
                        checkBox.IsChecked = true;
                    }

                    rowCheckBoxes.Add(checkBox);

                    dayPanel.Children.Add(dayText);
                    dayPanel.Children.Add(dateText);
                    dayPanel.Children.Add(checkBox);

                    rowPanel.Children.Add(dayPanel);
                }

                // "Select All" checkbox logic remains unchanged
                CheckBox selectAllCheckBox = new CheckBox
                {
                    Margin = new Thickness(20, 0, 0, 0),
                    LayoutTransform = new ScaleTransform(1.75, 1.75),
                    VerticalAlignment = VerticalAlignment.Center
                };

                selectAllCheckBox.Checked += (s, e) =>
                {
                    foreach (var cb in rowCheckBoxes) cb.IsChecked = true;
                };

                selectAllCheckBox.Unchecked += (s, e) =>
                {
                    foreach (var cb in rowCheckBoxes) cb.IsChecked = false;
                };

                rowPanel.Children.Add(selectAllCheckBox);
                MyStackPanel.Children.Add(rowPanel);
            }
        }



        private void SaveSelectedDates()
        {
            var selectedDates = new List<DateTime>();

            foreach (var child in MyStackPanel.Children)
            {
                if (child is StackPanel rowPanel)
                {
                    foreach (var dayPanel in rowPanel.Children.OfType<StackPanel>())
                    {
                        var checkBox = dayPanel.Children.OfType<CheckBox>().FirstOrDefault();
                        var dateText = dayPanel.Children.OfType<TextBlock>().LastOrDefault();

                        if (checkBox?.IsChecked == true && dateText != null)
                        {
                            if (DateTime.TryParse(dateText.Text, out DateTime parsedDate))
                            {
                                selectedDates.Add(parsedDate);
                            }
                        }
                    }
                }
            }

            var prefs = PreferencesStorage.Load();
            prefs.SelectedSprintDates = selectedDates;
            PreferencesStorage.Save(prefs);
        }

        private void ViewSavedDates_Click(object sender, RoutedEventArgs e)
        {
            var prefs = PreferencesStorage.Load();
            var savedDates = prefs.SelectedSprintDates;
            if (savedDates == null || !savedDates.Any())
            {
                MessageBox.Show("No dates have been saved yet.", "No Saved Dates", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string datesList = string.Join("\n", savedDates.Select(d => d.ToString("MMM dd, yyyy")));
            MessageBox.Show($"Saved Dates:\n{datesList}", "Saved Dates", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSelectedDates();
            MessageBox.Show("Selected dates saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
