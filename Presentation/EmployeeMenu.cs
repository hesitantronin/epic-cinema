class EmployeeMenu
{
    public static MovieLogic movie = new MovieLogic();
    protected static List<string> StartList = new List<string>()
    {
        "Movies",
        "Catering",
        "Seats"
    };
    protected static List<string> MovieAdd = new List<string>()
    {
        "Current movies",
        "Edit movies",
        "Remove movies"
    };
    protected static List<string> CateringEditorList = new List<string>()
    {
        "Menu",
        "Edit menu",
        "Remove items"
    };
    protected static List<string> SeatEditorList = new List<string>()
    {
        "Current seats",
        "Edit seat prices",
        "remove ?",
    };

    public static void StartEmployee()
    {
        while (true)
        {
            Console.Clear();

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(StartList, "Edit menu", "Select what category you want to edit.", true, true);

            // depending on the option that was chosen, it will clear the console and call the right function
            if (option == 1)
            {
                Console.Clear();
                movie.EmployeeMovies();
            }
            else if (option == 2)
            {
                //catering
            }
            else if (option == 3)
            {
                //seats
            }
            if (option == 4)
            {
                break;
            }
        }
    }
}
