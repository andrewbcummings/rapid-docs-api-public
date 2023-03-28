
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rapid_docs_api.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace rapid_docs_api.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Creates a paged set of results.
        /// </summary>
        /// <typeparam name="T">The type of the source IQueryable.</typeparam>
        /// <typeparam name="TReturn">The type of the returned paged results.</typeparam>
        /// <param name="queryable">The source IQueryable.</param>
        /// <param name="page">The page number you want to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="orderBy">The field or property to order by.</param>
        /// <param name="ascending">Indicates whether or not the order should be ascending (true) or descending (false.)</param>
        /// <returns>Returns a paged set of results.</returns>
        //protected async Task<PagedResults<TReturn>> CreatePagedResults<T, TReturn>(
        //    IQueryable<T> queryable,
        //    int page,
        //    int pageSize,
        //    string orderBy,
        //    bool ascending)
        //{
        //    var skipAmount = pageSize * (page - 1);

        //    var projection = queryable
        //        .OrderByPropertyOrField(orderBy, ascending)
        //        .Skip(skipAmount)
        //        .Take(pageSize).ProjectTo<TReturn>( );

        //    var totalNumberOfRecords = await queryable.CountAsync();
        //    var results = await projection.ToListAsync();

        //    var mod = totalNumberOfRecords % pageSize;
        //    var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

        //    var nextPageUrl =
        //    page == totalPageCount
        //        ? null
        //        : Url?.Link("DefaultApi", new
        //        {
        //            page = page + 1,
        //            pageSize,
        //            orderBy,
        //            ascending
        //        });

        //    return new PagedResults<TReturn>
        //    {
        //        Results = results,
        //        PageNumber = page,
        //        PageSize = results.Count,
        //        TotalNumberOfPages = totalPageCount,
        //        TotalNumberOfRecords = totalNumberOfRecords,
        //        NextPageUrl = nextPageUrl
        //    };
        //}
    }
}
