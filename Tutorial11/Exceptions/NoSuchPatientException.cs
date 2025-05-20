namespace Tutorial11.Exceptions;

public class NoSuchPatientException : Exception
{
    public NoSuchPatientException(string? message) : base(message)
    {
    }
}