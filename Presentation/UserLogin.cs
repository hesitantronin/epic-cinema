static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");

        while (true) 
        {
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine() + "";
            string password = accountsLogic.GetMaskedPassword();

            AccountModel currentAccount = accountsLogic.Auth(email, password);
            if(currentAccount.Authorized == true)
                break;
        }
    }

    public static void Register()
    {
        Console.WriteLine("Welcome to the registration page");

        string email = string.Empty;
        bool test = false;

        while(test == false) {
            Console.WriteLine("Please enter your email address");
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

        while(password != confirmedPassword) {
            password = accountsLogic.GetMaskedPassword();

            Console.WriteLine("Please confirm your password");
            confirmedPassword = accountsLogic.GetMaskedPassword();

            if (password != confirmedPassword)
                Console.WriteLine("Passwords do not match, please try again.");
        }

        Console.WriteLine("Please enter your full name");
        string fullName = Console.ReadLine() + "";

        AccountModel acc = new AccountModel(accountsLogic.GetNextId(), email, password, fullName);
        accountsLogic.UpdateList(acc);

        Console.WriteLine("Account created successfully!");
        Console.WriteLine($"Welcome, {fullName}.");
    }
}