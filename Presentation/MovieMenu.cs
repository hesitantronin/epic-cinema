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

        // Prints some instructions for the user
        Console.WriteLine("Would you like to sort, filter or search? Or would you like to see the entire movie list?");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nUse ⬆ and ⬇ to navigate and press Enter to select:");
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
            OptionsMenu.GoBack();
        }
    }

    static public void Sort()
    {
        // makes a new movielogic to work with
        MovieLogic movielogic = new MovieLogic();

        //Some settings for how the menu will look/act
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        // Prints some instructions for the user
        Console.WriteLine("What would you like to sort by?");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nUse ⬆ and ⬇ to navigate and press Enter to select:");
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
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Date\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Genre\u001b[0m");
            Console.WriteLine($"{(option == 3 ? decorator : "   ")}Name\u001b[0m");
            Console.WriteLine($"{(option == 4 ? decorator : "   ")}Rating\u001b[0m");
            Console.WriteLine($"{(option == 5 ? decorator : "   ")}Publishing Date\u001b[0m");

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
    }

    static public void Filter()
    {

    }

    static public void Search()
    {

    }
}