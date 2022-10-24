using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace OpenData.Domain.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDescriptionString(this Enum val)
        {
            try
            {
                var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes.Length > 0 ? attributes[0].Description : val.ToString();
            }
            catch (Exception)
            {
                return val.ToString();
            }
        }
    }
}