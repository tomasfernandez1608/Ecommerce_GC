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
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria CategoriaRegistrar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(CategoriaRegistrar.Descripcion) ||
                string.IsNullOrWhiteSpace(CategoriaRegistrar.Descripcion))
            {
                Mensaje = "El nombre de Categoria no puede ser vacio";
            }
            

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Registrar(CategoriaRegistrar, out Mensaje);

            }
            else { return 0; }

        }

        public bool Editar(Categoria CategoriaEditar, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validamos que el campo nombre,apellido, correo; no esten vacios
            if (string.IsNullOrEmpty(CategoriaEditar.Descripcion) ||
                string.IsNullOrWhiteSpace(CategoriaEditar.Descripcion))
            {
                Mensaje = "El nombre de la Categoria no puede ser vacio";
            }
            

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(CategoriaEditar, out Mensaje);
            }
            else { return false; }
        }

        public bool Eliminar(int Id, out string Mensaje)
        {
            return objCapaDato.Eliminar(Id, out Mensaje);
        }


    }
}
