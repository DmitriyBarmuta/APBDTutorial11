namespace Tutorial11.Exceptions;

public class NoSuchDoctorException : Exception
{
    public NoSuchDoctorException(string? message) : base(message)
    {
    }
}