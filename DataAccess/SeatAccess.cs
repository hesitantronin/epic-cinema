using System.Text.RegularExpressions;

static class SeatAccess
{
    public static void PrintAuditorium(string auditoriumPath)
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, auditoriumPath));

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
                    if (values[i] == "0")
                    {
                        if (i % 14 != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
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

    public static void PrintAuditorium(string[][] data)
    {
        // Loop through each row of data and print it to the console
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                Console.Write("{0,3}", data[i][j]);
            }
            Console.WriteLine();
        }
    }

    public static string[][] LoadAuditorium(string auditoriumPath)
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, auditoriumPath));

        // Initialize the jagged array with the header row
        string[][] data = new string[1][];

        // Open the CSV file using a StreamReader
        using (var reader = new StreamReader(path))
        {
            // Read the header line
            var header = reader.ReadLine();

            // Split the header line into column names
            var columns = header.Split(',');

            // Add the header row to the data array
            data[0] = columns;

            int linecounter = 0;

            // Read and add each data row to the array
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                Array.Resize(ref data, data.Length + 1);
                data[data.Length - 1] = values;
                linecounter += 1;

                if (linecounter > 17)
                {
                    break;
                }
            }
        }

        // Return the completed data array
        return data;
    }

    public static void UpdateSeatValue(string[][] seatArray, string ID, string newValue)
    {
        // Split ID into the letter and the number
        char letterOfID = ID[0];
        int numberOfID = Convert.ToInt32(ID[1..]);

        // Find the column index corresponding to the letter in the ID
        int columnIndex = -1;

        for (int i = 0; i < seatArray[0].Length; i++)
        {
            if (seatArray[0][i] == Convert.ToString(letterOfID))
            {
                columnIndex = i;
                break;
            }
        }

        // Find the row index corresponding to the number in the ID
        int rowIndex = -1;
        for (int i = 1; i < seatArray.Length; i++)
        {
            for (int j = 0; j < seatArray[i].Length; j++)
            {
                if (!string.IsNullOrEmpty(seatArray[j][0]))
                {
                    if (Convert.ToInt32(seatArray[j][0]) == numberOfID)
                    {
                        rowIndex = j;
                        break;
                    }
                }
            }
        }

        // If the row index is still -1, the ID was not found in the array
        if (rowIndex == -1)
        {
            Console.WriteLine("Seat not found");
            return;
        }

        // Update the value at the corresponding row and column index
        seatArray[rowIndex][columnIndex] = newValue;
    }

    public static void WriteToCSV(string[][] arrayData, string CSVToWriteTo)
    {
        using (StreamWriter writer = new StreamWriter(CSVToWriteTo, false))
        {
            foreach (string[] row in arrayData)
            {
                writer.WriteLine(string.Join(",", row));
            }
        }
    }


    public static string NewAuditorium(MovieModel movie)
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/TestAuditorium/Plattegrond.csv"));
        string templatePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/TestAuditorium/Plattegrond.csv"));
        
        // Remove all spaces and special characters from the movie title to avoid conflict with names
        string movieTitle = Regex.Replace(movie.Title, @"[^0-9a-zA-Z\._]", string.Empty);

        // Copy the template auditorium into a new file named after the movie
        File.Copy(templatePath, $@"DataSources/MovieAuditoriums/ID_{movie.Id}_{movieTitle}.csv");

        return $@"DataSources/MovieAuditoriums/{movie.Title}{movie.Id}.csv";
    }
}