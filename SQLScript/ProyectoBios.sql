USE MASTER
GO

IF EXISTS(SELECT * FROM SYSDATABASES WHERE NAME = 'BiosMoney')
BEGIN
	DROP DATABASE BiosMoney
	DROP SERVER ROLE [GerentesSQL]
END
GO

CREATE DATABASE BiosMoney ON(
	NAME = BiosMoney,
	FILENAME = 'C:\DataBases\BiosMoney.mdf'
)
GO

--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--

-- Creo un server role para manejar los grant y alter logins de los Cajeros
CREATE SERVER ROLE [GerentesSQL]
GO

-- Le doy al role de sql permisos para modificar logins
GRANT ALTER ANY LOGIN TO [GerentesSQL]
GO

CREATE SERVER ROLE [IIS]
CREATE LOGIN [IIS APPPOOL\DefaultAppPool] FROM WINDOWS WITH DEFAULT_DATABASE = master
GO

EXEC master..sp_addsrvrolemember @loginame = N'IIS APPPOOL\DefaultAppPool', @rolename = [IIS]
GO

USE BiosMoney
GO

CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool]
GO

-- Creo el role Gerentes en la base de datos BiosMoney
CREATE ROLE [Gerentes]
GO

-- Le doy al rol Gerentes los permisos para modificar cualquier usuario
GRANT ALTER ANY USER TO [Gerentes]

-- Creo el rol Cajeros en la base de datos con autorizacion del rol Gerentes. 
-- Esto hace que el dueño del rol Cajeros sean los usuarios del rol Gerentes.
CREATE ROLE [Cajeros] AUTHORIZATION [Gerentes];  
GO  

-- Creo las tablas
CREATE TABLE Usuarios (
Ci BIGINT CHECK(LEN(Ci) = 8) PRIMARY KEY NOT NULL,
Usuario VARCHAR(30) CHECK(LEN(Usuario) <= 30) UNIQUE NOT NULL,
Clave VARCHAR(7) CHECK(LEN(Clave) <= 7) NOT NULL,
NomCompleto VARCHAR(50) CHECK(LEN(NomCompleto) <= 50) NOT NULL
)

CREATE TABLE Gerentes (
Ci BIGINT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Usuarios(Ci),
Email VARCHAR(50) CHECK(LEN(Email) <= 50) NOT NULL
)

CREATE TABLE Cajeros (
Ci BIGINT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Usuarios(Ci),
HoraInicio VARCHAR(4) CHECK(LEN(HoraInicio) <= 4) NOT NULL,
HoraFin VARCHAR(4) CHECK(LEN(HoraFin) <= 4) NOT NULL,
Baja BIT
)

CREATE TABLE Empresas (
Codigo INT CHECK (Codigo BETWEEN 1000 AND 9999) PRIMARY KEY NOT NULL,
Rut BIGINT CHECK (Rut BETWEEN 0 AND 999999999999) UNIQUE,
Nombre VARCHAR(100) CHECK(LEN(Nombre) <= 100) NOT NULL,
DirFiscal VARCHAR(100) CHECK(LEN(DirFiscal) <= 100) NOT NULL,
Tel BIGINT NOT NULL,
Baja BIT
)

CREATE TABLE Contratos (
CodEmpresa INT CHECK(CodEmpresa BETWEEN 1000 AND 9999) FOREIGN KEY REFERENCES Empresas(Codigo) NOT NULL,
CodTipo INT CHECK(CodTipo BETWEEN 10 AND 99) NOT NULL,
Nombre VARCHAR(100) NOT NULL,
Baja BIT,
PRIMARY KEY (CodEmpresa, CodTipo)
)

CREATE TABLE Pagos(
NumInterno INT IDENTITY (10000,1) PRIMARY KEY NOT NULL,
Fecha DATETIME DEFAULT GETDATE() NOT NULL,
Monto BIGINT NOT NULL,
Cajero BIGINT FOREIGN KEY REFERENCES Usuarios(CI)
)

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
GO

-- Creo los Stored Procedures

-- USUARIO	-- USUARIO	-- USUARIO	-- USUARIO	-- USUARIO	-- USUARIO	-- USUARIO	-- USUARIO	-- USUARIO	--

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

