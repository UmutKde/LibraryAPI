namespace LibrarySystem.Domain.Exceptions;

public class UserLoanLimitExceededException : ConflictException
{
    public UserLoanLimitExceededException(int maxLimit)
        : base($"User has reached the maximum loan limit of {maxLimit}")
    {
    }
}