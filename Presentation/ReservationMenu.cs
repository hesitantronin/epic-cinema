using System.Text;
using System.Text.RegularExpressions;

class ReservationMenu
{
    static public void Start()
    {
        AccountsLogic accountsLogic = new AccountsLogic();
        CateringAccess cateringAccess = new CateringAccess();

        // With a guest account, we won't have any information about the person who's making a reservation. So when they're finishing their reservation, they're asked to make an account
        // this way, you can look at your options without having to create an account, but won't have to go back to create an account if you do decide to make a reservation
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

            // Update the info from the guest account, which was previously just "" or null, to the entered information
            AccountsLogic.CurrentAccount.EmailAddress = email;
            AccountsLogic.CurrentAccount.Password = accountsLogic.HashPassword(password);
            AccountsLogic.CurrentAccount.FullName = fullName;

            // Since they're no longer a guest, their account type is also switched from guest to customer
            AccountsLogic.CurrentAccount.Type = AccountModel.AccountType.CUSTOMER;
            accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
        }

        AccountsLogic accountslogic = new AccountsLogic();

        List<string> ReturnList = new List<string>()
        {
            "Yes",
            "No"
        };

        // This is for accessibility, customers can input a special request here
        int option1 = OptionsMenu.DisplaySystem(ReturnList, "", "\nWill you need any special assistance to make going to our cinema a more accessible experience for you?");

        if (option1 == 1)
        {
            Console.WriteLine("\nPlease write down how we can be of assistance");
                string? accessibilityReq = Console.ReadLine();
                Console.WriteLine("");

                if (accessibilityReq != null)
                {
                    // sets the AccessibilityRequest part of an account to their specified request
                    AccountsLogic.CurrentAccount.AccessibilityRequest = accessibilityReq;
                    accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
                }
        }

        // final reservation overview

        Console.CursorVisible = true;
        OptionsMenu.Logo("reservation");
        Console.WriteLine("Here are the details of your reservation:\n");
        Console.WriteLine($"NAME: {AccountsLogic.CurrentAccount.FullName}");
        Console.WriteLine($"MOVIE: {AccountsLogic.CurrentAccount.Movie.Title}");

        double finalPrice = 0.0;

        Console.WriteLine($"SEAT(S):");
        string seatReservations = "";
        foreach (var seat in AccountsLogic.CurrentAccount.SeatReservation)
        {
            seatReservations += $"    {seat.Id} ({seat.SeatTypeName})\n";
            finalPrice += seat.Price;
        }

        Console.WriteLine($"{seatReservations}");
        Console.WriteLine($"DATE AND TIME: {AccountsLogic.CurrentAccount.Movie.ViewingDate}");


        // A customer doesn't have to reserve catering items, so this checks whether or not they have
        if (AccountsLogic.CurrentAccount.CateringReservation.Count > 0)
        {

            Console.WriteLine($"CATERING:");

            string menuReservations = "";

            // cateringItems is a list of all cateringModels in catering.json
            List<CateringModel> cateringItems = CateringAccess.LoadAll();
            foreach (var item in AccountsLogic.CurrentAccount.CateringReservation)
            {
                // Gets every catering reservation from the cateringReservation dictionary which is a part of their account, and adds it to a string so it can be shown in the overview
                menuReservations += $"    {item.Key}: x{item.Value}\n";

                // to get the total price, this finds the current catering item, locates it in cateringItems based on whether the name is the same as the current item (item.Key)
                // and then once its selected the right cateringModel, it gets the price of one of those specific items. Then that price is multiplied by how many times the
                // user has reserved that item (item.Value)
                finalPrice += (Convert.ToInt32(item.Value) * cateringItems.Where(c => c.Name == item.Key).Select(c => c.Price).Sum());
            }

            // prints all previously gathered menu reservations
            Console.WriteLine($"{menuReservations}");
        }

        Random random = new Random();
        Console.WriteLine($"RESERVATION CODE: {random.Next(1000000, 9999999)}");

        if (AccountsLogic.CurrentAccount.AccessibilityRequest != "")
        {
            Console.WriteLine($"\nREQUEST: {AccountsLogic.CurrentAccount.AccessibilityRequest}");
        }

        Console.WriteLine($"\nTOTAL PRICE: {finalPrice} euros");

        // Update the values in the CSV to show that the seats have been booked
        string movieTitle = Regex.Replace(AccountsLogic.CurrentAccount.Movie.Title, @"[^0-9a-zA-Z\._]", string.Empty);
        string[] movieViewingDate1 = AccountsLogic.CurrentAccount.Movie.ViewingDate.ToString().Split(" ");
        string movieViewingDate2 = string.Join(" ", movieViewingDate1[1].Replace(":", "_"));
        string movieViewingDate3 = string.Join(" ", movieViewingDate1[0].Replace("/", "_"));

        string pathToCsv = $@"DataSources/MovieAuditoriums/{movieTitle}/ID_{AccountsLogic.CurrentAccount.Movie.Id}_{movieTitle}_{movieViewingDate3 + "_" + movieViewingDate2}.csv";

        string[][] auditorium = SeatAccess.LoadAuditorium(pathToCsv);

        foreach (SeatModel seat in AccountsLogic.CurrentAccount.SeatReservation)
        {
            SeatAccess.UpdateSeatValue(auditorium, seat.Id, "0");
        }

        SeatAccess.WriteToCSV(auditorium, pathToCsv);

        // Temporary end of the program? without displaying anything it automatically goes back to another menu (seatMenu/movieMenu/cateringMenu) so this is to prevent that
        // if you press enter itll still go to a menu though
        List<string> emptyList = new List<string>(){};
        OptionsMenu.DisplaySystem(emptyList, "", $"\n", false, false, "");
    }
}
