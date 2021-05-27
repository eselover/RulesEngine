using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RulesEngineApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            RulesEngine rulesEngine = new RulesEngine();

            var rules = RuleLoader.LoadRules();

            var product = GetProductInformation();

            var person = GetPersonInformation();

            rulesEngine.RunRules(person, product, rules);

            Console.WriteLine("Run again? (y/n): ");
            var answer = Console.ReadLine();

            if (answer == "Y" || answer == "y") Main(args);
        }

        /// <summary>
        /// Prompts user to get Product object information
        /// </summary>
        /// <returns>Product object</returns>
        private static Product GetProductInformation()
        {
            var product = new Product();

            Console.WriteLine("Please enter the name of the product: ");
            product.name = Console.ReadLine().Trim();

            Console.WriteLine("Please enter the base interest rate of the product (e.g 5.0): ");
            string interest_rate_string = Console.ReadLine().Trim();

            if(decimal.TryParse(interest_rate_string, out decimal result))
            {
                product.interest_rate = result;
            }

            return product;
        }

        /// <summary>
        /// Prompts user to get Person object information
        /// </summary>
        /// <returns>Person object</returns>
        private static Person GetPersonInformation()
        {
            var person = new Person();

            Console.WriteLine("Please enter the state the person lives in: ");
            person.state = Console.ReadLine().Trim();

            Console.WriteLine("Please enter the person's credit score: ");
            string credit_score_string = Console.ReadLine().Trim();

            if(int.TryParse(credit_score_string, out int result))
            {
                person.credit_score = result;
            }

            return person;
        }
    }
}
