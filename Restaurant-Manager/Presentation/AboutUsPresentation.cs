using Spectre.Console;

static class AboutUsPresentation
{
    public static void DisplayAboutUs()
    {
        Console.Clear();

        AnsiConsole.Write(new Rule("[bold maroon]About Us[/]").RuleStyle("maroon"));

        AnsiConsole.MarkupLine("The best escape rooms in The Netherlands!");
        AnsiConsole.MarkupLine("Will you conquer the most thrilling experience?\n");

        AnsiConsole.MarkupLine("Immerse yourself in one of our escape rooms, and who knows what might await you at the end.");
        AnsiConsole.MarkupLine("At our escape rooms, you’ll experience an exhilarating adventure where you (and your team) are locked inside one of our unique themed rooms. What’s the challenge? You must give all your brain power to unravel riddles, solve puzzles, and complete various tasks to escape within 60 minutes.");
        AnsiConsole.MarkupLine("[italic grey]Each room is a portal to a different world, each with its own secrets to unravel.[/]\n");

        AnsiConsole.Write(new Rule("[bold maroon]Choose Your Location[/]").RuleStyle("maroon"));

        var userSelectionPrompt = new SelectionPrompt<string>()
        .AddChoices(new[]
        {
                        "Amsterdam", "Groningen", "Rotterdam", "Utrecht", "Zwolle", "Back"
        });

            string userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        if (userSelection == "Back")
        {
            return;
        }

        List<Location> chosenLocations = Database.GetAllLocations().Where(l => l.City == userSelection).ToList();

        foreach (Location location in chosenLocations)
        {
            AnsiConsole.Write(new Panel($"[italic]{location.Storyline}[/]")
                .Header($"[bold maroon]{location.Name}[/]")
                .Expand());
                Console.WriteLine();
        }

        AnsiConsole.MarkupLine("[grey italic]Escape & Dine offers an adventure like no other, full of mystery, puzzles and unforgettable surprises.[/]");
        AnsiConsole.MarkupLine("[italic]We can't wait to see you![/]\n");

        AnsiConsole.MarkupLine("[italic grey]Press any key to return...[/]");
        Console.ReadKey();
        Console.Clear();
        Console.WriteLine("\x1b[3J");
    }
}