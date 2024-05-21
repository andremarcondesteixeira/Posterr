import { Result } from "../Util/Result";

export async function makeRequest<RESPONSE_TYPE>(url: string, init?: RequestInit): Promise<Result<RESPONSE_TYPE, Error>> {
    try {
        const requestResponse = await fetch(url, init);

        if (!requestResponse.ok) {
          throw new Error(`Response HTTP status code was ${requestResponse.status} when fetching ${url}`, {
            cause: requestResponse
          });
        }

        const responseAsJson = await requestResponse.json();
        return Result.Ok<RESPONSE_TYPE>(responseAsJson);
    } catch(error: unknown) {
        // Fetch API only throws error objects. This is safe.
        return Result.Error<RESPONSE_TYPE>(error as Error);
    }
}
