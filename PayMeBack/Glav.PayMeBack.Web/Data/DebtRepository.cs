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

                var user = context.UserDetails.First(u => u.Id == userId);

				var paymentPlan = (from plan in context.UserPaymentPlanDetails.Include("UserDetail").Include("DebtDetails").Include("DebtDetails.UserDetail").Include("DebtDetails.DebtPaymentInstallmentDetails")
								   where plan.UserId == userId
								   select plan).FirstOrDefault();

				var debtsOwed = (from d in context.DebtDetails.Include("UserDetail").Include("DebtPaymentInstallmentDetails")
								 where d.UserIdWhoOwesDebt == userId
                                 || d.UserDetail.EmailAddress == user.EmailAddress
								 select d).ToList();

				if (paymentPlan == null)
				{
					paymentPlan = new UserPaymentPlanDetail() { UserId = userId };
					paymentPlan.DebtDetails = new List<DebtDetail>();
				}

				debtsOwed.ForEach(d => paymentPlan.DebtDetails.Add(d));
				return paymentPlan;
			}
		}

		public void UpdateUserPaymentPlan(UserPaymentPlanDetail userPaymentPlan)
		{
			using (var ctxt = new PayMeBackEntities())
			{
				ctxt.SetDataState<UserDetail>(userPaymentPlan.UserDetail, EntityState.Unchanged);

				if (userPaymentPlan.Id == Guid.Empty)
				{
					userPaymentPlan.Id = Guid.NewGuid();
					ctxt.UserPaymentPlanDetails.Add(userPaymentPlan);
					AssociateChildEntities(userPaymentPlan, ctxt);
				}
				else
				{
					AssociateChildEntities(userPaymentPlan, ctxt);
					ctxt.SetDataState<UserPaymentPlanDetail>(userPaymentPlan, EntityState.Modified);
				}
				ctxt.SaveChanges();
			}
		}

		private static void AssociateChildEntities(UserPaymentPlanDetail userPaymentPlan, PayMeBackEntities ctxt)
		{
			foreach (var d in userPaymentPlan.DebtDetails)
			{
				if (d.UserIdWhoOwesDebt == userPaymentPlan.UserId)
				{
					// do not update debts not controlled by the user
					// ie. what they owe to others
					continue;
				}

				ctxt.SetDataState<UserDetail>(d.UserDetail, EntityState.Unchanged);

				// Ensure our debt record has all its referential integrity and
				// necessary fields set
				d.UserPaymentPlanDetail = userPaymentPlan;

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

				//Now populate each debt payment installment with the required
				//relationship fields
				if (d.DebtPaymentInstallmentDetails != null)
				{
					d.DebtPaymentInstallmentDetails.ToList().ForEach(i =>
																		{
																			i.DebtDetail = d;
																			i.DebtId = d.Id;
																		});

					foreach (var installment in d.DebtPaymentInstallmentDetails)
					{
						if (installment.Id == Guid.Empty)
						{
							ctxt.DebtPaymentInstallmentDetails.Add(installment);
							installment.Id = Guid.NewGuid();
						}
						else
						{
							//ctxt.DebtPaymentInstallmentDetails.Attach(installment);
							ctxt.SetDataState<DebtPaymentInstallmentDetail>(installment, EntityState.Modified);
						}
					}
				}

				// Finally attach the parent to the context to prep for updating
				if (d.Id == Guid.Empty)
				{
					//d.Id = Guid.NewGuid();
					ctxt.DebtDetails.Add(d);
				    d.Id = Guid.NewGuid();
					d.IsOutstanding = true;
				}
				else
				{
					// do we have to attach?
					//ctxt.DebtDetails.Attach(d);
					ctxt.SetDataState<DebtDetail>(d, EntityState.Modified);
				}

			}
		}


		public IEnumerable<DebtDetail> GetAllPaymentPlansForUser(Guid userPaymentPlanId)
		{
			throw new NotImplementedException();
		}

	}
}