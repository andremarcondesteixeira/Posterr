import { Publication } from "../Domain/Entities/Publication";
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
  reposts: Object.freeze({
    url: `${baseUrl}/Reposts`,
  }),
  publications: Object.freeze({
    url: `${baseUrl}/Publications`,
    GET: (pageNumber: number) => makeRequest<{
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
        }
      }
    }>(`${ApiEndpoint.publications.url}?pageNumber=${pageNumber}`),
  }),
});
