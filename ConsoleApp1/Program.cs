using System;
using System.Security.Cryptography;
using System.Text;

namespace SHA256RSAApp
{
    public class Program
    {
        static void Main()
        {
            // İmzalanacak mesaj
            string message = "Bu imzalanacak mesajdır.";

            // RSA anahtar çifti oluşturma
            using (RSA rsa = RSA.Create(1024))
            {
                // Özel anahtarı içeren XML formatında anahtar çifti
                string privateKey = rsa.ToXmlString(true);
                // Sadece public anahtarı içeren XML formatında anahtar çifti
                string publicKey = rsa.ToXmlString(false);

                // SHA-256 hash oluşturma
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
                    Console.WriteLine("SHA-256 Hash: " + BitConverter.ToString(hashValue).Replace("-", ""));

                    // Özel anahtarla imza oluşturma
                    byte[] signedHash;
                    string base64Encoded;
                    using (RSA rsaPrivate = RSA.Create())
                    {
                        rsaPrivate.FromXmlString(privateKey);
                        signedHash = rsaPrivate.SignHash(hashValue, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                        base64Encoded = Convert.ToBase64String(signedHash);
                    }
                    Console.WriteLine("İmzalanmış Hash: " + BitConverter.ToString(signedHash).Replace("-", ""));
                    Console.WriteLine("Base64: " + base64Encoded);

                    // Public anahtarla imza doğrulama
                    bool verified;
                    using (RSA rsaPublic = RSA.Create())
                    {
                        rsaPublic.FromXmlString(publicKey);
                        verified = rsaPublic.VerifyHash(hashValue, signedHash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    }
                    Console.WriteLine("Public Key ile İmza Doğrulandı: " + verified);

                    // Public ve private key'leri yazdırma
                    Console.WriteLine("Private Key: " + privateKey);
                    Console.WriteLine("Public Key: " + publicKey);
                }
            }
        }
    }
}

