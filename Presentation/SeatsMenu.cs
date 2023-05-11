static class SeatsMenu
{
    public static void SeatLegend()
    {                        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - UNAVAILABLE\n");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - PRICERANGE 1\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - PRICERANGE 2\n");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - PRICERANGE 3\n");

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("■");
        Console.ResetColor();
        Console.Write(" - SELECTED SEATS\n");
    }
}