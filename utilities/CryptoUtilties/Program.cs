// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;

using (var rng = RandomNumberGenerator.Create())
{
    byte[] key = new byte[64]; // 256 bits for HS512
    rng.GetBytes(key);
    string base64Key = Convert.ToBase64String(key);
    Console.WriteLine($"Generated JWT Signing Key: {base64Key}");
}