USE EJEMPLO_DESARROLLO

IF NOT EXISTS(SELECT * FROM master..syslogins WHERE name = 'dbo')
	EXEC sp_addlogin 'dbo', '', 'EJEMPLO_DESARROLLO'
GO

IF NOT EXISTS (SELECT * FROM [dbo].sysusers WHERE NAME = N'dbo' AND uid < 16382)
	EXEC sp_grantdbaccess N'dbo', N'dbo'
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_Insert]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_Insert]
(
	@Codigo varchar(150)
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

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_Insert] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_Update]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_Update]
(
	@Codigo varchar(150),
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

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_Update] TO [dbo]
GO


	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_Delete]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_Delete]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

DELETE FROM [lut_Parametros]
WHERE [Codigo] = @Codigo
GO

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_Delete] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_Get]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_Get]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Parametros]
WHERE [Codigo] = @Codigo

GO

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_Get] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_GetAll]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_GetAll]

AS

SET NOCOUNT ON

SELECT * FROM [lut_Parametros] 

GO

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_GetAll] TO [dbo]
GO


GRANT EXECUTE ON [dbo].[sp_lut_Parametros_GetBy_Codigo] TO [dbo]
GO
DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_GetBy_Codigo]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_GetBy_Codigo]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Parametros]
WHERE [Codigo] = @Codigo

GO

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_GetBy_Codigo] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Parametros_GetBy_Codigo_Cantidad]
GO

CREATE PROCEDURE [dbo].[sp_lut_Parametros_GetBy_Codigo_Cantidad]
(
	@Codigo varchar(150)
)

AS

SET NOCOUNT ON

SELECT COUNT(*) Cantidad
FROM [lut_Parametros]
WHERE [Codigo] = @Codigo
GO

GRANT EXECUTE ON [dbo].[sp_lut_Parametros_GetBy_Codigo_Cantidad] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_Insert]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_Insert]
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

SELECT @Id = IDENT_CURRENT('lut_Periodos')
GO

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_Insert] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_Update]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_Update]
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

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_Update] TO [dbo]
GO


	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_Delete]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_Delete]
(
	@Id int
)

AS

SET NOCOUNT ON

DELETE FROM [lut_Periodos]
WHERE [Id] = @Id
GO

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_Delete] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_Get]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_Get]
(
	@Id int
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Periodos]
WHERE [Id] = @Id
ORDER BY Descripcion
GO

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_Get] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_GetAll]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_GetAll]

AS

SET NOCOUNT ON

SELECT * FROM [lut_Periodos] 
ORDER BY Descripcion
GO

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_GetAll] TO [dbo]
GO


GRANT EXECUTE ON [dbo].[sp_lut_Periodos_GetBy_Id] TO [dbo]
GO
DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_GetBy_Id]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_GetBy_Id]
(
	@Id int
)

AS

SET NOCOUNT ON

SELECT * FROM [lut_Periodos]
WHERE [Id] = @Id
ORDER BY Descripcion
GO

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_GetBy_Id] TO [dbo]
GO

	DROP PROCEDURE IF EXISTS [dbo].[sp_lut_Periodos_GetBy_Id_Cantidad]
GO

CREATE PROCEDURE [dbo].[sp_lut_Periodos_GetBy_Id_Cantidad]
(
	@Id int
)

AS

SET NOCOUNT ON

SELECT COUNT(*) Cantidad
FROM [lut_Periodos]
WHERE [Id] = @Id
GO

GRANT EXECUTE ON [dbo].[sp_lut_Periodos_GetBy_Id_Cantidad] TO [dbo]
GO
