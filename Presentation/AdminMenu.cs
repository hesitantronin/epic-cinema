using System.Text;

static class AdminMenu
{
    public static void Start()
    {
        Console.Clear();

        // list of options that will be displayed
        List<string> StartList = new List<string>()
        {
            "Create admin account?",
            "Change Cinema font?",
            "IDK YET MAN???",
            "Exit"
        };

        // the necessary info gets used in the display method
        int option = OptionsMenu.DisplaySystem(StartList, "START", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, false);

        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 1)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
        }
        else if (option == 2)
        {
            Console.Clear();
            Console.WriteLine("Not yet implemented");
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
}