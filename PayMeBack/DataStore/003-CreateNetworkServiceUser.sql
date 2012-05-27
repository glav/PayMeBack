/* Script originally created by: GLAVTOP\Glav on 5/18/2012 11:14:30 AM */
USE [PayMeBack]
GO

/****** Object:  Table [dbo].[OAuthToken]    Script Date: 05/18/2012 11:12:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  User [NETWORK SERVICE]    Script Date: 05/28/2012 07:49:39 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NETWORK SERVICE')
CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO

