using System;
using Spectre.Console;

public static class ReviewOperations
{
    public static void ReplyToReview(Review review)
    {
        string reply = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your reply:")
                .AllowEmpty());

        try
        {
            Database.UpdateReviewReply(review.ID, reply);
            AnsiConsole.MarkupLine("[green]Reply has been added![/]");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void DeleteReview(Review review)
    {
        try
        {
            Database.DeleteReview(review.ID);
            AnsiConsole.MarkupLine("[green]Review has been deleted![/]");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}