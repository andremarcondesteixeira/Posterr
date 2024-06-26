import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { RequestAbortedError } from "../../Services/RequestAbortedError";
import { Result } from "../../Util/Result";
import { PosterrAPIErrorResponse, PublicationsListAPIResource } from "../../types";
import { SortingOrder } from "../SortingOrder";

export async function ListPublicationsUseCase(
  lastSeenPublicationId: number,
  sortingOrder: SortingOrder,
  signal: AbortSignal
): Promise<Result<PublicationsListAPIResource, PosterrAPIErrorResponse | RequestAbortedError | string>> {
  if (!Number.isInteger(lastSeenPublicationId) || lastSeenPublicationId < 0) {
    return Result.Error<PublicationsListAPIResource, string>(
      `Last seen publication ID must be a positive integer or 0 (zero). Got ${lastSeenPublicationId} instead.`
    );
  }

  return ApiEndpoint.publications.GET(lastSeenPublicationId, sortingOrder, lastSeenPublicationId === 0, signal);
}
