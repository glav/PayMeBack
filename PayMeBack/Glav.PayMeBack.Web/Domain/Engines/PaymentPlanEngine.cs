using Glav.CacheAdapter.Core;
using Glav.PayMeBack.Core.Domain;
using Glav.PayMeBack.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Domain.Engines
{
    public class PaymentPlanEngine : IPaymentPlanEngine
    {
        private ICacheProvider _cacheProvider;
        private Data.IDebtRepository _debtRepository;
        private ICrudRepository _crudRepository;
        private const string UserPaymentPlanCacheKey = "UserPaymentPlan_{0}";


        public PaymentPlanEngine(ICacheProvider cacheProvider, Data.IDebtRepository debtRepository, ICrudRepository crudRepository)
        {
            _cacheProvider = cacheProvider;
            _debtRepository = debtRepository;
            _crudRepository = crudRepository;
        }

        public UserPaymentPlan GetPaymentPlan(Guid userId)
        {
            var paymentPlanDetail = _cacheProvider.Get<UserPaymentPlanDetail>(GetCacheKeyForUserPaymentPlan(userId), DateTime.Now.AddHours(1), () =>
            {
                return _debtRepository.GetUserPaymentPlan(userId);
            });

            var paymentPlan = new UserPaymentPlan();
            if (paymentPlanDetail == null || paymentPlanDetail.Id == Guid.Empty)
            {
                paymentPlan.User = _crudRepository.GetSingle<Data.UserDetail>(u => u.Id == userId).ToModel();
                paymentPlan.DebtsOwedToMe = new List<Debt>();
                paymentPlan.DebtsOwedToOthers = new List<Debt>();
                paymentPlan.Id = Guid.Empty;
                paymentPlan.DateCreated = DateTime.UtcNow;
            }
            else
            {
                paymentPlan.Id = paymentPlanDetail.Id;
                paymentPlan.User = paymentPlanDetail.UserDetail.ToModel();
                paymentPlan.DateCreated = paymentPlanDetail.DateCreated;
            }

            paymentPlan.DebtsOwedToMe = GetDebtsRelatedToUser(userId, paymentPlanDetail, true);
            paymentPlan.DebtsOwedToOthers = GetDebtsRelatedToUser(userId, paymentPlanDetail, false);
            return paymentPlan;
        }

        public string GetCacheKeyForUserPaymentPlan(Guid userId)
        {
            return string.Format(UserPaymentPlanCacheKey, userId.ToString());
        }


        private List<Debt> GetDebtsRelatedToUser(Guid userId, UserPaymentPlanDetail paymentPlanDetail, bool debtsOwedToUser)
        {
            var debts = new List<Debt>();
            if (paymentPlanDetail != null && paymentPlanDetail.DebtDetails != null && paymentPlanDetail.DebtDetails.Count > 0)
            {
                paymentPlanDetail.DebtDetails.ToList().ForEach(p =>
                {
                    if ((p.UserIdWhoOwesDebt != userId && debtsOwedToUser) || (p.UserIdWhoOwesDebt == userId && !debtsOwedToUser))
                    {
                        debts.Add(p.ToModel());
                    }
                });
            }

            return debts;
        }

        /// <summary>
        /// We invalidate the cache for the user making the change but also for all
        /// users related to this user who owe this user money so that their plan
        /// is not in cache and they get an updated copy when it is requested
        /// </summary>
        /// <param name="usersPaymentPlan"></param>
        public void InvalidateCacheForAllUsersRelatedToPlan(UserPaymentPlan usersPaymentPlan)
        {
            _cacheProvider.InvalidateCacheItem(GetCacheKeyForUserPaymentPlan(usersPaymentPlan.User.Id));
            usersPaymentPlan.DebtsOwedToMe.ForEach(d =>
            {
                if (d.UserWhoOwesDebt != null)
                {
                    _cacheProvider.InvalidateCacheItem(GetCacheKeyForUserPaymentPlan(d.UserWhoOwesDebt.Id));
                }
            });

        }

    }
}