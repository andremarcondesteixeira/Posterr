import useSWR from "swr";

export function useApiEndpoint<RESPONSE_TYPE>(url: string) {
  const { data, error, isLoading } = useSWR<RESPONSE_TYPE>(url, async (url: string) => {
    const response = await fetch(url);
    return await response.json();
  });

  return { data, error, isLoading };
}
