namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed record CreateNewPostRequest
{
    public string Username { get; }
    public string Content { get; }

    public CreateNewPostRequest(string username, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
        Username = username;
        Content = content;
    }
}
