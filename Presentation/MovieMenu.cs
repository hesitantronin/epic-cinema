class MovieMenu
{
    static public void Start()
    {
        MovieLogic movielogic = new MovieLogic();
        movielogic.PrintMovies();

        movielogic.PrintMovies(movielogic.SortBy("GENRE"));

    }
}