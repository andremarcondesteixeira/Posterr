import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { RequestAbortedError } from "../../Services/RequestAbortedError";
import { Result } from "../../Util/Result";
import config from "../../config.json";
import { PosterrAPIErrorResponse, PublicationAPIResource } from "../../types";

export async function CreateNewRepostUseCase(
  authorUsername: string,
  content: string,
  originalPostId: number,
): Promise<Result<PublicationAPIResource, PosterrAPIErrorResponse | RequestAbortedError | string>> {
  if (!authorUsername) {
    return Result.Error<PublicationAPIResource, string>("The author's username must not be empty");
  }

  if (content.length > config.maxPublicationContentLength) {
    return Result.Error<PublicationAPIResource, string>("The content must not have more than 777 characters.");
  }

  if (!Number.isInteger(originalPostId) || originalPostId < 1) {
    return Result.Error<PublicationAPIResource, string>(`The original post ID must be a positive integer. Got ${originalPostId} instead.`);
  }

  return ApiEndpoint.publications.PATCH(authorUsername, content, originalPostId);
}
