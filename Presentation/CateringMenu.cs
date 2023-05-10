using System.Text;

class CateringMenu
{
    public static CateringLogic cateringlogic = new CateringLogic();
    static public void Start()
    {
        // list of options to display
        List<string> OptionList = new List<string>()
        {
            "Sort",
            "Filter",
            "Search",
            "Show Whole Menu"
        };

        // the necessary info gets used in the display method
        int option = OptionsMenu.DisplaySystem(OptionList, "MOVIES");  

        // depending on the option that was chosen, it will clear the console and call the right function
        if (option == 1)
        {
            Console.Clear();
            //Sort();
        }
        else if (option == 2)
        {
            Console.Clear();
            //Filter();
        }
        else if (option == 3)
        {
            Console.Clear();
            //Search();
        }
        else if (option == 4)
        {
            Console.Clear();
            cateringlogic.PrintMenu();
        }

        else if (option == 5)
        {
            Console.Clear();
            OptionsMenu.Start();
        }
    }

}
