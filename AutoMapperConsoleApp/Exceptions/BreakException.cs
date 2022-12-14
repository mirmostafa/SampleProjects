namespace AutoMapperConsoleApp.Exceptions;

[Serializable]
public class BreakException : Exception
{
    public BreakException()
    {
    }

    /// <summary>
    /// Throws a new instance of Break Exception
    /// </summary>
    /// <exception cref="HanyCo.Mes20.Infra.Exceptions.BreakException"></exception>
    [DoesNotReturn]
    public static void Throw()
        => throw new BreakException();
}