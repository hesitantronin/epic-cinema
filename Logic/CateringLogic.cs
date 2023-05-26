using System.Text.Json;
using System.Globalization;

class CateringLogic
{
    private List<CateringModel> _menu = new();

    public CateringLogic()
    {
        // uses the loadall function to load the json to the list
        LoadCatering();
    }
    private void LoadCatering()
    {
        _menu = CateringAccess.LoadAll();
    }
    public CateringModel? GetById(int id)
    {
        // returns the movie data that matches the id
        return _menu.Find(i => i.Id == id);
    }
    public void PrintMenu(List<CateringModel> FoodList, bool IsEmployee = false, bool IsEdit = false)
    {
        while (true)
        {
            Console.Clear();

            // prints an error message if nothing was found
            if (FoodList.Count() == 0)
            {
                // list of options that will be displayed
                List<string> ReturnList = new List<string>();

                // the necessary info gets used in the display method
                int option = OptionsMenu.DisplaySystem(ReturnList, "Catering", "No catering items were found that matched the criteria.");

                // depending on the option that was chosen, it will clear the console and call the right function
                if (option == 1)
                {
                    break;
                }
            }
            else
            {
                int BaseLine = 0;
                int MaxItems = 5;
                int pageNr = 1;

                bool previousButton = false;
                bool nextButton = true;

                while (BaseLine < FoodList.Count())
                {
                    if (BaseLine + 5 > FoodList.Count())
                    {
                        MaxItems = FoodList.Count() % 5;
                        nextButton = false;
                    }
                    else
                    {
                        MaxItems = 5;
                        nextButton = true;
                    }
                    
                    if (BaseLine < 0)
                    {
                        BaseLine = 0;
                        pageNr = 0;
                    }

                    if (BaseLine != 0)
                    {
                        previousButton = true;
                    }
                    else
                    {
                        previousButton = false;
                    }

                    // the necessary info gets used in the display method
                    List<CateringModel> subList = FoodList.GetRange(BaseLine, MaxItems);

                    int option = OptionsMenu.CateringDisplaySystem(subList, "Catering", $"Page {pageNr}", true, previousButton, nextButton);

                    // depending on the option that was chosen, it will clear the console and call the right function  
                    if ((option == subList.Count() + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton) && previousButton && !nextButton) || (option == subList.Count() + 1 && previousButton && nextButton))
                    {
                        BaseLine -= 5;
                        pageNr -= 1;
                    }
                    else if ((option == subList.Count() + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton) && nextButton))
                    {
                        BaseLine += 5;
                        pageNr += 1;
                    }
                    else if (option == subList.Count() + 1 + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton))
                    {
                        return;
                    }
                    else
                    {
                        if (IsEmployee && IsEdit)
                        {
                            continue;
                        }
                        else if (IsEmployee && !IsEdit)
                        {
                            LoadCatering();
                            EditCatering(subList[option - 1]);
                            return;
                        }
                        else
                        {
                            Console.Clear();
                            CateringInfo(subList[option - 1]);
                        }
                    }
                }
            }
        }
    }

    public void EditCatering(CateringModel foodItem)
    {
        bool remove = false;

        while (true)
        {
            Console.Clear();

            // shows the banner and title
            OptionsMenu.Logo(foodItem.Name);
            List<string> foodInfoList = new List<string>();
            foodInfoList.Add($"Name: {foodItem.Name}");
            foodInfoList.Add($"Type: {foodItem.Type}");
            foodInfoList.Add($"Price: {foodItem.Price}");
            foodInfoList.Add($"Description: {MovieLogic.SpliceText(foodItem.Description, "   ")}\n");
            foodInfoList.Add("\u001b[31mRemove Item\u001b[0m");
            int edit = OptionsMenu.DisplaySystem(foodInfoList, "Edit food item", "Select which field to edit, or choose to delete this item.", true, true);
            if (edit == 1)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit food item");

                    Console.Write("New name: ");
                    string newName = Console.ReadLine() + "";
                    if (!string.IsNullOrEmpty(newName))
                    {
                        foodItem.Name = newName;
                        break;
                    }
                    Console.WriteLine("\nThe name can't be empty.");
                    
                    // prints a fake return option hehe
                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                
                    // actually returns you to the main menu
                    Console.ReadLine();
                }

                break;
            }
            else if (edit == 2)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit food item");

                    Console.Write("New type: ");
                    string newType = Console.ReadLine() + "";
                    if (!string.IsNullOrEmpty(newType))
                    {
                        foodItem.Type = newType;
                        break;
                    }
                    Console.WriteLine("\nThe type can't be empty.");
                    
                    // prints a fake return option hehe
                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                
                    // actually returns you to the main menu
                    Console.ReadLine();
                }

                break;
            }
            else if (edit == 3)
            {
                double price;
                while (true)
                {
                    OptionsMenu.Logo("edit food item");

                    Console.Write($"New price (old price was {foodItem.Price}): ");
                    string priceInput = Console.ReadLine() + "";

                    if (double.TryParse(priceInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
                    {
                        foodItem.Price = price;
                        break;
                    }

                    Console.WriteLine("Invalid price. Please enter a valid decimal number.");
                    
                    // prints a fake return option hehe
                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                
                    // actually returns you to the main menu
                    Console.ReadLine();
                }
                break;
            }
            else if (edit == 4)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit food item");

                    Console.Write("New description: ");
                    string newDescription = Console.ReadLine() + "";
                    if (!string.IsNullOrEmpty(newDescription))
                    {
                        foodItem.Description = newDescription;
                        break;
                    }
                    Console.WriteLine("\nThe description can't be empty.");
                    
                    // prints a fake return option hehe
                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                
                    // actually returns you to the main menu
                    Console.ReadLine();
                }

                break;
            }
            else if (edit == 5)
            {
                int removeOptions = OptionsMenu.DisplaySystem(YN, "confirm", "Are you sure you want to delete this item?", true, false);
                if (removeOptions == 1)
                {
                    remove = true;
                    break;
                }
            }
            else
            {
                return;
            }
        }
        
        // Find the index of the movie to update
        int index = _menu.FindIndex(c => c.Id == foodItem.Id);

        if (!remove)
        {
            Console.CursorVisible = false;

            // Update the movie in the list
            _menu[index] = foodItem;

            // Write the updated movie list to the JSON file
            CateringAccess.WriteAll(_menu);
            
            OptionsMenu.Logo("menu updated");
            Console.WriteLine("Menu updated successfully.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();

            Console.CursorVisible = true;
        }
        else
        {
            Console.CursorVisible = false;

            _menu.Remove(_menu[index]);

            CateringAccess.WriteAll(_menu);
            
            OptionsMenu.Logo("item deleted");
            Console.WriteLine("Item deleted successfully.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();

            Console.CursorVisible = true;
        }
    }
    public void PrintMenu() => PrintMenu(_menu);
  
    public void CateringInfo(CateringModel foodItem)
    {
        OptionsMenu.Logo(foodItem.Name);

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Type");
        Console.ResetColor();
        Console.WriteLine($" {foodItem.Type}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Description");
        Console.ResetColor();
        Console.WriteLine($" {foodItem.Description}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Price");
        Console.ResetColor();
        Console.WriteLine($" ${foodItem.Price}\n");

        List<string> ReturnList = new List<string>()
        {
            "Yes",
            "No"
        };

        int option = OptionsMenu.DisplaySystem(ReturnList, "", "\nDo you want to reserve this menu item?", false, false);

        switch (option)
        {
            case 1: // Customer wants to reserve selected item
                Console.WriteLine("Please enter the amount that you would like to reserve: ");
                string? amount = Console.ReadLine();
                
                if (amount != null && amount.All(Char.IsDigit)) // check if entered amount is a valid number (0-9)
                {
                    int numAmount = Convert.ToInt32(amount);
        
                    if (AccountsLogic.CurrentAccount != null)
                    {
                            // takes the dictionary bound to the current account that contains its previous menu reservations & adds onto it
                            Dictionary<string, string> TotalReservations = AccountsLogic.CurrentAccount.CateringReservation;

                            // check whether or not the customer has already reserved this item. If yes, add to the previously entered amount, otherwise add a new item & amount to their cateringReservations dictionary
                            if (TotalReservations.ContainsKey(foodItem.Name))
                            {
                                int newAmount = Convert.ToInt32(TotalReservations[foodItem.Name]) + numAmount;
                                TotalReservations[foodItem.Name] = Convert.ToString(newAmount);
                            }
                            else
                            {
                            TotalReservations.Add(foodItem.Name, amount);
                            }
        
                            // updates the json file so that the dictionary has the new/updated menu item
                            AccountsLogic accountslogic = new AccountsLogic();
                            AccountsLogic.CurrentAccount.CateringReservation = TotalReservations;
                            accountslogic.UpdateList(AccountsLogic.CurrentAccount);
                    }
                }
                else // if the entered number is not a valid number (0-9)
                {
                    List<string> EmptyReturnList = new List<string>() {};
                    int option3 = OptionsMenu.DisplaySystem(EmptyReturnList, "", "Please enter a valid number (ex. 5)");
                    
                    if (option3 == 1)
                    {
                        break; // go back to the start of the catering menu
                    }
                }
                break;
    
            case 2: // Customer does not want to reserve selected item
                break;
        }

        // show all currently reserved menu items and asks whether the customer would like to reserve more
        string menuReservations = "";
        foreach (var item in AccountsLogic.CurrentAccount.CateringReservation)
        {
            menuReservations += $"{item.Key}        Amount: {item.Value}\n";
        }

        int option2 = OptionsMenu.DisplaySystem(ReturnList, "", $"You've selected these menu items:\n{menuReservations}\nIs this all you want to reserve?", true, false);


        switch(option2)
        {
            case 1: // don't reserve more
                Console.Clear();
                ReservationMenu.Start();
                break;

            case 2: // reserve more
                Console.Clear();
                CateringMenu.Start();
                break;
        }
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

    public void RemoveCateringID()
    {
        int removeID;

        do
        {
            Console.Clear();
            Console.WriteLine("Please enter the food item ID you would like to remove.");
            if (!int.TryParse(Console.ReadLine(), out removeID))
            {
                Console.WriteLine("Invalid ID. Please enter a valid integer ID.");
                continue;
            }

            // Find the index of the movie with the specified ID
            int index = _menu.FindIndex(s => s.Id == removeID);

            if (index != -1)
            {
                // Remove the movie from the list
                _menu.RemoveAt(index);

                // Update the JSON file
                CateringAccess.WriteAll(_menu);

                Console.WriteLine("Food item removed successfully.\n\nPress enter to continue.");
                Console.ReadLine();
                break;
            }
            else
            {
                List<string> EList = new List<string>() { "Continue" };
                int option = OptionsMenu.DisplaySystem(EList, "", $"\nID {removeID} not found, make sure to enter a valid ID", false, true);
                if (option == 2)
                {
                    return;
                }
            }
        } while (true);
    }


    public static void AddMultiplefoodJSON()
    {
        string filename;
        do
        {
            OptionsMenu.Logo("json item adding");
            Console.WriteLine("Please save the JSON file in the DataSources folder.");
            Console.WriteLine("Enter the JSON file name without '.json': ");
            string jsonFile = Console.ReadLine() + "";
            filename = @$"DataSources\{jsonFile}.json";

            if (!File.Exists(filename))
            {
                List<string> EList = new List<string>() { "Continue" };
                int option = OptionsMenu.DisplaySystem(EList, "file not found", $"File '{jsonFile}.json' not found in directory: {filename}, make sure the file is saved in DataSources and the input is without '.json'", true, true);
                if (option == 2)
                {
                    return;
                }
            }
        } while (!File.Exists(filename));

        // Read file with new food items
        string jsonstring = ReadJSON(filename);
        List<CateringModel> newFoodData = new();

        if (!string.IsNullOrEmpty(jsonstring))
        {
            newFoodData = JsonSerializer.Deserialize<List<CateringModel>>(jsonstring)!;
        }

        // Get the original menu that were already in the JSON file
        List<CateringModel> originalFoodData = CateringAccess.LoadAll();

        // Get the maximum ID from the original menu
        int maxId = originalFoodData.Max(food => food.Id);

        // Track already existing dishes
        List<CateringModel> existingFoods = new List<CateringModel>();

        // Check if the new dishes already exist in the original data
        foreach (CateringModel newFood in newFoodData)
        {
            bool foodExists = originalFoodData.Any(food => food.Name == newFood.Name && food.Type == newFood.Type && food.Price == newFood.Price);
            if (!foodExists)
            {
                // Increment the ID for each new movie to ensure uniqueness
                newFood.Id = ++maxId;
                originalFoodData.Add(newFood);
            }
            else
            {
                existingFoods.Add(newFood);
            }
        }

        // Write new + old movies to file
        CateringAccess.WriteAll(originalFoodData);

        // Display existing movies message
        if (existingFoods.Count > 0)
        {
            OptionsMenu.Logo("double items");
            Console.WriteLine("The following food items already exist and were not added:\n");
            foreach (CateringModel existingFood in existingFoods)
            {
                Console.WriteLine($"- Food ID: {existingFood.Id}\nName: {existingFood.Name}\nType: {existingFood.Type}\nPrice: {existingFood.Price}\n");
            }
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine(); 
        }
        else
        {
            OptionsMenu.Logo("Items added");
            Console.WriteLine("Food items have been succesfully added.");
           
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();         
        }
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

    public static void AddMultipleFoodCSV()
    {
        string filename;
        do
        {
            OptionsMenu.Logo("CSV item adding");
            Console.WriteLine("Please save the CSV file in the DataSources folder.");
            Console.WriteLine("Enter the CSV file name without '.csv': ");
            string csvFile = Console.ReadLine() + "";
            filename = @$"DataSources\{csvFile}.csv";

            if (!File.Exists(filename))
            {
                Console.Clear();
                List<string> EList = new List<string>() { "Continue" };
                int option = OptionsMenu.DisplaySystem(EList, "file not found", $"File '{csvFile}.csv' not found in directory: {filename}, make sure the file is saved in DataSources and the input is without '.csv'");
                if (option == 2)
                {
                    return;
                }
            }
        } while (!File.Exists(filename));

        var csvMenu = new List<CateringModel>();
        var filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, filename));


        // Read lines from the CSV file using the file path
        // Skip(1) is used to skip the header line
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
        }

        // Load movies that were already in the file
        List<CateringModel> originalFoods = CateringAccess.LoadAll();

        // Get the maximum ID from the existing movies
        int maxId = originalFoods.Max(food => food.Id);

        // Track already existing movies
        List<CateringModel> existingFoods = new List<CateringModel>();

        // Check if the new movies already exist in the original data
        foreach (CateringModel food in csvMenu)
        {
            bool foodExists = originalFoods.Any(f => f.Name == food.Name && f.Type == food.Type && f.Description == food.Description && f.Price == food.Price);
            if (!foodExists)
            {
                // Increment the ID for each new movie to ensure uniqueness
                food.Id = ++maxId;
                originalFoods.Add(food);
            }
            else
            {
                existingFoods.Add(food);
            }
        }

        // Write original movies + new movies back to file
        CateringAccess.WriteAll(originalFoods);

        // Display existing movies message
        if (existingFoods.Count > 0)
        {
            OptionsMenu.Logo("double items");

            Console.WriteLine("The following food items already exist and were not added:\n");
            foreach (CateringModel existingFood in existingFoods)
            {
                Console.WriteLine($"- Food ID: {existingFood.Id}\nName: {existingFood.Name}\nType: {existingFood.Type}\nPrice: {existingFood.Price}");
            }
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();  
        }
        else
        {
            OptionsMenu.Logo("Items added");

            Console.WriteLine("Food items have been succesfully added.\n");
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();          
        }
    }
    public List<CateringModel> SortBy(string input, bool ascending)
    {
        List<CateringModel> unsorted = _menu;

        // depending on if its ascending or not, sorts _menu based on input (name, type, price) in ascending or descending
        if (input.ToUpper() == "NAME")
        {
            return (ascending) ? unsorted.OrderBy(c => c.Name).ToList() : unsorted.OrderByDescending(c => c.Name).ToList();
        }
        else if (input.ToUpper() == "TYPE")
        {
            return (ascending) ? unsorted.OrderBy(c => c.Type).ToList() : unsorted.OrderByDescending(c => c.Type).ToList();
        }
        else if (input.ToUpper() == "PRICE")
        {
            return (ascending) ? unsorted.OrderBy(c => c.Price).ToList() : unsorted.OrderByDescending(c => c.Price).ToList();
        }
        else
        {
            return unsorted;
        }
    }

    public List<CateringModel> FilterBy(string type)
    {
        List<CateringModel> filtered = _menu;

        if (type != null)
        {
            filtered = _menu.Where(c => c.Type == type).ToList();
        }

        return filtered;
    }

    public List<CateringModel> SearchBy(string query)
    {
        List<CateringModel> searched = new();
        foreach(CateringModel menuItem in _menu)
        {
            if (menuItem.Name.ToLower().Contains(query.ToLower()) || menuItem.Description.ToLower().Contains(query.ToLower()))
            {
                searched.Add(menuItem);
            }
        }

        return searched;
    }

    public static void AddOrUpdateFood()
    {
        List<CateringModel> foods = CateringAccess.LoadAll();
        CateringModel? existingCatering = null;

        string name;
        while (true)
        {
            OptionsMenu.Logo("add food item");
            Console.WriteLine("Enter the food details.");

            Console.Write("Name: ");
            name = Console.ReadLine();

            existingCatering = foods.FirstOrDefault(food => food.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existingCatering != null)
            {
                int YNQ = OptionsMenu.DisplaySystem(YN, "Food item already exists", $"Food item with the name '{name}' already exists, do you want to update the details instead?", true, false);
                if (YNQ == 2)
                {
                    OptionsMenu.Logo("Canceled");
                    Console.WriteLine("Update operation cancelled.");

                    // prints a fake return option hehe
                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                
                    // actually returns you to the main menu
                    Console.ReadLine();
                    return;
                }
            }
            if (name != "")
            {
                break;
            }
            
            Console.WriteLine("\nThe name can't be empty.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();
        }

        string type;
        while (true)
        {
            OptionsMenu.Logo("add food item");
            Console.WriteLine("Enter the food details.");

            Console.Write("Type: ");
            type = Console.ReadLine();
            
            if (type != "")
            {
                break;
            }

            Console.WriteLine("\nThe type can't be empty.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();
        }

        double price;
        while (true)
        {
            OptionsMenu.Logo("add food item");
            Console.WriteLine("Enter the food details.");

            Console.Write("Price: ");
            string priceInput = Console.ReadLine() + "";

            if (double.TryParse(priceInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
            {
                break;
            }

            Console.WriteLine("\nInvalid Price. Please enter a valid decimal number.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();
        }

        string description;
        while (true)
        {
            OptionsMenu.Logo("add food item");
            Console.WriteLine("Enter the food details.");

            Console.Write("Description: ");
            description = Console.ReadLine() + "";

            if (description != "")
            {
                break;
            }

            Console.WriteLine("\nThe description can't be empty.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();
        }

        int maxId = foods.Count > 0 ? foods.Max(food => food.Id) : 0;

        if (existingCatering != null)
        {
            // Update the existing movie
            existingCatering.Name = name;
            existingCatering.Type = type;
            existingCatering.Price = price;
            existingCatering.Description = description;

            OptionsMenu.Logo("menu updated");
            Console.WriteLine("Menu updated successfully!");

            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();        
        }
        else
        {
            // Create a new movie with a unique ID
            CateringModel newFood = new CateringModel(++maxId, name, type, price, description);
            foods.Add(newFood);

            OptionsMenu.Logo("Item added");
            Console.WriteLine("Food item added successfully!");

            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();
        }
        // Save the updated movies list
        CateringAccess.WriteAll(foods);
    }

    // THE FUNCTION BELOW IS NO LONGER NEEDED
    // private static void RemoveFood()
    // {
    //     List<CateringModel> foods = CateringAccess.LoadAll();

    //     // Retrieve and display the movies
    //     List<string> foodList = new List<string>();
    //     foreach (CateringModel food in foods)
    //     {
    //         string FoodInfo = $"ID: {food.Id}\nName: {food.Name}\nType: {food.Type}\nDescription: {food.Description}\nPrice: {food.Price}\n";
    //         foodList.Add(FoodInfo);

    //     }

    //     int option = OptionsMenu.DisplaySystem(foodList, "Current menu", "Use ⬆ and ⬇ to navigate and press Enter to remove the selected food item:", true, true);

    //     if (option >= 1 && option <= foodList.Count)
    //     {
    //         // Get the index of the selected option (adjusted for 0-based indexing)
    //         int selectedOptionIndex = option - 1;

    //         // Extract the movie ID from the selected option string
    //         string selectedOption = foodList[selectedOptionIndex];
    //         int startIndex = selectedOption.IndexOf("ID: ") + 4;
    //         int endIndex = selectedOption.IndexOf('\n', startIndex);
    //         string idString = selectedOption.Substring(startIndex, endIndex - startIndex).Trim();

    //         int idToRemove;
    //         bool isValidId = int.TryParse(idString, out idToRemove);

    //         if (isValidId)
    //         {
    //             // Remove the movie with the specified ID
    //             CateringModel? foodToRemove = foods.Find(food => food.Id == idToRemove);
    //             if (foodToRemove != null)
    //             {
    //                 foods.Remove(foodToRemove);
    //                 CateringAccess.WriteAll(foods);
    //                 Console.WriteLine("Food item removed successfully.");
    //             }
    //         }
    //         else
    //         {
    //             Console.WriteLine("Invalid ID found for the selected Food item.");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine("Invalid option selected.");
    //     }
    // }
    protected static List<string> FoodEditorList = new List<string>()
    {
        "Add food item(s)",
        "Edit/Remove food item(s)"
    };
    protected static List<string> YN = new List<string>()
    {
        "Yes",
        "No"
    };
    protected static List<string> AddFoodList = new List<string>()
    {
        "Single food item entry",
        "JSON File",
        "CSV File"

    };
    protected static List<string> RemoveList = new List<string>()
    {
        "Remove food item selection",
        "Remove food item by ID"
    };
    public void EmployeeCatering()
    {
        while (true)
        {
            Console.Clear();
            int MenuOptions = OptionsMenu.DisplaySystem(FoodEditorList, "edit Catering", "Select what you want to do.", true, true);

            if (MenuOptions == 1)
            {
                Console.Clear();
                int addOptions = OptionsMenu.DisplaySystem(AddFoodList, "Add Food items", "To add food items by file, please save the json or csv file in the DataSources folder", true, true);

                if (addOptions == 1)
                {
                    AddOrUpdateFood();
                }
                else if (addOptions == 2)
                {
                    Console.Clear();
                    AddMultiplefoodJSON();
                }
                else if (addOptions == 3)
                {
                    Console.Clear();
                    AddMultipleFoodCSV();
                }
            }
            else if (MenuOptions == 2)
            {
                LoadCatering();
                while (true)
                {
                    // list of options to display
                    List<string> OptionList = new List<string>()
                    {
                    "Sort",
                    "Filter",
                    "Search",
                    "Show Whole Menu"
                    };

                    // the necessary info gets used in the display method
                    int option = OptionsMenu.DisplaySystem(OptionList, "edit CATERING");

                    // depending on the option that was chosen, it will clear the console and call the right function
                    if (option == 1)
                    {
                        CateringMenu.Sort(true);
                    }
                    else if (option == 2)
                    {
                        CateringMenu.Filter(true);
                    }
                    else if (option == 3)
                    {
                        while (true)
                        {
                            List<string> id_or_else = new(){"Id", "Other"};
                            int option2 = OptionsMenu.DisplaySystem(id_or_else, "Search by", "What do you want to search by?");

                            if (option2 == 1)
                            {
                                CateringModel result = CateringMenu.SearchId();
                                if (result == null)
                                {
                                    OptionsMenu.Logo("ID NOT FOUND");
                                    Console.WriteLine("No item with this id was found.");

                                    // prints a fake return option hehe
                                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                                
                                    // actually returns you to the main menu
                                    Console.ReadLine();  
                                }
                                else
                                {
                                    EditCatering(result);
                                }
                            }
                            if (option2 == 2)
                            {
                                CateringMenu.Search(true);
                            }
                            
                            else if (option2 == 3)
                            {
                                break;
                            }
                        }

                    }
                    else if (option == 4)
                    {
                        PrintMenu(_menu, true);
                    }

                    else if (option == 5)
                    {
                        break;
                    }
                }
            }
            else if (MenuOptions == 3)
            {
                break;
            }
        }
        
    }
}