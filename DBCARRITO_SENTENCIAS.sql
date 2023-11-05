

SELECT * FROM USUARIO

INSERT INTO USUARIO(Nombres,Apellidos,Correo,Clave) values(
'Tomas', 'Fernandez', 'tomasfernandez1608@hotmail.com','ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae')


SELECT * FROM CATEGORIA

INSERT INTO CATEGORIA(Descripcion) values
('Tecnologia'),
('Muebles'),
('Deportes')

SELECT * FROM MARCA 

INSERT INTO MARCA(idMarca,Descripcion) VALUES 
('LG'),
('HyperX'),
('EscritorioGAMERX'),
('INTEL'),
('NIKE'),
('AMD')

DROP TABLE MARCA

DELETE FROM MARCA
WHERE IdMarca >= 1 AND IdMarca <= 18;

SELECT * FROM PROVINCIAS;

INSERT INTO PROVINCIAS (IdProvincia, Descripcion) VALUES
(1, 'Buenos Aires'),
(2, 'Córdoba'),
(3, 'Santa Fe'),
(4, 'Mendoza'),
(5, 'Tucumán'),
(6, 'Entre Ríos'),
(7, 'Salta'),
(8, 'Misiones'),
(9, 'Chaco'),
(10, 'Corrientes'),
(11, 'San Juan'),
(12, 'San Luis'),
(13, 'La Pampa'),
(14, 'Formosa'),
(15, 'Río Negro'),
(16, 'Neuquén'),
(17, 'Catamarca'),
(18, 'La Rioja'),
(19, 'Santiago del Estero'),
(20, 'Jujuy'),
(21, 'Santa Cruz'),
(22, 'Tierra del Fuego'),
(23, 'Ciudad Autónoma de Buenos Aires');

SELECT * FROM LOCALIDADES

INSERT INTO LOCALIDADES(IdLocalidad, Descripcion, IdProvincia) VALUES
(101, 'La Plata', 1),
(102, 'Mar del Plata', 1),
(103, 'Quilmes', 1),
(104, 'Bahía Blanca', 1),
(105, 'Lomas de Zamora', 1),
(106, 'Avellaneda', 1),
(107, 'San Isidro', 1),
(108, 'Tigre', 1),
(109, 'San Nicolás', 1),
(110, 'Tandil', 1)

-- Insertar al menos una localidad para cada provincia
-- Buenos Aires
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (101, 'La Plata', 1);

-- Córdoba
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (201, 'Córdoba', 2);

-- Santa Fe
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (301, 'Rosario', 3);

-- Mendoza
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (401, 'Mendoza', 4);

-- Tucumán
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (501, 'San Miguel de Tucumán', 5);

-- Entre Ríos
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (601, 'Paraná', 6);

-- Salta
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (701, 'Salta', 7);

-- Misiones
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (801, 'Posadas', 8);

-- Chaco
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (901, 'Resistencia', 9);

-- Corrientes
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1001, 'Corrientes', 10);

-- San Juan
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1101, 'San Juan', 11);

-- San Luis
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1201, 'San Luis', 12);

-- La Pampa
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1301, 'Santa Rosa', 13);

-- Formosa
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1401, 'Formosa', 14);

-- Río Negro
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1501, 'Viedma', 15);

-- Neuquén
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1601, 'Neuquén', 16);

-- Catamarca
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1701, 'San Fernando del Valle de Catamarca', 17);

-- La Rioja
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1801, 'La Rioja', 18);

-- Santiago del Estero
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (1901, 'Santiago del Estero', 19);

-- Jujuy
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (2001, 'San Salvador de Jujuy', 20);

-- Santa Cruz
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (2101, 'Río Gallegos', 21);

-- Tierra del Fuego
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (2201, 'Ushuaia', 22);

-- Ciudad Autónoma de Buenos Aires
INSERT INTO Localidades (IdLocalidad, Descripcion, IdProvincia) VALUES (2301, 'Ciudad Autónoma de Buenos Aires', 23);
