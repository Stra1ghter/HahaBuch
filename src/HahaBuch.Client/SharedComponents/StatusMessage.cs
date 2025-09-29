namespace HahaBuch.Client.SharedComponents;

public enum StatusType
{
    Success,
    Error,
    Warning,
    Info
}

public record StatusMessage(string Message, StatusType Type = StatusType.Info)
{
    public static StatusMessage Success(string message) => new(message, StatusType.Success);
    public static StatusMessage Warning(string message) => new(message, StatusType.Warning);
    public static StatusMessage Error(string message) => new(message, StatusType.Error);
    public static StatusMessage Info(string message) => new(message);
}