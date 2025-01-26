using System.Globalization;
using Spectre.Console;

public static class ReservationPresentation
{
    public static void Present()
    {
        Console.Clear();
        var userSelectionPrompt = new SelectionPrompt<string>()
            .Title("[cyan]Please select an option:[/]")
            .AddChoices(new[]
            {
                    "Make a Reservation", "View Reservations", "Edit Reservation", "Remove Reservation", "Back"
            });

        string userSelection = AnsiConsole.Prompt(userSelectionPrompt);

        switch (userSelection)
        {
            case "Back":
                Console.Clear();
                return;
            case "Make a Reservation":
                Console.Clear();
                ReservationPresentation.CreateReservationPresentation();
                break;
            case "View Reservations":
                Console.Clear();
                ViewReservations.PrintUserReservations();
                break;
            case "Edit Reservation":
                Console.Clear();
                EditReservationPresentation.Present();
                break;
            case "Remove Reservation":
                //Console.WriteLine("TBA...");
                //Thread.Sleep(1000);
                Console.Clear();
                RemoveReservationPresentation.Present();
                break;
        }
    }

    public static void CreateReservationPresentation()
    {
        Console.Clear();
        AnsiConsole.Write(new Rule($" [maroon]Make Reservation ({State.LoggedInUser.GetFullName()})[/] "));
        long userID = State.LoggedInUser.ID;

        long locID = SelectLocation();
        if (locID == -1) return;
        string locMessage = ReservationLogic.GetLocationDescription(locID);
        string locName = ReservationLogic.GetLocationName(locID);
        
        (int Day, int Month, int Year) Date = SelectDate();
        if (Date is (0, 0, 0)) return;
        string dateString = $"{Date.Day}-{Date.Month}-{Date.Year}";
        DateOnly date = ReservationLogic.ParseDate(dateString);

        string timeslot = SelectTimeslot();
        if (timeslot == "NULL") return;

        int groupsize = SelectGroupSize();
        if (groupsize == -1) return;

        int table = ReservationLogic.GetTableCount(locID, timeslot, date);

        EnterDiscountCode();

        (bool success, string message) = ReservationLogic.CreateReservation(userID, locID, timeslot, date, groupsize, table);
        if (success)
        {
            if (!EmailService.SendReservationEmail(State.LoggedInUser.GetFullName(), Database.GetLocationByID(locID)?.City, ReservationLogic.GetLocationName(locID), dateString, timeslot, groupsize, State.LoggedInUser.Email)) {
                AnsiConsole.MarkupLine("[gray]Press any key to continue...[/]");
                Console.ReadKey();
                return;
            }
            
            string text = "";
            if (HiddenDiscount.selectedDiscountCode != null)
            {
                text = $"[green]Your reservation has been made.[/]\nYour Table Number: {table}\n\n{locMessage}\n\nA confirmation email has been sent to {State.LoggedInUser.Email} which also contains the discount code you can show to staff.\n\nPress any key to continue.";
            }
            else
            {
                text = $"[green]Your reservation has been made.[/]\nYour Table Number: {table}\n\n{locMessage}\n\nA confirmation email has been sent to {State.LoggedInUser.Email}.\n\nPress any key to continue.";
            }
            
            Panel panel = new(new Markup(text).Centered()); // Update the panel and the text in it with the updated buffer
            panel.Expand = true; // Set expand again
            Console.Clear();
            AnsiConsole.Write(panel);
            Console.ReadKey();
        }
        else
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }

    private static long SelectLocation()
    {
        Console.CursorVisible = false;

        // User selects a Location, of which the ID gets stored
        var locationChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[cyan]Select a location:[/]")
            .AddChoices(ReservationLogic.LocationNamesToList()));

        if (locationChoice == "Back")
        {
            return -1;
        }
        
