

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

            // Read and print each data row
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                foreach (var value in values)
                {
                    Console.Write("{0,3}", value);
                }
                Console.WriteLine();
            }
        }
    }
}