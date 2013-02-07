using Glav.PayMeBack.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Helpers;
using Glav.PayMeBack.Web.Domain.Engines;

namespace Glav.PayMeBack.Web.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private ICrudRepository _crudRepository;
        private IPaymentPlanEngine _planEngine;
        private IAuthorisationEngine _authEngine;

        public NotificationService(ICrudRepository crudRepository, IPaymentPlanEngine planEngine, IAuthorisationEngine authEngine)
        {
            _crudRepository = crudRepository;
            _planEngine = planEngine;
            _authEngine = authEngine;
        }
        public Core.Domain.NotificationOptions GetNotificationOptionsForUserDebt(Guid userId, Guid debtId)
        {
            var optionRecord = _crudRepository.GetSingle<NotificationOptionsDetail>(r => r.UserId == userId && r.DebtId == debtId);
            if (optionRecord != null)
            {
                return optionRecord.ToModel();
            }

            // return default record
            return new Core.Domain.NotificationOptions
            {
                UserId = userId,
                DebtId=debtId,
                NotificationMethod = Core.Domain.NotificationType.None,
                NotificationEmailAddress = string.Empty,
                Interval = new Core.Domain.ReminderInterval()
            };
        }

        public Data.DataAccessResult UpdateNotificationOptionsForUserDebt(Core.Domain.NotificationOptions notifyOptions)
        {
            var result = ValidateUserAndDebt(notifyOptions.UserId, notifyOptions.DebtId);
            if (!result.WasSuccessfull)
            {
                return result;
            }

            var record = notifyOptions.ToDataRecord();
            try
            {
                if (record.Id != Guid.Empty)
                {
                    _crudRepository.Update<NotificationOptionsDetail>(record);
                }
                else
                {
                    record.Id = Guid.NewGuid();
                    _crudRepository.Insert<NotificationOptionsDetail>(record);
                }
                result.WasSuccessfull = true;
            }
            catch (Exception ex)
            {
                result.WasSuccessfull = false;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        private DataAccessResult ValidateUserAndDebt(Guid userId, Guid debtId)
        {
            var result = new DataAccessResult { WasSuccessfull = false };
            var debtExists = _crudRepository.GetSingle<DebtDetail>(d => d.Id == debtId) != null;
            if (!debtExists)
            {
                result.Errors.Add("Debt does not exist");
                return result;
            }
            var user = _crudRepository.GetSingle<UserDetail>(u => u.Id == userId);
            if (user ==null)
            {
                result.Errors.Add("User does not exist");
                return result;
            }

            var paymentPlan = _planEngine.GetPaymentPlan(userId);
            var canModify = _authEngine.EnsureUserCanModifyDebt(debtId,paymentPlan);
            if (!canModify)
            {
                result.Errors.Add("Debt does not exist for user or user is not authorised to modify the debt");
                return result;
            }

            result.WasSuccessfull = true;
            return result;

        }
    }
}