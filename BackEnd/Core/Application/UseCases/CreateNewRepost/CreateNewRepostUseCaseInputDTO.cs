namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostUseCaseInputDTO(string AuthorUsername, string Content, long OriginalPostId);
