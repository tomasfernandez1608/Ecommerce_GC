using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using System.Web.Security;

namespace CapaTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Restablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombre"] = string.IsNullOrEmpty(objeto.Nombre) ? "" : objeto.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(objeto.Apellido) ? "" : objeto.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if(objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Contraseña no coinciden";
                return View();
            }

            resultado = new CN_Cliente().Registrar(objeto, out mensaje);
            
            if(resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;
            correo = correo.ToLower();
            oCliente = new CN_Cliente().Listar().Where(o=> o.Clave == CN_Recursos.CalculateSHA256Hash(clave) && o.Correo == correo).FirstOrDefault();

            if(oCliente==null)
            {
                ViewBag.Error = "Email o Constraseña, INCORRECTAS";
                return View();
            }
            else
            {
                if(oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    //Voy a crear una sesion del cliente para poder acceder a la informacion del cliente en todo momento
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }


            }
            return View();
        }
        [HttpPost]
        public ActionResult Restablecer(string Correo)
        {
            Cliente oCliente = new Cliente();
            string Mensaje = string.Empty;
            bool resultado = false;
            Correo = Correo.ToLower();
            oCliente = new CN_Cliente().Listar().Where(u => u.Correo == Correo).FirstOrDefault();

            if (oCliente == null)//NO LO ENCONTRO
            {
                ViewBag.Error = "El correo ingresado no pertenece a un Cliente";
                return View();
            }

            resultado = new CN_Cliente().RestablecerClave(Convert.ToInt32(oCliente.IdCliente), Correo, out Mensaje);

            if (resultado)//Operacion exitosa
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = Mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string claveActual, string claveNueva, string confirmarClave)
        {
            Cliente oCliente;
            string Mensaje = string.Empty;
            bool respuesta = false;

            string clave = CN_Recursos.CalculateSHA256Hash(claveActual);
            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == Convert.ToInt32(idCliente)).FirstOrDefault();

            if (oCliente.Clave != clave)
            {
                TempData["IdCliente"] = idCliente;
                ViewData["eClave"] = "";
                ViewBag.Error = "Clave Actual INCORRECTA";
                return View();
            }
            else if (claveNueva != confirmarClave)
            {
                TempData["IdCliente"] = idCliente;
                ViewData["eClave"] = claveActual;
                ViewBag.Error = "Claves NO COINCIDEN";
                return View();
            }
            ViewData["eClave"] = "";

            string nuevaClave = CN_Recursos.CalculateSHA256Hash(claveNueva);

            respuesta = new CN_Cliente().CambiarClave(Convert.ToInt32(idCliente), nuevaClave, out Mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                TempData["IdCliente"] = idCliente;
                ViewBag.Error = Mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}