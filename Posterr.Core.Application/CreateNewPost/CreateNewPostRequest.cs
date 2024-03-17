namespace Posterr.Core.Application.CreateNewPost;

public sealed record CreateNewPostRequest
{
    public string Content { get; }
    public string Username { get; }

    public CreateNewPostRequest(string content, string username)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        Content = content;
        Username = username;
    }
}
