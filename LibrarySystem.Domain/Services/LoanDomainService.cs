using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;

namespace LibrarySystem.Domain.Services;

public class LoanDomainService
{
    private const int maxLoanLimit = 3;
    public void ValidateLoanEligibility(int userActiveLoanCount,bool isCopyAvailable, int bookId)
    {
        if(userActiveLoanCount >= maxLoanLimit)
        {
            throw new UserLoanLimitExceededException(userActiveLoanCount);
        }

        if(!isCopyAvailable)
        {
            throw new BookOutOfStockException(bookId);
        }
    }
}