static class SeatsMenu
{
    public static void SeatLegend(MovieModel movie)
    {
        Console.WriteLine();
        Console.WriteLine($"{movie.Title} ({movie.MoviePrice})");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - UNAVAILABLE\n");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - OUTER RING\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - MIDDLE RING (+ amount)\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - INNER RING (+ amount)\n");

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("■");
        Console.ResetColor();
        Console.Write($" - SELECTED SEATS\n\n");
    }
}