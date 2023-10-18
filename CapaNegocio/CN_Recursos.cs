using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.IO;
namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0,6);
            // Guid no va a retornar una clave(string) aleatoria de 6 digitos
            return clave;
        }

        public static string CalculateSHA256Hash(string EncriptarClave)
        {

            

            using (SHA256 sha256 = SHA256.Create())
            {
                // Convierte la cadena de entrada en un arreglo de bytes
                byte[] bytes = Encoding.UTF8.GetBytes(EncriptarClave);

                // Calcula el hash SHA-256
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convierte el hash en una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        /* Incluimos using System.Net.Mail;
                     using System.Net;
                     using System.IO;
            para poder generar el correo que le llegue al usuario con su clave
        */

        /*
         Datos de cuenta:
            gamingcityecommerce@gmail.com

         */
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);//Indico hacia quien va dirigido el mail
                mail.From = new MailAddress("gamingcityecommerce@gmail.com"); //Indico la cuenta desde donde envio el mail
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()//smpt va a ser nuestro servidor cliente que envia el email
                {
                    Credentials = new NetworkCredential("gamingcityecommerce@gmail.com", "bufjzdyrfcginmin"),
                    Host = "smtp.gmail.com",// el host va a ser el servidor que usa gmail para enviar los correos
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex) 
            {
                string error = ex.Message;
                resultado = false;
            }

            return resultado;

        }

        public static string ConvertirBase64(string ruta, out bool resultado_operacion)
        {
            string Base64 = string.Empty;
            resultado_operacion = true;
            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                Base64 = Convert.ToBase64String(bytes);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                resultado_operacion = false;
            }
            return Base64;
        }


    }
}
