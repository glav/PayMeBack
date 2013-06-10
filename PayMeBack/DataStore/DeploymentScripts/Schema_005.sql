/*** Notification Options ****/


USE [PayMeBack]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Payment].[DF_NotificationOptionsDetail_ReminderIntervalFrequency]') AND type = 'D')
BEGIN
ALTER TABLE [Payment].[NotificationOptionsDetail] DROP CONSTRAINT [DF_NotificationOptionsDetail_ReminderIntervalFrequency]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Payment].[DF_Payment.NotificationOptionsDetail_Method]') AND type = 'D')
BEGIN
ALTER TABLE [Payment].[NotificationOptionsDetail] DROP CONSTRAINT [DF_Payment.NotificationOptionsDetail_Method]
END

GO

/****** Object:  Table [Payment].[NotificationOptions]    Script Date: 6/02/2013 7:24:16 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Payment].[NotificationOptionsDetail]') AND type in (N'U'))
DROP TABLE [Payment].[NotificationOptionsDetail]
GO

/****** Object:  Table [Payment].[NotificationOptions]    Script Date: 6/02/2013 7:24:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Payment].[NotificationOptionsDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Payment].[NotificationOptionsDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DebtId] [uniqueidentifier] NOT NULL,
	[Method] [int] NOT NULL,
	[EmailAddress] [nvarchar](256) NULL,
	[SmsPhoneNumber] [nvarchar](50) NULL,
	[ReminderIntervalFrequency] [int] NOT NULL,
	[ReminderIntervalCount] [int] NULL,
	[ReminderIntervalDayOfWeek] [int] NULL,
	[ReminderTime] [time](7) NULL,
 CONSTRAINT [PK_Payment.NotificationOptionsDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Payment].[DF_Payment.NotificationOptionsDetail_Method]') AND type = 'D')
BEGIN
ALTER TABLE [Payment].[NotificationOptionsDetail] ADD  CONSTRAINT [DF_Payment.NotificationOptionsDetail_Method]  DEFAULT ((0)) FOR [Method]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[Payment].[DF_NotificationOptionsDetail_ReminderIntervalFrequency]') AND type = 'D')
BEGIN
ALTER TABLE [Payment].[NotificationOptionsDetail] ADD  CONSTRAINT [DF_NotificationOptionsDetail_ReminderIntervalFrequency]  DEFAULT ((0)) FOR [ReminderIntervalFrequency]
END

GO


/**** And the indexes ******/

USE [PayMeBack]
GO

/****** Object:  Index [IX_ByUserByDebt]    Script Date: 6/02/2013 7:26:37 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Payment].[NotificationOptionsDetail]') AND name = N'IX_ByUserByDebt')
DROP INDEX [IX_ByUserByDebt] ON [Payment].[NotificationOptionsDetail]
GO

/****** Object:  Index [IX_ByUserByDebt]    Script Date: 6/02/2013 7:26:37 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Payment].[NotificationOptionsDetail]') AND name = N'IX_ByUserByDebt')
CREATE NONCLUSTERED INDEX [IX_ByUserByDebt] ON [Payment].[NotificationOptionsDetail]
(
	[UserId] ASC,
	[DebtId] ASC
)
INCLUDE ( 	[Id],
	[Method],
	[EmailAddress],
	[SmsPhoneNumber],
	[ReminderIntervalFrequency],
	[ReminderIntervalCount],
	[ReminderIntervalDayOfWeek]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


