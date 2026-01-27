using LibrarySystem.Application.Dtos;

namespace LibrarySystem.Application.Interfaces;

public interface ILoanService
{
    Task<LoanResponseDto> CreateLoan(LoanDtoForInsertion loanDtoForInsertion);
    Task<(decimal totalCost, string message)> UpdateLoan(LoanDtoForUpdate loanDtoForUpdate);
}