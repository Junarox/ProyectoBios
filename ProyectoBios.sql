USE MASTER
GO

IF EXISTS(SELECT * FROM SYSDATABASES WHERE NAME = 'BiosMoney')
BEGIN
	DROP DATABASE BiosMoney
END
GO

CREATE DATABASE BiosMoney ON(
	NAME = BiosMoney,
	FILENAME = 'C:\DataBases\BiosMoney.mdf'
)
GO

USE BiosMoney
GO

CREATE TABLE Usuarios (
Ci BIGINT CHECK(LEN(Ci) = 8) PRIMARY KEY NOT NULL,
Usuario VARCHAR(30) CHECK(LEN(Usuario) <= 30) UNIQUE NOT NULL,
Clave VARCHAR(7) CHECK(LEN(Clave) <= 7) NOT NULL,
NomCompleto VARCHAR(50) CHECK(LEN(NomCompleto) <= 50) NOT NULL
)
go

CREATE TABLE Gerentes (
Ci BIGINT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Usuarios(Ci),
Email VARCHAR(50) CHECK(LEN(Email) <= 50) NOT NULL
)
go

select * from Usuarios
insert Usuarios values(48328032, 'Juna', 'asd', 'Diego Furtado')
insert Gerentes values(48328032, 'diego32junarox@gmail.com')

CREATE TABLE Cajeros (
Ci BIGINT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Usuarios(Ci),
HoraInicio VARCHAR(4) CHECK(LEN(HoraInicio) <= 4) NOT NULL,
HoraFin VARCHAR(4) CHECK(LEN(HoraFin) <= 4) NOT NULL,
Baja BIT
)
go

CREATE TABLE Empresas (
Codigo INT CHECK (Codigo BETWEEN 1000 AND 9999) PRIMARY KEY NOT NULL,
Rut BIGINT CHECK (Rut BETWEEN 0 AND 999999999999) UNIQUE,
Nombre VARCHAR(100) CHECK(LEN(Nombre) <= 100) NOT NULL,
DirFiscal VARCHAR(100) CHECK(LEN(DirFiscal) <= 100) NOT NULL,
Tel BIGINT NOT NULL,
Baja BIT
)
go

CREATE TABLE Contratos (
CodEmpresa INT CHECK(CodEmpresa BETWEEN 1000 AND 9999) FOREIGN KEY REFERENCES Empresas(Codigo) NOT NULL,
CodTipo INT CHECK(CodTipo BETWEEN 10 AND 99) NOT NULL,
Nombre VARCHAR(100) NOT NULL,
Baja BIT,
PRIMARY KEY (CodEmpresa, CodTipo)
)
GO

--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--	ROLES	--

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
		IF(@@ERROR != 0)
			RETURN -2;
		ELSE
			RETURN 1;
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
					COMMIT TRANSACTION;
					RETURN 1;
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

CREATE PROCEDURE AltaCajero (@Usuario VARCHAR(30), @Clave VARCHAR(7), @CI INT, 
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
			IF EXISTS(SELECT * FROM Usuarios U LEFT JOIN Cajeros C ON U.Ci = C.Ci WHERE U.Usuario = @Usuario)
			BEGIN
				ROLLBACK TRANSACTION;
				RETURN -1;
			END
			--Si existe el cajero con esa cedula y est� dado de baja lo doy de alta.
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
					COMMIT TRANSACTION;
					RETURN 1;
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
					COMMIT TRANSACTION;
					RETURN 1;
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

	IF EXISTS(SELECT * FROM Usuarios U WHERE U.Usuario = @Usuario)
		RETURN -2;

	BEGIN TRANSACTION;

	UPDATE Usuarios SET Usuario = @Usuario, NomCompleto = @NomCompleto WHERE Ci = @Ci
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
		--Si existe la empresa con ese codigo y est� dado de baja, lo doy de alta.
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
		IF EXISTS(SELECT * FROM Empresas E WHERE E.Rut = @Rut)
			RETURN -2;
		ELSE
		BEGIN
			UPDATE Empresas SET Rut = @Rut, Nombre = @Nombre, DirFiscal = @DirFiscal, Tel = @Tel 
							WHERE Codigo = @Codigo
			IF(@@ERROR != 0)
				RETURN -3;
			ELSE
				RETURN 1;
		END
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