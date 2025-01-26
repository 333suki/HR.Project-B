using Spectre.Console;

public static class ViewReservations
{
    public static void PrintUserReservations()
    {
        AnsiConsole.Write(new Rule($"[yellow]View Reservations ({State.LoggedInUser.GetFullName()})[/]"));
        var reservations = Database.GetAllReservations();
        var userId = State.LoggedInUser.ID;

        var table = new Table().Centered();

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
                Update(200, () => table.AddColumn("Table"));
                Update(200, () => table.AddColumn("Date"));
                Update(200, () => table.AddColumn("Time"));
                Update(200, () => table.AddColumn("Guests"));

                // Rows
                foreach (var reservation in reservations)
                {
                    if (reservation.UserID == userId)
                    {
                        Update(100, () => table.AddRow(
                            reservation.Table.ToString(),
                            reservation.Date?.ToString("dd-MM-yyyy"),
                            reservation.Timeslot,
                            reservation.GroupSize.ToString()
                        ));
                    }
                }
            });

        // Display the prompt after the table animation completes
        AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
}
