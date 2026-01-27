namespace LibrarySystem.Domain.Exceptions;

public class DueDateHasPassedException : ConflictException
{
    public DueDateHasPassedException(DateTime dateTime)
        : base($"Book copy due date {dateTime} has passed. Please initiate legal proceedings.")
    {
    }
}