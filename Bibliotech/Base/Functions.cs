using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Base
{
    public class Functions
    {
        public static bool IsNull(object obj)
        {
            if (obj == null)
                return true;
            return !obj.GetType().GetProperties().Any(p => p.GetValue(obj) != null);
        }

        public static bool IsNullExcludingProperties(object obj, params string[] properties)
        {
            if (obj == null)
                return true;
            return !obj.GetType().GetProperties().Any(p => p.GetValue(obj) != null && !properties.Contains(p.Name));
        }
    }
}