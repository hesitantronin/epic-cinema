using System.Text.Json.Serialization;

class MovieModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("genre")]
    public string Genre { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("viewing_date")]
    public DateTime ViewingDate { get; set; }

    [JsonPropertyName("publish_date")]
    public DateTime PublishDate { get; set; }

    [JsonPropertyName("movie_price")]
    public double MoviePrice { get; set; }


    public MovieModel(int id, string title, string genre, double rating, string description, int age, DateTime viewingdate, DateTime publishdate, double movieprice = 10.99)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Rating = rating;
        Description = description;
        Age = age;
        ViewingDate = viewingdate;
        PublishDate = publishdate;
        MoviePrice = movieprice;
    }
}




