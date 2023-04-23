using System.Text.RegularExpressions;

static class SeatLogic
{
    public static void SeatSelection()
    {
        string selectedChair = string.Empty;
        List<SeatModel> chosenSeats = new();
        List<SeatModel> unavailableSeats = new();

        string pattern = @"^[a-l]([1-9]|1[0-4])$";

        while (true)
        {
            OptionsMenu.Logo("Seat selection");

            SeatAccess.LoadAuditorium();

            bool validID = false;
            while (true)
            {
                Console.WriteLine("Pick a chair");
                selectedChair = Console.ReadLine();

                if (!string.IsNullOrEmpty(selectedChair))
                {
                    if (Regex.IsMatch(selectedChair, pattern, RegexOptions.IgnoreCase))
                    {
                        Console.WriteLine("yippie");
                        break;
                    }
                }
                Console.WriteLine("That chair is not available");
            }
        }
    }
}