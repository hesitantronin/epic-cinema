using System.Text;

static class OptionsMenu
{ 
    public static void Start()
    {
        Console.Clear();

        // logo gets printed
        OptionsMenu.Logo();

        //Some settings for how the menu will look/act
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        // Prints some instructions for the user
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
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Login\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Register\u001b[0m");
            Console.WriteLine($"{(option == 3 ? decorator : "   ")}Guest\u001b[0m");

            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 3 : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == 3 ? 1 : option + 1;
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
            UserLogin.Start();
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
        MovieMenu.Start();
    }

    static public void Logo()
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
    }

    static public void GoBack()
    {
        OptionsMenu.Start();
    }
}