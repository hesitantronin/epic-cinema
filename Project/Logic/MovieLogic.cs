using System.Text.Json;
using System.Globalization;
using System.Text.RegularExpressions;

public class MovieLogic
{
    private List<MovieModel> _movies = new();
    static public MovieModel? CurrentMovie { get; private set; }
    public static List<string> ContinueList = new List<string>() { "Retry" };
    public static TimeSpan oneTimeSlotAmount = new TimeSpan(2, 0, 0); // one timeslot == 2 hours


    public MovieLogic()
    {
        LoadMovies();
    }

    private void LoadMovies()
    {
        _movies = MovieAccess.LoadAll();
    }

    // public void UpdateList(MovieModel mov)
    // {
    //     // finds if there is already a movie with the same id
    //     int index = _movies.FindIndex(s => s.Id == mov.Id);

    //     // if the index exists, itll update the movie, otherwhise itll add a new one
    //     if (index != -1)
    //     {
    //         // updates existing movie
    //         _movies[index] = mov;
    //     }
    //     else
    //     {
    //         //adds new movie
    //         _movies.Add(mov);
    //     }

    //     // writes the changed data to the json file
    //     MovieAccess.WriteAll(_movies);
    // }

    public MovieModel? GetById(int id)
    {
        // returns the movie data that matches the id
        return _movies.Find(i => i.Id == id);
    }

