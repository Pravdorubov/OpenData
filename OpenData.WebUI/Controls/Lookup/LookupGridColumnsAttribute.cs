using System;

namespace TestApp.Controls.Lookup
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LookupGridColumnsAttribute : Attribute
    {
        public string[] LookupColumns { get; set; }

        public LookupGridColumnsAttribute(params string[] values)
        {
            LookupColumns = values;
        }
    }
}