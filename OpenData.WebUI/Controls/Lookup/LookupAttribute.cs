using System;
namespace TestApp.Controls.Lookup
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class LookupAttribute : Attribute
    {
        public Type Model { get; set; }
        public string NameField { get; set; }
    }
}