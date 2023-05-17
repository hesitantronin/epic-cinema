using System.Text.Json;
using System.Globalization;

class MovieLogic
{
    private List<MovieModel> _movies = new();
    static public MovieModel? CurrentMovie { get; private set; }

    public MovieLogic()
    {
        LoadMovies();
    }

    private void LoadMovies()
    {
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

    public MovieModel? GetById(int id)
    {
        // returns the movie data that matches the id
        return _movies.Find(i => i.Id == id);
    }

    public void PrintMovies(List<MovieModel> MovieList, bool IsEmployee = false, bool IsEdit = false)
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
                        if (IsEmployee && IsEdit)
                        {
                            continue;
                        }
                        else if (IsEmployee && !IsEdit)
                        {
                            EditMovie(subList[option - 1]);
                            return;
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
    public static void EditMovie(MovieModel movie)
    {
        Console.Clear();

        // shows the banner and title
        OptionsMenu.Logo(movie.Title);

        List<string> movieInfoList = new List<string>();
        movieInfoList.Add($"Title: {movie.Title}");
        movieInfoList.Add($"Genre: {movie.Genre}");
        movieInfoList.Add($"Rating: {movie.Rating}");
        movieInfoList.Add($"Age Restriction: {movie.Age}");
        movieInfoList.Add($"Description: {MovieLogic.SpliceText(movie.Description, " ")}");
        movieInfoList.Add($"Viewing Date: {movie.ViewingDate}");
        movieInfoList.Add($"Publish Date: {movie.PublishDate}");
        int edit = OptionsMenu.DisplaySystem(movieInfoList, "Edit existing movie", "Use ⬆ and ⬇ to navigate and press Enter to select what you would like to edit:", true, true);
        if (edit == 1)
        {
            Console.Write("New Title: ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTitle))
            {
                movie.Title = newTitle;
            }
        }
        else if (edit == 2)
        {
            Console.Write("New genre: ");
            string newGenre = Console.ReadLine();
            if (!string.IsNullOrEmpty(newGenre))
            {
                movie.Genre = newGenre;
            }
        }
        else if (edit == 3)
        {
            double rating;
            while (true)
            {
                Console.Write("Rating: ");
                string ratingInput = Console.ReadLine();

                if (double.TryParse(ratingInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rating))
                {
                    movie.Rating = rating;
                    break;
                }

                Console.WriteLine("Invalid rating. Please enter a valid decimal number.");
            }
        }
        else if (edit == 4)
        {
            int age;
            while (true)
            {
                Console.Write("Age: ");
                string newAgeInput = Console.ReadLine();

                if (int.TryParse(newAgeInput, out age) && age >= 0)
                {
                    movie.Age = age;
                    break;
                }

                Console.WriteLine("Invalid age. Please enter a valid number.");
            }
        }
        else if (edit == 5)
        {
            Console.Write("Description: ");
            string newDescription = Console.ReadLine();
            if (!string.IsNullOrEmpty(newDescription))
            {
                movie.Description = newDescription;
            }
        }
        else if (edit == 6)
        {
            DateTime viewingDate;
            while (true)
            {
                Console.WriteLine("Date? (MM-DD-YYYY)");
                string[] date = Console.ReadLine().Split("-");

                if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }

                Console.WriteLine("Time? (ex. 16:30)");
                string[] times = Console.ReadLine().Split(":");

                if (times.Length != 2 || !int.TryParse(times[0], out int hour) || !int.TryParse(times[1], out int minute))
                {
                    Console.WriteLine("Invalid time format. Please try again.");
                    continue;
                }

                try
                {
                    viewingDate = new DateTime(year, month, day, hour, minute, 0);
                    movie.ViewingDate = viewingDate;
                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid date or time. Please try again.");
                }
            }
        }
        else if (edit == 7)
        {
            DateTime publishDate;
            while (true)
            {
                Console.WriteLine("Date? (MM-DD-YYYY)");
                string[] date = Console.ReadLine().Split("-");

                if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }

                Console.WriteLine("Time? (ex. 16:30)");
                string[] times = Console.ReadLine().Split(":");

                if (times.Length != 2 || !int.TryParse(times[0], out int hour) || !int.TryParse(times[1], out int minute))
                {
                    Console.WriteLine("Invalid time format. Please try again.");
                    continue;
                }

                try
                {
                    publishDate = new DateTime(year, month, day, hour, minute, 0);
                    movie.PublishDate = publishDate;
                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid date or time. Please try again.");
                }
            }
        }
        else
        {
            return;
        }
        // Load all movies from the JSON file
        List<MovieModel> movies = MovieAccess.LoadAll();

        // Find the index of the movie to update
        int index = movies.FindIndex(m => m.Id == movie.Id);

        if (index != -1)
        {
            // Update the movie in the list
            movies[index] = movie;

            // Write the updated movie list to the JSON file
            MovieAccess.WriteAll(movies);
            Console.Clear();
            Console.WriteLine("Movie updated successfully.\n\n Press enter to continue.");
            Console.ReadLine();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Movie not found.\n\n Press enter to continue.");
            Console.ReadLine();
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
    public static void AddMultipleMoviesJSON()
    {
        string filename;
        do
        {
            Console.WriteLine("Please save the JSON file in the DataSources folder.\n\n");
            Console.WriteLine("Enter the JSON file name without '.json' (or press enter to return): ");
            string jsonFile = Console.ReadLine() + "";
            filename = @$"DataSources\{jsonFile}.json";

            if (jsonFile == "")
            {
                return;
            }
            else if (!File.Exists(filename))
            {
                Console.Clear();
                Console.WriteLine("File not found, please try again");
            }
        } while (!File.Exists(filename));

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
        else
        {
            Console.Clear();
            Console.WriteLine("Movies have been succesfully added.\n\nPress enter to continue");
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

    public static void AddMultipleMoviesCSV()
    {
        string filename;
        do
        {
            Console.WriteLine("Please save the CSV file in the DataSources folder.\n\n");
            Console.WriteLine("Enter the CSV file name without '.csv' (or press enter to return): ");
            string csvFile = Console.ReadLine() + "";
            filename = @$"DataSources\{csvFile}.csv";

            if (csvFile == "")
            {
                return;
            }
            else if (!File.Exists(filename))
            {
                Console.Clear();
                Console.WriteLine("File not found, please try again");
            }
        } while (!File.Exists(filename));
       
        var csvMovies = new List<MovieModel>();
        var filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, filename));


        // Read lines from the CSV file using the file path
        // Skip(1) is used to skip the header line
        foreach (var line in File.ReadLines(filePath).Skip(1))
        {
            // Create new MovieModels for each movie in the CSV and add to csvMovies list
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

        // Load movies that were already in the file
        List<MovieModel> originalMovies = MovieAccess.LoadAll();

        // Get the maximum ID from the existing movies
        int maxId = originalMovies.Max(movie => movie.Id);

        // Track already existing movies
        List<MovieModel> existingMovies = new List<MovieModel>();

        // Check if the new movies already exist in the original data
        foreach (MovieModel movie in csvMovies)
        {
            bool movieExists = originalMovies.Any(m => m.Title == movie.Title && m.Genre == movie.Genre && m.Description == movie.Description);
            if (!movieExists)
            {
                // Increment the ID for each new movie to ensure uniqueness
                movie.Id = ++maxId;
                originalMovies.Add(movie);
            }
            else
            {
                existingMovies.Add(movie);
            }
        }
        
        // Write original movies + new movies back to file
        MovieAccess.WriteAll(originalMovies);

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
        else
        {
            Console.Clear();
            Console.WriteLine("Movies have been succesfully added.\n\nPress enter to continue");
            Console.ReadLine();
        }
    }
    private static void RemoveMovie()
    {
        List<MovieModel> movies = MovieAccess.LoadAll();

        // Retrieve and display the movies
        List<string> movieList = new List<string>();
        foreach (MovieModel movie in movies)
        {
            string MovieInfo = $"ID: {movie.Id}\nTitle: {movie.Title}\nGenre: {movie.Genre}\nAge: {movie.Age}\nViewing Date: {movie.ViewingDate}\nPublish Date: {movie.PublishDate}\n";
            movieList.Add(MovieInfo);
            
        }

        int option = OptionsMenu.DisplaySystem(movieList, "Current movies", "Use ⬆ and ⬇ to navigate and press Enter to remove the selected movie:", true, true);

        if (option >= 1 && option <= movieList.Count)
        {
            // Get the index of the selected option (adjusted for 0-based indexing)
            int selectedOptionIndex = option - 1;

            // Extract the movie ID from the selected option string
            string selectedOption = movieList[selectedOptionIndex];
            int startIndex = selectedOption.IndexOf("ID: ") + 4;
            int endIndex = selectedOption.IndexOf('\n', startIndex);
            string idString = selectedOption.Substring(startIndex, endIndex - startIndex).Trim();

            int idToRemove;
            bool isValidId = int.TryParse(idString, out idToRemove);

            if (isValidId)
            {
                // Remove the movie with the specified ID
                MovieModel? movieToRemove = movies.Find(movie => movie.Id == idToRemove);
                if (movieToRemove != null)
                {
                    movies.Remove(movieToRemove);
                    MovieAccess.WriteAll(movies);
                    Console.WriteLine("Movie removed successfully.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID found for the selected movie.");
            }
        }
        else
        {
            Console.WriteLine("Invalid option selected.");
        }
    }
    public void RemoveMovieID(int id)
    {
        // finds if there is a movie with the same id
        int index = _movies.FindIndex(s => s.Id == id);

        // removes the movie with that id, and updates the json file
        _movies.Remove(_movies[index]);
        MovieAccess.WriteAll(_movies);
    }
    public static void AddOrUpdateMovie()
    {
        List<MovieModel> movies = MovieAccess.LoadAll();

        Console.WriteLine("Enter the movie details:");

        Console.Write("Title: ");
        string title = Console.ReadLine();

        MovieModel? existingMovie = movies.FirstOrDefault(movie => movie.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (existingMovie != null)
        {
            int YNQ = OptionsMenu.DisplaySystem(YN, "Movie already exists", $"Movie with the title '{title}' already exists, do you want to update the details instead?", true, false);
            if (YNQ == 2)
            {
                Console.WriteLine("Update operation cancelled, press enter to continue.");
                Console.ReadLine();
                return;
            }
        }

        Console.Write("Genre: ");
        string genre = Console.ReadLine();

        double rating;
        while (true)
        {
            Console.Write("Rating: ");
            string ratingInput = Console.ReadLine();

            if (double.TryParse(ratingInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rating))
            {
                break;
            }

            Console.WriteLine("Invalid rating. Please enter a valid decimal number.");
        }

        Console.Write("Description: ");
        string description = Console.ReadLine();

        int age;
        while (true)
        {
            Console.Write("Age: ");
            string ageInput = Console.ReadLine();

            if (int.TryParse(ageInput, out age) && age >= 0)
            {
                break;
            }

            Console.WriteLine("Invalid age. Please enter a valid mumber.");
        }

        DateTime viewingDate;
        while (true)
        {
            Console.WriteLine("Viewing Date? (MM-DD-YYYY)");
            string[] date = Console.ReadLine().Split("-");

            if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                continue;
            }

            Console.WriteLine("Time? (ex. 16:30)");
            string[] times = Console.ReadLine().Split(":");

            if (times.Length != 2 || !int.TryParse(times[0], out int hour) || !int.TryParse(times[1], out int minute))
            {
                Console.WriteLine("Invalid time format. Please try again.");
                continue;
            }

            try
            {
                viewingDate = new DateTime(year, month, day, hour, minute, 0);
                break;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid date or time. Please try again.");
            }
        }
        DateTime publishDate;
        while (true)
        {
            Console.WriteLine("Publish Date? (MM-DD-YYYY)");
            string[] date = Console.ReadLine().Split("-");

            if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                continue;
            }

            Console.WriteLine("Time? (ex. 16:30)");
            string[] times = Console.ReadLine().Split(":");

            if (times.Length != 2 || !int.TryParse(times[0], out int hour) || !int.TryParse(times[1], out int minute))
            {
                Console.WriteLine("Invalid time format. Please try again.");
                continue;
            }

            try
            {
                publishDate = new DateTime(year, month, day, hour, minute, 0);
                break;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid date or time. Please try again.");
            }
        }

        int maxId = movies.Count > 0 ? movies.Max(movie => movie.Id) : 0;

        if (existingMovie != null)
        {
            // Update the existing movie
            existingMovie.Title = title;
            existingMovie.Genre = genre;
            existingMovie.Rating = rating;
            existingMovie.Description = description;
            existingMovie.Age = age;
            existingMovie.ViewingDate = viewingDate;
            existingMovie.PublishDate = publishDate;

            Console.Clear();
            Console.WriteLine("Movie updated successfully!\n\nPress enter to continue.");
            Console.ReadLine();
        }
        else
        {
            // Create a new movie with a unique ID
            MovieModel newMovie = new MovieModel(++maxId, title, genre, rating, description, age, viewingDate, publishDate);
            movies.Add(newMovie);

            Console.Clear();
            Console.WriteLine("Movie added successfully!\n\nPress enter to continue.");
            Console.ReadLine();
        }
        // Save the updated movies list
        MovieAccess.WriteAll(movies);
    }
    protected static List<string> MovieEditorList = new List<string>()
    {
        "Current movies",
        "Add movies",
        "Edit movies",
        "Remove movies"
    };
    protected static List<string> YN = new List<string>()
    {
        "Yes",
        "No"
    };
    protected static List<string> AddMovieList = new List<string>()
    {
        "Single movie entry",
        "JSON File",
        "CSV File"
       
    };
    protected static List<string> RemoveList = new List<string>()
    {
        "Remove movie selection",
        "Remove movie by ID"
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
                LoadMovies();
                PrintMovies(_movies, true, true);
            }
            else if (MovieOptions == 2)
            {
                Console.Clear();
                int addOptions = OptionsMenu.DisplaySystem(AddMovieList, "Add movies", "To add movies by file, please save the json or csv file in the DataSources folder", true, true);

                if (addOptions == 1)
                {
                    Console.Clear();
                    AddOrUpdateMovie();
                }
                else if (addOptions == 2)
                {
                    Console.Clear();
                    AddMultipleMoviesJSON();
                }
                else if (addOptions == 3)
                {
                    Console.Clear();
                    AddMultipleMoviesCSV();
                }
            }
            else if (MovieOptions == 3)
            {
                Console.Clear();
                LoadMovies();
                PrintMovies(_movies, true);
            }
            else if (MovieOptions == 4)
            {
                int removeOptions = OptionsMenu.DisplaySystem(RemoveList, "Remove movies", "Use ⬆ and ⬇ to navigate and press Enter to select:", true, true);
                if (removeOptions == 1)
                {
                    RemoveMovie();
                }
                else if (removeOptions == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Please enter the movie ID you would like to remove.");
                    int removeID = int.Parse(Console.ReadLine() + "");
                    RemoveMovieID(removeID);
                }
            }
            else if (MovieOptions == 5)
            {
                break;
            }
        }
    }
}