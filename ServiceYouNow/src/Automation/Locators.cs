using Microsoft.Playwright;
using System.Windows;
using ServiceYouNow.Helpers;
namespace ServiceYouNow.src.automation;

public static class Locators
{
    // HOOPP sign in
    public static ILocator EmailField(IPage page) =>
        page.GetByRole(AriaRole.Textbox, new() { Name = "someone@example.com" });

    public static ILocator NextButton(IPage page) =>
        page.GetByRole(AriaRole.Button, new() { Name = "Next" });

    public static ILocator PasswordField(IPage page)
    {
        var prefs = PreferencesStorage.Load();
        string email = prefs.Username ?? "";
        string namePart = email.Contains('@') ? email.Split('@')[0] : email;
        string fieldLabel = $"Enter the password for {namePart}@";
        return page.GetByRole(AriaRole.Textbox, new() { Name = fieldLabel });
    }

    public static ILocator SignInButton(IPage page) =>
    page.GetByRole(AriaRole.Button, new() { Name = "Sign in" });

    // ServiceNow Homepage

    public static ILocator Searchbar(IPage page) =>
        page.GetByRole(AriaRole.Textbox, new() { Name = "How can we help?" });

    public static ILocator SearchButton(IPage page) =>
        page.GetByRole(AriaRole.Button, new() { Name = "Search" });

    // PSG Database Access Request Process

    public static ILocator PSGDatabaseAccessRequest(IPage page) =>
        page.GetByRole(AriaRole.Link, new() { Name = " PSG Database Access Request" });

    public static ILocator ActionDropdown(IPage page) =>
        page.Locator("#s2id_sp_formfield_action a");

    public static ILocator ActionDropdownOption_AddTemporary(IPage page) =>
        page.GetByRole(AriaRole.Option, new() { Name = "Add Temporary" });

    public static ILocator EnvironmentDropdown(IPage page) =>
        page.GetByRole(AriaRole.Link, new() { Name = "Lookup using list" });

    public static ILocator EnvironmentDropdownOption(IPage page, string environmentName) =>
        page.GetByRole(AriaRole.Option, new() { Name = environmentName });

    public static ILocator EnvironmentServerDropdown(IPage page) =>
        page.GetByRole(AriaRole.Link, new() { Name = "Lookup using list" });

    public static ILocator EnvironmentServerDropdownOption(IPage page, string serverName) =>
        page.GetByRole(AriaRole.Option, new() { Name = serverName, Exact = true });

    public static ILocator AccessStartDate(IPage page) =>
        page.GetByRole(AriaRole.Textbox, new() { Name = "Access Start Date" });

    public static ILocator AccessEndDate(IPage page) =>
        page.GetByRole(AriaRole.Textbox, new() { Name = "Access End Date" });

    public static ILocator AccessDropdown(IPage page) =>
    page.Locator("#s2id_sp_formfield_access a");

    public static ILocator AccessDropdownOption(IPage page, string accessLevel) =>
        page.GetByRole(AriaRole.Option, new() { Name = accessLevel, Exact = true });

    public static ILocator AssociatedWorkItemNumber(IPage page) =>
        page.GetByRole(AriaRole.Textbox, new() { Name = "Associated Work Item #" });

    public static ILocator BusinessCase(IPage page) =>
        page.GetByRole(AriaRole.Textbox, new() { Name = "Business Case (Do not specify" });

    public static ILocator OrderNow(IPage page) =>
        page.GetByRole(AriaRole.Button, new() { Name = "Order Now" });

    public static ILocator HomeButton(IPage page) =>
        page.GetByRole(AriaRole.Link, new() { Name = "Home", Exact = true });

}