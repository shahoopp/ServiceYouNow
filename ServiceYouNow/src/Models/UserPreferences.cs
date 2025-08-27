namespace ServiceYouNow.Models;

public class UserPreferences
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string WorkItemNumber { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Notes { get; set; } = "";
    public List<DateTime> SelectedSprintDates { get; set; } = new();
}