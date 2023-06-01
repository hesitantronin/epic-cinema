using System.Text;

static class OptionsMenu
{ 
    // a list with the options yes and no, useful for the display system, since yes or no questions are common in this program
    public static List<string> YesNoList = new List<string>()
    {
        "Yes",
        "No"
    };

    // starts up the program
    public static void Start()
    {
        // deletes any movies or reservations where the viewing date was more than two weeks ago
        DeleteOldData();

        // a check that deletes any stray guest accounts
        StrayGuestAccKiller();
        
        // main loop for the start menu
        while (true)
        {
            // resets the boolean so that the return loops work again
            ReservationMenu.reservationMade = false;

            // menu layout 1, for if the user isnt logged in yet
            if (AccountsLogic.CurrentAccount == null || AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST)
            {

                // list of options that will be displayed in start
                List<string> StartList = new List<string>()
                {
                    "Login",
                    "Register",
                    "Guest",
                    "Info",
                };

                // the necessary info gets used in the display method
                // depending on the option that was chosen, it will call the right function
                int option = OptionsMenu.DisplaySystem(StartList, "START", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, true, "Exit");

                // Login
                if (option == 1)
                {
                    AccountsLogic accLogic = new AccountsLogic();
                    accLogic.Login();
                }

                // Register
                else if (option == 2)
                {
                    AccountsLogic.Register();
                }

                // Guest Login
                else if (option == 3)
                {
                    AccountsLogic.Guest();
                }

                // Shows the Info Page
                else if (option == 4)
                {
                    InfoPage();
                }

                // exits the program by breaking out op the loop;
                else if (option == 5)
                {

                    if (AccountsLogic.CurrentAccount != null && AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST)
                    {
                        AccountsLogic accLog = new AccountsLogic();
                        accLog.RemoveAcc(AccountsLogic.CurrentAccount.Id);
                        AccountsLogic.CurrentAccount = null;
                    }

                    break;
                }
            }

            // menu layout 2, for if the user IS logged in
            else
            {
                string menuOption;

                if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.EMPLOYEE)
                {
                    menuOption = "Employee Menu";
                }
                else if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.EMPLOYEE)
                {
                    menuOption = "Admin Menu";
                }
                else
                {
                    menuOption = "Make Reservation";
                }
                // list of options that will be displayed in start
                List<string> StartList = new List<string>()
                {
                    menuOption,
                    "View Reservations",
                    "Cinema information",
                };

                // the necessary info gets used in the display method
                // depending on the option that was chosen, it will call the right function
                int option = OptionsMenu.DisplaySystem(StartList, "START", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, true, "Logout");

                // // Logout
                // if (option == 1)
                // {
                //     LogOut();
                // }

                // depending on the kind of account, it will restart the right menu for you
                if (option == 1)
                {
                    if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST || AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.CUSTOMER)
                    {
                        MovieMenu.Start();
                    }
                    else if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.EMPLOYEE)
                    {
                        EmployeeMenu.StartEmployee();
                    }
                    else if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.ADMIN)
                    {
                        AdminMenu.StartAdmin();
                    }
                }

                // Shows the Info Page
                else if (option == 3)
                {
                    InfoPage();
                }

                else if (option == 2)
                {
                    // View reservations
                    ReservationsLogic reservationsLogic = new ReservationsLogic();
                    reservationsLogic.PrintReservations(reservationsLogic.GetOwnReservations());
                }
                // asks the user if they want to log out.
                // If the return of the logout functiun is true, the program quits by breaking out of the loop
                else if (option == 4)
                {
                    if (AccountsLogic.CurrentAccount != null)
                    {
                        if (LogOut())
                        {

                            break;
                        }
                    }
                }
            }
        }
    }

    // displays the current logo with account status
    static public void Logo(string title)
    {
        Console.Clear();
        
        // prints current logo in red
        AdminMenu.SetLogo();

        // prints acc name and acc type beneath the logo
        string accType = "";
        Console.ForegroundColor = ConsoleColor.DarkGray;

        if (AccountsLogic.CurrentAccount != null)
        {
            if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST)
            {
                Console.WriteLine("\n(Guest)");
            }
            else
            {
                if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.CUSTOMER)
                {
                    accType = "Customer";
                }            
                else if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.EMPLOYEE)
                {
                    accType = "Employee";
                }            
                else if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.ADMIN)
                {
                    accType = "Admin";
                }

                Console.Write($"\n{AccountsLogic.CurrentAccount.FullName} ({accType})\n");
            }
        }
        else
        {
            Console.WriteLine("\n(Not Logged in)");
        }
        Console.ResetColor();

        // prints a title
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n{title.ToUpper()}\n");
        Console.ResetColor();
    }

    // displays all the info about the cinema
    static public void InfoPage()
    {
        Logo("CINEMA INFO");

        // location info
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Location");
        Console.ResetColor();
        Console.WriteLine("Wijnhaven 107, 3011WN, Rotterdam");

        // contact info
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("\nContact");
        Console.ResetColor();
        Console.WriteLine("06 56745873, info@epic.cinema.nl");

        // opening hours
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("\nOpening Hours");
        Console.ResetColor();

        Console.WriteLine("Monday: 9:00 to 23:00");
        Console.WriteLine("Tuesday: 9:00 to 23:00");
        Console.WriteLine("Wednesday: 9:00 to 23:00");
        Console.WriteLine("Thursday: 9:00 to 23:00");
        Console.WriteLine("Friday: 9:00 to 23:00");
        Console.WriteLine("Saturday: 12:00 to 24:00");
        Console.WriteLine("Sunday: 13:00 to 22:00");

        FakeReturn();
    }

    // a quicker way of adding a visual return button
    public static void FakeReturn()
    {
        Console.CursorVisible = false;

        // prints a fake return option hehe
        Console.WriteLine("\n > \u001b[32mReturn\u001b[0m");
        
        // actually returns you >:)
        Console.ReadLine();

        Console.CursorVisible = true;
    }

    // a quicker way of adding a visual continue button, with added option for logo display, if you want the message to be on a new page
    public static void FakeContinue(string comment, string title = "")
    {
        Console.CursorVisible = false;

        if (title != "")
        {
            Logo(title);
        }

        Console.WriteLine(comment);
        
        // prints a fake continue option hehe
        Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
    
        // actually lets you continue >:)
        Console.ReadLine();

        Console.CursorVisible = true;
    }

    // this function will allow you to display a list of options that the user can select between using the arrow keys
    static public int DisplaySystem(List<string> list, string title, string question = "", bool showlogo = true, bool showreturn = true, string returntext = "Return")
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

            // this will show the return button
            if (showreturn)
            {
                Console.WriteLine($"\n{(option == list.Count() + 1 ? decorator : "   ")}{returntext}\u001b[0m");
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

    // an adapted version of the display system that has an added page system and can display info with the movie titles
    static public int MovieDisplaySystem(List<MovieModel> list, string title, string question = "", bool showlogo = true, bool previousButton = false, bool nextButton = false)
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
                Console.WriteLine($"    Viewing Date: {list[i].ViewingDate.ToString("dddd, dd MMMM yyyy, HH:mm")}\n    Base Price: € {String.Format("{0:0.00}", list[i].MoviePrice)}\n");
            }

            returncount = 1;

            // these two options get displayed depending on which page the user is.
            // so if the user is on the last page, the next won't be displayed and vice versa
            if (previousButton)
            {
                Console.WriteLine($"{(option == list.Count() + returncount ? decorator : "   ")}Previous\u001b[0m");
                returncount += 1;
            }
            if (nextButton)
            {
                Console.WriteLine($"{(option == list.Count() + returncount ? decorator : "   ")}Next\u001b[0m");
                returncount += 1;
            }

            // this will show the return button
            Console.WriteLine($"\n{(option == list.Count() + returncount ? decorator : "   ")}Return\u001b[0m");

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

    // a similarly adapted version of the display function, but this time for the catering system. 
    static public int CateringDisplaySystem(List<CateringModel> list, string title, string question = "", bool showlogo = true, bool previousButton = false, bool nextButton = false, bool showreturn = true)
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
                Console.WriteLine($"{(option == i + 1 ? decorator : "   ")}{list[i].Name}\u001b[0m");
                Console.ResetColor();

                // prints the description
                Console.WriteLine($"    Type: {list[i].Type}");
                Console.WriteLine($"    Price: € {String.Format("{0:0.00}", (list[i].Price))}\n");
            }

            returncount = 1;

            // these two options get displayed depending on which page the user is.
            // so if the user is on the last page, the next won't be displayed and vice versa
            if (previousButton)
            {
                Console.WriteLine($"{(option == list.Count() + returncount ? decorator : "   ")}Previous\u001b[0m");
                returncount += 1;
            }
            if (nextButton)
            {
                Console.WriteLine($"{(option == list.Count() + returncount ? decorator : "   ")}Next\u001b[0m");
                returncount += 1;
            }

            // this will show the return button
            Console.WriteLine($"\n{(option == list.Count() + returncount ? decorator : "   ")}Return\u001b[0m");

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

    static public int ReservationsDisplaySystem(List<ReservationsModel> list, string title, string question = "", bool showLogo = true, bool previousButton = false, bool nextButton = false, bool showReturn = true)
    {
        // makes the cursor invisible
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        // prints the banner and title
        if (showLogo)
        {
            OptionsMenu.Logo(title);
        }

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

            // prints the reservations one by one
            for (int i = 0; i < list.Count(); i++)
            {
                // writes the movie title in red
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{(option == i + 1 ? decorator : "   ")}{list[i].Movie.Title}\u001b[0m");
                Console.ResetColor();

                // prints the date and time of the reservation
                if (list[i].Cancelled)
                {
                    Console.WriteLine($"    Canceled\n");
                }
                else
                {
                    Console.WriteLine($"    Date and time: {list[i].Movie.ViewingDate.ToString("dddd, dd MMMM yyyy, HH:mm")}\n    Tickets: {list[i].SeatReservation.Count()}\n");
                }
            }

            returncount = 1;

            // this will show the return button
            if (previousButton)
            {
                Console.WriteLine($"{(option == list.Count() + returncount ? decorator : "   ")}Previous\u001b[0m");
                returncount += 1;
            }

            if (nextButton)
            {
                Console.WriteLine($"{(option == list.Count() + returncount ? decorator : "   ")}Next\u001b[0m");
                returncount += 1;
            }

            Console.WriteLine($"\n{(option == list.Count() + returncount ? decorator : "   ")}Return\u001b[0m");

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

    // a function that will reset the current account to null
    public static bool LogOut()
    {
        if (AccountsLogic.CurrentAccount != null)
        {
            int opt = OptionsMenu.DisplaySystem(YesNoList, "LOGOUT", "Are you sure you want to log out?", true, false);  

            if (opt == 1)
            {
                AccountsLogic.CurrentAccount = null;

                FakeContinue("You have been logged out.", "logout");
                
                StrayGuestAccKiller();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    // a function that is called at start up that will delete reservations and movies that were more than 2 weeks old
    public static void DeleteOldData()
    {
        // deletes old movies
        List<MovieModel> movies = MovieAccess.LoadAll();
        List<MovieModel> newmovies = new();

        foreach (MovieModel movie in movies)
        {
            if (movie.ViewingDate > DateTime.Now - TimeSpan.FromDays(14))
            {
                newmovies.Add(movie);
            }
        }
        MovieAccess.WriteAll(newmovies);

        // deletes old reservations
        List<ReservationsModel> reservations = ReservationsAccess.LoadAll();
        List<ReservationsModel> newres = new();
        foreach (ReservationsModel reservation in reservations)
        {
            if (reservation.Movie.ViewingDate > DateTime.Now - TimeSpan.FromDays(14))
            {
                newres.Add(reservation);
            }
        }
        ReservationsAccess.WriteAll(reservations);
    }

    // will delete any guest accounts that stay behind because of the program not quitting correctly
    public static void StrayGuestAccKiller()
    {
        List<AccountModel> Accounts = AccountsAccess.LoadAll();
        foreach (AccountModel acc in Accounts)
        {
            if (acc.Type == 0)
            {
                AccountsLogic accLog = new AccountsLogic();
                accLog.RemoveAcc(acc.Id);
            }
        }
    }

    // if you log in and reserve things but don't complete your reservation, the data from the old reservation will be removed from your account so you can make a new reservation
    public static void RemoveUnfinishedReservation()
    {
        if (AccountsLogic.CurrentAccount.Movie != null)
        {
            AccountsLogic accountsLogic = new AccountsLogic();
            AccountsLogic.CurrentAccount.Movie = null;
            AccountsLogic.CurrentAccount.CateringReservation = new Dictionary<string, string>();
            AccountsLogic.CurrentAccount.SeatReservation = new List<SeatModel>();
            AccountsLogic.CurrentAccount.AccessibilityRequest = "";

            // update the json to reflect this change
            accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
        }
    }
}