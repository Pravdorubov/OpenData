using System;

namespace TestApp.Controls.Lookup
{
    public enum SearchOperator
    {
        Equal,
        NotEqual,
        Contains
    }

    public class FilterSettings
    {
        public string SearchString;
        public string SearchField;
        public SearchOperator Operator;
    }

    public class GridSettings
    {
        public bool IsSearch { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortColumn { get; set; }
        public bool Asc { get; set; }
    }

    public class LookupSettings
    {
        public Type Model { get; set; }
        public FilterSettings Filter { get; set; }
        public GridSettings GridSettings { get; set; }
        public string IdField { get; set; }
        public string NameField { get; set; }
    }
}