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

    public MovieModel(int id, string title, string genre, double rating, string description)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Rating = rating;
        Description = description;
    }

}




