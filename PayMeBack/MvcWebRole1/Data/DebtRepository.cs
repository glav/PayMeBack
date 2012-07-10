using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Domain = Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Helpers;

namespace Glav.PayMeBack.Web.Data
{
	public class DebtRepository : IDebtRepository
	{
		public DebtDetail GetDebt(Guid debtId)
		{
			//TODO: Stub for now
			return new DebtDetail { Id = Guid.NewGuid(), Notes = "Dummy", ReasonForDebt = "test", TotalAmountOwed = 100 };
		}

		public void UpdateDebt(DebtDetail debt)
		{
			//TODO: Stub for now
		}

		public void DeleteDebt(Guid debtId)
		{
			//TODO: Stub for now
		}

		public UserPaymentPlanDetail GetUserPaymentPlan(Guid userId)
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				context.Configuration.LazyLoadingEnabled = false;

				var paymentPlan = (from plan in context.UserPaymentPlanDetails.Include("UserDetail").Include("DebtDetails").Include("DebtDetails.DebtPaymentInstallmentDetails")
									where plan.UserId == userId
									select plan).FirstOrDefault();

				if (paymentPlan != null)
				{
					return paymentPlan;
				}
				return new UserPaymentPlanDetail() { UserId = userId };
			}
		}

		public void UpdateUserPaymentPlan(UserPaymentPlanDetail userPaymentPlan)
		{
			using (var ctxt = new PayMeBackEntities())
			{
				//ctxt.UserPaymentPlanDetails.Attach(userPaymentPlan);
				ctxt.SetDataState<UserDetail>(userPaymentPlan.UserDetail,EntityState.Unchanged);
				
				if (userPaymentPlan.Id == Guid.Empty)
				{
					userPaymentPlan.Id = Guid.NewGuid();
					ctxt.UserPaymentPlanDetails.Add(userPaymentPlan);
					AssociateChildEntities(userPaymentPlan, ctxt);
				}
				else
				{
					// do we have to attach? userPaymentPlan to context
					AssociateChildEntities(userPaymentPlan, ctxt);
				    ctxt.SetDataState<UserPaymentPlanDetail>(userPaymentPlan,EntityState.Modified);
				}
				ctxt.SaveChanges();
			}
		}

		private static void AssociateChildEntities(UserPaymentPlanDetail userPaymentPlan, PayMeBackEntities ctxt)
		{
			foreach (var d in userPaymentPlan.DebtDetails)
			{
				d.UserPaymentPlanDetail = userPaymentPlan;
				ctxt.SetDataState<UserDetail>(d.UserDetail, EntityState.Unchanged);

				if (d.DebtPaymentInstallmentDetails != null)
				{
					foreach (var installment in d.DebtPaymentInstallmentDetails)
					{
						installment.DebtDetail = d;
						if (installment.DebtId == Guid.Empty)
						{
							installment.DebtId = d.Id;
						}
						if (installment.Id == Guid.Empty)
						{
							installment.Id = Guid.NewGuid();
							ctxt.DebtPaymentInstallmentDetails.Add(installment);
							//ctxt.SetDataState<DebtPaymentInstallmentDetail>(installment, EntityState.Added);
						}
						else
						{
							ctxt.DebtPaymentInstallmentDetails.Attach(installment);
						}
					}
				}

				if (d.StartDate == DateTime.MinValue)
				{
					d.StartDate = DateTime.UtcNow;
				}
				if (d.DateCreated == DateTime.MinValue)
				{
					d.DateCreated = DateTime.UtcNow;
				}
				if (d.UserPaymentPlanId == Guid.Empty)
				{
					d.UserPaymentPlanId = userPaymentPlan.Id;
				}
				if (d.Id == Guid.Empty)
				{
					d.Id = Guid.NewGuid();
					ctxt.DebtDetails.Add(d);
					d.IsOutstanding = true;
				}
				else
				{
				    // do we have to attach?
					ctxt.DebtDetails.Attach(d);
//				    ctxt.SetDataState<DebtDetail>(d, EntityState.Modified);
				}

			}
		}


		public IEnumerable<DebtDetail> GetAllPaymentPlansForUser(Guid userPaymentPlanId)
		{
			throw new NotImplementedException();
		}

	}
}