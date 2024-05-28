namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostRequestDTO(string AuthorUsername, long OriginalPostId);
