using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace TestApp.Controls.Lookup
{
    /// <summary>
    /// Prepare simple queries and provides data for lookup control
    /// </summary>
    public class LookupDataResolver
    {
        /// <summary>
        /// Delegate which could be used to alter query 
        /// </summary>
        /// <param name="query">Generic query</param>
        /// <param name="settings">Lookup control settings</param>
        /// <returns>Changed query</returns>
        /// 
        public delegate IQueryable OnAfterQueryPrepared(IQueryable query, LookupSettings settings);

        /// <summary>
        /// Provides generic logic to fetch data for lookup control grid
        /// </summary>
        /// <param name="settings">Lookup control settings</param>
        /// <param name="dbContext">DataBase context to fetch data</param>
        /// <param name="onAfterQueryPrepared">Query delegate</param>
        /// <returns></returns>
        public static ActionResult BasicGrid(LookupSettings settings, 
                                             DbContext dbContext, 
                                             OnAfterQueryPrepared onAfterQueryPrepared)
        {
            return LookupMethodCall("LookupDataForGrid", settings, dbContext, onAfterQueryPrepared);
        }

        /// <summary>
        /// Provides generic logic to search data when user changes text directly in lookup control
        /// </summary>
        /// <param name="settings">Lookup control settings</param>
        /// <param name="dbContext">DataBase context to fetch data</param>
        /// <param name="onAfterQueryPrepared">Query delegate</param>
        /// <returns></returns>
        public static ActionResult BasicLookup(LookupSettings settings,
                                               DbContext dbContext,
                                               OnAfterQueryPrepared onAfterQueryPrepared)
        {
            return LookupMethodCall("LookupSearch", settings, dbContext, onAfterQueryPrepared);
        }



        private static ActionResult LookupMethodCall(string methodName, LookupSettings settings,
                                        DbContext dbContext,
                                        OnAfterQueryPrepared onAfterQueryPrepared)
        {
            var methodLookupCall = typeof(LookupDataResolver).
            GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            methodLookupCall = methodLookupCall.MakeGenericMethod(settings.Model);
            var lookupSettings = Expression.Parameter(typeof(LookupSettings), "settings");
            var dbCtx = Expression.Parameter(typeof(DbContext), "dbContext");
            var funct = Expression.Parameter(typeof(OnAfterQueryPrepared), "onAfterQueryPrepared");
            var lookupSearch = Expression.Lambda(
                    Expression.Call(
                        null,
                        methodLookupCall,
                        lookupSettings, dbCtx, funct),
                    lookupSettings, dbCtx, funct);
            var lookupSearchDelegate = (Func<LookupSettings, DbContext, OnAfterQueryPrepared, JsonResult>)
                lookupSearch.Compile();
            return lookupSearchDelegate(settings, dbContext, onAfterQueryPrepared);
        }

// ReSharper disable UnusedMember.Local
        private static JsonResult LookupSearch<T>(LookupSettings settings, DbContext dbContext, 
            OnAfterQueryPrepared onAfterQueryPrepared) where T : class
// ReSharper restore UnusedMember.Local
        {
            var modelType = typeof(T);
            var request = dbContext.Set<T>().AsQueryable();
            if (onAfterQueryPrepared != null)
            {
                var query = onAfterQueryPrepared(request, settings);
                if (query != null) request = query.Cast<T>();
            }
            request = request.WhereStartsWith(settings.Filter.SearchField, settings.Filter.SearchString);
            return new JsonResult
            {
                Data = request.ToList().Select(t => new
                {
                    label = modelType.GetProperty(settings.NameField).GetValue(t).ToString(),
                    id = modelType.GetProperty(settings.IdField).GetValue(t).ToString()
                }).ToList(),
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private static IEnumerable<string> GetDataFromColumns(Type model, LookupSettings settings, object instance)
        {
            var dataArray = new List<string>
                {
                    model.GetProperty(settings.IdField).GetValue(instance).ToString(),
                    model.GetProperty(settings.NameField).GetValue(instance).ToString()
                };
            var gridColumns = model.GetCustomAttributeByType<LookupGridColumnsAttribute>();
            if (gridColumns != null)
            {
                dataArray.AddRange(from column in gridColumns.LookupColumns 
                                   select model.GetProperty(column).GetValue(instance) 
                                   into val where val != null 
                                   select val.ToString());
            }
            return dataArray;
        }

// ReSharper disable UnusedMember.Local
        private static JsonResult LookupDataForGrid<T>(LookupSettings settings, DbContext dbContext, OnAfterQueryPrepared onAfterQueryPrepared) where T : class
// ReSharper restore UnusedMember.Local
        {
            var modelType = typeof(T);
            var pageIndex = settings.GridSettings.PageIndex - 1;
            var pageSize = settings.GridSettings.PageSize;
            var request = dbContext.Set<T>().AsQueryable();
            if (onAfterQueryPrepared != null)
            {
                var query = onAfterQueryPrepared(request, settings);
                if (query != null) request = query.Cast<T>();
            }
            if (settings.GridSettings.IsSearch)
            {
                switch (settings.Filter.Operator)
                {
                    case SearchOperator.Equal:
                        request = request.Equal(settings.Filter.SearchField, settings.Filter.SearchString); break;
                    case SearchOperator.NotEqual:
                        request = request.NotEqual(settings.Filter.SearchField, settings.Filter.SearchString); break;
                    case SearchOperator.Contains:
                        request = request.WhereContains(settings.Filter.SearchField, settings.Filter.SearchString); break;
                }
            }

            var totalRecords = request.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            var userGroups = request
               .OrderBy(!settings.GridSettings.Asc, settings.GridSettings.SortColumn)
               .Skip(pageIndex * pageSize)
               .Take(pageSize);

            return new JsonResult
            {
                Data = new
                {
                    total = totalPages,
                    settings.GridSettings.PageIndex,
                    records = totalRecords,
                    rows = (
                            userGroups.AsEnumerable().Select(t => new
                            {
                                id = modelType.GetProperty(settings.IdField).GetValue(t).ToString(),
                                cell = GetDataFromColumns(modelType, settings, t)

                            }).ToList())
                },
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}