using Spectre.Console;

class Program
{
    public static void Main()
    {
        try
        {
            Database.ConnectionString = "database.db";
            Database.CreateUsersTable();
            Database.CreateDishesTable();
            Database.CreateLocationsTable();
            Database.CreateReservationsTable();
            Database.CreateTimeslotsTable();
            Database.CreateReviewsTable();

            Database.InsertLocationsTable("Amsterdam", "The Vanishing Vault", "You’ve been hired to investigate an old bank that mysteriously shut down overnight in 1923. The vault remains sealed, and every previous attempt to open it ended in failure—--or worse, disappearance. Can you uncover the truth before the vault consumes you too?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Amsterdam", "Labyrinth of Shadows", "Hidden beneath the ruins of an ancient cathedral is a labyrinth filled with shifting walls and whispers of a lost relic. Your team must navigate the maze, solve its riddles, and escape before the shadows take form.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Amsterdam", "The Alchemist's Sanctum", "In the heart of a crumbling tower lies the laboratory of an alchemist who sought the secret of eternal life. The room is alive with his unfinished experiments, and it’s up to you to escape before becoming his next subject.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Amsterdam", "Secrets of the Forgotten Asylum", "An abandoned asylum holds the secrets of a doctor who conducted unethical experiments on his patients. You must uncover his dark research to find a way out before the spirits of the past catch up to you.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");

            Database.InsertLocationsTable("Groningen", "Timekeeper’s Paradox", "You’ve stumbled upon the workshop of a mysterious clockmaker who vanished decades ago. The clocks within tick backward, and reality seems to shift with each chime. Can you repair the timeline and escape?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Groningen", "Curse of the Crimson Manor", "The once-grand Crimson Manor is rumored to be cursed, and those who enter are said to vanish forever. As you investigate, the walls seem to close in, and a sinister presence begins to manifest.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Groningen", "The Phantom's Playhouse", "An abandoned theater is haunted by the spirit of a playwright whose final work was never performed. You must bring the script to life to free his soul and escape the stage before the final curtain falls.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Groningen",  "The Oracle’s Chamber", "Buried deep within the mountains is the Oracle’s Chamber, a place said to reveal your future—if you survive its trials. The chamber tests your mind and resolve. Will you uncover the truth or be lost to time?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");

            Database.InsertLocationsTable("Rotterdam", "The Asylum of Whisperers", "The abandoned asylum whispers secrets of its tormented patients. As you explore its shadowy halls, the whispers grow louder, revealing dark truths about the past—and your own connection to them. Can you escape before the whispers consume you?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Rotterdam", "The Silent Hotel", "An eerie hotel sits empty, its halls echoing with the absence of guests who vanished one fateful night. As you investigate, the silence feels oppressive, and each room hides a piece of the mystery. Will you check out in time?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Rotterdam", "Midnight at the Observatory", "At midnight, the abandoned observatory comes alive with celestial maps and strange instruments. The stars shift unnaturally, and you must decode the astronomer’s last discovery before the sky locks you in forever.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Rotterdam", "Depths of the Abyss", "Deep beneath the ocean, an underwater station has gone silent. As you descend, cryptic clues and a growing sense of dread point to something ancient lurking in the depths. Escape before the abyss swallows you whole.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");

            Database.InsertLocationsTable("Utrecht", "Echoes of the Enchanted Library", "Books in this ancient library hold more than words—they hold trapped souls. You must solve the riddles within the enchanted texts to find your way out before you become part of the collection.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Utrecht", "The Blackwell Experiment", "A biotech lab known as Blackwell Industries has gone dark. Inside, you discover a chilling experiment that has gone horribly wrong. The only way out is through a series of locked doors—and the answers lie in the experiments themselves.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Utrecht", "The Silent Carnival", "A carnival suddenly appeared on the outskirts of town, but it’s eerily silent. Each attraction seems designed to test your courage, wit, and sanity. Can you survive the games and escape?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Utrecht", "Portal of the Forgotten Realm", "An abandoned observatory is home to a strange portal humming with energy. When you step inside, you’re transported to a realm of impossible geometry and ancient beings. Solve the puzzles to reopen the portal and return to your world.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");

            Database.InsertLocationsTable("Zwolle", "Whispering Woods Cottage", "You’ve sought refuge in a small cottage in the woods, but as night falls, the house begins to whisper your darkest fears. Solve its mysteries to escape before dawn breaks—or you may never leave.", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Zwolle", "Crypt of the Forgotten Pharaoh", "You’ve been hired to explore an undiscovered Egyptian tomb. But the deeper you go, the more it feels like the tomb is alive. Can you escape before the Forgotten Pharaoh claims you as part of his eternal court?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Zwolle", "The Ship of Lost Souls", "A ghostly ship has reappeared after centuries, shrouded in mist. You and your team board it, but the doors slam shut behind you. Solve its mysteries to escape before the ship disappears again—with you on it.",  "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");
            Database.InsertLocationsTable("Zwolle", "The Midnight Apothecary", "An ancient apothecary’s shop opens only at midnight, selling cures for ailments no modern doctor can treat. But the cure comes with a price—and a clock counting down. Can you escape before it’s too late?", "Enter 'Freddy's Comfort Food, ask for 'Secret Menu #2' and an employee will lead you to your designated starting point.");

            Database.InsertTimeslotsTable("12:00");
            Database.InsertTimeslotsTable("15:00");
            Database.InsertTimeslotsTable("18:00");
            Database.InsertTimeslotsTable("21:00");

            // In future move to different location
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(180);
            
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error initializing the database: {ex.Message}[/]");
            Environment.Exit(1);
        }

        while (!Authenticator.Authenticate()) { }
        
        //Call the SendEmail method
        //EmailService.SendReservationEmail("Joshua van der Jagt", "Rotterdam", "24/10/2024", "21:00", 4, "1092067@hr.nl");

        MainMenuPresentation.ShowMainMenu();
    }
}
