using System.Text.RegularExpressions;

static class SeatLogic
{
    public static void SeatSelection(MovieModel movie)
    {
        List<string> selectedChairs = new();
        string currentlySelectedChair = string.Empty;

        string movieTitle = Regex.Replace(movie.Title, @"[^0-9a-zA-Z\._]", string.Empty);
        string pathToCsv = $@"DataSources/MovieAuditoriums/ID_{movie.Id}_{movieTitle}.csv";

        // Check for a valid ID
        string pattern = @"^[a-l]([1-9]|1[0-4])$";

        if (!File.Exists(pathToCsv))
        {
            SeatAccess.NewAuditorium(movie);
        }

        while (true)
        {
            // Visualisation of the menu
            Console.Clear();
            OptionsMenu.Logo("Seat selection");
            SeatAccess.PrintAuditorium(pathToCsv);
            string[][] auditoriumArray = SeatAccess.LoadAuditorium(pathToCsv);

            Console.WriteLine("Please select a seat");
            currentlySelectedChair = Console.ReadLine();

            if (!string.IsNullOrEmpty(currentlySelectedChair))
            {
                if (Regex.IsMatch(currentlySelectedChair, pattern, RegexOptions.IgnoreCase))
                {
                    if (!selectedChairs.Contains(currentlySelectedChair))
                    {

                    }
                    selectedChairs.Add(currentlySelectedChair.ToUpper());
                    Console.WriteLine($"You've Selected seat(s) {String.Join(", ", selectedChairs)}, do you wish to select any more?");
                    Console.WriteLine("Yes/No");
                    string answer = Console.ReadLine().ToUpper();
                    bool no = "NO".Equals(answer);

                    if (no)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("That chair is not available");
                }
            }

            else
            {
                Console.WriteLine("Please fill in the ID of a chair");
            }

            // Update CSV with selected seats to show which seats are selected during the process
            foreach (string seat in selectedChairs)
            {
                SeatAccess.UpdateSeatValue(auditoriumArray, seat, "4");
            }

            SeatAccess.WriteToCSV(auditoriumArray, pathToCsv);
        }

        Console.WriteLine("Are you ok with the current seat selection");

        // Going to food reservations
    }
}