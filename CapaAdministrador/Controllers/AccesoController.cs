using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaAdministrador.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Correo, string Clave)
        {
            Usuario oUsuario;
            Correo=Correo.ToLower();
            string clave = CN_Recursos.CalculateSHA256Hash(Clave);
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == Correo && u.Clave == clave).FirstOrDefault();

            if(oUsuario == null)
            {
                // EL view bag me permite guardar informacion que va utilizarce en la misma vista, es decir compartir informacion dentro de la misma vista
                ViewBag.Error = "Correo o Contraseña no correcta";
                return View();
            }
            else
            {
                if(oUsuario.Reestablecer)
                {
                    // TempData me permite compartir informacion con otras vistas que esten dentro de un mismo controlador

                    TempData["IdUsuario"] = oUsuario.IdUsuario;

                    return RedirectToAction("CambiarClave");
                }

                /*FormsAuthentication crear una cookie de autenticación en el navegador del usuario basada en el correo electrónico (u otro identificador) proporcionado y configurar la autenticación del usuario en la aplicación web. Esto permite que el usuario permanezca autenticado durante su sesión actual en la aplicación sin tener que volver a iniciar sesión en cada solicitud.*/
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);
                Session["Usuario"] = oUsuario;
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CambiarClave(string idUsuario,string claveActual, string claveNueva,string confirmarClave)
        {

            Usuario oUsuario;
            string Mensaje = string.Empty;
            bool respuesta = false;
            string clave = CN_Recursos.CalculateSHA256Hash(claveActual);
            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == Convert.ToInt32(idUsuario)).FirstOrDefault();

            if (oUsuario.Clave != clave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["eClave"] = "";
                ViewBag.Error = "Clave Actual INCORRECTA";
                return View();
            }
            else if (claveNueva!=confirmarClave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["eClave"] = claveActual;
                ViewBag.Error = "Claves NO COINCIDEN";
                return View();
            }
            ViewData["eClave"] = "";

            string nuevaClave = CN_Recursos.CalculateSHA256Hash(claveNueva);

            respuesta = new CN_Usuarios().CambiarClave(Convert.ToInt32(idUsuario),nuevaClave,out Mensaje);

            if(respuesta)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = Mensaje;
                return View();
            }

        }
        public ActionResult RestablecerClave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RestablecerClave(string Correo)
        {
            Usuario oUsuario= new Usuario();
            string Mensaje = string.Empty;
            bool resultado = false;
            Correo = Correo.ToLower();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == Correo).FirstOrDefault();

            if(oUsuario==null)//NO LO ENCONTRO
            {
                ViewBag.Error = "El correo ingresado no pertenece a un usuario";
                return View();
            }
            
            resultado = new CN_Usuarios().RestablecerClave(Convert.ToInt32(oUsuario.IdUsuario), Correo, out Mensaje);
            
            if(resultado)//Operacion exitosa
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = Mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }


    }
}