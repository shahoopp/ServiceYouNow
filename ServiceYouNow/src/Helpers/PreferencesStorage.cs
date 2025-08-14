using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ServiceYouNow.Models;

namespace ServiceYouNow.Helpers;

public static class PreferencesStorage
{
    private static readonly string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CondecoAssistant");

    private static readonly string filepath = Path.Combine(folderPath, "prefs.dat");

    public static void Save(UserPreferences prefs)
    {
        Directory.CreateDirectory(folderPath);

        string json = JsonSerializer.Serialize(prefs);
        byte[] data = Encoding.UTF8.GetBytes(json);
        byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

        File.WriteAllBytes(filepath, encrypted);
    }

    public static UserPreferences Load()
    {
        if (!File.Exists(filepath))
        {
            return new UserPreferences();
        }
        byte[] encrypted = File.ReadAllBytes(filepath);
        byte[] decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
        string json = Encoding.UTF8.GetString(decrypted);

        return JsonSerializer.Deserialize<UserPreferences>(json) ?? new UserPreferences();
    }
}