using System.Text;
using System.Security.Cryptography;

public static class Encryptor {
    public static string Encrypt(string ToEncrypt) {
        string EncryptionKey = "WELOVEGERT";
        byte[] ClearBytes = Encoding.Unicode.GetBytes(ToEncrypt);
        using (Aes Encryptor = Aes.Create()) {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76]);
            Encryptor.Key = pdb.GetBytes(32);
            Encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, Encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {
                    cs.Write(ClearBytes, 0, ClearBytes.Length);
                    cs.Close();
                }
                ToEncrypt = Convert.ToBase64String(ms.ToArray());
            }
        }
        return ToEncrypt;
    }

    public static string Decrypt(string ToDecrypt) {
        string EncryptionKey = "WELOVEGERT";
        ToDecrypt = ToDecrypt.Replace(" ", "+");
        byte[] CipherBytes = Convert.FromBase64String(ToDecrypt);
        using (Aes encryptor = Aes.Create()) {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76]);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)) {
                    cs.Write(CipherBytes, 0, CipherBytes.Length);
                    cs.Close();
                }
                ToDecrypt = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return ToDecrypt;
    }
}
