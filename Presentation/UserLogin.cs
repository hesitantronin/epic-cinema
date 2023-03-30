static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");

        while (true)
        {
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();

            string password = accountsLogic.GetMaskedPassword();

            AccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("Welcome back " + acc.FullName);
                Console.WriteLine("Your email address is " + acc.EmailAddress);

                break;
            }
            else
            {
                Console.WriteLine("No account found with these credentials");
            }
        }
    }

    public static void Register()
    {
        Console.WriteLine("Welcome to the registration page");

        string email;
        bool test = false;
        AccountModel registerAcc = null; // Initialize to null
        do
        {
            Console.WriteLine("Please enter your email address");
            email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
            {
                Console.WriteLine($"Invalid email, please try again.");
                continue;
            }
            registerAcc = accountsLogic.CheckEmailExists(email);
            test = true;

        } while (test == false);

        string password;
        string confirmedPassword;
        do
        {
            password = accountsLogic.GetMaskedPassword();
            Console.WriteLine("Please confirm your password");
            confirmedPassword = accountsLogic.GetMaskedPassword();
            if (password != confirmedPassword)
            {
                Console.WriteLine("Passwords do not match, please try again.");
            }
        } while (password != confirmedPassword);

        Console.WriteLine("Please enter your full name");
        string fullName = Console.ReadLine();

        int id = accountsLogic.GetNextId();
        AccountModel acc = new AccountModel(id, email, password, fullName);
        accountsLogic.UpdateList(acc);

        Console.WriteLine("Account created successfully!");
        Console.WriteLine($"Welcome {fullName}");
    }
}