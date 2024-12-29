namespace Selah.Core.ApiContracts;

public class BaseHttpResponse<T>
{
    public int StatusCode { get; set; }

    public T? Data { get; set; }

    public List<FluentValidation.Results.ValidationFailure> Errors{ get; set; } = new List<FluentValidation.Results.ValidationFailure>();
}

/*
 * Most API commands can simply return the newly created or updated Id  so this
   provides a common interface of accomplishing that
 */
public class BaseCommandResponse
{
    public string Id { get; set; }
}