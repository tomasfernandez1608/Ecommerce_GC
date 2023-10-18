using CapaDato;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    
                    SqlCommand cmd = new SqlCommand("DB_ListarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Producto()
                                {
                                    IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    oMarca = new Marca(){ IdMarca = Convert.ToInt32(reader["IdMarca"]), Descripcion = reader["DesMarca"].ToString() },
                                    oCategoria = new Categoria(){ IdCategoria= Convert.ToInt32(reader["IdCategoria"]), Descripcion = reader["DesCategoria"].ToString() },
                                    Precio = Convert.ToDecimal(reader["Precio"]),
                                    Stock = Convert.ToInt32(reader["Stock"]),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString(),
                                    Activo = Convert.ToBoolean(reader["Activo"])
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Producto>();
            }

            return lista;
        }


        public int Registrar(Producto ProductoRegistrar, out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    
                    SqlCommand cmd = new SqlCommand("DB_RegistrarProducto", conexion);

                    cmd.Parameters.AddWithValue("Nombre", ProductoRegistrar.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", ProductoRegistrar.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", ProductoRegistrar.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", ProductoRegistrar.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", ProductoRegistrar.Precio);
                    cmd.Parameters.AddWithValue("Stock", ProductoRegistrar.Stock);
                    cmd.Parameters.AddWithValue("Activo", ProductoRegistrar.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    IdAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                IdAutogenerado = 0;
                Mensaje = ex.Message;
            }
            return IdAutogenerado;
        }

        public bool Editar(Producto ProductoEditar, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_EditarProducto", conexion);

                    cmd.Parameters.AddWithValue("IdProducto", ProductoEditar.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", ProductoEditar.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", ProductoEditar.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", ProductoEditar.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", ProductoEditar.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", ProductoEditar.Precio);
                    cmd.Parameters.AddWithValue("Stock", ProductoEditar.Stock);
                    cmd.Parameters.AddWithValue("Activo", ProductoEditar.Activo);
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


        public bool GuardarDatosImagen(Producto producto, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    string query = "UPDATE PRODUCTO SET RutaImagen = @RutaImagen, NombreImagen = @NombreImagen WHERE IdProducto = @IdProducto";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("RutaImagen", producto.RutaImagen);
                    cmd.Parameters.AddWithValue("NombreImagen", producto.NombreImagen);
                    cmd.Parameters.AddWithValue("IdProducto", producto.IdProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery()>0)
                    {
                        Resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar";
                    }
                    

                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }

            return Resultado;

        }




        public bool Eliminar(int Id, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_EliminarProducto", conexion);

                    cmd.Parameters.AddWithValue("IdProducto", Id);

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
