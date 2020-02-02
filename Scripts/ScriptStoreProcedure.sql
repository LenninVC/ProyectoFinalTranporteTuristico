USE [TransportesDB]
GO
CREATE PROCEDURE [dbo].[uspClientePagedList]
	@startRow int,
	@endRow int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH ClienteResult AS 
	(
		SELECT 
		   [IdCliente]
		  ,[Documento]
		  ,[Nombres]
		  ,[Apellidos]
		  ,[Direccion]
		  ,[Telefono]
		  ,[Celular]
		  ,[Email]
		  ,[Estado]
		  ,ROW_NUMBER() OVER (ORDER BY IdCliente) AS RowNum
	  FROM [TransportesDB].[dbo].[Cliente]
	)
	SELECT  
		   [IdCliente]
		  ,[Documento]
		  ,[Nombres]
		  ,[Apellidos]
		  ,[Direccion]
		  ,[Telefono]
		  ,[Celular]
		  ,[Email]
		  ,[Estado]
	FROM ClienteResult
	WHERE Rownum BETWEEN @startRow AND @endRow
END
GO
CREATE PROCEDURE [dbo].[uspColaboradorPagedList]
	@startRow int,
	@endRow int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH ColaboradorResult AS 
	(
		SELECT 
		   [IdColaborador]
		  ,[Documento]
		  ,[Nombres]
		  ,[Apellidos]
		  ,[Direccion]
		  ,[Telefono]
		  ,[Celular]
		  ,[EmailPersonal]
		  ,[Estado]
		  ,ROW_NUMBER() OVER (ORDER BY IdColaborador) AS RowNum
	  FROM [TransportesDB].[dbo].[Colaborador]
	)
	SELECT  
		   [IdColaborador]
		  ,[Documento]
		  ,[Nombres]
		  ,[Apellidos]
		  ,[Direccion]
		  ,[Telefono]
		  ,[Celular]
		  ,[EmailPersonal]
		  ,[Estado]
	FROM ColaboradorResult
	WHERE Rownum BETWEEN @startRow AND @endRow
END
GO
CREATE PROC [dbo].[uspGeListItinerarios]
AS
BEGIN
	SELECT 
	I.IdItinerario
	, I.IdConductor
	, C.Nombres + ' ' + C.Apellidos Conductor
	, C.Licencia
	, I.Descripcion
	, I.Origen
	, I.Destino
	, I.Costo
	, I.Estado FROM dbo.Itinerario I
	INNER JOIN dbo.Conductor c ON I.IdConductor = C.IdConductor
END
GO
CREATE PROCEDURE [dbo].[uspItinerarioPagedList]
	@startRow int,
	@endRow int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH ItinerarioResult AS 
	(

		SELECT 
		I.IdItinerario
		, I.IdConductor
		, C.Nombres + ' ' + C.Apellidos Conductor
		, C.Licencia
		, I.Descripcion
		, I.Origen
		, I.Destino
		, I.Costo
		, I.Estado
		,ROW_NUMBER() OVER (ORDER BY IdItinerario) AS RowNum 
		FROM Itinerario I
		INNER JOIN Conductor c ON I.IdConductor = C.IdConductor
	)
	SELECT 
	 [IdItinerario]
	, [IdConductor]
	, [Conductor]
	, [Licencia]
	, [Descripcion]
	, [Origen]
	, [Destino]
	, [Costo]
	, [Estado]
	FROM ItinerarioResult
	WHERE Rownum BETWEEN @startRow AND @endRow
END
GO
CREATE PROCEDURE dbo.uspCreateUser  
(  
 @email varchar(250),  
 @password varchar(250),  
 @idColaborador int  
)  
AS  
BEGIN TRY  
 BEGIN TRAN  
  INSERT INTO Usuario(Email,[Password],idColaborador)  
  VALUES(@email,PWDENCRYPT(@password),@idColaborador)  
 COMMIT TRAN  
  
 SELECT  
  Email,  
  Nombres,  
  Apellidos,  
  Colaborador.IdColaborador   
 FROM Usuario  INNER JOIN Colaborador ON Usuario.IdUsuario=Colaborador.IdColaborador  
 WHERE Email=@email AND PWDCOMPARE(@password,[Password])=1  
  
END TRY  
BEGIN CATCH  
 ROLLBACK TRAN  
