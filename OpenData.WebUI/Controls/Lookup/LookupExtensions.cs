using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using TestApp.Controls.Lookup;

// ReSharper disable CheckNamespace
namespace System.Web.Mvc.Html
// ReSharper restore CheckNamespace
{

    public static class LookupExtensions
    {
        /// <summary>
        /// Returns an HTML representation of lookup control for object that is represented by System.Linq.Expressions.Expression
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">An expression that indentifies object that contains properties to display</param>
        /// <returns></returns>
        public static MvcHtmlString LookupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                         Expression<Func<TModel, TProperty>> expression)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                // ReSharper disable Mvc.ActionNotResolved
            return LookupFor(htmlHelper, expression, urlHelper.Action("LookupData"), null, null, null);
                // ReSharper restore Mvc.ActionNotResolved
        }

        /// <summary>
        /// Returns an HTML representation of lookup control for object that is represented by System.Linq.Expressions.Expression
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">An expression that indentifies object that contains properties to display</param>
        /// <param name="filterAction">Url that should be launched to fetch the data</param>
        /// <returns></returns>
        public static MvcHtmlString LookupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                 Expression<Func<TModel, TProperty>> expression,
                                                                 string filterAction)
        {
            return LookupFor(htmlHelper, expression, filterAction, null, null, null);
        }

        /// <summary>
        /// Returns an HTML representation of lookup control for object that is represented by System.Linq.Expressions.Expression
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">An expression that indentifies object that contains properties to display</param>
        /// <param name="filterAction">Url that should be launched to fetch the data</param>
        /// <param name="htmlAttributes">Additional html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString LookupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                         Expression<Func<TModel, TProperty>> expression,
                                                         string filterAction, IDictionary<string, object> htmlAttributes)
        {
            return LookupFor(htmlHelper, expression, filterAction, null, null, htmlAttributes);
        }


        /// <summary>
        /// Returns an HTML representation of lookup control for object that is represented by System.Linq.Expressions.Expression
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">An expression that indentifies object that contains properties to display</param>
        /// <param name="filterAction">Url that should be launched to fetch the data</param>
        /// <param name="modelType">Type of db model that used as source for lookup</param>
        /// <param name="nameField">Field from releated model that used as display text for lookup</param>
        /// <param name="htmlAttributes">Additional html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString LookupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                 Expression<Func<TModel, TProperty>> expression, 
                                                                 string filterAction, Type modelType, 
                                                                 String nameField, 
                                                                 IDictionary<string, object> htmlAttributes)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var commonMetadata = PrepareLookupCommonMetadata(
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), 
                htmlHelper.ViewData.ModelMetadata, modelType, nameField);
            var lookupAttribute = commonMetadata.AdditionalValues[LookupConsts.LookupMetadata] as LookupAttribute;
            return LookupHtmlInternal(htmlHelper, commonMetadata, lookupAttribute, fieldName, filterAction, htmlAttributes);
        }

        private static ModelMetadata PrepareLookupCommonMetadata(ModelMetadata fieldMetadata, 
                                                                 ModelMetadata modelMetadata , 
                                                                 Type modelType, String nameField)
        {
            LookupAttribute lookupMetadata;
            if (modelType != null && nameField != null)
            {
                lookupMetadata = new LookupAttribute { Model = modelType, NameField = nameField };
                if (fieldMetadata.AdditionalValues.ContainsKey(LookupConsts.LookupMetadata))
                    fieldMetadata.AdditionalValues.Remove(LookupConsts.LookupMetadata);
                fieldMetadata.AdditionalValues.Add(LookupConsts.LookupMetadata, lookupMetadata);
            }

            if (fieldMetadata.AdditionalValues != null && fieldMetadata.AdditionalValues.ContainsKey(LookupConsts.LookupMetadata))
            {
                lookupMetadata = fieldMetadata.AdditionalValues[LookupConsts.LookupMetadata] as LookupAttribute;
                if (lookupMetadata != null)
                {
                    var prop = lookupMetadata.Model.GetPropertyWithAttribute("KeyAttribute");
                    var releatedTableKey = prop != null ? prop.Name : String.Format("{0}Id", lookupMetadata.Model.Name);
                    fieldMetadata.AdditionalValues.Add("idField", releatedTableKey);
                    var releatedTableMetadata =
                            modelMetadata.Properties.FirstOrDefault(proper
                                                                                        =>
                                                                                        proper.PropertyName ==
                                                                                        lookupMetadata.Model.Name);
                    if (releatedTableMetadata != null)
                    {
                        UpdateLookupColumnsInfo(releatedTableMetadata, fieldMetadata);
                        UpdateNameFieldInfo(lookupMetadata.NameField, releatedTableMetadata, fieldMetadata);
                    }
                    else
                    {
                        throw new ModelValidationException(String.Format(
                            "Couldn't find data from releated table. Lookup failed for model {0}",
                            lookupMetadata.Model.Name));
                    }
                }
            }
            else
            {
                throw new ModelValidationException(String.Format("Couldn't find releated model type. Lookup field"));
            }

            return fieldMetadata;
        }

        private static void UpdateNameFieldInfo(string nameField, ModelMetadata releatedTableMetadata, 
            ModelMetadata commonMetadata)
        {
            var nameFieldMetedata =
                releatedTableMetadata.Properties.FirstOrDefault(propt => propt.PropertyName == nameField);
            if (nameFieldMetedata != null)
            {
                commonMetadata.AdditionalValues.Add("lookupFieldValue", nameFieldMetedata.SimpleDisplayText);
                commonMetadata.AdditionalValues.Add("lookupFieldDisplayValue", nameFieldMetedata.DisplayName);
            }
            else
            {
                throw new ModelValidationException(String.Format("Couldn't find name field in releated table {0}",
                                                                 releatedTableMetadata.GetDisplayName()));
            }
        }

        private static void UpdateLookupColumnsInfo(ModelMetadata releatedTableMetadata, ModelMetadata metadata)
        {
            IDictionary<string, string> columns = new Dictionary<string, string>();
            var gridColumns = releatedTableMetadata.ModelType.GetCustomAttributeByType<LookupGridColumnsAttribute>();
            if (gridColumns != null)
            {
                foreach (var column in gridColumns.LookupColumns)
                {
                    var metadataField =
                        releatedTableMetadata.Properties.FirstOrDefault(
                            propt => propt.PropertyName == column);
                    if (metadataField != null)
                    {
                        columns.Add(column, metadataField.DisplayName);
                    }
                    else
                    {
                        throw new ModelValidationException(
                            String.Format("Couldn't find column in releated table {0}", 
                            releatedTableMetadata.GetDisplayName()));
                    }
                }
                metadata.AdditionalValues.Add("lookupColumns", columns);
            }
        }

        private static MvcHtmlString LookupHtmlInternal(HtmlHelper htmlHelper, ModelMetadata metadata, 
                                                        LookupAttribute lookupMetadata, string name,
                                                        string action, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Error", "htmlHelper");
            }

            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttribute("id", String.Format("{0}_{1}", name, "div"));
            divBuilder.MergeAttribute("class", "form-wrapper cf");
            divBuilder.MergeAttribute("type", lookupMetadata.Model.FullName);
            divBuilder.MergeAttribute("nameField", lookupMetadata.NameField);
            divBuilder.MergeAttribute("idField", metadata.AdditionalValues["idField"] as string);
            divBuilder.MergeAttribute("nameFieldDisplay", metadata.AdditionalValues["lookupFieldDisplayValue"] as string);
            divBuilder.MergeAttribute("action", action);

            var columnsDivBuilder = new TagBuilder("div");
            columnsDivBuilder.MergeAttribute("id", String.Format("{0}_{1}", name, "columns"));

            if (metadata.AdditionalValues.ContainsKey("lookupColumns"))
            {
                var columns = ((IDictionary<string, string>)metadata.AdditionalValues["lookupColumns"]);
                var columnString = String.Empty;
                foreach (var column in columns.Keys)
                {
                    var columnDiv = new TagBuilder("div");
                    columnDiv.MergeAttribute("colName", column);
                    columnDiv.MergeAttribute("displayName", columns[column]);
                    columnString += columnDiv.ToString(TagRenderMode.SelfClosing);
                }
                columnsDivBuilder.InnerHtml = columnString;
            }

            var inputBuilder = new TagBuilder("input");
            inputBuilder.MergeAttributes(htmlAttributes);
            inputBuilder.MergeAttribute("type", "text");
            inputBuilder.MergeAttribute("class", "lookup", true);
            inputBuilder.MergeAttribute("id", String.Format("{0}_{1}", name, "lookup"), true);
            inputBuilder.MergeAttribute("value", metadata.AdditionalValues["lookupFieldValue"] as string, true);

            var hiddenInputBuilder = new TagBuilder("input");
            hiddenInputBuilder.MergeAttribute("type", "hidden");
            hiddenInputBuilder.MergeAttribute("name", name, true);
            hiddenInputBuilder.MergeAttribute("id", name, true);
            hiddenInputBuilder.MergeAttribute("value", metadata.SimpleDisplayText, true);

            var buttonBuilder = new TagBuilder("input");
            buttonBuilder.MergeAttribute("type", "button");
            buttonBuilder.MergeAttribute("value", "Lookup");
            buttonBuilder.MergeAttribute("class", "lookupbutton");
            buttonBuilder.MergeAttribute("id", String.Format("{0}_{1}", name, "lookupbtn"), true);

            divBuilder.InnerHtml = String.Format(@"{0}{1}{2}{3}", inputBuilder.ToString(TagRenderMode.SelfClosing),
                                                 hiddenInputBuilder.ToString(TagRenderMode.SelfClosing),
                                                 buttonBuilder.ToString(TagRenderMode.SelfClosing),
                                                 columnsDivBuilder.ToString(TagRenderMode.Normal)
                                                 );

            return new MvcHtmlString(divBuilder.ToString(TagRenderMode.Normal));
        }
    }
}