using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Core.Domain
{
	public class User : BaseModel
	{
		public User()
		{
			Id = Guid.Empty;
		}
		public Guid Id { get; set; }
		public string EmailAddress { get; set; }
		public string FirstNames { get; set; }
		public string Surname { get; set; }
	}
}