import { Publication } from "@Core/Domain/Entities/Publication";
import { ApiEndpoint } from "@Core/Services/ApiEndpointsService";
import { makeRequest } from "@Core/Services/HttpRequestService";
import { Result } from "@Core/Util/Result";

export type GetPostsPageUseCaseResponse = Promise<Result<Publication[], Error>>;

export async function getPostsPage(page: number): GetPostsPageUseCaseResponse {
  const pageSize = page === 1 ? 15 : 20;
  return makeRequest<Publication[]>(`${ApiEndpoint.posts}?page=${page}&pageSize=${pageSize}`);
}
