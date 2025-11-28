
USE [Ejemplo.Desarrollo]

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_Insert]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_Insert]
(
	@Codigo varchar(150)
--,	@Valor varchar(-1)
,	@Valor varchar(MAX)
,	@Observaciones varchar(400)
)

AS

SET NOCOUNT ON

INSERT INTO [lut_Parametros]
(
	[Codigo]
	, [Valor]
	, [Observaciones]
)
VALUES
(
	@Codigo
	, @Valor
	, @Observaciones
)
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_Update]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_Update]
(
	@Codigo varchar(150),
	--@Valor varchar(-1),
	@Valor varchar(MAX),
	@Observaciones varchar(400)
)

AS

SET NOCOUNT ON

UPDATE [lut_Parametros]
SET [Valor] = @Valor,
	[Observaciones] = @Observaciones
WHERE [Codigo] = @Codigo
GO


	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_Delete]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_Delete]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

DELETE FROM [lut_Parametros]
WHERE [Codigo] = @Codigo
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_Get]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_Get]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Parametros]
WHERE [Codigo] = @Codigo

GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_GetAll]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_GetAll]

AS

SET NOCOUNT ON

SELECT * FROM [lut_Parametros] 

GO

DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_GetBy_Codigo]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_GetBy_Codigo]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Parametros]
WHERE [Codigo] = @Codigo

GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Parametros_GetBy_Codigo_Cantidad]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Parametros_GetBy_Codigo_Cantidad]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

SELECT COUNT(*) Cantidad
FROM [lut_Parametros]
WHERE [Codigo] = @Codigo
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_Insert]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_Insert]
(
	@Descripcion varchar(100)
,	@Dias int
	, @Id Numeric Output
)

AS

SET NOCOUNT ON

INSERT INTO [lut_Periodos]
(
	[Descripcion]
	, [Dias]
)
VALUES
(
	@Descripcion
	, @Dias
)

SELECT @Id = SCOPE_IDENTITY()
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_Update]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_Update]
(
	@Id int,
	@Descripcion varchar(100),
	@Dias int
)

AS

SET NOCOUNT ON

UPDATE [lut_Periodos]
SET [Descripcion] = @Descripcion,
	[Dias] = @Dias
WHERE [Id] = @Id
GO


	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_Delete]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_Delete]
(
	@Id int
)

AS

SET NOCOUNT ON

DELETE FROM [lut_Periodos]
WHERE [Id] = @Id
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_Get]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_Get]
(
	@Id int
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Periodos]
WHERE [Id] = @Id
ORDER BY Descripcion
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_GetAll]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_GetAll]

AS

SET NOCOUNT ON

SELECT * FROM [lut_Periodos] 
ORDER BY Descripcion
GO

DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_GetBy_Id]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_GetBy_Id]
(
	@Id int
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Periodos]
WHERE [Id] = @Id
ORDER BY Descripcion
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplolut_Periodos_GetBy_Id_Cantidad]
GO

CREATE PROCEDURE [dbo].[Ejemplolut_Periodos_GetBy_Id_Cantidad]
(
	@Id int
)

AS

SET NOCOUNT ON

SELECT COUNT(*) Cantidad
FROM [lut_Periodos]
WHERE [Id] = @Id
GO




	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_Insert]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_Insert]
(
	@Usuario nchar(50)
,	@Clave nchar(100)
,	@Activo bit
,	@Usuario_Alta varchar(50)
)

AS

SET NOCOUNT ON

INSERT INTO [sys_Usuarios]
(
	[Usuario]
	, [Clave]
	, [Activo]
	, [Usuario_Alta]
)
VALUES
(
	@Usuario
	, @Clave
	, @Activo
	, @Usuario_Alta
)
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_Update]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_Update]
(
	@Usuario nchar(50),
	@Clave nchar(100),
	@Activo bit,
	@Usuario_Modificacion varchar(50)
)

AS

SET NOCOUNT ON

UPDATE [sys_Usuarios]
SET [Clave] = @Clave,
	[Activo] = @Activo,
	[Fecha_Modificacion] = Getdate(),
	[Usuario_Modificacion] = @Usuario_Modificacion
WHERE [Usuario] = @Usuario
GO


	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_Delete]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_Delete]
(
	@Usuario nchar(50)
)

AS

SET NOCOUNT ON

UPDATE sys_Usuarios SET Activo = 0
WHERE [Usuario] = @Usuario
GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_Get]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_Get]
(
	@Usuario nchar(50)
)

AS

SET NOCOUNT ON

SELECT * FROM [sys_Usuarios]
WHERE [Usuario] = @Usuario

GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_GetAll]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_GetAll]

AS

SET NOCOUNT ON

SELECT * FROM [sys_Usuarios] 

GO

DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_GetBy_Activo]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_GetBy_Activo]
(
	@Activo bit
)

AS

SET NOCOUNT ON

SELECT * FROM [sys_Usuarios]
WHERE [Activo] = @Activo

GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_GetBy_Activo_Cantidad]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_GetBy_Activo_Cantidad]
(
	@Activo bit
)

AS

SET NOCOUNT ON

SELECT COUNT(*) Cantidad
FROM [sys_Usuarios]
WHERE [Activo] = @Activo
GO

DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_GetBy_Usuario]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_GetBy_Usuario]
(
	@Usuario nchar(50)
)

AS

SET NOCOUNT ON

SELECT * FROM [sys_Usuarios]
WHERE [Usuario] = @Usuario

GO

	DROP PROCEDURE IF EXISTS [dbo].[Ejemplosys_Usuarios_GetBy_Usuario_Cantidad]
GO

CREATE PROCEDURE [dbo].[Ejemplosys_Usuarios_GetBy_Usuario_Cantidad]
(
	@Usuario nchar(50)
)

AS

SET NOCOUNT ON

SELECT COUNT(*) Cantidad
FROM [sys_Usuarios]
WHERE [Usuario] = @Usuario
GO
