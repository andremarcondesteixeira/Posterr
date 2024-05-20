import { Publication } from "@/core/domain/entities/Publication";
import { ApiEndpoint } from "@/core/services/apiEndpointsService";
import { useApiEndpoint } from "./useApiEndpoint";

export function usePostsRequest(page: number) {
  return useApiEndpoint<Publication>(`${ApiEndpoint.posts}?page=${page}`);
}
