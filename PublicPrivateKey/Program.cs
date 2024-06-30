using System;
using System.Security.Cryptography;

namespace PublicPrivateKey
{
    public class Program
    {
        static void Main()
        {
            // RSA servisini başlat
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    // Public ve Private key çiftini üret
                    RSAParameters rsaKeyInfo = rsa.ExportParameters(true);

                    // Private Key
                    string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                    Console.WriteLine("Private Key:");
                    Console.WriteLine(privateKey);

                    // Public Key
                    string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                    Console.WriteLine("Public Key:");
                    Console.WriteLine(publicKey);
                }
                finally
                {
                    // Kaynakları serbest bırak
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}



