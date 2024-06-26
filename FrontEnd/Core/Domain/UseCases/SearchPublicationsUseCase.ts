import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { RequestAbortedError } from "../../Services/RequestAbortedError";
import { Result } from "../../Util/Result";
import { PosterrAPIErrorResponse, PublicationsListAPIResource } from "../../types";

export async function SearchPublicationsUseCase(
  searchTerm: string,
  signal: AbortSignal
):
  Promise<Result<PublicationsListAPIResource, string | PosterrAPIErrorResponse | RequestAbortedError>>
{
  if (!searchTerm) {
    return Result.Error("The search term must not be empty");
  }

  return ApiEndpoint.publications.search(searchTerm, signal);
}
