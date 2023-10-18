using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
namespace CapaTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idproducto=0)
        {
            Producto oProducto = new Producto();
            bool encontrado=false;

            oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();
            
            if (oProducto != null)
            {
                //oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen,oProducto.NombreImagen),out conversion);
                //oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
                encontrado = true;
            }
            if (encontrado)
            {
                return View(oProducto);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();

            return Json(new {data=lista},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMarcaporCategorias(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen,p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo=p.Activo
            }).Where(p => p.oCategoria.IdCategoria ==  (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
            p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&  p.Stock > 0 && p.Activo==true).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;
            
            if(existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);

            return Json(new { cantidad = cantidad}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public JsonResult ListarProductoCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<Carrito> lista = new List<Carrito>();

            bool conversion;

            lista = new CN_Carrito().ListarProducto(idcliente).Select(
                oc => new Carrito()
                {
                    oProducto = new Producto()
                    {
                        IdProducto = oc.oProducto.IdProducto,
                        Nombre = oc.oProducto.Nombre,
                        oMarca = oc.oProducto.oMarca,
                        Precio = oc.oProducto.Precio,
                        RutaImagen = oc.oProducto.RutaImagen,
                        Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion)
                    },
                    Cantidad = oc.Cantidad
                }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            
            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);
            
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult ObtenerProvincia()
        {
            List<Provincias> lista = new List<Provincias>();

            lista = new CN_Ubicacion().ObtenerProvincia();

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ObtenerLocalidad(string idprovincia)
        {
            List<Localidades> lista = new List<Localidades>();

            lista = new CN_Ubicacion().ObtenerLocalidad(idprovincia);

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);

        }
        

        public ActionResult Carrito()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito>oListaCarrito, Venta oVenta)
        {
            decimal total = 0;
            Random random = new Random(1513);
            string codigoventa = "COD" +  Convert.ToString(random.Next());
            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new System.Globalization.CultureInfo("es-AR");
            detalle_venta.Columns.Add("IdProducto",typeof(string));
            detalle_venta.Columns.Add("Cantidad",typeof(int));
            detalle_venta.Columns.Add("Total",typeof(decimal));
            
            foreach(Carrito ocarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(ocarrito.Cantidad.ToString()) * ocarrito.oProducto.Precio;
                total += subtotal;

                detalle_venta.Rows.Add(new object[]
                {
                    ocarrito.oProducto.IdProducto,
                    ocarrito.Cantidad,
                    subtotal
                });
            }

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new { Status = true, Link = "/Tienda/PagoEfectuado?idTransaccion="+codigoventa+"&status=true" }, JsonRequestBehavior.AllowGet);


        }

        public async Task<ActionResult> PagoEfectuado()
        {
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean( Request.QueryString["status"] );

            ViewData["Status"] = status;

            if(status)
            {
                // El temp data nos sirve para compartir informacion entre metodos que estan dentro de un mismo controlador 
                // en este caso estamos tomando la informacion de venta guardada cuando realizamos el proceso de pago
                Venta oVenta = (Venta)TempData["Venta"];

                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = idtransaccion;

                bool respuesta = new CN_Venta().RegistrarVenta(oVenta, detalle_venta, out string mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return View();

        }



    }
}