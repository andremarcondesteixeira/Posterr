import { Publication } from "@Core/Domain/Entities/Publication";
import { ApiEndpoint } from "@Core/Services/ApiEndpointsService";
import { useApiEndpoint } from "./useApiEndpoint";

export type PostRequestResponse = {
  posts: Publication[],
  isLoading: boolean;
  error: unknown;
}

export function usePostsRequest(page: number): PostRequestResponse {
  const { isLoading, data, error} = useApiEndpoint<Publication[]>(`${ApiEndpoint.posts}?page=${page}`);
  return {
    isLoading,
    posts: data ?? [],
    error
  }
}
