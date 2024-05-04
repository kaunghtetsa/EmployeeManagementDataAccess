using System;
using System.Linq;
using System.Linq.Expressions;

using ASM.EmployeeManagement.DataAccess.Common.Enums;
using ASM.EmployeeManagement.DataAccess.Common.Paging;

namespace ASM.EmployeeManagement.DataAccess.Common.Extension
{
    internal static class ExtensionMethods
    {

        /// <summary>
        /// Order By
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        internal static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool asc)
        {
            var type = typeof(T);
            string methodName = asc ? "OrderBy" : "OrderByDescending";
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var orderByExp1 = Expression.Lambda(propertyAccess, parameter);

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                              new Type[] { type, property.PropertyType },
                              source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// Order By Skip Take
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageInfo"></param>
        /// <param name="defaultSortKey"></param>
        /// <param name="sDefaultOrder"></param>
        /// <returns></returns>
        internal static IQueryable<T> OrderBySkipTake<T>(this IQueryable<T> source, PageInfo pageInfo, string defaultSortKey, SortOrderType sDefaultOrder)
        {
            bool bContinue = false;

            if (pageInfo != null)
            {
                bContinue = true;
            }

            bool bOrder = GetOrderType(sDefaultOrder, bContinue, pageInfo);

            source = SortAndSkip(source, pageInfo, bContinue, bOrder, defaultSortKey);

            return source;
        }

        /// <summary>
        /// Get Order Type
        /// </summary>
        /// <param name="sDefaultOrder"></param>
        /// <param name="bContinue"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        private static bool GetOrderType(SortOrderType sDefaultOrder, bool bContinue, PageInfo pageInfo)
        {
            bool bOrder = false;

            if (sDefaultOrder == SortOrderType.Ascending)
            {
                bOrder = true;
            }

            if (bContinue && pageInfo.SortOrder != null)
            {
                bOrder = pageInfo.SortOrder == (int)SortOrderType.Ascending ? true : false;
            }

            return bOrder;
        }

        /// <summary>
        /// Sort And Skip
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageInfo"></param>
        /// <param name="bContinue"></param>
        /// <param name="bOrder"></param>
        /// <param name="defaultSortKey"></param>
        /// <returns></returns>
        private static IQueryable<T> SortAndSkip<T>(this IQueryable<T> source, PageInfo pageInfo, bool bContinue, bool bOrder, string defaultSortKey)
        {
            string sPropertyName = defaultSortKey;

            if (bContinue && pageInfo.SortKey != null)
            {
                string propertyName = pageInfo.SortKey;
                sPropertyName = string.IsNullOrEmpty(propertyName) ? defaultSortKey : propertyName;
            }

            source = source.OrderBy(sPropertyName, bOrder);

            if (bContinue && pageInfo.StartIndex != null && pageInfo.StartIndex > 0)
            {
                source = source.Skip((int)pageInfo.StartIndex);
            }

            if (bContinue && pageInfo.Num != null)
            {
                source = source.Take((int)pageInfo.Num);
            }

            return source;
        }
    }
}
