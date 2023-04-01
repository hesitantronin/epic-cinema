using System.Text;

class MovieMenu
{
    static public void Start()
    {
        // makes a new movielogic to work with
        MovieLogic movielogic = new MovieLogic();

        //Some settings for how the menu will look/act
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        OptionsMenu.Logo();
        // writes header for movies
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nMOVIES\n");
        Console.ResetColor();

        // gets the cursor position and sets option to 1
        (int left, int top) = Console.GetCursorPosition();
        var option = 1;

        // this is the decorator that will help you see where the cursor is at
        var decorator = " > \u001b[32m";

        // sets a variable for 'key' that will be used later
        ConsoleKeyInfo key;

        // the loop in which an option is chosen from a list
        bool isSelected = false;
        while (!isSelected)
        {
            // sets the cursor to the right position
            Console.SetCursorPosition(left, top);

            // prints the options and uses the decorator depending on what value 'option' has
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Sort\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Filter\u001b[0m");
            Console.WriteLine($"{(option == 3 ? decorator : "   ")}Search\u001b[0m");
            Console.WriteLine($"{(option == 4 ? decorator : "   ")}Show All Movies\u001b[0m");
            Console.WriteLine($"\n{(option == 5 ? decorator : "   ")}Return\u001b[0m");

            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 5 : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == 5 ? 1 : option + 1;
                    break;

                // if enter is pressed, breaks out of the while loop
                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }
        }

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
        // makes a new movielogic to work with
        MovieLogic movielogic = new MovieLogic();

        OptionsMenu.Logo();
        // writes header for movies
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nSORT MOVIES\n");
        Console.ResetColor();

        //Some settings for how the menu will look/act
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        // gets the cursor position and sets option to 1
        (int left, int top) = Console.GetCursorPosition();
        var option = 1;

        // this is the decorator that will help you see where the cursor is at
        var decorator = " > \u001b[32m";

        // sets a variable for 'key' that will be used later
        ConsoleKeyInfo key;

        // the loop in which an option is chosen from a list
        bool isSelected = false;
        while (!isSelected)
        {
            // sets the cursor to the right position
            Console.SetCursorPosition(left, top);

            // prints the options and uses the decorator depending on what value 'option' has
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Date\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Genre\u001b[0m");
            Console.WriteLine($"{(option == 3 ? decorator : "   ")}Title\u001b[0m");
            Console.WriteLine($"{(option == 4 ? decorator : "   ")}Rating\u001b[0m");
            Console.WriteLine($"{(option == 5 ? decorator : "   ")}Publishing Date\u001b[0m");
            Console.WriteLine($"\n{(option == 6 ? decorator : "   ")}Return\u001b[0m");


            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 6 : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == 6 ? 1 : option + 1;
                    break;

                // if enter is pressed, breaks out of the while loop
                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }
        }

        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 1)
        {
            Console.Clear();
            movielogic.PrintMovies(movielogic.SortBy("DATE"));
        }
        else if (option == 2)
        {
            Console.Clear();
            movielogic.PrintMovies(movielogic.SortBy("GENRE"));
        }
        else if (option == 3)
        {
            Console.Clear();
            movielogic.PrintMovies(movielogic.SortBy("NAME"));
        }
        else if (option == 4)
        {
            Console.Clear();
            movielogic.PrintMovies(movielogic.SortBy("RATING"));
        }
        else if (option == 5)
        {
            Console.Clear();
            movielogic.PrintMovies(movielogic.SortBy("PUBLISH"));
        }
        else if (option == 6)
        {
            Console.Clear();
            MovieMenu.Start();
        }
    }

    static public void Filter()
    {
        MovieLogic movielogic = new MovieLogic();

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
        MovieLogic movielogic = new MovieLogic();
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