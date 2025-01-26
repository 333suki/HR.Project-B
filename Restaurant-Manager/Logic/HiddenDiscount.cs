public static class HiddenDiscount
{
    public static List<string> HiddenCodes = new()
        { "A9fL2", "Xb3Kd", "J7qYp", "L5rCt", "Z1NmW"};

    public static string selectedDiscountCode = null;
    public const int HiddenCodeID = 0;
    public static bool ChangeMenuHead { get; set; }

    public static string RandomCodePicker()
    {
        int amountOfCodes = HiddenCodes.Count;
        Random rand = new();
        int randomCodeIndex = rand.Next(0, amountOfCodes);

        return HiddenCodes[randomCodeIndex];
    }

    public static void SetDiscountCode(string code)
    {
        selectedDiscountCode = code;
    }

    private static void AddCodeToMenu()
    {
        Database.InsertDishesTable(RandomCodePicker(), "0", true, true, true, true);
    }

    public static void RemoveCodeFromMenu()
    {
        foreach (string HiddenCode in HiddenCodes)
        {
            try
            {
                Database.DeleteDishesTable(HiddenCode);
            }
            catch (Exception ex){}
        }
    }

    public static void AddCodeToReservations()
    {
        DateOnly date = ReservationLogic.ParseDate("9-9-9999");
        Database.InsertReservationsTable(HiddenCodeID, State.LoggedInUser.ID, 0, RandomCodePicker(), date, 0, 0);
    }

    public static void RemoveCodeFromReservations()
    {
        try
        {
            Database.DeleteReservationsTable(HiddenCodeID);
        }
        catch (Exception ex){}
    }

    private static void ChangeMenuHeadToTrue()
    {
        ChangeMenuHead = true;
    }

    public static void ChangeMenuHeadToFalse()
    {
        ChangeMenuHead = false;
    }

    public static void InsertCodeIntoUI()
    {
        List<Action> HiddenCodeFunctions = new()
            { AddCodeToMenu, AddCodeToReservations, ChangeMenuHeadToTrue };

        int amountOfFunctions = HiddenCodeFunctions.Count;
        Random rand = new();
        int randomCodeIndex = rand.Next(0, amountOfFunctions);

        HiddenCodeFunctions[randomCodeIndex]();
    }
}