import config from "../config.json";
import { PublicationAPIResource, PublicationsListAPIResource, UsersListAPIResource } from "../types";
import { makeRequest } from "./HttpRequestService";

const baseUrl = config.apiBaseUrl;

export const ApiEndpoint = {
  publications: {
    url: `${baseUrl}/publications`,
    GET: publications_GET,
    POST: publications_POST,
    PATCH: publications_PATCH,
    search: searchPublications,
  },
  users: {
    url: `${baseUrl}/users`,
    GET: users_GET,
  }
};

function publications_GET(lastSeenPublicationId: number, isFirstPage: boolean, signal: AbortSignal) {
  const url = `${ApiEndpoint.publications.url}?lastSeenPublicationId=${lastSeenPublicationId}&isFirstPage=${isFirstPage}`;
  return makeRequest<PublicationsListAPIResource>(url, { signal });
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

function searchPublications(searchTerm: string, lastSeenPublicationId: number, isFirstPage: boolean, signal: AbortSignal) {
  return makeRequest<PublicationsListAPIResource>(`${ApiEndpoint.publications.url}/search?searchTerm=${encodeURI(searchTerm)}&lastSeenPublicationId=${lastSeenPublicationId}&isFirstPage=${isFirstPage}`, {
    signal
  });
}

function users_GET() {
  return makeRequest<UsersListAPIResource>(ApiEndpoint.users.url);
}
