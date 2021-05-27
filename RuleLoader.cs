using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace RulesEngineApplication
{
    static class RuleLoader
    {
        /// <summary>
        /// Reads the list of rules in from a CSV file
        /// </summary>
        /// <returns>A list of Rule objects</returns>
        public static List<Rule> LoadRules()
        {
            try
            {
                Console.WriteLine("Loading Rules... \n");
                var rules = new List<Rule>();

                //Loads a local copy of the rules file

                var filePath = ConfigurationManager.AppSettings["RulesPath"];
                if (filePath == "[Enter Path Here]")
                {
                    Console.WriteLine("Path to rules file invalid please input the full path of the rules file: ");
                    filePath = Console.ReadLine().Trim();
                }
                var file = File.OpenRead(filePath);
                var reader = new StreamReader(file);

                //Skip First two rows of CSV
                reader.ReadLine();
                reader.ReadLine();
                int ruleCount = 0;
                while (!reader.EndOfStream)
                {
                    ruleCount++;
                    string rule_string = reader.ReadLine();
                    string[] rule_values = rule_string.Split(',');
                    var rule = new Rule();
                    var conditions = new List<Condition>();
                    var actions = new List<Action>();

                    for (int i = 0; i < rule_values.Length; i++)
                    {
                        if (i < 4 && !string.IsNullOrEmpty(rule_values[i]))
                        {
                            if (i == 5) continue;
                            var condition = new Condition();
                            switch (i)
                            {
                                case 0: //State
                                    condition.property_id = PropertyNameIDs.STATE;
                                    condition.comparison_value = rule_values[i];
                                    condition.comparison_operator = "==";
                                    break;
                                case 1: // Credit score
                                    condition.property_id = PropertyNameIDs.CREDIT_SCORE;
                                    if (rule_values[i].StartsWith(">=") || rule_values[i].StartsWith("<="))
                                    {
                                        condition.comparison_operator = rule_values[i].Substring(0, 2);
                                        if (int.TryParse(rule_values[i].Substring(2), out int integer_value))
                                        {
                                            condition.comparison_value = integer_value;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Rule #{ruleCount + 1} Error: Credit score has invalid Data");
                                        }
                                    }
                                    else
                                    {
                                        condition.comparison_operator = rule_values[i].Substring(0, 1);
                                        if (int.TryParse(rule_values[i].Substring(1), out int integer_result))
                                        {
                                            condition.comparison_value = integer_result;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Rule #{ruleCount + 1} Error: Credit score has invalid Data");
                                        }
                                    }
                                    break;
                                case 2: // Product Name
                                    condition.property_id = PropertyNameIDs.NAME;
                                    condition.comparison_operator = "==";
                                    condition.comparison_value = rule_values[i];
                                    break;
                                case 3: // Product Interest Rate
                                    condition.property_id = PropertyNameIDs.INTEREST_RATE;
                                    condition.comparison_operator = "<";
                                    if (decimal.TryParse(rule_values[i], out decimal result))
                                    {
                                        condition.comparison_value = result;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Rule #{ruleCount + 1} Error: Product Interest Rate value has invalid data");
                                    }
                                    break;
                                default:
                                    break;
                            }
                            conditions.Add(condition);
                        }
                        else if (i > 5 && !string.IsNullOrEmpty(rule_values[i]))
                        {
                            var action = new Action();
                            if (!string.IsNullOrEmpty(rule_values[4]))
                            {
                                if (rule_values[4] == "BASE") action.is_base_value_action = true;
                                else if (rule_values[4] == "All") rule.all_conditions_needed = true;
                            }
                            switch (i)
                            {
                                case 6: // Action Interest Rate Change
                                    action.property_id = PropertyNameIDs.INTEREST_RATE;
                                    if (decimal.TryParse(rule_values[i], out decimal action_decimal))
                                    {
                                        action.new_value = action_decimal;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Rule #{ruleCount + 1} Error: Interest Action has invalid data");
                                    }
                                    break;
                                case 7: // Action Disqualification Change
                                    action.property_id = PropertyNameIDs.DISQUALIFIED;
                                    if (bool.TryParse(rule_values[i], out bool new_value))
                                    {
                                        action.new_value = new_value;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Rule #{ruleCount + 1} Error: Disqualification Action has invalid data");
                                    }
                                    break;
                                default:
                                    break;
                            }
                            actions.Add(action);
                        }
                    }
                    rule.conditions = conditions;
                    rule.actions = actions;
                    rules.Add(rule);
                }

                reader.Close();
                file.Close();

                return rules;
            }
            catch (IOException e)
            {
                Console.WriteLine($"Load rules encountered an error: {e}");
                return new List<Rule>();
            }
        }
    }
}
