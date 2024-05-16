namespace Posterr.Core.Application.UseCases.PaginatePublications;

public sealed record PaginatePublicationsRequest(int LastSeenRow, short PageSize);
