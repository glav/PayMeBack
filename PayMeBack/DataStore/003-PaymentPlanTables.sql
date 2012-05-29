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
	[UserId] [uniqueidentifier] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
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

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Payment].[DebtPaymentPlan](
	[Id] [uniqueidentifier] NOT NULL,
	[UserPaymentPlanId] [uniqueidentifier] NOT NULL,
	[DebtId] [uniqueidentifier] NOT NULL,
	[UserIdWhoOwesDebt] [uniqueidentifier] NOT NULL,
	[PaymentPeriod] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[ExpectedEndDate] [date] NULL,
	[InitialPayment] [money] NULL,
	[IsOutstanding] [bit] NULL,
 CONSTRAINT [PK_DebtPaymentPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Payment].[DebtPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentPlan_Debt] FOREIGN KEY([DebtId])
REFERENCES [Payment].[Debt] ([Id])
GO

ALTER TABLE [Payment].[DebtPaymentPlan] CHECK CONSTRAINT [FK_DebtPaymentPlan_Debt]
GO

ALTER TABLE [Payment].[DebtPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentPlan_UserDetail] FOREIGN KEY([UserIdWhoOwesDebt])
REFERENCES [Security].[UserDetail] ([Id])
GO

ALTER TABLE [Payment].[DebtPaymentPlan] CHECK CONSTRAINT [FK_DebtPaymentPlan_UserDetail]
GO

ALTER TABLE [Payment].[DebtPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentPlan_UserPaymentPlan] FOREIGN KEY([UserPaymentPlanId])
REFERENCES [Payment].[UserPaymentPlan] ([Id])
GO

ALTER TABLE [Payment].[DebtPaymentPlan] CHECK CONSTRAINT [FK_DebtPaymentPlan_UserPaymentPlan]
GO

ALTER TABLE [Payment].[DebtPaymentPlan] ADD  CONSTRAINT [DF_DebtPaymentPlan_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [Payment].[DebtPaymentPlan] ADD  CONSTRAINT [DF_DebtPaymentPlan_IsOutstanding]  DEFAULT ((1)) FOR [IsOutstanding]
GO


/****** Object:  ForeignKey [FK_UserPaymentPlan_UserDetail]    Script Date: 05/30/2012 07:17:16 ******/
ALTER TABLE [Payment].[UserPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_UserPaymentPlan_UserDetail] FOREIGN KEY([UserId])
REFERENCES [Security].[UserDetail] ([Id])
GO
ALTER TABLE [Payment].[UserPaymentPlan] CHECK CONSTRAINT [FK_UserPaymentPlan_UserDetail]
GO
/****** Object:  ForeignKey [FK_Debt_UserDetail]    Script Date: 05/30/2012 07:17:16 ******/
ALTER TABLE [Payment].[Debt]  WITH CHECK ADD  CONSTRAINT [FK_Debt_UserDetail] FOREIGN KEY([UserId])
REFERENCES [Security].[UserDetail] ([Id])
GO
ALTER TABLE [Payment].[Debt] CHECK CONSTRAINT [FK_Debt_UserDetail]
GO
/****** Object:  ForeignKey [FK_DebtPaymentPlan_Debt]    Script Date: 05/30/2012 07:17:16 ******/
ALTER TABLE [Payment].[DebtPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentPlan_Debt] FOREIGN KEY([DebtId])
REFERENCES [Payment].[Debt] ([Id])
GO
ALTER TABLE [Payment].[DebtPaymentPlan] CHECK CONSTRAINT [FK_DebtPaymentPlan_Debt]
GO
/****** Object:  ForeignKey [FK_DebtPaymentPlan_UserPaymentPlan]    Script Date: 05/30/2012 07:17:16 ******/
ALTER TABLE [Payment].[DebtPaymentPlan]  WITH CHECK ADD  CONSTRAINT [FK_DebtPaymentPlan_UserPaymentPlan] FOREIGN KEY([UserPaymentPlanId])
REFERENCES [Payment].[UserPaymentPlan] ([Id])
GO
ALTER TABLE [Payment].[DebtPaymentPlan] CHECK CONSTRAINT [FK_DebtPaymentPlan_UserPaymentPlan]
GO
