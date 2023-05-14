using System.Text.Json;

class CateringLogic
{
    private List<CateringModel> _menu = new();

    public CateringLogic()
    {
        // uses the loadall function to load the json to the list
        _menu = CateringAccess.LoadAll();
    }

    public void PrintMenu(List<CateringModel> FoodList)
    {
        Console.WriteLine("printing");
        if (FoodList.Count() == 0)
        {
            List<string> ReturnList = new List<string>();
            int option = OptionsMenu.DisplaySystem(ReturnList, "CATERING", "No catering items were found that matched the criteria.");
            if (option == 1)
            {
                Console.Clear();
                CateringMenu.Start();
            }
        }
        else
        {
            int option = OptionsMenu.CateringDisplaySystem(FoodList, "CATERING");
                        // depending on the option that was chosen, it will clear the console and call the right function     
            if (option == FoodList.Count() + 1)
            {
                Console.Clear();
                MovieMenu.Start();
            } 
            else
            {
                Console.Clear();
                CateringInfo(FoodList[option - 1]);
            }
        }
    }

    public void PrintMenu() => PrintMenu(_menu);
  
    public void CateringInfo(CateringModel foodItem)
    {
        OptionsMenu.Logo(foodItem.Name);

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Type");
        Console.ResetColor();
        Console.WriteLine($" {foodItem.Type}\n");

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Description");
        Console.ResetColor();
        Console.WriteLine($" {foodItem.Description}\n");

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Price");
        Console.ResetColor();
        Console.WriteLine($" ${foodItem.Price}\n");
    }

    public void UpdateList(CateringModel foodItem)
    {
        // finds if there is already a movie with the same id
        int index = _menu.FindIndex(s => s.Id == foodItem.Id);

        // if the index exists, itll update the movie, otherwhise itll add a new one
        if (index != -1)
        {
            // updates existing catering list
            _menu[index] = foodItem;
        }
        else
        {
            //adds new food
            _menu.Add(foodItem);
        }

        // writes the changed data to the json file
        CateringAccess.WriteAll(_menu);
    }

    public void RemoveCateringItem(int id)
    {
        // finds if there is a movie with the same id
        int index = _menu.FindIndex(s => s.Id == id);

        // removes the movie with that id, and updates the json file
        _menu.Remove(_menu[index]);
        CateringAccess.WriteAll(_menu);
    }

    
    public static void AddMultipleMenuItemsJSON(string filename)
    {
        // read file with new movies 
        string jsonstring = ReadJSON(filename);
        List<CateringModel> newMenuData = new();

        // 
        if(!string.IsNullOrEmpty(jsonstring))
        {
            newMenuData = JsonSerializer.Deserialize<List<CateringModel>>(jsonstring!)!;
        }

        // get the original movies that were already in the json file
        List<CateringModel> originalMenuData = CateringAccess.LoadAll();

        // add new movies to list containing old movies
        foreach(CateringModel newMenuItem in newMenuData)
        {
            originalMenuData.Add(newMenuItem);
        }

        // write new + old movies to file
        CateringAccess.WriteAll(originalMenuData);
    }

    public static string ReadJSON(string filename)
    {
        StreamReader? reader = null;

        try
        {
            // get new movie data from new json file
            reader = new StreamReader(filename);
            return(reader.ReadToEnd());
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found");
            return "";
        }
        finally
        {
            reader?.Close();
            reader?.Dispose();
        }
    }

    public static void AddMultipleMenuItemsCSV(string filename)
    {
        var csvMenu = new List<CateringModel>();
        var filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, filename));

        // read lines from csv file using the filepath
        // skip(1) is so that the header line will be skipped
        foreach (var line in File.ReadLines(filePath).Skip(1))
        {
            // create new MovieModels for each movie in the csv and add to csvMovies list
            csvMenu.Add(new CateringModel
            (
                Convert.ToInt32(line.Split(",")[0]), // id
                line.Split(",")[1], // name
                line.Split(",")[2], // type
                Convert.ToDouble(line.Split(",")[3]), // price
                line.Split(",")[4] // description
            ));
            
            foreach (CateringModel cm in csvMenu)
            {
                Console.WriteLine(cm.Name);
            }
            //CateringModel c = new CateringModel(4, "Chocolate", "Non-Vegan", 1.99, "Rich dark chocolate");
            // 4, Chocolate, Non-vegan, 1.99, Rich dark chocolate
        }

        // load movies that were already in the file
        List<CateringModel> originalMenu = CateringAccess.LoadAll();

        // add each new movie to the original list of movies
        foreach(CateringModel menuItem in csvMenu)
        {
            originalMenu.Add(menuItem);
        }

        // write original movies + new movies back to file
        CateringAccess.WriteAll(originalMenu);
    }
}