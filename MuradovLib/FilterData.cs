namespace Muradov.Domain
{
    public class FilterData
    {
        public int? Century { get; set; }
        public decimal? MinEstimatedValue { get; set; }
        public decimal? MaxEstimatedValue { get; set; }
        public bool? RequiresRestoration { get; set; }
    }

    public class FilterVariant
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public FilterData Filter { get; set; } = new();
    }
}
