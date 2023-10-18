using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Producto ProductoRegistrar, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(ProductoRegistrar.Descripcion) ||
                string.IsNullOrWhiteSpace(ProductoRegistrar.Descripcion))
            {
                Mensaje = "La descripcion del Producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(ProductoRegistrar.Nombre) ||
                string.IsNullOrWhiteSpace(ProductoRegistrar.Nombre))
            {
                Mensaje = "La Nombre del Producto no puede ser vacio";
            }
            else if (ProductoRegistrar.oMarca.IdMarca==0)
            {
                Mensaje = "Debe Seleccionar una marca";
            }
            else if (ProductoRegistrar.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe Seleccionar una marca";
            }
            else if (ProductoRegistrar.Stock== 0)
            {
                Mensaje = "Debe Ingresar el Stock";
            }
            else if (ProductoRegistrar.Precio == 0)
            {
                Mensaje = "Debe Ingresar el Precio del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Registrar(ProductoRegistrar, out Mensaje);

            }
            else { return 0; }

        }

        public bool Editar(Producto ProductoEditar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(ProductoEditar.Descripcion) ||
                string.IsNullOrWhiteSpace(ProductoEditar.Descripcion))
            {
                Mensaje = "La descripcion del Producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(ProductoEditar.Nombre) ||
                string.IsNullOrWhiteSpace(ProductoEditar.Nombre))
            {
                Mensaje = "La Nombre del Producto no puede ser vacio";
            }
            else if (ProductoEditar.oMarca.IdMarca == 0)
            {
                Mensaje = "Debe Seleccionar una marca";
            }
            else if (ProductoEditar.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe Seleccionar una marca";
            }
            else if (ProductoEditar.Stock == 0)
            {
                Mensaje = "Debe Ingresar el Stock";
            }
            else if (ProductoEditar.Precio == 0)
            {
                Mensaje = "Debe Ingresar el Precio del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(ProductoEditar, out Mensaje);
            }
            else { return false; }
        }


        public bool Eliminar(int Id, out string Mensaje)
        {
            return objCapaDato.Eliminar(Id, out Mensaje);
        }

        public bool GuardarDatosImagen(Producto producto, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(producto, out Mensaje);
        }



    }
}
