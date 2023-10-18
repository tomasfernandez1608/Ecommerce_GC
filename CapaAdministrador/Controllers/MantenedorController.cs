using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using Firebase.Auth;
using Firebase.Storage;
using CapaNegocio;
using System.Web.Services.Description;
using Newtonsoft.Json;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

namespace CapaAdministrador.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {

        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }
        

        public ActionResult Index()
        {
            return View();
        }
        
        // Categoria
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Esto mantiene la capitalización exacta de las propiedades
            };
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Guardar( Categoria categoria)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (categoria.IdCategoria == 0)
            {
                // Voy a crear un nueva categoria.
                Resultado = new CN_Categoria().Registrar(categoria, out Mensaje);
            }
            else
            {
                // Voy a Actualizar una categoria existente. 
                Resultado = new CN_Categoria().Editar(categoria, out Mensaje);
            }

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new CN_Categoria().Eliminar(id, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }

        // MARCA
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().Listar();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Esto mantiene la capitalización exacta de las propiedades
            };
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarMarca( Marca Marca)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (Marca.IdMarca == 0)
            {
                // Voy a crear un nueva categoria.
                Resultado = new CN_Marca().Registrar(Marca, out Mensaje);
            }
            else
            {
                // Voy a Actualizar una categoria existente. 
                Resultado = new CN_Marca().Editar(Marca, out Mensaje);
            }

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }
        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new CN_Marca().Eliminar(id, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }
        /// PRODUCTO
        /// 

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Esto mantiene la capitalización exacta de las propiedades
            };
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new CN_Producto().Eliminar(id, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje });
        }



        public async Task<JsonResult> GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion = true;
            bool guardarimagen = true;

            Producto producto = new Producto();
            producto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;
            // validamos el formato del precio
            if (decimal.TryParse(producto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR"), out precio))
            {
                producto.Precio = precio;
            }
            else
            {
                return Json(new { operacion = false, mensaje = "No se acepta el formato del precio(XX.XX)" });
            }


            if (producto.IdProducto == 0)
            {
                int idgenerado = new CN_Producto().Registrar(producto, out mensaje);

                if(idgenerado !=0)
                {
                    producto.IdProducto= idgenerado;
                }
                else
                {
                    operacion = false;
                }
            }
            else
            {
                operacion = new CN_Producto().Editar(producto, out mensaje);
            }

            if(operacion)
            {
                if(archivoImagen!=null)// validamos que se haya cargado la imagen
                {
                    
                    //string rutaguardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extensionImg = Path.GetExtension(archivoImagen.FileName);//Obtenemos la extencion de la imagen 
                    string nombre_img = string.Concat(producto.IdProducto.ToString(), extensionImg);
                    string rutaguardar="Error al cargar la imagen";
                    try
                    {
                        //archivoImagen.SaveAs(Path.Combine(rutaguardar,nombre_img));//Vamos a guardar en la ruta la img con el nombre asignado
                        Stream stream = archivoImagen.InputStream;
                        rutaguardar = await SubirStorage(stream, nombre_img) ;
                        if(rutaguardar == null)
                        {
                            return Json(new { resultadooperacion = operacion, idgenerado = producto.IdProducto, mensaje = "Error al guardar la imagen" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch(Exception ex)
                    {
                        string mensageimagenerror = ex.Message;
                        guardarimagen = false;
                    }
                    if(guardarimagen)
                    {
                        producto.RutaImagen = rutaguardar;
                        producto.NombreImagen = nombre_img;
                        bool respuestaImagen = new CN_Producto().GuardarDatosImagen(producto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen ";
                    }

                }
            }

            return Json(new { resultadooperacion = operacion, idgenerado = producto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool operacion;
            Producto producto = new CN_Producto().Listar().Where(prod =>prod.IdProducto == id).FirstOrDefault();
            //string Base64 = CN_Recursos.ConvertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out operacion);
            if(producto != null)
            {
                operacion = true;
            }
            else
            {
                operacion = false;
            }
            return Json(new { resultadooperacion = operacion, ruta = producto.RutaImagen },JsonRequestBehavior.AllowGet);


        }


        //Guardar imagen en firebase

        public async Task<string> SubirStorage(Stream archivo, string nombre)
        {
            string email = "gamingcityecommerce@gmail.com";
            string clave = "123456";
            string ruta = "eco-gm.appspot.com";
            string api_key = "AIzaSyAwjRQDQsDi8j_XMGJEl8X5a7n964T5fLY";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                )
                .Child("Fotos_Producto")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;

            return downloadURL;

        }


    }
}