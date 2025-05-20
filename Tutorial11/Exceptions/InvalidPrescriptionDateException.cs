namespace Tutorial11.Exceptions;

public class InvalidPrescriptionDateException : Exception
{
    public InvalidPrescriptionDateException(string? message) : base(message)
    {
    }
}