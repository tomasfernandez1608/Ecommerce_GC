using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Globalization;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;

namespace CapaAdministrador.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Usuarios()
        {

            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().Listar();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Esto mantiene la capitalización exacta de las propiedades
            };
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Guardar(Usuario usuario)
        {
            object Resultado;
            string Mensaje = string.Empty;

            usuario.Correo = usuario.Correo.ToLower();

            if (usuario.IdUsuario == 0)
            {
                // Voy a crear un nuevo usuario.
                Resultado = new CN_Usuarios().Registrar(usuario, out Mensaje);
            }
            else
            {
                // Voy a Actualizar un usuario existente. 
                Resultado = new CN_Usuarios().Editar(usuario, out Mensaje);
            }

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new CN_Usuarios().Eliminar(id, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }

        [HttpGet]
        public JsonResult DatosDashboard()
        {
            Dashboard respuesta;

            respuesta = new CN_Reporte().DatosDashboard();

            return Json(new { datos = respuesta }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult ReporteVentas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Ventas(fechainicio,fechafin,idtransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public FileResult ExportarVentas(string inputfechainicio, string inputfechafin, string inputidtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Ventas(inputfechainicio, inputfechafin, inputidtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new CultureInfo("es-AR");//Definimos la region con la que estamos trabajando

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach(Reporte rp in oLista)
            {
                dt.Rows.Add(new Object[] {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion

            });
            }
            dt.TableName = "Datos";

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }


    }
}