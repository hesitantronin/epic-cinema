using System.ComponentModel;
class SeatModel
{
    public string Id { get; set; }
    public double Price { get; set; }
    public SeatType SeatTypeValue { get; set; }
    public string SeatTypeName { get; set; }

    public SeatModel()
    {
        // Default parameterless constructor required for deserialization
    }

    public SeatModel(string id, int seatType)
    {
        Id = id;
        SeatTypeValue = (SeatType)seatType;
        SeatTypeName = GetSeatTypeName();
        Price = GetPrice();
    }

    public double GetPrice()
    {
        switch (SeatTypeValue)
        {
            case SeatType.OccupiedSeat:
                Price = 0.0;
                break;

            case SeatType.Loveseat:
                Price = 12.5;
                break;

            case SeatType.NormalSeat:
                Price = 8.50;
                break;

            case SeatType.VIPseat:
                Price = 20.0;
                break;

            case SeatType.SelectedSeat:
                Price = 0.0;
                break;
        }
        return Price;
    }

    public static string GetDescription<TEnum>(TEnum value) where TEnum : Enum
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute != null ? attribute.Description : value.ToString();
    }

    public string GetSeatTypeName()
    {
        return GetDescription(SeatTypeValue);
    }
}