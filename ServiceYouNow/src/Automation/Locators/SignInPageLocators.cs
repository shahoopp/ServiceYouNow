using Microsoft.Playwright;
using System.Windows;
using ServiceYouNow.Helpers;
namespace ServiceYouNow.src.automation;

public static class SignInPageLocators
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

}