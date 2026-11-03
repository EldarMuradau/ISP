namespace ipr1.Models;

public class Rate
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public string Abbreviation { get; set; } = "";
    public int Scale { get; set; }
    public string Name { get; set; } = "";
    public decimal? OfficialRate { get; set; }

    public string Display => $"{Name} ({Abbreviation})";
}
