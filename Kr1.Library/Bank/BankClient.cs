namespace Kr1.Library.Bank;

public class BankClient
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime AccountOpenedDate { get; set; }

    public override string ToString() => $"{Id}: {FullName}, account opened {AccountOpenedDate:yyyy-MM-dd}";
}
