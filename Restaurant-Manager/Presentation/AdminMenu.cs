using Spectre.Console;

static class AdminMenu
{
    public static void ShowAdminMenu()
    {
        while (State.LoggedInUser is not null)
        {
            Console.Clear();

            AnsiConsole.Write(new Rule($" [blue]Admin Menu ({State.LoggedInUser.GetFullName()})[/] "));

            var userSelectionPrompt = new SelectionPrompt<string>()
                .Title("[cyan]Please select an option:[/]")
                .AddChoices(new[] { "Manage reservations", "Manage dishes", "Manage reviews", "View all users", "Logout" });

            var userSelection = AnsiConsole.Prompt(userSelectionPrompt);

            switch (userSelection)
            {
                case "Manage reservations":
                    ShowManageReservationsMenu();
                    break;
                case "Manage dishes":
                    ShowManageDishesMenu();
                    break;
                case "Manage reviews":
                    ShowManageReviewsMenu();
                    break;
                case "View all users":
                    ViewUsers.ViewAllUsers();
                    AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
                    Console.ReadKey();
                    break;
                case "Logout":
                    AnsiConsole.MarkupLine("[red]Logging out...[/]");
                    State.LoggedInUser = null;
                    Thread.Sleep(1000);
                    Console.Clear();
                    MainMenuPresentation.ShowMainMenu();
                    break;
            }
        }
    }

    private static void ShowManageDishesMenu()
    {
        Console.Clear();

        AnsiConsole.Write(new Rule($" [blue]Manage Dishes ({State.LoggedInUser.GetFullName()})[/] "));

        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please select an option:[/]")
            .AddChoices(new[] { "View dishes", "Add dish", "Edit dish", "Delete dish", "Back" });

        var userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        switch (userSelection)
        {
            case "View dishes":
                MenuCard.DisplayMenuCard();
                AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
                Console.ReadKey();
                break;
            case "Add dish":
                MenuManagement.AddDish();
                break;
            case "Edit dish":
                MenuManagement.EditDish();
                break;
            case "Delete dish":
                MenuManagement.DeleteDish();
                break;
            case "Back":
                break;
        }
    }

    private static void ShowManageReservationsMenu()
    {
        Console.Clear();

        AnsiConsole.Write(new Rule($" [blue]Manage Reservations ({State.LoggedInUser.GetFullName()})[/] "));

        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please select an option:[/]")
            .AddChoices(new[] { "View reservations", "Create reservation", "Edit reservation", "Delete reservation", "Back" });

        var userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        switch (userSelection)
        {
            case "View reservations":
                ShowViewReservationsMenu();
                break;
            case "Create reservation":
                ReservationManagement.CreateReservation();
                break;
            case "Edit reservation":
                ReservationManagement.EditReservation();
                break;
            case "Delete reservation":
                ReservationManagement.DeleteReservation();
                break;
            case "Back":
                break;
        }
    }

    private static void ShowViewReservationsMenu()
    {
        Console.Clear();

        AnsiConsole.Write(new Rule($" [blue]View Reservations ({State.LoggedInUser.GetFullName()})[/] "));

        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please select an option:[/]")
            .AddChoices(new[] { "View all", "Search by email", "Back" });

        var userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        switch (userSelection)
        {
            case "View all":
                ReservationManagement.ViewAllReservations();
                AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
                Console.ReadKey();
                break;
            case "Search by email":
                ReservationManagement.ViewReservationsByEmail();
                break;
            case "Back":
                break;
        }
    }

    private static void ShowManageReviewsMenu()
    {
        Console.Clear();

        AnsiConsole.Write(new Rule($" [blue]Manage Reviews ({State.LoggedInUser.GetFullName()})[/] "));

        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please select an option:[/]")
            .AddChoices(new[] { "View reviews", "Reply to review", "Delete review", "Back" });

        var userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        switch (userSelection)
        {
            case "View reviews":
                PrintAllReviews();
                break;
            case "Reply to review":
                SelectReviewForAction("Reply");
                break;
            case "Delete review":
                SelectReviewForAction("Delete");
                break;
            case "Back":
                break;
        }
    }

    private static void SelectReviewForAction(string action)
    {
        var reviews = Database.GetAllReviews();
        if (!reviews.Any())
        {
            AnsiConsole.MarkupLine("[red]No reviews available to manage.[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            Console.ReadKey();
            return;
        }

        var reviewSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Select a review to manage:[/]")
            .AddChoices(reviews.Select(review => $"{review.ID}: {review.UserMessage ?? "No review text"}").ToArray().Append("Back"));

        var selectedReviewString = AnsiConsole.Prompt(reviewSelectionPrompt);

        if (selectedReviewString == "Back") {
            return;
        }
        var selectedReviewId = long.Parse(selectedReviewString.Split(':')[0]);
        var selectedReview = reviews.First(review => review.ID == selectedReviewId);

        switch (action)
        {
            case "Reply":
                ReviewOperations.ReplyToReview(selectedReview);
                break;
            case "Delete":
                ReviewOperations.DeleteReview(selectedReview);
                break;
        }
    }


    private static void PrintAllReviews()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule($" [blue]All Reviews[/] "));

        var reviews = Database.GetAllReviews();
        var table = new Table().Centered();

        table.AddColumn("Review ID");
        table.AddColumn("User");
        table.AddColumn("Rating");
        table.AddColumn("Review");
        table.AddColumn("Date");
        table.AddColumn("Reply");

        foreach (var review in reviews)
        {
            table.AddRow(
                review.ID.ToString(),
                Database.GetUserByID(review.User).GetFullName(),
                review.Rating.ToString(),
                review.UserMessage ?? "No review text",
                review.Date.ToString(),
                review.AdminMessage ?? "No reply"
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
}
