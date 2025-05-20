namespace Tutorial11.Exceptions;

public class TooMuchMedicationsException : Exception
{
    public TooMuchMedicationsException(string? message) : base(message)
    {
    }
}