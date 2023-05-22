using System.ComponentModel;
public enum SeatType 
{
    [Description("Occupied Seat")]
    OccupiedSeat,
    [Description("Normal Seat")]
    NormalSeat,
    [Description("VIP Seat")]
    VIPseat,
    [Description("Love Seat")]
    Loveseat,
    [Description("Currently Selected Seat")]
    SelectedSeat
}