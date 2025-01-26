using Spectre.Console;

static class RegisterPresentation
{
    public static void Present(bool InsertIntoUsersTable)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Rule(" [yellow]Register[/] ")
        );

        AnsiConsole.WriteLine();

        // Ask for new user email
        string Email = PromptEmail().ToLower();
        if (string.IsNullOrEmpty(Email)) {
            AnsiConsole.Clear();
            MainMenuPresentation.ShowMainMenu();
        }

        // Ask for new user password
        string Password = PromptPassword(Email);
        if (string.IsNullOrEmpty(Password)) {
            AnsiConsole.Clear();
            MainMenuPresentation.ShowMainMenu();
        }

        // Confirm password
        string ConfirmPassword = PromptPasswordConfirmation(Email, Password);
        if (string.IsNullOrEmpty(ConfirmPassword)) {
            AnsiConsole.Clear();
            MainMenuPresentation.ShowMainMenu();
        }

        if (Password != ConfirmPassword) {
            AnsiConsole.MarkupLine("[red]Passwords do not match. Please try registering again.[/]");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        // Ask for user first name
        string FirstName = PromptFirstName(Email, Password);

        // Ask for user last name
        string LastName = PromptLastName(Email, Password, FirstName);

        // Optionally insert a new user into the table
        if (InsertIntoUsersTable)
        {
            Database.InsertUsersTable(Email, Password, FirstName, LastName, "USER");
        }

        // Display registration success message and redirect to choice page
        //ShowSuccessMessageAndRedirect();
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold green]Registration Successful![/]");
        AnsiConsole.MarkupLine("[bold yellow]Please log in.[/]");
        Console.Read();
        AnsiConsole.Clear();
    }

    private static string PromptEmail()
    {
        AnsiConsole.MarkupLine("[blue]Please enter your email, or leave empty to cancel:[/]");
        while (true)
        {
            string Email = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Email:[/]")
                .PromptStyle("yellow")
                .Validate(n => {
                    // Another user with that Email already exists
                    if (Database.UsersTableContainsUser(n.ToLower()))
                    {
                        return ValidationResult.Error("[red]This email is already registered to another account[/]");
                    }

                    // Invalid Email length, null or only whitespace
                    if (!Util.EmailValidOrEmpty(n))
                    {
                        return ValidationResult.Error("[red]Invalid email[/]");
                    }

                    // If all checks pass
                    return ValidationResult.Success();
                })
                .AllowEmpty()
            ).ToLower();

            return Email;
        }
    }

    private static string PromptPassword(string Email)
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule(" [yellow]Register[/] ")
            );
            AnsiConsole.WriteLine();
            //AnsiConsole.MarkupLine("[blue]Please enter your email, or leave empty to cancel:[/]");
            AnsiConsole.MarkupLine($"[green]Email:[/] {Email}");

            AnsiConsole.MarkupLine("[blue]Please enter your password, or leave empty to cancel [/][gray](min. 8 characters)[/]:");
            string Password = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Enter your password:[/]")
                .PromptStyle("yellow")
                .Validate(p => {
                    // Invalid password length, null or only whitespace
                    if (!Util.PasswordValidOrEmpty(p))
                    {
                        return ValidationResult.Error("[red]Password must be at least 8 characters long[/]");
                    }

                    // If all checks pass
                    return ValidationResult.Success();
                })
                .Secret('*')
                .AllowEmpty()
            );

            return Password;
        }
    }

    private static string PromptPasswordConfirmation(string Email, string Password) {
        while (true) {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Rule(" [yellow]Register[/] ")
            );
            AnsiConsole.WriteLine();
            //AnsiConsole.MarkupLine("[blue]Please enter your email, or leave empty to cancel:[/]");
            AnsiConsole.MarkupLine($"[green]Email:[/] {Email}");
            //AnsiConsole.MarkupLine("[blue]Please enter your password, or leave empty to cancel:[/]");
            AnsiConsole.MarkupLine($"[green]Password:[/] {new string('*', Password.Length)}");

            AnsiConsole.MarkupLine("[blue]Please enter your password again, or leave empty to cancel:[/]");
            string ConfirmPassword = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Enter your password:[/]")
                .PromptStyle("yellow")
                //.Validate(p => {
                //    // If all checks pass
                //    return ValidationResult.Success();
                //})
                .Secret('*')
                .AllowEmpty()
            );

            return ConfirmPassword;
        }
    }

    private static string PromptFirstName(string Email, string Password)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Rule(" [yellow]Register[/] ")
        );
        AnsiConsole.MarkupLine($"[green]Email:[/] {Email}");
        AnsiConsole.MarkupLine($"[green]Password:[/] {new string('*', Password.Length)}");
        return AnsiConsole.Prompt(new TextPrompt<string>("[blue]First name (Optional):[/]").AllowEmpty());
    }

    private static string PromptLastName(string Email, string Password, string FirstName)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Rule(" [yellow]Register[/] ")
        );
        AnsiConsole.MarkupLine($"[green]Email:[/] {Email}");
        AnsiConsole.MarkupLine($"[green]Password:[/] {new string('*', Password.Length)}");
        AnsiConsole.MarkupLine($"[blue]First name (Optional):[/] {FirstName}");
        return AnsiConsole.Prompt(new TextPrompt<string>("[blue]Last name (Optional):[/]").AllowEmpty());
    }

    private static void ShowSuccessMessageAndRedirect()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold green]Registration Successful![/]");
        AnsiConsole.MarkupLine("[bold yellow]Press any key to continue...[/]");
        Console.Read();
        AnsiConsole.Clear();

        // Redirect to the choice page
        MainMenuPresentation.ShowMainMenu();
    }
}
