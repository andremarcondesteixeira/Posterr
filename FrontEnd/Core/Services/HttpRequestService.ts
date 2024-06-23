import { Result } from "../Util/Result";
import { PosterrAPIErrorResponse } from "../types";
import { RequestAbortedError } from "./RequestAbortedError";

export async function makeRequest<RESPONSE>(url: string, init?: RequestInit): Promise<Result<RESPONSE, PosterrAPIErrorResponse | RequestAbortedError>> {
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
        if (init?.signal?.aborted) {
          return Result.Error(new RequestAbortedError());
        }

        // Fetch API only throws error objects, and the API should always return an ErrorDetails object when there is an error.
        // This should be safe.
        return Result.Error(new PosterrAPIErrorResponse({
          message: (error as any).message,
          cause: { ...(error as any).cause }
        }));
    }
}
