public static class SeatsMenu
{
    // legend for customers
    public static void SeatLegend(MovieModel movie)
    {
        Dictionary<int, (string, double)> seatdata = SeatAccess.LoadGlobalSeatData();

        Console.WriteLine();
        Console.WriteLine($"Movie: {movie.Title}\nBase Price: € {String.Format("{0:0.00}", movie.MoviePrice)}\n");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - UNAVAILABLE\n");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - {seatdata[1].Item1.ToUpper()}: + € {String.Format("{0:0.00}", seatdata[1].Item2)}\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - {seatdata[2].Item1.ToUpper()}: + € {String.Format("{0:0.00}", seatdata[2].Item2)}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - {seatdata[3].Item1.ToUpper()}: + € {String.Format("{0:0.00}", seatdata[3].Item2)}\n");

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - SELECTED SEATS\n\n");
    }

    // legend for heatmap editing
    public static void SeatLegendDefault()
    {
        Dictionary<int, (string, double)> seatdata = SeatAccess.LoadGlobalSeatData();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - {seatdata[1].Item1.ToUpper()}: + €{seatdata[1].Item2}\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - {seatdata[2].Item1.ToUpper()}: + €{seatdata[2].Item2}\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - {seatdata[3].Item1.ToUpper()}: + €{seatdata[3].Item2}\n\n");
    }
}