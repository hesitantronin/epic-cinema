namespace ProjectTests;

[TestClass]
public class UnitTest1
{

    // MOVIES //
    [TestMethod]
    public void TestMovieAddSingle()
    {
        // Set Up
        List<MovieModel> movies = MovieAccess.LoadAll();
        MovieLogic.AddOrUpdateMovieLogic(null, "Gladiator", "Adventure", 8.5, "A former Roman General sets out to exact vengeance against the corrupt emperor who murdered his family and sent him into slavery.", 16, new DateTime (2023, 12, 30, 16, 30, 0), new DateTime (1993, 10, 25, 0, 0, 0), 10.99, "1:20", new List<int> () {2});
        List<MovieModel> new_movies = MovieAccess.LoadAll();

        // Cleanup
        MovieAccess.WriteAll(movies);

        // Asserts
        Assert.AreEqual(movies.Count() + 1, new_movies.Count());
    }

    [TestMethod]
    public void TestMoviesAddJson()
    {
        // Set Up
        List<MovieModel> movies = MovieAccess.LoadAll();
        MovieLogic.AddMultipleMoviesJsonLogic(@"DataSources\testmovies.json");
        List<MovieModel> new_movies = MovieAccess.LoadAll();
        
        // Cleanup
        MovieAccess.WriteAll(movies);

        // Asserts
        Assert.AreEqual(movies.Count() + 3, new_movies.Count());
    }

    [TestMethod]
    public void TestMoviesAddCSV()
    {
        // Set Up
        List<MovieModel> movies = MovieAccess.LoadAll();
        MovieLogic.AddMultipleMoviesCsvLogic(@"DataSources\testmovies.csv");
        List<MovieModel> new_movies = MovieAccess.LoadAll();

        // Cleanup
        MovieAccess.WriteAll(movies);

        // Asserts
        Assert.AreEqual(movies.Count() + 1, new_movies.Count());
    }

    [TestMethod]
    public void TestMoviesDelete()
    {
        // Set Up
        List<MovieModel> movies = MovieAccess.LoadAll();
        MovieLogic.RemoveLogic(1);
        List<MovieModel> new_movies = MovieAccess.LoadAll();

        // Cleanup
        MovieAccess.WriteAll(movies);

        // Asserts
        Assert.AreEqual(movies.Count() - 1, new_movies.Count());
    }



    // CATERING //    
    [TestMethod]
    public void TestCateringAddSingle()
    {
        // Set Up
        List<CateringModel> foods = CateringAccess.LoadAll();
        CateringLogic.AddOrUpdateFoodLogic(null, "Bacon", "Snack", 2.99, "Salty bacon");
        List<CateringModel> new_foods = CateringAccess.LoadAll();
        
        // Cleanup
        CateringAccess.WriteAll(foods);

        // Asserts
        Assert.AreEqual(foods.Count() + 1, new_foods.Count()); 
    }

    [TestMethod]
    public void TestCateringAddJson()
    {
        // Set Up
        List<CateringModel> foods = CateringAccess.LoadAll();
        CateringLogic.AddMultipleFoodsJsonLogic(@"DataSources\testfoods.json");
        List<CateringModel> new_foods = CateringAccess.LoadAll();
        
        // Cleanup
        CateringAccess.WriteAll(foods);

        // Asserts
        Assert.AreEqual(foods.Count() + 3, new_foods.Count());    
    }

    [TestMethod]
    public void TestCateringAddCSV()
    {
        // Set Up
        List<CateringModel> foods = CateringAccess.LoadAll();
        CateringLogic.AddMultipleFoodsCsvLogic(@"DataSources\testfoods.csv");
        List<CateringModel> new_foods = CateringAccess.LoadAll();
        
        // Cleanup
        CateringAccess.WriteAll(foods);

        // Asserts
        Assert.AreEqual(foods.Count() + 1, new_foods.Count());      
    }

