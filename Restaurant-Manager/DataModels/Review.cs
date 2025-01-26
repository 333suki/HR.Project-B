public class Review
{
    public long ID { get; }
    public long User { get; }
    public int Rating { get; }
    public string UserMessage { get; }
    public DateOnly Date { get; }
    public long? Admin { get; }
    public string? AdminMessage { get; }

    public Review(long id, long user, int rating, string userMessage, DateOnly date, long? admin, string? adminMessage)
    {
        this.ID = id;
        this.User = user;
        this.Rating = rating;
        this.UserMessage = userMessage;
        this.Date = date;
        this.Admin = admin;
        this.AdminMessage = adminMessage;
    }
}