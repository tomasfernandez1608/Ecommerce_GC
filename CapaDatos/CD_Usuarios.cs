using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections;


namespace CapaDato
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            var newConexion = new Conexion();

            try
            {
                using(var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    string query = "SELECT IdUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo FROM USUARIO;";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                    Nombres = reader["Nombres"].ToString(),
                                    Apellidos = reader["Apellidos"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Clave = reader["Clave"].ToString(),
                                    Reestablecer =Convert.ToBoolean( reader["Reestablecer"]),
                                    Activo = Convert.ToBoolean(reader["Activo"])
                                });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                lista = new List<Usuario>();
            }

            return lista;
        }

        public int Registrar (Usuario UsuarioRegistrar, out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    
                    SqlCommand cmd = new SqlCommand("DB_RegistrarUsuario", conexion);
                    
                    cmd.Parameters.AddWithValue("Nombres", UsuarioRegistrar.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", UsuarioRegistrar.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", UsuarioRegistrar.Correo);
                    cmd.Parameters.AddWithValue("Clave", UsuarioRegistrar.Clave);
                    cmd.Parameters.AddWithValue("Activo", UsuarioRegistrar.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output ;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    IdAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                    
                }
            }
            catch(Exception ex)
            {
                IdAutogenerado = 0;
                Mensaje =ex.Message;
            }
            return IdAutogenerado;
        }

        public bool Editar(Usuario UsuarioEditar, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DB_EditarUsuario", conexion);

                    cmd.Parameters.AddWithValue("IdUsuario", UsuarioEditar.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", UsuarioEditar.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", UsuarioEditar.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", UsuarioEditar.Correo);
                    cmd.Parameters.AddWithValue("Activo", UsuarioEditar.Activo);
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
                Resultado= false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool Eliminar (int Id, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("DELETE TOP(1) FROM USUARIO WHERE IdUsuario=@Id;", conexion);

                    cmd.Parameters.AddWithValue("@Id", Id);
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
        public bool CambiarClave(int IdUsuario,string nuevaClave, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET Clave = @nuevaClave, Reestablecer = 0 WHERE IdUsuario = @IdUsuario", conexion);

                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
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

        public bool RestablecerClave(int IdUsuario, string Clave, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = "";
            var newConexion = new Conexion();
            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET Clave = @Clave, Reestablecer = 1 WHERE IdUsuario = @IdUsuario;", conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
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
