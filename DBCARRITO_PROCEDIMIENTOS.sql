

create proc DB_RegistrarUsuario(
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Clave varchar(150),
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as 
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo=@Correo)
	begin 
		INSERT INTO USUARIO(Nombres,Apellidos,Correo,Clave,Activo) VALUES
		(@Nombres, @Apellidos,@Correo,@Clave,@Activo)

		SET @Resultado = SCOPE_IDENTITY() /* SCOPE IDENTITY nos devuelve el ultimo id registrado en la base de datos*/
	end
	else
		set @Mensaje = 'El correo del usuario ya existe'
end

GO

CREATE PROC DB_EditarUsuario(
@IdUsuario int, 
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Activo bit,
@Mensaje varchar(500)output,
@Resultado bit output
)
AS
BEGIN
	SET @Resultado=0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE	Correo = @Correo and IdUsuario!=@IdUsuario)
	begin
		UPDATE top(1) USUARIO SET
		Nombres=@Nombres,
		Apellidos=@Apellidos,
		Correo = @Correo,
		Activo = @Activo
		WHERE IdUsuario = @IdUsuario

		SET @Resultado = 1
	end
	else
		SET @Mensaje = 'El Correo/ID del usuario ya existe'
end



CREATE PROC DB_RegistrarCategoria(
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500)output,
@Resultado int output
)
AS
BEGIN	
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion=@Descripcion)
	begin 
		INSERT INTO CATEGORIA(Descripcion,Activo) VALUES (@Descripcion, @Activo)
		SET @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		SET @Mensaje = 'La Categoria ya existe'
END


CREATE PROC DB_EditarCategoria(
@IdCategoria int,
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500)output,
@Resultado bit output
)
AS
BEGIN	
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion=@Descripcion and IdCategoria != @IdCategoria)
	begin 
		UPDATE top(1) CATEGORIA SET Descripcion=@Descripcion, Activo=@Activo WHERE IdCategoria=@IdCategoria
		SET @Resultado = 1
	end
	ELSE
		SET @Mensaje = 'La Categoria ya existe'
END

CREATE PROC DB_EliminarCategoria(
@IdCategoria int,
@Mensaje varchar(500) output,
@Resultado bit output
)
AS
BEGIN
	SET @Resultado = 0
	IF  NOT EXISTS ( SELECT * FROM PRODUCTO p INNER JOIN CATEGORIA c ON 
					c.IdCategoria = p.IdCategoria WHERE p.IdCategoria = @IdCategoria)
	begin
		DELETE TOP(1) FROM CATEGORIA WHERE IdCategoria = @IdCategoria
		SET @Resultado = 1
	end
	ELSE
		set @Mensaje = 'La categoria se encuentra relacionada a un producto '
END




CREATE PROC DB_RegistrarMarca(
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500)output,
@Resultado int output
)
AS
BEGIN	
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion=@Descripcion)
	begin 
		INSERT INTO MARCA(Descripcion,Activo) VALUES (@Descripcion, @Activo)
		SET @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		SET @Mensaje = 'La Marca ya existe'
END


CREATE PROC DB_EditarMarca(
@IdMarca int,
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500)output,
@Resultado bit output
)
AS
BEGIN	
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion=@Descripcion and IdMarca != @IdMarca)
	begin 
		UPDATE top(1) MARCA SET Descripcion=@Descripcion, Activo=@Activo WHERE IdMarca=@IdMarca
		SET @Resultado = 1
	end
	ELSE
		SET @Mensaje = 'La Categoria ya existe'
END

CREATE PROC DB_EliminarMarca(
@IdMarca int,
@Mensaje varchar(500) output,
@Resultado bit output
)
AS
BEGIN
	SET @Resultado = 0
	IF  NOT EXISTS ( SELECT * FROM PRODUCTO p INNER JOIN MARCA c ON 
					c.IdMarca = p.IdMarca WHERE p.IdMarca = @IdMarca)
	begin
		DELETE TOP(1) FROM MARCA WHERE IdMarca = @IdMarca
		SET @Resultado = 1
	end
	ELSE
		set @Mensaje = 'La Marca se encuentra relacionada a un producto '
END



