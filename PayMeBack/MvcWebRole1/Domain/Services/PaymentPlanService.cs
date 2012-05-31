using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data = Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.CacheAdapter.Core;
using System.Transactions;
using Glav.PayMeBack.Web.Helpers;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class PaymentPlanService : IPaymentPlanService
	{
		private IUserEngine _userEngine;
		private Data.ICrudRepository _crudRepository;
		private Data.IDebtRepository _debtRepository;
		private ICacheProvider _cacheProvider;
		private const string UserPaymentPlanCacheKey = "UserPaymentPlan_{0}";

		public PaymentPlanService(IUserEngine userEngine, Data.ICrudRepository crudRepository, Data.IDebtRepository debtRepository, ICacheProvider cacheProvider)
		{
			_userEngine = userEngine;
			_crudRepository = crudRepository;
			_debtRepository = debtRepository;
			_cacheProvider = cacheProvider;
		}

		public UserPaymentPlan GetPaymentPlan(Guid userId)
		{
			var paymentPlanDetail = _cacheProvider.Get<Data.UserPaymentPlan>(GetCacheKeyForUserPaymentPlan(userId), DateTime.Now.AddHours(1), () =>
				{
					return _debtRepository.GetUserPaymentPlan(userId);
				});

			var paymentPlan = new UserPaymentPlan();
			if (paymentPlanDetail == null || paymentPlanDetail.Id == Guid.Empty)
			{
				paymentPlan.User = new User(_crudRepository.GetSingle<Data.UserDetail>(u => u.Id == userId));
				paymentPlan.DebtsOwedToMe = new List<Debt>();
				paymentPlan.DebtsOwedToOthers = new List<Debt>();
				paymentPlan.Id = Guid.Empty;
				return paymentPlan;
			}

			paymentPlan.Id = paymentPlanDetail.Id;
			paymentPlan.User = new Domain.User(paymentPlanDetail.UserDetail);

			paymentPlan.DebtsOwedToMe = GetDebtsRelatedToUser(userId, paymentPlanDetail, true);
			paymentPlan.DebtsOwedToOthers = GetDebtsRelatedToUser(userId, paymentPlanDetail, false);
			return paymentPlan;
		}

		private string GetCacheKeyForUserPaymentPlan(Guid userId)
		{
			return string.Format(UserPaymentPlanCacheKey, userId.ToString());
		}

		private List<Debt> GetDebtsRelatedToUser(Guid userId, Data.UserPaymentPlan paymentPlanDetail, bool debtsOwedToUser)
		{
			var debts = new List<Debt>();
			if (paymentPlanDetail.Debts != null && paymentPlanDetail.Debts.Count > 0)
			{
				paymentPlanDetail.Debts.ToList().ForEach(p =>
				{
					if ((p.UserIdWhoOwesDebt != userId && debtsOwedToUser) || (p.UserIdWhoOwesDebt == userId && !debtsOwedToUser))
					{
						debts.Add(new Debt(p));
					}
				});
			}

			return debts;
		}

		public void AddDebtOwed(Guid userId, Debt debt)
		{
			var userPaymentPlan = GetPaymentPlan(userId);
			userPaymentPlan.DebtsOwedToMe.Add(new Debt()
				{
					UserWhoOwesDebt = userPaymentPlan.User,
				});
			UpdatePaymentPlan(userPaymentPlan);
		}

		public void AddDebtOwing(Guid userId, Debt debt)
		{
			var userPaymentPlan = GetPaymentPlan(userId);
			userPaymentPlan.DebtsOwedToOthers.Add(new Debt()
			{
				UserWhoOwesDebt = userPaymentPlan.User,
			});
			UpdatePaymentPlan(userPaymentPlan);
		}

		public void UpdatePaymentPlan(UserPaymentPlan usersPaymentPlan)
		{
			_cacheProvider.InvalidateCacheItem(GetCacheKeyForUserPaymentPlan(usersPaymentPlan.User.Id));

			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
			{
				var existingPlan = _debtRepository.GetUserPaymentPlan(usersPaymentPlan.User.Id);
				if (existingPlan.Debts != null)
				{
					usersPaymentPlan.DebtsOwedToMe.ForEach(d =>
					{
						var foundPlan = existingPlan.Debts.Where(p => p.Id == d.Id).FirstOrDefault();
						if (foundPlan == null)
						{
							existingPlan.Debts.Add(d.ToDataRecord());
						}
						else
						{
							foundPlan = d.ToDataRecord();
						}
					});
					usersPaymentPlan.DebtsOwedToOthers.ForEach(d =>
					{
						var foundPlan = existingPlan.Debts.Where(p => p.Id == d.Id).FirstOrDefault();
						if (foundPlan == null)
						{
							existingPlan.Debts.Add(d.ToDataRecord());
						}
						else
						{
							foundPlan = d.ToDataRecord();
						}
					});
				}
				else
				{
					usersPaymentPlan.DebtsOwedToMe.ForEach(d =>
						{
							_crudRepository.Insert<Data.Debt>(d.ToDataRecord());
						});
				}
				_debtRepository.UpdateUserPaymentPlan(existingPlan);
			}
		}

		public void RemoveDebt(Guid userId, Debt debt)
		{
			throw new NotImplementedException();
		}
	}
}