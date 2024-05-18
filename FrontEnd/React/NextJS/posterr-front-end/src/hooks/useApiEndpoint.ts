import useSWR from "swr";

export function useApiEndpoint(url: string) {
  const { data, error, isLoading } = useSWR(url, async (...args) => {
    const response = await fetch(...args);
    return await response.json();
  });

  return { data, error, isLoading };
}
