/*USE MASTER
GO

IF EXISTS(SELECT * FROM SYSDATABASES WHERE NAME = 'BiosMoney')
BEGIN
	DROP DATABASE BiosMoney
END
GO

CREATE DATABASE BiosMoney ON(
	NAME = BiosMoney,
	FILENAME = 'D:\ProyectoBios\Sistema\BiosMoney.mdf'
)
GO

USE BiosMoney
GO*/

CREATE TABLE Usuarios (
Ci BIGINT PRIMARY KEY NOT NULL,
Usuario VARCHAR(30) UNIQUE NOT NULL,
Clave VARCHAR(7) NOT NULL,
NomCompleto VARCHAR(50) NOT NULL
)
go

CREATE TABLE Gerentes (
Ci BIGINT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Usuarios(Ci),
Email VARCHAR(50) NOT NULL
)
go

CREATE TABLE Cajeros (
Ci BIGINT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Usuarios(Ci),
HoraInicio VARCHAR(4) NOT NULL,
HoraFin VARCHAR(4) NOT NULL,
Baja BIT
)
go



CREATE TABLE Empresas (
Codigo INT CHECK (Codigo BETWEEN 1000 AND 9999) PRIMARY KEY NOT NULL,
Rut BIGINT CHECK (Rut BETWEEN 0 AND 999999999999) NOT NULL,
Nombre VARCHAR(100),
DirFiscal VARCHAR(100) NOT NULL,
Tel INT NOT NULL,
Baja BIT
)
go

CREATE TABLE Contratos(
CodEmpresa INT CHECK(CodEmpresa BETWEEN 1000 AND 9999) FOREIGN KEY REFERENCES Empresas(Codigo) NOT NULL,
CodTipo INT CHECK(CodTipo BETWEEN 10 AND 99) NOT NULL,
Nombre VARCHAR(100) NOT NULL,
Baja BIT,
PRIMARY KEY (CodEmpresa, CodTipo)
)
go

CREATE TABLE Pagos(
NumInterno INT IDENTITY (10000,1) PRIMARY KEY NOT NULL,
Fecha DATETIME DEFAULT GETDATE() NOT NULL,
Monto INT CHECK(Monto<99999) NOT NULL,
Cajero BIGINT FOREIGN KEY REFERENCES Usuarios(CI)
)
go

CREATE TABLE FacturasPagos(
NumInterno INT NOT NULL FOREIGN KEY REFERENCES Pagos(NumInterno),
CodigoEmpresa INT NOT NULL, 
TipoContrato INT NOT NULL,
CodCliente INT CHECK(CodCliente BETWEEN 100000 AND 999999) NOT NULL,
FechaVencimiento DATE NOT NULL,
Monto INT CHECK(Monto<99999) NOT NULL,
FOREIGN KEY (CodigoEmpresa, TipoContrato) REFERENCES Contratos(CodEmpresa, CodTipo),
PRIMARY KEY(NumInterno, CodigoEmpresa, TipoContrato) 
)
go

CREATE PROCEDURE Logueo(@Usuario VARCHAR(30), @Clave VARCHAR(7)) AS
BEGIN
	IF exists(SELECT *
			FROM Usuarios U
			WHERE U.Usuario = @Usuario and U.Clave = @Clave)
	BEGIN
		DECLARE @cedula int;
		SELECT @cedula = U.Ci FROM Usuarios U WHERE U.Usuario = @Usuario;
		IF exists (SELECT * FROM Cajeros C WHERE C.Ci = @cedula)
			BEGIN

				return 1;
			END
		ELSE IF exists (SELECT * FROM Gerentes G WHERE G.Ci = @cedula)
		BEGIN
			return 2;
		END
	END
END
GO

CREATE PROCEDURE BuscarCajeroLogueo(@Usuario VARCHAR(30)) AS
BEGIN
	SELECT *
	FROM Usuarios U INNER JOIN Cajeros C
	ON U.Usuario = @Usuario and C.Ci= U.Ci and C.Baja = 0
END
GO

CREATE PROCEDURE BuscarGerenteLogueo(@Usuario VARCHAR(30)) AS
BEGIN
	SELECT *
	FROM Usuarios U INNER JOIN Gerentes G
	ON U.Usuario = @Usuario and G.Ci= U.Ci
END
GO

