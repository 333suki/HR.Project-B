using Spectre.Console;

static class ViewUsers
{
    public static void ViewAllUsers()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule($" [yellow]View Users[/] "));
        var table = new Table().Centered();
        List<User> users = Database.GetAllUsers();

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
                Update(200, () => table.AddColumn("User ID"));
                Update(200, () => table.AddColumn("Email"));
                Update(200, () => table.AddColumn("First Name"));
                Update(200, () => table.AddColumn("Last Name"));
                Update(200, () => table.AddColumn("Role"));

                // Rows
                foreach (User user in users)
                {
                    Update(100, () => table.AddRow($"{user.ID}", $"{user.Email}", $"{user.FirstName}", $"{user.LastName}", $"{user.Role}"));
                }
            });
        
        Console.CursorVisible = true;
    }
}