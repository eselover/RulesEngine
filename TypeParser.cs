using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineApplication
{
    /// <summary>
    /// Converter class to parse strings into different types
    /// </summary>
    public static class TypeParser
    {
        public static dynamic GetValue(string typeName, string value)
        {
            if (typeName == typeof(int).FullName)
                return ParseInt(value);
            if (typeName == typeof(decimal).FullName)
                return ParseDecimal(value);
            if (typeName == typeof(double).FullName)
                return ParseDouble(value);
            if (typeName == typeof(bool).FullName)
                return ParseBoolean(value);

            return null;
        }

        private static int? ParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            else return null;
        }

        private static decimal? ParseDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
                return result;
            else return null;
        }

        private static double? ParseDouble(string value)
        {
            if (double.TryParse(value, out double result))
                return result;
            else return null;
        }

        private static bool? ParseBoolean(string value)
        {
            if (bool.TryParse(value, out bool result))
                return result;
            else return null;
        }
    }
}
