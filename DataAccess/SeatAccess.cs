using System.Text.RegularExpressions;
using Newtonsoft.Json;

static class SeatAccess
{
    public static void PrintAuditorium(string[][] auditorium)
    {
        // Print the column names
        foreach (var column in auditorium[0])
        {
            Console.Write("{0,3}", column);
        }
        Console.WriteLine();

        int linecounter = 0;

        // Read and print each data row
        foreach (var row in auditorium.Skip(1))
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (linecounter == 17)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                if (row[i] == "0")
                {
                    if (i % 14 != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }
                if (row[i] == "1")
                {
                    if (i % 14 != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                    }
                }
                else if (row[i] == "2")
                {
                    if (i % 14 != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                }
                else if (row[i] == "3")
                {
                    if (i % 14 != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                }
                else if (row[i] == "4")
                {
                    if (i % 14 != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                }
                if (row[i] != "")
                {
                    if (i == 0 || linecounter > 14)
                    {
                        Console.Write("{0,3}", row[i]);
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

    public static Dictionary<int, (string, double)> LoadGlobalSeatData()
    {
        Dictionary<int, (string, double)> SeatData = new();
        string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"DataSources/seatPrices.json"));

        // Read the JSON file into a string
        string json = File.ReadAllText(path);

        // Deserialize the JSON string into a dictionary
        if (!string.IsNullOrEmpty(json))
        {
            return JsonConvert.DeserializeObject<Dictionary<int, (string, double)>>(json);
        } 

        else return new Dictionary<int, (string, double)>();
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

    public static string FindSeatValueInArray(string[][] seatArray, string ID)
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
            return "0";
        }

        return seatArray[rowIndex][columnIndex];

    }

    public static string FindDefaultSeatValueArray(string ID)
    {
        string [][] seatArray = LoadAuditorium(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/DefaultAuditorium/Auditorium.csv"));

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
            return "";
        }

        else if (seatArray[rowIndex][columnIndex] != "")
        {
            return seatArray[rowIndex][columnIndex];
        }

        return "";

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


    public static void NewAuditorium(MovieModel movie)
    {
        string templatePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/DefaultAuditorium/Auditorium.csv"));
        
        // Strings for converting the datetime to a format which is useable in the title of the CSV
        string[] movieViewingDate1 = movie.ViewingDate.ToString().Split(" ");
        string movieViewingDate2 = string.Join(" ", movieViewingDate1[1].Replace(":", "_"));
        string movieViewingDate3 = string.Join(" ", movieViewingDate1[0].Replace("/", "_"));

        // Remove all spaces and special characters from the movie title to avoid conflict with names
        string movieTitle = Regex.Replace(movie.Title, @"[^0-9a-zA-Z\._]", string.Empty);

        if (!Directory.Exists($@"DataSources/MovieAuditoriums/{movieTitle}"))
        {
            Directory.CreateDirectory($@"DataSources/MovieAuditoriums/{movieTitle}");
        }

        // Copy the template auditorium into a new file named after the movie
        File.Copy(templatePath, $@"DataSources/MovieAuditoriums/{movieTitle}/ID_{movie.Id}_{movieTitle}_{movieViewingDate3 + "_" + movieViewingDate2}.csv");
    }
}