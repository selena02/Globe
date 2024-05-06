export interface AuthResponse {
  token: string;
  id: string;
  roles: string[] | null;
  pictureUrl: string | null;
}
