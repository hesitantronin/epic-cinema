using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;


//This class is not static so later on we can use inheritance and interfaces
class AccountsLogic
{
    private List<AccountModel> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    private static AccountModel? CurrentAccount;

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
    }

    public void SetCurrentAccount(AccountModel account) {
        CurrentAccount = account;
    }


    public void UpdateList(AccountModel acc)
    {
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            _accounts[index] = acc;
        }
        else
        {
            //add new model
            _accounts.Add(acc);
        }
        AccountsAccess.WriteAll(_accounts);

    }

    public AccountModel? GetById(int id) => _accounts.Find(i => i.Id == id);

    // Return false if either of the parameters are empty or null
    public bool IsLoginValid(string email, string password) => (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password));


    //Return authorized accountmodel that matches both credentials
    //If no matching account is found, return an empty unauthorized account
    public AccountModel Auth(string email, string password) 
    {
        if(IsLoginValid(email, password)) 
        {
            AccountModel? accountModel = _accounts.Find(a => a.EmailAddress == email && a.Password == HashPassword(password));
            if(accountModel == null) 
            {
                List<string> EList = new List<string>(){"Continue"};

                OptionsMenu.DisplaySystem(EList, "", "\nNo account found with these credentials.", false, false);
                
                Console.Clear();
                
                return new AccountModel(0, email, password, string.Empty);
            } 
            else 
            {
                Console.Clear();
                
                List<string> EList = new List<string>(){"Continue"};

                OptionsMenu.DisplaySystem(EList, "welcome page", $"Welcome back {accountModel.FullName}.\nYour email address is {accountModel.EmailAddress}", true, false);
                
                accountModel.Authorize();
                return accountModel;
            }
        }
        else
        {
            List<string> EList = new List<string>(){"Continue"};

            OptionsMenu.DisplaySystem(EList, "", "\nNo account found with these credentials.", false, false);
            
            Console.Clear();
        }
        return new AccountModel(0, email, password, string.Empty);
    } 

    // Returns true if the email is not empty, contains an '@', contains a '.' and does not contain any white space.
    public bool IsEmailValid(string email) => (!string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains(".") && !email.Contains(" "));

    // Return true if an account with this email is found in the JSON.
    public bool IsEmailInUse(string email) => _accounts.Any(i => string.Equals(i.EmailAddress, email, StringComparison.OrdinalIgnoreCase));

    // Return true if the given password matches the criteria
    public bool IsPasswordValid(string password)
    {
        return ((password.Length >= 8) && (password.Length <= 32)
                && password.Any(char.IsDigit)
                && password.Any(char.IsUpper)
                && password.Any(c => !char.IsLetterOrDigit(c)));
    }
    // Returns the nextID
    public int GetNextId() 
    {
        int maxId = 0;

        foreach (var acc in _accounts)
            if (acc.Id > maxId)
                maxId = acc.Id;

        return maxId + 1;
    }
    // Masks passwords in the terminal
    public string GetMaskedPassword()
    {
        string password = "";

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            // This code checks on the user input including space, and special symbols and converts them to asterisks in the terminal
            if (keyInfo.Key == ConsoleKey.Enter)
            {            
                break;
            }
            // This code block removes the asterisks if backspace is used in the code
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            // This code checks the ascii table for special characters and converts them to asterisks
            else if (keyInfo.KeyChar >= 32 && keyInfo.KeyChar <= 126)
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
    }

    public string HashPassword(string raw) 
    {
        // Using statement to ensure proper disposal of SHA256 instance.
        // Create SHA256 instance to generate hash
        using (SHA256 hash = SHA256.Create()) 
        {
            // Combine the hash bytes into a single string, compute the hash of the input ('raw')
            // And convert each byte to a string representation of its hex value.
            return String.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(raw)).Select(i => i.ToString("x2")));
        }
    }
}