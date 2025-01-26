using Spectre.Console;

class Authenticator
{
    public const string SecretCode = "3781";

    public static bool Authenticate()
    {
        Console.CursorVisible = false;
        string Buffer = ""; // Define an empty string as buffer
        Panel Panel = new(new Text($"Please enter your authentication code:\n{Buffer}\n").Centered()); // Define the Panel and Text within it
        Panel.Header = new PanelHeader("[blue] Welcome to GertSoft Authenticator [/]").Centered(); // Set the panel header
        Panel.Expand = true; // Panel takes full width
        AnsiConsole.Write(Panel); // Render it

        while (true)
        { // Keep going
            ConsoleKeyInfo Input = Console.ReadKey(); // Read the pressed key
            if (Input.Key == ConsoleKey.Enter)
            { // If key was Enter, break the loop
                break;
            } else if (Input.Key == ConsoleKey.Backspace) {
                if (!string.IsNullOrEmpty(Buffer)) {
                    Buffer = Buffer.Remove(Buffer.Length - 1);
                }
            } else {
                Buffer += Input.KeyChar; // Append the character of pressed key to the buffer
            }
            
            Panel = new(new Text($"Please enter your authentication code:\n{Buffer}\n").Centered()); // Update the panel and the text in it with the updated buffer
            Panel.Header = new PanelHeader("[blue] Welcome to GertSoft Authenticator [/]").Centered(); // Set the header again
            Panel.Expand = true; // Set expand again
            AnsiConsole.Clear(); // Clear the previous print of the panel
            AnsiConsole.Write(Panel); // Re-render the panel with the updated text
        }
        if (Buffer == SecretCode)
        {
            Console.WriteLine("\x1b[32mAccess Granted!\x1b[37m");
            Console.Write("Press enter to continue...");
            Console.ReadKey();
            Console.Clear();
            // Real application will start
            Console.CursorVisible = false;
            return true;
        }
        else
        { Console.Clear();
            Console.CursorVisible = false;
            return false;
        }
    }
}
