import useSWR from "swr";

export type ApiResponse<RESPONSE_TYPE> = {
  data: RESPONSE_TYPE | undefined;
  isLoading: boolean;
  error: unknown;
}

/*
This function is a isolation layer between the application's code and the outside world.
It allows us to chose whatever fetching library we want, be it useSWR, Tanstack Query, vanilla fetch calls, etc...
Whatever fetching approach is used, it will not impact the application's architecture, because it is safely contained here
*/
export function useApiEndpoint<RESPONSE_TYPE>(url: string): ApiResponse<RESPONSE_TYPE> {
  const { data, error, isLoading } = useSWR<RESPONSE_TYPE>(url, async (url: string) => {
    const response = await fetch(url);
    return await response.json();
  });

  return { data, error, isLoading };
}
