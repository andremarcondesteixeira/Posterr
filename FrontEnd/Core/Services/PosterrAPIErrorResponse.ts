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
