using System.Text;

class MovieLogic
{
    private List<MovieModel> _movies = new();
    static public MovieModel CurrentMovie { get; private set; }

    public MovieLogic()
    {
        // uses the loadall function to load the json to the list
        _movies = MovieAccess.LoadAll();
    }


    public void UpdateList(MovieModel mov)
    {
        // finds if there is already a movie with the same id
        int index = _movies.FindIndex(s => s.Id == mov.Id);

        // if the index exists, itll update the movie, otherwhise itll add a new one
        if (index != -1)
        {
            // updates existing movie
            _movies[index] = mov;
        }
        else
        {
            //adds new movie
            _movies.Add(mov);
        }

        // writes the changed data to the json file
        MovieAccess.WriteAll(_movies);
    }

    public void RemoveMovie(int id)
    {
        // finds if there is a movie with the same id
        int index = _movies.FindIndex(s => s.Id == id);

        // removes the movie with that id, and updates the json file
        _movies.Remove(_movies[index]);
        MovieAccess.WriteAll(_movies);
    }

    public MovieModel GetById(int id)
    {
        // returns the movie data that matches the id
        return _movies.Find(i => i.Id == id);
    }

    private SortOrder GetOrder()
    {
        //Some settings for how the menu will look/act
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        // Prints some instructions for the user
        Console.WriteLine("Would you like to sort ascending or descending?");

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
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Ascending\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Descending\u001b[0m");

            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 2 : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == 2 ? 1 : option + 1;
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
            return SortOrder.ASCENDING;
        }
        else if (option == 2)
        {
            Console.Clear();
            return SortOrder.DESCENDING;
        }

        // failsafe so code always returns something
        return SortOrder.ASCENDING;
    }

    enum SortOrder    
    {
        ASCENDING,
        DESCENDING
    }

