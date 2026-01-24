namespace LibrarySystem.Domain.ErrorModels;

public class ValidationErrorDetails : ErrorDetails
{
    public IEnumerable<string> Errors {get;set;}

    public ValidationErrorDetails()
    {
        Errors = new List<string>();
    }
}