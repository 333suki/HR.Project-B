using Spectre.Console;

static class MenuCard
{
    public static void DisplayMenuCard()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule($"[maroon]Menu Card[/]"));
        var table = new Table().Centered();
        List<Dish> dishes = Database.GetAllDishes();

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
                Update(200, () => table.AddColumn("Name"));
                Update(200, () => table.AddColumn("Price"));
                Update(200, () => table.AddColumn("Vegan"));
                Update(200, () => table.AddColumn("Vegetarian"));
                Update(200, () => table.AddColumn("Halal"));
                Update(200, () => table.AddColumn("Gluten Free"));

                // Rows
                foreach (Dish dish in dishes)
                {
                    Update(100, () => table.AddRow(dish.Name, $"${dish.Price:F2}", dish.IsVegan ? "[green]Yes[/]" : "[red]No[/]", dish.IsVegetarian ? "[green]Yes[/]" : "[red]No[/]", dish.IsHalal ? "[green]Yes[/]" : "[red]No[/]", dish.IsGlutenFree ? "[green]Yes[/]" : "[red]No[/]"));
                }

                // Column footer
                //Update(230, () => table.Columns[2].Footer("$1,633,000,000"));
                //Update(230, () => table.Columns[3].Footer("$928,119,224"));
                //Update(400, () => table.Columns[4].Footer("$10,318,030,576"));

                //// Column alignment
                //Update(230, () => table.Columns[2].RightAligned());
                //Update(230, () => table.Columns[3].RightAligned());
                //Update(400, () => table.Columns[4].RightAligned());

                //// Column titles
                //Update(70, () => table.Columns[0].Header("[bold]Release date[/]"));
                //Update(70, () => table.Columns[1].Header("[bold]Title[/]"));
                //Update(70, () => table.Columns[2].Header("[red bold]Budget[/]"));
                //Update(70, () => table.Columns[3].Header("[green bold]Opening Weekend[/]"));
                //Update(400, () => table.Columns[4].Header("[blue bold]Box office[/]"));

                //// Footers
                //Update(70, () => table.Columns[2].Footer("[red bold]$1,633,000,000[/]"));
                //Update(70, () => table.Columns[3].Footer("[green bold]$928,119,224[/]"));
                //Update(400, () => table.Columns[4].Footer("[blue bold]$10,318,030,576[/]"));

                //// Title
                //Update(500, () => table.Title("Star Wars Movies"));
                //Update(400, () => table.Title("[[ [yellow]Star Wars Movies[/] ]]"));

                //// Borders
                //Update(230, () => table.BorderColor(Color.Yellow));
                //Update(230, () => table.MinimalBorder());
                //Update(230, () => table.SimpleBorder());
                //Update(230, () => table.SimpleHeavyBorder());

                //// Caption
                //Update(400, () => table.Caption("[[ [blue]THE END[/] ]]"));
            });
    }
}
