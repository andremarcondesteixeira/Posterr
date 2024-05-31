namespace Posterr.Core.Application.UseCases.CreateNewRepost;

public sealed record CreateNewRepostUseCaseInputDTO(string AuthorUsername, long OriginalPostId);
