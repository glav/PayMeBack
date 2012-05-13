using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Domain;

namespace Glav.PayMeBack.Web.Data
{
	public interface IDebtRepository
	{
		IEnumerable<Debt> GetAllDebtsOwedToUser(Guid userId);
		IEnumerable<Debt> GetAllDebtsOwedByUser(Guid userId);
		IEnumerable<Debt> GetAllPaymentPlansOwedToUser(Guid userId);
		IEnumerable<Debt> GetAllPaymentPlansOwedByUser(Guid userId);
		Debt GetDebt(Guid debtId);
		void UpdateDebt(Debt debt);
		void DeleteDebt(Guid debtId);
	}
}