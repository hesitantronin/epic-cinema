class SeatModel
{
    public int Id { get; set; }
    public double Price = 0.0;

    public double GetPrice()
    {
        switch (Seat)
        {
            case SeatType.Loveseat:
                Price = 12.5;
                break;

            case SeatType.NormalSeat:
                Price = 8.50;
                break;

            case SeatType.VIPseat:
                Price = 20.0;
                break;

        }
        return Price;

    }

    public SeatType Seat { get; set; }

    public SeatModel(int id, SeatType seat)
    {
        Id = id;
        Seat = Seat;
        GetPrice();
    }

}