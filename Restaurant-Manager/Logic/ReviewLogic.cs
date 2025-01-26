static class ReviewLogic
{
    public static bool ReservationCheck(long userID)
    {
        List<Reservation> reservations = Database.GetReservationsByUserID(userID);
        foreach (Reservation reservation in reservations)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeSpan difference = today.ToDateTime(new TimeOnly(0, 0)) - reservation.Date.Value.ToDateTime(new TimeOnly(0, 0));
            if (difference.Days < 91)
            {
                return true;
            }
        }
        return false;
    }
}

