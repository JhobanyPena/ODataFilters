using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace ODataFilters.WebApi.Extensions
{
    public static class QueryableExtensions
    {
        public class DataSourceResult
        {
            #region properties...

            public IQueryable Data { get; set; }

            public Uri NextPageLink { get; set; }

            public long? Count { get; set; }

            #endregion
        }

        public static DataSourceResult ToDataSourceResult(this IQueryable query, ODataQueryOptions ops, HttpRequest request)
        {
            var pageSize = 10;

            var items = ops.ApplyTo(query, new ODataQuerySettings() { PageSize = pageSize });
            var count = ops.Count?.GetEntityCount(ops.Filter?.ApplyTo(query, new ODataQuerySettings()) ?? query);
            var nextLink = request.GetNextPageLink(pageSize);

            return new DataSourceResult()
            {
                Data = items,
                NextPageLink = nextLink,
                Count = count
            };
        }
    }
}