    public void PrintMovies(List<MovieModel> TempMovieList, bool IsEmployee = false, bool IsEdit = false, bool SeatEdit = false)
    {
        // checks whether the acc is admin or customer, if customer, only future movies get shown, otherwise all movies get shown
        List<MovieModel> MovieList = new();
        if (AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.CUSTOMER || AccountsLogic.CurrentAccount.Type == AccountModel.AccountType.GUEST)
        {
            foreach (MovieModel movie in TempMovieList)
            {   
                if (movie.ViewingDate > DateTime.Now)
                {
                    MovieList.Add(movie);
                }
            }
        }
        else
        {
            MovieList = TempMovieList;
        }
        
        while (true)
        {
            if (ReservationMenu.reservationMade)
            {
                return;
            }

            // prints an error message if nothing was found
            if (MovieList.Count() == 0)
            {
                // list of options that will be displayed
                List<string> ReturnList = new List<string>() {"Return"};

                // the necessary info gets used in the display method
                int option = OptionsMenu.DisplaySystem(ReturnList, "MOVIES", "No movies were found that matched the criteria.", true, false);

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

                while (BaseLine < MovieList.Count())
                {
                    if (ReservationMenu.reservationMade)
                    {
                        return;
                    }
                    
                    if (BaseLine + 5 > MovieList.Count())
                    {
                        MaxItems = MovieList.Count() % 5;
                        nextButton = false;
                    }
                    else if (BaseLine + 5 == MovieList.Count())
                    {
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

                    double totalPages = Math.Ceiling((double)MovieList.Count() / 5);
                    
                    // the necessary info gets used in the display method
                    List<MovieModel> subList = MovieList.GetRange(BaseLine, MaxItems);

                    int option = OptionsMenu.MovieDisplaySystem(subList, "MOVIES", $"Page {pageNr} (of {totalPages})", true, previousButton, nextButton);

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
                        }
                        else if (SeatEdit)
                        {
                            SeatLogic.SeatSelectionEdit(subList[option - 1]);
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
    public void EditMovie(MovieModel movie)
    {
        bool seatEdit = false;
        bool remove = false;
        bool Return = false;

        while (true)
        {
            Return = false;

            // shows the banner and title
            OptionsMenu.Logo(movie.Title);

            List<string> movieInfoList = new List<string>();
            movieInfoList.Add($"Title: {movie.Title}");
            movieInfoList.Add($"Genre: {movie.Genre}");
            movieInfoList.Add($"Rating: {movie.Rating}");
            movieInfoList.Add($"Age Restriction: {movie.Age}");
            movieInfoList.Add($"Publish Date: {movie.PublishDate}");
            movieInfoList.Add($"Description: {MovieLogic.SpliceText(movie.Description, "   ")}");
            movieInfoList.Add($"Viewing Date: {movie.ViewingDate}");
            movieInfoList.Add($"Runtime: {movie.RunTime}");
            movieInfoList.Add($"Base Price: {movie.MoviePrice}");
            movieInfoList.Add("Auditorium seat editer\n");
            movieInfoList.Add("\u001b[31mRemove Movie\u001b[0m");

            int edit = OptionsMenu.DisplaySystem(movieInfoList, "Edit movie", "Select which field to edit, or choose to delete this movie.", true, true);
            if (edit == 1)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.Write("New Title: ");
                    string newTitle = Console.ReadLine() + "";
                    if (!string.IsNullOrEmpty(newTitle))
                    {
                        movie.Title = newTitle;
                        break;
                    }

                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nThe title can't be empty.", false, true);

                    if (Answer == 2)
                    {
                        Return = true;
                        break;
                    }
                }
            }
            else if (edit == 2)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.Write("New genre: ");
                    string newGenre = Console.ReadLine() + "";
                    if (!string.IsNullOrEmpty(newGenre))
                    {
                        movie.Genre = newGenre;
                        break;
                    }
                    
                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nThe genre can't be empty.", false, true);

                    if (Answer == 2)
                    {
                        Return = true;
                        break;
                    }                    
                }
            }
            else if (edit == 3)
            {
                double rating;
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.Write("Rating: ");
                    string ratingInput = Console.ReadLine() + "";

                    if (double.TryParse(ratingInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rating))
                    {
                        movie.Rating = rating;
                        break;
                    }
                    
                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid rating. Please enter a valid decimal number.", false, true);

                    if (Answer == 2)
                    {
                        Return = true;
                        break;
                    }
                }
            }
            else if (edit == 4)
            {
                int age;
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.Write("Age: ");
                    string newAgeInput = Console.ReadLine() + "";

                    if (int.TryParse(newAgeInput, out age) && age >= 0)
                    {
                        movie.Age = age;
                        break;
                    }

                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid age. Please enter a valid number.", false, true);

                    if (Answer == 2)
                    {
                        Return = true;
                        break;
                    }
                }
            }
            else if (edit == 5)
            {
                DateTime publishDate;
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.WriteLine("Publish Date: (MM-DD-YYYY)");
                    string[] date = Console.ReadLine().Split("-");

                    if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
                    {
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date format. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                        continue;
                    }

                    try
                    {
                        publishDate = new DateTime(year, month, day, 0, 0, 0);
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        OptionsMenu.FakeContinue("\nInvalid date. Please try again.");
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                    }
                }
            }
            else if (edit == 6)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.Write("Description: ");
                    string newDescription = Console.ReadLine() + "";
                    if (!string.IsNullOrEmpty(newDescription))
                    {
                        movie.Description = newDescription;
                        break;
                    }
                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nThe description can't be empty.", false, true);

