using Microsoft.Playwright;
using System.Windows;
using ServiceYouNow.Helpers;
namespace ServiceYouNow.src.automation;

public static class ExportLocators
{
    public static ILocator NumberColumn(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Button, new() { Name = "Number column options" });

    public static ILocator ExportButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Menuitem, new() { Name = "Export " });

    public static ILocator ExcelButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Menuitem, new() { Name = "Excel (.xlsx)" });

    public static ILocator DownloadButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Button, new() { Name = "Download" });

}