class EmployeeMenu
{
    public static MovieLogic movie = new MovieLogic();
    public static CateringLogic food = new CateringLogic();
    public static AccountsLogic account = new AccountsLogic();

    protected static List<string> StartList = new List<string>()
    {
        "Movies and seats",
        "Catering",
        "Global seat data"
    };
    public static void StartEmployee()
    {
        while (true)
        {
            Console.Clear();

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(StartList, "Editing menu", "Select what category you want to edit.", true, true);

            // depending on the option that was chosen, it will clear the console and call the right function
            if (option == 1)
            {
                Console.Clear();
                movie.EmployeeMovies();
            } 
            
            else if (option == 2)
            {
                food.EmployeeCatering();
            }

            else if (option == 3)
            {
                Console.Clear();
                EditGlobalSeatData();
            }

            if (option == 4)
            {
                break;
            }
        }
    }

    public static void EditGlobalSeatData()
    {
        List<string> optionList = new List<string>()
        {
            "Price Range 1",
            "Price Range 2",
            "Price Range 3"
        };

        var SeatData = SeatAccess.LoadGlobalSeatData();
        (string, double) seatType = new();

        while (true)
        {
            
            Console.Clear();
            int option = OptionsMenu.DisplaySystem(optionList, "Editing menu", "Select what category you want to edit.", true, true);

            int key = option;

            if (key == 4)
            {
                return;
            }
            
            seatType = SeatData[key];
            break;
        }

        Console.WriteLine($"Current state of Seat Type 1:\n\nName: {seatType.Item1}\nPrice: {seatType.Item2}");
        

        Console.ReadLine();

    }
}