    [TestMethod]
    public void TestCateringDelete()
    {
        // Set Up
        List<CateringModel> foods = CateringAccess.LoadAll();
        CateringLogic.RemoveLogic(1);
        List<CateringModel> new_foods = CateringAccess.LoadAll();

        // Cleanup
        CateringAccess.WriteAll(foods);

        // Asserts
        Assert.AreEqual(foods.Count() - 1, new_foods.Count());
    }



    // ACCOUNTS // 
    [TestMethod]
    public void TestAccAddEmp()
    {
        // Set Up
        string email = "testing@emp.com";
        string password = "Testing1.";
        string name = "Test Employee";

        List<AccountModel> accounts = AccountsAccess.LoadAll();
        AccountsLogic.RegisterLogic(true, false, email, password, name);
        List<AccountModel> new_accounts = AccountsAccess.LoadAll();

        // Cleanup
        AccountsAccess.WriteAll(accounts);
        
        // Assert
        Assert.AreEqual(new_accounts[^1].EmailAddress, email);
        Assert.AreEqual(new_accounts[^1].Type, AccountModel.AccountType.EMPLOYEE);
    }

    [TestMethod]
    public void TestAccAddAdmin()
    {
        // Set Up
        string email = "testing@admin.com";
        string password = "Testing1.";
        string name = "Test Admin";

        List<AccountModel> accounts = AccountsAccess.LoadAll();
        AccountsLogic.RegisterLogic(false, true, email, password, name);
        List<AccountModel> new_accounts = AccountsAccess.LoadAll();

        // Cleanup
        AccountsAccess.WriteAll(accounts);
        
        // Assert
        Assert.AreEqual(new_accounts[^1].EmailAddress, email);
        Assert.AreEqual(new_accounts[^1].Type, AccountModel.AccountType.ADMIN);
    }

    [TestMethod]
    public void TestAccDelete()
    {
        // Set Up
        List<AccountModel> accounts = AccountsAccess.LoadAll();
        AccountsLogic.RemoveAcc(1);
        List<AccountModel> new_accounts = AccountsAccess.LoadAll();

        // Cleanup
        AccountsAccess.WriteAll(accounts);

        // Asserts
        Assert.AreEqual(accounts.Count() - 1, new_accounts.Count());
    }



    // RESERVATIONS // 
    [TestMethod]
    public void TestResAdd()
    {
        // Set Up
        MovieModel movie = MovieAccess.LoadAll()[0];
        Dictionary<string, string> food = new Dictionary<string, string>{{"Popcorn", "2"}};
        List<SeatModel> seats = new() {new SeatModel("A5", 2)};
        AccountModel account = new AccountModel(999, "mail", "password", "Name");
        AccountsLogic.CurrentAccount = account;
        account.Movie = movie;
        account.CateringReservation = food;
        account.SeatReservation = seats;

        string resCode = "892384959237";
        double finalPrice = 44.50;

        List<ReservationsModel> reservations = ReservationsAccess.LoadAll();
        ReservationMenu.ReservationToJson(resCode, finalPrice);
        List<ReservationsModel> new_reservations = ReservationsAccess.LoadAll();

        // Cleanup
        ReservationsAccess.WriteAll(reservations);

        // Assert
        Assert.AreEqual(reservations.Count() + 1, new_reservations.Count());
    }

    [TestMethod]
    public void TestResCancel()
    {
        // Set Up
        List<ReservationsModel> reservations = ReservationsAccess.LoadAll();

        string pathToCsv = $@"DataSources/MovieAuditoriums/TheShawshankRedemption/ID_1_TheShawshankRedemption_10_12_2023_10_30_00.csv";
        string[][] seatArray = SeatAccess.LoadAuditorium(pathToCsv);

        ReservationsLogic.CancelResLogic(reservations[0]);

        // Cleanup
        ReservationsAccess.WriteAll(reservations);
        SeatAccess.WriteToCSV(seatArray, pathToCsv);

        // Assert
        Assert.AreEqual(reservations[0].Cancelled, true);
    }
}
