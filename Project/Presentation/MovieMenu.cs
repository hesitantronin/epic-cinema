using System.Globalization;

public class MovieMenu
{
    // makes a new instance of movielogic that can be used throughout this entire class
    public static MovieLogic movielogic = new MovieLogic();

    // a list of possible genres, later also possibleused for the employee menu


    // starts up the main movie menu
    static public void Start()
    {
        // the while loop makes it possible for the return buttons to work
        while (true)
        {
            // if the user comes back from the last reservation page, we want them to return to the start menu
            // so thats what this boolean is for
            if (ReservationMenu.reservationMade)
            {
                return;
            }

            // list of options to display
            List<string> OptionList = new List<string>()
            {
                "Sort",
                "Filter",
                "Search",
                "Show All Movies"
            };

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(OptionList, "MOVIES");  

            // depending on the option that was chosen, it will call the right function
            if (option == 1)
            {
                Sort();
            }
            else if (option == 2)
            {
                Filter();
            }
            else if (option == 3)
            {
                Search();
            }
            else if (option == 4)
            {
                movielogic.PrintMovies();
            }

            // breaks out of the while loop if return is selected
            else if (option == 5)
            {
                break;
            }
        }
    }

    // the menu function for the sort option
    static public void Sort(bool IsEmployee = false)
    {
        while (true)
        {
            if (ReservationMenu.reservationMade)
            {
                return;
            }

            // list of options to display
            List<string> OptionList = new List<string>()
            {
                "Viewing Date",
                "Genre",
                "Title",
                "Rating",
                "Publishing Date"
            };

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(OptionList, "SORT MOVIES");
            
            // if the user chose return, the loop will end, otherwise it'll ask about the sort order
            if (option == 6)
            {
                break;
            }
            else
            {
                // list of options that will be displayed
                List<string> AscDescList = new List<string>()
                {
                    "Ascending",
                    "Descending"
                };

                // the necessary info gets used in the display method
                int option2 = OptionsMenu.DisplaySystem(AscDescList, "SORT MOVIES");

                // the boolean gets changed according to the option chosen by the user
                bool ascending = true;
                if (option2 == 2)
                {
                    ascending = false;
                }

                // depending on the selected options, the movies are sorted in the correct way
                // the is employee boolean is there to start up the right menu later
                if (option2 != 3)
                {
                    if (option == 1)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("DATE", ascending), IsEmployee);
                    }
                    else if (option == 2)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("GENRE", ascending), IsEmployee);
                    }
                    else if (option == 3)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("NAME", ascending), IsEmployee);
                    }
                    else if (option == 4)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("RATING", ascending), IsEmployee);
                    }
                    else if (option == 5)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("PUBLISH", ascending), IsEmployee);
                    }
                }
            }
        }
    }

    // the menu for the filter function
    static public void Filter(bool IsEmployee = false)
    {
        // Options list for the filter menu
        List<string> filters = new() {"Genres", "Price Range", "Rating", "Date", "Remove active filters", "Continue with these filters"};

        // List of genres, changes when selected: the selected genre will have " [selected]" added onto it
        List<string> Genres = new List<string>()
        {"Action", "Adventure", "Comedy", "Crime", "Mystery", "Fantasy", "Historical", "Horror", "Romance", "SciFi", "Thriller"};

        // For returning to the previous menu if desired
        bool Return = false;

        // Instanciating of the different types of filters so that they can be used in all types of combinations regardless of what the user selects
        // the standard values are here for instanciating so if they stay at the original value the filter will be ignored 
        // (except for the price range which will just show all movies from 0 to the most expensive one)
        double highestPrice = MovieAccess.LoadAll().OrderByDescending(x => x.MoviePrice).FirstOrDefault()?.MoviePrice ?? 0;

        double from = 0;
        double to = highestPrice;
        double rating = 0;
        DateTime date = DateTime.MinValue;

        // The list of tuples that will be sent to the actual function where the filtering happens
        List<(object, string)> selectedFilters = new()
        {
            ((object)from, "price range value 1"),
            ((object)to, "price range value 2"),
            ((object)rating, "rating"),
            ((object)date, "date")
        };

        // Displays the active filters when the user is selecting them
        string DisplaySelectedFilters()
        {
            return (@$"
Active filters
Genres: [{string.Join(", ", selectedFilters.Where(pair => pair.Item2 == "genre").Select(pair => pair.Item1))}]
Price Range: [{(from)} - {(to)}]
Rating: [{(rating == 0 ? "" : rating)}]
Date: [{(date.ToString("dd/MM/yyyy") == "01/01/0001" ? "" : date.ToString("dd/MM/yyyy"))}]
            ");
        }

        if (ReservationMenu.reservationMade)
        {
            return;
        }

        // Start of main selecting
        while (true)
        {
            int option = OptionsMenu.DisplaySystem(filters, $"SELECT FILTER", DisplaySelectedFilters());


            if (option == 1) // Selecting the genres
            {
                if (ReservationMenu.reservationMade)
                    {
                        return;
                    }

                while (true)
                {

                    option = OptionsMenu.DisplaySystem(Genres, "GENRES");
                    
                    if (option == 12)
                    {
                        break;
                    }
                    else
                    {
                        string selectedGenre = Genres[option - 1];

                        if (!selectedFilters.Any(genre => genre.Item2 == "genre" && genre.Item1.ToString() == selectedGenre.Split(" ")[0])) // Split removes the " [selected]" text from the item for comparing
                        {
                            selectedFilters.Add(((object)selectedGenre, "genre"));
                            Genres[option - 1] += " [selected]"; 
                        }

                        else
                        {
                            selectedFilters.RemoveAll(genre => genre.Item2 == "genre" && genre.Item1.ToString() == selectedGenre.Split(" ")[0]);
                            Genres[option - 1 ] = (string)Genres[option - 1].Split(" ")[0];
                        }
                    }                
                }  
            }

            // Selecting the price range
            else if (option == 2)
            {
                if (ReservationMenu.reservationMade)
                {
                    return;
                }

                while (true)
                {
                    Return = false;

                    while (true)
                    {
                        OptionsMenu.Logo("PRICE RANGE");
    
                        Console.WriteLine("From: ");
                        string fromPrice = Console.ReadLine();
    
                        if (double.TryParse(fromPrice.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out from))
                        {                            
                            if (!(from > highestPrice))
                            {
                                break;
                            }

                            else
                            {
                                OptionsMenu.FakeContinue($"The value cannot exceed the highest movie price ({highestPrice})");
                            }
                        }
    
                        else
                        {
                            int Answer = OptionsMenu.DisplaySystem(new(){"retry"}, "", "\nInvalid price. Please enter a valid decimal number.", false);
        
                            if (Answer == 2)
                            {
                                from = 0; // Returns the first value to default if return is selcted instead of proceeding
                                Return = true;
                                break;
                            }
                        }
                    }

                    if (Return) break;
    
                    while (true)
                    {
                        OptionsMenu.Logo("PRICE RANGE");
    
                        Console.WriteLine("To: ");
                        string toPrice = Console.ReadLine();
    
                        if (double.TryParse(toPrice.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out to))
                        {
                            if (!(from >= to && from > -1))
                            {
                                if (!(to > highestPrice))
                                {
                                    break;
                                }
                                
                                else
                                {
                                    OptionsMenu.FakeContinue($"The value cannot exceed the highest movie price ({highestPrice})");
                                }
                            }
    
                            else
                            {
                                OptionsMenu.FakeContinue("Value must exceed preceeding value, and higher");
                            }
                        }
    
                        else
                        {
                            int Answer = OptionsMenu.DisplaySystem(new(){"retry"}, "", "\nInvalid price. Please enter a valid decimal number.", false);
        
                            if (Answer == 2)
                            {
                                to = highestPrice; // Returns the second value to default if return is selected
                                Return = true;
                                break;
                            } 
                        }                      
                    }

                    if (!Return) break;
                }
            }

            // Selecting minimum rating
            else if (option == 3)
            {
                while (true)
                {
                    OptionsMenu.Logo("Rating");
                    Console.WriteLine("What should the minimum rating be? (1/10)");
                    string toRating = Console.ReadLine();
    
                    if (double.TryParse(toRating.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rating))
                    {
                        if (!(rating >= 1 && rating <= 10))
                        {
                            int Answer = OptionsMenu.DisplaySystem(new(){"retry"}, "", "\nValue must be between 1 and 10 (including)", false);

                            if (Answer == 2)
                            {
                                Return = true;
                                break;
                            }
                        }

                        else
                        {
                            break;
                        }
                    } 

                    else
                    {
                        int Answer = OptionsMenu.DisplaySystem(new(){"retry"}, "", "\nInvalid rating, please try again.", false);

                        if (Answer == 2)
                        {
                            break;
                        }
                    }
                }

            }

            // Selecting the viewing date of the film the user wants to see
            else if (option == 4)
            {
                while (true)
                {
                    OptionsMenu.Logo("Add movie");

                    Console.WriteLine("Viewing Date: (MM-DD-YYYY)");
                    string[] dateInput = Console.ReadLine().Split("-");

                    if (dateInput.Length != 3 || !int.TryParse(dateInput[0], out int month) || !int.TryParse(dateInput[1], out int day) || !int.TryParse(dateInput[2], out int year))
                    {
                        int Answer = OptionsMenu.DisplaySystem(new() {"retry"}, "", "\nInvalid date format. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            return;
                        }
                        continue; // without continue, month, day, year will be considered unassigned
                    }

                    try
                    {
                        date = new(year, month, day);
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        int Answer = OptionsMenu.DisplaySystem(new(){"retry"}, "", "\nInvalid date, please try again.", false);

                        if (Answer == 2)
                        {
                            break;
                        }
                    }
                }
            }

            // Reset filters
            else if (option == 5)
            {
                from = 0;
                to = highestPrice;
                rating = 0;
                date = DateTime.MinValue;
                selectedFilters = new()
                {
                    ((object)from, "price range value 1"),
                    ((object)to, "price range value 2"),
                    ((object)rating, "rating"),
                    ((object)date, "date")
                };
                Genres = new()
                {"Action", "Adventure", "Comedy", "Crime", "Mystery", "Fantasy", "Historical", "Horror", "Romance", "SciFi", "Thriller"};

                OptionsMenu.Logo("cleared!");
                OptionsMenu.FakeContinue("Filters have been cleared!");
            }
            
            // Continue
            else if (option == 6)
            {
                break;
            }

            // Return
            else if (option == 7)
            {
                Return = true;
                break;
            }

        }

        if (!Return)
        {
            // the necessary info gets used in the display method
            int option2 = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "FILTER MOVIES", "Show movies with mature rating:");

            // depending on the option that was chosen, it will call the right function
            if (option2 == 1)
            {
                movielogic.PrintMovies(movielogic.FilterBy(selectedFilters, true), IsEmployee);
            }
            else if (option2 == 2)
            {
                movielogic.PrintMovies(movielogic.FilterBy(selectedFilters, false), IsEmployee);
            }
        }
    }

    // search menu for the search function
    static public void Search(bool IsEmployee = false)
    {
        Console.CursorVisible = true;

        // shows banner and title
        OptionsMenu.Logo("SEARCH MOVIES");

        // asks for an input to search for and searches
        Console.WriteLine("Search: ");
        string query = Console.ReadLine() + "";

        movielogic.PrintMovies(movielogic.SearchBy(query), IsEmployee);

        Console.CursorVisible = false;
    }

    // an extra search function only used for employees and admin, to easily find a movie by id
    static public MovieModel? SearchId()
    {
        Console.Clear();
        Console.CursorVisible = true;

        int id;
        while (true)
        {
            // shows banner and title
            OptionsMenu.Logo("SEARCH MENU");

            // asks for an input to search for and searches for it
            Console.WriteLine("Search: ");
            string query = Console.ReadLine() + "";

            if (int.TryParse(query.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out id))
            {
                break;
            }

            OptionsMenu.FakeContinue("Invalid ID. Please enter a valid number.");
        }

        Console.CursorVisible = false;

        return movielogic.GetById(id);

    }
}