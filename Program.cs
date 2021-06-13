using System;

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

            rulesEngine.SetOriginalProduct(product);

            rulesEngine.RunRules(ref person, ref product, rules);

            DisplayResults(product, rulesEngine.GetCalculationString());

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

            if (decimal.TryParse(interest_rate_string, out decimal result))
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

            Console.WriteLine("Please enter the city the person lives in: ");
            person.city = Console.ReadLine().Trim();

            Console.WriteLine("Please enter the person's credit score: ");
            string credit_score_string = Console.ReadLine().Trim();

            if (int.TryParse(credit_score_string, out int result))
            {
                person.credit_score = result;
            }

            return person;
        }



        /// <summary>
        /// Displays the result of the actions
        /// </summary>
        /// <param name="product">The object that has been changed via the actions</param>
        /// <param name="interest_rate_changes">Breakout of Interest rate changes as a string</param>
        private static void DisplayResults(Product product, string interest_rate_changes)
        {
            Console.WriteLine($"\nProduct Name: \t{product.name}");
            Console.WriteLine($"\nProduct Interest Rate: \t{product.interest_rate} ({interest_rate_changes})");
            Console.WriteLine($"\nPerson Disqualified: \t{product.disqualified}");
        }
    }
}
