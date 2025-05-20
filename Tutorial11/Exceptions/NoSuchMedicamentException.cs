namespace Tutorial11.Exceptions;

public class NoSuchMedicamentException : Exception
{
    public NoSuchMedicamentException(string? message) : base(message)
    {
    }
}