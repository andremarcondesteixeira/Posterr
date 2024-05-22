import { Result } from "../Util/Result";

export async function makeRequest<RESPONSE>(url: string, init?: RequestInit): Promise<Result<RESPONSE, Error>> {
    try {
        const requestResponse = await fetch(url, init);

        if (!requestResponse.ok) {
          throw new Error(`Response HTTP status code was ${requestResponse.status} when fetching ${url}`, {
            cause: requestResponse
          });
        }

        const responseAsJson = await requestResponse.json();
        return Result.Ok<RESPONSE>(responseAsJson);
    } catch(error: unknown) {
        // Fetch API only throws error objects. This is safe.
        return Result.Error(error as Error);
    }
}
