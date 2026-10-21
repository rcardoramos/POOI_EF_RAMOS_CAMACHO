USE ExamenFinal;
GO

CREATE TABLE tb_paises
(
    idpais CHAR(3) PRIMARY KEY,
    nombrepais VARCHAR(50) NOT NULL
);
GO

INSERT INTO tb_paises VALUES
('PER', 'Perú'),
('ARG', 'Argentina'),
('CHL', 'Chile'),
('COL', 'Colombia'),
('MEX', 'México');
GO

--Tablas

CREATE TABLE tb_Consumidor
(
    dnicont CHAR(8) PRIMARY KEY,
    nomcont VARCHAR(40) NOT NULL,
    apecont VARCHAR(40) NOT NULL,
    emailcont VARCHAR(250) UNIQUE,
    idpais CHAR(3),

    CONSTRAINT FK_Consumidor_Pais
        FOREIGN KEY (idpais)
        REFERENCES tb_paises(idpais)
);
GO

--Agregar consumidor

CREATE PROCEDURE usp_agrega_consumidor
    @dnicont CHAR(8),
    @nomcont VARCHAR(40),
    @apecont VARCHAR(40),
    @emailcont VARCHAR(250),
    @idpais CHAR(3),
    @mensaje VARCHAR(200) OUTPUT
AS
BEGIN
    IF LEN(@dnicont) <> 8 OR ISNUMERIC(@dnicont) = 0
    BEGIN
        SET @mensaje = 'El DNI debe ser numérico y tener 8 dígitos';
        RETURN;
    END

    IF LTRIM(RTRIM(@nomcont)) = ''
    BEGIN
        SET @mensaje = 'El nombre no puede estar vacío';
        RETURN;
    END

    IF LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @mensaje = 'El apellido no puede estar vacío';
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM tb_paises
        WHERE idpais = @idpais
    )
    BEGIN
        SET @mensaje = 'El país seleccionado no existe';
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM tb_Consumidor
        WHERE dnicont = @dnicont
    )
    BEGIN
        SET @mensaje = 'El consumidor ya se encuentra registrado';
        RETURN;
    END

    INSERT INTO tb_Consumidor
    (
        dnicont,
        nomcont,
        apecont,
        emailcont,
        idpais
    )
    VALUES
    (
        @dnicont,
        @nomcont,
        @apecont,
        @emailcont,
        @idpais
    );

    SET @mensaje = 'Consumidor registrado correctamente';
END;
GO

--Eliminar consumidor

DROP PROCEDURE IF EXISTS usp_eliminar_consumidor;
GO

CREATE PROCEDURE usp_eliminar_consumidor
    @dnicont CHAR(8),
    @mensaje VARCHAR(200) OUTPUT
AS
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM tb_Consumidor
        WHERE dnicont = @dnicont
    )
    BEGIN
        SET @mensaje = 'El consumidor no existe';
        RETURN;
    END

    DELETE FROM tb_Consumidor
    WHERE dnicont = @dnicont;

    SET @mensaje = 'Consumidor eliminado correctamente';
END;
GO

DECLARE @mensaje VARCHAR(200);

EXEC usp_eliminar_consumidor
    @dnicont = '12345678',
    @mensaje = @mensaje OUTPUT;

SELECT @mensaje AS Mensaje;
GO

--Listar consumidor

DROP PROCEDURE IF EXISTS usp_listar_consumidor;
GO

CREATE PROCEDURE usp_listar_consumidor
AS
BEGIN
    SELECT
        c.dnicont,
        c.nomcont,
        c.apecont,
        c.emailcont,
        c.idpais,
        p.nombrepais
    FROM tb_Consumidor c
    INNER JOIN tb_paises p
        ON c.idpais = p.idpais
    ORDER BY c.apecont, c.nomcont;
END;
GO
--´ruebas
DECLARE @mensaje VARCHAR(200);

EXEC usp_agrega_consumidor
    @dnicont = '87654321',
    @nomcont = 'Patricia',
    @apecont = 'Camacho',
    @emailcont = 'patricia_camacho@gmail.com',
    @idpais = 'PER',
    @mensaje = @mensaje OUTPUT;

SELECT @mensaje AS Mensaje;
GO

DECLARE @mensaje VARCHAR(200);

EXEC usp_agrega_consumidor
    @dnicont = '72564818',
    @nomcont = 'Ricardo',
    @apecont = 'Ramos',
    @emailcont = 'ricardoramos26@gmail.com',
    @idpais = 'PER',
    @mensaje = @mensaje OUTPUT;

SELECT @mensaje AS Mensaje;
GO

EXEC usp_listar_consumidor;
GO
--pruebas

--Buscar comsumidor

DROP PROCEDURE IF EXISTS usp_buscar_consumidor;
GO

CREATE PROCEDURE usp_buscar_consumidor
    @dnicont CHAR(8)
AS
BEGIN
    SELECT
        c.dnicont,
        c.nomcont,
        c.apecont,
        c.emailcont,
        c.idpais,
        p.nombrepais
    FROM tb_Consumidor c
    INNER JOIN tb_paises p
        ON c.idpais = p.idpais
    WHERE c.dnicont = @dnicont;
END;
GO

EXEC usp_buscar_consumidor
    @dnicont = '87654321';
GO

--Actualizar

DROP PROCEDURE IF EXISTS usp_actualiza_consumidor;
GO

CREATE PROCEDURE usp_actualiza_consumidor
    @dnicont CHAR(8),
    @nomcont VARCHAR(40),
    @apecont VARCHAR(40),
    @emailcont VARCHAR(250),
    @idpais CHAR(3),
    @mensaje VARCHAR(200) OUTPUT
AS
BEGIN
    -- Verificar consumidor
    IF NOT EXISTS
    (
        SELECT 1
        FROM tb_Consumidor
        WHERE dnicont = @dnicont
    )
    BEGIN
        SET @mensaje = 'El consumidor no existe';
        RETURN;
    END

    -- Validar nombres
    IF LTRIM(RTRIM(@nomcont)) = ''
    BEGIN
        SET @mensaje = 'El nombre no puede estar vacío';
        RETURN;
    END

    -- Validar apellidos
    IF LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @mensaje = 'El apellido no puede estar vacío';
        RETURN;
    END

    -- Validar país
    IF NOT EXISTS
    (
        SELECT 1
        FROM tb_paises
        WHERE idpais = @idpais
    )
    BEGIN
        SET @mensaje = 'El país seleccionado no existe';
        RETURN;
    END

    -- Evitar email repetido en otro consumidor
    IF EXISTS
    (
        SELECT 1
        FROM tb_Consumidor
        WHERE emailcont = @emailcont
          AND dnicont <> @dnicont
    )
    BEGIN
        SET @mensaje = 'El correo ya pertenece a otro consumidor';
        RETURN;
    END

    UPDATE tb_Consumidor
    SET
        nomcont = @nomcont,
        apecont = @apecont,
        emailcont = @emailcont,
        idpais = @idpais
    WHERE dnicont = @dnicont;

    SET @mensaje = 'Consumidor actualizado correctamente';
END;
GO

DECLARE @mensaje VARCHAR(200);

EXEC usp_actualiza_consumidor
    @dnicont = '87654321',
    @nomcont = 'Patricia',
    @apecont = 'Camacho',
    @emailcont = 'patricia.datoactualizado@gmail.com',
    @idpais = 'PER',
    @mensaje = @mensaje OUTPUT;

SELECT @mensaje AS Mensaje;
GO

EXEC usp_buscar_consumidor
    @dnicont = '87654321';

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';