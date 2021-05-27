using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineApplication
{
    class RulesEngine
    {
        /// <summary>
        /// Iterates over each rule object and compares the rule values against its associated object properties
        /// </summary>
        /// <param name="person">A Person object to compare the rules to</param>
        /// <param name="product">A Product object to compare the rules to</param>
        /// <param name="rules">A list of Rule objects to iterate over</param>
        public void RunRules(Person person, Product product, List<Rule> rules)
        {
            List<Action> actions_to_execute = new List<Action>();

            foreach(var rule in rules)
            {
                foreach(var condition in rule.conditions)
                {
                    switch (condition.property_id)
                    {
                        case PropertyNameIDs.STATE:
                            condition.conditions_met = Compare(person.state, condition.comparison_operator, condition.comparison_value);
                            break;
                        case PropertyNameIDs.CREDIT_SCORE:
                            condition.conditions_met = Compare(person.credit_score, condition.comparison_operator, condition.comparison_value);
                            break;
                        case PropertyNameIDs.NAME:
                            condition.conditions_met = Compare(product.name, condition.comparison_operator, condition.comparison_value);
                            break;
                        case PropertyNameIDs.INTEREST_RATE:
                            condition.conditions_met = Compare(product.interest_rate, condition.comparison_operator, condition.comparison_value);
                            break;
                        default:
                            break;
                    }
                    if(rule.all_conditions_needed)
                    {
                        if(rule.conditions.All(x => x.conditions_met))
                        {
                            foreach(var action in rule.actions)
                            {
                                actions_to_execute.Add(action);
                            }
                        }
                    }
                    else
                    {
                        if(rule.conditions.Any(x => x.conditions_met))
                        {
                            foreach(var action in rule.actions)
                            {
                                actions_to_execute.Add(action);
                            }
                        }
                    }
                }
            }

            ExecuteActions(ref product, actions_to_execute);
        }


        /// <summary>
        /// Executes each of the action
        /// </summary>
        /// <param name="product">An object of Type Product that the actions will change the property values</param>
        /// <param name="actions">List of actions for the conditions met</param>
        private void ExecuteActions(ref Product product, List<Action> actions)
        {
            var original_product_interest_rate = product.interest_rate;
            string interest_rate_changes = "";

            foreach(var action in actions)
            {
                switch (action.property_id)
                {
                    case PropertyNameIDs.INTEREST_RATE:
                        if (action.is_base_value_action)
                        {
                            Console.WriteLine("Base Interest rate is lower than the starting rate rule - setting base interest rate to rule requirements...");
                            product.interest_rate = (product.interest_rate - original_product_interest_rate) + action.new_value;
                            original_product_interest_rate = action.new_value;
                        }
                        else
                        {
                            product.interest_rate += action.new_value;
                            interest_rate_changes += $" + {action.new_value}";
                        }
                        break;
                    case PropertyNameIDs.DISQUALIFIED:
                        product.disqualified = action.new_value;
                        break;
                    default:
                        break;
                }
            }

            DisplayResults(product, original_product_interest_rate.ToString() + interest_rate_changes);
        }


        /// <summary>
        /// Method to parse the 'Comparison Operator' string from a property
        /// </summary>
        /// <param name="current_value">The value to compare against the rule</param>
        /// <param name="comparison_operator">The comparison operator as a string to compare the two values</param>
        /// <param name="new_value">The rule value to compare an object to</param>
        /// <returns></returns>
        private bool Compare(dynamic current_value, string comparison_operator, dynamic new_value)
        {
            switch (comparison_operator)
            {
                case ">":
                    return current_value > new_value;
                case "<":
                    return current_value < new_value;
                case "<=":
                    return current_value <= new_value;
                case ">=":
                    return current_value >= new_value;
                case "==":
                    return current_value == new_value;
                case "!=":
                    return current_value != new_value;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Displays the result of the actions
        /// </summary>
        /// <param name="product">The object that has been changed via the actions</param>
        /// <param name="interest_rate_changes">Breakout of Interest rate changes as a string</param>
        private void DisplayResults(Product product, string interest_rate_changes)
        {
            Console.WriteLine($"\nProduct Name: \t{product.name}");
            Console.WriteLine($"\nProduct Interest Rate: \t{product.interest_rate} ({interest_rate_changes})");
            Console.WriteLine($"\nPerson Disqualified: \t{product.disqualified}");
        }
    }
}
