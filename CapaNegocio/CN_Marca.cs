using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapaDato = new CD_Marca();

        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Marca MarcaRegistrar, out string Mensaje)
        {
            Mensaje = string.Empty;
            
            if (string.IsNullOrEmpty(MarcaRegistrar.Descripcion) ||
                string.IsNullOrWhiteSpace(MarcaRegistrar.Descripcion))
            {
                Mensaje = "El nombre de la Marca no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Registrar(MarcaRegistrar, out Mensaje);

            }
            else { return 0; }

        }


        public bool Editar(Marca MarcaEditar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(MarcaEditar.Descripcion) ||
                string.IsNullOrWhiteSpace(MarcaEditar.Descripcion))
            {
                Mensaje = "El nombre de la Marca no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(MarcaEditar, out Mensaje);
            }
            else { return false; }
        }

        public bool Eliminar(int Id, out string Mensaje)
        {
            return objCapaDato.Eliminar(Id, out Mensaje);
        }
        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            return objCapaDato.ListarMarcaporCategoria(idcategoria);
        }
    }
}
