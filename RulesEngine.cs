using System;
using System.Collections.Generic;
using System.Linq;

namespace RulesEngineApplication
{
    class RulesEngine
    {
        private Product _original_Product_ref { get; set; }
        private string _calculation_string { get; set; } = String.Empty;
        
        /// <summary>
        /// Initializes a copy of the product object for reference of the original inputed data
        /// </summary>
        /// <param name="product">User inputed data of object Product</param>
        public void SetOriginalProduct(Product product)
        {
            _original_Product_ref = product.Clone(product);
            _calculation_string += _original_Product_ref.interest_rate.ToString();
        }
        /// <summary>
        /// Returns the actions performed on the interest rate
        /// </summary>
        /// <returns>string calculations on interest rate</returns>
        public string GetCalculationString() => _calculation_string;

        /// <summary>
        /// Recursively runs through each rule and performs the condition and the executes the action if the condition is true
        /// </summary>
        /// <param name="person"></param>
        /// <param name="product"></param>
        /// <param name="rules">List of strings of a condition and action seperated by ','</param>
        /// <param name="index">For recursive use only</param>
        public void RunRules(ref Person person, ref Product product, List<string> rules, int index = 0)
        {
            if (index < rules.Count)
            {
                var rule = rules[index];
                var condition_and_actions = rule.Split(',');
                var propertyName = condition_and_actions[0];
                var compareOperator = condition_and_actions[1];
                var compareValue = condition_and_actions[2];
                var actionProperty = condition_and_actions[4];
                var actionOperator = condition_and_actions[5];
                var actionValue = condition_and_actions[6];
                //Check Condition
                var checkPerson = Compare<Person>(person, propertyName, compareOperator, compareValue);
                var checkProduct = Compare<Product>(product, propertyName, compareOperator, compareValue);

                //Execute Action
                if(checkPerson != null && checkPerson == true)
                {
                    ExecuteAction(ref product, actionProperty, actionOperator, actionValue);
                }
                else if(checkProduct != null && checkProduct == true)
                {
                    ExecuteAction(ref product, actionProperty, actionOperator, actionValue);
                }


                RunRules(ref person, ref product, rules, ++index);
            }
        }

        /// <summary>
        /// Gets the property to perform the action on, parses the operator of the action, and sets the new value based on the operator
        /// </summary>
        /// <param name="product"></param>
        /// <param name="actionProperty"></param>
        /// <param name="actionOperator"></param>
        /// <param name="actionValue"></param>
        private void ExecuteAction(ref Product product, string actionProperty, string actionOperator, string actionValue)
        {
            var properties = product.GetType().GetProperties().ToList();
            if(properties.Any(x => x.Name == actionProperty.ToLower()))
            {
                var property = properties.Where(x => x.Name == actionProperty.ToLower()).First();
                var propertyType = property.GetValue(product).GetType().FullName;
                dynamic new_value = actionValue;
                if(propertyType != typeof(string).FullName)
                {
                    new_value = TypeParser.GetValue(propertyType, actionValue);
                }
                if(new_value != null)
                {
                    var parseOperator = OperatorParser.ParseArthimeticOperator<dynamic>(actionOperator);
                    if(parseOperator != null)
                    {
                        if (property.Name == "interest_rate" && parseOperator.Method.Name == "AssignValue")
                        {
                            if (property.CanWrite)
                            {
                                property.SetValue(product, ((dynamic)property.GetValue(product)) - _original_Product_ref.interest_rate + new_value);
                                if (!string.IsNullOrEmpty(_calculation_string))
                                {
                                    _calculation_string = new_value.ToString() + _calculation_string.Substring(_original_Product_ref.interest_rate.ToString().Length);
                                }
                            }
                        }
                        else if (property.CanWrite)
                        {
                            property.SetValue(product, parseOperator.Invoke(property.GetValue(product), new_value));
                            if (actionValue.ToLower() != "true" && actionValue.ToLower() != "false")
                            {
                                if (parseOperator.Method.Name == "AddValue")
                                    _calculation_string += $" + {new_value}";
                                else _calculation_string += $" - {new_value}";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parses the condition from the string rule, gets the property to check the operator for the condition and the value to check against
        /// </summary>
        /// <typeparam name="T">The object to get the property and value information from</typeparam>
        /// <param name="compareObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="compareOperator"></param>
        /// <param name="value"></param>
        /// <returns>null if property name doesn't exist on the object or invalid data</returns>
        public bool? Compare<T>(T compareObject, string propertyName, string compareOperator, string value)
        {
            var properties = compareObject.GetType().GetProperties().ToList();
            if (properties.Any(x => x.Name == propertyName.ToLower()))
            {
                var property = properties.Where(x => x.Name == propertyName.ToLower()).First();
                var propertyType = property.GetValue(compareObject).GetType().FullName;
                dynamic new_value = value;
                if (propertyType != typeof(string).FullName)
                {
                    new_value = TypeParser.GetValue(propertyType, value);
                }
                if(new_value != null)
                {
                    var parsedOperator = OperatorParser.ParseOperator<dynamic>(compareOperator);
                    if (parsedOperator != null)
                    {
                        if (property.Name == "interest_rate")
                        {
                            var result = (bool)parsedOperator.Invoke(_original_Product_ref.interest_rate, new_value);
                            return result;
                        }
                        else
                        {
                            var result = (bool)parsedOperator.Invoke(property.GetValue(compareObject), new_value);
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}
