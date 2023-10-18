using CapaDato;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {

        public bool ExisteCarrito(int idcliente,int idproducto)
        {
            bool resultado=true;
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_ExisteCarrito", conexion);
                    
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                resultado = false;
            }
            return resultado;
        }


        public bool OperacionCarrito(int idcliente, int idproducto,bool sumar, out string Mensaje)
        {
            Mensaje = "";
            bool resultado = true;
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_OperacionCarrito", conexion);

                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();



                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public int CantidadEnCarrito(int idcliente)
        {
            int Resultado = 0;
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("SELECT count(*) from CARRITO WHERE IdCliente = @idcliente", conexion);

                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    Resultado = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                Resultado = 0;
            }
            return Resultado;
        }

        public List<Carrito> ListarProducto(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    string query = "SELECT * FROM DB_ObtenerCarritoCliente(@idcliente)";

                    SqlCommand cmd = new SqlCommand(query,conexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Carrito()
                                {
                                    oProducto = new Producto()
                                    {
                                        IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                        Nombre = reader["Nombre"].ToString(),
                                        oMarca = new Marca() {  Descripcion = reader["DesMarca"].ToString() },
                                        Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-AR")),
                                        RutaImagen = reader["RutaImagen"].ToString(),
                                        NombreImagen = reader["NombreImagen"].ToString(),
                                    },
                                    Cantidad = Convert.ToInt32(reader["Cantidad"])
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Carrito>();
            }

            return lista;
        }

        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_EliminarCarrito", conexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                resultado = false;
            }
            return resultado;
        }

    }
}
