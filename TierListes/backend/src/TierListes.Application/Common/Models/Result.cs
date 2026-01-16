namespace TierListes.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; }

    private Result(bool isSuccess, T? data, string? errorMessage, int statusCode)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T data, int statusCode = 200) =>
        new(true, data, null, statusCode);

    public static Result<T> Failure(string errorMessage, int statusCode = 400) =>
        new(false, default, errorMessage, statusCode);

    public static Result<T> NotFound(string errorMessage = "Resource not found") =>
        new(false, default, errorMessage, 404);

    public static Result<T> Conflict(string errorMessage) => new(false, default, errorMessage, 409);

    public static Result<T> Unauthorized(string errorMessage = "Unauthorized") =>
        new(false, default, errorMessage, 401);
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; private set; }

    private Result(bool isSuccess, string? errorMessage, int statusCode)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }

    public static Result Success(int statusCode = 200) => new(true, null, statusCode);

    public static Result Failure(string errorMessage, int statusCode = 400) =>
        new(false, errorMessage, statusCode);
}
