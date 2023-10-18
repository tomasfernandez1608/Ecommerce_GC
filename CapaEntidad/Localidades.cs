using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    /*
        CREATE TABLE LOCALIDADES (
            IdLocalidad INT PRIMARY KEY,
            Descripcion VARCHAR(100) NOT NULL,
            IdProvincia INT,
            FOREIGN KEY (IdProvincia) REFERENCES Provincias(IdProvincia)
        );
     */
    public class Localidades
    {
        public string IdLocalidad { get; set; }
        public string Descripcion { get; set; }
        public string IdProvincia { get; set; }

    }
}
