using System;

namespace RulesEngineApplication
{
    class Product
    {
        public string name { get; set; }
        public decimal interest_rate { get; set; }
        public bool disqualified { get; set; }

        public Product Clone(Product product)
        {
            return (Product)product.MemberwiseClone();
        }

    }
}
