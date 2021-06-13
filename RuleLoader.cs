using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace RulesEngineApplication
{
    static class RuleLoader
    {
        /// <summary>
        /// Opens the rules file and loads in the list of strings from the CSV, separated by ','
        /// </summary>
        /// <returns>The list of strings read in from the file</returns>
        public static List<string> LoadRules()
        {
            try
            {
                Console.WriteLine("Loading Rules..\n");
                var rules = new List<string>();
                var filePath = ConfigurationManager.AppSettings["RulesPath"];
                if(filePath == "[Enter Path Here]")
                {
                    Console.WriteLine("Path to rules file invalid please input the full path of the rules file: ");
                    filePath = Console.ReadLine().Trim();
                }
                var file = File.OpenRead(filePath);
                var reader = new StreamReader(file);
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    rules.Add(reader.ReadLine());
                }

                reader.Close();
                file.Close();

                return rules;
            }
            catch (IOException e)
            {
                Console.WriteLine($"Load rules encountered an error: {e}");
                return new List<string>();
            }
        }
    }
}
