USE [PayMeBack]
GO
/****** Object:  Table [Payment].[UserPaymentPlan]    Script Date: 05/30/2012 07:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payment].[UserPaymentPlan](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_UserPaymentPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payment].[Debt]    Script Date: 05/30/2012 07:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payment].[Debt](
	[Id] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[UserPaymentPlanId] [uniqueidentifier] NOT NULL,
	[UserIdWhoOwesDebt] [uniqueidentifier] NOT NULL,
	[PaymentPeriod] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[ExpectedEndDate] [date] NULL,
	[InitialPayment] [money] NULL,
	[IsOutstanding] [bit] NULL,
	[TotalAmountOwed] [money] NOT NULL,
	[ReasonForDebt] [nvarchar](250) NULL,
	[Notes] [nvarchar](1024) NULL,
 CONSTRAINT [PK_Debt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payment].[DebtPaymentPlan]    Script Date: 05/30/2012 07:17:16 ******/
USE [PayMeBack]
GO

/****** Object:  Table [Payment].[DebtPaymentPlan]    Script Date: 05/30/2012 07:31:34 ******/
SET ANSI_NULLS ON
GO

ALTER TABLE [Payment].[Debt]  WITH CHECK ADD  CONSTRAINT [FK_Debt_UserDetail] FOREIGN KEY([UserIdWhoOwesDebt])
REFERENCES [Security].[UserDetail] ([Id])
GO

ALTER TABLE [Payment].[Debt] CHECK CONSTRAINT [FK_Debt_UserDetail]
GO

ALTER TABLE [Payment].[Debt]  WITH CHECK ADD  CONSTRAINT [FK_Debt_UserPaymentPlan] FOREIGN KEY([UserPaymentPlanId])
REFERENCES [Payment].[UserPaymentPlan] ([Id])
GO

ALTER TABLE [Payment].[Debt] CHECK CONSTRAINT [FK_Debt_UserPaymentPlan]
GO

ALTER TABLE [Payment].[Debt] ADD  CONSTRAINT [DF_Debt_IsOutstanding]  DEFAULT ((1)) FOR [IsOutstanding]
GO


/****** Object:  ForeignKey [FK_UserPaymentPlan_UserDetail]    Script Date: 05/30/2012 07:17:16 ******/
ALTER TABLE [Payment].[UserPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_UserPaymentPlan_UserDetail] FOREIGN KEY([UserId])
REFERENCES [Security].[UserDetail] ([Id])
GO
ALTER TABLE [Payment].[UserPaymentPlan] CHECK CONSTRAINT [FK_UserPaymentPlan_UserDetail]
GO


USE [PayMeBack]
GO

/****** Object:  Table [dbo].[DebtPaymentInstallments]    Script Date: 06/02/2012 16:29:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Payment].[DebtPaymentInstallments](
	[Id] [uniqueidentifier] NOT NULL,
	[DebtId] [uniqueidentifier] NOT NULL,
	[PaymentDate] [date] NOT NULL,
	[AmountPaid] [money] NOT NULL,
	[PaymentMethod] [int] NOT NULL,
 CONSTRAINT [PK_DebtPaymentInstallments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PayMeBack]
/****** Object:  Index [IX_DebtPaymentInstallments]    Script Date: 06/02/2012 16:29:24 ******/
CREATE NONCLUSTERED INDEX [IX_DebtPaymentInstallments] ON [Payment].[DebtPaymentInstallments] 
(
	[DebtId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [Payment].[DebtPaymentInstallments]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentInstallments_Debt] FOREIGN KEY([DebtId])
REFERENCES [Payment].[Debt] ([Id])
GO

ALTER TABLE [Payment].[DebtPaymentInstallments] CHECK CONSTRAINT [FK_DebtPaymentInstallments_Debt]
GO

ALTER TABLE [Payment].[DebtPaymentInstallments] ADD  CONSTRAINT [DF_DebtPaymentInstallments_Id]  DEFAULT (newid()) FOR [Id]
GO




USE [PayMeBack]
GO

/****** Object:  Table [dbo].[PaymentMethodType]    Script Date: 06/02/2012 16:38:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Payment].[PaymentMethodType](
	[Id] [int] NOT NULL,
	[PaymentMethod] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_PaymentMethodType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


insert into [Payment].[PaymentMethodType] ([Id], [PaymentMethod]) values (1,'Cash')
insert into [Payment].[PaymentMethodType] ([Id], [PaymentMethod]) values (2,'Bank Transfer')
insert into [Payment].[PaymentMethodType] ([Id], [PaymentMethod]) values (3,'Services')
insert into [Payment].[PaymentMethodType] ([Id], [PaymentMethod]) values (4,'Goods')
