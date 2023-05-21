class EmployeeMenu
{
    public static MovieLogic movie = new MovieLogic();
    public static CateringLogic food = new CateringLogic();
    public static AccountsLogic account = new AccountsLogic();

    protected static List<string> StartList = new List<string>()
    {
        "Movies and seats",
        "Catering"
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
                Console.Clear();
                food.EmployeeCatering();
            }
            if (option == 3)
            {
                break;
            }
        }
    }
}
