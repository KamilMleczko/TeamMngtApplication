using System.Security.Cryptography;
using System.Text;

namespace TeamMngt.MoreClasses;

public class Hasher
{
    
    
    public static byte[] HashPasswordMD5(string password)
    {
        var hasher = new Hasher();
        var hashedPassword = hasher.ComputeHash(password, "MD5"); // Use MD5 for demonstration (not recommended)
        return Encoding.UTF8.GetBytes(hashedPassword); // Convert to byte array for storage
    }
    
    public string ComputeHash(string inputFile, string hashAlgorithm)
    {
        var hashAlgorithmInstance = GetHashAlgorithm(hashAlgorithm);
        var byteConverter = new UnicodeEncoding();
        string text = inputFile;
        byte[] textBytes = byteConverter.GetBytes(text);
        byte[] hashBytes = hashAlgorithmInstance.ComputeHash(textBytes);
        

        return BitConverter.ToString(hashBytes).ToLower();
    }

    public HashAlgorithm GetHashAlgorithm(string hashAlgorithm)
    {
        switch (hashAlgorithm)
        {
            case "SHA256":
                return SHA256.Create();
            case "SHA512":
                return SHA512.Create();
            case "MD5":
                return MD5.Create();
            default:
                throw new ArgumentException("Nieprawidłowy algorytm hashowania.");
        }
    }
}