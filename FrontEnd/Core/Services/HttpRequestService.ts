import { Result } from "../Util/Result";

export async function makeRequest<RESPONSE_TYPE>(url: string, init?: RequestInit): Promise<Result<RESPONSE_TYPE, Error>> {
    try {
        const requestResponse = await fetch(url, init);
        const responseAsJson = await requestResponse.json();
        return Result.Ok<RESPONSE_TYPE>(responseAsJson);
    } catch(error) {
        if (error instanceof Error) {
            return Result.Error<RESPONSE_TYPE>(error);
        }

        if (typeof error === "string") {
            return Result.Error<RESPONSE_TYPE>(new Error(error));
        }

        return Result.Error<RESPONSE_TYPE>(new Error(`Error when making http request to url: ${url}`, { cause: error }))
    }
}