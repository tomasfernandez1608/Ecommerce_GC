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
    public class CD_Ubicacion
    {
        public List<Provincias> ObtenerProvincia()
        {
            List<Provincias> lista = new List<Provincias>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    string query = "SELECT * FROM PROVINCIAS";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Provincias()
                                {
                                    IdProvincia = reader["IdProvincia"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Provincias>();
            }

            return lista;
        }

        public List<Localidades> ObtenerLocalidad(string idprovincia)
        {
            List<Localidades> lista = new List<Localidades>();

            var newConexion = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(newConexion.getCadenaSQL()))
                {
                    conexion.Open();
                    string query = "SELECT * FROM LOCALIDADES WHERE IdProvincia = @idprovincia";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idprovincia", idprovincia);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Localidades()
                                {
                                    IdLocalidad = reader["IdLocalidad"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                lista = new List<Localidades>();
            }

            return lista;
        }
    }
}
