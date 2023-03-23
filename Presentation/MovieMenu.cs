class MovieMenu
{
    static public void Start()
    {
        Console.WriteLine("MOVIES\n");
        List<MovieModel> movielist = MovieAccess.LoadAll();
        foreach (MovieModel movie in movielist)
        {
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Genre: {movie.Genre}");
            Console.WriteLine($"Rating: {movie.Rating}");
            Console.WriteLine($"Description: {movie.Description}");
            Console.WriteLine();
        }
    }
}