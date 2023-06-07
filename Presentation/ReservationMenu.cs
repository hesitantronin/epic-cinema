using System.Text.RegularExpressions;

class ReservationMenu
{
    // boolean so the user gets send back to the start menu after reservation.
    public static bool reservationMade = false;

    public static AccountsLogic accountsLogic = new AccountsLogic();
    public static ReservationsLogic reservationsLogic = new ReservationsLogic();


    // starts up the final reservation menu
    static public void Start()
    {
        // With a guest account, we won't have any information about the person who's making a reservation. So when they're finishing their reservation, they're asked to make an account
        // this way, you can look at your options without having to create an account, but won't have to go back to create an account if you do decide to make a reservation
        bool IsLoggedIn = false; 
        while (true)
        {
            if (ReservationMenu.reservationMade)
            {
                return;
            }

            if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST)
            {
                List<string> LoginOrRegister = new() {"Login", "Register"};
                int optione = OptionsMenu.DisplaySystem(LoginOrRegister, "registration/login", "In order to finalize your reservation, please create an account or login.");

                if (optione == 1)
                {
                    IsLoggedIn = GuestToLogin();
                    
                }
                else if (optione == 2)
                {
                    IsLoggedIn = GuestToRegister();
                }
                else
                {
                    return;
                }
            }
            else
            {
                IsLoggedIn = true;
            }
            
            if (IsLoggedIn)
            {
                if (!AssistanceOption())
                {
                    return;
                }
            }

        }
    }

    // a login that transports the reservation data from the guest acc to the acc that has been logged in
    public static bool GuestToLogin()
    {
        Console.CursorVisible = true;

        string email = "";
        string password = "";
        bool loggedin = false;

        while (true)
        {
            OptionsMenu.Logo("login");

            Console.WriteLine("Email address: ");
            email = Console.ReadLine() + "";

            Console.WriteLine("\nPassword: ");
            password = accountsLogic.GetMaskedPassword();

            //Return authorized accountmodel that matches both credentials. If no matching account is found, return an empty unauthorized account
            AccountModel currentAccount;
            List<AccountModel> accounts = AccountsAccess.LoadAll();
            AccountModel? accountModel = accounts.Find(a => a.EmailAddress == email && a.Password == AccountsLogic.HashPassword(password));

            if(AccountsLogic.IsLoginValid(email, password) && accountModel != null) 
            { 
                accountModel.Authorize();

                // transfers the reservation data
                accountModel.AccessibilityRequest = AccountsLogic.CurrentAccount.AccessibilityRequest;
                accountModel.CateringReservation = AccountsLogic.CurrentAccount.CateringReservation;
                accountModel.SeatReservation = AccountsLogic.CurrentAccount.SeatReservation;
                accountModel.Movie = AccountsLogic.CurrentAccount.Movie;

                currentAccount = accountModel;

                OptionsMenu.StrayGuestAccKiller();

                if (currentAccount.Authorized == true && currentAccount.Type == AccountModel.AccountType.CUSTOMER)
                {
                    accountsLogic.SetCurrentAccount(currentAccount);
                    loggedin = true;
                }
                else
                {
                    OptionsMenu.Logo("Wrong account");
                    Console.WriteLine("This login is for customers only.\nIf you want to log in with a different kind of account, go back to the main menu.");
                    Console.ReadLine();

                }
                break;
            }
            else
            {
                List<string> EList = new List<string>(){"Continue"};

                int option = OptionsMenu.DisplaySystem(EList, "", "\nNo account found with these credentials.", false, true);
                
                currentAccount = new AccountModel(0, email, password, string.Empty);

                if (option == 2)
                {
                    break;
                }
            }
        }

        Console.CursorVisible = false;

        return loggedin;
    }

    // a login that transports the reservation data from the guest acc to the acc that has been created
    public static bool GuestToRegister()
    {
        Console.CursorVisible = true;
        string email = "";

        while(true)
        {
            OptionsMenu.Logo("registration");
            Console.WriteLine("Email Address:");
            email = Console.ReadLine() + "";

            if (!accountsLogic.IsEmailValid(email))
            {
                List<string> EList = new List<string>(){"Continue"};

                int option = OptionsMenu.DisplaySystem(EList, "", "\nInvalid email, please try again.", false, true);

                if (option == 2)
                {
                    return false;
                }
            }
            else if(accountsLogic.IsEmailInUse(email))
            {
                List<string> EList = new List<string>(){"Continue"};

                int option = OptionsMenu.DisplaySystem(EList, "", "\nThis email is already in use, please try again.", false, true);

                if (option == 2)
                {
                    return false;
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
            OptionsMenu.Logo("registration");
            Console.WriteLine("Password:");

            password = accountsLogic.GetMaskedPassword();
            if (accountsLogic.IsPasswordValid(password))
            {
                OptionsMenu.Logo("registration");

                Console.WriteLine("Confirm Password:");
                confirmedPassword = accountsLogic.GetMaskedPassword();

                if (password != confirmedPassword)
                {
                    List<string> BList = new List<string>() { "Continue" };

                    int option = OptionsMenu.DisplaySystem(BList, "", "\nPasswords do not match, please try again.", false, true);

                    if (option == 2)
                    {
                        return false;
                    }
                }
            }
            else
            {
                List<string> CList = new List<string>() { "Continue" };
                int option = OptionsMenu.DisplaySystem(CList, "", "\nPassword must be between 8 and 32 characters long and contain atleast one number, one capital letter and one special character", false, true);

                if (option == 2)
                {
                    return false;
                }
            }
        }

        OptionsMenu.Logo("Registration");
        Console.WriteLine("Please enter your name:");
        string fullName = Console.ReadLine() + "";

        // Update the info from the guest account, which was previously just "" or null, to the entered information
        AccountsLogic.CurrentAccount.EmailAddress = email;
        AccountsLogic.CurrentAccount.Password = AccountsLogic.HashPassword(password);
        AccountsLogic.CurrentAccount.FullName = fullName;

        // Since they're no longer a guest, their account type is also switched from guest to customer
        AccountsLogic.CurrentAccount.Type = AccountModel.AccountType.CUSTOMER;
        accountsLogic.UpdateList(AccountsLogic.CurrentAccount);

        return true;
    }

    // asks the user if they would need any help
    public static bool AssistanceOption()
    {   
        while (true)
        {
            if (ReservationMenu.reservationMade)
            {
                return false;
            }
            // This is for accessibility, customers can input a special request here
            int option1 = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "Accessibility", "Will you need any special assistance to make going to our cinema a more accessible experience for you?");

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
            else if (option1 == 3)
            {
                return false;
            }
          
            ApplyDiscount();
        }

    }

    public static void ApplyDiscount()
    {
        while (true)
        {
            if (ReservationMenu.reservationMade)
            {
                return;
            }
            int discountOption = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "Discounts", "Would you like to apply a discount code?");

            if (discountOption == 1)
            {
                string discountCode;
                while (true)
                {
                    OptionsMenu.Logo("Discounts");
                    Console.WriteLine("Please enter a discount code.");
                    discountCode = Console.ReadLine();

                    if (discountCode == "STUDENT2023" || discountCode == "PETTYME110" || discountCode == "WE-HATE-OLD-PEOPLE")
                    {
                        OptionsMenu.FakeContinue("Your discount code has been added to your reservation.", "Discounts");
                        break;
                    }
                    else
                    {
                        int option = OptionsMenu.DisplaySystem(MovieLogic.ContinueList, "", "\nInvalid promo code. Please enter a valid promo code.", false, true);

                        if (option == 1)
                        {
                            continue;
                        }
                        else if (option == 2)
                        {
                            break;
                        }
                    }
                }
                ReservationOverview(true);

            }
            else if (discountOption == 2)
            {
                ReservationOverview(false);
            }
            else if (discountOption == 3)
            {
                return;
            }
        }
        
    }

    // an overview of the final reservation, calculates the final price and returns it, together with the reservation code, so it doesnt have to be calculated twice
    public static void ReservationOverview(bool discount)
    {
        Console.CursorVisible = true;
        OptionsMenu.Logo("reservation");
        Console.WriteLine("Here are the details of your reservation:\n");
        Console.WriteLine($"NAME: {AccountsLogic.CurrentAccount.FullName}");
        Console.WriteLine($"MOVIE: {AccountsLogic.CurrentAccount.Movie.Title} ({AccountsLogic.CurrentAccount.SeatReservation.Count()} X € {String.Format("{0:0.00}", AccountsLogic.CurrentAccount.Movie.MoviePrice)})");


        double finalPrice = 0.0;

        Console.WriteLine($"\nSEAT(S):");
        string seatReservations = "";
        foreach (var seat in AccountsLogic.CurrentAccount.SeatReservation)
        {
            seatReservations += $"    [{seat.Id}] {seat.SeatTypeName} (+ € {String.Format("{0:0.00}", seat.Price)})\n";

            finalPrice += AccountsLogic.CurrentAccount.Movie.MoviePrice;
            finalPrice += seat.Price;
        }

        Console.WriteLine($"{seatReservations}");
        Console.WriteLine($"DATE AND TIME: {AccountsLogic.CurrentAccount.Movie.ViewingDate.ToString("dddd, dd MMMM yyyy, HH:mm")}");


        // A customer doesn't have to reserve catering items, so this checks whether or not they have
        if (AccountsLogic.CurrentAccount.CateringReservation.Count > 0)
        {

            Console.WriteLine($"\nCATERING:");

            string menuReservations = "";

            // cateringItems is a list of all cateringModels in catering.json
            List<CateringModel> cateringItems = CateringAccess.LoadAll();
            foreach (var item in AccountsLogic.CurrentAccount.CateringReservation)
            {
                // Gets every catering reservation from the cateringReservation dictionary which is a part of their account, and adds it to a string so it can be shown in the overview
                menuReservations += $"    {item.Key}: ({item.Value} X € {String.Format("{0:0.00}", Convert.ToDouble(cateringItems.Where(c => c.Name == item.Key).Select(c => c.Price).Sum()))})\n";

                // to get the total price, this finds the current catering item, locates it in cateringItems based on whether the name is the same as the current item (item.Key)
                // and then once its selected the right cateringModel, it gets the price of one of those specific items. Then that price is multiplied by how many times the
                // user has reserved that item (item.Value)
                finalPrice += (Convert.ToInt32(item.Value) * cateringItems.Where(c => c.Name == item.Key).Select(c => c.Price).Sum());
            }

            // prints all previously gathered menu reservations
            Console.WriteLine($"{menuReservations}");
        }

        // added date to make the uniqueness more foolproof
        Random random = new Random();
        string randomCode = Convert.ToString(random.Next(1000000, 9999999));
        string resCode = DateTime.Now.ToString("dd-MM-yyyy HH:mm/") + randomCode;

        Console.WriteLine($"RESERVATION CODE: {resCode}");

        if (AccountsLogic.CurrentAccount.AccessibilityRequest != "")
        {
            Console.WriteLine($"\nREQUEST: {MovieLogic.SpliceText(AccountsLogic.CurrentAccount.AccessibilityRequest, "    ")}");
        }
        if (discount)
        {
            //if there is a discount then there will be a 10% discount
            Console.WriteLine($"\nTOTAL PRICE: € {String.Format("{0:0.00}", finalPrice)}");
            Console.WriteLine($"DISCOUNT: € {String.Format("{0:0.00}", finalPrice * 0.1)}");
            Console.WriteLine($"\nFINAL PRICE: € {String.Format("{0:0.00}", finalPrice * 0.9)}");
        }
        else
        {
            Console.WriteLine($"\nTOTAL PRICE: {String.Format("{0:0.00}", finalPrice)} euros");
        }
        
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\nYour reservation has been made successfully!");
        Console.ResetColor();

        Console.WriteLine("");

        // // Add reservation to json
        ReservationToJson(resCode, finalPrice);

        // updates the seat csv
        UpdateSeatCsv();

        // Reservation data is temporarily stored in a users account. After the reservation is finalized, the data is removed from their account
        // This way, they can create multiple reservations
        reservationsLogic.RemoveReservationFromAccount();

        // return has been fixed thanks to the reservation boolean, would appreciate it if someone could give feedback
        List<string> emptyList = new List<string>(){"Return to Start"};
        int brek = OptionsMenu.DisplaySystem(emptyList, "", $"", false, false, "");
        if (brek == 1)
        {
            reservationMade = true;
        }

    }

    // writes the final reservation to a separate json
    public static void ReservationToJson(string resCode, double finalPrice)
    {
        // Get reservations already present in file
        List<ReservationsModel> originalReservations = ReservationsAccess.LoadAll();
        
        // Get maximum ID from the original reservations
        int maxId = 0;
        try
        {
            maxId = originalReservations.Max(reservation => reservation.Id);
        }
        catch (System.InvalidOperationException) // If the reservations file has no entries yet, there will be no max (no ids), so this is to circumvent that
        {
            maxId = 0;
        }

        // Update reservations.json to include this new reservation
        ReservationsModel newReservation = new ReservationsModel(maxId + 1, AccountsLogic.CurrentAccount.EmailAddress, AccountsLogic.CurrentAccount.FullName, AccountsLogic.CurrentAccount.Movie, AccountsLogic.CurrentAccount.SeatReservation, AccountsLogic.CurrentAccount.CateringReservation, AccountsLogic.CurrentAccount.AccessibilityRequest, AccountsLogic.CurrentAccount.Movie.ViewingDate, resCode, System.Math.Round(finalPrice, 2));
        reservationsLogic.UpdateList(newReservation);
    }

    // updates the seats from the reservation to unavailable/taken
    public static void UpdateSeatCsv()
    {
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
    }
}
