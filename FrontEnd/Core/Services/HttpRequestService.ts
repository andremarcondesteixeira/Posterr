import { Result } from "../Util/Result";

export type PosterrAPIErrorResponse = Error & {
  cause: {
    detail: string;
    instance: string;
    status: number;
    title: string;
    traceId: string;
    type: string;
  };
};

export const REQUEST_ABORTED = "aborted";

export async function makeRequest<RESPONSE>(url: string, init?: RequestInit): Promise<Result<RESPONSE, PosterrAPIErrorResponse>> {
    try {
        const requestResponse = await fetch(url, init);

        if (!requestResponse.ok) {
          throw new Error(`Response HTTP status code was ${requestResponse.status} when fetching ${url}`, {
            cause: await requestResponse.json(),
          });
        }

        const responseAsJson = await requestResponse.json();
        return Result.Ok<RESPONSE>(responseAsJson);
    } catch(error: unknown) {
        if (error === REQUEST_ABORTED) {
          return Result.Error({
            cause: {
              detail: REQUEST_ABORTED,
              instance: url,
              status: undefined,
              title: REQUEST_ABORTED,
              traceId: '',
              type: REQUEST_ABORTED,
            },
            message: REQUEST_ABORTED
          } as PosterrAPIErrorResponse);
        }

        // Fetch API only throws error objects, and the API should always return an ErrorDetails object when there is an error.
        // This should be safe.
        return Result.Error(error as PosterrAPIErrorResponse);
    }
}
