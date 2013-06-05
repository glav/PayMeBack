using Glav.PayMeBack.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Models
{
    public class AccountSettingsViewModel
    {
        public User UserDetails { get; set; }
        public string NewPassword { get; set; }
    }
}