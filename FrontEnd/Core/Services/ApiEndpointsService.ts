import { Author, PublicationEntity } from "../Domain/Entities/types";
import { makeRequest } from "./HttpRequestService";

const baseUrl = `${process.env["NEXT_PUBLIC_API_SERVER_URL"]}/api`;

export const ApiEndpoint = {
  publications: {
    url: `${baseUrl}/publications`,
    GET: publications_GET,
    POST: publications_POST,
    PATCH: publications_PATCH,
  },
  users: {
    url: `${baseUrl}/users`,
    GET: users_GET,
  }
};

function publications_GET(lastSeenPublicationId: number, isFirstPage: boolean, signal: AbortSignal) {
  const url = `${ApiEndpoint.publications.url}?lastSeenPublicationId=${lastSeenPublicationId}&isFirstPage=${isFirstPage}`;
  return makeRequest<Publications_GET_Response>(url, { signal });
}

function publications_POST(authorUsername: string, content: string) {
  return makeRequest<PublicationAPIResource>(ApiEndpoint.publications.url, {
    method: "POST",
    body: JSON.stringify({ authorUsername, content }),
    headers: {
      "accept": "*/*",
      "Content-Type": "application/json"
    },
  });
}

function publications_PATCH(authorUsername: string, content: string, originalPostId: number) {
  return makeRequest<PublicationAPIResource>(`${ApiEndpoint.publications.url}/${originalPostId}`, {
    method: "PATCH",
    body: JSON.stringify({ authorUsername, content }),
    headers: {
      "accept": "*/*",
      "Content-Type": "application/json"
    },
  });
}

function users_GET() {
  return makeRequest<Users_GET_Response>(ApiEndpoint.users.url);
}

export type APIResource<EMBEDDED = undefined, LINKS = undefined> = {
  _links: { self: { href: string } } & LINKS;
  _embedded: EMBEDDED;
}
export type AuthorAPIResource = Author & APIResource;
export type NextPageLink = { next?: { href: string } };
export type PublicationAPIResource = PublicationEntity & APIResource<AuthorAPIResource>;
export type Publications_GET_Response = { count: number } & APIResource<{ publications: PublicationAPIResource[] }, NextPageLink>;
export type Users_GET_Response = { count: number } & APIResource<{ users: AuthorAPIResource[] }>;
