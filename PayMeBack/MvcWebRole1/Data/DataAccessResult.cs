using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;
using System.Text;

namespace Glav.PayMeBack.Web.Data
{
	public class DataAccessResult
	{
		public DataAccessResult()
		{
			Errors = new List<string>();
		}
		public bool WasSuccessfull { get; set; }
		public List<string> Errors { get; set; }
	}

	public static class DataAccessResultExtensions
	{
		public static DataAccessResult ToDataResult(this DbEntityValidationException valEx)
		{
			var descritpiveList = new List<string>();
			foreach (var err in valEx.EntityValidationErrors)
			{
				if (!err.IsValid && err.ValidationErrors != null)
				{
					err.ValidationErrors.ToList().ForEach(e =>
					{
						descritpiveList.Add(string.Format("{0}: {1}", e.PropertyName, e.ErrorMessage));
					});
				}
			}

			if (descritpiveList.Count == 0)
			{
				descritpiveList.Add("Validation Error. Problem with data submitted.");
			}

			return new DataAccessResult { WasSuccessfull = false, Errors = descritpiveList };
		}

		public static DataAccessResult ToDataResult(this Exception ex)
		{
			var msg = new StringBuilder("Error: ");
			if (ex.InnerException != null)
			{
				msg.Append(ex.GetBaseException().Message);
			}
			else
			{
				msg.Append(ex.Message);
			}

			var result = new DataAccessResult { WasSuccessfull = false};
			result.Errors.Add(msg.ToString());
			return result;
		}

		public static string ToSingleMessage(this DataAccessResult result)
		{
			if (result.WasSuccessfull)
			{
				return "ok";
			}

			if (result.Errors != null)
			{
				var msg = new StringBuilder();
				result.Errors.ForEach(e =>
				                      	{
											if (msg.Length > 0)
											{
												msg.Append(", ");
											}
				                      		msg.Append(e);
				                      	});
				return msg.ToString();
			}

			return "System Error";
		}

	}
}