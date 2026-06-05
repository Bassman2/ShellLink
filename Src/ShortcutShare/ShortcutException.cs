namespace ShortcutShare;

public class ShortcutException : ApplicationException
{
    public ShortcutException() : base()
    { }

    public ShortcutException(string message) : base(message)
    { }

    public ShortcutException(string message, Exception innerException) : base(message, innerException)
    { }
}
