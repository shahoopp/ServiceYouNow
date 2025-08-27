namespace ServiceYouNow.src.automation
{
    using Microsoft.Playwright;
    using ServiceYouNow.Helpers;
    using System.Threading.Tasks;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Controls;
    using System.Windows;

    public class Automation
    {
        public async Task LaunchBrowserAsync()
        {
            var prefs = PreferencesStorage.Load();
            string username = prefs.Username;
            string password = prefs.Password;
            string workItemNumber = prefs.WorkItemNumber;
            string fullName = prefs.FullName;
            List<DateTime> selectedDates = prefs.SelectedSprintDates;
            // Check if the username or password is empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please set your login details in the Login page before running the automation.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // check if the work item number is empty

            // check if the full name is empty

            // check if the selected dates list is empty

            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true
            });
            var page = await context.NewPageAsync();
            // Go to ServiceNow
            await page.GotoAsync("https://hoopp.service-now.com/sp?id=index");
            // Login
            await SignInPageLocators.EmailField(page).ClickAsync();
            await SignInPageLocators.EmailField(page).FillAsync(username);
            await SignInPageLocators.NextButton(page).ClickAsync();
            await SignInPageLocators.PasswordField(page).ClickAsync();
            await SignInPageLocators.PasswordField(page).FillAsync(password);
            await SignInPageLocators.SignInButton(page).ClickAsync();

            // add code here that checks if login was successful using the username and password
            // if not, messagebox that the credentials were incorrect, and return

            await page.PauseAsync();
            // Head to the incidents page
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

            await SavedFilterLocators.ActionsButton(page).ClickAsync();
            await SavedFilterLocators.FiltersButton(page).ClickAsync();
            await SavedFilterLocators.AutomationFilterButton(page).ClickAsync();
            await Task.Delay(2000);
            await Locators.FilterButton(page).ClickAsync();
            await Locators.ChooseDateButton(page).ClickAsync();
            await Locators.DateButton(page, "Monday, August 25").ClickAsync();
            await Locators.RunFilterButton(page).ClickAsync();
            await Task.Delay(2000);
            await ExportLocators.NumberColumn(page).ClickAsync();
            await ExportLocators.ExportButton(page).ClickAsync();
            await ExportLocators.ExcelButton(page).ClickAsync();
            var downloadTask = page.WaitForDownloadAsync();
            await ExportLocators.DownloadButton(page).ClickAsync();
            var download = await downloadTask;
            string fileName = $"incidents_{DateTime.Now:MMM_dd_yyyy}.xlsx";
            var filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $@"..\..\..\{fileName}"));
            await download.SaveAsAsync(filePath);

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
            //await Locators.ChooseFileButton(page).ClickAsync();


            var fileInput = page
                .FrameLocator("iframe[name='gsft_main']")
                .Locator("input[type='file']");

            await fileInput.WaitForAsync(new() { State = WaitForSelectorState.Attached });
            await fileInput.SetInputFilesAsync(
Path.Combine(
    @"C:\Users\slone\Desktop\ServiceYouNow\ServiceYouNow",
    $"incidents_{DateTime.Now:MMM_dd_yyyy}.xlsx"
)
            );
            await Locators.DownloadAllButton(page).WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await Locators.CloseAttachmentsButton(page).ClickAsync();
            await Locators.IncidentState(page).ClickAsync();
            await Locators.IncidentState(page).SelectOptionAsync(new[] { "5" }); // Completed
            await Locators.IncidentState(page).ClickAsync(); // To click on it again so the dropdown goes away
            await Locators.DeploymentAssignedTo(page).ClickAsync();
            await Locators.DeploymentAssignedToClicked(page).FillAsync(fullName);
            await Locators.DevelopedBy(page).ClickAsync();
            await Locators.DevelopedBy(page).SelectOptionAsync(new[] { "hoopp internal" }); // HOOPP interal
            await Locators.DevelopmentArtifactLocation(page).ClickAsync();
            await Locators.DevelopmentArtifactLocation(page).FillAsync(workItemNumber);
            await Locators.PREDeploymentTestArtifact(page).ClickAsync();
            await Locators.PREDeploymentTestArtifact(page).FillAsync(workItemNumber);
            await Locators.POSTDeploymentTestArtifact(page).ClickAsync();
            await Locators.POSTDeploymentTestArtifact(page).FillAsync(workItemNumber);
            await Locators.DeploymentArtifactLocation(page).ClickAsync();
            await Locators.DeploymentArtifactLocation(page).FillAsync(workItemNumber);
            await Locators.ScheduleTab(page).ClickAsync();

            string morning = "AM";
            string afternoon = "PM";
            await Locators.PlannedStartDate(page).ClickAsync();
            await Locators.PlannedStartDate(page).FillAsync($"{DateTime.Now:dd-MMM-yy} 08:00 {morning}");
            await Locators.PlannedEndDate(page).ClickAsync();
            await Locators.PlannedEndDate(page).FillAsync($"{DateTime.Now:dd-MMM-yy} 05:00 {afternoon}");
            await Locators.ActualStartDate(page).ClickAsync();
            await Locators.ActualStartDate(page).FillAsync($"{DateTime.Now:dd-MMM-yy} 08:00 {morning}");
            await Locators.ActualEndDate(page).ClickAsync();
            await Locators.ActualEndDate(page).FillAsync($"{DateTime.Now:dd-MMM-yy} 05:00 {afternoon}");
            await page.PauseAsync();
            await browser.CloseAsync();
        }
    }
}
