import { ApiEndpoint } from "../../Services/ApiEndpointsService";
import { makeRequest } from "../../Services/HttpRequestService";
import { Result } from "../../Util/Result";
import { PublicationEntity } from "../Entities/types";

export type ListPublicationsUseCaseResponse = {
  count: number;
  _embedded: {
    publications: PublicationEntity[];
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

export function ListPublicationsUseCase(page: number): Promise<Result<ListPublicationsUseCaseResponse, Error>> {
  if (!Number.isInteger(page) || page < 1) {
    throw new Error(`Tried to use invalid page number ${page} when calling GetPostsPageUseCase`);
  }

  return makeRequest<ListPublicationsUseCaseResponse>(PostsPageUrl(page));
}
