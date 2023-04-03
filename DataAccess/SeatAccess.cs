

static class SeatAccess
{
    public static void LoadAuditorium()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/TestAuditorium/Plattegrond.csv"));

        // Open the CSV file using a StreamReader
        using (var reader = new StreamReader(path))
        {
            // Read the header line
            var header = reader.ReadLine();

            // Split the header line into column names
            var columns = header.Split(',');

            // Print the column names
            foreach (var column in columns)
            {
                Console.Write("{0,3}", column);
            }
            Console.WriteLine();

            int linecounter = 0;

            // Read and print each data row
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                
                for (int i = 0; i < values.Count();i ++)
                {
                    if (linecounter == 17)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    if (values[i] == "1")
                    {
                        if (i % 14 != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                        }
                    }
                    else if (values[i] == "2")
                    {
                        if (i % 14 != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                    }
                    else if (values[i] == "3")
                    {
                        if (i % 14 != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                    }
                    if (values[i] != "")
                    {
                        if (i == 0 || linecounter > 14)
                        {
                            Console.Write("{0,3}", values[i]);
                        }
                        else
                        {
                            Console.Write("{0,3}", "■");
                        }
                    }
                    else
                    {
                        Console.Write("{0,3}", " ");
                    }
                    Console.ResetColor();
                }
                if (linecounter > 17)
                {
                    break;
                }
                linecounter += 1;
                Console.WriteLine();
            }
        }
    }
}