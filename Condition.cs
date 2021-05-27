using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineApplication
{
    class Condition
    {
        public PropertyNameIDs property_id { get; set; }
        public string comparison_operator { get; set; }
        public dynamic comparison_value { get; set; }
        public bool conditions_met { get; set; }
    }
}
