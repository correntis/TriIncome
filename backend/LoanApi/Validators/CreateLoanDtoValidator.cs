using FluentValidation;
using LoanApi.Models;

namespace LoanApi.Validators;

public class CreateLoanDtoValidator : AbstractValidator<CreateLoanDto>
{
    public CreateLoanDtoValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Номер заявки обязателен")
            .MaximumLength(50)
            .WithMessage("Номер заявки не должен превышать 50 символов");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Сумма должна быть больше 0");

        RuleFor(x => x.TermValue)
            .GreaterThan(0)
            .WithMessage("Срок займа должен быть больше 0");

        RuleFor(x => x.InterestValue)
            .GreaterThan(0)
            .WithMessage("Процентная ставка должна быть больше 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Процентная ставка не может превышать 100%");
    }
}

