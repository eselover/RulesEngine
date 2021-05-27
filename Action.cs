namespace RulesEngineApplication
{
    class Action
    {
        public PropertyNameIDs property_id { get; set; }
        public dynamic new_value { get; set; }
        public bool is_base_value_action { get; set; }
    }
}