CREATE PROC DB_RegistrarProducto(
@Nombre varchar(100),
@Descripcion varchar(100),
@IdMarca varchar(100),
@IdCategoria varchar(100),
@Precio decimal(10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
AS 
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS ( SELECT * FROM PRODUCTO WHERE Nombre = @Nombre)
	begin
		INSERT INTO PRODUCTO (Nombre,Descripcion,IdMarca,IdCategoria,Precio,Stock,Activo) values
		                   (@Nombre,@Descripcion,@IdMarca,@IdCategoria,@Precio,@Stock,@Activo)
		SET @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		SET @Mensaje = 'El producto ya existe'
END


CREATE PROC DB_EditarProducto(
@IdProducto int,
@Nombre varchar(100),
@Descripcion varchar(100),
@IdMarca varchar(100),
@IdCategoria varchar(100),
@Precio decimal(10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado bit output
)
AS 
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre and IdProducto!=IdProducto)
	BEGIN
		UPDATE PRODUCTO SET
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdMarca = @IdMarca,
		IdCategoria = @IdCategoria,
		Precio = @Precio,
		Stock = @Stock,
		Activo = @Activo
		WHERE IdProducto = @IdProducto

		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'El Producto ya existe'
END

CREATE PROC DB_EliminarProducto(
@IdProducto int,
@Mensaje varchar(500) output,
@Resultado bit output
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM DETALLE_VENTA dv
	inner join PRODUCTO p on p.IdProducto = dv.IdProducto
	WHERE p.IdProducto = @IdProducto)
	begin
		DELETE TOP(1) FROM PRODUCTO WHERE IdProducto = @IdProducto
		SET @Resultado = 1
	end
	ELSE
		SET @Mensaje = 'El producto se encuentra relacionado a una venta'
END

SELECT * FROM CARRITO
SELECT * FROM PRODUCTO

UPDATE PRODUCTO SET
		Stock = 100
		WHERE IdProducto = '1'

CREATE PROCEDURE DB_ListarProducto
AS
BEGIN
    SELECT  
        p.IdProducto, 
        p.Nombre, 
        p.Descripcion,
        m.IdMarca, 
        m.Descripcion AS DesMarca, -- Utiliza AS para asignar alias
        c.IdCategoria, 
        c.Descripcion AS DesCategoria, -- Utiliza AS para asignar alias
        p.Precio,
        p.Stock,
        p.RutaImagen,
        p.NombreImagen,
        p.Activo
    FROM PRODUCTO p 
    INNER JOIN MARCA m ON m.IdMarca = p.IdMarca
    INNER JOIN CATEGORIA c ON c.IdCategoria = p.IdCategoria -- Usar la columna correcta para la relación
END




CREATE PROC DB_ReporteDashboard
as 
begin
	SELECT

		(SELECT count(*) FROM CLIENTE)[TotalCliente],

		(SELECT ISNULL(sum(cantidad),0) FROM DETALLE_VENTA)[TotalVentas],

		(SELECT COUNT(*) FROM PRODUCTO)[TotalProductos]
End 
/*
EXEC DB_ReporteVentas
    @fechainicio = '12/09/2023',
    @fechafin = '12/09/2023',
    @idtransaccion = '0';
	*/

	SELECT * FROM CLIENTE

CREATE PROC DB_ReporteVentas(
 @fechainicio varchar(10),
 @fechafin varchar(10),
 @idtransaccion varchar(50)
 )
 as
 begin
	set dateformat dmy;
	SELECT CONVERT(char(10), v.FechaVenta, 103)[FechaVenta], CONCAT(c.Nombres, ' ' , c.Apellido)[Cliente],
	p.Nombre[Producto], p.Precio,dv.Cantidad,dv.Total,v.IdTransaccion
	from DETALLE_VENTA dv
	inner join PRODUCTO p on p.IdProducto = dv.IdProducto
	inner join VENTA v on v.IdVenta=dv.IdVenta
	inner join CLIENTE c on c.IdCliente = v.IdCliente
	WHERE CONVERT(date,v.FechaVenta) between @fechainicio and @fechafin
	and v.IdTransaccion=iif(@idtransaccion='', v.IdTransaccion,@idtransaccion)
end



CREATE PROC DB_RegistrarCliente(
@Nombre varchar(100),
@Apellido varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@Mensaje varchar(500) output,
@Resultado int output
)
AS
BEGIN
	 SET @Resultado = 0
	 IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Correo = @Correo)
	 begin 
		INSERT INTO CLIENTE (Nombres,Apellido,Correo,Clave,Reestablecer) VALUES
		(@Nombre,@Apellido,@Correo,@Clave,0)

		SET @Resultado = SCOPE_IDENTITY()
	 end
	 ELSE
		SET @Mensaje='El correo de usuario ya existe'
END

SELECT * fROM CLIENTE
/*

declare @idcategoria int =1
SELECT distinct m.IdMarca, m.Descripcion FROM PRODUCTO p
inner join CATEGORIA c on c.IdCategoria = p.IdCategoria
inner join MARCA m on m.IdMarca = p.IdMarca and m.Activo=1
WHERE c.IdCategoria=IIF(@idcategoria = 0,c.IdCategoria,@idcategoria)
*/

CREATE PROC DB_ExisteCarrito(
@IdCliente int,
@IdProducto int,
@Resultado bit output
)
as
begin
	if exists (SELECT * FROM CARRITO WHERE IdCliente=@IdCliente and IdProducto=@IdProducto)
		set @Resultado = 1
	else
		set @Resultado = 0
end

CREATE PROC DB_OperacionCarrito(
@IdCliente int,
@IdProducto int,
@Sumar bit,
@Mensaje varchar(500) output,
@Resultado bit output
)
as
begin
	set @Resultado=1
	set @Mensaje = ''

	declare @existecarrito bit = iif(exists(SELECT * FROM CARRITO where IdCliente=@IdCliente and IdProducto =@IdProducto),1,0)
	declare @stockproducto int = (SELECT Stock FROM PRODUCTO where IdProducto = @IdProducto)

	BEGIN TRY

		BEGIN TRANSACTION OPERACION
		
		if(@Sumar=1)
		begin
			if(@stockproducto > 0)
			begin
				if(@existecarrito=1)
					UPDATE CARRITO SET Cantidad = Cantidad+1 where IdCliente = @IdCliente and IdProducto=@IdProducto
				else
					INSERT INTO CARRITO (IdCliente,IdProducto,Cantidad) values(@IdCliente,@IdProducto,1)
				
				UPDATE PRODUCTO SET Stock=Stock-1 WHERE IdProducto = @IdProducto
			end
			else
			begin 
				set @Resultado = 0
				set @Mensaje = 'El producto no cuenta con stock disponible'
			end
		end
		else
		begin
			UPDATE CARRITO SET Cantidad = Cantidad-1 WHERE IdCliente=@IdCliente and IdProducto=@IdProducto
			UPDATE PRODUCTO SET Stock = Stock+1 WHERE IdProducto=@IdProducto
		end

		COMMIT TRANSACTION OPERACION

	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
		ROLLBACK TRANSACTION OPERACION
	END CATCH
END

drop function DB_ObtenerCarritoCliente

CREATE FUNCTION DB_ObtenerCarritoCliente(
@idcliente int
)
returns table 
as
return (
	SELECT p.IdProducto,p.Nombre,m.Descripcion[DesMarca],p.Precio,c.Cantidad,p.RutaImagen,p.NombreImagen 
	from CARRITO c 
	inner join PRODUCTO p on p.IdProducto = c.IdProducto
	inner join MARCA m on m.IdMarca = p.IdMarca
	WHERE IdCliente = @idcliente
)




CREATE PROC DB_EliminarCarrito(
@IdCliente int,
@IdProducto int,
@Resultado bit output
)
as
begin
	set @Resultado=1
	declare @cantidadproducto int = (select Cantidad from CARRITO where IdCliente = @IdCliente and IdProducto = @IdProducto)

	begin try 
		begin transaction operacion

		update PRODUCTO set Stock = Stock + @cantidadproducto where IdProducto=@IdProducto
		delete top(1) from CARRITO where IdCliente =@IdCliente and IdProducto=@IdProducto

		commit transaction operacion

	end try
	begin catch
		set @Resultado =0
		rollback transaction operacion
	end catch
end






/*Creamos una lista de todos los productos*/

CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[IdProducto] int null,
	[Cantidad] int null,
	[Total] decimal(18,2) null
)


