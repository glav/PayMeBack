using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public class DebtRepository : IDebtRepository
	{
		public IEnumerable<Debt> GetAllDebtsOwedByUser(Guid userId)
		{
			//TODO: Stub for now
			var debts = new List<Debt>();
			debts.Add(new Debt { Id = Guid.NewGuid(), Notes = "You owe", ReasonForDebt = "test", TotalAmountOwed = 100 });
			return debts;
		}

		public IEnumerable<Debt> GetAllDebtsOwedToUser(Guid userId)
		{
			//TODO: Stub for now
			var debts = new List<Debt>();
			debts.Add(new Debt { Id = Guid.NewGuid(), Notes = "They owe", ReasonForDebt = "test", TotalAmountOwed = 100 });
			return debts;
		}

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


		IEnumerable<DebtPaymentPlan> IDebtRepository.GetAllPaymentPlansOwedToUser(Guid userId)
		{
			//TODO: Stub for now
			var list = new List<DebtPaymentPlan>();
			list.Add(GetPaymentPlan(Guid.NewGuid()));
			list.Add(GetPaymentPlan(Guid.NewGuid()));
			return list;
		}

		IEnumerable<DebtPaymentPlan> IDebtRepository.GetAllPaymentPlansOwedByUser(Guid userId)
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
			dummyPlan.DebtOwed = new Debt { Id = Guid.NewGuid()};
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