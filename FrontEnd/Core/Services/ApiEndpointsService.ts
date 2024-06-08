import { Author, Publication } from "../Domain/Entities/types";
import { makeRequest } from "./HttpRequestService";

const baseUrl = `${process.env["NEXT_PUBLIC_API_SERVER_URL"]}/api`;

export const ApiEndpoint = Object.freeze({
  posts: Object.freeze({
    url: `${baseUrl}/Posts`,
    POST: ({ username, content }: { username: string; content: string }) =>
      makeRequest(ApiEndpoint.posts.url, {
        method: "POST",
        body: JSON.stringify({ username, content }),
        headers: {
          "accept": "*/*",
          "Content-Type": "application/json"
        }
      }),
  }),
  publications: Object.freeze({
    url: `${baseUrl}/Publications`,
    GET: (pageNumber: number) => makeRequest<{
      count: number;
    } & APIResource<{
      publications: PublicationAPIResource[];
    }, {
      next?: {
        href: string;
      };
    }>>(`${ApiEndpoint.publications.url}?pageNumber=${pageNumber}`),
  }),
});

export type PublicationAPIResource = Publication & APIResource<AuthorAPIResource>;

export type AuthorAPIResource = Author & APIResource;

export type APIResource<EMBEDDED = undefined, LINKS = undefined> = {
  _links: {
    self: {
      href: string;
    }
  } & LINKS;
  _embedded: EMBEDDED;
}
