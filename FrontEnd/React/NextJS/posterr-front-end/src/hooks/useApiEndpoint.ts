import useSWR from "swr";

export type ApiResponse<RESPONSE_TYPE> = {
  data: RESPONSE_TYPE | undefined;
  isLoading: boolean;
  error: unknown;
}

export function useApiEndpoint<RESPONSE_TYPE>(url: string): ApiResponse<RESPONSE_TYPE> {
  const { data, error, isLoading } = useSWR<RESPONSE_TYPE>(url, async (url: string) => {
    const response = await fetch(url);
    return await response.json();
  });

  return { data, error, isLoading };
}
