using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Glav.PayMeBack.Web.Controllers.Api
{
    public class PeopleController : ApiController
    {
        // GET /api/people
        public IEnumerable<string> Get()
        {
        	return OweMeMoney();
        }

		public IEnumerable<string> OweMeMoney()
		{
			return new string[] {"1", "2"};
		}

		public IEnumerable<string> OweMoneyTo()
		{
			return new string[] { "1", "2" };
		}

        // GET /api/people/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
