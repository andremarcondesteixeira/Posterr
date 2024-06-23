import { Author, Publication } from "./Domain/Entities/types";

export type APIResource<EMBED = undefined, LINKS = undefined> = { _links: { self: { href: string } } & LINKS; _embedded: EMBED; }
export type AuthorAPIResource = Author & APIResource;
export type NextPageLink = { next?: { href: string } };
export type PublicationAPIResource = Publication & APIResource<AuthorAPIResource>;
export type PublicationsListAPIResource = { count: number } & APIResource<{ publications: PublicationAPIResource[] }, NextPageLink>;
export type UsersListAPIResource = { count: number } & APIResource<{ users: AuthorAPIResource[] }>;

export type PosterrAPIErrorCause =  {
  detail: string;
  instance: string;
  status?: number;
  title: string;
  traceId?: string;
  type: string;
};

export class PosterrAPIErrorResponse {
  #errorMessage: string;
  #cause: PosterrAPIErrorCause;

  constructor({ message, cause }: { message: string, cause: PosterrAPIErrorCause }) {
    this.#errorMessage = message;
    this.#cause = Object.freeze(cause);
  }

  get message() {
    return this.#errorMessage;
  }

  get cause() {
    return this.#cause;
  }
};
