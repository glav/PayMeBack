/* Script originally created by: GLAVTOP\Glav on 5/18/2012 11:14:30 AM */
USE [PayMeBack]
GO

/****** Object:  Table [dbo].[OAuthToken]    Script Date: 05/18/2012 11:12:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create schema [Security]

CREATE TABLE [Security].[OAuthToken](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccessToken] [nvarchar](128) NOT NULL,
	[AccessTokenExpiry] [datetime] NOT NULL,
	[RefreshToken] [nvarchar](128) NOT NULL,
	[RefreshTokenExpiry] [datetime] NOT NULL,
	[AssociatedUserId] [UniqueIdentifier] NOT NULL,
	[Scope] [nvarchar](256) NULL,
 CONSTRAINT [PK_OAuthToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PayMeBack]
/****** Object:  Index [IX_ByToken]    Script Date: 05/18/2012 11:12:53 ******/
CREATE NONCLUSTERED INDEX [IX_ByAccessToken] ON [Security].[OAuthToken] 
(
	[AccessToken] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_ByRefreshToken] ON [Security].[OAuthToken] 
(
	[RefreshToken] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


