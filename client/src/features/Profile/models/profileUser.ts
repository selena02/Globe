export interface ProfileUser {
  id: string;
  userName: string;
  fullName: string;
  email: string;
  profilePictureUrl: string | null;
  location: string | null;
  bio: string | null;
  followersCount: number;
  followingCount: number;
}
