import { Publication } from "@Core/Domain/Entities/Publication";
import { useHttpRequest } from "./useHttpRequest";
import { PostsPageUrl, GetPostsPageUseCase } from "@Core/Domain/UseCases/GetPostsPageUseCase";

export function usePostsPage(page: number) {
  const { data, isLoading, error } = useHttpRequest<Publication[]>(PostsPageUrl(page), () => GetPostsPageUseCase(page));

  return {
    posts: data,
    isLoadingPosts: isLoading,
    errorLoadingPosts: error
  };
}
