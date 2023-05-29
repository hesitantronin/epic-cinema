using System.Globalization;

class MovieMenu
{
    // makes a new instance of movielogic that can be used throughout this entire class
    public static MovieLogic movielogic = new MovieLogic();

    // a list of possible genres, later also possibleused for the employee menu
    public static List<string> Genres = new List<string>()
    {
        "Action",
        "Adventure",
        "Comedy",
        "Crime",
        "Mystery",
        "Fantasy",
        "Historical",
        "Horror",
        "Romance",
        "SciFi",
        "Thriller"
    };

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
            // list of options to display
            List<string> OptionList = new List<string>()
            {
                "Date",
                "Genre",
                "Title",
                "Rating",
                "Publishing Date"
            };

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(OptionList, "SORT MOVIES");
            
            // if the user chose return, the loop will end, otherwise itll ask about the sort order
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
        while (true)
        {
            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(Genres, "FILTER MOVIES");

            if (option == 12)
            {
                break;
            }
            else
            {
                // the necessary info gets used in the display method
                int option2 = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "FILTER MOVIES", "Show movies with mature rating:");

                // depending on the option that was chosen, it will call the right function
                if (option2 == 1)
                {
                    movielogic.PrintMovies(movielogic.FilterBy(Genres[option - 1], true), IsEmployee);
                }
                else if (option2 == 2)
                {
                    movielogic.PrintMovies(movielogic.FilterBy(Genres[option - 1], false), IsEmployee);
                }
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