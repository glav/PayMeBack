USE [PayMeBack]
GO

USE [master]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'PayMeBackDbUser')
DROP LOGIN [PayMeBackDbUser]
GO

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'PayMeBackDbUser')
CREATE LOGIN [PayMeBackDbUser] WITH PASSWORD=N'MyMoneyBack', DEFAULT_DATABASE=[PayMeBack], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=ON, CHECK_POLICY=ON
GO

USE [PayMeBack]
GO


/****** Object:  User [PayMeBackDbUser]    Script Date: 12/10/2012 7:15:19 AM ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'PayMeBackDbUser')
DROP USER [PayMeBackDbUser]
GO

/****** Object:  User [PayMeBackDbUser]    Script Date: 12/10/2012 7:15:19 AM ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'PayMeBackDbUser')
CREATE USER [PayMeBackDbUser] FOR LOGIN [PayMeBackDbUser] WITH DEFAULT_SCHEMA=[dbo]
GO




/****** Object:  User [NETWORK SERVICE]    Script Date: 05/30/2012 07:17:14 ******/
--CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Schema [Security]    Script Date: 05/30/2012 07:17:14 ******/
CREATE SCHEMA [Security] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [Payment]    Script Date: 05/30/2012 07:17:14 ******/
CREATE SCHEMA [Payment] AUTHORIZATION [dbo]
GO

USE [PayMeBack]
GO
EXEC sp_addrolemember N'db_owner', N'PayMeBackDbUser'
GO

