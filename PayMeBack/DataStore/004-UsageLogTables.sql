USE [PayMeBack]
GO

/****** Object:  Table [dbo].[UsageLog]    Script Date: 06/30/2012 13:38:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsageLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IsRequest] [bit] NOT NULL,
	[TimeOfOperation] [datetime] NOT NULL,
	[HttpMethod] [nchar](10) NOT NULL,
	[ClientIdentifier] [nvarchar](128) NULL,
	[Uri] [nvarchar](1024) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[StatusCode] [int] NULL,
 CONSTRAINT [PK_UsageLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PayMeBack]
/****** Object:  Index [IX_ClientByDateByType]    Script Date: 06/30/2012 13:38:18 ******/
CREATE NONCLUSTERED INDEX [IX_ClientByDateByType] ON [dbo].[UsageLog] 
(
	[ClientIdentifier] ASC,
	[TimeOfOperation] ASC,
	[IsRequest] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UsageLog] ADD  CONSTRAINT [DF_UsageLog_IsRequest]  DEFAULT ((1)) FOR [IsRequest]
GO

ALTER TABLE [dbo].[UsageLog] ADD  CONSTRAINT [DF_UsageLog_TimeOfOperation]  DEFAULT (getdate()) FOR [TimeOfOperation]
GO


