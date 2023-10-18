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
    public class CD_Marca
    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    string query = "SELECT IdMarca,Descripcion, Activo FROM MARCA;";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    Activo = Convert.ToBoolean(reader["Activo"])
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Marca>();
            }

            return lista;
        }


        public int Registrar(Marca MarcaRegistrar, out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_RegistrarMarca", conexion);
                    cmd.Parameters.AddWithValue("Descripcion", MarcaRegistrar.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", MarcaRegistrar.Activo);
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

        public bool Editar(Marca MarcaEditar, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_EditarMarca", conexion);

                    cmd.Parameters.AddWithValue("IdMarca", MarcaEditar.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", MarcaEditar.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", MarcaEditar.Activo);
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

                    SqlCommand cmd = new SqlCommand("DB_EliminarMarca", conexion);

                    cmd.Parameters.AddWithValue("IdMarca", Id);

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

        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    StringBuilder str = new StringBuilder();

                    str.AppendLine("SELECT distinct m.IdMarca, m.Descripcion FROM PRODUCTO p");
                    str.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");
                    str.AppendLine("inner join MARCA m on m.IdMarca = p.IdMarca and m.Activo=1");
                    str.AppendLine("WHERE c.IdCategoria=IIF(@idcategoria = 0,c.IdCategoria,@idcategoria)");


                    SqlCommand cmd = new SqlCommand(str.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(reader["IdMarca"]),
                                    Descripcion = reader["Descripcion"].ToString(),
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Marca>();
            }

            return lista;
        }
    }
}
