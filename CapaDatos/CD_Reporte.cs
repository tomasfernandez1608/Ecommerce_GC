using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDato;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public Dashboard DatosDashboard()
        {
            Dashboard objeto = new Dashboard();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("DB_ReporteDashboard", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            objeto = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                TotalProducto = Convert.ToInt32(reader["TotalProductos"]),
                                TotalVenta = Convert.ToInt32(reader["TotalVentas"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                objeto = new Dashboard();
            }

            return objeto;
        }


        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();
            /*lista.Add(
                                new Reporte()
                                {
                                    FechaVenta = " ",
                                    Cliente = " ",
                                    Producto = " ",
                                    Precio = 0,
                                    Cantidad = 0,
                                    Total = 0,
                                    IdTransaccion = "",
                                });*/
            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("DB_ReporteVentas", conexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Reporte()
                                {
                                    FechaVenta = reader["FechaVenta"].ToString(),
                                    Cliente = reader["Cliente"].ToString(),
                                    Producto = reader["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-AR")),
                                    Cantidad = Convert.ToInt32(reader["Cantidad"].ToString()),
                                    Total = Convert.ToDecimal(reader["Total"],new CultureInfo("es-AR")),
                                    IdTransaccion = reader["IdTransaccion"].ToString()
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Reporte>();
                
            }

            return lista;
        }



    }
}