                    if (Answer == 2)
                    {
                        Return = true;
                        break;
                    }

                }
            }
            else if (edit == 7)
            {
                DateTime viewingDate;
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.WriteLine("Viewing Date: (MM-DD-YYYY)");
                    string[] date = Console.ReadLine().Split("-");

                    if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
                    {
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date format. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                        continue;
                    }

                    Console.WriteLine("Viewing Time: (ex. 16:30)");
                    string[] times = Console.ReadLine().Split(":");

                    if (times.Length != 2 || !int.TryParse(times[0].TrimStart('0'), out int hour) || !int.TryParse(times[1], out int minute))
                    {
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid time format. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                        continue;
                    }

                    try
                    {
                        viewingDate = new DateTime(year, month, day, hour, minute, 0);
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date or time. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                    }
                }
            }
            else if (edit == 8)
            {
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.WriteLine("Runtime: (ex. 1:30)");
                    string[] times = Console.ReadLine().Split(":");

                    if (times.Length != 2 || times[1].Length != 2 || !int.TryParse(times[0].TrimStart('0'), out int hour) || !int.TryParse(times[1], out int minute))
                    {
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid time format. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                        continue;
                    }

                    try
                    {
                        TimeOnly runTimeTest = new TimeOnly(hour, minute, 0);
                        movie.RunTime = $"{runTimeTest.ToString("hh\\:mm")}";
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date or time. Please try again.", false, true);

                        if (Answer == 2)
                        {
                            Return = true;
                            break;
                        }
                    }
                }
            }
            else if (edit == 9)
            {
                double price;
                while (true)
                {
                    OptionsMenu.Logo("edit movie");

                    Console.Write("Base price: ");
                    string newAgeInput = Console.ReadLine() + "";

                    if (double.TryParse(newAgeInput, out price) && price >= 0)
                    {
                        movie.MoviePrice = price;
                        break;
                    }

                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid price. Please enter a valid decimal number.", false, true);

                    if (Answer == 2)
                    {
                        Return = true;
                        break;
                    }
                }   
            }
            else if (edit == 10)
            {
                seatEdit = true;
                SeatLogic.SeatSelectionEdit(movie);
            }
            else if (edit == 11)
            {

                int removeOptions = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "confirm", "Are you sure you want to delete this item?", true, false);
                if (removeOptions == 1)
                {
                    remove = true;
                }
                else
                {
                    Return = true;
                }
            }
            else
            {
                return;
            }

            if (!Return)
            {
                // Find the index of the movie to update
                int index = _movies.FindIndex(m => m.Id == movie.Id);

                if (!remove)
                {
                    // Update the movie in the list
                    _movies[index] = movie;
                    
                    // Write the updated movie list to the JSON file
                    MovieAccess.WriteAll(_movies);
                    if (!seatEdit)
                    {
                        OptionsMenu.FakeContinue("Movie updated successfully.", "movie updated");

                    }
                }
                else
                {
                    RemoveLogic(index);
                    
                    OptionsMenu.FakeContinue("Movie deleted successfully.", "movie deleted");

                    return;

                }
            }
        }
    }
    
    public static void RemoveLogic(int index)
    {
        List<MovieModel> movies = MovieAccess.LoadAll();
        
        movies.Remove(movies[index]);

        MovieAccess.WriteAll(movies);
    }

    public void PrintMovies() => PrintMovies(_movies);
 
    public void MovieInfo(MovieModel movie)
    {
        while (true)
        {
            if (ReservationMenu.reservationMade)
            {
                return;
            }

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
            Console.WriteLine($"Release Date");
            Console.ResetColor();
            Console.WriteLine($" {movie.PublishDate.ToString("MMMM yyyy")}\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Description");
            Console.ResetColor();
            Console.WriteLine($" {MovieLogic.SpliceText(movie.Description, " ")}\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Viewing Date");
            Console.ResetColor();
            Console.WriteLine($" {movie.ViewingDate.ToString("dddd, dd MMMM yyyy, HH:mm")}\n");
            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Runtime");
            Console.ResetColor();
            Console.WriteLine($" {movie.RunTime}\n");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Base Price");
            Console.ResetColor();
            Console.WriteLine($" € {String.Format("{0:0.00}", movie.MoviePrice)}\n");

            // the necessary info gets used in the display method
            int option = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "", "\nDo you want to select this movie?", false, false);

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
            return (ascending) ? unsorted.OrderBy(m => m.ViewingDate).ToList() : unsorted.OrderByDescending(m => m.ViewingDate).ToList();
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

    public static void AddMultipleMoviesJSON( )
    {
        string filename;
        do
        {
            OptionsMenu.Logo("json movie adding");
            Console.WriteLine("Please save the JSON file in the DataSources folder.");
            Console.WriteLine("Enter the JSON file name without '.json': ");
            string jsonFile = Console.ReadLine() + "";
            filename = @$"DataSources\{jsonFile}.json";

            if (!File.Exists(filename))
            {
                List<string> EList = new List<string>() { "Continue" };
                int option = OptionsMenu.DisplaySystem(EList, "", $"\nFile '{jsonFile}.json' not found in directory: {filename}, make sure the file is saved in DataSources and the input is without '.json'", false, true);
                if (option == 2)
                {
                    return;
                }
            }
        } while (!File.Exists(filename));

        List<MovieModel> existingMovies = AddMultipleMoviesJsonLogic(filename);

        // Display existing movies message
        if (existingMovies.Count > 0)
        {
            OptionsMenu.Logo("double movies");

            Console.WriteLine("The following movies already exist and were not added:\n");
            foreach (MovieModel existingMovie in existingMovies)
            {
                Console.WriteLine($"- Movie ID: {existingMovie.Id}\nTitle: {existingMovie.Title}\nGenre: {existingMovie.Genre}\n");
            }

            Console.CursorVisible = false;
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();

            Console.CursorVisible = true; 
        }
        else
        {
            OptionsMenu.FakeContinue("Movies have been succesfully added.", "Movies added");          
        }  
    }

    public static List<MovieModel> AddMultipleMoviesJsonLogic(string filename)
    {
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
        return existingMovies;
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
            OptionsMenu.Logo("CSV movie adding");
            Console.WriteLine("Please save the CSV file in the DataSources folder.\n\n");
            Console.WriteLine("Enter the CSV file name without '.csv': ");
            string csvFile = Console.ReadLine() + "";
            filename = @$"DataSources\{csvFile}.csv";

            if (!File.Exists(filename))
            {
                Console.Clear();
                List<string> EList = new List<string>() { "Continue" };
                int option = OptionsMenu.DisplaySystem(EList, "", $"\nFile '{csvFile}.csv' not found in directory: {filename}, make sure the file is saved in DataSources and the input is without '.csv'", false, true);
                if (option == 2)
                {
                    return;
                }
            }
        } while (!File.Exists(filename));
       
        List<MovieModel> existingMovies = AddMultipleMoviesCsvLogic(filename);

        // Display existing movies message
        if (existingMovies.Count > 0)
        {
            OptionsMenu.Logo("double movies");

            Console.WriteLine("The following movies already exist and were not added:\n");
            foreach (MovieModel existingMovie in existingMovies)
            {
                Console.WriteLine($"- Movie ID: {existingMovie.Id}\nTitle: {existingMovie.Title}\nGenre: {existingMovie.Genre}\n");
            }

            Console.CursorVisible = false;
            // prints a fake return option hehe
            Console.WriteLine("\n > \u001b[32mContinue\u001b[0m");
        
            // actually returns you to the main menu
            Console.ReadLine();

            Console.CursorVisible = true;
        }
        else
        {
            OptionsMenu.FakeContinue("Movies have been succesfully added.", "movies added");      
        }
    }

    public static List<MovieModel> AddMultipleMoviesCsvLogic(string filename)
    {
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
                DateTime.Parse(line.Split(",")[7]), // publish date
                line.Split(",")[8], // runtime
                line.Split(",")[9].Select(s => Convert.ToInt32(s)).ToList() // timeslot(s), converts string of numbers s
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

        return existingMovies;
    }
    
    // private static void RemoveMovie()
    // {
    //     List<MovieModel> movies = MovieAccess.LoadAll();

    //     // Retrieve and display the movies
    //     List<string> movieList = new List<string>();
    //     foreach (MovieModel movie in movies)
    //     {
    //         string MovieInfo = $"ID: {movie.Id}\nTitle: {movie.Title}\nGenre: {movie.Genre}\nAge: {movie.Age}\nViewing Date: {movie.ViewingDate}\nPublish Date: {movie.PublishDate}\n";
    //         movieList.Add(MovieInfo);
            
    //     }

    //     int option = OptionsMenu.DisplaySystem(movieList, "Current movies", "Use ⬆ and ⬇ to navigate and press Enter to remove the selected movie:", true, true);

    //     if (option >= 1 && option <= movieList.Count)
    //     {
    //         // Get the index of the selected option (adjusted for 0-based indexing)
    //         int selectedOptionIndex = option - 1;

    //         // Extract the movie ID from the selected option string
    //         string selectedOption = movieList[selectedOptionIndex];
    //         int startIndex = selectedOption.IndexOf("ID: ") + 4;
    //         int endIndex = selectedOption.IndexOf('\n', startIndex);
    //         string idString = selectedOption.Substring(startIndex, endIndex - startIndex).Trim();

    //         int idToRemove;
    //         bool isValidId = int.TryParse(idString, out idToRemove);

    //         if (isValidId)
    //         {
    //             // Remove the movie with the specified ID
    //             MovieModel? movieToRemove = movies.Find(movie => movie.Id == idToRemove);
    //             if (movieToRemove != null)
    //             {
    //                 movies.Remove(movieToRemove);
    //                 MovieAccess.WriteAll(movies);
    //                 Console.WriteLine("Movie removed successfully.");
    //             }
    //         }
    //         else
    //         {
    //             Console.WriteLine("Invalid ID found for the selected movie.");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine("Invalid option selected.");
    //     }
    // }

    // public void RemoveMovieID()
    // {
    //     int removeID;

    //     do
    //     {
    //         Console.Clear();
    //         Console.WriteLine("Please enter the movie ID you would like to remove.");
    //         if (!int.TryParse(Console.ReadLine(), out removeID))
    //         {
    //             Console.WriteLine("Invalid ID. Please enter a valid integer ID.");
    //             continue;
    //         }

    //         // Find the index of the movie with the specified ID
    //         int index = _movies.FindIndex(s => s.Id == removeID);

    //         if (index != -1)
    //         {
    //             // Remove the movie from the list
    //             _movies.RemoveAt(index);

    //             // Update the JSON file
    //             MovieAccess.WriteAll(_movies);

    //             Console.WriteLine("Movie removed successfully.\n\nPress enter to continue.");
    //             Console.ReadLine();
    //             break;
    //         }
    //         else
    //         {
    //             List<string> EList = new List<string>() { "Continue" };
    //             int option = OptionsMenu.DisplaySystem(EList, "", $"\nID {removeID} not found, make sure to enter a valid ID", false, true);
    //             if (option == 2)
    //             {
    //                 return;
    //             }
    //         }
    //     } while (true);
    // }
        public static Dictionary<String, List<String>> TimeSlots = new Dictionary<string, List<String>> () 
    {
       { "Monday", new List<String> { "9:00", "11:00", "13:00", "15:00", "17:00", "19:00", "21:00" } },
       { "Tuesday", new List<String> { "9:00", "11:00", "13:00", "15:00", "17:00", "19:00", "21:00" } },
       { "Wednesday", new List<String> { "9:00", "11:00", "13:00", "15:00", "17:00", "19:00", "21:00" } },
       { "Thursday", new List<String> { "9:00", "11:00", "13:00", "15:00", "17:00", "19:00", "21:00" } },
       { "Friday", new List<String> { "9:00", "11:00", "13:00", "15:00", "17:00", "19:00", "21:00" } },
       { "Saturday", new List<String> { "12:00", "14:00", "16:00", "18:00", "20:00", "22:00" } },
       { "Sunday", new List<String> { "13:00", "15:00", "17:00", "19:00", "21:00" } },
    };

    public static List<int> CheckTimeSlotAvailability(DateTime viewingDate, TimeSpan runTime)
    {
        // get list of all movies
        List<MovieModel> movies = MovieAccess.LoadAll();

        // get list of movies where the viewing day is the same as the day and timeslots we want to check
        List<MovieModel> relevantMovies = movies.Where(m => m.ViewingDate.Date == viewingDate.Date).ToList();

        List<int> movieTimeSlots = new List<int>(){};
        TimeSpan viewingTime = viewingDate.TimeOfDay; // viewing time
        TimeSpan neededTimeSlot = TimeSpan.Zero;
        int neededTimeSlotIndex = 0;
        
        foreach (string timeslot in TimeSlots[$"{viewingDate.DayOfWeek}"])
        {
            TimeSpan start = TimeSpan.Parse(timeslot);
            TimeSpan end = TimeSpan.Parse(timeslot).Add(oneTimeSlotAmount);

            // find the correct timeslot
            if (start <= viewingTime && end > viewingTime)
            {
                neededTimeSlot = TimeSpan.Parse(timeslot);
                neededTimeSlotIndex = TimeSlots[$"{viewingDate.DayOfWeek}"].FindIndex(d => d == timeslot);
                break;
            }
        }

        if (neededTimeSlot == TimeSpan.Zero) // timeslot wasn't found (timeslot not within opening/closing hours)
        {
            return Enumerable.Empty<int>().ToList(); // return an empty list
        }
        else
        {
            // check if timeslot is available
            foreach (MovieModel movie in relevantMovies)
            {
                foreach (int takenTimeslot in movie.TimeSlot)
                {
                    // check if the timeslot we need is taken already
                    if (takenTimeslot == neededTimeSlotIndex)
                    {
                        return Enumerable.Empty<int>().ToList(); // return an empty list
                    }
                }
            }

            if (runTime < oneTimeSlotAmount) // runtime fits within one timeslot, and its available
            {
                return new List<int>(){neededTimeSlotIndex};
            }
            else // runtime does not fit within current available timeslot
            {
                // check how many timeslots will be needed
                int timeSlotAmount = (int)Math.Ceiling(runTime / oneTimeSlotAmount);

                // go through every seperate movie in relevantMovies
                foreach (MovieModel movie in relevantMovies)
                {
                    // go through list of timeslots for current movie
                    foreach (int takenTimeslot in movie.TimeSlot)
                    {
                        // check the current movies list of timeslots for each timeslot that we'll need
                        for (int timeslot = neededTimeSlotIndex; timeslot < ((neededTimeSlotIndex + timeSlotAmount) - 1); timeslot++)
                        {
                            if (takenTimeslot == timeslot)
                            {
                                Console.WriteLine("Timeslot is taken");
                                return Enumerable.Empty<int>().ToList(); // return an empty list
                            }
                        }
                    }
                }

                for (int i = neededTimeSlotIndex; i < (timeSlotAmount + 1); i++)
                {
                    movieTimeSlots.Add(i);
                }
                return movieTimeSlots;
            }
        }
    }

    public static void AddOrUpdateMovie()
    {
        List<MovieModel> movies = MovieAccess.LoadAll();
        MovieModel? existingMovie = null;

        string title;
        while (true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.Write("Title: ");
            title = Console.ReadLine();

            existingMovie = movies.FirstOrDefault(movie => movie.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (existingMovie != null)
            {
                int YNQ = OptionsMenu.DisplaySystem(OptionsMenu.YesNoList, "Movie already exists", $"Movie with the title '{title}' already exists, do you want to update the details instead?", true, false);
                if (YNQ == 2)
                {
                    OptionsMenu.FakeContinue("Update operation cancelled.", "Canceled");
                    return;
                }
            }
            if (title != "")
            {
                break;
            }

            int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nThe title can't be empty.", false, true);

            if (Answer == 2)
            {
                return;
            }
        }

        string genre;
        while (true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.Write("Genre: ");
            genre = Console.ReadLine();

            if (genre != "")
            {
                break;
            }

            int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nThe genre can't be empty.", false, true);

            if (Answer == 2)
            {
                return;
            }
        }
        
        double rating = 0;
        while (true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.Write("Rating: ");
            string ratingInput = Console.ReadLine() + "";

            if (double.TryParse(ratingInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rating))
            {
                break;
            }

            int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid rating. Please enter a valid decimal number.", false, true);

            if (Answer == 2)
            {
                return;
            }

        }

        string description;
        while(true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.Write("Description: ");
            description = Console.ReadLine() + "";

            if (description != "")
            {
                break;
            }

            int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nThe description can't be empty.", false, true);

            if (Answer == 2)
            {
                return;
            }

        }

        int age;
        while (true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.Write("Age: ");
            string ageInput = Console.ReadLine() + "";

            if (int.TryParse(ageInput, out age) && age >= 0)
            {
                break;
            }

            int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid age. Please enter a valid mumber.", false, true);

            if (Answer == 2)
            {
                return;
            }
        }

        DateTime viewingDate = new DateTime();
        string runTime = "";
        List<int> movieTimeSlots = new List<int>();
        TimeOnly runTimeTest;
        while (true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.WriteLine("Viewing Date: (MM-DD-YYYY)");
            string[] date = Console.ReadLine().Split("-");

            if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date format. Please try again.", false, true);

                if (Answer == 2)
                {
                    return;
                }
                continue; // without continue, month, day, year will be considered unassigned
            }

            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.WriteLine("Viewing Time: (ex. 16:30)");
            string[] times = Console.ReadLine().Split(":");

            if (times.Length != 2 || !int.TryParse(times[0], out int hour) || !int.TryParse(times[1], out int minute)
            || (hour < 0 || hour > 23) || (minute < 0 || minute > 59))
            {
                if (times[0].Length > 2 || times[1].Length > 2)
                {
                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid time format. Please try again.", false, true);
                    if (Answer == 2)
                    {
                        return;
                    }
                }
                else
                {
                    int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid time format. Hours should be between 0 and 23. Minutes should be between 0 and 59.", false, true);
                    if (Answer == 2)
                    {
                        return;
                    }
                }
                continue;
            }

            try
            {
                viewingDate = new DateTime(year, month, day, hour, minute, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date or time. Please try again.", false, true);

                if (Answer == 2)
                {
                    return;
                }
            }

            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.WriteLine("Runtime: (ex. 2:30)");
            string runTimes = Console.ReadLine() + "";
            string pattern = "^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";
    
            if (!Regex.IsMatch(runTimes, pattern))
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid time format. Please try again.", false, true);
                if (Answer == 2)
                {
                    return;
                }
            }

            try
            {
                string[] runtimes = runTimes.Split(":");
                runTimeTest = new TimeOnly(Convert.ToInt32(runtimes[0]), Convert.ToInt32(runtimes[1]), 0);
                runTime = $"{Convert.ToInt32(runtimes[0])}:{Convert.ToInt32(runtimes[1]):00}";
            }
            catch (ArgumentOutOfRangeException)
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid time. Please try again.", false, true);

                if (Answer == 2)
                {
                    return;
                }
            }

            movieTimeSlots = CheckTimeSlotAvailability(viewingDate, TimeSpan.Parse(runTime));
            if (movieTimeSlots.Count == 0)
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nTimeslot not available. Please try again.", false, true);
                if (Answer == 2)
                {
                    return;
                }
            }
            else
            {
                break;
            }

        }


        DateTime publishDate;
        while (true)
        {
            OptionsMenu.Logo("Add movie");
            Console.WriteLine("Enter the movie details.");

            Console.WriteLine("Publish Date: (MM-DD-YYYY)");
            string[] date = Console.ReadLine().Split("-");

            if (date.Length != 3 || !int.TryParse(date[0], out int month) || !int.TryParse(date[1], out int day) || !int.TryParse(date[2], out int year))
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date format. Please try again.", false, true);

                if (Answer == 2)
                {
                    return;
                }

                continue;
            }
            try
            {
                publishDate = new DateTime(year, month, day, 0, 0, 0);
                break;
            }
            catch (ArgumentOutOfRangeException)
            {
                int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid date. Please try again.", false, true);

                if (Answer == 2)
                {
                    return;
                }
            }
        }

        double price;
        while (true)
        {
            OptionsMenu.Logo("add movie");
            Console.WriteLine("Enter the movie details.");

            Console.Write("Base price: ");
            string newAgeInput = Console.ReadLine() + "";

            if (double.TryParse(newAgeInput, out price) && price >= 0)
            {
                break;
            }

            int Answer = OptionsMenu.DisplaySystem(ContinueList, "", "\nInvalid price. Please enter a valid decimal number.", false, true);

            if (Answer == 2)
            {
                return;
            }
        }   

        AddOrUpdateMovieLogic(existingMovie, title, genre, rating, description, age, viewingDate, publishDate, price, runTime, movieTimeSlots);

        if (existingMovie != null)
        {
            OptionsMenu.FakeContinue("Movie updated successfully!", "movie updated");
        }
        else
        {
            OptionsMenu.FakeContinue("Movie added successfully!", "movie added");
        }
    }

    public static void AddOrUpdateMovieLogic(MovieModel existingMovie, string title, string genre, double rating, string description, int age, DateTime viewingDate, DateTime publishDate, double price, string runTime, List<int> movieTimeSlots)
    {
        List<MovieModel> movies = MovieAccess.LoadAll();

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
            existingMovie.MoviePrice = price;
            existingMovie.RunTime = runTime;
            existingMovie.TimeSlot = movieTimeSlots;
        }
        else
        {
            // Create a new movie with a unique ID
            MovieModel newMovie = new MovieModel(++maxId, title, genre, rating, description, age, viewingDate, publishDate, runTime, movieTimeSlots, price);
            movies.Add(newMovie);
        }
        // Save the updated movies list
        MovieAccess.WriteAll(movies);
    }

    public void EmployeeMovies()
    {
        while (true)
        {
            List<string> MovieEditorList = new List<string>()
            {
                "Add movies",
                "Edit/Remove Movies and seats",
            };

            int MovieOptions = OptionsMenu.DisplaySystem(MovieEditorList, "edit Movies", "Select what you want to do.", true, true);
            if (MovieOptions == 1)
            {
                while (true)
                {
                    List<string> AddMovieList = new List<string>()
                    {
                        "Single movie entry",
                        "JSON File",
                        "CSV File"
                    };

                    int addOptions = OptionsMenu.DisplaySystem(AddMovieList, "Add movies", "To add movies by file, please save the json or csv file in the DataSources folder", true, true);

                    if (addOptions == 1)
                    {
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
                    else 
                    {
                        break;
                    }
                }
            }
            else if (MovieOptions == 2)
            {
                while (true)
                {
                    // list of options to display
                    List<string> OptionList = new List<string>()
                    {
                    "Sort",
                    "Filter",
                    "Search",
                    "Show All Movies"
                    };

                    // the necessary info gets used in the display method
                    int option = OptionsMenu.DisplaySystem(OptionList, "edit Movies");

                    // depending on the option that was chosen, it will clear the console and call the right function
                    if (option == 1)
                    {
                        MovieMenu.Sort(true);
                    }
                    else if (option == 2)
                    {
                        MovieMenu.Filter(true);
                    }
                    else if (option == 3)
                    {
                        while (true)
                        {
                            List<string> id_or_else = new(){"Id", "Other"};
                            int option2 = OptionsMenu.DisplaySystem(id_or_else, "Search by", "What do you want to search by?");

                            if (option2 == 1)
                            {
                                MovieModel result = MovieMenu.SearchId();
                                if (result == null)
                                {
                                    OptionsMenu.FakeContinue("No item with this id was found.", "ID NOT FOUND"); 
                                }
                                else
                                {
                                    EditMovie(result);
                                }
                            }
                            if (option2 == 2)
                            {
                                MovieMenu.Search(true);
                            }
                            
                            else if (option2 == 3)
                            {
                                break;
                            }
                        }

                    }
                    else if (option == 4)
                    {
                        LoadMovies();
                        PrintMovies(_movies, true);
                    }

                    else if (option == 5)
                    {
                        break;
                    }
                }
            }
            else if (MovieOptions == 3)
            {
                break;
            }
        }
    }
}