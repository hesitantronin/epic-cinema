class EmployeeMenu
{
    public static MovieLogic movie = new MovieLogic();
    protected static List<string> StartList = new List<string>()
    {
        "Movies",
        "Catering",
        "Seats",
        "Exit"
    };
    protected static List<string> MovieEditorList = new List<string>()
    {
        "Current movies",
        "Edit movies",
        "Remove movies",
        "Exit"
    };
    protected static List<string> MovieAdd = new List<string>()
    {
        "Current movies",
        "Edit movies",
        "Remove movies",
        "Exit"
    };
    protected static List<string> CateringEditorList = new List<string>()
    {
        "Menu",
        "Edit menu",
        "Remove items",
        "Exit"
    };
    protected static List<string> SeatEditorList = new List<string>()
    {
        "Current seats",
        "Edit seat prices",
        "remove ?",
        "Exit"
    };

    public static void StartEmployee()
    {
        Console.Clear();

        // the necessary info gets used in the display method
        int option = OptionsMenu.DisplaySystem(StartList, "Employee Editor menu", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, false);

        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 1)
        {
            Console.Clear();
            Movies();
        }
        else if (option == 2)
        {
            Console.Clear();
        }
        else if (option == 3)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
        }
        if (option != 4)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
        }
    }
    private static void Movies()
    {
        Console.Clear();
        int MovieOptions = OptionsMenu.DisplaySystem(MovieEditorList, "Movie menu", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, false);
        if (MovieOptions == 1)
        {
            Console.Clear();
            movie.PrintMovies();
        }
        else if (MovieOptions == 2)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
        }
        else if (MovieOptions == 3)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
        }
        else if (MovieOptions != 4)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
        }
    }

}
