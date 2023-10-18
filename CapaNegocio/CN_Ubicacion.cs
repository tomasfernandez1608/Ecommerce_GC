using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDato;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objcapadato = new CD_Ubicacion();


        public List<Provincias> ObtenerProvincia()
        {
            return objcapadato.ObtenerProvincia();
        }

        public List<Localidades> ObtenerLocalidad(string idprovincia)
        {
            return objcapadato.ObtenerLocalidad(idprovincia);
        }

    }
}