    public void PrintMovies(List<MovieModel> to_print)
    {
        // writes header for movies
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("MOVIES\n");
        Console.ResetColor();

        // prints an error message if nothing was found
        if (to_print.Count() == 0)
        {

            Console.CursorVisible = false;
            Console.WriteLine("No movies were found that matched the criteria.\n");

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
                Console.WriteLine($"{(option == 1 ? decorator : "   ")}Return\u001b[0m");

                // sees what key has been pressed
                key = Console.ReadKey(false);

                // a switch case that changes the value from 'option', depending on the key input
                switch (key.Key)
                {
                    // moves one up
                    case ConsoleKey.UpArrow:
                        option = option == 1 ? 1 : option - 1;
                        break;
                        
                    // moves one down
                    case ConsoleKey.DownArrow:
                        option = option == 1 ? 1 : option + 1;
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
                MovieMenu.Start();
            }
        }
        else
        {           
            // makes a new movielogic to work with
            MovieLogic movielogic = new MovieLogic();

            //Some settings for how the menu will look/act
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            // Prints some instructions for the user
            Console.WriteLine("Select the movie you want to take a closer look at.\n");

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

                // prints the movies one by one
                for (int i = 0; i < to_print.Count(); i++)
                {
                    // writes movie title in red
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{(option == i + 1 ? decorator : "   ")}{to_print[i].Title}\u001b[0m");
                    Console.ResetColor();

                    // prints the description
                    Console.WriteLine($"    Description:\n    {MovieLogic.SpliceText(to_print[i].Description, "    ")}\n");
                }

                Console.WriteLine($"\n{(option == to_print.Count() + 1 ? decorator : "   ")}Return\u001b[0m");

                // sees what key has been pressed
                key = Console.ReadKey(false);

                // a switch case that changes the value from 'option', depending on the key input
                switch (key.Key)
                {
                    // moves one up
                    case ConsoleKey.UpArrow:
                        option = option == 1 ? to_print.Count() + 1 : option - 1;
                        break;
                        
                    // moves one down
                    case ConsoleKey.DownArrow:
                        option = option == to_print.Count() + 1 ? 1 : option + 1;
                        break;

                    // if enter is pressed, breaks out of the while loop
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }
                
            if (option == to_print.Count() + 1)
            {
                Console.Clear();
                MovieMenu.Start();
            } 
            else
            {
                Console.Clear();
                MovieInfo(to_print[option - 1]);
            }
        }

    }

    public void PrintMovies() => PrintMovies(_movies);
 
    public void MovieInfo(MovieModel movie)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{movie.Title.ToUpper()}\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Genre");
        Console.ResetColor();
        Console.WriteLine($" {movie.Genre}\n");


        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Rating");
        Console.ResetColor();
        Console.WriteLine($" {movie.Rating}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Age Restriction");
        Console.ResetColor();
        Console.WriteLine($" {movie.Age}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Description");
        Console.ResetColor();
        Console.WriteLine($" {MovieLogic.SpliceText(movie.Description, " ")}\n");

        //Some settings for how the menu will look/act
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        // Prints some instructions for the user
        Console.WriteLine("\nDo you want to select this movie?");

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
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Continue\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Return\u001b[0m");

            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 2 : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == 2 ? 1 : option + 1;
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
            Console.WriteLine("This option will lead to the catering page");
        }
        else if (option == 2)
        {
            Console.Clear();
            MovieMenu.Start();
        }
    }
    public List<MovieModel> SortBy(string input)
    {
        SortOrder order = GetOrder();
        List<MovieModel> unsorted = _movies;

        // Check what to sort by per subject
        if (input.ToUpper() == "DATE")
        {
            //This is if the user is a customer, they cannot see movies that have already played anymore.
            DateTime currentDateTime = DateTime.Now;
            if (order == SortOrder.ASCENDING)
                return unsorted.Where(m => m.PublishDate >= currentDateTime).OrderBy(m => m.PublishDate).ToList();
            if (order == SortOrder.DESCENDING)
                return unsorted.Where(m => m.PublishDate >= currentDateTime).OrderByDescending(m => m.PublishDate).ToList();
        }
        else if (input.ToUpper() == "GENRE")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.Genre).ToList() : unsorted.OrderByDescending(m => m.Genre).ToList();
        }
        else if (input.ToUpper() == "NAME")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.Title).ToList() : unsorted.OrderByDescending(m => m.Title).ToList();
        }
        else if (input.ToUpper() == "RATING")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.Rating).ToList() : unsorted.OrderByDescending(m => m.Rating).ToList();
        }
        else if (input.ToUpper() == "PUBLISH")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.PublishDate).ToList() : unsorted.OrderByDescending(m => m.PublishDate).ToList();
        }
        return unsorted;
    }

    public List<MovieModel> FilterBy(string? genre, bool? mature)
    {
        // copies the original list
        List<MovieModel> filtered = _movies;

        if (genre != null)
            filtered = _movies.Where(movie => movie.Genre == genre).ToList();
        if (mature == false)
            filtered = filtered.Where(movie => movie.Age < 18).ToList();

        return filtered;
    }

    public List<MovieModel> SearchBy(string query) 
    {
        List<MovieModel> searched = new();
        foreach(MovieModel m in _movies)
            if(m.Title.ToLower().Contains(query.ToLower()) || m.Description.ToLower().Contains(query.ToLower()))
                searched.Add(m);
        
        return searched;
    }

    public static string SpliceText(string inputText, string spacing) 
    {
        int lineLength = 50;
        string[] stringSplit = inputText.Split(' ');
        int charCounter = 0;
        string finalString = "";
    
        for(int i=0; i < stringSplit.Length; i++)
        {
            finalString += stringSplit[i] + " ";
            charCounter += stringSplit[i].Length;
    
            if(charCounter > lineLength)
            {
                finalString += $"\n{spacing}";
                charCounter = 0;
            }
        }
        if (inputText.Length < lineLength)
        {
            finalString += $"\n{spacing}";
        }
        return finalString;
    }
}