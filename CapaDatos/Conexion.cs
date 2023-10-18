using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace CapaDato
{
    public class Conexion
    {

        private string cadenaSQL = string.Empty;

        public Conexion()
        {
            //cadenaSQL = "Data Source=DESKTOP-IBGN7G2; Initial Catalog=DBCARRITO; Integrated Security=true";
            cadenaSQL = "Data Source=GCecommerce.mssql.somee.com; Initial Catalog=GCecommerce;;user id=tomasfernandez22_SQLLogin_2;pwd=i4rjshit4s;";

        }
        public string getCadenaSQL()//Este metodo el string que contiene la conexion obtenida
        {
            return cadenaSQL;
        }
    }
}
