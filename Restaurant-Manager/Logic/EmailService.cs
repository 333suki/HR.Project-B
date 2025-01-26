using System.Net;
using System.Net.Mail;
using Spectre.Console;

public static class EmailService
{
    public static bool SendReservationEmail(string RecipientName, string LocationCity, string LocationName, string Date, string Time, int PlayerAmount, string RecipientEmail)
    {
        // Define the email message
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("mail.escapedine@gmail.com");
        mail.To.Add(RecipientEmail);
        mail.Subject = "Your Escape and Dine reservation";
        mail.Body = $"<p><strong>Dear {RecipientName},</strong></p><p>Thank you for booking your adventure at Escape &amp; Dine! We&rsquo;re excited to confirm your reservation and look forward to your visit.</p><hr /><p><strong>Reservation Details:</strong></p><ul><li><strong>Name</strong>: {RecipientName}</li><li><strong>City: </strong>{LocationCity}</li><li><strong>Escape Room</strong>: {LocationName}</li><li><strong>Date</strong>: {Date}</li><li><strong>Time</strong>: {Time}</li><li><strong>Guests</strong>: {PlayerAmount}</li><li><strong>Duration</strong>: Approx. 3 Hours</li></ul><hr /><p><strong>After the Game:</strong></p><p>Once you've conquered the challenge, you're invited to relax and celebrate with a meal at our restaurant! If there are no wishes to eat at our restaurant, you are free to leave after completing the escape room.</p><hr /><p>If you need to modify your reservation, feel free to use the reservation manager you used to make this reservation.</p><p>We look forward to seeing you soon for an exciting and unforgettable experience!</p><p>Best regards,<br /><br /><strong>Team Escape &amp; Dine</strong><br />Hogeschool Rotterdam | Wijnhaven 99<br />mail.escapedine@gmail.com</p>";
        mail.IsBodyHtml = true;

        // Define the SMTP client
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        smtpClient.Port = 587; // SMTP port for TLS
        smtpClient.Credentials = new NetworkCredential("mail.escapedine@gmail.com", "czzw ygug utxw fpwv");
        smtpClient.EnableSsl = true; // Enable SSL for secure connection

        try {
            // Send the email
            smtpClient.Send(mail);
        } catch (Exception ex) {
            AnsiConsole.MarkupLine("[red]Error: " + ex.Message + "[/]");
            return false;
        }
        return true;
    }
    
    public static bool SendEditedReservationEmail(string RecipientName, string LocationCity, string LocationName, string Date, string Time, int PlayerAmount, string RecipientEmail)
    {
        // Define the email message
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("mail.escapedine@gmail.com");
        mail.To.Add(RecipientEmail);
        mail.Subject = "Your Escape and Dine reservation";
        mail.Body = $"<p><strong>Dear {RecipientName},</strong></p><p>Hereby your details for your <strong>Edited</strong> reservation, from now on please consider this your new <strong>Confirmation</strong> mail and discard the old one.</p><hr /><p><strong>Reservation Details:</strong></p><ul><li><strong>Name</strong>: {RecipientName}</li><li><strong>City</strong>: {LocationCity}</li><li><strong>Escape Room</strong>: {LocationName}</li><li><strong>Date</strong>: {Date}</li><li><strong>Time</strong>: {Time}</li><li><strong>Guests</strong>: {PlayerAmount}</li><li><strong>Duration</strong>: Approx. 3 Hours</li></ul><hr /><p><strong>After the Game:</strong></p><p>Once you've conquered the challenge, you're invited to relax and celebrate with a meal at our restaurant! If there are no wishes to eat at our restaurant, you are free to leave after completing the escape room.</p><hr /><p>If you need to modify your reservation, feel free to use the reservation manager you used to make this reservation.</p><p>We look forward to seeing you soon for an exciting and unforgettable experience!</p><p>&nbsp;</p><p>Best regards,<br /><br /><strong>Team Escape &amp; Dine</strong><br />Hogeschool Rotterdam | Wijnhaven 99<br />mail.escapedine@gmail.com</p>";
        mail.IsBodyHtml = true;

        // Define the SMTP client
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        smtpClient.Port = 587; // SMTP port for TLS
        smtpClient.Credentials = new NetworkCredential("mail.escapedine@gmail.com", "czzw ygug utxw fpwv");
        smtpClient.EnableSsl = true; // Enable SSL for secure connection

        try {
            // Send the email
            smtpClient.Send(mail);
        } catch (Exception ex) {
            AnsiConsole.MarkupLine("[red]Error: " + ex.Message + "[/]");
            return false;
        }
        return true;
    }

    public static bool SendPasswordForgorEmail(string RecipientEmail, string Code) {
        // Define the email message
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("mail.escapedine@gmail.com");
        mail.To.Add(RecipientEmail);
        mail.Subject = "Escape and Dine password reset request";
        mail.Body = $"<p><strong>Dear user,</strong></p><hr/><p>We have received a request to reset the password for the account registered to this email. To continue, please enter the code down below into the verification box:</p><p><strong>{Code}</strong></p><p>If you didn&rsquo;t request a password reset, please ignore this email, and your password will remain unchanged. If you have any concerns, you can contact us at mail.escapedine@gmail.com.</p><hr/><p>Thank you,<br />Escape &amp; Dine Support Team</p>";
        mail.IsBodyHtml = true;

        // Define the SMTP client
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        smtpClient.Port = 587; // SMTP port for TLS
        smtpClient.Credentials = new NetworkCredential("mail.escapedine@gmail.com", "czzw ygug utxw fpwv");
        smtpClient.EnableSsl = true; // Enable SSL for secure connection

        try {
            // Send the email
            smtpClient.Send(mail);
        } catch (Exception ex) {
            AnsiConsole.MarkupLine("[red]Error: " + ex.Message + "[/]");
            return false;
        }
        return true;
    }
    
    
    
    
}
