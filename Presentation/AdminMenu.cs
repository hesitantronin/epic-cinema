class AdminMenu : EmployeeMenu
{
    public static void StartAdmin()
    {
        while (true)
        {
            Console.Clear();

            // Combine the StartList from AdminMenu and EmployeeMenu
            List<string> startList = new List<string>()
            {
                "Change Cinema font",
                "Create admin / employee account",
                "Remove accounts"
            };
            

            startList.AddRange(StartList); // Add the options from EmployeeMenu

            // Display the menu and get the selected option
            int option = OptionsMenu.DisplaySystem(startList, "Admin Menu", "Select what you want to do.");

            // Handle the selected option
            if (option == 1)
            {
                Console.WriteLine("Not yet implemented");
            }
            else if (option == 2)
            {
                account.LoadAccounts();
                AccountCreator();
            }
            else if (option == 3)
            {
                RemoveEmployeeAccount();
            }
            else if (option == 4)
            {
                movie.EmployeeMovies();
            }
            else if (option == 5)
            {
                food.EmployeeCatering();
            }
            else if (option == 6)
            {
                EmployeeMenu.EditGlobalSeatData();
            }
            else if (option == 7)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }
    }

    private static void AccountCreator()
    {
        List<string> account = new List<string>()
        {
            "Create Admin account",
            "Create Employee account",
        };

        int option = OptionsMenu.DisplaySystem(account, "Account creator menu", "What type of account do you want to add", true, true);
        Console.Clear();

        // Handle the selected option
        if (option == 1)
        {
            AccountsLogic.Register(false, true);
        }
        else if (option == 2)
        {
            AccountsLogic.Register(true);
        }
    }
    private static void RemoveEmployeeAccount()
    {
        List<AccountModel> tempaccounts = AccountsAccess.LoadAll();
        List<AccountModel> accounts = new();
        foreach (AccountModel acc in tempaccounts)
        {
            if (acc.Id != AccountsLogic.CurrentAccount.Id)
            {
                accounts.Add(acc);
            }
        }

        // Retrieve and display the employee accounts
        List<string> employeeList = new List<string>();
        foreach (AccountModel account in accounts)
        {
            if (account.Type == AccountModel.AccountType.EMPLOYEE || account.Type == AccountModel.AccountType.ADMIN)
            {
                string employeeInfo = $"ID: {account.Id}\n   Name: {account.FullName}\n   Email: {account.EmailAddress}\n   Account Type: {account.Type}\n";
                employeeList.Add(employeeInfo);
            }
        }

        int option = OptionsMenu.DisplaySystem(employeeList, "Employee / admin accounts", "Choose what account you want to remove");

        if (option >= 1 && option <= employeeList.Count)
        {
            // Get the index of the selected option (adjusted for 0-based indexing)
            int selectedOptionIndex = option - 1;

            // Extract the employee ID from the selected option string
            string selectedOption = employeeList[selectedOptionIndex];
            int startIndex = selectedOption.IndexOf("ID: ") + 4;
            int endIndex = selectedOption.IndexOf('\n', startIndex);
            string idString = selectedOption.Substring(startIndex, endIndex - startIndex).Trim();

            int idToRemove;
            bool isValidId = int.TryParse(idString, out idToRemove);

            if (isValidId)
            {
                // Create an instance of AccountsLogic
                AccountsLogic accountsLogic = new AccountsLogic();

                // Call the RemoveAcc method on the instance
                accountsLogic.RemoveAccAdmin(idToRemove);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid ID found for the selected employee account.");
                Console.ReadLine();
            }
        }
        else
        {
            return;
        }
    }
}