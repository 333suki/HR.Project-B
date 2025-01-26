public static class LoginLogic {
    public static bool VerifyPassword(string Email, string Password) {
        if (!Database.UsersTableContainsUser(Email)) {
            return false;
        }
        //Console.WriteLine(Password == Encryptor.Decrypt(Database.GetEncryptedPassword(Username)!));
        return Password == Encryptor.Decrypt(Database.GetEncryptedPassword(Email)!);
    }
}
