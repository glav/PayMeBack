using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glav.PayMeBack.Web.Data;

namespace Glav.PayMeBack.Web.Domain
{
	public class User : BaseModel
	{
		public User() { }
		public User(UserDetail userDetail)
		{
			MapDetailToDomainEntity(userDetail);
		}

		public Guid Id { get; set; }
		public string EmailAddress { get; set; }
		public string FirstNames { get; set; }
		public string Surname { get; set; }

		private void MapDetailToDomainEntity(UserDetail userDetail)
		{
			Id = userDetail.Id;
			EmailAddress = userDetail.EmailAddress;
			FirstNames = userDetail.FirstNames;
			Surname = userDetail.Surname;
		}

	}
}