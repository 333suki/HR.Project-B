public class User {
    public long ID { get; }
    public string Email { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string Role { get; } // "ADMIN" or "USER"

    public User(long ID, string Email, string? FirstName, string? LastName, string Role) {
        this.ID = ID;
        this.Email = Email;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Role = Role;
    }

    public User(string Email, string? FirstName, string? LastName, string Role) {
        this.ID = -1;
        this.Email = Email;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Role = Role;
    }

    public string GetFullName() {
        if (FirstName is null && LastName is null) {
            return Util.GetUntil(Email, "@");
        }
        if (FirstName is null) {
            return $"Mr/Ms {LastName}";
        }
        if (LastName is null) {
            return FirstName;
        }
        return $"{FirstName} {LastName}";
    }

    public override string ToString() {
        return $"User [{ID}]: {GetFullName()} - {Role}";
    }
}
