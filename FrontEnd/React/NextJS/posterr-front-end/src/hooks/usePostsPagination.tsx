import { Publication } from "@/core/domain/entities/Publication";

export type PostsPagination = {
  isLoading: boolean;
  posts: Publication[];
  error: string | null;
  nextPage: () => void;
}

export function usePostsPagination(): PostsPagination {
  const nextPage = () => {}

  return {
    isLoading: true,
    posts: [
      { postId: 1, content: "post 1", authorUsername: "user 1", publicationDate: new Date() },
      { postId: 2, content: "post 1", authorUsername: "user 1", publicationDate: new Date() },
      { postId: 3, content: "post 1", authorUsername: "user 1", publicationDate: new Date() }
    ],
    error: null,
    nextPage
  }
}
