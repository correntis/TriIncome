namespace LoanApi.Models;

public class CreateLoanDto
{
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int TermValue { get; set; }
    public TermUnit TermUnit { get; set; } = TermUnit.Days;
    public decimal InterestValue { get; set; }
}

