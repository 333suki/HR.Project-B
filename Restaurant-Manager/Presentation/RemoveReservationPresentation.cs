using Spectre.Console;

public static class RemoveReservationPresentation {
    public static void Present() {
        AnsiConsole.Write(new Rule($"[maroon]Remove Reservation ({State.LoggedInUser.GetFullName()})[/]"));
        List<Reservation> userReservations = ReservationLogic.GetReservationsByUserID(State.LoggedInUser.ID);

        string reservationChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Select reservation to remove:[/]")
                .AddChoices(ReservationLogic.ReservationsToString(userReservations))
        );

        if (reservationChoice == "Back" || !ConfirmDeletion()) {
            return;
        }

        long reservationID = ReservationLogic.ParseIDFromString(reservationChoice);
        
        if (reservationID == HiddenDiscount.HiddenCodeID)
        {
            Console.WriteLine("This reservation exists only to contain the hidden discount. You cannot remove this.");
            AnsiConsole.Markup("[gray]Press any key to continue...[/]");
            Console.ReadKey();
            return;
        }
        
        Database.DeleteReservationsTable(reservationID);
        AnsiConsole.WriteLine("Reservation Removed");
        AnsiConsole.Markup("[gray]Press any key to continue...[/]");
        Console.ReadKey();
    }

    private static bool ConfirmDeletion() {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Do you really want to remove this reservation?")
                .AddChoices("Yes", "No"));

        if (choice == "No") {
            return false;
        }
        return true;
    }
}
