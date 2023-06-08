using System.Text.Json;

public static class MovieAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/movies.json"));


    public static List<MovieModel> LoadAll()
    {
        //Read the json into a string
        string json = File.ReadAllText(path); 

        // Return the JSON data as a list if "json" is not null, else return an empty list
        if(!string.IsNullOrEmpty(json))
            return JsonSerializer.Deserialize<List<MovieModel>>(json!)!;
        else
            return new List<MovieModel>();
    }


    public static void WriteAll(List<MovieModel> movies)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(movies, options);
        File.WriteAllText(path, json);
    }

}