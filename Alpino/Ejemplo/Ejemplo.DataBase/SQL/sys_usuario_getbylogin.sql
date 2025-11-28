

USE [Ejemplo.Desarrollo]

GO

CREATE OR ALTER PROCEDURE [dbo].[sp_sys_Usuarios_GetBy_Login]
	@Usuario Varchar(200), @Password Varchar(100)
AS
BEGIN

    SELECT * FROM sys_Usuarios WHERE Usuario = @Usuario AND [Clave] = @Password AND Activo = 1

END

