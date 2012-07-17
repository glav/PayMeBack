use PayMeBack

--select * from dbo.UsageLog order by Id desc

--truncate table dbo.UsageLog

select * from Payment.UserPaymentPlanDetail
select * from Payment.DebtDetail
select * from Payment.DebtPaymentInstallmentDetail
select * from [Security].UserDetail
/*

delete from Payment.DebtPaymentInstallmentDetail
delete from Payment.DebtDetail
delete from Payment.UserPaymentPlanDetail
delete from [Security].UserDetail

*/
