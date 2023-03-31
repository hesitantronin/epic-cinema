using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class AccountsLogic
{
    private List<AccountModel> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
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
            AccountModel? accountModel = _accounts.Find(a => a.EmailAddress == email && a.Password == password);
            if(accountModel == null) 
            {
                Console.WriteLine("No account found with these credentials");
                return new AccountModel(0, email, password, string.Empty);
            } 
            else 
            {
                Console.WriteLine($"Welcome back {accountModel.FullName}.");
                Console.WriteLine($"Your email address is {accountModel.EmailAddress}.");
                accountModel.Authorize();
                return accountModel;
            }
        }
        return new AccountModel(0, email, password, string.Empty);
    } 

    // Returns true if the email is not empty, contains an '@', contains a '.' and does not contain any white space.
    public bool IsEmailValid(string email) => (!string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains(".") && !email.Contains(" "));
    
    // Return true if an account with this email is found in the JSON.
    public bool IsEmailInUse(string email) => (_accounts.Find(i => i.EmailAddress == email) != null);

    public int GetNextId()
    {
        int maxId = 0;

        foreach (var acc in _accounts)
            if (acc.Id > maxId)
                maxId = acc.Id;

        return maxId + 1;
    }
    public string GetMaskedPassword()
    {
        Console.Write("Enter password: ");
        string password = "";

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            else if (keyInfo.KeyChar >= 32 && keyInfo.KeyChar <= 126)
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
    }
}