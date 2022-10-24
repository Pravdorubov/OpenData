using System;
using System.Web;
using System.Web.Mvc;

namespace TestApp.Controls.Lookup
{
    public class LookupModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            var lookupSettings = new LookupSettings
                {
                    Model = Type.GetType(request["modelType"]),
                    IdField = request["IdField"],
                    NameField = request["NameField"],
                    Filter = new FilterSettings
                        {
                            SearchString = request["searchString"] ?? String.Empty,
                            SearchField = request["searchField"]
                        }
                };
            if(request["searchOper"] != null)
            {
                switch (request["searchOper"])
                {
                    case "eq": lookupSettings.Filter.Operator = SearchOperator.Equal; break; 
                    case "ne": lookupSettings.Filter.Operator = SearchOperator.NotEqual; break; 
                    case "cn": lookupSettings.Filter.Operator = SearchOperator.Contains; break;
                }
            }
            lookupSettings.GridSettings = new GridSettings {Asc = request["sord"] == "asc"};
            if (request["isSearch"] != null) lookupSettings.GridSettings.IsSearch = Convert.ToBoolean(request["isSearch"]);
            if (request["page"] != null) lookupSettings.GridSettings.PageIndex = Convert.ToInt32(request["page"]);
            if (request["rows"] != null) lookupSettings.GridSettings.PageSize = Convert.ToInt32(request["rows"]);
            lookupSettings.GridSettings.SortColumn = request["sidx"];
            if (lookupSettings.Filter.SearchField == null) { lookupSettings.Filter.SearchField = request["NameField"];
                lookupSettings.Filter.Operator = SearchOperator.Contains;
            }


            return lookupSettings;
        }
    }
}