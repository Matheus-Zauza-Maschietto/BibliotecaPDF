namespace bibliotecaPDF.Exceptions;

public class NotFoundException : BussinessException
{
    public NotFoundException(string errorMessage) : base(errorMessage)
    {

    }
}
