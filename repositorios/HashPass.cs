using System;
using BCrypt.Net;

public class HashPass
{ //usando libreria Bcrypt --> se usa parecido que en Node.  
    public static string HashearPass(string password)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine("CONTRASEÑA HASHEADA -->" + hashedPassword);
        return hashedPassword;
    }

    public static bool VerificarPassword(string password, string hashedPassword)
    {
        bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        Console.WriteLine(result);
        return result;
    }
}
