namespace PuppyLearn.Models
{
    public class TableFilters
    {
        public string Msg { get; set; } = null!;
        public Dictionary<string, List<FilterMetadata>>? Filters { get; set; }
    }

    public class FilterMetadata 
    { 
        public string? Value { get; set; }
        public string? MatchMode { get; set; }
        public string? Operator { get; set; }
    }

}
