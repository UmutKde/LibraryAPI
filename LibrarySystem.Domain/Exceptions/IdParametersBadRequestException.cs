namespace LibrarySystem.Domain.Exceptions;

public sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException(int parameterId,int bodyId)
        : base($"Parameter id: {parameterId} and body id: {bodyId} do not match.")
    {
    }
}