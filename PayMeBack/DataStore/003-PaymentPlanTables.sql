USE [PayMeBack]
GO
/****** Object:  Table [Payment].[UserPaymentPlanDetail]    Script Date: 05/30/2012 07:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Payment].[UserPaymentPlanDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_UserPaymentPlanDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Payment].[DebtDetail]    Script Date: 05/30/2012 07:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Payment].[DebtDetail](
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

ALTER TABLE [Payment].[DebtDetail]  WITH CHECK ADD  CONSTRAINT [FK_Debt_UserDetail] FOREIGN KEY([UserIdWhoOwesDebt])
REFERENCES [Security].[UserDetail] ([Id])
GO

ALTER TABLE [Payment].[DebtDetail] CHECK CONSTRAINT [FK_Debt_UserDetail]
GO

ALTER TABLE [Payment].[DebtDetail]  WITH CHECK ADD  CONSTRAINT [FK_Debt_UserPaymentPlanDetail] FOREIGN KEY([UserPaymentPlanId])
REFERENCES [Payment].[UserPaymentPlanDetail] ([Id])
GO

ALTER TABLE [Payment].[DebtDetail] CHECK CONSTRAINT [FK_Debt_UserPaymentPlanDetail]
GO

ALTER TABLE [Payment].[DebtDetail] ADD  CONSTRAINT [DF_Debt_IsOutstanding]  DEFAULT ((1)) FOR [IsOutstanding]
GO


/****** Object:  ForeignKey [FK_UserPaymentPlan_UserDetail]    Script Date: 05/30/2012 07:17:16 ******/
ALTER TABLE [Payment].[UserPaymentPlanDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserPaymentPlanDetail_UserDetail] FOREIGN KEY([UserId])
REFERENCES [Security].[UserDetail] ([Id])
GO
ALTER TABLE [Payment].[UserPaymentPlanDetail] CHECK CONSTRAINT [FK_UserPaymentPlanDetail_UserDetail]
GO


USE [PayMeBack]
GO

/****** Object:  Table [dbo].[DebtPaymentInstallmentDetail]    Script Date: 06/02/2012 16:29:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Payment].[DebtPaymentInstallmentDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[DebtId] [uniqueidentifier] NOT NULL,
	[PaymentDate] [date] NOT NULL,
	[AmountPaid] [money] NOT NULL,
	[PaymentMethod] [int] NOT NULL,
 CONSTRAINT [PK_DebtPaymentInstallmentDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PayMeBack]
/****** Object:  Index [IX_DebtPaymentInstallmentDetail]    Script Date: 06/02/2012 16:29:24 ******/
CREATE NONCLUSTERED INDEX [IX_DebtPaymentInstallmentDetail] ON [Payment].[DebtPaymentInstallmentDetail] 
(
	[DebtId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [Payment].[DebtPaymentInstallmentDetail]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentInstallmentDetail_Debt] FOREIGN KEY([DebtId])
REFERENCES [Payment].[DebtDetail] ([Id])
GO

ALTER TABLE [Payment].[DebtPaymentInstallmentDetail] CHECK CONSTRAINT [FK_DebtPaymentInstallmentDetail_Debt]
GO

ALTER TABLE [Payment].[DebtPaymentInstallmentDetail] ADD  CONSTRAINT [DF_DebtPaymentInstallmentDetail_Id]  DEFAULT (newid()) FOR [Id]
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