--ACA ESTA LA CREACION DE USUARIO CON DB
CREATE PROCEDURE AltaGerente (@Usuario VARCHAR(30), @Clave VARCHAR(7), @Ci INT, @NomCompleto VARCHAR(50), @Email VARCHAR(50)) AS
BEGIN
	if exists(select * from Usuarios U where U.Ci = @Ci)
	begin
		return -1;
	end
	IF NOT(EXISTS(SELECT *
					FROM Usuarios U
					WHERE U.Usuario = @Usuario))
	BEGIN
		BEGIN TRANSACTION;
		INSERT 
		INTO Usuarios 
		VALUES(@Ci, @Usuario, @Clave, @NomCompleto)
		IF(@@ERROR = 0)
			BEGIN
				INSERT 
				INTO Gerentes
				VALUES(@Ci, @Email)
				IF(@@ERROR = 0)
				BEGIN
					COMMIT TRANSACTION

					--primero creo el usuario de logueo
					Declare @VarSentencia varchar(200)
					Set @VarSentencia = 'CREATE LOGIN [' +  'pepito' + '] WITH PASSWORD = ' + QUOTENAME('asd', '''')
					Exec (@VarSentencia)
	
					if (@@ERROR <> 0)
						return -1
		
	
					--segundo asigno rol especificao al usuario recien creado
					Exec sp_addsrvrolemember @loginame='pepito', @rolename=sysadmin --ESTO ES LO QUE NO FUNCA CON NUESTRO ROL GERENTE
	
					if (@@ERROR = 0)
					begin
						Declare @VarSentencia2 varchar(200)
						Set @VarSentencia2 = 'Create User [' +  'pepito' + '] From Login [' + 'pepito' + ']'
						Exec (@VarSentencia2)
	
						if (@@ERROR <> 0)
							return -1
						RETURN 1;
					end
					else
						return -2
				END
				ELSE
				BEGIN
					ROLLBACK TRANSACTION;
					RETURN -2;
				END
			END
		ELSE
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -2;
		END
	END
	ELSE
		RETURN -1;
END
GO

CREATE PROCEDURE AltaCajero (@Usuario VARCHAR(30), @Clave VARCHAR(7), @CI INT, 
@NomCompleto VARCHAR(50), @HoraInicio VARCHAR(4), @HoraFin VARCHAR(4)) AS
BEGIN
	BEGIN TRANSACTION;
	IF EXISTS(SELECT * FROM Usuarios U WHERE U.Ci = @CI)
	BEGIN
		IF EXISTS(SELECT * FROM Usuarios U LEFT JOIN Cajeros C ON U.Ci = C.Ci WHERE C.Baja = 1 AND U.Usuario = @Usuario)
		BEGIN
			--Si existe el cajero con esa cedula y está dado de baja lo doy de alta.
			UPDATE Usuarios SET Clave = @Clave, NomCompleto = @NomCompleto WHERE Ci = @CI
			IF(@@ERROR = 0)
			BEGIN
				UPDATE Cajeros SET HoraInicio = @HoraInicio, HoraFin = @HoraFin, Baja = 0 WHERE Ci = @CI
				IF(@@ERROR = 0)
				BEGIN
					COMMIT TRANSACTION;
					RETURN 1;
				END
			END
			ELSE
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -3;
			END
		END
		ELSE
			ROLLBACK TRANSACTION;
			RETURN -2;
	END
	ELSE
	BEGIN
		--Si no existe el usuario con esa cedula lo agrego.
		IF(EXISTS(SELECT * FROM Usuarios U WHERE U.Usuario = @Usuario))
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -1;
		END
		ELSE
		BEGIN
			INSERT Usuarios VALUES(@CI,@Usuario,@Clave,@NomCompleto)
			IF(@@ERROR = 0)
			BEGIN
				INSERT Cajeros VALUES(@CI, @HoraInicio, @HoraFin, 0)
				IF(@@ERROR = 0)
				BEGIN
					COMMIT TRANSACTION;
					RETURN 1;
				END
				ELSE
				BEGIN
					ROLLBACK TRANSACTION;
					RETURN -3;
				END
			END
			ELSE
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -3;
			END
		END
	END
END
GO

CREATE PROCEDURE ModCajero (@Ci INT, @NomCompleto VARCHAR(7), @HoraInicio INT, @HoraFin INT) AS
BEGIN
	IF NOT(EXISTS(SELECT *
					FROM Cajeros C
					WHERE C.Ci = @Ci))
		RETURN -1;
	IF EXISTS(SELECT *
					FROM Cajeros C
					WHERE C.Ci = @Ci and C.Baja = 1)
		RETURN -1;
	BEGIN TRANSACTION;
	UPDATE Usuarios
	SET NomCompleto = @NomCompleto 
	WHERE Ci = @Ci
	IF(@@ERROR = 0)
	BEGIN
		UPDATE Cajeros
		SET HoraInicio = @HoraInicio, HoraFin = @HoraFin 
		WHERE Ci = @Ci
		IF(@@ERROR = 0)
		BEGIN
			COMMIT TRANSACTION;
			RETURN 1;
		END
		ELSE
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -2;
		END
	END
	ELSE
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -2;
		END
END
GO

CREATE PROCEDURE BajaCajero(@Ci INT) AS
BEGIN
	IF NOT(EXISTS(SELECT *
					FROM Cajeros C
					WHERE C.Ci = @Ci))
		RETURN -1;
	ELSE IF EXISTS(SELECT *
					FROM Cajeros C
					WHERE C.Ci = @Ci AND C.Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Cajeros
		SET Baja = 1 WHERE Ci = @Ci
		IF(@@ERROR <> 0)
			RETURN -2;
	END
END
GO

CREATE PROCEDURE BuscarCajero(@Ci INT) AS
BEGIN
	SELECT *
	FROM Usuarios U INNER JOIN Cajeros C
	ON U.Ci = @Ci and C.Ci= U.Ci and C.Baja = 0
END
GO

CREATE PROCEDURE ListarCajero AS
BEGIN
	SELECT * FROM Cajeros inner join Usuarios ON Cajeros.Ci = Usuarios.Ci WHERE Cajeros.Baja = 0
END
GO

CREATE PROCEDURE ModClave (@Ci VARCHAR(30), @Clave VARCHAR(7)) AS
BEGIN
	IF NOT (EXISTS (SELECT *
					FROM Usuarios U
					WHERE U.Ci = @Ci))
		RETURN -1;
	IF EXISTS (SELECT *
				FROM Cajeros C
				WHERE C.Baja = 1)
		RETURN -1;
	BEGIN
		UPDATE Usuarios
		SET Clave = @Clave
		WHERE Ci = @Ci
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
END
GO

CREATE PROCEDURE AltaEmpresa (@Codigo INT, @Rut BIGINT, @DirFiscal VARCHAR(100),@Nombre VARCHAR(100), @Tel BIGINT) AS
BEGIN
	IF EXISTS(SELECT *
				FROM Empresas E
				WHERE E.Rut = @Rut AND E.Baja = 0)
		RETURN -1;
	IF EXISTS(SELECT *
				FROM Empresas E
				WHERE E.Rut = @Rut AND E.Baja = 1)
	BEGIN
		UPDATE Empresas
		SET Baja = 0, DirFiscal = @DirFiscal, Nombre = @Nombre, Tel = @Tel, Rut = @Rut
		WHERE Codigo = @Codigo
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
	ELSE
	BEGIN
		INSERT Empresas
		VALUES (@Codigo, @Rut, @DirFiscal, @Nombre,@Tel,0)
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
END
GO

CREATE PROCEDURE ModEmpresa (@Codigo INT, @Rut BIGINT, @DirFiscarl VARCHAR(100), @Tel BIGINT) AS
BEGIN
	IF NOT (EXISTS(SELECT *
					FROM Empresas E
					WHERE E.Rut = @Rut))
		RETURN -1;
	IF EXISTS(SELECT*
				FROM Empresas E
				WHERE E.Rut = @Rut AND E.Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Empresas
		SET DirFiscal = @DirFiscarl, Tel = @Tel, Rut = @Rut
		WHERE Codigo = @Codigo
	END
END
GO

CREATE PROCEDURE BajaEmpresa (@Codigo INT) AS
BEGIN
	IF NOT (EXISTS(SELECT *
					FROM Empresas E
					WHERE E.Codigo = @Codigo))
		RETURN -1;
	IF EXISTS(SELECT *
				FROM Empresas E
				WHERE E.Codigo = @Codigo AND E.Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Empresas
		SET Baja = 1
		WHERE Codigo = @Codigo
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
END
GO

CREATE PROCEDURE BuscarEmpresa (@Codigo INT) AS
BEGIN
	SELECT * 
	FROM Empresas E 
	WHERE E.Codigo = @Codigo AND E.Baja = 0
END
GO

CREATE PROCEDURE ListarEmpresas AS
BEGIN
	SELECT *
	FROM Empresas E
	WHERE E.Baja = 0
END
GO

CREATE PROCEDURE AltaContrato (@CodEmpresa INT, @CodTipo INT, @Nombre VARCHAR(100)) AS
BEGIN
	IF EXISTS(SELECT *
					FROM Contratos C
					WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo)
		RETURN -1;
	IF EXISTS(SELECT *
					FROM Contratos C
					WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND Baja = 1)
	BEGIN
		UPDATE Contratos
		SET Baja = 0, Nombre = @Nombre
		WHERE CodEmpresa = @CodEmpresa AND CodTipo = @CodTipo
	END
	ELSE
	BEGIN
		INSERT Contratos
		VALUES(@CodEmpresa,@CodTipo,@Nombre,0)
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
END
GO

CREATE PROCEDURE ModContrato (@CodEmpresa INT, @CodTipo INT, @Nombre VARCHAR(100)) AS
BEGIN
	IF NOT EXISTS(SELECT *
					FROM Contratos C
					WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo)
		RETURN -1;
	IF EXISTS(SELECT *
					FROM Contratos C
					WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Contratos
		SET Nombre = @Nombre
		WHERE CodEmpresa = @CodEmpresa AND CodTipo = @CodTipo
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
END
GO

CREATE PROCEDURE BajaContrato (@CodEmpresa INT, @CodTipo INT) AS
BEGIN
	IF NOT EXISTS(SELECT *
					FROM Contratos C
					WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo)
		RETURN -1;
	IF EXISTS(SELECT *
					FROM Contratos C
					WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND Baja = 0)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Contratos
		SET Baja = 1
		WHERE CodEmpresa = @CodEmpresa AND CodTipo = @CodTipo
		IF(@@ERROR = 0)
			RETURN 1;
		ELSE
			RETURN -2;
	END
END
GO

CREATE PROCEDURE BuscarContrato (@CodEmpresa INT, @CodTipo INT) AS
BEGIN
	SELECT C.*, E.Rut
	FROM Contratos C INNER JOIN Empresas E
	ON C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND C.CodEmpresa = E.Codigo AND C.Baja = 0
END
GO

CREATE PROCEDURE ListarContrato (@CodEmpresa INT) AS
BEGIN
	SELECT C.*, E.Rut
	FROM Contratos C INNER JOIN Empresas E
	ON C.CodEmpresa = @CodEmpresa AND C.CodEmpresa = E.Codigo
END
GO

CREATE PROCEDURE ListarTodosLosContratos AS
BEGIN
	SELECT * FROM Contratos
END
GO

CREATE PROCEDURE ChequearFacturaPaga (@CodigoEmpresa INT, @TipoContrato INT, @FechaVencimiento DATE, @CodigoCliente INT, @Monto INT)AS
BEGIN
	SELECT p.Fecha FROM Pagos P INNER JOIN FacturasPagos F 	
	ON P.NumInterno = F.NumInterno
	WHERE F.CodigoEmpresa = @CodigoEmpresa and F.TipoContrato = @TipoContrato and F.FechaVencimiento = @FechaVencimiento and F.CodCliente = @CodigoCliente and F.Monto = @Monto 
END
GO

CREATE PROCEDURE AltaPago(@Fecha DATETIME, @Monto INT, @Cajero INT) AS
BEGIN
	IF EXISTS(SELECT * FROM Cajeros WHERE Ci = @Cajero)
		BEGIN
			INSERT INTO Pagos VALUES(@Fecha,@Monto,@Cajero)
			IF(@@ERROR = 0)
			RETURN 0
			ELSE
			RETURN -1
		END
	ELSE
		BEGIN
			RETURN -2
		END
END
GO

CREATE PROCEDURE RegistrarFacturaEnPago(@CodigoEmpresa INT, @TipoContrato INT, @CodCliente INT, @FechaVencimiento DATE, @Monto INT) AS
BEGIN
	IF EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodigoEmpresa AND C.CodTipo = @TipoContrato)
	AND NOT EXISTS(SELECT * FROM FacturasPagos FP WHERE FP.NumInterno = IDENT_CURRENT('Pagos') AND FP.CodigoEmpresa = @CodigoEmpresa AND FP.TipoContrato = @TipoContrato)
	BEGIN
		INSERT INTO FacturasPagos VALUES(IDENT_CURRENT('Pagos'), @CodigoEmpresa, @TipoContrato, @CodCliente, @FechaVencimiento, @Monto)
		RETURN 0
		IF(@@ERROR = 0)
		RETURN 0
		ELSE
		RETURN -1
	END
	ELSE
	BEGIN
	RETURN -2
	END
	
		
END
GO

CREATE PROCEDURE ListarPagos AS
BEGIN
	SELECT * FROM Pagos		
END
GO

CREATE PROCEDURE ListarFacturas(@NumeroInterno INT) AS
BEGIN
SELECT * FROM FacturasPagos WHERE NumInterno = @NumeroInterno
END
GO

-- EL NUMERO INTERNO NO SE AUTOGENEREA HASTA QUE SE DA EL COMMIT, ENTONCES, COMO INGRESO EL NUMERO INTERNO EN AMBAS
-- TABLAS SIN TENER QUE HACER PROCEDIMIENTOS SEPARADOS?

/*CREATE PROCEDURE AltaPago(@Fecha DATETIME, @Monto INT) AS
BEGIN
	BEGIN TRANSACTION
	INSERT Pagos
	VALUES(@Fecha,@Monto)
	IF(@@ERROR = 0)
		DECLARE @INT INT;
		SET @INT = FacturaPagos.NumInterno;
		INSERT FacturasPagos
		VALUES (,)
	ELSE
	BEGIN
		ROLLBACK TRANSACTION
		RETURN -1;
	END
END
GO
*/

--LISTADO DE PAGOS

--Mejor opcion para relacionar el cajero con el pago?
--Como relaciono la empresa con la factura? substring del codigo de barras para obtener el numInterno de la empresa?

--CONSULTA EMPRESAS

--Como muestro la empresa? Es un formulario publico, no tengo ningun nombre para la empresa, solo Rut, codInterno, su dirFiscal y tel

insert into Usuarios values(48328032, 'juna', 'asd', 'Diego Furtado')
insert into Cajeros values (48328032, 1230, 1600, 0)
insert into Usuarios values(48328031, 'juna2', 'asd', 'Diego Furtado2')
insert into Cajeros values (48328031, 1230, 1600, 0)

insert into Usuarios values(51515060, 'mateo', 'qwe', 'Mateo Mendaro')
insert into Gerentes values (51515060, 'mateomendaro@gmail.com')



insert into Empresas values (1123, 999999999991, 'Abitab', 'San Fructuoso 864', 29020465, 0)
insert into Empresas values (1124, 899999999992, 'OSE', 'Ejido 928', 24005623, 0)
insert into Empresas values (1125, 899999999993, 'Antel', 'Isla de Flores 335', 27170527, 0)

insert into Contratos values (1123, 25, 'Entrada Futbol', 0)
insert into Contratos values (1123, 28, 'Entrada Musica', 0)
insert into Contratos values (1124, 28, 'Factura OSE', 0)

/*
insert into Pagos values(GETDATE(),23300,48328031)
insert into FacturasPagos values(IDENT_CURRENT('Pagos'),1123, 25, 567891, '25/03/2018', 10000)
insert into FacturasPagos values(IDENT_CURRENT('Pagos'),1123, 28, 567891, '15/03/2018', 13300)
*/


/*
insert into Pagos values(GETDATE(), 11300,48328032)
insert into FacturasPagos values(IDENT_CURRENT('Pagos'),1123, 25, 567891, '25/03/2018', 1000)
insert into FacturasPagos values(IDENT_CURRENT('Pagos'),1123, 28, 567891, '15/03/2018', 10300)
*/


/*
select * from Usuarios
select * from Contratos

select * from Pagos
select * from FacturasPagos
select * from Empresas
*/
/*
CREATE TABLE Pagos(
NumInterno INT IDENTITY (10000,1) PRIMARY KEY NOT NULL,
Fecha DATETIME DEFAULT GETDATE() NOT NULL,
Monto INT CHECK(Monto<99999) NOT NULL,
Cajero BIGINT FOREIGN KEY REFERENCES Usuarios(CI)
)
go
*/

/*
Codigo INT CHECK (Codigo BETWEEN 1000 AND 9999) PRIMARY KEY NOT NULL,
Rut BIGINT CHECK (Rut BETWEEN 0 AND 999999999999) NOT NULL,
Nombre VARCHAR(100),
DirFiscal VARCHAR(100) NOT NULL,
Tel INT NOT NULL,
Baja BIT
*/

/*
delete from Contratos
delete from Empresas
*/
