namespace Posterr.Core.Application.UseCases.PaginatePublications;

public sealed record PaginatePublicationsRequestDTO(int PageNumber, short PageSize);
