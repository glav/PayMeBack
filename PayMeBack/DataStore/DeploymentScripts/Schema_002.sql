USE [PayMeBack]
GO
/****** Object:  Table [Security].[UserDetail]    Script Date: 05/30/2012 07:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Security].[UserDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[EmailAddress] [nvarchar](128) NOT NULL,
	[FirstNames] [nvarchar](128) NULL,
	[Surname] [nvarchar](128) NULL,
	[Password] [nvarchar](128) NULL,
	[IsValidated] [bit] NOT NULL,
	[MobilePhone] [nvarchar](20) NULL,
 CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ByEmail] ON [Security].[UserDetail] 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [Security].[OAuthToken]    Script Date: 05/30/2012 07:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Security].[OAuthToken](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccessToken] [nvarchar](128) NOT NULL,
	[AccessTokenExpiry] [datetime] NOT NULL,
	[RefreshToken] [nvarchar](128) NOT NULL,
	[RefreshTokenExpiry] [datetime] NOT NULL,
	[AssociatedUserId] [uniqueidentifier] NOT NULL,
	[Scope] [nvarchar](256) NULL,
 CONSTRAINT [PK_OAuthToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
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

/**** Search User Sproc ***/
USE [PayMeBack]
GO

/****** Object:  StoredProcedure [dbo].[usp_SearchUsers]    Script Date: 07/24/2012 18:06:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SearchUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_SearchUsers]
GO

USE [PayMeBack]
GO

/****** Object:  StoredProcedure [dbo].[usp_SearchUsers]    Script Date: 07/24/2012 18:06:17 ******/
USE [PayMeBack]
GO

/****** Object:  StoredProcedure [dbo].[usp_SearchUsers]    Script Date: 07/24/2012 18:06:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SearchUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_SearchUsers]
GO

USE [PayMeBack]
GO

/****** Object:  StoredProcedure [dbo].[usp_SearchUsers]    Script Date: 07/24/2012 18:06:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_SearchUsers]
	@searchCriteria as nvarchar(50),
	@pageNumber as int,
	@pageSize as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @startRow as int
declare @endRow as int
set @startRow = (@pageNumber-1) * @pageSize + 1;
set @endRow = @startRow + @pageSize - 1;

with Users as
(	
	select
		ROW_NUMBER() OVER(ORDER BY EmailAddress) AS 'RowNumber',
		[Id], [EmailAddress], [FirstNames], 
		[Surname], [Password], [IsValidated], [MobilePhone]
	from 
		[Security].[UserDetail]
	where
		([EmailAddress] like '%'+@searchCriteria+'%'
		or [FirstNames] like '%'+@searchCriteria+'%'
		or [Surname] like '%'+@searchCriteria+'%')
)

select 
	[Id], [EmailAddress], [FirstNames], 
	[Surname], [Password], [IsValidated], [MobilePhone]
 from Users
	where
		(RowNumber >= @startRow and RowNumber <= @endRow)
		
	order by [EmailAddress]
END

GO


