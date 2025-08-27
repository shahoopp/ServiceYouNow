using Microsoft.Playwright;
using System.Windows;
using ServiceYouNow.Helpers;
namespace ServiceYouNow.src.automation;

public static class SavedFilterLocators
{
    public static ILocator ActionsButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Button, new() { Name = "Actions" });

    public static ILocator FiltersButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Menuitem, new() { Name = "Filters " });

    public static ILocator AutomationFilterButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Menuitem, new() { Name = "AUTOMATION_FILTER" });


}