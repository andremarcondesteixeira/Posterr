import { ApiEndpoint, PublicationAPIResource } from "../../Services/ApiEndpointsService";
import { PosterrAPIErrorResponse } from "../../Services/PosterrAPIErrorResponse";
import { RequestAbortedError } from "../../Services/RequestAbortedError";
import { Result } from "../../Util/Result";
import config from "../config.json";

export async function CreateNewPostUseCase(
  authorUsername: string,
  content: string,
): Promise<Result<PublicationAPIResource, PosterrAPIErrorResponse | RequestAbortedError | string>> {
  if (!authorUsername) {
    return Result.Error<PublicationAPIResource, string>("The author's username must not be empty.");
  }

  if (!content) {
    return Result.Error<PublicationAPIResource, string>("The content must not be empty. Write something.");
  }

  if (content.length > config.maxPublicationContentLength) {
    return Result.Error<PublicationAPIResource, string>("The content must not have more than 777 characters.");
  }

  return ApiEndpoint.publications.POST(authorUsername, content);
}
