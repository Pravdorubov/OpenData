using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace TestApp.Controls.Lookup
{
    /// <summary>
    /// Implements lookup control generic logic
    /// </summary>
    public class LookupBasicController : Controller
    {
        /// <summary>
        /// Has to return correct DataBase context to fetch data for lookup control
        /// </summary>
        protected virtual DbContext GetDbContext
        {
            get { throw new NotImplementedException("You have to implement this method to return correct db context"); }
        }

        /// <summary>
        /// This method allows to update query, should be overridden in derived class
        /// </summary>
        /// <param name="query">Generic query</param>
        /// <param name="settings">Lookup control settings</param>
        /// <returns>Updated query</returns>
        protected virtual IQueryable LookupBaseQuery(IQueryable query, LookupSettings settings)
        {
            return null;
        }

        /// <summary>
        /// Used as standard method to fetch data for lookup control
        /// </summary>
        /// <param name="settings">Lookup settings</param>
        /// <returns></returns>
        public virtual ActionResult LookupData([ModelBinder(typeof(LookupModelBinder))] LookupSettings settings)
        {
            return LookupDataResolver.BasicLookup(settings, GetDbContext, LookupBaseQuery);
        }

        /// <summary>
        /// Used as standard method to fetch data for lookup grid
        /// </summary>
        /// <param name="settings">Lookup settings</param>
        /// <returns></returns>
        public virtual ActionResult LookupDataGrid([ModelBinder(typeof(LookupModelBinder))] LookupSettings settings)
        {
            return LookupDataResolver.BasicGrid(settings, GetDbContext, LookupBaseQuery);
        }

    }
}
