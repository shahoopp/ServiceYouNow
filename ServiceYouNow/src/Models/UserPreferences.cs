namespace ServiceYouNow.Models;

public class UserPreferences
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public List<PSGDatabaseAccessRequestData> SavedRequests { get; set; } = new();
}