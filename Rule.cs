using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineApplication
{
    class Rule
    {
        public List<Condition> conditions { get; set; }
        public List<Action> actions { get; set; }
        public bool all_conditions_needed { get; set; }
    }
}
