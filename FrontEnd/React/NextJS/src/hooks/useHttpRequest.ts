import { Result } from "@Core/Util/Result";
import useSWR from "swr";

/*
This function is an isolation layer between the application's code and the outside world.
It allows us to chose whatever fetching library we want, be it useSWR, Tanstack Query, vanilla fetch calls, etc...
Whatever fetching approach is used, it will not impact the application's architecture, because it is safely contained here
*/
export function useHttpRequest<RESPONSE_TYPE>(requestId: string, fetcher: () => Promise<Result<RESPONSE_TYPE, Error>>) {
  const { data, error, isLoading } = useSWR<RESPONSE_TYPE, Error, string>(requestId, async () => {
    const response = await fetcher();
    return response.okOrThrow();
  });

  return { data, error, isLoading };
}
