class MovieMenu
{
    public static MovieLogic movielogic = new MovieLogic();
    static public void Start()
    {
        while (true)
        {
            Console.Clear();

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

    static public void Sort()
    {
        while (true)
        {
            Console.Clear();

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
            
            // depending on the option that was chosen, it will clear the console and call the right function
            if (option == 6)
            {
                break;
            }
            else
            {
                Console.Clear();

                // list of options that will be displayed
                List<string> AscDescList = new List<string>()
                {
                    "Ascending",
                    "Descending"
                };

                // the necessary info gets used in the display method
                int option2 = OptionsMenu.DisplaySystem(AscDescList, "SORT MOVIES");

                // depending on the option that was chosen, it will clear the console and call the right function
                bool ascending = true;

                if (option2 == 2)
                {
                    ascending = false;
                }

                if (option2 != 3)
                {
                    // depending on the option that was chosen, it will clear the console and call the right function
                    Console.Clear();

                    if (option == 1)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("DATE", ascending));
                    }
                    else if (option == 2)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("GENRE", ascending));
                    }
                    else if (option == 3)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("NAME", ascending));
                    }
                    else if (option == 4)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("RATING", ascending));
                    }
                    else if (option == 5)
                    {
                        movielogic.PrintMovies(movielogic.SortBy("PUBLISH", ascending));
                    }
                }
            }
        }
    }

    static public void Filter()
    {
        while (true)
        {
            Console.Clear();

            // a list of possible genres
            List<string> Genres = new List<string>()
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

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(Genres, "FILTER MOVIES");

            if (option == 12)
            {
                break;
            }
            else
            {
                Console.Clear();

                // list of options that will be displayed
                List<string> YesNoList = new List<string>()
                {
                    "Yes",
                    "No"
                };

                // the necessary info gets used in the display method
                int option2 = OptionsMenu.DisplaySystem(YesNoList, "FILTER MOVIES", "Show movies with mature rating:");

                // depending on the option that was chosen, it will clear the console and call the right function
                if (option2 == 1)
                {
                    movielogic.PrintMovies(movielogic.FilterBy(Genres[option - 1], true));
                }
                else if (option2 == 2)
                {
                    movielogic.PrintMovies(movielogic.FilterBy(Genres[option - 1], false));
                }
            }
        }  
    }

    static public void Search()
    {
        Console.Clear();
        Console.CursorVisible = true;

        // shows banner and title
        OptionsMenu.Logo("SEARCH MOVIES");

        // asks for an input to search for and searches
        Console.WriteLine("Search: ");
        string query = Console.ReadLine() + "";

        movielogic.PrintMovies(movielogic.SearchBy(query));

        Console.CursorVisible = false;
    }
}