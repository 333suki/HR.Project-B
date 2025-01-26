using Spectre.Console;

static class MainMenuPresentation {
    public static void ShowMainMenu() {
        while (true) {
            AnsiConsole.Clear();
            // Display welcome message in a box
            AnsiConsole.Write(new Rule("[maroon] Welcome [/]"));

            // Display selection menu
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("")
                    //.Title("[yellow]Please make a choice:[/]")
                    .AddChoices(new[] { "Login", "Register", "Forgot Password", "Exit" }));

            // Handle user selection
            switch (selection) {
                case "Login":
                    LoginPresentation.Present();
                    break;
                case "Register":
                    RegisterPresentation.Present(true);
                    break;
                case "Forgot Password":
                    PasswordRecoveryPresentation.Present();
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[red]Exiting the program...[/]");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
            }

            if (State.LoggedInUser?.Role == "USER") {
                UserMenu.ShowUserMenu();
            } else {
                AdminMenu.ShowAdminMenu();
            }
        }
    }
}
