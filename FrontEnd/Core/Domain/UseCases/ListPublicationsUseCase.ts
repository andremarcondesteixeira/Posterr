import { ApiEndpoint, PublicationsListAPIResource } from "../../Services/ApiEndpointsService";
import { PosterrAPIErrorResponse } from "../../Services/PosterrAPIErrorResponse";
import { RequestAbortedError } from "../../Services/RequestAbortedError";
import { Result } from "../../Util/Result";

export async function ListPublicationsUseCase(
  lastSeenPublicationId: number,
  signal: AbortSignal
): Promise<Result<PublicationsListAPIResource, PosterrAPIErrorResponse | RequestAbortedError | string>> {
  if (!Number.isInteger(lastSeenPublicationId) || lastSeenPublicationId < 0) {
    return Result.Error<PublicationsListAPIResource, string>(
      `Last seen publication ID must be a positive integer or 0 (zero). Got ${lastSeenPublicationId} instead.`
    );
  }

  return ApiEndpoint.publications.GET(lastSeenPublicationId, lastSeenPublicationId === 0, signal);
}
