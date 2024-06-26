import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { RequestAbortedError } from "../../Services/RequestAbortedError";
import { Result } from "../../Util/Result";
import { PosterrAPIErrorResponse, PublicationsListAPIResource } from "../../types";

export async function SearchPublicationsUseCase(
  searchTerm: string,
  lastSeenPublicationId: number,
  signal: AbortSignal
):
  Promise<Result<PublicationsListAPIResource, string | PosterrAPIErrorResponse | RequestAbortedError>>
{
  if (!searchTerm) {
    return Result.Error("The search term must not be empty");
  }

  if (!Number.isInteger(lastSeenPublicationId) || lastSeenPublicationId < 0) {
    return Result.Error<PublicationsListAPIResource, string>(
      `Last seen publication ID must be a positive integer or 0 (zero). Got ${lastSeenPublicationId} instead.`
    );
  }

  return ApiEndpoint.publications.search(searchTerm, lastSeenPublicationId, lastSeenPublicationId === 0, signal);
}
