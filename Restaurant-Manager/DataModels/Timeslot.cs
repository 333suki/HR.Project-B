public class Timeslot
{
    public long ID { get; }
    public string Slot { get; }

    public Timeslot(long id, string slot)
    {
        ID = id;
        Slot = slot;
    }

    public override string ToString()
    {
        return $"ID: {ID}\nTimeslot: {Slot}";
    }
}