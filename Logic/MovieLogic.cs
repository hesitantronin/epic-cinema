using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class MovieLogic
{
    private List<MovieModel> _movies = new();

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public MovieModel? CurrentMovie { get; private set; }

    public MovieLogic()
    {
        _movies = MovieAccess.LoadAll();
    }


    public void UpdateList(MovieModel mov)
    {
        //Find if there is already an model with the same id
        int index = _movies.FindIndex(s => s.Id == mov.Id);

        if (index != -1)
        {
            //update existing model
            _movies[index] = mov;
        }
        else
        {
            //add new model
            _movies.Add(mov);
        }
        MovieAccess.WriteAll(_movies);
    }

    public void RemoveMovie(int id)
    {
        int index = _movies.FindIndex(s => s.Id == id);
        _movies.Remove(_movies[index]);
        MovieAccess.WriteAll(_movies);
    }

    public MovieModel GetById(int id)
    {
        return _movies.Find(i => i.Id == id);
    }

    public void AddMovie()
    {

    }

    public List<MovieModel> FilterBy(string? genre, bool? mature, List<MovieModel>? sorted)
    {
        List<MovieModel> filtered = (sorted != null) ? sorted : _movies;
        if (genre != null)
            filtered = _movies.Where(movie => movie.Genre == genre).ToList();
        if (mature == true)
            filtered = filtered.Where(movie => movie.Age >= 18).ToList();

        return filtered;
    }

    public List<MovieModel> Search(string query, List<MovieModel>? sorted) 
    {
        List<MovieModel> unsorted = (sorted != null) ? sorted : _movies;
        List<MovieModel> searched = new();

        //Filter only items containing query
        foreach(MovieModel movie in unsorted) 
            if(movie.Title.ToLower().Contains(query.ToLower()) || movie.Description.ToLower().Contains(query.ToLower())) 
                searched.Add(movie);

        return searched;
    }

    public List<MovieModel> SortBy(string input, List<MovieModel>? filtered)
    {
        SortOrder order = GetOrder();
        List<MovieModel> unsorted = (filtered != null) ? filtered : _movies;

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
            //Decide the sort order based on user input
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