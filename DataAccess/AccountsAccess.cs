using System.Text.Json;

static class AccountsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/accounts.json"));


    public static List<AccountModel> LoadAll()
    {
<<<<<<< HEAD
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AccountModel>>(json);
=======
        //Read the json into a string
        string json = File.ReadAllText(path); 

        // Return the JSON data as a list if "json" is not null, else return an empty list
        if(!string.IsNullOrEmpty(json)) 
            return JsonSerializer.Deserialize<List<AccountModel>>(json!)!;
        else
            return new List<AccountModel>();
>>>>>>> a2f6b86b1514c136bfa02313da4e1818232a84e8
    }


    public static void WriteAll(List<AccountModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }



}