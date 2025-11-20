using LoanApi.Models;

namespace LoanApi.Services;

public interface ILoanService
{
    Task<IEnumerable<Loan>> GetLoansAsync(LoanStatus? status, decimal? minAmount, decimal? maxAmount, int? minTerm, int? maxTerm);
    Task<Loan?> GetLoanByIdAsync(int id);
    Task<Loan> CreateLoanAsync(CreateLoanDto createLoanDto);
    Task<Loan?> ToggleLoanStatusAsync(int id);
}

