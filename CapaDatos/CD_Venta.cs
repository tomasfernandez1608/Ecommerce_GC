using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using CapaDato;



namespace CapaDatos
{
    public class CD_Venta
    {
        public bool RegistrarVenta(Venta objeto, DataTable DetalleVenta ,out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_RegistrarVenta", conexion);

                    cmd.Parameters.AddWithValue("IdCliente", objeto.IdCliente);
                    cmd.Parameters.AddWithValue("TotalProducto", objeto.TotalProducto);
                    cmd.Parameters.AddWithValue("MontoTotal", objeto.MontoTotal);
                    cmd.Parameters.AddWithValue("Contacto", objeto.Contacto);
                    cmd.Parameters.AddWithValue("IdLocalidad", objeto.IdLocalidad);
                    cmd.Parameters.AddWithValue("Telefono", objeto.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", objeto.Dirrecion);
                    cmd.Parameters.AddWithValue("IdTransaccion", objeto.IdTransaccion);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }
    }
}
