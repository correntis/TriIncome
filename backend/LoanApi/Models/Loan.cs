namespace LoanApi.Models;

public class Loan
{
    public int Id { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Published;
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int TermValue { get; set; }
    public TermUnit TermUnit { get; set; } = TermUnit.Days;
    public decimal InterestValue { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
}

