using System.Text;

class MovieMenu
{
    public static MovieLogic movielogic = new MovieLogic();
    static public void Start()
    {
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

        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 1)
        {
            Console.Clear();
            Sort();
        }
        else if (option == 2)
        {
            Console.Clear();
            Filter();
        }
        else if (option == 3)
        {
            Console.Clear();
            Search();
        }
        else if (option == 4)
        {
            Console.Clear();
            movielogic.PrintMovies();
        }

        else if (option == 5)
        {
            Console.Clear();
            OptionsMenu.Start();
        }
    }

    static public void Sort()
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
        
        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 6)
        {
            Console.Clear();
            MovieMenu.Start();
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
            if (option2 == 1)
            {
                ascending = true;
                
                // depending on the option that was chosen, it will clear the console and call the right function
                if (option == 1)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("DATE", ascending));
                }
                else if (option == 2)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("GENRE", ascending));
                }
                else if (option == 3)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("NAME", ascending));
                }
                else if (option == 4)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("RATING", ascending));
                }
                else if (option == 5)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("PUBLISH", ascending));
                }
            }
            else if (option2 == 2)
            {
                ascending = false;

                // depending on the option that was chosen, it will clear the console and call the right function
                if (option == 1)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("DATE", ascending));
                }
                else if (option == 2)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("GENRE", ascending));
                }
                else if (option == 3)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("NAME", ascending));
                }
                else if (option == 4)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("RATING", ascending));
                }
                else if (option == 5)
                {
                    Console.Clear();
                    movielogic.PrintMovies(movielogic.SortBy("PUBLISH", ascending));
                }
            }
            else if (option2 == 3)
            {
                Console.Clear();
                MovieMenu.Sort();
            }

        }
    }

    static public void Filter()
    {
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
            Console.Clear();
            MovieMenu.Start();
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
                Console.Clear();
                movielogic.PrintMovies(movielogic.FilterBy(Genres[option - 1], true));
            }
            else if (option2 == 2)
            {
                Console.Clear();
                movielogic.PrintMovies(movielogic.FilterBy(Genres[option - 1], false));
            }
            else if (option2 == 3)
            {
                Console.Clear();
                MovieMenu.Filter();
            }
        }
        
    }

    static public void Search()
    {
        Console.CursorVisible = true;

        // shows banner and title
        OptionsMenu.Logo("SEARCH MOVIES");

        // asks for an input to search for and searches
        Console.WriteLine("Search: ");
        string? query = Console.ReadLine();
        Console.Clear();
        movielogic.PrintMovies(movielogic.SearchBy(query));

        Console.CursorVisible = false;
    }
}