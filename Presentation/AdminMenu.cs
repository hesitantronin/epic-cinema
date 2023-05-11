public class AdminMenu : EmployeeMenu
{
    public static new void StartAdmin()
    {
        Console.Clear();

        // Combine the StartList from AdminMenu and EmployeeMenu
        List<string> startList = new List<string>()
        {
            "Change Cinema font",
            // "Create admin account" (Optional if needed),
            "Create employee account",
            "Remove accounts"
        };

        startList.AddRange(StartList); // Add the options from EmployeeMenu

        // Display the menu and get the selected option
        int option = OptionsMenu.DisplaySystem(startList, "Admin Menu", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, false);

        Console.Clear();

        // Handle the selected option
        if (option == 1)
        {
            Console.WriteLine("Not yet implemented");
        }
        else if (option == 2)
        {
            UserLogin.Register(true);
        }
        // else if (option == 3)
        // {
        //     UserLogin.Register(false, true);
        // }
        else if (option == 3)
        {
            RemoveEmployeeAccount();
        }
        else if (option == 4)
        {
            Console.WriteLine("Not yet implemented");
        }
        else if (option == 5)
        {
            Console.WriteLine("Not yet implemented");
        }
        else if (option == 6)
        {
            Console.WriteLine("Not yet implemented");
        }
        else if (option == 7)
        {
            StartEmployee();
        }
        else if (option == 8)
        {
            Console.WriteLine("Exiting...");
        }
        else
        {
            Console.WriteLine("Invalid option");
        }
    }

    private static void RemoveEmployeeAccount()
    {
        List<AccountModel> accounts = AccountsAccess.LoadAll();

        // Retrieve and display the employee accounts
        List<string> employeeList = new List<string>();
        foreach (AccountModel account in accounts)
        {
            // if (account.Type == AccountModel.AccountType.EMPLOYEE || account.Type == AccountModel.AccountType.ADMIN)
            if (account.Type == AccountModel.AccountType.EMPLOYEE)
            {
                string employeeInfo = $"ID: {account.Id}\nName: {account.FullName}\nEmail: {account.EmailAddress}\n";
                employeeList.Add(employeeInfo);
            }
        }

        int option = OptionsMenu.DisplaySystem(employeeList, "Employee accounts", "Use ⬆ and ⬇ to navigate and press Enter to remove the selected account:", true, false);

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
                AccountModel accountToRemove = accounts.Find(account => account.Id == idToRemove);
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
