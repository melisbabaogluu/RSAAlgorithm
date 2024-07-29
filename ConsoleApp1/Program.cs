using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public class RSAKeyLoader
{
    public static RSA LoadPemPrivateKey(string pem)
    {
        var privateKeyBits = GetPrivateKeyBitsFromPem(pem);
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKeyBits, out _);
        return rsa;
    }

    public static RSA LoadPemPublicKey(string pem)
    {
        var publicKeyBits = GetPublicKeyBitsFromPem(pem);
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBits, out _);
        return rsa;
    }

    private static byte[] GetPrivateKeyBitsFromPem(string pem)
    {
        const string privateKeyHeader = "-----BEGIN PRIVATE KEY-----";
        const string privateKeyFooter = "-----END PRIVATE KEY-----";
        var privateKeyContent = ExtractPemContent(pem, privateKeyHeader, privateKeyFooter);
        return Convert.FromBase64String(privateKeyContent);
    }

    private static byte[] GetPublicKeyBitsFromPem(string pem)
    {
        const string publicKeyHeader = "-----BEGIN PUBLIC KEY-----";
        const string publicKeyFooter = "-----END PUBLIC KEY-----";
        var publicKeyContent = ExtractPemContent(pem, publicKeyHeader, publicKeyFooter);
        return Convert.FromBase64String(publicKeyContent);
    }

    private static string ExtractPemContent(string pem, string header, string footer)
    {
        int start = pem.IndexOf(header, StringComparison.Ordinal) + header.Length;
        int end = pem.IndexOf(footer, StringComparison.Ordinal);
        var base64 = pem.Substring(start, end - start).Trim();
        return base64;
    }
}

// Örnek kullanım:
string privateKeyPem = "-----BEGIN PRIVATE KEY-----\n...your private key here...\n-----END PRIVATE KEY-----";
string publicKeyPem = "-----BEGIN PUBLIC KEY-----\n...your public key here...\n-----END PUBLIC KEY-----";

RSA privateKey = RSAKeyLoader.LoadPemPrivateKey(privateKeyPem);
RSA publicKey = RSAKeyLoader.LoadPemPublicKey(publicKeyPem);
