import {
  ListPublicationsWithPaginationUseCase,
  PostsPageUrl,
  type ListPublicationsWithPaginationUseCaseResponse
} from "@Core/Domain/UseCases/ListPublicationsWithPaginationUseCase";
import { useHttpRequest } from "./useHttpRequest";

export function usePostsPagination(pageNumber: number) {
  const { data, isLoading, error } = useHttpRequest<ListPublicationsWithPaginationUseCaseResponse>(
    PostsPageUrl(pageNumber),
    () => ListPublicationsWithPaginationUseCase(pageNumber)
  );

  return {
    posts: data,
    isLoadingPosts: isLoading,
    errorLoadingPosts: error
  };
}
