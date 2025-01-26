using Spectre.Console;

static class EditReservationPresentation
{
    public static void Present()
    {
        AnsiConsole.Write(new Rule($"[yellow]Edit Reservation ({State.LoggedInUser.GetFullName()})[/]"));
        long currentUserID = State.LoggedInUser.ID;
        
        Reservation reservationToEdit = SelectReservation(currentUserID);

        if (reservationToEdit is null)
        {
            return;
        }
        DateOnly? date = reservationToEdit.Date;
        string dateString = $"{date?.Day}-{date?.Month}-{date?.Year}";
        string timeslot = reservationToEdit.Timeslot;
        int groupSize = reservationToEdit.GroupSize;
        
        List<string> dataToChange = InfoToEdit();

        if (!dataToChange.Any()) return;


        foreach (string variable in dataToChange)
        {
            if (variable == "Date")
            {
                date = EditDate();
                if (date == null) {
                    return;
                }
                dateString = $"{date?.Day}-{date?.Month}-{date?.Year}";
                reservationToEdit.UpdateDate(date);
            }
            if (variable == "Timeslot")
            {
                timeslot = EditTimeslot();
                reservationToEdit.UpdateTimeslot(timeslot);
            }
            if (variable == "Group size")
            {
                groupSize = EditGroupSize();
                reservationToEdit.UpdateGroupSize(groupSize);
            }
        }

        string locMessage = ReservationLogic.GetLocationDescription(reservationToEdit.LocationID);

        (bool success, string message) = ReservationLogic.UpdateReservation(reservationToEdit);
        if (success)
        {
            if (!EmailService.SendEditedReservationEmail(State.LoggedInUser.GetFullName(), Database.GetLocationByID(reservationToEdit.LocationID)?.City, ReservationLogic.GetLocationName(reservationToEdit.LocationID), dateString, timeslot, groupSize, State.LoggedInUser.Email)) {
                AnsiConsole.MarkupLine("[gray]Press any key to continue...[/]");
                Console.ReadKey();
                return;
            }
            
            string text = $"[green]Your reservation has been edited.[/]\nYour Table Number: {reservationToEdit.Table}\n\n{locMessage}\n\nPress any key to continue.";

            Panel panel = new(new Markup(text).Centered()); // Update the panel and the text in it with the updated buffer
            panel.Expand = true; // Set expand again
            Console.Clear();
            AnsiConsole.Write(panel);
            Console.ReadKey();
        }
        else
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    public static Reservation SelectReservation(long currentUserID)
    {
        List<Reservation> userReservations = ReservationLogic.GetReservationsByUserID(currentUserID);

        var reservationChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[cyan]Select a reservation to edit:[/]")
            .AddChoices(ReservationLogic.ReservationsToString(userReservations)));

        if (reservationChoice == "Back")
        {
            return null;
        }
        
        long reservationID = ReservationLogic.ParseIDFromString(reservationChoice);

        return ReservationLogic.GetReservationByID(reservationID);
    }

    public static List<string> InfoToEdit()
    {
        var dataToChange = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("What data would you like to edit?")
                .NotRequired()
                .InstructionsText(
                    "[gray](Press [blue]space[/] to select what to edit, " + 
                    "[green]Enter[/] to accept)[/]")
                .AddChoices(new[] {"Date", "Timeslot", "Group size"}));
        
        return dataToChange;
    }

    public static DateOnly? EditDate()
    {
        (int Day, int Month, int Year) Date = ReservationPresentation.SelectDate();
        if (Date == (0, 0, 0)) {
            return null;
        }
        string dateString = $"{Date.Day}-{Date.Month}-{Date.Year}";
        DateOnly date = ReservationLogic.ParseDate(dateString);

        return date;
    }

    public static string EditTimeslot()
    {
        string timeslot = ReservationPresentation.SelectTimeslot();
        return timeslot;
    }

    public static int EditGroupSize()
    {
        int groupSize = ReservationPresentation.SelectGroupSize();
        return groupSize;
    }
}

//asdasd