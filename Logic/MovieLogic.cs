using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class MovieLogic
{
    private List<MovieModel> _movies;

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

    public MovieModel GetById(int id)
    {
        return _movies.Find(i => i.Id == id);
    }
}




