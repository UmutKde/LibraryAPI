using LibrarySystem.Application.Dtos;

namespace LibrarySystem.Application.Interfaces;

public interface ILoanService
{
    Task<LoanResponseDto> CreateLoan(LoanDtoForInsertion loanDtoForInsertion);
    Task<(decimal totalCost, string message)> UpdateLoan(LoanDtoForUpdate loanDtoForUpdate);

    Task<IEnumerable<LoanDto>> GetAllActiveLoanAsync();
    Task<IEnumerable<LoanDto>> GetAllLoansAsync();
    Task<IEnumerable<LoanDto>> GetLoansByUserIdAsync(int userId);
    Task<IEnumerable<LoanDto>> GetActiveLoansByUserIdAsync(int userId);
    Task<LoanDtoForGet> GetLoanByIdAsync(int id);
    Task<LoanDto> GetLoanByBarcode(string barcode);

}