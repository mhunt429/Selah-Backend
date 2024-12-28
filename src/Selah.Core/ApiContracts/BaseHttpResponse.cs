namespace Selah.Core.ApiContracts;

public class BaseHttpResponse<T>
{
    public int StatusCode { get; set; }

    public T? Data { get; set; }

    public List<string> Errors { get; set; } = new List<string>();
}

/*
 * Most API commands can simply return the newly created or updated Id  so this
   provides a common interface of accomplishing that
 */
public class BaseCommandResponse
{
    public string Id { get; set; }
}