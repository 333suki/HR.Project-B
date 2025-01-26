public class Location
{
    public long ID { get; }
    public string City { get; }
    public string Name { get; }
    public string Storyline { get; }
    public string Message { get; }
    public static long NextID { get; set; } = 0;

    public Location(int id, string city, string name, string storyline, string message)
    {
        ID = id;
        City = city;
        Name = name;
        Storyline = storyline;
        Message = message;
    }

    public override string ToString()
    {
        return $"Location {ID}: {City} - {Name}\nInstructions: {Message}";
    }
}