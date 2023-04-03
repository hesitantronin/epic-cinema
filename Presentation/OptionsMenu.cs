using System.Text;

static class OptionsMenu
{ 
    public static void Start()
    {
        Console.Clear();

        // list of options that will be displayed
        List<string> StartList = new List<string>()
        {
            "Login",
            "Register",
            "Guest",
            "Exit"
        };

        // the necessary info gets used in the display method
        int option = OptionsMenu.DisplaySystem(StartList, "START", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, false);
        
        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 1)
        {
            Console.Clear();
            UserLogin.Login();
        }
        else if (option == 2)
        {
            Console.Clear();
            UserLogin.Register();
        }
        else if (option == 3)
        {
            Console.Clear();
        }

        // starts up the movie menu
        if (option != 4)
        {
            MovieMenu.Start();
        }
    }

    static public void Logo(string title = "")
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(@"      ,----.      _ __     .=-.-.  _,.----.            _,.----.    .=-.-. .-._            ,----.         ___     ,---.      ");
        Console.WriteLine(@"   ,-.--` , \  .-`.' ,`.  /==/_ /.' .' -   \         .' .' -   \  /==/_ //==/ \  .-._  ,-.--` , \ .-._ .'=.'\  .--.'  \     ");
        Console.WriteLine(@"  |==|-  _.-` /==/, -   \|==|, |/==/  ,  ,-'        /==/  ,  ,-' |==|, | |==|, \/ /, /|==|-  _.-`/==/ \|==|  | \==\-/\ \    ");
        Console.WriteLine(@"  |==|   `.-.|==| _ .=. ||==|  ||==|-   |  .        |==|-   |  . |==|  | |==|-  \|  | |==|   `.-.|==|,|  / - | /==/-|_\ |   ");
        Console.WriteLine(@" /==/_ ,    /|==| , '=',||==|- ||==|_   `-' \       |==|_   `-' \|==|- | |==| ,  | -|/==/_ ,    /|==|  \/  , | \==\,   - \  ");
        Console.WriteLine(@" |==|    .-' |==|-  '..' |==| ,||==|   _  , |       |==|   _  , ||==| ,| |==| -   _ ||==|    .-' |==|- ,   _ | /==/ -   ,|  ");
        Console.WriteLine(@" |==|_  ,`-._|==|,  |    |==|- |\==\.       /       \==\.       /|==|- | |==|  /\ , ||==|_  ,`-._|==| _ /\   |/==/-  /\ - \ ");
        Console.WriteLine(@" /==/ ,     //==/ - |    /==/. / `-.`.___.-'         `-.`.___.-' /==/. / /==/, | |- |/==/ ,     //==/  / / , /\==\ _.\=\.-' ");
        Console.WriteLine(@" `--`-----`` `--`---'    `--`-`                                  `--`-`  `--`./  `--``--`-----`` `--`./  `--`  `--`         ");
        Console.ResetColor();

        if (title != "")
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n{title.ToUpper()}\n");
            Console.ResetColor();
        }
    }

    static public int DisplaySystem(List<string> list, string title, string question = "", bool showlogo = true, bool showreturn = true)
    {
        // makes the cursor invisible
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        // prints the banner and the title
        if (showlogo)
        {
            OptionsMenu.Logo(title);
        }

        // adds extra info if provided
        if (question != "")
        {
            Console.WriteLine($"{question}\n");
        }

        // gets the cursor position and sets option to 1
        (int left, int top) = Console.GetCursorPosition();
        int option = 1;
        int returncount = 0;

        // this is the decorator that will help you see where the cursor is at
        var decorator = " > \u001b[32m";

        // sets a variable for 'key' that will be used later
        ConsoleKeyInfo key;

        // the loop in which an option is chosen from a list
        bool isSelected = false;
        while (!isSelected)
        {
            // sets the cursor to the previously determined location
            Console.SetCursorPosition(left, top);

            // prints the options one by one
            for (int i = 0; i < list.Count(); i++)
            {
                //writes the option and gives it a number
                Console.WriteLine($"{(option == i + 1 ? decorator : "   ")}{list[i]}\u001b[0m");
            }

            if (showreturn)
            {
                Console.WriteLine($"\n{(option == list.Count() + 1 ? decorator : "   ")}Return\u001b[0m");
                returncount = 1;
            }
            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? list.Count() + returncount : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == list.Count() + returncount ? 1 : option + 1;
                    break;

                // if enter is pressed, breaks out of the while loop
                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }
        }
        Console.CursorVisible = true;

        return option;
    }

    static public int DisplaySystem(List<MovieModel> list, string title, string question = "", bool showlogo = true, bool showreturn = true)
    {
        // makes the cursor invisible
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        // prints the banner and the title
        if (showlogo)
        {
            OptionsMenu.Logo(title);
        }

        // adds extra info if provided
        if (question != "")
        {
            Console.WriteLine($"{question}\n");
        }

        // gets the cursor position and sets option to 1
        (int left, int top) = Console.GetCursorPosition();
        int option = 1;
        int returncount = 0;

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
            for (int i = 0; i < list.Count(); i++)
            {
                // writes movie title in red
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{(option == i + 1 ? decorator : "   ")}{list[i].Title}\u001b[0m");
                Console.ResetColor();

                // prints the description
                Console.WriteLine($"    Description:\n    {MovieLogic.SpliceText(list[i].Description, "    ")}\n");
            }

            if (showreturn)
            {
                Console.WriteLine($"\n{(option == list.Count() + 1 ? decorator : "   ")}Return\u001b[0m");
                returncount = 1;
            }

            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? list.Count() + returncount : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == list.Count() + returncount ? 1 : option + 1;
                    break;

                // if enter is pressed, breaks out of the while loop
                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }
        }
        Console.CursorVisible = true;

        return option;
    }
}