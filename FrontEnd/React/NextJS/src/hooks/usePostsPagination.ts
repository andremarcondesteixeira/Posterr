import { Publication } from "@Core/Domain/Entities/Publication";
import { useHttpRequest } from "./useHttpRequest";
import { PostsPageUrl, GetPostsPageUseCase } from "@Core/Domain/UseCases/GetPostsPageUseCase";

export function usePostsPagination(pageNumber: number) {
  if (!Number.isInteger(pageNumber) || pageNumber <= 0) {
    throw new Error("Page number must be a positive integer");
  }

  const { data, isLoading, error } = useHttpRequest<Publication[]>(
    PostsPageUrl(pageNumber),
    () => GetPostsPageUseCase(pageNumber)
  );

  return {
    posts: data,
    isLoadingPosts: isLoading,
    errorLoadingPosts: error
  };
}
