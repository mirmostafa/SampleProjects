using AutoMapperConsoleApp.Exceptions;

namespace Library.Exceptions;

public interface IApiException : IException
{
    int? StatusCode { get; }
}
