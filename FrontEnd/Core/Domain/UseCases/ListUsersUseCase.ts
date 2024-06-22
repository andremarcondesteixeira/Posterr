import { ApiEndpoint } from "../../Services/ApiEndpointsService";

export function ListUsersUseCase() {
  return ApiEndpoint.users.GET();
}
