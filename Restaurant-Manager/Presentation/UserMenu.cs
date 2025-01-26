using Spectre.Console;


class UserMenu
{
    public static void ShowUserMenu()
    {
        HiddenDiscount.RemoveCodeFromMenu();
        HiddenDiscount.RemoveCodeFromReservations();
        HiddenDiscount.ChangeMenuHeadToFalse();
        HiddenDiscount.InsertCodeIntoUI();
        
        while (State.LoggedInUser is not null)
        {
            Console.Clear();
            if (HiddenDiscount.ChangeMenuHead)
            {
                AnsiConsole.Write(new Rule($"[maroon]Main Menu ({HiddenDiscount.RandomCodePicker()}) ({State.LoggedInUser.GetFullName()})[/]"));
            }
            else AnsiConsole.Write(new Rule($"[maroon]Main Menu ({State.LoggedInUser.GetFullName()})[/]"));

            var userSelectionPrompt = new SelectionPrompt<string>()
                .Title("[gray]A 10% off discount code is hidden somewhere...[/]\n[cyan]Please select an option:[/]")
                .AddChoices(new[]
                {
                    "Manage Reservations", "View Menu", "About Us", "Reviews", "Sign Out"
                });
            
            string userSelection = AnsiConsole.Prompt(userSelectionPrompt);

            switch (userSelection)
            {
                case "Manage Reservations":
                    Console.Clear();
                    ReservationPresentation.Present();
                    break;
                case "View Menu":
                    MenuCard.DisplayMenuCard();
                    AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
                    Console.ReadKey();
                    break;
                case "About Us":
                    Console.Clear();
                    AboutUsPresentation.DisplayAboutUs();
                    break;
                case "Reviews":
                    Console.Clear();
                    ReviewPresentation.Present();
                    break;
                case "Sign Out":
                    AnsiConsole.MarkupLine("[red]Logging out...[/]");
                    State.LoggedInUser = null;
                    Thread.Sleep(1000);
                    Console.Clear();
                    MainMenuPresentation.ShowMainMenu();
                    break;
            }
        }
    }
}