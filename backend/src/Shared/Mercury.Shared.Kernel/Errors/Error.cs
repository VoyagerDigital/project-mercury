using FluentResults;

namespace Mercury.Shared.Kernel.Errors;

public static class Error
{
    public abstract class NotFound : IError
    {
        public string Message { get; set; } = null!;
        public Dictionary<string, object> Metadata { get; set; } = null!;
        public List<IError> Reasons { get; set; } = null!;
    }
    
    public sealed class NotFound<T> : NotFound
    {
        public NotFound()
        {
            Message = $"{typeof(T).Name}.NotFound";
        }
    }
    
    public sealed class ValidationError<T> : IError
    {
        public string Message { get; set; } = null!;
        public Dictionary<string, object> Metadata { get; set; } = null!;
        public List<IError> Reasons { get; set; } = null!;
    }
}