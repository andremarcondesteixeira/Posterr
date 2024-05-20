import { Result } from "../Util/Result";

export async function makeRequest<RESPONSE_TYPE>(url: string): Promise<Result<RESPONSE_TYPE>> {
    try {
        const requestResponse = await fetch(url);
        const responseAsJson = await requestResponse.json();
        return Result.Ok(responseAsJson);
    } catch(error) {
        if (error instanceof Error) {
            return Result.Error<RESPONSE_TYPE>(error);
        }

        if (typeof error === "string") {
            return Result.Error<RESPONSE_TYPE>(new Error(error));
        }

        return Result.Error(new Error(`Error when making http request to url: ${url}`, { cause: error }))
    }
}
