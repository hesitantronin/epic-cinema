using System.Text.RegularExpressions;

public class ReservationsLogic
{
    private List<ReservationsModel> _reservations = new();

    public ReservationsLogic()
    {
        // uses the LoadAll function to load the json to the list
        LoadReservations();
    }

    private void LoadReservations()
    {
        _reservations = ReservationsAccess.LoadAll();
    }

    public void UpdateList(ReservationsModel reservation)
    {
        LoadReservations();

        _reservations.Add(reservation);
        ReservationsAccess.WriteAll(_reservations);
    }

    public void RemoveReservationFromAccount()
    {
        // Sets all reservation related properties from the current account back to as they were at the start, empty
        AccountsLogic accountsLogic = new AccountsLogic();
        AccountsLogic.CurrentAccount.Movie = null;
        AccountsLogic.CurrentAccount.CateringReservation = new Dictionary<string, string>();
        AccountsLogic.CurrentAccount.SeatReservation = new List<SeatModel>();
        AccountsLogic.CurrentAccount.AccessibilityRequest = "";

        // update the json to reflect this change
        accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
    }

    public static void RemoveReservationFromCSV(ReservationsModel reservation)
    {
        // When reserving a seat, the visual auditorium will be updated to show that the seat is reserved. When a reservation is cancelled, this function will be used to revert the seat(s) back to showing up as available

        // CSV names are based on information about the movie, so this uses that information to find the name of the CSV
        string movieTitle = Regex.Replace(reservation.Movie.Title, @"[^0-9a-zA-Z\._]", string.Empty);

        // This will change the movies viewingDate (8-13-2023 1:15:00) to how it is saved in the CSV title (8_13_2023_1_15_00)
        string[] movieViewingDate1 = reservation.Movie.ViewingDate.ToString().Split(" ");
        string movieViewingDate2 = string.Join(" ", movieViewingDate1[1].Replace(":", "_"));
        string movieViewingDate3 = string.Join(" ", movieViewingDate1[0].Replace("/", "_"));

        string pathToCsv = $@"DataSources/MovieAuditoriums/{movieTitle}/ID_{reservation.Movie.Id}_{movieTitle}_{movieViewingDate3 + "_" + movieViewingDate2}.csv";

        // Array containing the values of each seat
        string[][] seatarray = SeatAccess.LoadAuditorium(pathToCsv);

        // Reservations can reserve multiple seats so this updates the CSV for each seat
        foreach (var seat in reservation.SeatReservation)
        {
            // Gets the default value of the seat, which the seat will be reverted to
            string defaultseatvalue = SeatAccess.FindDefaultSeatValueArray(seat.Id);

            // Updates the seat value back to the default in the array, and then uses the array to update the CSV
            SeatAccess.UpdateSeatValue(seatarray, seat.Id, defaultseatvalue);
            SeatAccess.WriteToCSV(seatarray, pathToCsv);
        }
    }

    public List<ReservationsModel> GetOwnReservations()
    {
        LoadReservations();

        List<ReservationsModel> reservations = new List<ReservationsModel>();
        var CurrentAcc = AccountsLogic.CurrentAccount; // less typing later
        
        // Find a customers own reservations when they're logged in, and add them to the list
        foreach (ReservationsModel reservation in _reservations)
        {
            if (reservation.FullName == CurrentAcc.FullName && reservation.EmailAddress == CurrentAcc.EmailAddress)
            {
                reservations.Add(reservation);
            }
        }

        return reservations;
    }

    public void ReservationInfo(ReservationsModel reservation)
    {
        // Below: show reservation info

        OptionsMenu.Logo(reservation.FullName);

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Movie");
        Console.ResetColor();
        Console.WriteLine($" {reservation.Movie.Title}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Seat(s)");
        Console.ResetColor();
        foreach (SeatModel seat in reservation.SeatReservation)
        {
            Console.WriteLine($" [{seat.Id}] {seat.SeatTypeName} (+ € {String.Format("{0:0.00}", seat.Price)})");
        }
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Date and time");
        Console.ResetColor();
        Console.WriteLine($" {reservation.Movie.ViewingDate.ToString("dddd, dd MMMM yyyy, HH:mm")}\n");

        // Reserving catering is optional, so catering information will only be shown if the customer elected to reserve catering
        if (reservation.CateringReservation.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Catering");
            Console.ResetColor();

            foreach (var menuItem in reservation.CateringReservation)
            {
                Console.WriteLine($" {menuItem.Key}: X € {menuItem.Value}");
            }
        }
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Reservation code");
        Console.ResetColor();
        Console.WriteLine($" {reservation.ReservationCode}\n");

        // Accessibility requests are also optional, so that will also only be shown if the customer chose to input a request
        if (reservation.AccessibilityRequest != "")
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Request");
            Console.ResetColor();
            Console.WriteLine($" {MovieLogic.SpliceText(reservation.AccessibilityRequest, " ")}");
        } 

