using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

public class HashPass
{
    public static string HashearPass(string password)
    {
        byte[] salt;
        using (var generadorAleatorio = RandomNumberGenerator.Create())
        {
            salt = new byte[16];
            generadorAleatorio.GetBytes(salt);
        }

        string passHasheada = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return passHasheada;
    }

    public static bool VerificarPassword(string password, string hashedPassword)
    {
        if (hashedPassword == null)
        {
            // Si el hashedPassword es nulo, no se puede verificar la contraseña
            return false;
        }

        byte[] storedSalt = Convert.FromBase64String(hashedPassword.Substring(0, 24));
        string hashedEnteredPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: storedSalt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Verificar si las contraseñas coinciden
        return hashedEnteredPassword.Equals(hashedPassword.Substring(24));
    }

}
