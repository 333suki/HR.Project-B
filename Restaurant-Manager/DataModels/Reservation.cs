public class Reservation
{
    public long ID { get; }
    public long UserID { get; set; }
    public long LocationID { get; set; }
    public string Timeslot { get; private set;}
    public DateOnly? Date { get; set;}
    public int GroupSize { get; set;}
    public int Table { get; private set;}

    public Reservation(long id, long user, long location, string timeslot, DateOnly date, int groupsize, int table)
    {
        ID = id;
        UserID= user;
        LocationID = location;
        Timeslot = timeslot;
        Date = date;
        GroupSize = groupsize;
        Table = table;
    }

    public override string ToString()
    {
        return $"Reservation {ID} by User {UserID}\nLocation ID: {LocationID}\nTimeslot of reservation: {Date}\nAmount of people: {GroupSize}\nTable: {Table}";
    }

    public void UpdateTimeslot(string timeslot)
    {
        Timeslot = timeslot;
    }

    public void UpdateDate(DateOnly? date)
    {
        Date = date;
    }

    public void UpdateGroupSize(int groupSize)
    {
        GroupSize = groupSize;
    }

    public void UpdateTable(int table)
    {
        Table = table;
    }
}