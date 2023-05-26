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
                ChangeLogo();
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

    public static void ChangeLogo()
    {
        while (true)
        {
            List<string> logoNames = new()
            {
                "Original",
                "Swirly",
                "Wobbly",
                "Neat",
                "Blocky",
                "Old Timey",
                "Horror",
                "Simple"
            };

            int option = OptionsMenu.DisplaySystem(logoNames, "Change Logo", "Choose which font you want to have");

            if (option != 9)
            {
                WriteLogo(Convert.ToString(option));

                OptionsMenu.Logo("logo updated");
                Console.WriteLine("The logo has been updated.");
                
                // prints a fake return option hehe
                Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
            
                // actually returns you to the main menu
                Console.ReadLine();
                break;

            }
            else
            {
                break;
            }
        }

    }
 
    public static string ReadLogo()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logo.txt"));
        return File.ReadAllText(path);
    }

    public static void WriteLogo(string choice)
    {
        string[] ch = {choice};
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/logo.txt"));
        File.WriteAllLines(path, ch);
    }

    public static void SetLogo()
    {
        int choice = Convert.ToInt32(ReadLogo());
        
        Console.ForegroundColor = ConsoleColor.DarkRed;

        if (choice == 1)
        {
            Console.WriteLine($@"      ,----.      _ __     .=-.-.  _,.----.            _,.----.    .=-.-. .-._            ,----.         ___     ,---.  ™");
            Console.WriteLine($@"   ,-.--` , \  .-`.' ,`.  /==/_ /.' .' -   \         .' .' -   \  /==/_ //==/ \  .-._  ,-.--` , \ .-._ .'=.'\  .--.'  \");
            Console.WriteLine($@"  |==|-  _.-` /==/, -   \|==|, |/==/  ,  ,-'        /==/  ,  ,-' |==|, | |==|, \/ /, /|==|-  _.-`/==/ \|==|  | \==\-/\ \");
            Console.WriteLine($@"  |==|   `.-.|==| _ .=. ||==|  ||==|-   |  .        |==|-   |  . |==|  | |==|-  \|  | |==|   `.-.|==|,|  / - | /==/-|_\ |");
            Console.WriteLine($@" /==/_ ,    /|==| , '=',||==|- ||==|_   `-' \       |==|_   `-' \|==|- | |==| ,  | -|/==/_ ,    /|==|  \/  , | \==\,   - \");
            Console.WriteLine($@" |==|    .-' |==|-  '..' |==| ,||==|   _  , |       |==|   _  , ||==| ,| |==| -   _ ||==|    .-' |==|- ,   _ | /==/ -   ,|");
            Console.WriteLine($@" |==|_  ,`-._|==|,  |    |==|- |\==\.       /       \==\.       /|==|- | |==|  /\ , ||==|_  ,`-._|==| _ /\   |/==/-  /\ - \");
            Console.WriteLine($@" /==/ ,     //==/ - |    /==/. / `-.`.___.-'         `-.`.___.-' /==/. / /==/, | |- |/==/ ,     //==/  / / , /\==\ _.\=\.-'");
            Console.WriteLine($@" `--`-----`` `--`---'    `--`-`                                  `--`-`  `--`./  `--``--`-----`` `--`./  `--`  `--`");
        }
        else if (choice == 2)
        {
            Console.WriteLine(@"
 ___                   ___                                 
/ (_)      o          / (_)o                               
\__     _      __    |         _  _    _   _  _  _    __,  
/     |/ \_|  /      |     |  / |/ |  |/  / |/ |/ |  /  |  
\___/ |__/ |_/\___/   \___/|_/  |  |_/|__/  |  |  |_/\_/|_/
     /|                                                    
     \|  ");
        }
        else if (choice == 3)
        {
            Console.WriteLine(@"
 ___               _                       
 )_    _  o  _    / ` o  _    _   _ _   _  
(__   )_) ( (_   (_.  ( ) )  )_) ) ) ) (_( 
     (                      (_  ");
        }
        else if (choice == 4)
        {
            Console.WriteLine(@"
 _______ .______    __    ______      ______  __  .__   __.  _______ .___  ___.      ___      
|   ____||   _  \  |  |  /      |    /      ||  | |  \ |  | |   ____||   \/   |     /   \     
|  |__   |  |_)  | |  | |  ,----'   |  ,----'|  | |   \|  | |  |__   |  \  /  |    /  ^  \    
|   __|  |   ___/  |  | |  |        |  |     |  | |  . `  | |   __|  |  |\/|  |   /  /_\  \   
|  |____ |  |      |  | |  `----.   |  `----.|  | |  |\   | |  |____ |  |  |  |  /  _____  \  
|_______|| _|      |__|  \______|    \______||__| |__| \__| |_______||__|  |__| /__/     \__\");
        }
        else if (choice == 5)
        {
            Console.WriteLine(@"
 .----------------.  .----------------.  .----------------.  .----------------.  .----------------.  .----------------.  .-----------------.  .----------------.  .----------------.  .----------------.
| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |
| |  _________   | || |   ______     | || |     _____    | || |     ______   | || |     ______   | || |     _____    | || | ____  _____  | || |  _________   | || | ____    ____ | || |      __      | |
| | |_   ___  |  | || |  |_   __ \   | || |    |_   _|   | || |   .' ___  |  | || |   .' ___  |  | || |    |_   _|   | || ||_   \|_   _| | || | |_   ___  |  | || ||_   \  /   _|| || |     /  \     | |
| |   | |_  \_|  | || |    | |__) |  | || |      | |     | || |  / .'   \_|  | || |  / .'   \_|  | || |      | |     | || |  |   \ | |   | || |   | |_  \_|  | || |  |   \/   |  | || |    / /\ \    | |
| |   |  _|  _   | || |    |  ___/   | || |      | |     | || |  | |         | || |  | |         | || |      | |     | || |  | |\ \| |   | || |   |  _|  _   | || |  | |\  /| |  | || |   / ____ \   | |
| |  _| |___/ |  | || |   _| |_      | || |     _| |_    | || |  \ `.___.'\  | || |  \ `.___.'\  | || |     _| |_    | || | _| |_\   |_  | || |  _| |___/ |  | || | _| |_\/_| |_ | || | _/ /    \ \_ | |
| | |_________|  | || |  |_____|     | || |    |_____|   | || |   `._____.'  | || |   `._____.'  | || |    |_____|   | || ||_____|\____| | || | |_________|  | || ||_____||_____|| || ||____|  |____|| |
| |              | || |              | || |              | || |              | || |              | || |              | || |              | || |              | || |              | || |              | |
| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |
 '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'");
        }
        else if (choice == 6)
        {
            Console.WriteLine(@"
      ..      .                      .                       ...             .                                                             
   x88f` `..x88. .>                 @88>                  xH88'`~ .x8X      @88>                                                           
 :8888   xf`*8888%   .d``           %8P                 :8888   .f'8888Hf   %8P      u.    u.                 ..    .     :                
:8888f .888  `'`     @8Ne.   .u      .          .      :8888>  X8L  ^""`     .     x@88k u@88c.      .u     .888: x888  x888.        u     
88888' X8888. >'8x   %8888:u@88N   .@88u   .udR88N     X8888  X888h        .@88u  ^'8888''8888'   ud8888.  ~`8888~'888X`?888f`    us888u.  
88888  ?88888< 888>   `888I  888. ''888E` <888'888k    88888  !88888.     ''888E`   8888  888R  :888'8888.   X888  888X '888>  .@88 '8888' 
88888   '88888 '8%     888I  888I   888E  9888 'Y'     88888   %88888       888E    8888  888R  d888 '88%'   X888  888X '888>  9888  9888  
88888 '  `8888>        888I  888I   888E  9888         88888 '> `8888>      888E    8888  888R  8888.+'      X888  888X '888>  9888  9888  
`8888> %  X88!       uW888L  888'   888E  9888         `8888L %  ?888   !   888E    8888  888R  8888L        X888  888X '888>  9888  9888  
 `888X  `~""`   :   '*88888Nu88P    888&  ?8888u../     `8888  `-*""   /    888&   '*88*' 8888' '8888c. .+  '*88%''*88' '888!` 9888  9888  
   '88k.      .~    ~ '88888F`      R888'  '8888P'        '888.      :'     R888'    ''   'Y'    '88888%      `~    ''    `'`   '888*''888' 
     `''*==~~`         888 ^         ''      'P'            `''***~'`        ''                    'YP'                         ^Y'   ^Y'  
                       *8E                                                                                                                ");
        }
        else if (choice == 7)
        {
            Console.WriteLine(@"
▓█████  ██▓███   ██▓ ▄████▄      ▄████▄   ██▓ ███▄    █ ▓█████  ███▄ ▄███▓ ▄▄▄
▓█   ▀ ▓██░  ██▒▓██▒▒██▀ ▀█     ▒██▀ ▀█  ▓██▒ ██ ▀█   █ ▓█   ▀ ▓██▒▀█▀ ██▒▒████▄
▒███   ▓██░ ██▓▒▒██▒▒▓█    ▄    ▒▓█    ▄ ▒██▒▓██  ▀█ ██▒▒███   ▓██    ▓██░▒██  ▀█▄
▒▓█  ▄ ▒██▄█▓▒ ▒░██░▒▓▓▄ ▄██▒   ▒▓▓▄ ▄██▒░██░▓██▒  ▐▌██▒▒▓█  ▄ ▒██    ▒██ ░██▄▄▄▄██ 
░▒████▒▒██▒ ░  ░░██░▒ ▓███▀ ░   ▒ ▓███▀ ░░██░▒██░   ▓██░░▒████▒▒██▒   ░██▒ ▓█   ▓██▒
░░ ▒░ ░▒▓▒░ ░  ░░▓  ░ ░▒ ▒  ░   ░ ░▒ ▒  ░░▓  ░ ▒░   ▒ ▒ ░░ ▒░ ░░ ▒░   ░  ░ ▒▒   ▓▒█░
 ░ ░  ░░▒ ░      ▒ ░  ░  ▒        ░  ▒    ▒ ░░ ░░   ░ ▒░ ░ ░  ░░  ░      ░  ▒   ▒▒ ░
   ░   ░░        ▒ ░░           ░         ▒ ░   ░   ░ ░    ░   ░      ░     ░   ▒
   ░  ░          ░  ░ ░         ░ ░       ░           ░    ░  ░       ░         ░  ░");
        }
        else if (choice == 8)
        {
            Console.WriteLine(@"
_____         _          ____  _                                
| ____| _ __  (_)  ___   / ___|(_) _ __    ___  _ __ ___    __ _ 
|  _|  | '_ \ | | / __| | |    | || '_ \  / _ \| '_ ` _ \  / _` |
| |___ | |_) || || (__  | |___ | || | | ||  __/| | | | | || (_| |
|_____|| .__/ |_| \___|  \____||_||_| |_| \___||_| |_| |_| \__,_|
       |_|");
        }
        Console.ResetColor();

    }
}