END CATCH
GO
CREATE PROCEDURE dbo.uspValidateUser  
(  
 @email varchar(250),  
 @password varchar(250)  
)  
AS  
BEGIN  
 SELECT  
  Email,  
  Nombres,  
  Apellidos 
 FROM Usuario INNER JOIN Colaborador ON Usuario.IdUsuario=Colaborador.IdColaborador  
 WHERE Email=@email AND PWDCOMPARE(@password,[Password])=1  
END  

GO

CREATE PROCEDURE [dbo].[uspReservaPagedList]
	@startRow int,
	@endRow int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH Reser
vaResult AS 
	(
		SELECT 
		   r.IdReserva
		  ,Fecha_Inicio
		  ,Fecha_Fin
		  ,i.Descripcion as 'Itinerario'
		  ,dr.Costo
		  ,cl.Nombres 
		  ,cl.Apellidos 
		  ,ROW_NUMBER() OVER (ORDER BY dr.IdReserva) AS RowNum
	  FROM [TransportesDB].[dbo].[Detall
e_Reserva] dr 
			inner join Reserva r on dr.IdReserva=r.IdReserva
			inner join Itinerario i on i.IdItinerario=dr.IdItinerario
			inner join Cliente cl on cl.IdCliente=r.IdCliente
			WHERE R.Estado=1
	)
	SELECT  
		    IdReserva
		  ,Fecha_Inicio
		  ,Fe
cha_Fin
		  ,Itinerario
		  ,Costo
		  ,Nombres
		  ,Apellidos
	FROM ReservaResult
	WHERE Rownum BETWEEN @startRow AND @endRow
END



GO

create PROCEDURE uspClienteReserva      
(      
   
@Fecha_Inicio date,    
@Fecha_Fin date,    
@IdItinerario int,    
@Costo decimal(12,2),    
@IdCliente int    
)      
as      
    
declare @idReserva int   
declare @origen varchar(50)
declare @destino varchar(50)
 
   
    

   

select top 1 @origen=Origen,@destino=Destino  from Itinerario where IdItinerario=  @IdItinerario

insert into Reserva(IdCliente, Estado) values(@IdCliente,1)  

set @idReserva=    
(    
select top 1 IdReserva    
  from Reserva    
 order by IdReserva desc)    


insert into Detalle_Reserva(IdReserva,Destino, Origen, Fecha_Inicio, Fecha_Fin, IdItinerario, Costo, Estado)     
  values(@idReserva,@Destino,@Origen,@Fecha_Inicio,@Fecha_Fin,@IdItinerario,@Costo,1)    


  GO

create PROCEDURE uspSearchReservaById        
( @Id INT )        
as        
      
  SELECT    
 TOP 1     
 cl.Nombres,  
 cl.Apellidos,  
 Fecha_Inicio,   
 Fecha_Fin,
 r.IdReserva,
 R.IdCliente,
 dr.Costo
 FROM   
 Reserva R INNER JOIN Detalle_Reserva DR ON R.IdReserva=DR.IdReserva    
 INNER JOIN Cliente cl on cl.IdCliente=r.IdCliente  
 WHERE DR.IdReserva = @Id


 GO

 CREATE PROCEDURE uspClienteUpdate         
(          
       
@Fecha_Inicio date,        
@Fecha_Fin date,        
@IdItinerario int,           
@IdCliente int,    
@NombreCliente varchar(100),    
@ApellidoCliente varchar(100),    
@IdReserva int        
)          
as          
        
declare @Costo decimal       
declare @origen varchar(50)    
declare @destino varchar(50)    
     
    
    
 select     
  top 1     
   @origen=Origen,    
   @destino=Destino,     
   @Costo=Costo      
   from Itinerario     
   where IdItinerario=  @IdItinerario    
    
    
   update Detalle_Reserva    
 set Fecha_Inicio=@Fecha_Inicio,    
  Fecha_Fin=@Fecha_Fin,    
  Origen=@origen,    
  Destino=@destino,    
  Costo=@Costo    
  where IdReserva=@IdReserva    
        
          
    update Cliente     
  set Nombres=@NombreCliente,    
   Apellidos=@ApellidoCliente    
   where IdCliente=@IdCliente 


   GO

CREATE PROCEDURE uspClienteDelete       
(   @IdReserva int    )        
as        
      
	
 update Reserva	
  set Estado=0
	where IdReserva=@IdReserva
	update Detalle_Reserva
	
	  
 set Estado=0
  where IdReserva=@IdReserva  

