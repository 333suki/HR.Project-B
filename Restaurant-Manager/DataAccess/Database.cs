using System.Data.Entity.Migrations.History;
using System.Data.SQLite;

public static class Database {
    public static string ConnectionString { get; set; } = "database.db";

    public static long GetUsersTableSize() {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "SELECT COUNT(*) FROM Users";
        Object result = cmd.ExecuteScalar();

        return (long)result;
    }

    public static void CreateUsersTable() {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Users(ID INTEGER PRIMARY KEY AUTOINCREMENT, Email TEXT NOT NULL, Password TEXT NOT NULL, FirstName TEXT, LastName TEXT, Role TEXT NOT NULL)";
        cmd.ExecuteNonQuery();
    }

    public static void CreateLocationsTable() {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Locations(ID INTEGER PRIMARY KEY AUTOINCREMENT, City TEXT NOT NULL, Name TEXT NOT NULL, Storyline TEXT NOT NULL, Message TEXT NOT NULL)";
        cmd.ExecuteNonQuery();
    }

    public static void CreateReservationsTable() {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Reservations(ID INTEGER PRIMARY KEY AUTOINCREMENT, User INTEGER, Location INTEGER, Timeslot TEXT NOT NULL, Date DATE NOT NULL, GroupSize INTEGER NOT NULL, GroupTable INTEGER NOT NULL, FOREIGN KEY(User) REFERENCES Users(ID), FOREIGN KEY(Location) REFERENCES Locations(ID))";
        cmd.ExecuteNonQuery();
    }

    public static void CreateDishesTable() {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Dishes(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Price TEXT NOT NULL, IsVegan INTEGER NOT NULL, IsVegetarian INTEGER NOT NULL, IsHalal INTEGER NOT NULL, IsGlutenFree INTEGER NOT NULL)";
        cmd.ExecuteNonQuery();
    }

