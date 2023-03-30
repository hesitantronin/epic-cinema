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

    public MovieModel GetById(int id)
    {
        // returns the movie data that matches the id
        return _movies.Find(i => i.Id == id);
    }

    public List<MovieModel> FilterBy(string? genre, bool? mature)
    {
        // copies the original list
        List<MovieModel> filtered = _movies;

        if (genre != null)
            filtered = _movies.Where(movie => movie.Genre == genre).ToList();
        if (mature == true)
            filtered = filtered.Where(movie => movie.Age >= 18).ToList();

        return filtered;
    }

    public List<MovieModel> SortBy(string input)
    {
        SortOrder order = GetOrder();
        List<MovieModel> unsorted = _movies;

        // Check what to sort by per subject, counting common misspellings in.
        if (input.ToUpper() == "DATE" || input.ToUpper() == "DARE" || input.ToUpper() == "DATR")
        {
            //This is if the user is a customer, they cannot see movies that have already played anymore.
            DateTime currentDateTime = DateTime.Now;
            if (order == SortOrder.ASCENDING)
                return unsorted.Where(m => m.PublishDate >= currentDateTime).OrderBy(m => m.PublishDate).ToList();
            if (order == SortOrder.DESCENDING)
                return unsorted.Where(m => m.PublishDate >= currentDateTime).OrderByDescending(m => m.PublishDate).ToList();
        }
        else if (input.ToUpper() == "GENRE")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.Genre).ToList() : unsorted.OrderByDescending(m => m.Genre).ToList();
        }
        else if (input.ToUpper() == "NAME")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.Title).ToList() : unsorted.OrderByDescending(m => m.Title).ToList();
        }
        else if (input.ToUpper() == "POPULARITY")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.Rating).ToList() : unsorted.OrderByDescending(m => m.Rating).ToList();
        }
        else if (input.ToUpper() == "PUBLISH")
        {
            return (order == SortOrder.ASCENDING) ? unsorted.OrderBy(m => m.PublishDate).ToList() : unsorted.OrderByDescending(m => m.PublishDate).ToList();
        }
        return unsorted;
    }

    private SortOrder GetOrder()
    {
        // Ask user what to sort by, if the input is not a string the question will repeat.
        Console.WriteLine("Would you like to sort ascending or descending? Type 'ASC' or 'DESC'.");
        while (true)
        {
            string? _order = Console.ReadLine();
            if (_order != null && _order is string)
                if (_order.ToUpper() == "ASC" || _order.ToUpper() == "DESC")
                    return (_order.ToUpper() == "ASC") ? SortOrder.ASCENDING : SortOrder.DESCENDING;
                else Console.WriteLine("Please use 'ASC' or 'DESC'.");
        }
    }

    enum SortOrder    
    {
        ASCENDING,
        DESCENDING
    }

    public void PrintMovies(List<MovieModel> to_print)
    {
        Console.WriteLine("MOVIES\n");
        foreach (MovieModel movie in to_print)
        {
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Genre: {movie.Genre}");
            Console.WriteLine($"Rating: {movie.Rating}");
            Console.WriteLine($"Description: {movie.Description}");
            Console.WriteLine();
        }
    }

    public void PrintMovies() => PrintMovies(_movies);
}