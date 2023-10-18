using CapaDato;
using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {

        private CD_Clientes objCapaDato = new CD_Clientes();
        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(Cliente ClienteRegistrar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(ClienteRegistrar.Nombre) ||
                string.IsNullOrWhiteSpace(ClienteRegistrar.Nombre))
            {
                Mensaje = "El nombre del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(ClienteRegistrar.Apellido) ||
                string.IsNullOrWhiteSpace(ClienteRegistrar.Apellido))
            {
                Mensaje = "El  Apellido del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(ClienteRegistrar.Correo) ||
                string.IsNullOrWhiteSpace(ClienteRegistrar.Correo))
            {
                Mensaje = "El Correo del Cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                ClienteRegistrar.Clave = CN_Recursos.CalculateSHA256Hash(ClienteRegistrar.Clave);

                return objCapaDato.Registrar(ClienteRegistrar, out Mensaje);

            }
            else { return 0; }

        }


        public bool CambiarClave(int IdCliente, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(IdCliente, nuevaClave, out Mensaje);
        }

        public bool RestablecerClave(int IdCliente, string Correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.RestablecerClave(IdCliente, CN_Recursos.CalculateSHA256Hash(nuevaClave), out Mensaje);


            if (resultado)
            {
                string asunto = "Contraseña Reestablecida - GamingCity";
                string mensajeCorreo = "<h3> Su cuenta fue reestablecida correctamente</h3></br>" +
                    "<p>Su nueva clave de acceso es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaClave);

                bool respuesta = CN_Recursos.EnviarCorreo(Correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se puede enviar el correo, correo no valido";
                    return false;
                }


            }
            Mensaje = "No se puede enviar el correo, correo no valido";
            return false;
        }


    }
}
