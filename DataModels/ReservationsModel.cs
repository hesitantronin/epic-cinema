using System.Text.Json.Serialization;

public class ReservationsModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

    [JsonPropertyName("movie")]
    public MovieModel Movie { get; set; }

    [JsonPropertyName("seatReservation")]
    public List<SeatModel> SeatReservation { get; set; }

    [JsonPropertyName("cateringReservation")]
    public Dictionary<string, string> CateringReservation { get; set; }

    [JsonPropertyName("accessibilityRequest")]
    public string AccessibilityRequest { get; set; }

    [JsonPropertyName("viewing_date")]
    public DateTime ViewingDate { get; set; }

    [JsonPropertyName("reservationCode")]
    public int ReservationCode { get; set; }

    [JsonPropertyName("totalPrice")]
    public double TotalPrice { get; set; }

    [JsonPropertyName("cancelled")]
    public bool Cancelled { get; set; }

    public ReservationsModel(int id, string emailAddress, string fullName, MovieModel movie, List<SeatModel> seatReservation, Dictionary<string, string> cateringReservation, string accessibilityRequest, DateTime viewingDate, int reservationCode, double totalPrice)
    {
        this.Id = id;
        this.EmailAddress = emailAddress;
        this.FullName = fullName;
        this.Movie = movie;
        this.CateringReservation = cateringReservation;
        this.SeatReservation = seatReservation;
        this.AccessibilityRequest = accessibilityRequest;
        this.ViewingDate = viewingDate;
        this.ReservationCode = reservationCode;
        this.TotalPrice = totalPrice;
        this.Cancelled = false; 
    }
}
