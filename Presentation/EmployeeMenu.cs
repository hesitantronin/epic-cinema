using System.Globalization;
using System.Text.Json;

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
        while (true)
        {
            var SeatData = SeatAccess.LoadGlobalSeatData();

            List<string> optionList = new List<string>()
            {
                $"Range 1: {SeatData[1].Item1} (+ {SeatData[1].Item2})",
                $"Range 2: {SeatData[2].Item1} (+ {SeatData[2].Item2})",
                $"Range 3: {SeatData[3].Item1} (+ {SeatData[3].Item2})"
            };

            int option = OptionsMenu.DisplaySystem(optionList, "edit seat info", "Select what category you want to edit.", true, true);

            int key = option;

            if (key == 4)
            {
                return;
            }
            else
            {
                List<string> change = new() {$"Name: {SeatData[key].Item1}", $"Price: {SeatData[key].Item2}"};
                while (true)
                {
                    int option2 = OptionsMenu.DisplaySystem(change, "edit seat info", "Select what field you want to edit.", true, true);
                    if (option2 == 1)
                    {
                        while (true)
                        {
                            OptionsMenu.Logo("edit seat info");
                            Console.Write("New name: ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newName))
                            {
                                SeatData[key] = (newName, SeatData[key].Item2);
                                break;
                            }

                            Console.WriteLine("\nThe name can't be empty.");
                            
                            // prints a fake return option hehe
                            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                        
                            // actually returns you to the main menu
                            Console.ReadLine();
                        }
                    }
                    else if (option2 == 2)
                    {
                        while (true)
                        {
                            double Price;
                            OptionsMenu.Logo("edit seat info");
                            Console.Write("New price: ");
                            string newPrice = Console.ReadLine();

                            if (double.TryParse(newPrice.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Price))
                            {
                                SeatData[key] = (SeatData[key].Item1, Price);

                                break;
                            }

                            Console.WriteLine("Invalid price. Please enter a valid decimal number.");
                            
                            // prints a fake return option hehe
                            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                        
                            // actually returns you to the main menu
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        break;
                    }

                    SeatAccess.WriteGlobalSeatData(SeatData);
            
                    OptionsMenu.Logo("Seat data updated");

                    Console.WriteLine("Seat Data updated successfully.");
                    
                    // prints a fake return option hehe
                    Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
                
                    // actually returns you to the main menu
                    Console.ReadLine();
                    break;
                }
            }
        }
    }
    

}