        return Database.GetLocationByCityAndName(locationChoice.Split("    \t - ")[0], locationChoice.Split("    \t - ")[1]).ID;
    }

    public static string SelectTimeslot()
    {
        Console.Clear();
        List<string> Options = ReservationLogic.TimeslotsToList();
        Options.Add("Exit Reservation");

        // Timeslot object gets converted into a list of times in string format, user can pick one
        var timeslotChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[cyan]Select a timeslot:[/]")
            .AddChoices(Options));
        
        if (timeslotChoice == "Exit Reservation")
        {
            return "NULL";
        }

        return timeslotChoice;
    }

    public static (int Day, int Month, int Year) SelectDate() {
        DateTime date = DateTime.MinValue;
        int SelectedDay = DateTime.Now.Day;
        int SelectedMonth = DateTime.Now.Month;
        int SelectedYear = DateTime.Now.Year;
        ReservationLogic.IncreaseDateByDay(ref SelectedDay, ref SelectedMonth, ref SelectedYear);
        ReservationLogic.IncreaseDateByDay(ref SelectedDay, ref SelectedMonth, ref SelectedYear);
        var Calendar = new Spectre.Console.Calendar(SelectedYear, SelectedMonth);

        while (true) {
            AnsiConsole.Clear();
            Calendar.CalendarEvents.Clear();
            Calendar.Year = SelectedYear;
            Calendar.Month = SelectedMonth;
            Calendar.AddCalendarEvent(SelectedYear, SelectedMonth, SelectedDay);
            Calendar.HighlightStyle(Style.Parse("yellow bold"));
            Console.CursorVisible = false;
            AnsiConsole.Write(Calendar);
            AnsiConsole.Markup("[gray]Left/Right arrow to change day. Up/Down arrow to change month. Enter to confirm. Escape to cancel.[/]");

            ConsoleKeyInfo KeyInfo = Console.ReadKey(intercept: true);

            if (KeyInfo.Key == ConsoleKey.RightArrow) {
                ReservationLogic.IncreaseDateByDay(ref SelectedDay, ref SelectedMonth, ref SelectedYear);
            } else if (KeyInfo.Key == ConsoleKey.LeftArrow) {
                ReservationLogic.DecreaseDateByDay(ref SelectedDay, ref SelectedMonth, ref SelectedYear);
            } else if (KeyInfo.Key == ConsoleKey.UpArrow) {
                ReservationLogic.IncreaseDateByMonth(ref SelectedDay, ref SelectedMonth, ref SelectedYear);
            } else if (KeyInfo.Key == ConsoleKey.DownArrow) {
                ReservationLogic.DecreaseDateByMonth(ref SelectedDay, ref SelectedMonth, ref SelectedYear);
            } else if (KeyInfo.Key == ConsoleKey.Enter) {
                Console.CursorVisible = true;
                return (SelectedDay, SelectedMonth, SelectedYear);
            } else if (KeyInfo.Key == ConsoleKey.Escape) {
                Console.CursorVisible = true;
                return (0, 0, 0);
            }
        }
    }

    public static int SelectGroupSize()
    {
        Console.CursorVisible = false;
        bool isSelected = false;
        int currentOption = 1;
        const int minGroupSize = 1;
        const int maxGroupSize = 6;

        DisplayGroupSizeSelectionPanel(currentOption, minGroupSize, maxGroupSize);

        while (!isSelected)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (currentOption != maxGroupSize && keyInfo.Key == ConsoleKey.UpArrow) currentOption += 1;

            if (currentOption != minGroupSize && keyInfo.Key == ConsoleKey.DownArrow) currentOption -= 1;

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                isSelected = true;
            }

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return -1;
            }

            // Replaces the first panel with one where the changes have been made
            DisplayGroupSizeSelectionPanel(currentOption, minGroupSize, maxGroupSize);
        }

        return currentOption;
    }

    private static void DisplayGroupSizeSelectionPanel(int currentOption, int minGroupSize, int maxGroupSize)
    {
        Console.Clear();

        string arrowUp = (currentOption < maxGroupSize) ?  "         ^" : "";
        string arrowDown = (currentOption > minGroupSize) ? "          v " : "";
        string personOrPeople = (currentOption == minGroupSize) ? "person" : "people";

        string displayText = $"\n{arrowUp}\nReservation for {currentOption} {personOrPeople}\n{arrowDown}";
        Panel panel = new(new Markup($"Enter the amount of people for the reservation using the arrow keys [gray](max. 6 people)[/]:\n{displayText}\n\n\nPress ESC to exit.").Centered());
        panel.Expand = true;

        AnsiConsole.Clear();
        AnsiConsole.Write(panel);
    }

    private static void EnterDiscountCode()
    {
        Console.Clear();
        Console.CursorVisible = false;
        
        string Buffer = "";
        Panel Panel = new(new Text($"Please enter a discount code or leave empty to continue:\n{Buffer}\n").Centered());
        Panel.Header = new PanelHeader("[blue] Discount Voucher [/]").Centered();
        Panel.Expand = true;
        AnsiConsole.Write(Panel);

        while (true)
        {
            ConsoleKeyInfo Input = Console.ReadKey(); // Read the pressed key
            if (Input.Key == ConsoleKey.Enter) { // If key was Enter, break the loop
                break;
            }
            else if (Input.Key == ConsoleKey.Backspace) {
                if (!string.IsNullOrEmpty(Buffer)) {
                    Buffer = Buffer.Remove(Buffer.Length - 1);
                }
            }
            else {
                Buffer += Input.KeyChar; // Append the character of pressed key to the buffer
            }

            Panel = new(new Text($"Please enter a discount code or leave empty to continue:\n{Buffer}\n").Centered());
            Panel.Header = new PanelHeader("[blue] Discount Voucher [/]").Centered();
            Panel.Expand = true;
            AnsiConsole.Clear();
            AnsiConsole.Write(Panel);
            Buffer = Buffer.Trim();

            if (Input.Key == ConsoleKey.Enter && HiddenDiscount.HiddenCodes.Contains(Buffer))
            {
                HiddenDiscount.SetDiscountCode(Buffer);
                Panel = new(new Markup($"[green]Discount code has been added. [/]").Centered());
                Panel.Header = new PanelHeader("[blue] Discount Voucher [/]").Centered();
                Panel.Expand = true;
                AnsiConsole.Clear();
                AnsiConsole.Write(Panel);
                Thread.Sleep(1500);
                break;
            }
            
            if (Input.Key == ConsoleKey.Enter && !(HiddenDiscount.HiddenCodes.Contains(Buffer)))
            {
                Panel = new(new Markup($"[red]Discount code does not exist. [/]").Centered());
                Panel.Header = new PanelHeader("[blue] Discount Voucher [/]").Centered();
                Panel.Expand = true;
                AnsiConsole.Clear();
                AnsiConsole.Write(Panel);
                Thread.Sleep(1300);
                Console.Clear();
                
                Buffer = "";
                Panel = new(new Text($"Please enter a discount code or leave empty to continue:\n{Buffer}\n").Centered());
                Panel.Header = new PanelHeader("[blue] Discount Voucher [/]").Centered();
                Panel.Expand = true;
                AnsiConsole.Write(Panel);
            }
        }
    }
}