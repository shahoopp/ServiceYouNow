namespace ServiceYouNow.src.automation
{
    using Microsoft.Playwright;
    using ServiceYouNow.Helpers;
    using System.Threading.Tasks;

    public class Automation
    {
        public async Task LaunchBrowserAsync()
        {
            var prefs = PreferencesStorage.Load();
            string username = prefs.Username;
            string password = prefs.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username or password is not set.");
                return;
            }

            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            // Go to ServiceNow
            await page.GotoAsync("https://hoopp.service-now.com/sp?id=index");
            // Enter username
            await Locators.EmailField(page).ClickAsync();
            await Locators.EmailField(page).FillAsync(username);
            await Locators.NextButton(page).ClickAsync();
            // Enter password
            await Locators.PasswordField(page).ClickAsync();
            await Locators.PasswordField(page).FillAsync(password);
            await Locators.SignInButton(page).ClickAsync();
            // Test
            await page.PauseAsync();

            List<PSGDatabaseAccessRequestData> requests = prefs.SavedRequests;

            for (int i = 0; i < requests.Count; i++)
            {
                var request = prefs.SavedRequests[i];
                var psgRequest = new PSGDatabaseAccessRequest(
                    page,
                    request.Environment,
                    request.ServerName,
                    request.AccessLevel,
                    request.WorkItemNumber,
                    request.BusinessCase
                );
                await psgRequest.FillFormAsync();
            }

            await page.PauseAsync();
            await browser.CloseAsync();
        }
    }
}
