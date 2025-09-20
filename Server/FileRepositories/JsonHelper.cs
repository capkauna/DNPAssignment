using System.Text.Json;

namespace FileRepositories;

public class JsonHelper
{
    private static readonly JsonSerializerOptions Options = new()
    //rulebook for reading and writing the json file (case insensitive)
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
        //easier read when true, but slower to write
    };
    public static void EnsureInitialized(string path)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "[]");
            return;
        }
        // If file exists but is empty, also initialize
        var info = new FileInfo(path);
        if (info.Length == 0)
        {
            File.WriteAllText(path, "[]");
        }
    }

    public static List<T> LoadList<T>(string path)
    {
        EnsureInitialized(path);
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
    }

    public static async Task<List<T>> LoadListAsync<T>(string path)
    {
        EnsureInitialized(path);
        string json = await File.ReadAllTextAsync(path);
        var list = JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        return list;
    }

    public static async Task SaveListAsync<T>(string path, List<T> list)
    {
        // Serialize first to avoid holding the lock longer than needed
        string json = JsonSerializer.Serialize(list, Options);
      // atomic-ish: write to temp then replace
            string temp = path + ".tmp";
            await File.WriteAllTextAsync(temp, json);
            File.Copy(temp, path, overwrite: true);
            File.Delete(temp);
    }

}