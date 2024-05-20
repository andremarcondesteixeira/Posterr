import { Result } from "@Core/Util/Result";
import useSWR from "swr";

// I decided to use a Result wrapper here only to remove the undefineds from the type definition
export type ApiResponse<RESPONSE_TYPE> = Result<{
  data: RESPONSE_TYPE;
  isLoading: boolean;
}, Error>;

/*
This function is an isolation layer between the application's code and the outside world.
It allows us to chose whatever fetching library we want, be it useSWR, Tanstack Query, vanilla fetch calls, etc...
Whatever fetching approach is used, it will not impact the application's architecture, because it is safely contained here
*/
export function useHttpRequest<RESPONSE_TYPE>(requestId: string, fetcher: () => Promise<Result<RESPONSE_TYPE, Error>>): ApiResponse<RESPONSE_TYPE> {
  const { data, error, isLoading } = useSWR<RESPONSE_TYPE, Error, string>(requestId, async () => {
    const response = await fetcher();
    return response.okOrThrow();
  });

  if (error) {
    return Result.Error(error);
  }

  return Result.Ok({ data, isLoading }) as ApiResponse<RESPONSE_TYPE>;
}
