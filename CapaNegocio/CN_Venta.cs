using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDato;
using System.Data;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCapaDato = new CD_Venta();
        public bool RegistrarVenta(Venta objeto, DataTable DetalleVenta, out string Mensaje)
        {
            return objCapaDato.RegistrarVenta(objeto,DetalleVenta,out Mensaje);
        }
    }
}
