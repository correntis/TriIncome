using Microsoft.EntityFrameworkCore;
using LoanApi.Data;
using LoanApi.Models;
using LoanApi.Exceptions;

namespace LoanApi.Services;

public class LoanService : ILoanService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<LoanService> _logger;

    public LoanService(ApplicationDbContext context, ILogger<LoanService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Loan>> GetLoansAsync(
        LoanStatus? status,
        decimal? minAmount,
        decimal? maxAmount,
        int? minTerm,
        int? maxTerm)
    {
        var query = _context.Loans.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }

        if (minAmount.HasValue)
        {
            query = query.Where(l => l.Amount >= minAmount.Value);
        }
        if (maxAmount.HasValue)
        {
            query = query.Where(l => l.Amount <= maxAmount.Value);
        }

        if (minTerm.HasValue)
        {
            query = query.Where(l => l.TermValue >= minTerm.Value);
        }
        if (maxTerm.HasValue)
        {
            query = query.Where(l => l.TermValue <= maxTerm.Value);
        }

        var loans = await query
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();

        _logger.LogInformation("Получено {Count} заявок с фильтрами", loans.Count);

        return loans;
    }

    public async Task<Loan?> GetLoanByIdAsync(int id)
    {
        var loan = await _context.Loans.FindAsync(id);

        if (loan == null)
        {
            _logger.LogWarning("Заявка {LoanId} не найдена", id);
            throw new NotFoundException($"Заявка с ID {id} не найдена");
        }

        _logger.LogInformation("Получена заявка {LoanId}", id);
        return loan;
    }

    public async Task<Loan> CreateLoanAsync(CreateLoanDto createLoanDto)
    {
        var existingLoan = await _context.Loans
            .FirstOrDefaultAsync(l => l.Number == createLoanDto.Number);

        if (existingLoan != null)
        {
            _logger.LogWarning("Попытка создать заявку с существующим номером {Number}", createLoanDto.Number);
            throw new ValidationException($"Заявка с номером {createLoanDto.Number} уже существует");
        }

        var loan = new Loan
        {
            Number = createLoanDto.Number,
            Amount = createLoanDto.Amount,
            TermValue = createLoanDto.TermValue,
            TermUnit = createLoanDto.TermUnit,
            InterestValue = createLoanDto.InterestValue,
            Status = LoanStatus.Published,
            CreatedAt = DateTimeOffset.UtcNow,
            ModifiedAt = DateTimeOffset.UtcNow
        };

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Создана новая заявка {LoanNumber} с ID {LoanId}", loan.Number, loan.Id);

        return loan;
    }

    public async Task<Loan?> ToggleLoanStatusAsync(int id)
    {
        var loan = await _context.Loans.FindAsync(id);

        if (loan == null)
        {
            _logger.LogWarning("Попытка изменить статус несуществующей заявки {LoanId}", id);
            throw new NotFoundException($"Заявка с ID {id} не найдена");
        }

        var oldStatus = loan.Status;

        loan.Status = loan.Status == LoanStatus.Published
            ? LoanStatus.Unpublished
            : LoanStatus.Published;

        loan.ModifiedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Статус заявки {LoanNumber} изменен с {OldStatus} на {NewStatus}",
            loan.Number,
            oldStatus,
            loan.Status);

        return loan;
    }
}

