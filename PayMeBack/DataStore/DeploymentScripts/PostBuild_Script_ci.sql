-- CI Specific POST SQL statements go here

USE [master]
GO

/****** Object:  Login [NT AUTHORITY\NETWORK SERVICE]    Script Date: 02/16/2011 07:35:07 ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'NT AUTHORITY\NETWORK SERVICE')
CREATE LOGIN [NT AUTHORITY\NETWORK SERVICE] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO

USE [ola_tran]
GO
/****** Object:  User [NETWORK SERVICE]    Script Date: 02/16/2011 07:35:07 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETWORK SERVICE') 
BEGIN
	USE [ola_tran]
	IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETWORK SERVICE')
	CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]

	USE [ola_log]
	IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETWORK SERVICE')
	CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
	
	USE [ola_audit_trail]
	IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETWORK SERVICE')
	CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
	
	USE [ola_state]
	IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETWORK SERVICE')
	CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
END
GO

USE [ola_tran]
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETACCOUNTS\CiWebAppUser')
DROP USER [NETACCOUNTS\CiWebAppUser]
GO
USE [ola_tran]
GO
CREATE USER [NETACCOUNTS\CiWebAppUser] FOR LOGIN [NETACCOUNTS\CiWebAppUser] WITH DEFAULT_SCHEMA=[dbo]
GO
USE [ola_audit_trail]
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETACCOUNTS\CiWebAppUser')
DROP USER [NETACCOUNTS\CiWebAppUser]
GO
USE [ola_audit_trail]
GO
CREATE USER [NETACCOUNTS\CiWebAppUser] FOR LOGIN [NETACCOUNTS\CiWebAppUser] WITH DEFAULT_SCHEMA=[dbo]
GO
USE [ola_log]
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETACCOUNTS\CiWebAppUser')
DROP USER [NETACCOUNTS\CiWebAppUser]
GO
USE [ola_log]
GO
CREATE USER [NETACCOUNTS\CiWebAppUser] FOR LOGIN [NETACCOUNTS\CiWebAppUser] WITH DEFAULT_SCHEMA=[dbo]
GO
USE [ola_state]
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETACCOUNTS\CiWebAppUser')
DROP USER [NETACCOUNTS\CiWebAppUser]
GO
USE [ola_state]
GO
CREATE USER [NETACCOUNTS\CiWebAppUser] FOR LOGIN [NETACCOUNTS\CiWebAppUser] WITH DEFAULT_SCHEMA=[dbo]
GO
USE [ola_audit_trail]
GO
EXEC sp_addrolemember N'db_owner', N'NETACCOUNTS\CiWebAppUser'
GO
USE [ola_log]
GO
EXEC sp_addrolemember N'db_owner', N'NETACCOUNTS\CiWebAppUser'
GO
USE [ola_state]
GO
EXEC sp_addrolemember N'db_owner', N'NETACCOUNTS\CiWebAppUser'
GO
USE [ola_tran]
GO
EXEC sp_addrolemember N'db_owner', N'NETACCOUNTS\CiWebAppUser'
GO
USE [ola_audit_trail]
GO
EXEC sp_addrolemember N'db_owner', N'NETWORK SERVICE'
GO
USE [ola_log]
GO
EXEC sp_addrolemember N'db_owner', N'NETWORK SERVICE'
GO
USE [ola_state]
GO
EXEC sp_addrolemember N'db_owner', N'NETWORK SERVICE'
GO
USE [ola_tran]
GO
EXEC sp_addrolemember N'db_owner', N'NETWORK SERVICE'
GO

ALTER DATABASE [ola_tran] SET MULTI_USER