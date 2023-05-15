using System.Text.Json;

class MovieLogic
{
    private List<MovieModel> _movies = new();
    static public MovieModel? CurrentMovie { get; private set; }

    public MovieLogic()
    {
        // uses the loadall function to load the json to the list
        _movies = MovieAccess.LoadAll();
    }

    public void UpdateList(MovieModel mov)
    {
        // finds if there is already a movie with the same id
        int index = _movies.FindIndex(s => s.Id == mov.Id);

        // if the index exists, itll update the movie, otherwhise itll add a new one
        if (index != -1)
        {
            // updates existing movie
            _movies[index] = mov;
        }
        else
        {
            //adds new movie
            _movies.Add(mov);
        }

        // writes the changed data to the json file
        MovieAccess.WriteAll(_movies);
    }

    public void RemoveMovie(int id)
    {
        // finds if there is a movie with the same id
        int index = _movies.FindIndex(s => s.Id == id);

        // removes the movie with that id, and updates the json file
        _movies.Remove(_movies[index]);
        MovieAccess.WriteAll(_movies);
    }

    public MovieModel? GetById(int id)
    {
        // returns the movie data that matches the id
        return _movies.Find(i => i.Id == id);
    }

    public void PrintMovies(List<MovieModel> MovieList, bool IsEmployee = false)
    {
        while (true)
        {
            Console.Clear();

            // prints an error message if nothing was found
            if (MovieList.Count() == 0)
            {
                // list of options that will be displayed
                List<string> ReturnList = new List<string>();

                // the necessary info gets used in the display method
                int option = OptionsMenu.DisplaySystem(ReturnList, "MOVIES", "No movies were found that matched the criteria.");

                // depending on the option that was chosen, it will clear the console and call the right function
                if (option == 1)
                {
                    break;
                }
            }
            else
            {   
                int BaseLine = 0;
                int MaxItems = 5;
                int pageNr = 1;

                bool previousButton = false;
                bool nextButton = true;

                while (BaseLine < MovieList.Count() - 1)
                {
                    if (BaseLine + 5 > MovieList.Count() - 1)
                    {
                        MaxItems = (MovieList.Count() - 1) % 5;
                        nextButton = false;
                    }
                    else
                    {
                        MaxItems = 5;
                        nextButton = true;
                    }
                    
                    if (BaseLine < 0)
                    {
                        BaseLine = 0;
                        pageNr = 0;
                    }

                    if (BaseLine != 0)
                    {
                        previousButton = true;
                    }
                    else
                    {
                        previousButton = false;
                    }

                    // the necessary info gets used in the display method
                    List<MovieModel> subList = MovieList.GetRange(BaseLine, MaxItems);

                    int option = OptionsMenu.MovieDisplaySystem(subList, "MOVIES", $"Page {pageNr}", true, previousButton, nextButton);

                    // depending on the option that was chosen, it will clear the console and call the right function  
                    if ((option == subList.Count() + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton) && previousButton && !nextButton) || (option == subList.Count() + 1 && previousButton && nextButton))
                    {
                        BaseLine -= 5;
                        pageNr -= 1;
                    }                     
                    else if ((option == subList.Count() + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton) && nextButton))
                    {
                        BaseLine += 5;
                        pageNr += 1;
                    }
                    else if (option == subList.Count() + 1 + Convert.ToInt32(previousButton) + Convert.ToInt32(nextButton))
                    {
                        return;
                    } 
                    else
                    {
                        if (IsEmployee)
                        {
                            continue;
                        }
                        else
                        {
                            MovieInfo(subList[option - 1]);
                        }
                    }
                }
            }
        }
    }

    public void PrintMovies() => PrintMovies(_movies);
 
    public void MovieInfo(MovieModel movie)
    {
        while (true)
        {
            Console.Clear();

            // shows the banner and title
            OptionsMenu.Logo(movie.Title);

            // shows all other info
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Genre");
            Console.ResetColor();
            Console.WriteLine($" {movie.Genre}\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Rating");
            Console.ResetColor();
            Console.WriteLine($" {movie.Rating}\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Age Restriction");
            Console.ResetColor();
            Console.WriteLine($" {movie.Age}\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Description");
            Console.ResetColor();
            Console.WriteLine($" {MovieLogic.SpliceText(movie.Description, " ")}\n");

            // list of options that will be displayed
            List<string> ReturnList = new List<string>()
            {
                "Yes",
                "No"
            };

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(ReturnList, "", "\nDo you want to select this movie?", false, false);

            // depending on the option that was chosen, it will clear the console and call the right function
            if (option == 1)
            {
                SeatLogic.SeatSelection(movie);
            }
            else if (option == 2)
            {
                break;
            }
        }
    }

    public List<MovieModel> SortBy(string input, bool ascending)
    {
        List<MovieModel> unsorted = _movies;

        // Check what to sort by per subject
        if (input.ToUpper() == "DATE")
        {
            //This is if the user is a customer, they cannot see movies that have already played anymore.
            DateTime currentDateTime = DateTime.Now;
            if (ascending)
            {
                return unsorted.Where(m => m.PublishDate >= currentDateTime).OrderBy(m => m.PublishDate).ToList();
            }
            if (!ascending)
            {
                return unsorted.Where(m => m.PublishDate >= currentDateTime).OrderByDescending(m => m.PublishDate).ToList();
            }
        }
        else if (input.ToUpper() == "GENRE")
        {
            return (ascending) ? unsorted.OrderBy(m => m.Genre).ToList() : unsorted.OrderByDescending(m => m.Genre).ToList();
        }
        else if (input.ToUpper() == "NAME")
        {
            return (ascending) ? unsorted.OrderBy(m => m.Title).ToList() : unsorted.OrderByDescending(m => m.Title).ToList();
        }
        else if (input.ToUpper() == "RATING")
        {
            return (ascending) ? unsorted.OrderBy(m => m.Rating).ToList() : unsorted.OrderByDescending(m => m.Rating).ToList();
        }
        else if (input.ToUpper() == "PUBLISH")
        {
            return (ascending) ? unsorted.OrderBy(m => m.PublishDate).ToList() : unsorted.OrderByDescending(m => m.PublishDate).ToList();
        }
        return unsorted;
    }

    public List<MovieModel> FilterBy(string? genre, bool? mature)
    {
        // copies the original list
        List<MovieModel> filtered = _movies;

        if (genre != null)
            filtered = _movies.Where(movie => movie.Genre == genre).ToList();
        if (mature == false)
            filtered = filtered.Where(movie => movie.Age < 18).ToList();

        return filtered;
    }

    public List<MovieModel> SearchBy(string query) 
    {
        List<MovieModel> searched = new();
        foreach(MovieModel m in _movies)
            if(m.Title.ToLower().Contains(query.ToLower()) || m.Description.ToLower().Contains(query.ToLower()))
                searched.Add(m);
        
        return searched;
    }

    public static string SpliceText(string inputText, string spacing) 
    {
        int lineLength = 50;
        string[] stringSplit = inputText.Split(' ');
        int charCounter = 0;
        string finalString = "";
    
        for(int i=0; i < stringSplit.Length; i++)
        {
            finalString += stringSplit[i] + " ";
            charCounter += stringSplit[i].Length;
    
            if(charCounter > lineLength)
            {
                finalString += $"\n{spacing}";
                charCounter = 0;
            }
        }
        if (inputText.Length < lineLength)
        {
            finalString += $"\n{spacing}";
        }
        return finalString;
    }

    public static void AddMultipleMoviesJSON(string filename)
{
    if (!File.Exists(filename))
    {
        Console.WriteLine("File not found, press enter to continue");
        Console.ReadLine();
    }

    // Read file with new movies
    string jsonstring = ReadJSON(filename);
    List<MovieModel> newMovieData = new();

    if (!string.IsNullOrEmpty(jsonstring))
    {
        newMovieData = JsonSerializer.Deserialize<List<MovieModel>>(jsonstring)!;
    }

    // Get the original movies that were already in the JSON file
    List<MovieModel> originalMovieData = MovieAccess.LoadAll();

    // Get the maximum ID from the original movies
    int maxId = originalMovieData.Max(movie => movie.Id);

    // Track already existing movies
    List<MovieModel> existingMovies = new List<MovieModel>();

    // Check if the new movies already exist in the original data
    foreach (MovieModel newMovie in newMovieData)
    {
        bool movieExists = originalMovieData.Any(movie => movie.Title == newMovie.Title && movie.Genre == newMovie.Genre && movie.Description == newMovie.Description);
        if (!movieExists)
        {
            // Increment the ID for each new movie to ensure uniqueness
            newMovie.Id = ++maxId;
            originalMovieData.Add(newMovie);
        }
        else
        {
            existingMovies.Add(newMovie);
        }
    }

    // Write new + old movies to file
    MovieAccess.WriteAll(originalMovieData);

    // Display existing movies message
    if (existingMovies.Count > 0)
    {
        Console.WriteLine("The following movies already exist and were not added:\n");
        foreach (MovieModel existingMovie in existingMovies)
        {
            Console.WriteLine($"- Movie ID: {existingMovie.Id}\nTitle: {existingMovie.Title}\nGenre: {existingMovie.Genre}\n");
        }
        Console.WriteLine("\nPress enter to continue");
        Console.ReadLine();
    }
}




    public static string ReadJSON(string filename)
    {
        StreamReader? reader = null;

        try
        {
            // get new movie data from new json file
            reader = new StreamReader(filename);
            return(reader.ReadToEnd());
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found");
            return "";
        }
        finally
        {
            reader?.Close();
            reader?.Dispose();
        }
    }

    public static void AddMultipleMoviesCSV(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found, press enter to continue");
            Console.ReadLine();
        }
        var csvMovies = new List<MovieModel>();
        var filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, filename));

        // read lines from csv file using the filepath
        // skip(1) is so that the header line will be skipped
        foreach (var line in File.ReadLines(filePath).Skip(1))
        {
            // create new MovieModels for each movie in the csv and add to csvMovies list
            csvMovies.Add(new MovieModel
            (
                Convert.ToInt32(line.Split(",")[0]), // id
                line.Split(",")[1], // title
                line.Split(",")[2], // genre
                Convert.ToDouble(line.Split(",")[3]), // rating
                line.Split(",")[4], // description
                Convert.ToInt32(line.Split(",")[5]), // age
                DateTime.Parse(line.Split(",")[6]), // viewing date
                DateTime.Parse(line.Split(",")[7]) // publish date
            ));
        }

        // load movies that were already in the file
        List<MovieModel> originalMovies = MovieAccess.LoadAll();

        // add each new movie to the original list of movies
        foreach(MovieModel movie in csvMovies)
        {
            originalMovies.Add(movie);
        }

        // write original movies + new movies back to file
        MovieAccess.WriteAll(originalMovies);
    }
    protected static List<string> MovieEditorList = new List<string>()
    {
        "Current movies",
        "Add movies",
        "Edit movies",
        "Remove movies"
    };
    protected static List<string> AddMovieList = new List<string>()
    {
        "Single movie entry",
        "JSON File",
        "CSV File"
       
    };
    public void EmployeeMovies()
    {
        while (true)
        {
            Console.Clear();
            int MovieOptions = OptionsMenu.DisplaySystem(MovieEditorList, "Movies", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, true);
            if (MovieOptions == 1)
            {
                Console.Clear();
                PrintMovies(_movies, true);
            }
            else if (MovieOptions == 2)
            {
                Console.Clear();
                int addOptions = OptionsMenu.DisplaySystem(AddMovieList, "Add movies", "To add movies by file, please save the json or csv file in DataSources", true, true);

                if (addOptions == 1)
                {
                    
                }
                else if (addOptions == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the name of the JSON file to add: ");
                    string jsonFile = Console.ReadLine() + "";
                    AddMultipleMoviesJSON(@$"DataSources\{jsonFile}.json");
                }
                else if (addOptions == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the name of the CSV file to add: ");
                    string csvFile = Console.ReadLine() + "";
                    AddMultipleMoviesJSON(@$"DataSources\{csvFile}.csv");
                }
            }
            else if (MovieOptions == 3)
            {
                Console.Clear();
                Console.WriteLine("Not yet implemented");
            }
            else if (MovieOptions == 4)
            {
                Console.Clear();
                Console.WriteLine("Not yet implemented");
            }
            else if (MovieOptions == 5)
            {
                break;
            }
        }
    }
}