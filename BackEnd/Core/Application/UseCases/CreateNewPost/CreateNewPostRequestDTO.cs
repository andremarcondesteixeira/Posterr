namespace Posterr.Core.Application.UseCases.CreateNewPost;

public sealed record CreateNewPostRequestDTO
{
    public string Username { get; }
    public string Content { get; }

    public CreateNewPostRequestDTO(string username, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
        Username = username;
        Content = content;
    }
}