/* Procedimiento almacenado para registrar la venta´*/

CREATE PROC DB_RegistrarVenta(
@IdCliente int,
@TotalProducto int,
@MontoTotal decimal(18,2),
@Contacto varchar(100),
@IdLocalidad varchar(6), 
@Telefono varchar (10),
@Direccion varchar(100),
@IdTransaccion varchar(50),
@DetalleVenta [EDetalle_Venta] READONLY,
@Resultado bit output,
@Mensaje varchar(500) output
)
AS
BEGIN 
	begin try
		
		declare @idventa int =0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro

		INSERT INTO VENTA(IdCliente,TotalProducto,MontoTotal,Contacto,IdDistrito,Telefono,Direccion,IdTransaccion)
		VALUES (@IdCliente,@TotalProducto,@MontoTotal,@Contacto,@IdLocalidad,@Telefono,@Direccion,@IdTransaccion)
		
		set @idventa = SCOPE_IDENTITY()

		INSERT INTO DETALLE_VENTA(IdVenta,IdProducto,Cantidad,Total)
		
		SELECT @idventa, IdProducto, Cantidad, Total FROM @DetalleVenta

		DELETE FROM CARRITO WHERE IdCliente = @IdCliente

		commit transaction registro

	end try
	begin catch 
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end




