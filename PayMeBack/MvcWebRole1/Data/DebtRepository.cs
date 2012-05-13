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


		public IEnumerable<Debt> GetAllPaymentPlansOwedToUser(Guid userId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Debt> GetAllPaymentPlansOwedByUser(Guid userId)
		{
			throw new NotImplementedException();
		}
	}
}