CREATE PROCEDURE ModClave (@Usuario VARCHAR(30), @Clave VARCHAR(7)) AS
BEGIN
	IF NOT (EXISTS (SELECT *
					FROM Usuarios U
					WHERE U.Usuario = @Usuario))
		RETURN -1;
	ELSE
	BEGIN
		DECLARE @Ci INT
		SELECT @Ci = C.Ci FROM Usuarios U INNER JOIN Cajeros C ON U.Ci = C.Ci WHERE U.Usuario = @Usuario
		IF EXISTS (SELECT * FROM Cajeros C WHERE C.Ci = @Ci)
			RETURN -1;
		ELSE
		BEGIN
			UPDATE Usuarios
			SET Clave = @Clave
			WHERE Usuario = @Usuario
			IF(@@ERROR != 0)
				RETURN -2;
			ELSE
			BEGIN
				Declare @VarSentencia varchar(200)
				Set @VarSentencia = 'ALTER LOGIN [' +  @Usuario + '] WITH PASSWORD = ' + QUOTENAME(@Clave, '''')
				Exec (@VarSentencia)
		
				IF (@@ERROR <> 0)
					RETURN -2
				ELSE
					RETURN 1;
			END
		END
	END
END
GO

--	GERENTE	--	GERENTE	--	GERENTE	--	GERENTE	--	GERENTE	--	GERENTE	--	GERENTE	--	GERENTE	--	GERENTE	--

CREATE PROCEDURE BuscarGerenteLogueo(@Usuario VARCHAR(30)) AS
BEGIN
	SELECT *
	FROM Usuarios U INNER JOIN Gerentes G
	ON U.Usuario = @Usuario and G.Ci= U.Ci
END
GO

CREATE PROCEDURE AltaGerente(@Usuario VARCHAR(30), @Clave VARCHAR(7), @CI INT, 
@NomCompleto VARCHAR(50), @Email VARCHAR(50)) AS
BEGIN
	BEGIN TRANSACTION;

	IF EXISTS(SELECT * FROM Usuarios U WHERE U.Ci = @CI)
	BEGIN
		ROLLBACK TRANSACTION;
		RETURN -1;
	END
	ELSE
	BEGIN
	-- No Permito que hayan 2 gerentes con un mismo mail
		IF EXISTS(SELECT * FROM Gerentes G WHERE G.Email = @Email)
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -2;
		END
		ELSE
		BEGIN
			INSERT Usuarios VALUES(@CI,@Usuario,@Clave,@NomCompleto)
			IF(@@ERROR != 0)
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -3;
			END
			ELSE
			BEGIN
				INSERT INTO Gerentes VALUES(@CI,@Email)
				IF(@@ERROR != 0)
				BEGIN
					ROLLBACK TRANSACTION;
					RETURN -3;
				END
				ELSE
				BEGIN
					-- Si todo salio bien, creo el usuario SQL y de DB y le asigno el rol
					
					-- Creo el Logueo SQL
					DECLARE @Sentencia VARCHAR(200)
					SET @Sentencia='CREATE LOGIN ['+@Usuario+']WITH PASSWORD ='+QUOTENAME(@Clave,'''')
					EXEC (@Sentencia)
			
					IF(@@ERROR != 0)
						BEGIN
							ROLLBACK TRANSACTION;
							RETURN -3;
						END
				
					-- Creo el Usuario de BD
					DECLARE @Sentencia2 VARCHAR(200)
					SET @Sentencia2 = 'Create User ['+@Usuario+']From Login ['+@Usuario+']'
					EXEC(@Sentencia2)
			
					IF(@@ERROR != 0)
						BEGIN
							ROLLBACK TRANSACTION;
							RETURN -3;
						END
					ELSE
					BEGIN
						COMMIT TRAN;
						-- Asigno el rol de DB
						EXEC sp_addrolemember @rolename=[Gerentes], @membername=@Usuario
				
						IF(@@ERROR != 0)
						BEGIN
							RETURN -3;
						END
						ELSE
						BEGIN
							-- Asigno el rol de SQL
							EXEC sp_addsrvrolemember @loginame=@Usuario, @rolename=[GerentesSQL]
							IF(@@ERROR != 0)
							BEGIN
								RETURN -3;
							END
							ELSE
							BEGIN
								RETURN 1;
							END
						END
					END
				END
			END
		END
	END
END
GO

CREATE PROCEDURE ListarGerentes AS
BEGIN
	SELECT * FROM Gerentes inner join Usuarios ON Gerentes.Ci = Usuarios.Ci
END
GO

--	CAJERO	--	CAJERO	--	CAJERO	--	CAJERO	--	CAJERO	--	CAJERO	--	CAJERO	--	CAJERO	--	CAJERO	--

CREATE PROCEDURE BuscarCajeroLogueo(@Usuario VARCHAR(30)) AS
BEGIN
	SELECT *
	FROM Usuarios U INNER JOIN Cajeros C
	ON U.Usuario = @Usuario and C.Ci= U.Ci and C.Baja = 0
END
GO

create PROCEDURE AltaCajero (@Usuario VARCHAR(30), @Clave VARCHAR(7), @CI INT, 
@NomCompleto VARCHAR(50), @HoraInicio VARCHAR(4), @HoraFin VARCHAR(4)) AS
BEGIN
	BEGIN TRANSACTION;
	IF EXISTS(SELECT * FROM Usuarios U WHERE U.Ci = @CI)
	BEGIN
		IF NOT(EXISTS(SELECT * FROM Usuarios U LEFT JOIN Cajeros C ON U.Ci = C.Ci WHERE C.Baja = 1))
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -2;
		END
		ELSE
		BEGIN
			--Si existe el cajero con esa cedula y está dado de baja lo doy de alta.
			UPDATE Usuarios SET Clave = @Clave, Usuario = @Usuario, NomCompleto = @NomCompleto WHERE Ci = @CI
			IF(@@ERROR != 0)
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -3;
			END
			ELSE
			BEGIN
				UPDATE Cajeros SET HoraInicio = @HoraInicio, HoraFin = @HoraFin, Baja = 0 WHERE Ci = @CI
				IF(@@ERROR != 0)
				BEGIN
					ROLLBACK TRANSACTION;
					RETURN -3;
				END
				ELSE
				BEGIN
					-- Creo el Usuario de logueo
					DECLARE @Sentencia varchar(200)
					SET @Sentencia='CREATE LOGIN ['+@Usuario+']WITH PASSWORD ='+QUOTENAME(@Clave,'''')
					EXEC (@Sentencia)
			
					IF(@@ERROR != 0)
					BEGIN
						ROLLBACK TRANSACTION;
						RETURN -3;
					END
				
					-- Creo el Usuario de DB
					DECLARE @Sentencia2 VARCHAR(200)
					SET @Sentencia2 = 'CREATE USER ['+@Usuario+'] FROM LOGIN ['+@Usuario+']'
					EXEC(@Sentencia2)
			
					IF(@@ERROR != 0)
					BEGIN
						ROLLBACK TRANSACTION;
						RETURN -3;
					END
					ELSE
					BEGIN
						COMMIT TRANSACTION;
					
						-- Asigno el rol de DB
						EXEC sp_addrolemember @rolename=[Cajeros], @membername=@Usuario
				
						IF(@@ERROR != 0)
							RETURN -3;
						ELSE
							RETURN 1;
					END
				END
			END
		END
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
			IF(@@ERROR != 0)
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -3;
			END
			ELSE
			BEGIN
				INSERT Cajeros VALUES(@CI, @HoraInicio, @HoraFin, 0)
				IF(@@ERROR != 0)
				BEGIN
					ROLLBACK TRANSACTION;
					RETURN -3;
				END
				ELSE
				BEGIN
					-- Creo el Usuario de logueo
					DECLARE @Sentencia3 varchar(200)
					SET @Sentencia3='CREATE LOGIN ['+@Usuario+']WITH PASSWORD ='+QUOTENAME(@Clave,'''')
					EXEC (@Sentencia3)
			
					IF(@@ERROR != 0)
					BEGIN
						ROLLBACK TRANSACTION;
						RETURN -3;
					END
				
					-- Creo el Usuario de DB
					DECLARE @Sentencia4 VARCHAR(200)
					SET @Sentencia4 = 'CREATE USER ['+@Usuario+'] FROM LOGIN ['+@Usuario+']'
					EXEC(@Sentencia4)
			
					IF(@@ERROR != 0)
					BEGIN
						ROLLBACK TRANSACTION;
						RETURN -3;
					END
					ELSE
					BEGIN
						COMMIT TRANSACTION;
					
						-- Asigno el rol de DB
						EXEC sp_addrolemember @rolename=[Cajeros], @membername=@Usuario
				
						IF(@@ERROR != 0)
							RETURN -3;
						ELSE
							RETURN 1;
					END
				END
			END
		END
	END
END
GO

CREATE PROCEDURE ModCajero (@Ci INT, @Usuario VARCHAR(30), @NomCompleto VARCHAR(50), @HoraInicio VARCHAR(4), @HoraFin VARCHAR(4)) AS
BEGIN
	IF NOT(EXISTS(SELECT * FROM Cajeros C WHERE C.Ci = @Ci))
		RETURN -1;
	IF EXISTS(SELECT * FROM Cajeros C WHERE C.Ci = @Ci and C.Baja = 1)
		RETURN -1;

	BEGIN TRANSACTION;

	UPDATE Usuarios SET NomCompleto = @NomCompleto WHERE Ci = @Ci
	IF(@@ERROR != 0)
	BEGIN
		ROLLBACK TRANSACTION;
		RETURN -3;
	END
	ELSE
	BEGIN
		UPDATE Cajeros
		SET HoraInicio = @HoraInicio, HoraFin = @HoraFin 
		WHERE Ci = @Ci
		IF(@@ERROR != 0)
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -3;
		END
		ELSE
		BEGIN
			COMMIT TRANSACTION;
			RETURN 1;
		END
	END
END
GO

CREATE PROCEDURE BajaCajero(@Ci INT) AS
BEGIN
	IF NOT(EXISTS(SELECT * FROM Cajeros C WHERE C.Ci = @Ci))
		RETURN -1;
	ELSE IF EXISTS(SELECT * FROM Cajeros C WHERE C.Ci = @Ci AND C.Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Cajeros SET Baja = 1 WHERE Ci = @Ci
		IF(@@ERROR != 0)
			RETURN -2;
		ELSE
		BEGIN
			DECLARE @Usuario varchar(200)
			Select @Usuario = Usuarios.Usuario from Usuarios where Usuarios.Ci = @Ci
			EXEC sp_droprolemember [Cajeros], @Usuario;
			if(@@ERROR != 0)
				RETURN -2;
			ELSE
			BEGIN
				EXECUTE sp_dropuser @Usuario;
				if(@@ERROR != 0)
				BEGIN
					RETURN -2;
				END
				ELSE
				BEGIN
					EXEC sp_droplogin @Usuario;
					IF(@@ERROR != 0)
					BEGIN
					RETURN -2;
					END
					ELSE
						RETURN 1;
				END
			END
		END
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

CREATE PROCEDURE ListarCajeros AS
BEGIN
	SELECT * FROM Cajeros inner join Usuarios ON Cajeros.Ci = Usuarios.Ci WHERE Cajeros.Baja = 0
END
GO

--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--	EMPRESA	--

CREATE PROCEDURE AltaEmpresa(@Codigo INT, @Rut BIGINT, @Nombre VARCHAR(100), @DirFiscal VARCHAR(100), @Tel BIGINT) AS
BEGIN
	BEGIN TRANSACTION;
	IF EXISTS(SELECT * FROM Empresas E WHERE E.Codigo = @Codigo AND E.Baja = 0)
	BEGIN
		ROLLBACK TRANSACTION;
		RETURN -1;
	END
	ELSE IF EXISTS(SELECT * FROM Empresas E WHERE E.Codigo = @Codigo AND E.Baja = 1)
	BEGIN
		--Si existe la empresa con ese codigo y está dado de baja, lo doy de alta.
		UPDATE Empresas SET Rut = @Rut, Nombre = @Nombre, DirFiscal = @DirFiscal, Tel = @Tel, Baja = 0 WHERE Codigo = @Codigo
		IF(@@ERROR != 0)
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -3;
		END
		ELSE
		BEGIN
			COMMIT TRANSACTION;
			RETURN 1;
		END
	END
	ELSE
	BEGIN
		--Si no existe la empresa con ese codigo, la agrego.
		IF(EXISTS(SELECT * FROM Empresas e WHERE e.Rut = @Rut))
		BEGIN
			ROLLBACK TRANSACTION;
			RETURN -2;
		END
		ELSE
		BEGIN
			INSERT Empresas VALUES(@Codigo, @Rut, @Nombre, @dirFiscal, @Tel, 0)
			IF(@@ERROR != 0)
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -3;
			END
			ELSE
			BEGIN
				COMMIT TRANSACTION;
				RETURN 1;
			END
		END
	END
END
GO

CREATE PROCEDURE ModEmpresa (@Codigo INT, @Rut BIGINT, @Nombre VARCHAR(100), @DirFiscal VARCHAR(100), @Tel BIGINT) AS
BEGIN
	IF NOT (EXISTS(SELECT * FROM Empresas E WHERE E.Codigo = @Codigo))
		RETURN -1;
	IF EXISTS(SELECT* FROM Empresas E WHERE E.Codigo = @Codigo AND E.Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Empresas SET Nombre = @Nombre, DirFiscal = @DirFiscal, Tel = @Tel 
						WHERE Codigo = @Codigo
		IF(@@ERROR != 0)
			RETURN -3;
		ELSE
			RETURN 1;
	END
END
GO

CREATE PROCEDURE BajaEmpresa (@Codigo INT) AS
BEGIN
	IF NOT (EXISTS(SELECT * FROM Empresas E WHERE E.Codigo = @Codigo))
		RETURN -1;
	IF EXISTS(SELECT * FROM Empresas E WHERE E.Codigo = @Codigo AND E.Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Empresas SET Baja = 1 WHERE Codigo = @Codigo
		IF(@@ERROR != 0)
			RETURN -2;
		ELSE
			RETURN 1;
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

--	CONTRATO	--	CONTRATO	--	CONTRATO	--	CONTRATO	--	CONTRATO	--	CONTRATO	--	CONTRATO	--	CONTRATO	--

CREATE PROCEDURE AltaContrato (@CodEmpresa INT, @CodTipo INT, @Nombre VARCHAR(100)) AS
BEGIN
	IF EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND C.Baja = 1)
	BEGIN
		UPDATE Contratos SET Baja = 0, Nombre = @Nombre WHERE CodEmpresa = @CodEmpresa AND CodTipo = @CodTipo
		IF(@@ERROR != 0)
			RETURN -2;
	END
	ELSE IF EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND C.Baja = 0)
		RETURN -1;
	ELSE
	BEGIN
		INSERT Contratos VALUES(@CodEmpresa,@CodTipo,@Nombre,0)
		IF(@@ERROR != 0)
			RETURN -2;
		ELSE
			RETURN 1;
	END
END
GO

CREATE PROCEDURE ModContrato (@CodEmpresa INT, @CodTipo INT, @Nombre VARCHAR(100)) AS
BEGIN
	IF NOT EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo)
		RETURN -1;
	IF EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Contratos SET Nombre = @Nombre WHERE CodEmpresa = @CodEmpresa AND CodTipo = @CodTipo
		IF(@@ERROR != 0)
			RETURN -2;
		ELSE
			RETURN 1;
	END
END
GO

CREATE PROCEDURE BajaContrato (@CodEmpresa INT, @CodTipo INT) AS
BEGIN
	IF NOT EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo)
		RETURN -1;
	IF EXISTS(SELECT * FROM Contratos C WHERE C.CodEmpresa = @CodEmpresa AND C.CodTipo = @CodTipo AND Baja = 1)
		RETURN -1;
	ELSE
	BEGIN
		UPDATE Contratos SET Baja = 1 WHERE CodEmpresa = @CodEmpresa AND CodTipo = @CodTipo
		IF(@@ERROR != 0)
			RETURN -2;
		ELSE
			RETURN 1;
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
	ON C.CodEmpresa = @CodEmpresa AND C.CodEmpresa = E.Codigo AND C.Baja = 0
END
GO

CREATE PROCEDURE ListarTodosLosContratos AS
BEGIN
	SELECT * FROM Contratos where Contratos.Baja = 0
END
GO

--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--	PAGOS	--

CREATE PROCEDURE AltaPago(@Fecha DATETIME, @Monto INT, @Cajero INT) AS
BEGIN
	IF NOT (EXISTS(SELECT * FROM Cajeros WHERE Ci = @Cajero))
	BEGIN
		RETURN -2
	END
	ELSE
	BEGIN
		INSERT INTO Pagos VALUES(@Fecha,@Monto,@Cajero)
		IF(@@ERROR != 0)
			RETURN -1
		ELSE
			RETURN 1
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


--	PERMISOS	--	PERMISOS	--	PERMISOS	--	PERMISOS	--	PERMISOS	--	PERMISOS	--	PERMISOS	--	PERMISOS	--

-- Asigno a los roles los permisos para acceder a los StoredProcedures.

-- Al ser roles creados sin un permiso predefinido por SQL o DB (ej: sysadmin, o dbowner),
-- los roles se crean sin ningún permiso predefinido.

-- GERENTES

grant execute on object::[dbo].[Logueo] to [Gerentes]
grant execute on object::[dbo].[AltaGerente] to [Gerentes]
grant execute on object::[dbo].[BuscarGerenteLogueo] to [Gerentes]
grant execute on object::[dbo].[ListarGerentes] to [Gerentes]
grant execute on object::[dbo].[ModClave] to [Gerentes]

grant execute on object::[dbo].[AltaCajero] to [Gerentes]
grant execute on object::[dbo].[BajaCajero] to [Gerentes]
grant execute on object::[dbo].[BuscarCajero] to [Gerentes]
grant execute on object::[dbo].[ListarCajeros] to [Gerentes]
grant execute on object::[dbo].[ModCajero] to [Gerentes]

grant execute on object::[dbo].[AltaEmpresa] to [Gerentes]
grant execute on object::[dbo].[BajaEmpresa] to [Gerentes]
grant execute on object::[dbo].[BuscarEmpresa] to [Gerentes]
grant execute on object::[dbo].[ListarEmpresas] to [Gerentes]
grant execute on object::[dbo].[ModEmpresa] to [Gerentes]

grant execute on object::[dbo].[AltaContrato] to [Gerentes]
grant execute on object::[dbo].[BajaContrato] to [Gerentes]
grant execute on object::[dbo].[BuscarContrato] to [Gerentes]
grant execute on object::[dbo].[ListarContrato] to [Gerentes]
grant execute on object::[dbo].[ModContrato] to [Gerentes]

-- CAJEROS
grant execute on object::[dbo].[BuscarCajeroLogueo] to [Cajeros]
grant execute on object::[dbo].[AltaPago] to [Cajeros]
grant execute on object::[dbo].[Logueo] to [Cajeros]
grant execute on object::[dbo].[ModClave] to [Cajeros]
grant execute on object::[dbo].[RegistrarFacturaEnPago] to [Cajeros]
grant execute on object::[dbo].[BuscarContrato] to [Cajeros]
go

-- DATOS PRUEBA  -- DATOS PRUEBA  -- DATOS PRUEBA  -- DATOS PRUEBA  -- DATOS PRUEBA  -- DATOS PRUEBA  -- DATOS PRUEBA  --

CREATE PROCEDURE DatosPrueba AS
BEGIN
	EXEC AltaGerente	'juna',		'asd',		48328032, 	'Diego Furtado',		'diego32junarox@gmail.com';

	EXEC AltaGerente	'sebaok',	'kkk',		11111111, 	'Sebastian Figueredo',	'sebaok@hotmail.com';
	
	EXEC AltaCajero		'desto',	'ooohhaa',	22222222, 	'Hernan Quiroga',	'0730',		'1300';

	EXEC AltaCajero		'optis91',	'uhaha',	33333333,	'Horacio Carcaja',	'1200',		'1600';

	EXEC AltaCajero		'lafne',	'qweok',	44444444,	'Mariana Larroza',	'1700',		'2230';

	EXEC AltaCajero		'raftz',	'Laft',		55555555,	'Sofía Urreta',		'0820',		'1630';

	EXEC AltaCajero		'tali652',	'yubi6',	66666666,	'Tatiana Lasteu',	'0640',		'1700';

	EXEC AltaCajero		'Joaco55',	'ruta27',	77777777,	'Joaquin Vila',		'1220',		'1650';

	EXEC AltaCajero		'uma1995',	'lulu123',	88888888,	'Lucia Ñedu',		'1800',		'2130';

	EXEC AltaCajero		'nandu',	'uhxu78',	99999999,	'Ximena Perez',		'1515',		'2150';

	EXEC BajaCajero 88888888
	EXEC BajaCajero 22222222

	INSERT Empresas VALUES(1234, 111111111111, 'Saura', '18 de julio 1234', 123456789, 0)
	INSERT Empresas VALUES(4567, 222222222222, 'Lacost', 'Yucatan 2598', 4861312, 0)
	INSERT Empresas VALUES(7891, 333333333333, 'CocaCola', '8 de Octubre 1296', 846615, 0)
	INSERT Empresas VALUES(1472, 444444444444, 'Lay´s', 'Guayaqui 159', 3258865, 0)
	INSERT Empresas VALUES(2583, 555555555555, 'Roller', 'Guayabos 2531', 985621, 0)
	INSERT Empresas VALUES(3691, 666666666666, 'Abasto', 'Av. Italia 3065', 62348, 0)
	INSERT Empresas VALUES(9874, 777777777777, 'Pop-UP', 'Bv. España 1232', 897415161156156, 0)
	INSERT Empresas VALUES(6541, 888888888888, 'Netul', 'Av. Brasil 5620', 6161651, 0)
	INSERT Empresas VALUES(3210, 999999999999, 'Tata', 'Gaboto 123', 656298012, 1)

	INSERT Contratos VALUES(1234, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(1234, 12, 'CONTRATO 2', 0)
	INSERT Contratos VALUES(1234, 13, 'CONTRATO 3', 0)
	INSERT Contratos VALUES(1234, 14, 'CONTRATO 4', 0)
	INSERT Contratos VALUES(1234, 15, 'CONTRATO 5', 1)
	INSERT Contratos VALUES(1234, 16, 'CONTRATO 6', 0)
	INSERT Contratos VALUES(1234, 17, 'CONTRATO 7', 0)

	INSERT Contratos VALUES(4567, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(4567, 12, 'CONTRATO 2', 0)
	INSERT Contratos VALUES(4567, 13, 'CONTRATO 3', 0)
	INSERT Contratos VALUES(4567, 14, 'CONTRATO 4', 0)
	INSERT Contratos VALUES(4567, 15, 'CONTRATO 5', 0)
	INSERT Contratos VALUES(4567, 16, 'CONTRATO 6', 1)
	INSERT Contratos VALUES(4567, 17, 'CONTRATO 7', 1)

	INSERT Contratos VALUES(7891, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(7891, 12, 'CONTRATO 2', 0)
	INSERT Contratos VALUES(7891, 13, 'CONTRATO 3', 0)
	INSERT Contratos VALUES(7891, 14, 'CONTRATO 4', 0)
	INSERT Contratos VALUES(7891, 15, 'CONTRATO 5', 0)
	
	INSERT Contratos VALUES(1472, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(1472, 12, 'CONTRATO 2', 0)
	INSERT Contratos VALUES(1472, 13, 'CONTRATO 3', 0)
	INSERT Contratos VALUES(1472, 14, 'CONTRATO 4', 0)
	INSERT Contratos VALUES(1472, 15, 'CONTRATO 5', 1)
	INSERT Contratos VALUES(1472, 16, 'CONTRATO 6', 0)

	INSERT Contratos VALUES(3691, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(3691, 12, 'CONTRATO 2', 0)

	INSERT Contratos VALUES(9874, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(9874, 12, 'CONTRATO 2', 0)
	INSERT Contratos VALUES(9874, 13, 'CONTRATO 3', 1)
	INSERT Contratos VALUES(9874, 14, 'CONTRATO 4', 1)
	INSERT Contratos VALUES(9874, 15, 'CONTRATO 5', 1)
	INSERT Contratos VALUES(9874, 16, 'CONTRATO 6', 1)

	INSERT Contratos VALUES(6541, 11, 'CONTRATO 1', 0)
	INSERT Contratos VALUES(6541, 12, 'CONTRATO 2', 1)
	INSERT Contratos VALUES(6541, 13, 'CONTRATO 3', 0)
	INSERT Contratos VALUES(6541, 14, 'CONTRATO 4', 0)
	INSERT Contratos VALUES(6541, 15, 'CONTRATO 5', 0)
	INSERT Contratos VALUES(6541, 16, 'CONTRATO 6', 0)

	INSERT Contratos VALUES(3210, 11, 'CONTRATO 1', 1)
	INSERT Contratos VALUES(3210, 12, 'CONTRATO 2', 1)
	INSERT Contratos VALUES(3210, 13, 'CONTRATO 3', 1)
	INSERT Contratos VALUES(3210, 14, 'CONTRATO 4', 1)
	INSERT Contratos VALUES(3210, 15, 'CONTRATO 5', 1)
	INSERT Contratos VALUES(3210, 16, 'CONTRATO 6', 1)
	
END
GO
