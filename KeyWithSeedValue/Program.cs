using System;
using System.Security.Cryptography;
using System.Text;

namespace KeyWithSeedValue
{
    class Program
    {
        static void Main()
        {
            // Belirli bir seed değeri kullanarak RSA anahtar çifti oluştur
            long seedValue = 333244546848546854; // Örnek seed değeri
            GenerateRSAKeysWithSeed(seedValue);
        }

        static void GenerateRSAKeysWithSeed(long seed)
        {
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    // Rastgele sayı üreticiye seed değeri atayın
                    var rng = new RNGCryptoServiceProvider();
                    byte[] seedBytes = BitConverter.GetBytes(seed);
                    rng.GetBytes(seedBytes);
                    var rngCryptoServiceProvider = new RNGCryptoServiceProvider(seedBytes);

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

                    // Dosyalara kaydetme
                    System.IO.File.WriteAllText("PublicKey.xml", publicKey);
                    System.IO.File.WriteAllText("PrivateKey.xml", privateKey);

                    Console.WriteLine("Anahtarlar başarıyla oluşturuldu ve dosyalara kaydedildi.");
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}

