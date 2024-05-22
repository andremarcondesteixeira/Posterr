import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { makeRequest } from "../../Services/HttpRequestService";
import { Result } from "../../Util/Result";
import { Publication } from "../Entities/Publication";

export type GetPostsPageUseCaseResponse = Promise<Result<Publication[], Error>>;

export function PostsPageUrl(page: number) {
  const pageSize = page === 1 ? 15 : 20;
  return `${ApiEndpoint.posts}?page=${page}&pageSize=${pageSize}`;
}

export function GetPostsPageUseCase(page: number): GetPostsPageUseCaseResponse {
  if (!Number.isInteger(page) || page < 1) {
    throw new Error(`Tried to use invalid page number ${page} when calling GetPostsPageUseCase`);
  }

  return makeRequest<Publication[]>(PostsPageUrl(page));
}
