namespace Muradov.Domain
{
    public class MuseumExhibit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Century { get; set; }
        public decimal EstimatedValue { get; set; }
        public bool RequiresRestoration { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
