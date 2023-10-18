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
    public class CD_Clientes
    {


        public int Registrar(Cliente ClienteRegistrar, out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_RegistrarCliente", conexion);

                    cmd.Parameters.AddWithValue("Nombre", ClienteRegistrar.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", ClienteRegistrar.Apellido);
                    cmd.Parameters.AddWithValue("Correo", ClienteRegistrar.Correo);
                    cmd.Parameters.AddWithValue("Clave", ClienteRegistrar.Clave);
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





        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    string query = "SELECT IdCliente, Nombres, Apellido, Correo, Clave, Reestablecer  FROM CLIENTE;";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                    Nombre = reader["Nombres"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Clave = reader["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(reader["Reestablecer"]),
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Cliente>();
            }

            return lista;
        }


        public bool CambiarClave(int IdCliente, string nuevaClave, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET Clave = @nuevaClave, Reestablecer = 0 WHERE IdCliente = @IdCliente", conexion);

                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                    cmd.CommandType = CommandType.Text;

                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool RestablecerClave(int IdCliente, string Clave, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Cliente SET Clave = @Clave, Reestablecer = 1 WHERE IdCliente = @IdCliente;", conexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@Clave", Clave);
                    cmd.CommandType = CommandType.Text;

                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

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
