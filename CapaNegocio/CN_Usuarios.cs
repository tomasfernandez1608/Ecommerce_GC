using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDato;
using System.Security.Cryptography;
using System.Security.Claims;


namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();
        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(Usuario UsuarioRegistrar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(UsuarioRegistrar.Nombres) ||
                string.IsNullOrWhiteSpace(UsuarioRegistrar.Nombres))
            {
                Mensaje = "El nombre de Usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(UsuarioRegistrar.Apellidos) ||
                string.IsNullOrWhiteSpace(UsuarioRegistrar.Apellidos))
            {
                Mensaje = "El nombre de Apellido no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(UsuarioRegistrar.Correo) ||
                string.IsNullOrWhiteSpace(UsuarioRegistrar.Correo))
            {
                Mensaje = "El nombre de Correo no puede ser vacio";
            }

            if(string.IsNullOrEmpty(Mensaje))
            {

                
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Clave de cuenta GamingCity";
                string mensajeCorreo = "<h3> Su cuenta fue creada correctamente</h3></br>" +
                    "<p>Su clave de acceso es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(UsuarioRegistrar.Correo, asunto, mensajeCorreo);

                if (respuesta) 
                {
                    UsuarioRegistrar.Clave = CN_Recursos.CalculateSHA256Hash(clave);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo, correo no valido";
                    return 0;
                }
                
                
                return objCapaDato.Registrar(UsuarioRegistrar,out Mensaje);

            }
            else { return 0; }

        }


        public bool Editar(Usuario UsuarioEditar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(UsuarioEditar.Nombres) ||
                string.IsNullOrWhiteSpace(UsuarioEditar.Nombres))
            {
                Mensaje = "El nombre de Usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(UsuarioEditar.Apellidos) ||
                string.IsNullOrWhiteSpace(UsuarioEditar.Apellidos))
            {
                Mensaje = "El nombre de Apellido no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(UsuarioEditar.Correo) ||
                string.IsNullOrWhiteSpace(UsuarioEditar.Correo))
            {
                Mensaje = "El nombre de Correo no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(UsuarioEditar,out Mensaje);
            }
            else { return false; }
        }


        public bool Eliminar(int Id, out string Mensaje)
        {
            return objCapaDato.Eliminar(Id, out Mensaje);
        }

        public bool CambiarClave(int IdUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(IdUsuario, nuevaClave,out Mensaje);
        }

        public bool RestablecerClave(int IdUsuario, string Correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.RestablecerClave(IdUsuario, CN_Recursos.CalculateSHA256Hash(nuevaClave), out Mensaje);


            if(resultado)
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
