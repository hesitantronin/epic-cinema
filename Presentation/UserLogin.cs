static class UserLogin
{
    private static AccountsLogic accountsLogic = new AccountsLogic();

    public static void Login()
    {
        Console.CursorVisible = true;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to the login page\n");
        Console.ResetColor();

        string email = "";
        string password = "";

        while (true)
        {
            Console.WriteLine("Please enter your email address: ");
            email = Console.ReadLine() + "";
            Console.WriteLine("Please enter your password: ");
            password = accountsLogic.GetMaskedPassword();

            AccountModel currentAccount = accountsLogic.Auth(email, password);
            if(currentAccount.Authorized == true) 
            {
                accountsLogic.SetCurrentAccount(currentAccount);
                break;
            }  
        }

        Console.CursorVisible = false;
    }

    public static void Register()
    {
        Console.CursorVisible = true;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to the registration page\n");
        Console.ResetColor();

        string email = string.Empty;
        bool test = false;

        while(test == false) {
            Console.WriteLine("Please enter your email address:");
            email = Console.ReadLine() + "";

            if (!accountsLogic.IsEmailValid(email)) 
                Console.WriteLine("Invalid email, please try again.");
            else if(accountsLogic.IsEmailInUse(email))
                Console.WriteLine("This email is already in use, please try again.");
            else
                test = true;
        }

        string password = string.Empty;
        string confirmedPassword = "no match";

        while (true)
        {
            Console.Write("\nEnter password:\n");
            password = accountsLogic.GetMaskedPassword();
            if (accountsLogic.IsPasswordValid(password))
            {
                while(password != confirmedPassword) 
                {           
                    Console.WriteLine("Please confirm your password");
                    confirmedPassword = accountsLogic.GetMaskedPassword();

                    if (password != confirmedPassword)
                    {
                        Console.WriteLine("Passwords do not match, please try again.");
                    }
                }
                break;
            }            
            Console.WriteLine("\nPassword must be between 8 and 32 characters long and contain atleast one number, uppercase character and special character");
        }

        Console.WriteLine("Please enter your full name");
        string fullName = Console.ReadLine() + "";

        AccountModel acc = new AccountModel(accountsLogic.GetNextId(), email, password, fullName);
        accountsLogic.UpdateList(acc);

        accountsLogic.SetCurrentAccount(acc);

        Console.WriteLine("\nAccount created successfully!");
        Console.WriteLine($"Welcome, {fullName}.");

        Console.CursorVisible = false;
        Thread.Sleep(7000);

        OptionsMenu.Start();
    }
}