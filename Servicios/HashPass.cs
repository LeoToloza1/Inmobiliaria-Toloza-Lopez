using System;
using BCrypt.Net;

namespace inmobiliaria_Toloza_Lopez.Servicios
{
    static public class HashPass
    { //usando libreria Bcrypt --> se usa parecido que en Node.  
        public static string HashearPass(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public static bool VerificarPassword(string password, string hashedPassword)
        {
            bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return result;
        }
    }
}