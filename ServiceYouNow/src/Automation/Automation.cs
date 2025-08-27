namespace ServiceYouNow.src.automation
{
    using Microsoft.Playwright;
    using ServiceYouNow.Helpers;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class Automation
    {
        /// <summary>
        /// Core per-date automation. Expects date in "dd-MMM-yy" (e.g., "25-Aug-25")
        /// Uses the parsed date everywhere you previously used DateTime.Now,
        /// including file names, date pickers, and date fields.
        /// </summary>
        public async Task RunAutomationAsync(IPage page, string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                throw new ArgumentException("date is required in format dd-MMM-yy");

            // Parse dd-MMM-yy (culture-invariant)
            var runDate = DateTime.ParseExact(date, "dd-MMM-yy", CultureInfo.InvariantCulture);

            // Formats:
            // 1) For ServiceNow date fields (text inputs)
            string snShort = runDate.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);     // e.g., 25-Aug-25
            // 2) For the UI date button you mentioned ("Monday, August 25")
            string snUiPicker = runDate.ToString("dddd, MMMM d", CultureInfo.InvariantCulture);
            // 3) For exported file name to make it deterministic by date
            string fileStamp = runDate.ToString("MMM_dd_yyyy", CultureInfo.InvariantCulture); // e.g., Aug_25_2025

            // --- Go to ServiceNow landing (context should already be logged-in if LaunchBrowserAsync logged-in once) ---
            await page.GotoAsync("https://hoopp.service-now.com/sp?id=index");

            // If you do need to assert logged-in status, you can add a quick check here.

            // Navigate to Incidents (existing logic)
            await Locators.SupportViewButton(page).ClickAsync();
            await Task.Delay(5000);

            if (await Locators.AllIncidentsButton(page).IsVisibleAsync())
            {
                await Locators.AllIncidentsButton(page).ClickAsync();
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            }
            else
            {
                await Locators.IncidentButton(page).ClickAsync();
                await Locators.AllIncidentsButton(page).ClickAsync();
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            }
            await Task.Delay(1000);

            // Saved filter
            await SavedFilterLocators.ActionsButton(page).ClickAsync();
            await SavedFilterLocators.FiltersButton(page).ClickAsync();
            await SavedFilterLocators.AutomationFilterButton(page).ClickAsync();
            await Task.Delay(2000);

            await Locators.FilterButton(page).ClickAsync();
            await Locators.ChooseDateButton(page).ClickAsync();

            // Use UI-format date here (Monday, August 25)
            await Locators.DateButton(page, snUiPicker).ClickAsync();

            await Locators.RunFilterButton(page).ClickAsync();
            await Task.Delay(2000);

            // Export
            await ExportLocators.NumberColumn(page).ClickAsync();
            await ExportLocators.ExportButton(page).ClickAsync();
            await ExportLocators.ExcelButton(page).ClickAsync();

            var downloadTask = page.WaitForDownloadAsync();
            await ExportLocators.DownloadButton(page).ClickAsync();
            var download = await downloadTask;

            // Save using the runDate-based stamp
            string fileName = $"incidents_{fileStamp}.xlsx";
            var filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $@"..\..\..\{fileName}"));
            await download.SaveAsAsync(filePath);

            // Standard Change navigation (existing branching preserved)
            if (await Locators.StandardChangeCatalogButton(page).IsVisibleAsync())
            {
                await Locators.StandardChangeCatalogButton(page).ClickAsync();
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            }
            else
            {
                if (await Locators.StandardChangeButton(page).IsVisibleAsync())
                {
                    await Locators.StandardChangeButton(page).ClickAsync();
                    await Locators.StandardChangeCatalogButton(page).ClickAsync();
                    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                }
                else
                {
                    await Locators.ChangeButton(page).ClickAsync();
                    await Locators.StandardChangeButton(page).ClickAsync();
                    await Locators.StandardChangeCatalogButton(page).ClickAsync();
                    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                }
            }
            await Task.Delay(1000);

            await Locators.PSGPension(page).ClickAsync();
            await Locators.NextPageButton(page).ClickAsync();
            await Locators.IncidentWorkaroundsAndResolutions(page).ClickAsync();
            await Locators.ManageAttachments(page).ClickAsync();

            // Upload the exact file we just saved
            var fileInput = page
                .FrameLocator("iframe[name='gsft_main']")
                .Locator("input[type='file']");

            await fileInput.WaitForAsync(new() { State = WaitForSelectorState.Attached });
            await fileInput.SetInputFilesAsync(filePath);

            await Locators.DownloadAllButton(page).WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await Locators.CloseAttachmentsButton(page).ClickAsync();

            // Form fills (unchanged except using runDate where dates are used)
            await Locators.IncidentState(page).ClickAsync();
            await Locators.IncidentState(page).SelectOptionAsync(new[] { "5" }); // Completed
            await Locators.IncidentState(page).ClickAsync(); // collapse dropdown

            // The following assumes these values are already placed by LaunchBrowserAsync in fields (workItemNumber, fullName, etc.)
            // If you prefer, pass them into this method, or query prefs again.

            var prefs = PreferencesStorage.Load();
            string workItemNumber = prefs.WorkItemNumber;
            string fullName = prefs.FullName;

            await Locators.DeploymentAssignedTo(page).ClickAsync();
            await Locators.DeploymentAssignedToClicked(page).FillAsync(fullName);

            await Locators.DevelopedBy(page).ClickAsync();
            await Locators.DevelopedBy(page).SelectOptionAsync(new[] { "hoopp internal" });

            await Locators.DevelopmentArtifactLocation(page).ClickAsync();
            await Locators.DevelopmentArtifactLocation(page).FillAsync(workItemNumber);

            await Locators.PREDeploymentTestArtifact(page).ClickAsync();
            await Locators.PREDeploymentTestArtifact(page).FillAsync(workItemNumber);

            await Locators.POSTDeploymentTestArtifact(page).ClickAsync();
            await Locators.POSTDeploymentTestArtifact(page).FillAsync(workItemNumber);

            await Locators.DeploymentArtifactLocation(page).ClickAsync();
            await Locators.DeploymentArtifactLocation(page).FillAsync(workItemNumber);

            await Locators.ScheduleTab(page).ClickAsync();

            // Date fields: use dd-MMM-yy with AM/PM
            string morning = "AM";
            string afternoon = "PM";
            await Locators.PlannedStartDate(page).ClickAsync();
            await Locators.PlannedStartDate(page).FillAsync($"{snShort} 08:00 {morning}");
            await Locators.PlannedEndDate(page).ClickAsync();
            await Locators.PlannedEndDate(page).FillAsync($"{snShort} 05:00 {afternoon}");
            await Locators.ActualStartDate(page).ClickAsync();
            await Locators.ActualStartDate(page).FillAsync($"{snShort} 08:00 {morning}");
            await Locators.ActualEndDate(page).ClickAsync();
            await Locators.ActualEndDate(page).FillAsync($"{snShort} 05:00 {afternoon}");

            // Optional for debugging only. Remove for parallel runs:
            await page.PauseAsync();
        }

        /// <summary>
        /// Launches the browser, validates required prefs, logs in once,
        /// then opens one tab per selected date and runs them all concurrently.
        /// </summary>
        public async Task LaunchBrowserAsync()
        {
            var prefs = PreferencesStorage.Load();
            string username = prefs.Username;
            string password = prefs.Password;
            string workItemNumber = prefs.WorkItemNumber;
            string notes = prefs.Notes;
            string fullName = prefs.FullName;
            var selectedDates = prefs.SelectedSprintDates; // List<DateTime>

            // --- Validation upfront ---
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please set your login details in the Login page before running the automation.",
                    "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(workItemNumber))
            {
                MessageBox.Show("Please set your work item number in the Triage Details page before running the automation.",
                    "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Please set your full name in the Triage Details page before running the automation.",
                    "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(notes))
            {
                MessageBox.Show("Please set your notes in the Triage Details page before running the automation.",
                    "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedDates == null || !selectedDates.Any())
            {
                MessageBox.Show("Please select and save at least one sprint date in the Date Selection page before running the automation.",
                    "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Prepare date strings in the exact format RunAutomationAsync expects ("dd-MMM-yy")
            var dateStrings = selectedDates
                .Select(d => d.ToString("dd-MMM-yy", CultureInfo.InvariantCulture))
                .ToList();

            // --- Browser/context setup ---
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true
            });

            // --- Login once on a bootstrap page so the session is shared across all new tabs ---
            var bootstrapPage = await context.NewPageAsync();

            await bootstrapPage.GotoAsync("https://hoopp.service-now.com/sp?id=index");

            // Login flow
            await SignInPageLocators.EmailField(bootstrapPage).ClickAsync();
            await SignInPageLocators.EmailField(bootstrapPage).FillAsync(username);
            await SignInPageLocators.NextButton(bootstrapPage).ClickAsync();
            await SignInPageLocators.PasswordField(bootstrapPage).ClickAsync();
            await SignInPageLocators.PasswordField(bootstrapPage).FillAsync(password);
            await SignInPageLocators.SignInButton(bootstrapPage).ClickAsync();

            // TODO: Ideally, add a small wait/verification that login succeeded.
            // e.g., wait for a known element that exists only after login.

            // --- Spawn one page per date and run concurrently ---
            var tasks = dateStrings.Select(async ds =>
            {
                var page = await context.NewPageAsync();

                try
                {
                    await RunAutomationAsync(page, ds);
                }
                catch (Exception ex)
                {
                    // Log or show a per-tab error if desired
                    Console.Error.WriteLine($"Automation failed for {ds}: {ex}");
                    // Optionally show a MessageBox once (UI thread permitting)
                    // Application.Current?.Dispatcher?.Invoke(() =>
                    //     MessageBox.Show($"Automation failed for {ds}: {ex.Message}", "Run Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    // );
                }
                finally
                {
                    // Close the tab to free resources
                    await page.CloseAsync();
                }
            }).ToList();

            await Task.WhenAll(tasks);

            // All done
            await browser.CloseAsync();
        }
    }
}