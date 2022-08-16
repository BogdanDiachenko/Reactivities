namespace Reactivities.Core.Models;

public class ServiceResult<TValue>
{
    public bool Success { get; set; }

    public TValue Value { get; set; }

    public string Error { get; set; }

    public static ServiceResult<TValue> CreateSuccess(TValue value) => new()
    {
        Value = value,
        Success = true
    };
    
    public static ServiceResult<TValue> CreateFailure(string error) => new()
    {
        Error = error,
        Success = false
    };
}