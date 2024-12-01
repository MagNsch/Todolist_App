namespace TodoList_App.Exceptions;

public class CustomValidationException : Exception
{
    public string PropertyName { get; set; }

    public object RejectedValue { get; set; }


    public CustomValidationException(string message, string propertyName = null, object rejectedValue = null)
        : base(message)
    {
        PropertyName = propertyName;
        RejectedValue = rejectedValue;
    }

}
