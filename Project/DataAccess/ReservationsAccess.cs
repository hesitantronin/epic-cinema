using System.Text.Json;

public static class ReservationsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/reservations.json"));

    public static List<ReservationsModel> LoadAll()
    {
        //Read the json into a string
        string json = File.ReadAllText(path); 

        // Return the JSON data as a list if "json" is not null, else return an empty list
        if(!string.IsNullOrEmpty(json))
            return JsonSerializer.Deserialize<List<ReservationsModel>>(json!)!;
        else
            return new List<ReservationsModel>();
    }

    public static void WriteAll(List<ReservationsModel> reservations)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(reservations, options);
        File.WriteAllText(path, json);
    }
}