        if (!reservation.Cancelled)
        {
            // Displays a question with below it the options from ReturnList, the choice between those options is saved
            int option = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "", "\nDo you want to cancel this reservation?", false, false);

            switch (option)
            {
                case 1: // Cancel
                    // reservations can only be cancelled 24+ hours in advance
                    if (reservation.ViewingDate > DateTime.Now.AddHours(24)) // attempt at cancelling is being made 24+ hours in advance
                    {                        
                        CancelResLogic(reservation);

                        // show confirmation that the reservation has been cancelled + return to go back to the menu
                        List<string> emptyList = new List<string>() {"Return"};
                        int option3 = OptionsMenu.DisplaySystem(emptyList, "reservation canceled", "Your reservation has been canceled.", true, false);

                        if (option3 == 1)
                        {
                            return;
                        }
                    }
                    else // attempt at cancelling is being made less than 24 hours in advance
                    {   
                        // show that the reservation cannot be cancelled + return to go back to menu
                        List<string> emptyList = new List<string>();
                        int option3 = OptionsMenu.DisplaySystem(emptyList, "reservation cannot be canceled", "Reservations may only be canceled 24+ hours in advance of the reservation date.", true, true);

                        if (option3 == 1)
                        {   
                            // Goes back to the menu
                            return;
                        }
                    }
                    break;
                
                case 2: // Don't cancel
                    // Goes back to the overview of their reservations
                    break;
            }   
        }
        else
        {
            OptionsMenu.FakeReturn();
            
            return;
        }
        
    }

    public static void CancelResLogic(ReservationsModel reservation)
    {
        List<ReservationsModel> reservations = ReservationsAccess.LoadAll();
        // find index of reservation to remove
        int index = reservations.FindIndex(r => r.ReservationCode == reservation.ReservationCode);
        
        // reservation will show up as "cancelled" in json
        reservations[index].Cancelled = true;
        
        // update the json so that the reservation is no longer there
        ReservationsAccess.WriteAll(reservations);

        RemoveReservationFromCSV(reservation);
    }

    public void PrintReservations() => PrintReservations(_reservations);

    public void PrintReservations(List<ReservationsModel> reservations)
    {   
        while (true)
        {
            // prints an error message if no reservations were found
            if (reservations.Count() == 0)
            {
                List<string> ReturnList = new List<string>() {"Return"};

                int option = OptionsMenu.DisplaySystem(ReturnList, "Reservations", "No reservations were found.", true, false);

                if (option == 1)
                {
                    break;
                }
            }
            else
            {
                int BaseLine = 0;
                int MaxItems = 5;
                int pageNr = 1;

                bool previousButton = false;
                bool nextButton = true;

                while (BaseLine < reservations.Count())
                {   
                    if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.CUSTOMER)
                    {
                        reservations = GetOwnReservations();
                    }
                    else
                    {
                        reservations = ReservationsAccess.LoadAll();
                    }

                    // if there are more than 5 reservations in the list, a "next page" button will be visible and there will be multiple pages available
                    if (BaseLine + 5 > reservations.Count())
                    {
                        MaxItems = reservations.Count() % 5;
                        nextButton = false;
                    }
                    else if (BaseLine + 5 == reservations.Count())
                    {
                        nextButton = false;
                    }   
                    // if there are less than 5 reservations in the list, a "next page" button won't be shown and there will only be one page available
                    else
                    {
                        MaxItems = 5;
                        nextButton = true;
                    }

                    if (BaseLine < 0)
                    {
                        BaseLine = 0;
                        pageNr = 0;
                    }

                    // if any other page than the first one is shown, a "previous page" button will be available to go back
                    if (BaseLine != 0)
                    {
                        previousButton = true;
                    }
                    // on the first page, there will be no "previous page" button available
                    else
                    {
                        previousButton = false;
                    }

                    double totalPages = Math.Ceiling((double)reservations.Count() / 5);

                    List<ReservationsModel> subList = reservations.GetRange(BaseLine, MaxItems);

                    int option = OptionsMenu.ReservationsDisplaySystem(subList, "RESERVATIONS", $"Page {pageNr} (of {totalPages})", true, previousButton, nextButton);

                    if ((option == subList.Count() + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton) && previousButton && !nextButton) || (option == subList.Count() + 1 && previousButton && nextButton))
                    {
                        BaseLine -= 5;
                        pageNr -= 1;
                    }                     
                    else if ((option == subList.Count() + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton) && nextButton))
                    {
                        BaseLine += 5;
                        pageNr += 1;
                    }
                    else if (option == subList.Count() + 1 + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton))
                    {
                        return;
                    } 
                    else
                    {
                        // show the information about the selected reservation from the list
                        ReservationInfo(subList[option - 1]);
                    }
                }
            }
        }
    }
}
