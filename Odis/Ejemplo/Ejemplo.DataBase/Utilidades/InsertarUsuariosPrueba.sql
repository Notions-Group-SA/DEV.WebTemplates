



USE [EJEMPLO_DESARROLLO]

--argumentos
DECLARE @Nueva_Clave NVARCHAR(200) = 'fernando';

--cambio de clave
DECLARE @hash VARBINARY(16);
SET @hash = HASHBYTES('MD5', CAST(@Nueva_Clave AS VARCHAR(MAX)));  --es importante que castee a VARCHAR 
declare @pass varchar(200);
SELECT @pass =CAST(@hash AS VARCHAR(200))


INSERT INTO sys_usuarios (Usuario, Clave, Activo, Usuario_Alta)
VALUES ('fernando', @pass, 1, 'fernando');


SELECT * FROM sys_usuarios 

--OJO!!! - porque si se le erra se blanque todo
--update Accesos_Usuarios set [Password]=@pass 
--where Telefono = @TEL_A_BLANQUEAR 

--SELECT * FROM Accesos_Usuarios WHERE Telefono = @TEL_A_BLANQUEAR
