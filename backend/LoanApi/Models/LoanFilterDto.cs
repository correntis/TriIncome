namespace LoanApi.Models;

public class LoanFilterDto
{
    public LoanStatus? Status { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public int? MinTerm { get; set; }
    public int? MaxTerm { get; set; }
}

