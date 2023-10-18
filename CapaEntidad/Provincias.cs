using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    /* 
     CREATE TABLE PROVINCIAS (
        IdProvincia INT PRIMARY KEY,
        Descripcion VARCHAR(100) NOT NULL
    );
     */
    public class Provincias
    {
        public string IdProvincia { get; set; }
        public string Descripcion { get; set; }

    }
}
