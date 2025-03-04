﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace rapid_docs_api.Utilities
{
    public class PagedResults<T>
    {
        /// <summary>
        /// The page number this page represents. 
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary> 
        /// The size of this page. 
        /// </summary> 
        public int PageSize { get; set; }

        /// <summary> 
        /// The total number of pages available. 
        /// </summary> 
        public int TotalNumberOfPages { get; set; }

        /// <summary> 
        /// The total number of records available. 
        /// </summary> 
        public int TotalNumberOfRecords { get; set; }

        /// <summary> 
        /// The URL to the next page - if null, there are no more pages. 
        /// </summary> 
        public string NextPageUrl { get; set; }

        /// <summary> 
        /// The records this page represents. 
        /// </summary> 
        public IEnumerable<T> Results { get; set; }
    }

    public static class PagedResultsExtensions
    {
        /// <summary>
        /// Order the IQueryable by the given property or field.
        /// </summary>

        /// <typeparam name="T">The type of the IQueryable being ordered.</typeparam>
        /// <param name="queryable">The IQueryable being ordered.</param>
        /// <param name="propertyOrFieldName">The name of the property or field to order by.</param>
        /// <param name="ascending">Indicates whether or not the order should be ascending (true) or descending (false.)</param>
        /// <returns>Returns an IQueryable ordered by the specified field.</returns>
        public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> queryable, string propertyOrFieldName, bool ascending = true)
        {
            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

            return queryable.Provider.CreateQuery<T>(orderByExpression);
        }
    }
}
