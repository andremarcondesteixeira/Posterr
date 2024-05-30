import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { makeRequest } from "../../Services/HttpRequestService";
import { Result } from "../../Util/Result";
import { Publication } from "../Entities/Publication";

export type ListPublicationsWithPaginationUseCaseResponse = {
  count: number;
  _embedded: {
    publications: Publication[];
  };
  _links: {
    self: {
      href: string;
    };
    next?: {
      href: string;
    };
  };
};

export function PostsPageUrl(page: number) {
  return `${ApiEndpoint.publications}?pageNumber=${page}`;
}

export function ListPublicationsWithPaginationUseCase(page: number): Promise<Result<ListPublicationsWithPaginationUseCaseResponse, Error>> {
  if (!Number.isInteger(page) || page < 1) {
    throw new Error(`Tried to use invalid page number ${page} when calling GetPostsPageUseCase`);
  }

  return makeRequest<ListPublicationsWithPaginationUseCaseResponse>(PostsPageUrl(page));
}
