using Spectre.Console;
using static System.Runtime.InteropServices.JavaScript.JSType;
static class ReviewPresentation
{
    public static void Present()
    {
        Console.Clear();
        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please select an option:[/]")
            .AddChoices(new[]
            {
                    "Leave a Review", "View Reviews", "Back"
            });

        string userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        switch (userSelection)
        {
            case "Back":
                Console.Clear();
                return;
            case "Leave a Review":
                Console.Clear();
                LeaveReview();
                break;
            case "View Reviews":
                Console.Clear();
                ReadReviews();
                break;
        }
    }

    private static void LeaveReview()
    {
        if (!ReviewLogic.ReservationCheck(State.LoggedInUser.ID))
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[red]You must have a reservation in the past 90 days to leave a review. Press any key to continue.[/]");
            Console.ReadKey();
            return;
        }

        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please choose a rating:[/]")
            .AddChoices(new[]
            {
                    "1", "2", "3", "4", "5", "Back"
            });

        string rating = AnsiConsole.Prompt(userSelectionPrompt);

        if (rating == "Back")
            return;

        string review = AnsiConsole.Prompt(
            new TextPrompt<string>("Write a Review [grey](optional)[/]:")
            .AllowEmpty());

        try
        {
            Database.InsertReviewsTable(State.LoggedInUser.ID, Int32.Parse(rating), review, DateOnly.FromDateTime(DateTime.Now));
            AnsiConsole.MarkupLine("[green]Your review has been added![/]");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void ReadReviews()
    {
        Console.Clear();
        var selection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Sort by?")
        //.Title("[yellow]Please make a choice:[/]")
        .AddChoices(new[] { "Rating (Highest to Lowest)", "Rating (Lowest to Highest)", "Back" }));

        // Handle user selection
        List<Review> reviews = new();
        switch (selection)
        {
            case "Rating (Lowest to Highest)":
                reviews = Database.GetAllReviews().OrderBy(r => r.Rating).ToList();
                break;
            case "Rating (Highest to Lowest)":
                reviews = Database.GetAllReviews().OrderByDescending(r => r.Rating).ToList();
                break;
            case "Back":
                return;
        }

        Console.Clear();
        AnsiConsole.Write(new Rule($"[maroon]User Reviews[/]"));
        var table = new Table().Centered();
        //List<Review> reviews = Database.GetAllReviews();

        // Animate
        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .Start(ctx =>
            {
                void Update(int delay, Action action)
                {
                    action();
                    ctx.Refresh();
                    Thread.Sleep(delay);
                }

                // Columns
                Update(200, () => table.AddColumn("Username"));
                Update(200, () => table.AddColumn("Rating"));
                Update(200, () => table.AddColumn("Review"));
                Update(200, () => table.AddColumn("Date"));
                Update(200, () => table.AddColumn("Reply"));
                //Update(200, () => table.AddColumn("Gluten Free"));

                // Rows
                foreach (Review review in reviews)
                {
                    Update(100, () => table.AddRow(Database.GetUserByID(review.User).GetFullName(), $"{review.Rating}/5", review.UserMessage, $"{review.Date}", review.AdminMessage is null ? "" : review.AdminMessage));
                }
            });
        AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
}