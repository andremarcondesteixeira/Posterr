namespace Posterr.Core.Application.UseCases.PaginatePublications;

public sealed record PaginatePublicationRequest(int LastSeenRow, short PageSize);
