/* Script originally created by: GLAVTOP\Glav on 5/18/2012 11:14:30 AM */
USE [PayMeBack]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Security].[UserDetail](
	[Id] [UniqueIdentifier]  NOT NULL,
	[EmailAddress] [nvarchar](128) NOT NULL,
	[FirstNames] [nvarchar](128) NOT NULL,
	[Surname] [nvarchar](128) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PayMeBack]
CREATE NONCLUSTERED INDEX [IX_ByEmail] ON [Security].[UserDetail] 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
