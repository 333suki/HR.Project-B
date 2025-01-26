using Spectre.Console;

static class PasswordRecoveryPresentation {
    public static void Present() {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Rule("[maroon]Forgot Password[/]")
        );

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[blue]Enter the email for which you forgot the password, or leave empty to cancel.[/]");
        string Email = PromptEmail();
        if (string.IsNullOrEmpty(Email)) {
            AnsiConsole.Clear();
            return;
        }

        string Code = Util.RandomString(5);
        AnsiConsole.MarkupLine($"[blue]Sending a recovery email to {Email}, this may take a moment.[/]");
        if (!EmailService.SendPasswordForgorEmail(Email, Code)) {
            AnsiConsole.MarkupLine("[gray]Press any key to continue...[/]");
            Console.ReadKey();
            return;
        }
        AnsiConsole.MarkupLine("[blue]Email sent, please enter the code provided in the recovery email.[/]");

        int attempts = 0;
        bool codeCorrect = false;
        while (attempts < 3) {
            string EnteredCode = PromptCode();
            AnsiConsole.Clear();

            if (EnteredCode == Code) {
                codeCorrect = true;
                break;
            } else {
                attempts++;
                AnsiConsole.MarkupLine($"[red]Incorrect code. You have {3 - attempts} attempts left.[/]");
            }
        }

        if (!codeCorrect) {
            AnsiConsole.MarkupLine("[red]You have entered the wrong code 3 times. Returning to the main menu.[/]");
            AnsiConsole.MarkupLine("[bold yellow]Press any key to continue...[/]");
            Console.ReadKey();
            AnsiConsole.Clear();
            return;
        }

        while (true) {
            AnsiConsole.MarkupLine("[blue]Please enter your new password [/][gray](min. 8 characters)[/]:");
            string NewPassword = PromptNewPassword();
            if (string.IsNullOrEmpty(NewPassword)) {
                AnsiConsole.Clear();
                return;
            }

            AnsiConsole.MarkupLine("[blue]Confirm your new password.[/]");
            string ConfirmPassword = PromptNewPassword();
            if (string.IsNullOrEmpty(ConfirmPassword)) {
                AnsiConsole.Clear();
                return;
            }

            if (NewPassword == ConfirmPassword) {
                Database.SetUserPassword(Email.ToLower(), NewPassword);
                AnsiConsole.MarkupLine("[green]Password updated successfully.[/]");
                break;
            } else {
                AnsiConsole.MarkupLine("[red]Passwords do not match. Please try again or press enter to cancel.[/]");
            }
        }

        AnsiConsole.MarkupLine("[bold yellow]Press any key to continue...[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    private static string PromptEmail() {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Email:[/]")
            .Validate(e => {
                // Invalid password length, null or only whitespace
                if (!Util.EmailValidOrEmpty(e)) {
                    return ValidationResult.Error("[red]Invalid email[/]");
                }

                // If all checks pass
                return ValidationResult.Success();
            })
                .PromptStyle("yellow")
                .AllowEmpty()
            );
    }

    private static string PromptCode() {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Code[/]:")
        );
    }

    private static string PromptNewPassword() {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Password[/]:")
            .PromptStyle("yellow")
            .Validate(p => {
                // Invalid password length, null or only whitespace
                if (!Util.PasswordValidOrEmpty(p)) {
                    return ValidationResult.Error("[red]Password must be at least 8 characters long[/]");
                }

                // If all checks pass
                return ValidationResult.Success();
            })
            .Secret('*')
            .AllowEmpty()
        );
    }
}