using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
namespace Glav.PayMeBack.Web.Data
{
	public interface ICrudRepository
	{
		/// <summary>
		/// Gets a list of records/entities from the data store (unsorted)
		/// </summary>
		/// <returns></returns>
		IEnumerable<T> GetAll<T>() where T : class;
		
			/// <summary>
		/// Gets a list of records/entities from the data store (unsorted)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <returns></returns>
		IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> query) where T : class;

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
		IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> query, int pageNumber, int pageSize,
											Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, out int recordCount) where T : class;


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
		IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> query, int pageNumber, int pageSize,
											Func<IQueryable<T>, IOrderedQueryable<T>> orderBy) where T : class;
		/// <summary>
		/// Gets a single entity/record from the data store
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <returns></returns>
		T GetSingle<T>(Expression<Func<T, bool>> query) where T : class;
		
		/// <summary>
		/// Inserts a record/entity into the data store
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Insert<T>(T entity) where T : class;
		
		/// <summary>
		/// Deletes an entity/record from the data store
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		void Delete<T>(Expression<Func<T, bool>> query) where T : class;
		
		/// <summary>
		/// Updates/saves a record/entity into the data store.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		void Update<T>(T entity) where T : class;
	}
}
