using System.Globalization;
using System.Text;

class CateringMenu
{
    public static CateringLogic cateringlogic = new CateringLogic();
    static public void Start()
    {
        while (true)
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
            int option = OptionsMenu.DisplaySystem(OptionList, "CATERING");

            // depending on the option that was chosen, it will clear the console and call the right function
            if (option == 1)
            {
                Console.Clear();
                Sort();
            }
            else if (option == 2)
            {
                Console.Clear();
                Filter();
            }
            else if (option == 3)
            {
                Console.Clear();
                Search();
            }
            else if (option == 4)
            {
                Console.Clear();
                cateringlogic.PrintMenu();
            }

            else if (option == 5)
            {
                break;
            }
        }
    }

    static public void Sort(bool IsEmployee = false)
    {
        while (true)
        {
            Console.Clear();

            // list of options to sort by
            List<string> OptionList = new List<string>()
            {
                "Name",
                "Type",
                "Price"
            };

            // display the options listed above so a choice can be made
            int option = OptionsMenu.DisplaySystem(OptionList, "SORT MENU");

            if (option == 4) // 4 = return/go back
            {
                break;
            }
            else
            {
                Console.Clear();

                List<string> AscDescList = new List<string>()
                {
                    "Ascending",
                    "Descending"
                };

                // show ascending/descending choice
                int option2 = OptionsMenu.DisplaySystem(AscDescList, "SORT MENU");

                bool ascending = true;

                if (option2 == 2) // descending
                {
                    ascending = false;
                }

                if (option2 != 3) // 3 = return/go back
                {
                    Console.Clear();

                    // the menu will be sorted & displayed depending on what was chosen (name, type, price)
                    switch (option)
                    {
                        case 1: // sort based on name
                            cateringlogic.PrintMenu(cateringlogic.SortBy("NAME", ascending), IsEmployee);
                            break;
                        case 2: // type
                            cateringlogic.PrintMenu(cateringlogic.SortBy("TYPE", ascending), IsEmployee);
                            break;
                        case 3: // price
                            cateringlogic.PrintMenu(cateringlogic.SortBy("PRICE", ascending), IsEmployee);
                            break;
                    }
                }
            }
        }
    }

    static public void Filter(bool IsEmployee = false)
    {
        while (true)
        {
            Console.Clear();

            // list of possible types
            List<string> types = new List<string>()
            {
                "Snack",
                "Beverage",
                "Candy"
            };

            int option = OptionsMenu.DisplaySystem(types, "FILTER MENU");

            if (option == 4) // 4 = return/go back
            {
                break;
            }            
            else
            {
                Console.Clear();

                // filter menu based on choice (snack, beverage, candy) and display menu
                switch (option)
                {
                    case 1:
                        cateringlogic.PrintMenu(cateringlogic.FilterBy("Snack"), IsEmployee);
                        break;
                    case 2:
                        cateringlogic.PrintMenu(cateringlogic.FilterBy("Beverage"), IsEmployee);
                        break;
                    case 3:
                        cateringlogic.PrintMenu(cateringlogic.FilterBy("Candy"), IsEmployee);
                        break;
                }
            }
        }
    }

    static public void Search(bool IsEmployee = false)
    {
        Console.Clear();
        Console.CursorVisible = true;

        // shows banner and title
        OptionsMenu.Logo("SEARCH MENU");

        // asks for an input to search for and searches for it
        Console.WriteLine("Search: ");
        string query = Console.ReadLine() + "";

        cateringlogic.PrintMenu(cateringlogic.SearchBy(query), IsEmployee);

        Console.CursorVisible = false;
    }

    static public CateringModel? SearchId()
    {
        Console.Clear();
        Console.CursorVisible = true;

        int id;
        while (true)
        {
            // shows banner and title
            OptionsMenu.Logo("SEARCH MENU");

            // asks for an input to search for and searches for it
            Console.WriteLine("Search: ");
            string query = Console.ReadLine() + "";

            if (int.TryParse(query.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out id))
            {
                break;
            }

            Console.WriteLine("\nInvalid ID. Please enter a valid number.");
            
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();
        }
        CateringLogic cl = new();
        return cl.GetById(id);

    }
}
