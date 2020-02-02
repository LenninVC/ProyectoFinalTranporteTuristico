USE master
GO

CREATE DATABASE [TransportesDB]
GO

USE [TransportesDB]
GO

CREATE TABLE Colaborador
(
	IdColaborador int identity(1,1) primary key,
	Documento varchar(25) NOT NULL,
	Nombres varchar (150) NOT NULL,
	Apellidos varchar (200) NOT NULL,
	Direccion varchar (200) NULL,
	Telefono varchar (15) NOT NULL,
	Celular varchar (15) NOT NULL,
	EmailPersonal varchar (200) NOT NULL,
	Estado bit NOT NULL
)	
GO

CREATE TABLE Cliente
( 
	IdCliente	int identity (1,1) primary key,
	Documento   varchar(20)  NOT NULL ,
	Nombres		varchar (150) NOT NULL,
	Apellidos	varchar (200) NOT NULL,
	Direccion	varchar (200) NULL,
	Telefono    varchar(15)  NULL ,
	Celular     varchar(15)  NULL ,
	Email       varchar(50)  NOT NULL ,
	Estado      bit NOT NULL
)
go

CREATE TABLE Comentarios
( 
	IdComentario         int identity (1,1) primary key ,
	Fecha                datetime,
	Comentario           varchar(255)  NULL ,
	IdReserva			 int,
	Estado               bit NULL 
)
go

CREATE TABLE Conductor
( 
	IdConductor          int identity (1,1) primary key ,
	IdVehiculo           int,
	Nombres 	         varchar(150) NULL,
	Apellidos 			 varchar(150) NULL,
	Licencia             varchar(50),
	Estado               bit	
	
)
go

CREATE TABLE Reserva
( 
	IdReserva			 int identity (1,1) primary key ,
	Fecha                datetime,
	Estado               bit,
	Numero               varchar(18) ,
	Costo_Total          decimal(8,2),
	IdCliente            int
)
go

CREATE TABLE Detalle_Reserva
( 
	IdReserva			 int,
	Secuencial			 int,
	IdItinerario         int,
	Fecha_Inicio         datetime,
	Fecha_Fin            datetime,
	Estado               bit,
	Origen               varchar(255)  NULL ,
	Destino              varchar(255)  NULL ,
	Costo                decimal(8,2)
)
go


CREATE TABLE Itinerario
( 
	IdItinerario         int identity (1,1) primary key ,
	IdConductor			 int NOT NULL,
	Descripcion          varchar(255)  NULL ,
	Origen               varchar(255)  NULL ,
	Destino              varchar(255)  NULL ,
	Costo                decimal(8,2),
	Estado               bit 
)
go

CREATE TABLE Usuario
( 
	IdUsuario            int identity (1,1) primary key ,
	Email                varchar(50)  NOT NULL ,
	[Password]           varbinary(max),
	Estado               bit,
	Foto                 varchar(255),
	idColaborador		 int NOT NULL
)
go


CREATE TABLE Vehiculo
( 
	IdVehiculo           int identity (1,1) primary key ,
	Placa                varchar(255)  NULL ,
	Modelo               varchar(255)  NULL ,
	Capacidad            int,
	Estado               bit,
	Marca                varchar(255)  NULL 
)
go


ALTER TABLE Usuario
	ADD CONSTRAINT R_9 FOREIGN KEY (IdColaborador) REFERENCES Colaborador(IdColaborador)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE Comentarios
	ADD CONSTRAINT R_4 FOREIGN KEY (IdReserva) REFERENCES Reserva(IdReserva)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE Conductor
	ADD CONSTRAINT R_5 FOREIGN KEY (IdVehiculo) REFERENCES Vehiculo(IdVehiculo)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go




ALTER TABLE Reserva
	ADD CONSTRAINT R_8 FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Detalle_Reserva
	ADD CONSTRAINT R_6 FOREIGN KEY (IdItinerario) REFERENCES Itinerario(IdItinerario)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Detalle_Reserva
	ADD CONSTRAINT R_1 FOREIGN KEY (IdReserva) REFERENCES Reserva(IdReserva)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Itinerario
	ADD CONSTRAINT R_7 FOREIGN KEY (IdConductor) REFERENCES Conductor(IdConductor)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go
