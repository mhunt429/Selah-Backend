namespace Selah.Core.Models;

public record ApiResponseResult<T>(ResultStatus status, string? message, T? data);

public record DbOperationResult(ResultStatus status, string? message);

public enum ResultStatus
{
    Success,
    Failed
}