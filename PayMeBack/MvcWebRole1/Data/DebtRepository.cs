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

				var paymentPlan = (from plan in context.UserPaymentPlanDetails
									where plan.UserId == userId
									select new {Plan = plan, User=plan.UserDetail, Debts = plan.DebtDetails}).FirstOrDefault();

				if (paymentPlan != null)
				{
					return paymentPlan.Plan;
				}
				return new UserPaymentPlanDetail() { UserId = userId };
			}
		}

		public void UpdateUserPaymentPlan(UserPaymentPlanDetail userPaymentPlan)
		{
			using (var ctxt = new PayMeBackEntities())
			{
				ctxt.SetDataState<UserDetail>(userPaymentPlan.UserDetail,EntityState.Unchanged);
				
				foreach (var d in userPaymentPlan.DebtDetails)
				{
					ctxt.SetDataState<UserDetail>(d.UserDetail, EntityState.Unchanged);

					if (d.StartDate == DateTime.MinValue)
					{
						d.StartDate = DateTime.UtcNow;
					}
					if (d.DateCreated == DateTime.MinValue)
					{
						d.DateCreated = DateTime.UtcNow;
					}
					if (d.Id == Guid.Empty)
					{
						d.Id = Guid.NewGuid();
						d.IsOutstanding = true;
					}
				}
				if (userPaymentPlan.Id == Guid.Empty)
				{
					userPaymentPlan.Id = Guid.NewGuid();
					ctxt.UserPaymentPlanDetails.Add(userPaymentPlan);
				}
				else
				{
					ctxt.SetDataState<UserPaymentPlanDetail>(userPaymentPlan,EntityState.Modified);
				}
				ctxt.SaveChanges();
			}
		}


		public IEnumerable<DebtDetail> GetAllPaymentPlansForUser(Guid userPaymentPlanId)
		{
			throw new NotImplementedException();
		}

	}
}