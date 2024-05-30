import { Publication } from "../Domain/Entities/Publication";
import { makeRequest } from "./HttpRequestService";

const baseUrl = '/api';

export const ApiEndpoint = Object.freeze({
  posts: Object.freeze({
    url: `${baseUrl}/posts`,
  }),
  reposts: Object.freeze({
    url: `${baseUrl}/reposts`,
  }),
  publications: Object.freeze({
    url: `${baseUrl}/publications`,
    GET: () => makeRequest<{
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
    }>(ApiEndpoint.publications.url),
  }),
});
