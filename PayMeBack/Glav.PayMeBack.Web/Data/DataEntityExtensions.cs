using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glav.PayMeBack.Web.Data
{
	public static class DataEntityExtensions
	{
		public static void SetDataState<T>(this PayMeBackEntities context, T entity, System.Data.EntityState state) where T : class
		{
			if (entity != null)
			{
				context.Entry<T>(entity).State = state;
			}
		}

	}
}