using System.ComponentModel;
using Newtonsoft.Json;
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
        GetData();
    }

    public double GetData()
    {
        var SeatData = SeatAccess.LoadGlobalSeatData();

        switch (SeatTypeValue)
        {
            case SeatType.OccupiedSeat:
                Price = 0.0;
                break;

            case SeatType.SeatType1:
                var seatType1Data = SeatData.FirstOrDefault(x => x.Key == 1);

                Price = seatType1Data.Value.Item2;
                SeatTypeName = seatType1Data.Value.Item1;

                break;

            case SeatType.SeatType2:
                var seatType2Data = SeatData.FirstOrDefault(x => x.Key == 2);
                
                Price = seatType2Data.Value.Item2;
                SeatTypeName = seatType2Data.Value.Item1;
                
                break;

            case SeatType.SeatType3:
                var seatType3Data = SeatData.FirstOrDefault(x => x.Key == 3);

                Price = seatType3Data.Value.Item2;
                SeatTypeName = seatType3Data.Value.Item1;
        
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