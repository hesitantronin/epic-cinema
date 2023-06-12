using System.Text.Json;

public class CateringAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/catering.json"));

    public static List<CateringModel> LoadAll()
    {
        //Read the json into a string
        string json = File.ReadAllText(path); 

        // Return the JSON data as a list if "json" is not null, else return an empty list
        if(!string.IsNullOrEmpty(json))
            return JsonSerializer.Deserialize<List<CateringModel>>(json!)!;
        else
            return new List<CateringModel>();
    }

    public static void WriteAll(List<CateringModel> menu)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(menu, options);
        File.WriteAllText(path, json);
    }
}
