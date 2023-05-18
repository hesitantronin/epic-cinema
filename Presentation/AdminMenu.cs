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
                "Create admin/ employee account",
                "Remove accounts"
            };
            
            


            startList.AddRange(StartList); // Add the options from EmployeeMenu

            // Display the menu and get the selected option
            int option = OptionsMenu.DisplaySystem(startList, "Admin Menu", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, true, "Return");

            // Handle the selected option
            if (option == 1)
            {
                Console.WriteLine("Not yet implemented");
            }
            else if (option == 2)
            {
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
                //Seats
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
        int option = OptionsMenu.DisplaySystem(account, "Account creator menu", "Use ⬆ and ⬇ to navigate and press Enter to select the account type to create:", true, true);
        Console.Clear();

        // Handle the selected option
        if (option == 1)
        {
            AccountsLogic.Register(true);
        }
        else if (option == 2)
        {
            AccountsLogic.Register(false, true);
        }
    }
    private static void RemoveEmployeeAccount()
    {
        List<AccountModel> accounts = AccountsAccess.LoadAll();

        // Retrieve and display the employee accounts
        List<string> employeeList = new List<string>();
        foreach (AccountModel account in accounts)
        {
            if (account.Type == AccountModel.AccountType.EMPLOYEE || account.Type == AccountModel.AccountType.ADMIN)
            {
                string employeeInfo = $"ID: {account.Id}\nName: {account.FullName}\nEmail: {account.EmailAddress}\nAccount Type: {account.Type}\n";
                employeeList.Add(employeeInfo);
            }
        }

        int option = OptionsMenu.DisplaySystem(employeeList, "Employee accounts", "Use ⬆ and ⬇ to navigate and press Enter to remove the selected account:", true, true);

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
                // Remove the account with the specified ID
                AccountModel? accountToRemove = accounts.Find(account => account.Id == idToRemove);
                if (accountToRemove != null)
                {
                    accounts.Remove(accountToRemove);
                    AccountsAccess.WriteAll(accounts);
                    Console.WriteLine("Employee account removed successfully.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID found for the selected employee account.");
            }
        }
        else
        {
            Console.WriteLine("Invalid option selected.");
        }
    }
}
