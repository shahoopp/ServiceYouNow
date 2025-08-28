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
        /// 
        public bool IsRunComplete { get; private set; }

        public volatile bool ProceedToNext;

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

            if (await Locators.StandardChangeButton(page).IsVisibleAsync())
            {
                await Locators.ChangeButton(page).ClickAsync();
            }
            await Task.Delay(2000);
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
            await Task.Delay(2000);
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
            string fileName = $"Incidents_{fileStamp}.xlsx";
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
            await Task.Delay(2000);

            await Locators.PSGPension(page).ClickAsync();
            await Locators.NextPageButton(page).ClickAsync();
            await Locators.IncidentWorkaroundsAndResolutions(page).ClickAsync();
            await Locators.ManageAttachments(page).ClickAsync();

            // Upload the exact file we just saved
            var fileInput = page
                .FrameLocator("iframe[name='gsft_main']")
                .Locator("input[type='file']");
            await Task.Delay(2000);
            await fileInput.WaitForAsync(new() { State = WaitForSelectorState.Attached });
            await fileInput.SetInputFilesAsync(filePath);

            await Locators.DownloadAllButton(page).WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await Locators.CloseAttachmentsButton(page).ClickAsync();

            // Form fills (unchanged except using runDate where dates are used)
            await Locators.IncidentState(page).ClickAsync();
            await Locators.IncidentState(page).SelectOptionAsync(new[] { "5" }); // Completed
            await Locators.IncidentState(page).ClickAsync(); // collapse dropdown

            var prefs = PreferencesStorage.Load();
            string workItemNumber = prefs.WorkItemNumber;
            string fullName = prefs.FullName;
            string notes = prefs.Notes;
            await Locators.DeploymentAssignedTo(page).ClickAsync();
            await Locators.DeploymentAssignedToClicked(page).FillAsync(fullName);
            await Locators.DeploymentAssignedToClicked(page).PressAsync("Enter");

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

            await Locators.NotesTab(page).ClickAsync();
            await Locators.CommentsField(page).ClickAsync();
            await Locators.CommentsField(page).FillAsync(notes);

            IsRunComplete = true;
            ProceedToNext = true;
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

            // ----- Validation (unchanged) -----
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

            var dateStrings = selectedDates
                .Select(d => d.ToString("dd-MMM-yy", CultureInfo.InvariantCulture))
                .ToList();

            // ----- One browser, one context, many tabs (sequential) -----
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            // Single window/context so all tabs appear in the same window
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true,
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
                ScreenSize = new ScreenSize { Width = 1920, Height = 1080 }
            });

            // ---- Login once in the SAME context (keep this tab open) ----
            var loginPage = await context.NewPageAsync();
            await loginPage.GotoAsync("https://hoopp.service-now.com/sp?id=index");

            await SignInPageLocators.EmailField(loginPage).ClickAsync();
            await SignInPageLocators.EmailField(loginPage).FillAsync(username);
            await SignInPageLocators.NextButton(loginPage).ClickAsync();
            await SignInPageLocators.PasswordField(loginPage).ClickAsync();
            await SignInPageLocators.PasswordField(loginPage).FillAsync(password);
            await SignInPageLocators.SignInButton(loginPage).ClickAsync();

            await loginPage.WaitForLoadStateAsync(LoadState.NetworkIdle);
            // Do not close this page; it remains as the first tab.

            // ---- Create one NEW TAB per date, sequentially; never close tabs ----
            foreach (var ds in dateStrings)
            {
                // New TAB (page) in the SAME context/window
                var page = await context.NewPageAsync();
                await page.BringToFrontAsync(); // surface the active tab

                // Reset the flag before starting this run
                ProceedToNext = false;

                // Kick off the run; DO NOT await if you plan to set the flag before a Pause
                // If your RunAutomationAsync completes fully and does not Pause, you can 'await' instead.
                var runTask = RunAutomationAsync(page, ds);

                // Log unhandled exceptions from the run without blocking the loop
                _ = runTask.ContinueWith(t =>
                {
                    if (t.IsFaulted && t.Exception != null)
                    {
                        Console.Error.WriteLine($"Automation failed for {ds}: {t.Exception.Flatten()}");
                    }
                }, TaskScheduler.Default);

                // Wait until RunAutomationAsync signals it's safe to proceed to the next tab.
                // If you keep 'await page.PauseAsync()' inside RunAutomationAsync, set the flag BEFORE the Pause.
                await WaitUntilAsync(() => ProceedToNext, timeout: TimeSpan.FromMinutes(15), pollMs: 200);

                // Proceed to the next date; do NOT close 'page' — we leave the tab open.
            }

            MessageBox.Show("All dates processed. Tabs remain open for review." +
                "\nDO NOT CLOSE THIS MESSAGE OR CLICK OK UNTIL REVIEW IS COMPLETE.",
                "Automation Complete", MessageBoxButton.OK, MessageBoxImage.Information);

            // Helper: local wait method that polls the boolean flag
            static async Task WaitUntilAsync(Func<bool> predicate, TimeSpan timeout, int pollMs)
            {
                var start = DateTime.UtcNow;
                while (!predicate())
                {
                    if (DateTime.UtcNow - start > timeout)
                        throw new TimeoutException($"Timed out waiting for condition after {timeout}.");
                    await Task.Delay(pollMs);
                }
            }

            // Note: Intentionally NOT closing the browser/context/pages so you can inspect all tabs.
        }


    }
}