using Spectre.Console;

static class MenuManagement
{
    public static void AddDish()
    {
        string DishName = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter dish name, or leave empty to cancel:")
                .PromptStyle("yellow")
                .AllowEmpty()
                .Validate(n => {
                    if (String.IsNullOrEmpty(n)) {
                        return ValidationResult.Success();
                    }
                    // TODO valid dishname checker, check if dish already exists
                    if (Database.DishesTableContainsDish(n)) {
                        return ValidationResult.Error("[red]A dish with this name already exists.[/]");
                    }

                    // If all checks pass
                    return ValidationResult.Success();
                })
            );

        if (String.IsNullOrEmpty(DishName)) {
            return;
        }

        string Price = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter dish price, or leave empty to cancel: ")
                .PromptStyle("yellow")
                .AllowEmpty()
                .Validate(n => {
                    if (String.IsNullOrEmpty(n)) {
                        return ValidationResult.Success();
                    }
                    // TO DO valid price checker
                    if (!double.TryParse(n, out double d) || n.Contains('.') || !n.Contains(',')) {
                        return ValidationResult.Error("[red]That is not a valid price, a valid example: 9,99[/]");
                    }

                    // If all checks pass
                    return ValidationResult.Success();
                })
            );

        if (String.IsNullOrEmpty(Price)) {
            return;
        }

        List<String> Allergens = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title($"Select all that are applicable for {DishName}:")
                .NotRequired()
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a choice, " +
                    "[green]<enter>[/] to accept)[/]")
                .AddChoices("Vegan", "Vegetarian", "Halal", "Gluten Free"));

        bool IsVegan = false;
        bool IsVegetarian = false;
        bool IsHalal = false;
        bool IsGlutenFree = false;
        
        foreach (string Allergen in Allergens) {
            switch (Allergen) {
                case "Vegan":
                    IsVegan = true;
                    break;
                case "Vegetarian":
                    IsVegetarian = true;
                    break;
                case "Halal":
                    IsHalal = true;
                    break;
                case "Gluten Free":
                    IsGlutenFree = true;
                    break;
                default:
                    break;
            }
        }
        
        // Add the dish to database
        try {
            Database.InsertDishesTable(DishName, Price, IsVegan, IsVegetarian, IsHalal, IsGlutenFree);
            AnsiConsole.MarkupLine($"[green]{DishName} was added successfully.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error adding dish: {ex.Message}[/]");
        }
        Console.ReadKey();
    }

    public static void DeleteDish()
    {
        // First get all the dishes from the database
        var dishes = Database.GetAllDishes();
        if (dishes == null || dishes.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No dishes available to edit.[/]");
            Console.ReadKey();
            return;
        }
        
        // Prompt the user to select a dish
        var DishToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a dish to [yellow]delete[/]")
                .PageSize(10)
                .MoreChoicesText("[grey]Move up or down to see more dishes[/]")
                .AddChoices(dishes.Select(d => d.Name).Append("Back"))
        );
        
        if (DishToDelete == "Back") {
            return;
        }
        
        // Get the selected dish's details and prompt for new details after
        var Dish = dishes.First(d => d.Name == DishToDelete);

        try {
            Database.DeleteDishesTable(Dish.Name);
            AnsiConsole.MarkupLine($"[green]{Dish.Name} was deleted successfully.[/]");
        } catch (Exception ex) {
            AnsiConsole.MarkupLine($"[red]Error deleting dish: {ex.Message}[/]");
        }
        Console.ReadKey();
    }

    public static void EditDish()
    {
        // First get all the dishes from the database
        var Dishes = Database.GetAllDishes();
        if (Dishes == null || Dishes.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No Dishes available to edit.[/]");
            Console.ReadKey();
            return;
        }

        // Prompt the user to select a dish
        var DishToEdit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select a dish to [yellow]edit[/]")
            .PageSize(10)
            .MoreChoicesText("[grey]Move up or down to see more dishes[/]")
            .AddChoices(Dishes.Select(d => d.Name).Append("Back"))
            );

        if (DishToEdit == "Back") {
            return;
        }

        // Get the selected dish's details and prompt for new details after
        var Dish = Dishes.First(d => d.Name == DishToEdit);

        string NewDishName = AnsiConsole.Prompt(
            new TextPrompt<string>($"New dish name (leave empty to keep '{Dish.Name}'): ")
            .PromptStyle("yellow")
            .AllowEmpty()
            );

        string NewPrice = AnsiConsole.Prompt(
            new TextPrompt<string>($"New price (leave empty to keep {Dish.Price:C}): ")
            .PromptStyle("yellow")
            .AllowEmpty()
            .Validate(n =>
            {
                if (string.IsNullOrEmpty(n)) {
                    return ValidationResult.Success();
                }
                if (!double.TryParse(n, out double _) || n.Contains('.')) {
                    return ValidationResult.Error("[red]That is not a valid price, a valid example: 9,99[/]");
                }
                return ValidationResult.Success();
            })
        );
        
        MultiSelectionPrompt<String> Prompt = new MultiSelectionPrompt<string>()
            .Title($"Select all that are applicable for {NewDishName}:")
            .NotRequired()
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to toggle a choice, " +
                "[green]<enter>[/] to accept)[/]")
            .AddChoices("Vegan", "Vegetarian", "Halal", "Gluten Free");
        
        // Check all allergens that were already selected before
        if (Dish.IsVegan) {
            Prompt.Select("Vegan");
        }
        if (Dish.IsVegetarian) {
            Prompt.Select("Vegetarian");
        }
        if (Dish.IsHalal) {
            Prompt.Select("Halal");
        }
        if (Dish.IsGlutenFree) {
            Prompt.Select("Gluten Free");
        }
        
        // Prompt new allergens
        List<String> Allergens = AnsiConsole.Prompt(Prompt);

        bool IsVegan = false;
        bool IsVegetarian = false;
        bool IsHalal = false;
        bool IsGlutenFree = false;
        
        foreach (string Allergen in Allergens) {
            switch (Allergen) {
                case "Vegan":
                    IsVegan = true;
                    break;
                case "Vegetarian":
                    IsVegetarian = true;
                    break;
                case "Halal":
                    IsHalal = true;
                    break;
                case "Gluten Free":
                    IsGlutenFree = true;
                    break;
                default:
                    break;
            }
        }

        // If strings left empty, keep the old values
        string FinalDishName = string.IsNullOrEmpty(NewDishName) ? Dish.Name : NewDishName;
        string FinalPrice = string.IsNullOrEmpty(NewPrice) ? Dish.Price : NewPrice;

        // Lastly, update the dish in the database
        try
        {
            Database.UpdateDishesTable(Dish.ID, FinalDishName, FinalPrice, IsVegan, IsVegetarian, IsHalal, IsGlutenFree);
            AnsiConsole.MarkupLine($"[green]{FinalDishName} was updated successfully.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error updating dish: {ex.Message}[/]");
        }
        Console.ReadKey();
    }
}
