namespace LibrarySystem.Domain.Exceptions;

public class CategoryAlreadyExistsException : ConflictException
{
    public CategoryAlreadyExistsException(string categoryName)
        : base($"The category with name : {categoryName} already exists.")
    {
    }
}