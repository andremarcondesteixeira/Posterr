import { Publication } from "@Core/Domain/Entities/Publication";
import { ApiEndpoint } from "@Core/Services/ApiEndpointsService";
import { makeRequest } from "@Core/Services/HttpRequestService";
import { Result } from "@Core/Util/Result";

export type GetPostsPageUseCaseResponse = Promise<Result<Publication[], Error>>;

export function PostsPageUrl(page: number) {
  const pageSize = page === 1 ? 15 : 20;
  return `${ApiEndpoint.posts}?page=${page}&pageSize=${pageSize}`;
}

export async function GetPostsPageUseCase(page: number): GetPostsPageUseCaseResponse {
  return makeRequest<Publication[]>(PostsPageUrl(page));
}
