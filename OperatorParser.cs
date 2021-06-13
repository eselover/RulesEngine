using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineApplication
{
    /// <summary>
    /// Converter class to parse strings into operators
    /// </summary>
    public static class OperatorParser
    {
        /// <summary>
        /// Converts string into conditional operators
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="compareOperator"></param>
        /// <returns>The method to execute the operator parsed</returns>
        public static Func<T, T, bool> ParseOperator<T>(string compareOperator)
        {
            switch (compareOperator)
            {
                case ">":
                    return GreaterThan<T>;
                case ">=":
                    return GreaterThanOrEqualTo<T>;
                case "<":
                    return LessThan<T>;
                case "<=":
                    return LessThanOrEqualTo<T>;
                case "==":
                    return EqualTo<T>;
                case "contains":
                    return Contains<T>;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert strings to arthimetic operators 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="compareOperator"></param>
        /// <returns>The method to perform the parsed operator</returns>
        public static Func<T, T, T> ParseArthimeticOperator<T>(string compareOperator)
        {
            switch (compareOperator)
            {
                case "+=":
                    return AddValue<T>;
                case "-=":
                    return SubtractValue<T>;
                case "=":
                    return AssignValue<T>;
                default:
                    return null;
            }
        }

        private static bool GreaterThan<T>(T obj1, T obj2)
        {
            return (obj1 as IComparable).CompareTo(obj2) > 0;
        }

        private static bool GreaterThanOrEqualTo<T>(T obj1, T obj2)
        {
            return (obj1 as IComparable).CompareTo(obj2) >= 0;
        }

        private static bool LessThan<T>(T obj1, T obj2)
        {
            return (obj1 as IComparable).CompareTo(obj2) < 0;
        }

        private static bool LessThanOrEqualTo<T>(T obj1, T obj2)
        {
            return (obj1 as IComparable).CompareTo(obj2) >= 0;
        }

        private static bool EqualTo<T>(T obj1, T obj2)
        {
            return (obj1 as IComparable).Equals(obj2);
        }

        private static bool Contains<T>(T obj1, T obj2)
        {
            var objectType = obj1.GetType();
            if (objectType.FullName == typeof(string).FullName)
            {
                return (obj2 as string).Contains((obj1 as string));
            }
            else return false;
        }

        private static T AddValue<T>(T obj1, T obj2)
        {
            dynamic a = obj1;
            dynamic b = obj2;
            return a += b;
        }

        private static T SubtractValue<T>(T obj1, T obj2)
        {
            dynamic a = obj1;
            dynamic b = obj2;
            return a -= b;
        }

        private static T AssignValue<T>(T obj1, T obj2)
        {
            return obj2;
        }
    }
}
