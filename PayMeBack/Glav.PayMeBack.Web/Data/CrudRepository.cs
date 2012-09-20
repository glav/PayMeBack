using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.Entity;

namespace Glav.PayMeBack.Web.Data
{
	public class CrudRepository : ICrudRepository
	{
		/// <summary>
		/// Gets a single entity/record from the data store
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <returns></returns>
		public T GetSingle<T>(Expression<Func<T, bool>> query) where T : class
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				var entityTable = context.Set<T>();
				var result = entityTable.Where<T>(query).FirstOrDefault();

				return result;
			}
		}

		/// <summary>
		/// Gets a list of records/entities from the data store (unsorted)
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> GetAll<T>() where T : class
		{
			return GetAll<T>((d) => true);
		}

		/// <summary>
		/// Gets a list of records/entities from the data store (unsorted)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <returns></returns>
		public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> query) where T : class
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				var entityTable = context.Set<T>();
				var result = entityTable.Where<T>(query);
				return result.ToList();
			}
		}

		/// <summary>
		/// Gets a list of records/entities from the data store using paging and sorting
		/// and also returning a record count
		/// </summary>
		/// <remarks>
		/// This method actually executes 2 queries to get the record count and the
		/// original paged query. Only use this if you really need a record count,
		/// else use the method that does not return a record count.
		/// </remarks>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="orderBy"></param>
		/// <param name="recordCount"></param>
		/// <returns></returns>
		public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> query, int pageNumber, int pageSize,
											Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, out int recordCount) where T : class
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				var entityTable = context.Set<T>();
				var result = entityTable
							.Where<T>(query);

				if (orderBy != null)
				{
					result = orderBy(result).Skip(pageNumber * pageSize);
				}

				recordCount = result.Count();

				return result.Take(pageSize).ToList();
			}
		}

		/// <summary>
		/// Gets a list of records/entities from the data store using paging and sorting
		/// </summary>
		/// <remarks>
		/// This method only executes 1 querie since it does not have to return the
		/// record count. Use this in preference to the method that returns a record count,
		/// as it is more efficient.
		/// </remarks>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="orderBy"></param>
		/// <returns></returns>
		public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> query, int pageNumber, int pageSize,
											Func<IQueryable<T>, IOrderedQueryable<T>> orderBy) where T : class
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				var entityTable = context.Set<T>();
				var result = entityTable
							.Where<T>(query);

				if (orderBy != null)
				{
					result = orderBy(result).Skip((pageNumber > 0 ? pageNumber-1 : 0) * pageSize);
				}

				return result.Take(pageSize).ToList();
			}
		}
		/// <summary>
		/// Deletes an entity/record from the data store
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		public void Delete<T>(Expression<Func<T, bool>> query) where T : class
		{
			using (var context = new PayMeBackEntities())
			{
				var entityTable = context.Set<T>();
				var result = entityTable.Where<T>(query);

				result.ToList().ForEach(e => entityTable.Remove(e));
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Inserts a record/entity into the data store
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public T Insert<T>(T entity) where T : class
		{
			using (var context = new PayMeBackEntities())
			{
				context.Configuration.ProxyCreationEnabled = false;
				var entityTable = context.Set<T>();
				entityTable.Add(entity);
				context.SaveChanges();
				return entity;
			}
		}

		/// <summary>
		/// Updates/saves a record/entity into the data store.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		public void Update<T>(T entity) where T : class
		{
			using (var context = new PayMeBackEntities())
			{

				var entityTable = context.Set<T>();
				entityTable.Attach(entity);
				var dbEntity = context.Entry<T>(entity);
				dbEntity.State = System.Data.EntityState.Modified;
				context.SaveChanges();
			}
		}
	}
}
