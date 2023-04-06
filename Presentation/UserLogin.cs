static class UserLogin
{
    private static AccountsLogic accountsLogic = new AccountsLogic();

    public static void Login()
    {
        Console.CursorVisible = true;

        string email = "";
        string password = "";

        while (true)
        {
            OptionsMenu.Logo("login");

            Console.WriteLine("Email address: ");
            email = Console.ReadLine() + "";
            Console.WriteLine("\nPassword: ");
            password = accountsLogic.GetMaskedPassword();

            AccountModel currentAccount = accountsLogic.Auth(email, password);
            if(currentAccount.Authorized == true && currentAccount.Type == AccountModel.AccountType.ADMIN)
            {
                accountsLogic.SetCurrentAccount(currentAccount);
                AdminMenu.Start();
                
            }
            else
            {
                accountsLogic.SetCurrentAccount(currentAccount);
                break;
            }  
        }

        Console.CursorVisible = false;
        Console.Clear();
    }

    public static void Register()
    {
        Console.CursorVisible = true;

        string email = string.Empty;
        bool test = false;

        while(test == false) 
        {
            OptionsMenu.Logo("registration");

            Console.WriteLine("Email Address:");
            email = Console.ReadLine() + "";

            if (!accountsLogic.IsEmailValid(email))
            {
                List<string> EList = new List<string>(){"Continue"};

                OptionsMenu.DisplaySystem(EList, "", "\nInvalid email, please try again.", false, false);
                
                Console.Clear();
            }
            else if(accountsLogic.IsEmailInUse(email))
            {
                List<string> EList = new List<string>(){"Continue"};

                OptionsMenu.DisplaySystem(EList, "", "\nThis email is already in use, please try again.", false, false);
                
                Console.Clear();
            }
            else
            {
                test = true;
            }
        }

        string password = string.Empty;
        string confirmedPassword = "no match";


        while (true)
        {
            Console.Clear();

            while (password != confirmedPassword)
            {
                OptionsMenu.Logo("registration");
                Console.WriteLine("Password:");
                password = accountsLogic.GetMaskedPassword();
                if (accountsLogic.IsPasswordValid(password))
                {
                    Console.Clear();
                    OptionsMenu.Logo("registration");

                    Console.WriteLine("Confirm Password:");
                    confirmedPassword = accountsLogic.GetMaskedPassword();

                    if (password != confirmedPassword)
                    {
                        List<string> BList = new List<string>() { "Continue" };

                        OptionsMenu.DisplaySystem(BList, "", "\nPasswords do not match, please try again.", false, false);

                        Console.Clear();
                    }
                }
                else
                {
                    List<string> CList = new List<string>() { "Continue" };
                    OptionsMenu.DisplaySystem(CList, "", "\nPassword must be between 8 and 32 characters long and contain atleast one number, one capital letter and one special character", false, false);
                    Console.Clear();
                }
            }
            break;
        }

        Console.Clear();
        OptionsMenu.Logo("registration");
        Console.WriteLine("Name:");
        string fullName = Console.ReadLine() + "";

        AccountModel acc = new AccountModel(accountsLogic.GetNextId(), email, accountsLogic.HashPassword(password), fullName);
        accountsLogic.UpdateList(acc);

        accountsLogic.SetCurrentAccount(acc);

        Console.Clear();

        List<string> DList = new List<string>(){"Continue"};

        OptionsMenu.DisplaySystem(DList, "welcome page", $"Account created successfully!\nWelcome, {fullName}.", true, false);
                
        Console.CursorVisible = false;

        Console.Clear();
    }
}