﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Core.Domain;
using Data = Glav.PayMeBack.Web.Data;
using Glav.PayMeBack.Web.Domain.Engines;
using Glav.CacheAdapter.Core;
using System.Transactions;
using Glav.PayMeBack.Web.Helpers;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain.Services
{
	public class PaymentPlanService : IPaymentPlanService
	{
		private IUserEngine _userEngine;
		private Data.ICrudRepository _crudRepository;
		private Data.IDebtRepository _debtRepository;
		private ICacheProvider _cacheProvider;
        private ICultureFormattingEngine _currencyEngine;
        private IAuthorisationEngine _authEngine;
        private IPaymentPlanEngine _planEngine;

		public PaymentPlanService(IUserEngine userEngine, Data.ICrudRepository crudRepository, Data.IDebtRepository debtRepository, 
                            ICacheProvider cacheProvider, ICultureFormattingEngine currencyEngine, 
                            IAuthorisationEngine authEngine, IPaymentPlanEngine planEngine)
		{
			_userEngine = userEngine;
			_crudRepository = crudRepository;
			_debtRepository = debtRepository;
			_cacheProvider = cacheProvider;
            _currencyEngine = currencyEngine;
            _authEngine = authEngine;
            _planEngine = planEngine;
		}

		public UserPaymentPlan GetPaymentPlan(Guid userId)
		{
            return _planEngine.GetPaymentPlan(userId);
		}

		public DataAccessResult AddDebtOwed(Guid userId, Debt debt)
		{
			var userPaymentPlan = GetPaymentPlan(userId);
			userPaymentPlan.DebtsOwedToMe.Add(debt);
			return UpdatePaymentPlan(userPaymentPlan);
		}

        public DataAccessResult AddPaymentInstallmentToPlan(Guid userId, DebtPaymentInstallment installment)
        {
            var paymentPlan = GetPaymentPlan(userId);
            var debt = paymentPlan.DebtsOwedToMe.Where(d => d.Id == installment.DebtId).FirstOrDefault();
            if (debt != null)
            {
                debt.PaymentInstallments.Add(installment);
            }
            return UpdatePaymentPlan(paymentPlan);
        }

		public DataAccessResult UpdatePaymentPlan(UserPaymentPlan usersPaymentPlan)
		{
			var result = new DataAccessResult();
            _planEngine.InvalidateCacheForAllUsersRelatedToPlan(usersPaymentPlan);

			using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
			{
				try
				{
					ValidatePlan(usersPaymentPlan);
					AdjustDebtAggregateValues(usersPaymentPlan);
					AddNewUsersIfRequired(usersPaymentPlan);
					_debtRepository.UpdateUserPaymentPlan(usersPaymentPlan.ToDataRecord());
					result.WasSuccessfull = true;
					scope.Complete();

				}
				catch (System.Data.Entity.Validation.DbEntityValidationException valEx)
				{
					// log it - return it
					return valEx.ToDataResult();
				}
				catch (Exception ex)
				{
					// log it - return it
					return ex.ToDataResult();
				}
			}
			return result;
		}


		/// <summary>
		/// If there are users (with email, first/last name) defined in the debt
		/// but they dont have a valid id, then we add a record into the system
		/// as unvalidated users. These are users defined in our systembut who
		/// cannot actually login. They may later join the system and that is when
		/// we convert to validated users and they can login.
		/// In addition, typically before a user can be notified via email or SMS
		/// they need to be validated. This requires that the user themselves login
		/// and verify they are ok with being added into the system and notified
		/// by our mechanisms. This way, people just cant go adding heaps of people who
		/// are spammed by our system. They will require an explicit OK before we
		/// notify them
		/// </summary>
		/// <param name="usersPaymentPlan"></param>
		private void AddNewUsersIfRequired(UserPaymentPlan usersPaymentPlan)
		{
			usersPaymentPlan.DebtsOwedToMe.ForEach(d =>
								{
									if (d.UserWhoOwesDebt.Id == Guid.Empty)
									{
                                        // If an empty Id has been passed in, we still need to check
                                        // if the user exists via their email address
                                        var existingUser = _userEngine.GetUserByEmail(d.UserWhoOwesDebt.EmailAddress);
                                        if (existingUser == null)
                                        {
                                            _userEngine.SaveOrUpdateUser(d.UserWhoOwesDebt);
                                        }
                                        else
                                        {
                                            d.UserWhoOwesDebt = existingUser;
                                        }
									}
								});
		}

		private void AdjustDebtAggregateValues(UserPaymentPlan usersPaymentPlan)
		{
			if (usersPaymentPlan.DebtsOwedToMe != null)
			{
				usersPaymentPlan
					.DebtsOwedToMe
					.ForEach(d =>
						{
							if (d.PaymentInstallments != null)
							{
								var totalPayedOff = d.InitialPayment;
								d.PaymentInstallments.ForEach(p =>
								{
									totalPayedOff += p.AmountPaid;
								});
								if (totalPayedOff == d.TotalAmountOwed)
								{
									d.IsOutstanding = false;
								}
							}
						});
			}
		}

		private void ValidatePlan(UserPaymentPlan usersPaymentPlan)
		{
			if (usersPaymentPlan == null)
			{
				throw new ArgumentException("Payment plan is empty");
			}

			//Ensure we have some user information associated with the debt
			if (usersPaymentPlan.User == null)
			{
				throw new ArgumentException("No user associated with this payment plan");
			}

			usersPaymentPlan.DebtsOwedToMe.ForEach(d =>
													{
														if (d.UserWhoOwesDebt == null)
														{
															throw new ArgumentException("No user information associated with a debt");
														}
													});

			// Ensure the amount of payments does not exceed the total debt.
			// If the debt has been paid in full, then ensure it is no longer
			// marked as outstanding
			if (usersPaymentPlan.DebtsOwedToMe != null)
			{
				usersPaymentPlan
					.DebtsOwedToMe
					.ForEach(d =>
							{
								if (d.PaymentInstallments != null)
								{
									var totalPayedOff = d.InitialPayment;
									d.PaymentInstallments.ForEach(p =>
												{
													totalPayedOff += p.AmountPaid;
												});
									if (totalPayedOff > d.TotalAmountOwed)
									{
										throw new ValidationException("Amount paid off exceeds total debt.");
									}
								}
							});
			}
		}

		public DebtSummary GetDebtSummaryForUser(Guid userId)
		{
			var summary = new DebtSummary();
			var paymentPlan = GetPaymentPlan(userId);

			paymentPlan.DebtsOwedToMe.ForEach(d =>
			{
				if (d.IsOutstanding)
				{
					summary.TotalAmountOwedToYou += d.AmountLeftOwing();
                    var amtOwing = d.AmountLeftOwing();
                    var lastAmtPaid = d.LastAmountPaid();
                    var debtItem = new DebtSummaryItem
                    {
                        Id = d.Id,
                        AmountOwing = amtOwing,
                        StartDate = d.StartDate,
                        LastAmountPaid = lastAmtPaid,
                        LastPaymentDate = d.LastPaymentDate(),
                        UserWhoOwesDebt = d.UserWhoOwesDebt
                    };
					summary.DebtsOwedToYou.Add(debtItem);
				}
			});
			paymentPlan.DebtsOwedToOthers.ForEach(d =>
			{
				if (d.IsOutstanding)
				{
					summary.TotalAmountYouOwe += d.AmountLeftOwing();
                    var amtOwing = d.AmountLeftOwing();
                    var lastAmtPaid = d.LastAmountPaid();
                    var debtItem = new DebtSummaryItem
                    {
                        Id = d.Id,
                        AmountOwing = amtOwing,
                        StartDate = d.StartDate,
                        LastAmountPaid = lastAmtPaid,
                        LastPaymentDate = d.LastPaymentDate(),
                        UserWhoOwesDebt = d.UserWhoOwesDebt
                    };
                    summary.DebtsYouOwe.Add(debtItem);
				}
			});

			return summary;
		}

		public DataAccessResult RemoveDebt(Guid userId, Guid debtId)
		{
			var result = new DataAccessResult();
			var user = _userEngine.GetUserById(userId);
			if (user == null)
			{
				result.Errors.Add("Invalid User");
				return result;
			}
			
			var usersPlan = GetPaymentPlan(user.Id);
			if (!_authEngine.EnsureUserCanModifyDebt(debtId, usersPlan))
			{
				result.Errors.Add("Insufficient access to remove debt or you do not manage the debt");
				return result;
			};

			try
			{
				using (var trx = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
				{
					_crudRepository.Delete<DebtPaymentInstallmentDetail>(p => p.DebtId == debtId);
					_crudRepository.Delete<DebtDetail>(d => d.Id == debtId);
					trx.Complete();
				}
				_cacheProvider.InvalidateCacheItem(_planEngine.GetCacheKeyForUserPaymentPlan(usersPlan.User.Id));
				result.WasSuccessfull = true;
			}
			catch (Exception ex)
			{
				result = ex.ToDataResult();
			}
			return result;
		}

	}
}