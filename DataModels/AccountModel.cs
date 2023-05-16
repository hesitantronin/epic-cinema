using System.Text.Json.Serialization;


class AccountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("accountType")]
    public AccountType Type { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } 

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

        [JsonPropertyName("cateringReservation")]
    public Dictionary<string, string> CateringReservation { get; set; }

    public bool Authorized = false;

    public AccountModel(int id, string emailAddress, string password, string fullName, AccountType type = AccountType.CUSTOMER)
    {
        Id = id;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        Type = type;
        CateringReservation = new Dictionary<string, string>();
    }

    public void Authorize() => Authorized = true;

    public enum AccountType 
    {
        GUEST,
        CUSTOMER,
        EMPLOYEE,
        ADMIN
    }

}