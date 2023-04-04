using System.Text;

static class SeatLogic
{
    public static List<string> SeatsToList()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/TestAuditorium/Plattegrond.csv"));
        List<string> rows = new List<string>();

        // Open the CSV file using a StreamReader
        using (var reader = new StreamReader(path))
        {
            // Read the header line
            var header = reader.ReadLine();

            // Split the header line into column names
            var columns = header.Split(',');

            string head = "";
            // Print the column names
            foreach (var column in columns)
            {
                head += $" {column} \u001b[0m";
            }
            rows.Add(head);
            rows.Add(" ");
            int linecounter = 0;

            // Read and print each data row
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                
                string row = "";
                for (int i = 0; i < values.Count();i ++)
                {
                    string color = "";
                    string reset = "\u001b[0m";
                    if (linecounter == 17)
                    {
                        color = "";
                    }
                    if (values[i] == "1")
                    {
                        if (i % 14 != 0)
                        {
                            color = "\u001b[36m";
                        }
                    }
                    else if (values[i] == "2")
                    {
                        if (i % 14 != 0)
                        {
                            color = "\u001b[33m";
                        }
                    }
                    else if (values[i] == "3")
                    {
                        if (i % 14 != 0)
                        {
                            color = "\u001b[31m";
                        }
                    }
                    else if (values[i] == "4")
                    {
                        if (i % 14 != 0)
                        {
                            color = "\u001b[32m";
                        }
                    }
                    if (values[i] != "")
                    {
                        if (i == 0 || linecounter > 14)
                        {
                            if (values[i] == "10" || values[i] == "11" || values[i] == "12" || values[i] == "13" || values[i] == "14")
                            {
                                row += $"{color} {values[i]}{reset}";
                            }
                            else
                            {
                                row += $"{color} {values[i]} {reset}";
                            }
                        }
                        else
                        {
                            row += $"{color} ■ {reset}";
                        }
                    }
                    else
                    {
                        row += "   ";
                    }
                    Console.ResetColor();
                }
                if (linecounter > 17)
                {
                    break;
                }
                linecounter += 1;
                rows.Add(row);
            }
        }
        return rows;
    }

    public static void PrintSeats()
    {
        List<string> rows = SeatsToList();

        for (int i = 0; i < rows.Count(); i ++)
        {
            Console.WriteLine(rows[i]);
        }
    }

    public static int SeatDisplay()
    {
        // makes the cursor invisible
        Console.Clear();
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;

        List<string> list = SeatsToList();

        // gets the cursor position and sets option to 1
        (int left, int top) = Console.GetCursorPosition();
        int option = 4;
        int optionx = 1;
        string spacing = new String(' ', 9);

        // this is the decorator that will help you see where the cursor is at
        var decorator = "\u001b[32m ■ \u001b[0m";

        // sets a variable for 'key' that will be used later
        ConsoleKeyInfo key;

        // the loop in which an option is chosen from a list
        bool isSelected = false;
        while (!isSelected)
        {
            // sets the cursor to the previously determined location
            Console.SetCursorPosition(left, top);

            // prints the options one by one
            for (int i = 0; i < list.Count(); i++)
            {
                //writes the option and gives it a number
                Console.WriteLine($"{(option == i + 1 ? $"{spacing}{decorator}" : "   ")}{list[i]}\u001b[0m");
            }

            // sees what key has been pressed
            key = Console.ReadKey(false);

            // a switch case that changes the value from 'option', depending on the key input
            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.UpArrow:
                    option = option == 4 ? list.Count() - 3 : option - 1;
                    break;
                    
                // moves one down
                case ConsoleKey.DownArrow:
                    option = option == list.Count() - 3 ? 4 : option + 1;
                    break;

                // if enter is pressed, breaks out of the while loop
                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }

            switch (key.Key)
            {
                // moves one up
                case ConsoleKey.RightArrow:
                    optionx = optionx == 1 ? list.Count() : optionx + 1;
                    spacing = optionx == 1 ? "         " : spacing += "   ";
                    break;
                    
                // moves one down
                case ConsoleKey.LeftArrow:
                    optionx = optionx == list.Count() ? 1 : optionx - 1;
                    spacing = optionx == list.Count() ? "         " : spacing.Replace("   ", "");
                    break;

                // if enter is pressed, breaks out of the while loop
                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }
        }
        Console.CursorVisible = true;
        return option;
    }
}