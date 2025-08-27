using Microsoft.Playwright;
using System.Windows;
using ServiceYouNow.Helpers;
namespace ServiceYouNow.src.automation;

public static class Locators
{
    // ServiceNow Homepage

    public static ILocator SupportViewButton(IPage page) =>
         page.GetByRole(AriaRole.Link, new() { Name = "Support View" });

    public static ILocator IncidentButton(IPage page) =>
        page.GetByRole(AriaRole.Button, new() { Name = "Incident", Exact = true });

    public static ILocator AllIncidentsButton(IPage page) =>
        page.GetByRole(AriaRole.Link, new() { Name = "All" }).First;

    public static ILocator ChangeButton(IPage page) =>
        page.GetByRole(AriaRole.Button, new() { Name = "Change", Exact = true });

    public static ILocator StandardChangeButton(IPage page) =>
        page.GetByRole(AriaRole.Button, new() { Name = " Standard Change" });

    public static ILocator StandardChangeCatalogButton(IPage page) =>
        page.GetByRole(AriaRole.Link, new() { Name = "Standard Change Catalog" });

    public static ILocator PSGPension(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Link, new() { Name = "PSG-Pension" });

    public static ILocator NextPageButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .Locator("#nav_nextTop");

    public static ILocator IncidentWorkaroundsAndResolutions(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Link, new() { Name = "Incident Workarounds &" });

    public static ILocator ManageAttachments(IPage page) =>
    page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Button, new() { Name = "Manage Attachments" });

    public static ILocator DownloadAllButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Button, new() { Name = "Download All" });

    public static ILocator CloseAttachmentsButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Button, new() { Name = "Close", Exact = true });

    public static ILocator IncidentState(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByLabel("State", new() { Exact = true });
    ////////////////////////////

    public static ILocator DeploymentAssignedTo(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Searchbox, new() { Name = "Mandatory - must be populated" });

    public static ILocator DeploymentAssignedToClicked(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Combobox, new() { Name = "Mandatory - must be populated" });

    public static ILocator DevelopedBy(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByLabel("Developed by");

    public static ILocator DevelopmentArtifactLocation(IPage page) =>
                page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Mandatory - must be populated before SubmitDevelopment Artifact Location (" });

    public static ILocator PREDeploymentTestArtifact(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Mandatory - must be populated before SubmitPRE Deployment Test Artifact Results" });

    public static ILocator POSTDeploymentTestArtifact(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "POST Deployment Test Artifact" });

    public static ILocator DeploymentArtifactLocation(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Mandatory - must be populated before SubmitDeployment Artifact Location (ID)" });

    public static ILocator ScheduleTab(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Tab, new() { Name = "Schedule" });

    public static ILocator PlannedStartDate(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Planned start date" });

    public static ILocator PlannedEndDate(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Planned end date" });

    public static ILocator ActualStartDate(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Actual start" });

    public static ILocator ActualEndDate(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Actual end" });





    // Incidents page

    public static ILocator FilterButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Button, new() { Name = " Show / hide filter" });

    public static ILocator ChooseFieldDropdown(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .Locator("a")
            .Filter(new() { HasText = "-- choose field --" });

    public static ILocator ChooseFieldOption(IPage page, string fieldText)
    {
        return page
            .FrameLocator("iframe[name=\"gsft_main\"]")
            .Locator("[id^='select2-result-label-']")
            .GetByText(fieldText, new() { Exact = true });
    }

    // Condition 1

    public static ILocator AssignmentGroupOption(IPage page) =>
    page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByLabel("select2-results")
        .GetByRole(AriaRole.Option, new() { Name = "Assignment group" });

    public static ILocator AssignmentGroupField(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Textbox, new() { Name = "Assignment group" });

    public static ILocator AssignmentGroupFieldClicked(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Combobox, new() { Name = "Assignment group", Exact = true });

    public static ILocator FirstANDButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Button, new() { Name = "Add AND Condition To" });

    // Condition 2

    public static ILocator ChooseStatusDropdown(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByLabel("Choose option for field:");

    public static ILocator SecondANDButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Button, new() { Name = "Add AND Condition To Condition 2: Status is Resolved" });

    // Conditon 3

    public static ILocator ChooseDateButton(IPage page) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
            .GetByRole(AriaRole.Button, new() { Name = "Choose date..." });

    public static ILocator DateButton(IPage page, string dateLabel) =>
        page.FrameLocator("iframe[name=\"gsft_main\"]")
                   .GetByRole(AriaRole.Button, new() { Name = dateLabel });


    public static ILocator RunFilterButton(IPage page) =>
    page.FrameLocator("iframe[name=\"gsft_main\"]")
        .GetByRole(AriaRole.Button, new() { Name = "Run filter" });








}