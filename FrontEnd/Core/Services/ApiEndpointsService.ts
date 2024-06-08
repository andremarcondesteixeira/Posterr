import { Author, Publication } from "../Domain/Entities/types";
import { makeRequest } from "./HttpRequestService";

const baseUrl = `${process.env["NEXT_PUBLIC_API_SERVER_URL"]}/api`;

export const ApiEndpoint = Object.freeze({
  publications: Object.freeze({
    url: `${baseUrl}/Publications`,
    GET: (pageNumber: number) => {
      return makeRequest<{
        count: number;
      } & APIResource<{
        publications: PublicationAPIResource[];
      }, {
        next?: {
          href: string;
        };
      }>>(`${ApiEndpoint.publications.url}?pageNumber=${pageNumber}`);
    },
    POST: ({ authorUsername, content }: { authorUsername: string; content: string; }) => {
      return makeRequest<PublicationAPIResource>(ApiEndpoint.publications.url, {
        method: "POST",
        body: JSON.stringify({ authorUsername, content }),
        headers: {
          "accept": "*/*",
          "Content-Type": "application/json"
        }
      });
    }
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