    public static void CreateTimeslotsTable()
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Timeslots(ID INTEGER PRIMARY KEY AUTOINCREMENT, Timeslot TEXT NOT NULL)";
        cmd.ExecuteNonQuery();
    }

    public static void CreateReviewsTable()
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Reviews(ID INTEGER PRIMARY KEY AUTOINCREMENT, User INTEGER NOT NULL, Rating INTEGER NOT NULL, UserMessage TEXT, Date DATE NOT NULL, Admin INTEGER, AdminMessage TEXT, FOREIGN KEY(User) REFERENCES Users(ID), FOREIGN KEY(Admin) REFERENCES Users(ID))";
        cmd.ExecuteNonQuery();
    }

    public static void SetUserPassword(string Email, string NewPassword) {
        using SQLiteConnection Connection = new SQLiteConnection($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "UPDATE Users SET Password = @NewPassword WHERE Email = @Email";
        cmd.Parameters.AddWithValue("@NewPassword", Encryptor.Encrypt(NewPassword));
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.ExecuteNonQuery();
    }

    public static void InsertDishesTable(string Name, string Price, bool IsVegan, bool IsVegetarian, bool IsHalal, bool IsGlutenFree)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "INSERT INTO Dishes(Name, Price, IsVegan, IsVegetarian, IsHalal, IsGlutenFree) VALUES(@Name, @Price, @IsVegan, @IsVegetarian, @IsHalal, @IsGlutenFree)";
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@Price", Price);
        cmd.Parameters.AddWithValue("@IsVegan", IsVegan ? 1 : 0);
        cmd.Parameters.AddWithValue("@IsVegetarian", IsVegetarian ? 1 : 0);
        cmd.Parameters.AddWithValue("@IsHalal", IsHalal ? 1 : 0);
        cmd.Parameters.AddWithValue("@IsGlutenFree", IsGlutenFree ? 1 : 0);
        cmd.ExecuteNonQuery();
    }

    public static void InsertTimeslotsTable(string timeslot)
    {
        using SQLiteConnection Connection = new SQLiteConnection($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = @"INSERT INTO Timeslots(Timeslot)
                            SELECT @Timeslot
                            WHERE NOT EXISTS (SELECT 1 FROM Timeslots WHERE Timeslot = @Timeslot)";
        cmd.Parameters.AddWithValue("@Timeslot", timeslot);
        cmd.ExecuteNonQuery();
    }

    public static void InsertReviewsTable(long user, int rating, string userMessage, DateOnly date)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        cmd.CommandText = "INSERT INTO Reviews(User, Rating, UserMessage, Date) VALUES (@User, @Rating, @UserMessage, @Date)";

        cmd.Parameters.AddWithValue("@User", user);
        cmd.Parameters.AddWithValue("@Rating", rating);
        cmd.Parameters.AddWithValue("@UserMessage", userMessage);
        cmd.Parameters.AddWithValue("@Date", $"{date.Day}-{date.Month}-{date.Year}");
        cmd.ExecuteNonQuery();
    }

    public static bool DishesTableContainsDish(string Name)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = $"SELECT COUNT(*) FROM Dishes WHERE Name = @Name";
        cmd.Parameters.AddWithValue("@Name", Name);
        Object result = cmd.ExecuteScalar();

        return (long)result > 0;
    }

    public static void UpdateDishesTable(long ID, string Name, string Price, bool IsVegan, bool IsVegetarian, bool IsHalal, bool IsGlutenFree)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "UPDATE Dishes SET Name = @Name, Price = @Price, IsVegan = @IsVegan, IsVegetarian = @IsVegetarian, IsHalal = @IsHalal, IsGlutenFree = @IsGlutenFree WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@Price", Price);
        cmd.Parameters.AddWithValue("@IsVegan", IsVegan ? 1 : 0);
        cmd.Parameters.AddWithValue("@IsVegetarian", IsVegetarian ? 1 : 0);
        cmd.Parameters.AddWithValue("@IsHalal", IsHalal ? 1 : 0);
        cmd.Parameters.AddWithValue("@IsGlutenFree", IsGlutenFree ? 1 : 0);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteDishesTable(string Name) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Dishes WHERE Name = @Name", Connection);
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.ExecuteNonQuery();
    }

    public static void InsertLocationsTable(string city, string name, string storyline, string message)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = @"INSERT INTO Locations(City, Name, Storyline, Message)
                            SELECT @City, @Name, @Storyline, @Message
                            WHERE NOT EXISTS (SELECT 1 FROM Locations WHERE Name = @Name)";

        cmd.Parameters.AddWithValue("@City", city);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Storyline", storyline);
        cmd.Parameters.AddWithValue("@Message", message);
        cmd.ExecuteNonQuery();
    }

    public static void InsertReservationsTable(long? id, long user_id, long loc_id, string timeslot, DateOnly date, int groupsize, int table)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        if (id.HasValue)
        {
            cmd.CommandText = "INSERT INTO Reservations(ID, User, Location, Timeslot, Date, GroupSize, GroupTable) VALUES (@ID, @User, @Location, @Timeslot, @Date, @GroupSize, @GroupTable)";
            cmd.Parameters.AddWithValue("@ID", id);
        }
        else
        {
            cmd.CommandText = "INSERT INTO Reservations(User, Location, Timeslot, Date, GroupSize, GroupTable) VALUES (@User, @Location, @Timeslot, @Date, @GroupSize, @GroupTable)";
        }

        cmd.Parameters.AddWithValue("@User", user_id);
        cmd.Parameters.AddWithValue("@Location", loc_id);
        cmd.Parameters.AddWithValue("@Timeslot", timeslot);
        cmd.Parameters.AddWithValue("@Date", $"{date.Day}-{date.Month}-{date.Year}");
        cmd.Parameters.AddWithValue("@GroupSize", groupsize);
        cmd.Parameters.AddWithValue("@GroupTable", table);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteReservationsTable(long ID) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "DELETE FROM Reservations WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.ExecuteNonQuery();
    }
    
    

    public static List<Location> GetAllLocations()
    {
        List<Location> locations = new();

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        cmd.CommandText = "SELECT * FROM Locations";
        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string city = reader.GetString(1);
            string name = reader.GetString(2);
            string storyline = reader.GetString(3);
            string message = reader.GetString(4);
            locations.Add(new Location(id, city, name, storyline, message));
        }

        return locations;
    }

    public static Location? GetLocationByID(long ID) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "SELECT * FROM Locations WHERE ID = @ID LIMIT 1";
        cmd.Parameters.AddWithValue("@ID", ID);
        using SQLiteDataReader reader = cmd.ExecuteReader();
        if (reader.Read()) {
            int id = reader.GetInt32(0);
            string city = reader.GetString(1);
            string name = reader.GetString(2);
            string storyline = reader.GetString(3);
            string message = reader.GetString(4);
            return new Location(id, city, name, storyline, message);
        }
        return null;
    }

    public static Location? GetLocationByCityAndName(string City, string Name) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "SELECT * FROM Locations WHERE (City = @City AND Name = @Name) LIMIT 1";
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@City", City);
        using SQLiteDataReader reader = cmd.ExecuteReader();
        if (reader.Read()) {
            int id = reader.GetInt32(0);
            string city = reader.GetString(1);
            string name = reader.GetString(2);
            string storyline = reader.GetString(3);
            string message = reader.GetString(4);
            return new Location(id, city, name, storyline, message);
        }
        return null;
    }

    public static List<Reservation> GetAllReservations()
    {
        List<Reservation> reservations = new(){};

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        cmd.CommandText = "SELECT * FROM Reservations";
        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            long ID = reader.GetInt32(0);
            long userID = reader.GetInt32(1);
            long locId = reader.GetInt32(2);
            string timeslot = reader.GetString(3);
            string date = reader.GetString(4);
            int groupsize = reader.GetInt32(5);
            int table = reader.GetInt32(6);

            reservations.Add(new Reservation(ID, userID, locId, timeslot, DateOnly.ParseExact(date, "d-M-yyyy"), groupsize, table));
        }

        return reservations;
    }

    public static List<Reservation> GetReservationsByUserID(long userID)
    {
        List<Reservation> reservations = new();
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        cmd.CommandText = "SELECT * FROM Reservations WHERE User = @ID";
        cmd.Parameters.AddWithValue("@ID", userID);
        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            long ID = reader.GetInt32(0);
            long locId = reader.GetInt32(2);
            string timeslot = reader.GetString(3);
            string date = reader.GetString(4);
            int groupsize = reader.GetInt32(5);
            int table = reader.GetInt32(6);

            reservations.Add(new Reservation(ID, userID, locId, timeslot, DateOnly.ParseExact(date, "d-M-yyyy"), groupsize, table));
        }
        return reservations;
    }

    public static List<Reservation> GetReservationsByEmail(string Email) {
        List<Reservation> reservations = new();

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        User? User = GetUserByEmail(Email);

        cmd.CommandText = "SELECT * FROM Reservations WHERE User = @ID";
        cmd.Parameters.AddWithValue("@ID", User?.ID);
        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
            long ID = reader.GetInt32(0);
            long userID = reader.GetInt32(1);
            long locId = reader.GetInt32(2);
            string timeslot = reader.GetString(3);
            string date = reader.GetString(4);
            int groupsize = reader.GetInt32(5);
            int table = reader.GetInt32(6);

            reservations.Add(new Reservation(ID, userID, locId, timeslot, DateOnly.ParseExact(date, "d-M-yyyy"), groupsize, table));
        }

        return reservations;
    }

    public static List<Dish> GetAllDishes() {
        List<Dish> dishes = new List<Dish>();
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Dishes", Connection);
        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
            dishes.Add(new Dish(
                (long)reader["ID"],
                (string)reader["Name"],
                (string)(reader["Price"]),
                Convert.ToBoolean(reader["IsVegan"]),
                Convert.ToBoolean(reader["IsVegetarian"]),
                Convert.ToBoolean(reader["IsHalal"]),
                Convert.ToBoolean(reader["IsGlutenFree"])
                ));
        }
        return dishes;
    }

    public static List<Timeslot> GetAllTimeslots()
    {
        List<Timeslot> timeslots = new(){};

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        cmd.CommandText = "SELECT * FROM Timeslots";
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            long id = reader.GetInt64(0);
            string timeslot = reader.GetString(1);

            timeslots.Add(new Timeslot(id, timeslot));
        }

        return timeslots;
    }

    public static List<Review> GetAllReviews()
    {
        List<Review> reviews = new();

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);

        cmd.CommandText = "SELECT * FROM Reviews";
        SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            long id = reader.GetInt64(0);
            long user = reader.GetInt64(1);
            int rating = reader.GetInt32(2);
            string userMessage = reader.GetString(3);
            string date = reader.GetString(4);
            long? admin = reader.IsDBNull(5) ? null : reader.GetInt64(5);
            string? adminMessage = reader.IsDBNull(6) ? null : reader.GetString(6);

            reviews.Add(new Review(id, user, rating, userMessage, DateOnly.ParseExact(date, "d-M-yyyy"), admin, adminMessage));
        }

        return reviews;
    }

    public static void InsertUsersTable(string Email, string Password, string? FirstName, string? LastName, string Role)
    {
        if (Role != "USER" && Role != "ADMIN")
        {
            throw new InvalidDataException($"Role has to be \"USER\" or \"ADMIN\". Found \"{Role}\"");
        }

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "INSERT INTO Users(Email, Password, FirstName, LastName, Role) VALUES(@Email, @Password, @FirstName, @LastName, @Role)";
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.Parameters.AddWithValue("@Password", Encryptor.Encrypt(Password));
        cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrWhiteSpace(FirstName) ? null : FirstName);
        cmd.Parameters.AddWithValue("@LastName", string.IsNullOrWhiteSpace(LastName) ? null : LastName);
        cmd.Parameters.AddWithValue("@Role", Role);
        cmd.ExecuteNonQuery();
    }

    public static void InsertUsersTable(User User, string Password)
    {
        if (User.Role != "USER" && User.Role != "ADMIN")
        {
            throw new InvalidDataException($"Role has to be \"USER\" or \"ADMIN\". Found \"{User.Role}\"");
        }
        if (User.ID <= 0)
        {
            throw new InvalidDataException($"ID has to be a positive non-zero long. Found \"{User.ID}\"");
        }

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "INSERT INTO Users(Email, Password, FirstName, LastName, Role) VALUES(@Email, @Password, @FirstName, @LastName, @Role)";
        cmd.Parameters.AddWithValue("@Email", User.Email);
        cmd.Parameters.AddWithValue("@Password", Encryptor.Encrypt(Password));
        cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrWhiteSpace(User.FirstName) ? null : User.FirstName);
        cmd.Parameters.AddWithValue("@LastName", string.IsNullOrWhiteSpace(User.LastName) ? null : User.LastName);
        cmd.Parameters.AddWithValue("@Role", User.Role);
        cmd.ExecuteNonQuery();
    }

    // This forces an ID for the user, use only for debugging
    public static void InsertUsersTable(long ID, string Email, string Password, string? FirstName, string? LastName, string Role)
    {
        if (Role != "USER" && Role != "ADMIN")
        {
            throw new InvalidDataException($"Role has to be \"USER\" or \"ADMIN\". Found \"{Role}\"");
        }
        if (ID <= 0)
        {
            throw new InvalidDataException($"ID has to be a positive non-zero long. Found \"{ID}\"");
        }

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = $"INSERT INTO Users(ID, Email, Password, FirstName, LastName, Role) VALUES(@ID, @Email, @Password, @FirstName, @LastName, @Role)";
        cmd.Parameters.AddWithValue("@ID", ID);
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.Parameters.AddWithValue("@Password", Encryptor.Encrypt(Password));
        cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrWhiteSpace(FirstName) ? null : FirstName);
        cmd.Parameters.AddWithValue("@LastName", string.IsNullOrWhiteSpace(LastName) ? null : LastName);
        cmd.Parameters.AddWithValue("@Role", Role);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateReservation(Reservation reservationToEdit)
    {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "UPDATE reservations SET Timeslot = @Timeslot, Date = @Date, Groupsize = @Groupsize WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@Timeslot", reservationToEdit.Timeslot);
        cmd.Parameters.AddWithValue("@Date", reservationToEdit.Date?.ToString("dd-MM-yyyy"));
        cmd.Parameters.AddWithValue("@Groupsize", reservationToEdit.GroupSize);
        cmd.Parameters.AddWithValue("@ID", reservationToEdit.ID);
        cmd.ExecuteNonQuery();
    }

    public static bool UsersTableContainsUser(string Email) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = $"SELECT COUNT(*) FROM Users WHERE Email = @Email";
        cmd.Parameters.AddWithValue("@Email", Email);
        Object result = cmd.ExecuteScalar();

        return (long)result > 0;
    }

    public static User? GetUserByEmail(string Email) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = $"SELECT * FROM Users WHERE Email = @Email LIMIT 1";
        cmd.Parameters.AddWithValue("@Email", Email);
        using SQLiteDataReader result = cmd.ExecuteReader();
        if (!result.HasRows)
        {
            return null;
        }
        result.Read();
        return new User((long)result["ID"], (string)result["Email"], result["FirstName"] == DBNull.Value ? null : (string)result["FirstName"], result["LastName"] == DBNull.Value ? null : (string)result["LastName"], (string)result["Role"]);
    }

    public static User? GetUserByID(long ID) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = $"SELECT * FROM Users WHERE ID = @ID LIMIT 1";
        cmd.Parameters.AddWithValue("@ID", ID);
        using SQLiteDataReader result = cmd.ExecuteReader();
        if (!result.HasRows) {
            return null;
        }
        result.Read();
        return new User((long)result["ID"], (string)result["Email"], result["FirstName"] == DBNull.Value ? null : (string)result["FirstName"], result["LastName"] == DBNull.Value ? null : (string)result["LastName"], (string)result["Role"]);
    }

    public static string? GetEncryptedPassword(string Email) {
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = $"SELECT Password FROM Users WHERE Email = @Email LIMIT 1";
        cmd.Parameters.AddWithValue("@Email", Email);
        using SQLiteDataReader result = cmd.ExecuteReader();
        if (!result.HasRows)
        {
            return null;
        }
        result.Read();
        return (string)result["Password"];
    }

    public static List<User> GetAllUsers() {
        List<User> users = new List<User>();

        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "SELECT ID, Email, FirstName, LastName, Role FROM Users";

        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
            long id = reader.GetInt64(0);
            string email = reader.GetString(1);
            string? firstName = reader.IsDBNull(2) ? null : reader.GetString(2);
            string? lastName = reader.IsDBNull(3) ? null : reader.GetString(3);
            string role = reader.GetString(4);

            User user = new User(id, email, firstName, lastName, role);
            users.Add(user);
        }
        return users;
    }

    public static List<int> GetAvailableTables(string Date, string Timeslot) {
        List<int> AvailableTables = [1, 2, 3, 4, 5, 6];
        using SQLiteConnection Connection = new($"Data Source={ConnectionString}");
        Connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(Connection);
        cmd.CommandText = "SELECT GroupTable FROM Reservations WHERE (Timeslot = @Timeslot AND Date = @Date)";
        cmd.Parameters.AddWithValue("@Timeslot", Timeslot);
        cmd.Parameters.AddWithValue("@Date", Date);
        
        using SQLiteDataReader reader = cmd.ExecuteReader();
        while (reader.Read()) {
            AvailableTables.Remove(reader.GetInt32(0));
        }
        return AvailableTables;
    }

    public static void UpdateReviewReply(long reviewId, string reply)
    {
        using SQLiteConnection connection = new($"Data Source={ConnectionString}");
        connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(connection);
        cmd.CommandText = "UPDATE Reviews SET AdminMessage = @AdminMessage WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@AdminMessage", reply);
        cmd.Parameters.AddWithValue("@ID", reviewId);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteReview(long reviewId)
    {
        using SQLiteConnection connection = new($"Data Source={ConnectionString}");
        connection.Open();
        using SQLiteCommand cmd = new SQLiteCommand(connection);
        cmd.CommandText = "DELETE FROM Reviews WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@ID", reviewId);
        cmd.ExecuteNonQuery();
    }

}
