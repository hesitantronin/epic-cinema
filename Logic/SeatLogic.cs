using System.Text.RegularExpressions;

static class SeatLogic
{
    public static void SeatSelection(MovieModel movie, bool isEdit = false)
    {
        List<string> answerList = new List<string>()
        {
            "Yes",
            "Add more seats",
            "Remove seats"
        };
        List<string> editList = new List<string>()
        {
            "Edit availability",
            "Edit seat type (price range)",
        };
        List<string> seatType = new List<string>()
        {
            "Normal seat",
            "VIP seat",
            "Lover seat"
        };

        // Regex pattern for checking the validity of the input ID later in the selection process
        string pattern = @"^[a-l]([1-9]|1[0-4])$";

        // List and string for keeping track of the selections
        List<string> selectedChairs = new();
        string currentlySelectedChair = string.Empty;

        // Creating the CSV'S with corresponding names
        string movieTitle = Regex.Replace(movie.Title, @"[^0-9a-zA-Z\._]", string.Empty);
        string[] movieViewingDate1 = movie.ViewingDate.ToString().Split(" ");
        string movieViewingDate2 = string.Join(" ", movieViewingDate1[1].Replace(":", "_"));
        string movieViewingDate3 = string.Join(" ", movieViewingDate1[0].Replace("/", "_"));

        string pathToCsv = $@"DataSources/MovieAuditoriums/{movieTitle}/ID_{movie.Id}_{movieTitle}_{movieViewingDate3 + "_" + movieViewingDate2}.csv";

        if (!File.Exists(pathToCsv)) // Checks if a CSV already exists, if it does that CSV will be loaded, otherwise a new one will be made based on the template
        {
            SeatAccess.NewAuditorium(movie);
        }

        string[][] auditoriumArray = SeatAccess.LoadAuditorium(pathToCsv); // Initialise array for searching and updating values

        // A bool and a variable used to reuse the same loop for removing seats from your selection
        bool removingMode = false;
        string four = "4";

        // Looping the selection of the seats until the user has selected all seats they'd want
        while (true)
        {
            // Visualisation of the menu
            Console.Clear();
            OptionsMenu.Logo("Seat selection");
            SeatAccess.PrintAuditorium(auditoriumArray);
            SeatsMenu.SeatLegend(movie);

            // Ask user for id of the seat
            Console.WriteLine($"Selected seats: [{String.Join(", ", selectedChairs)}]");
            Console.WriteLine($"Type in the ID of the seat you want to {(isEdit ? "edit" : (removingMode ? "remove from your selection" : "select"))} (I.E. - A6)");

            currentlySelectedChair = Console.ReadLine();

            // If removing mode is on the 4 check in the csv will be negated so you can remove your own selections
            // Otherwise if removing mode is off, the check will be on and you cannot select the seats you selected again
            four = removingMode ? "" : "4";

            // Checking the validity of the input ID and preventing any crashes
            if (!string.IsNullOrEmpty(currentlySelectedChair))
            {
                if (Regex.IsMatch(currentlySelectedChair, pattern, RegexOptions.IgnoreCase))
                {
                    if (SeatAccess.FindSeatValueInArray(auditoriumArray, currentlySelectedChair.ToUpper()) != "0" &&
                        SeatAccess.FindSeatValueInArray(auditoriumArray, currentlySelectedChair.ToUpper()) != four &&
                        SeatAccess.FindSeatValueInArray(auditoriumArray, currentlySelectedChair.ToUpper()) != "")
                    {

                        // Check if the user selected the option to remove a movie and change the logic based on that
                        if (!removingMode)
                        {
                            selectedChairs.Add(currentlySelectedChair.ToUpper());

                            // Update array to show which chairs are currently selected in the selection process
                            foreach (string seat in selectedChairs)
                            {
                                SeatAccess.UpdateSeatValue(auditoriumArray, seat, "4");
                            }
                        }

                        else
                        {
                            SeatAccess.UpdateSeatValue(auditoriumArray, currentlySelectedChair.ToUpper(), SeatAccess.FindDefaultSeatValueArray(currentlySelectedChair.ToUpper()));
                            selectedChairs.Remove(currentlySelectedChair.ToUpper());
                        }

                        Console.Clear();
                        OptionsMenu.Logo("Seat selection");
                        SeatAccess.PrintAuditorium(auditoriumArray);
                        SeatsMenu.SeatLegend(movie);
                        
                        // Prepare option for use in checking if there are any seats selected
                        int optionInLoop = 0;

                        // If there are no seats selected option 2 is automatically selected and the user is prompted to select a seat again
                        if (!selectedChairs.Any()) optionInLoop = 2;
                        
                        else optionInLoop = OptionsMenu.DisplaySystem(answerList, "", $"You've Selected seat(s) [{String.Join(", ", selectedChairs)}], are you satisfied with these selections?", false, true);

                        if (optionInLoop == 1)
                        {
                            if (isEdit)
                            {
                                Console.Clear();
                                int seatEdit = OptionsMenu.DisplaySystem(editList, "", $"Select an option what you want to do with the following seats: {String.Join(", ", selectedChairs)}", false, true);
                                if (seatEdit == 1)
                                {
                                    //edit availability function
                                }
                                else if (seatEdit == 2)
                                {
                                    //edit seat type or price function
                                    int typeEdit = OptionsMenu.DisplaySystem(seatType, "", $"Select an option what you want change the following seat(s) in: {String.Join(", ", selectedChairs)}", false, true);

                                    foreach (string chair in selectedChairs)
                                    SeatAccess.UpdateSeatValue(auditoriumArray, chair, Convert.ToString(typeEdit));

                                    if (typeEdit == 4)
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else if (optionInLoop == 2)
                        {
                            removingMode = false;
                        }

                        else if (optionInLoop == 3)
                        {
                            removingMode = true;
                        }

                        else if (optionInLoop == 4)
                        {
                            return;
                        }
                    }

                    else
                    {
                        List<string> EList = new List<string>(){"Continue"};
                        int option = OptionsMenu.DisplaySystem(EList, "", "\nThat seat is already occupied or it does not exist", false, true);

                        if (option == 1)
                        {
                            continue;
                        }

                        if (option == 2)
                        {
                            return;
                        }
                    }

                }

                else
                {
                    List<string> EList = new List<string>(){"Continue"};
                    int option = OptionsMenu.DisplaySystem(EList, "", "\nThat is not a seat ID", false, true);

                    if (option == 1)
                    {
                        continue;
                    }

                    if (option == 2)
                    {
                        return;
                    }
                }
            }

            else
            {
                List<string> EList = new List<string>(){"Continue"};
                int option = OptionsMenu.DisplaySystem(EList, "", "\nPlease fill in the ID of a seat", false, true);

                if (option == 1)
                {
                    continue;
                }

                if (option == 2)
                {
                    return;
                }
            }

        }
        Console.Clear();
        OptionsMenu.Logo("Seat selection");

        // Going to food reservations and saving the reserved seats/movie to the current account
        if (AccountsLogic.CurrentAccount != null)
        {
            List<SeatModel> finalSeatSelection = new();

            foreach (string ID in selectedChairs)
            {
                SeatAccess.FindDefaultSeatValueArray(ID);
                finalSeatSelection.Add(new SeatModel(ID, Convert.ToInt32(SeatAccess.FindDefaultSeatValueArray(ID))));
            }

            AccountsLogic accountslogic = new AccountsLogic();
            AccountsLogic.CurrentAccount.Movie = movie;
            AccountsLogic.CurrentAccount.SeatReservation = finalSeatSelection;
            accountslogic.UpdateList(AccountsLogic.CurrentAccount);
        }

        if (!isEdit)
        {
            List<string> ReturnList = new List<string>
            {
                "Yes",
                "No"
            };

            int option4 = OptionsMenu.DisplaySystem(ReturnList, "", "\nWould you like to reserve catering menu items?", true, false);

            switch(option4)
            {
                case 1:
                    Console.Clear();
                    CateringMenu.Start();
                    break;
                case 2:
                    Console.Clear();
                    ReservationMenu.Start();
                    break;
            }
        }
    }
}