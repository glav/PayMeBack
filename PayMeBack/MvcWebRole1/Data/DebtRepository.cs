using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain = Glav.PayMeBack.Web.Domain;

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

		IEnumerable<DebtPaymentPlan> IDebtRepository.GetAllPaymentPlansForUser(Guid userPaymentPlanId)
		{
			//TODO: Stub for now
			var list = new List<DebtPaymentPlan>();
			list.Add(GetPaymentPlan(Guid.NewGuid()));
			list.Add(GetPaymentPlan(Guid.NewGuid()));
			return list;
		}

		public DebtPaymentPlan GetPaymentPlan(Guid paymentPlanId)
		{
			//TODO: Stub for now
			var dummyPlan = new DebtPaymentPlan();
			dummyPlan.Id = Guid.NewGuid();
			dummyPlan.Debt = new Debt { Id = Guid.NewGuid() };
			return dummyPlan;
		}

		public void UpdatePaymentPlan(DebtPaymentPlan paymentPlan)
		{
			//TODO: Stub for now
		}

		public void DeletePaymentPlan(Guid paymentPlanId)
		{
			//TODO: Stub for now
		}
	}
}