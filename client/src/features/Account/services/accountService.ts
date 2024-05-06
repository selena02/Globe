import fetchAPI from "../../../shared/utils/fetchAPI";
import { LoginDto } from "../models/LoginDto";
import { AuthResponse } from "../models/AuthResponse";
import { ApiError } from "../../../shared/models/ApiError";

export async function loginUser(
  credentials: LoginDto
): Promise<AuthResponse | ApiError> {
  try {
    const data = await fetchAPI<AuthResponse>("login", {
      method: "POST",
      body: JSON.stringify(credentials),
    });
    return data;
  } catch (error) {
    throw error;
  }
}
