using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IncidenciasUdec.Servicios
{
    public static class Utilidades
    {
        public static string EncriptarContraseña(string pass)
        {
            string hash = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashvalue = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                foreach (byte b in hashvalue)
                {
                    hash += $"{b:X2}";
                }
            }
            return hash;
        }

        public static string GenerarToken()
        {
            string Token = Guid.NewGuid().ToString("N");
            return Token;
        }
    }
}