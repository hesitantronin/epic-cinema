using System.Text.Json;

class ReservationsLogic
{
    private List<ReservationsModel> _reservations = new List<ReservationsModel>();

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
        _reservations.Add(reservation);
        ReservationsAccess.WriteAll(_reservations);
    }

    public void RemoveReservationFromAccount()
    {
        AccountsLogic accountsLogic = new AccountsLogic();
        AccountsLogic.CurrentAccount.Movie = null;
        AccountsLogic.CurrentAccount.CateringReservation = new Dictionary<string, string>();
        AccountsLogic.CurrentAccount.SeatReservation = new List<SeatModel>();
        AccountsLogic.CurrentAccount.AccessibilityRequest = "";
        accountsLogic.UpdateList(AccountsLogic.CurrentAccount);
    }

    public void ViewOwnReservations()
    {   
        var CurrentAcc = AccountsLogic.CurrentAccount; // less typing later

        // Find a customers own reservations when they're logged in, and display them
        foreach (ReservationsModel reservation in _reservations)
        {
            if (reservation.FullName == CurrentAcc.FullName && reservation.EmailAddress == CurrentAcc.EmailAddress)
            {
                Console.WriteLine($"NAME: {reservation.FullName}");
                Console.WriteLine($"MOVIE: {reservation.Movie}");
                Console.WriteLine("SEAT(S):");
                foreach (SeatModel seat in reservation.SeatReservation)
                {
                    Console.WriteLine($"    {seat.Id} ({seat.SeatTypeName}\n");
                }
                Console.WriteLine($"DATE AND TIME: {reservation.Movie.ViewingDate}");
                Console.WriteLine("CATERING:");
                foreach (var menuItem in reservation.CateringReservation)
                {
                    Console.WriteLine($"    {menuItem.Key}: x{menuItem.Value}\n");
                }
                Console.WriteLine($"RESERVATION CODE: {reservation.ReservationCode}\n");
                Console.WriteLine($"REQUEST: {reservation.AccessibilityRequest}\n");
                Console.WriteLine("--------------------------------------------------");
            }
        }
    }
}
