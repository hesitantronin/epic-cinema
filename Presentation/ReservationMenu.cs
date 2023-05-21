using System.Text;

class ReservationMenu
{
    static public void Start()
    {
        List<string> ReturnList = new List<string>()
        {
            "Yes",
            "No"
        };

        int option1 = OptionsMenu.DisplaySystem(ReturnList, "", "\nWill you need any special assistance to make going to our cinema a more accessible experience for you?");

        switch (option1)
        {
            case 1:
                Console.WriteLine("\nPlease write down how we can be of assistance");
                string? accessibilityReq = Console.ReadLine();
                Console.WriteLine("");

                if (accessibilityReq != null)
                {
                    AccountsLogic accountslogic = new AccountsLogic();
                    AccountsLogic.CurrentAccount.AccessibilityRequest = accessibilityReq;
                    accountslogic.UpdateList(AccountsLogic.CurrentAccount);
                }
                break;
            
            case 2:
                break;
        }

        // finalize reservation

    }
}