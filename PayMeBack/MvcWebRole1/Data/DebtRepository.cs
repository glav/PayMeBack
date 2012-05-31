using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain = Glav.PayMeBack.Web.Domain;
using Glav.PayMeBack.Web.Helpers;

namespace Glav.PayMeBack.Web.Data
{
	public class DebtRepository : IDebtRepository
	{
		public Debt GetDebt(Guid debtId)
		{
			//TODO: Stub for now
			return new Debt { Id = Guid.NewGuid(), Notes = "Dummy", ReasonForDebt = "test", TotalAmountOwed = 100 };
		}

		public void UpdateDebt(Debt debt)
		{
			//TODO: Stub for now
		}

		public void DeleteDebt(Guid debtId)
		{
			//TODO: Stub for now
		}

		public UserPaymentPlan GetUserPaymentPlan(Guid userId)
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				context.Configuration.LazyLoadingEnabled = false;

				var paymentPlan = (from plan in context.UserPaymentPlans
									where plan.UserId == userId
									select plan).FirstOrDefault();

				if (paymentPlan != null)
				{
					return paymentPlan;
				}
				return new UserPaymentPlan();
			}
		}

		public void UpdateUserPaymentPlan(UserPaymentPlan userPaymentPlan)
		{
			using (var ctxt = new PayMeBackEntities())
			{
				if (userPaymentPlan.Id == Guid.Empty)
				{
					ctxt.UserPaymentPlans.Add(userPaymentPlan);
				}
				else
				{
					var dbEntry = ctxt.Entry<UserPaymentPlan>(userPaymentPlan);
					dbEntry.State = System.Data.EntityState.Modified;
				}
				ctxt.SaveChanges();
			}
			throw new NotImplementedException();
		}


		public IEnumerable<Debt> GetAllPaymentPlansForUser(Guid userPaymentPlanId)
		{
			throw new NotImplementedException();
		}
	}
}