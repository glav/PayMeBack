USE [PayMeBack]
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
EXEC sp_addrolemember N'db_owner', N'NETWORK SERVICE'
GO

