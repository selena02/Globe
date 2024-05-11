import { ApiError } from "../models/ApiError";

interface FetchAPIOptions extends RequestInit {
  body?: any;
}

const API_URL = "https://localhost:7063/api";

async function fetchAPI<T>(
  endpoint: string,
  options: FetchAPIOptions = {}
): Promise<T> {
  const token = localStorage.getItem("token");
  const headers: any = {
    "Content-Type": "application/json",
    ...options.headers,
  };

  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(`${API_URL}/${endpoint}`, {
    ...options,
    headers,
    body: options.body ? JSON.stringify(options.body) : null,
  });

  const data = await response.json();
  if (!response.ok) {
    throw new ApiError(
      data.message || "Something went wrong",
      response.status,
      data.errors
    );
  }

  return data as T;
}

export default fetchAPI;
