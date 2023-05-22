using System.Text;

class ReservationMenu
{
    static public void Start()
    {
        AccountsLogic accountsLogic = new AccountsLogic();

        if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST)
        {
            Console.Clear();
            OptionsMenu.Logo("registration");
            Console.WriteLine("In order to finalize your reservation, please create an account.");
            
            Console.CursorVisible = true;
            string email = "";

            while(true)
            {
                Console.WriteLine("Email Address:");
                email = Console.ReadLine() + "";

                 if (!accountsLogic.IsEmailValid(email))
                {
                    List<string> EList = new List<string>(){"Continue"};

                    int option = OptionsMenu.DisplaySystem(EList, "", "\nInvalid email, please try again.", false, true);

                    if (option == 2)
                    {
                        return;
                    }
                }
                else if(accountsLogic.IsEmailInUse(email))
                {
                    List<string> EList = new List<string>(){"Continue"};

                    int option = OptionsMenu.DisplaySystem(EList, "", "\nThis email is already in use, please try again.", false, true);

                    if (option == 2)
                    {
                        return;
                    }
                }
                else
                {
                    break;
                }
            }

            string password = "";
            string confirmedPassword = "no match";

            while (password != confirmedPassword)
            {
                Console.Clear();

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

                        int option = OptionsMenu.DisplaySystem(BList, "", "\nPasswords do not match, please try again.", false, true);

                        if (option == 2)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    List<string> CList = new List<string>() { "Continue" };
                    int option = OptionsMenu.DisplaySystem(CList, "", "\nPassword must be between 8 and 32 characters long and contain atleast one number, one capital letter and one special character", false, true);
                
                    if (option == 2)
                    {
                        return;
                    }
                }
            }

            Console.Clear();
            OptionsMenu.Logo("Registration");
            Console.WriteLine("Please enter your name:");
            string fullName = Console.ReadLine() + "";

            AccountsLogic.CurrentAccount.EmailAddress = email;
            AccountsLogic.CurrentAccount.Password = accountsLogic.HashPassword(password);
            AccountsLogic.CurrentAccount.FullName = fullName;
            AccountsLogic.CurrentAccount.Type = AccountModel.AccountType.CUSTOMER;
            accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
        }
        
        AccountsLogic accountslogic = new AccountsLogic();

        List<string> ReturnList = new List<string>()
        {
            "Yes",
            "No"
        };

        int option1 = OptionsMenu.DisplaySystem(ReturnList, "", "\nWill you need any special assistance to make going to our cinema a more accessible experience for you?");

        switch (option1)
        {
            case 1:
                Console.WriteLine("\nPlease write down how we can be of assistance");
                string? accessibilityReq = Console.ReadLine();
                Console.WriteLine("");

                if (accessibilityReq != null)
                {
                    AccountsLogic.CurrentAccount.AccessibilityRequest = accessibilityReq;
                    accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
                }
                break;
            
            case 2:
                break;
        }

        // finalize reservation

    }
}
