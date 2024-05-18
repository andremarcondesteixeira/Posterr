import { Publication } from "@/core/domain/entities/Publication";
import { ApiEndpoint } from "@/core/services/apiEndpointsService";
import { useApiEndpoint } from "./useApiEndpoint";
import { useEffect, useState } from "react";

export type PostsPagination = {
  isLoading: boolean;
  posts: Publication[];
  error: string | null;
  nextPage: () => void;
}

export function usePostsPagination(): PostsPagination {
  const [isLoading, setIsLoading] = useState(true);
  const [posts, setPosts] = useState<Publication[]>([]);
  const [error, setError] = useState<string | null>(null);

  const response = useApiEndpoint(ApiEndpoint.posts);

  useEffect(() => {
    setIsLoading(response.isLoading);
    setPosts(response.data);
    setError(response.error);
  }, [response]);

  const nextPage = () => {
    setIsLoading(true);

  };

  return {
    isLoading,
    posts,
    error,
    nextPage
  }
}
