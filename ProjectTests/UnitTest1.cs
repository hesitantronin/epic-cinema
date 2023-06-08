namespace ProjectTests;
using System;
using System.IO;

[TestClass]
public class UnitTest1
{
    // Movies
    [TestMethod]
    public void TestMovieAddSingle()
    {
        
    }

    [TestMethod]
    public void TestMoviesAddJson()
    {
        using(StreamReader streamReader = new StreamReader(@"DataSources\testpath.txt"))
        {

            // Arrange
            Console.SetIn(streamReader);

            string l;
            while ((l = Console.ReadLine()) != null) 
            {

                // Act
                List<MovieModel> movies = MovieAccess.LoadAll();
                MovieLogic.AddMultipleMovi esJSON();
                List<MovieModel> new_movies = MovieAccess.LoadAll();

                // Assert
                Assert.AreEqual(movies.Count() + 3, new_movies.Count());
                
                // Cleanup
            }
        }



    }

    [TestMethod]
    public void TestMoviesAddCSV()
    {

    }

    [TestMethod]
    public void TestMoviesDelete()
    {

    }

    [TestMethod]
    public void TestMoviesEdit()
    {

    }

    // Catering    
    [TestMethod]
    public void TestCateringAddSingle()
    {
        
    }

    [TestMethod]
    public void TestCateringAddJson()
    {
        
    }

    [TestMethod]
    public void TestCateringAddCSV()
    {
        
    }

    [TestMethod]
    public void TestCateringDelete()
    {
        
    }

    [TestMethod]
    public void TestCateringEdit()
    {
        
    }

    // Accounts
    [TestMethod]
    public void TestAccAddEmp()
    {
        // //Arrange
        // string email = "testing@emp.com";
        // string password = "Testing1.";
        // string name = "Test Employee";
        // StringReader stringReader = new(email);
        // Console.SetIn(stringReader);

        // stringReader = new(password);
        // Console.SetIn(stringReader);
        
        // stringReader = new(name);
        // Console.SetIn(stringReader);

        // // Act
        // AccountsLogic.Register(true);
        // List<AccountModel> accounts = AccountsAccess.LoadAll();

        // // Assert
        // Assert.AreEqual(accounts[^1].EmailAddress, email);
        // Assert.AreEqual(accounts[^1].Type, AccountModel.AccountType.EMPLOYEE);
    }

    [TestMethod]
    public void TestAccAddAdmin()
    {
        // AccountsLogic.Register(false, true);
    }

    [TestMethod]
    public void TestAccDelete()
    {

    }

    // Reservations
    [TestMethod]
    public void TestResAdd()
    {

    }

    [TestMethod]
    public void TestResCancel()
    {

    }

    [TestMethod]
    public void TestResCancelEmployee()
    {

    }
}
