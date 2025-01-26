using System.Text.RegularExpressions;

static class Util {
    public static string GetUntil(this string text, string stopAt) {
        if (!String.IsNullOrWhiteSpace(text)) {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0) {
                return text.Substring(0, charLocation);
            }
        }

        return String.Empty;
    }

    public static string RandomString(int length) {
        Random random = new();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static bool EmailValidOrEmpty(string Email) {
        if (string.IsNullOrEmpty(Email)) {
            return true;
        }
        if (string.IsNullOrWhiteSpace(Email)) {
            return false;
        }

        return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(Email);
    }

    public static bool PasswordValidOrEmpty(string Password) {
        if (string.IsNullOrEmpty(Password)) {
            return true;
        }
        if (string.IsNullOrWhiteSpace(Password)) {
            return false;
        }

        return new Regex(@".{8,}").IsMatch(Password);
    }
}
